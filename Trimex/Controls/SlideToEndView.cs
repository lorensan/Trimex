using Microsoft.Maui.Controls.Shapes;

namespace Trimex.Controls;

public sealed class SlideToEndView : ContentView
{
    public static readonly BindableProperty TrackTextProperty = BindableProperty.Create(
        nameof(TrackText),
        typeof(string),
        typeof(SlideToEndView),
        "End Session  ›",
        propertyChanged: OnTrackTextChanged);

    public event EventHandler? SlideCompleted;

    private const double ThumbSize = 52;
    private const double TrackHeight = 62;
    private const double ThumbPadding = 5;
    private const double SlideThreshold = 0.75;

    private readonly Border _thumbBorder;
    private readonly Label _thumbGlyph;
    private readonly Color _thumbIdleColor;
    private readonly Color _thumbSlidingColor;

    private readonly View _thumb;
    private readonly Label _trackLabel;
    private double _thumbTranslation;
    private double _maxTranslation;
    private bool _completed;

    public SlideToEndView()
    {
        _thumbIdleColor = GetColor("AccentNeonYellow", Colors.Yellow);
        _thumbSlidingColor = GetColor("AccentRed", Colors.Red);

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
            Text = TrackText,
            TextColor = Color.FromArgb("#8A8A8A"),
            FontFamily = "OpenSansSemibold",
            FontSize = 15,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            InputTransparent = true
        };

        _thumbGlyph = new Label
        {
            Text = "›",
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            TextColor = Color.FromArgb("#0D0D0D"),
            InputTransparent = true
        };

        _thumbBorder = new Border
        {
            WidthRequest = ThumbSize,
            HeightRequest = ThumbSize,
            BackgroundColor = _thumbIdleColor,
            StrokeThickness = 0,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(ThumbPadding, 0, 0, 0),
            StrokeShape = new RoundRectangle { CornerRadius = ThumbSize / 2 },
            InputTransparent = true,
            Content = _thumbGlyph
        };

        _thumb = _thumbBorder;

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

    public string TrackText
    {
        get => (string)GetValue(TrackTextProperty);
        set => SetValue(TrackTextProperty, value);
    }

    public void Reset()
    {
        _completed = false;
        _thumbTranslation = 0;
        _thumb.TranslationX = 0;
        _trackLabel.Opacity = 1;
        _thumbBorder.BackgroundColor = _thumbIdleColor;
    }

    private static void OnTrackTextChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is SlideToEndView view)
        {
            view._trackLabel.Text = (string?)newValue ?? string.Empty;
        }
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
                _thumbBorder.BackgroundColor = _thumbSlidingColor;
                _thumbTranslation = Math.Clamp(e.TotalX, 0, _maxTranslation);
                _thumb.TranslationX = _thumbTranslation;
                _trackLabel.Opacity = _maxTranslation <= 0
                    ? 1
                    : Math.Max(0, 1 - _thumbTranslation / _maxTranslation * 1.5);
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                if (_maxTranslation > 0 && _thumbTranslation >= _maxTranslation * SlideThreshold)
                {
                    _completed = true;
                    _thumb.TranslationX = _maxTranslation;
                    SlideCompleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Reset();
                }
                break;
        }
    }

    private static Color GetColor(string key, Color fallback)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) is true && value is Color color)
        {
            return color;
        }

        return fallback;
    }
}
