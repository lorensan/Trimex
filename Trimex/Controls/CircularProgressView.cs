namespace Trimex.Controls;

public sealed class CircularProgressView : GraphicsView, IDrawable
{
    public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
        nameof(Progress),
        typeof(double),
        typeof(CircularProgressView),
        0d,
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(
        nameof(AccentColor),
        typeof(Color),
        typeof(CircularProgressView),
        Color.FromArgb("#423BFF"),
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty TrackColorProperty = BindableProperty.Create(
        nameof(TrackColor),
        typeof(Color),
        typeof(CircularProgressView),
        Color.FromArgb("#2E2E2E"),
        propertyChanged: OnVisualPropertyChanged);

    public CircularProgressView()
    {
        Drawable = this;
        HeightRequest = 300;
        WidthRequest = 300;
    }

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, Math.Clamp(value, 0d, 1d));
    }

    public Color AccentColor
    {
        get => (Color)GetValue(AccentColorProperty);
        set => SetValue(AccentColorProperty, value);
    }

    public Color TrackColor
    {
        get => (Color)GetValue(TrackColorProperty);
        set => SetValue(TrackColorProperty, value);
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var strokeSize = 16f;
        var inset = strokeSize;
        var size = Math.Min(dirtyRect.Width, dirtyRect.Height) - (inset * 2);
        var x = (dirtyRect.Width - size) / 2;
        var y = (dirtyRect.Height - size) / 2;

        canvas.Antialias = true;
        canvas.StrokeLineCap = LineCap.Round;

        canvas.StrokeColor = TrackColor;
        canvas.StrokeSize = strokeSize;
        canvas.DrawArc(x, y, size, size, -90, 360, false, false);

        var progressAngle = (float)(360 * Progress);

        if (progressAngle <= 0)
        {
            return;
        }

        canvas.StrokeColor = AccentColor.WithAlpha(0.25f);
        canvas.StrokeSize = strokeSize + 6;
        canvas.DrawArc(x, y, size, size, -90, progressAngle, false, false);

        canvas.StrokeColor = AccentColor;
        canvas.StrokeSize = strokeSize;
        canvas.DrawArc(x, y, size, size, -90, progressAngle, false, false);
    }

    private static void OnVisualPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        ((CircularProgressView)bindable).Invalidate();
    }
}
