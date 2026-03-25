using Microsoft.Maui.Controls.Shapes;

namespace Trimex.Controls;

public sealed class SlideToEndView : ContentView
{
    public event EventHandler? SlideCompleted;

    private const double ThumbSize = 52;
    private const double TrackHeight = 62;
    private const double ThumbPadding = 5;
    private const double SlideThreshold = 0.75;

    private readonly View _thumb;
    private readonly Label _trackLabel;
    private double _thumbTranslation;
    private double _maxTranslation;
    private bool _completed;

    public SlideToEndView()
    {
        var trackBorder = new Border
        {
            BackgroundColor = Color.FromArgb("#202020"),
            StrokeThickness = 0,
            HeightRequest = TrackHeight,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            StrokeShape = new RoundRectangle { CornerRadius = TrackHeight / 2 }
        };

        _trackLabel = new Label
        {
            Text = "End Session  ›",
            TextColor = Color.FromArgb("#8A8A8A"),
            FontFamily = "OpenSansSemibold",
            FontSize = 15,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            InputTransparent = true
        };

        _thumb = new Border
        {
            WidthRequest = ThumbSize,
            HeightRequest = ThumbSize,
            BackgroundColor = Colors.White,
            StrokeThickness = 0,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(ThumbPadding, 0, 0, 0),
            StrokeShape = new RoundRectangle { CornerRadius = ThumbSize / 2 },
            InputTransparent = true,
            Content = new Label
            {
                Text = "›",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Colors.Black,
                InputTransparent = true
            }
        };

        // Pan gesture on the whole track so the user can drag from anywhere
        var pan = new PanGestureRecognizer();
        pan.PanUpdated += OnPanUpdated;

        var grid = new Grid { HeightRequest = TrackHeight };
        grid.GestureRecognizers.Add(pan);
        grid.Add(trackBorder);
        grid.Add(_trackLabel);
        grid.Add(_thumb);

        Content = grid;
        HeightRequest = TrackHeight;
        SizeChanged += (_, _) => _maxTranslation = Math.Max(0, Width - ThumbSize - ThumbPadding * 2);
    }

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (_completed)
        {
            return;
        }

        if (_maxTranslation <= 0)
        {
            _maxTranslation = Math.Max(0, Width - ThumbSize - ThumbPadding * 2);
        }

        switch (e.StatusType)
        {
            case GestureStatus.Running:
                _thumbTranslation = Math.Clamp(e.TotalX, 0, _maxTranslation);
                _thumb.TranslationX = _thumbTranslation;
                _trackLabel.Opacity = Math.Max(0, 1 - _thumbTranslation / _maxTranslation * 1.5);
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                // Use cached _thumbTranslation — TotalX may be 0 on some platforms at Completed
                if (_maxTranslation > 0 && _thumbTranslation >= _maxTranslation * SlideThreshold)
                {
                    _completed = true;
                    _thumb.TranslationX = _maxTranslation;
                    SlideCompleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    _thumbTranslation = 0;
                    _thumb.TranslationX = 0;
                    _trackLabel.Opacity = 1;
                }
                break;
        }
    }
}
