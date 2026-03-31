using Trimex.Models;

namespace Trimex.Controls;

public sealed class HistoryChartView : GraphicsView, IDrawable
{
    private const float Padding = 40f;
    private const float RightPadding = 16f;
    private const float BottomPadding = 30f;
    private const float PointRadius = 8f;
    private const float MaxMinutes = 120f;

    private IReadOnlyList<HeroWodHistory> _dataPoints = [];
    private readonly List<PointF> _renderedPoints = [];
    private int _selectedIndex = -1;

    public event EventHandler<int>? PointSelected;

    public HistoryChartView()
    {
        Drawable = this;
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
        BackgroundColor = Colors.Transparent;

        StartInteraction += OnStartInteraction;
    }

    public IReadOnlyList<HeroWodHistory> DataPoints
    {
        get => _dataPoints;
        set
        {
            _dataPoints = value;
            _selectedIndex = -1;
            _renderedPoints.Clear();
            Invalidate();
        }
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
            Invalidate();
        }
    }

    private void OnStartInteraction(object? sender, TouchEventArgs e)
    {
        if (_renderedPoints.Count == 0 || e.Touches.Length == 0)
            return;

        var touch = e.Touches[0];
        var closestIndex = -1;
        var closestDist = float.MaxValue;

        for (var i = 0; i < _renderedPoints.Count; i++)
        {
            var pt = _renderedPoints[i];
            var dist = MathF.Sqrt(MathF.Pow(pt.X - touch.X, 2) + MathF.Pow(pt.Y - touch.Y, 2));

            if (dist < closestDist && dist < 40f)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        if (closestIndex >= 0)
        {
            _selectedIndex = closestIndex;
            Invalidate();
            PointSelected?.Invoke(this, closestIndex);
        }
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        _renderedPoints.Clear();

        if (_dataPoints.Count == 0)
            return;

        canvas.Antialias = true;

        var chartLeft = Padding;
        var chartTop = 12f;
        var chartRight = dirtyRect.Width - RightPadding;
        var chartBottom = dirtyRect.Height - BottomPadding;
        var chartWidth = chartRight - chartLeft;
        var chartHeight = chartBottom - chartTop;

        if (chartWidth <= 0 || chartHeight <= 0)
            return;

        DrawGrid(canvas, chartLeft, chartTop, chartRight, chartBottom, chartWidth, chartHeight);
        DrawAxesLabels(canvas, chartLeft, chartTop, chartBottom, chartWidth);
        DrawDataLine(canvas, chartLeft, chartTop, chartWidth, chartHeight);
        DrawDataPoints(canvas);
    }

    private void DrawGrid(ICanvas canvas, float left, float top, float right, float bottom, float width, float height)
    {
        canvas.StrokeColor = Color.FromArgb("#1E1E1E");
        canvas.StrokeSize = 1f;

        int[] ySteps = [0, 30, 60, 90, 120];
        foreach (var step in ySteps)
        {
            var y = bottom - (step / MaxMinutes * height);
            canvas.DrawLine(left, y, right, y);
        }
    }

    private void DrawAxesLabels(ICanvas canvas, float left, float top, float bottom, float chartWidth)
    {
        var chartHeight = bottom - top;

        canvas.FontSize = 10f;
        canvas.FontColor = Color.FromArgb("#8A8A8A");

        int[] ySteps = [0, 30, 60, 90, 120];
        foreach (var step in ySteps)
        {
            var y = bottom - (step / MaxMinutes * chartHeight) - 6f;
            canvas.DrawString(step.ToString(), 0, y, left - 4f, 12f, HorizontalAlignment.Right, VerticalAlignment.Center);
        }

        if (_dataPoints.Count <= 1)
        {
            canvas.DrawString("1", left, bottom + 4f, 30f, 16f, HorizontalAlignment.Center, VerticalAlignment.Top);
            return;
        }

        var xStep = chartWidth / (_dataPoints.Count - 1);
        for (var i = 0; i < _dataPoints.Count; i++)
        {
            var x = left + (i * xStep);
            canvas.DrawString((i + 1).ToString(), x - 10f, bottom + 4f, 20f, 16f, HorizontalAlignment.Center, VerticalAlignment.Top);
        }
    }

    private void DrawDataLine(ICanvas canvas, float left, float top, float chartWidth, float chartHeight)
    {
        if (_dataPoints.Count == 0)
            return;

        var bottom = top + chartHeight;

        for (var i = 0; i < _dataPoints.Count; i++)
        {
            var x = _dataPoints.Count == 1
                ? left
                : left + (i * chartWidth / (_dataPoints.Count - 1));
            var minutes = _dataPoints[i].DurationSeconds / 60f;
            var y = bottom - (Math.Min(minutes, MaxMinutes) / MaxMinutes * chartHeight);
            _renderedPoints.Add(new PointF(x, y));
        }

        if (_renderedPoints.Count < 2)
            return;

        canvas.StrokeColor = Color.FromArgb("#CAFD00");
        canvas.StrokeSize = 2.5f;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.StrokeLineJoin = LineJoin.Round;

        for (var i = 0; i < _renderedPoints.Count - 1; i++)
        {
            canvas.DrawLine(_renderedPoints[i], _renderedPoints[i + 1]);
        }
    }

    private void DrawDataPoints(ICanvas canvas)
    {
        for (var i = 0; i < _renderedPoints.Count; i++)
        {
            var pt = _renderedPoints[i];
            var isSelected = i == _selectedIndex;

            if (isSelected)
            {
                canvas.FillColor = Color.FromArgb("#CAFD00");
                canvas.FillCircle(pt.X, pt.Y, PointRadius + 3f);
                canvas.FillColor = Color.FromArgb("#0D0D0D");
                canvas.FillCircle(pt.X, pt.Y, PointRadius);
                canvas.FillColor = Color.FromArgb("#CAFD00");
                canvas.FillCircle(pt.X, pt.Y, PointRadius - 3f);
            }
            else
            {
                canvas.FillColor = Color.FromArgb("#CAFD00");
                canvas.FillCircle(pt.X, pt.Y, PointRadius);
            }
        }
    }
}
