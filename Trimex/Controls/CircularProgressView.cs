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
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
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
        var glowStrokeSize = strokeSize + 6;
        var inset = glowStrokeSize;
        var size = Math.Min(dirtyRect.Width, dirtyRect.Height) - (inset * 2);

        if (size <= 0)
        {
            return;
        }

        var x = (dirtyRect.Width - size) / 2;
        var y = (dirtyRect.Height - size) / 2;
        const float startAngle = -90f;

        canvas.Antialias = true;
        canvas.StrokeLineCap = LineCap.Round;

        canvas.StrokeColor = TrackColor;
        canvas.StrokeSize = strokeSize;
        canvas.DrawEllipse(x, y, size, size);

        var progressAngle = (float)(360 * Progress);

        if (progressAngle <= 0)
        {
            return;
        }

        if (progressAngle >= 360f)
        {
            canvas.StrokeColor = AccentColor.WithAlpha(0.25f);
            canvas.StrokeSize = glowStrokeSize;
            canvas.DrawEllipse(x, y, size, size);

            canvas.StrokeColor = AccentColor;
            canvas.StrokeSize = strokeSize;
            canvas.DrawEllipse(x, y, size, size);
            return;
        }

        var endAngle = startAngle + progressAngle;

        canvas.StrokeColor = AccentColor.WithAlpha(0.25f);
        canvas.StrokeSize = glowStrokeSize;
        canvas.DrawArc(x, y, size, size, startAngle, endAngle, false, false);

        canvas.StrokeColor = AccentColor;
        canvas.StrokeSize = strokeSize;
        canvas.DrawArc(x, y, size, size, startAngle, endAngle, false, false);
    }

    private static void OnVisualPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        ((CircularProgressView)bindable).Invalidate();
    }
}
