using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace MAUIGameLoop;

public partial class MainPage : ContentPage
{
    private readonly Stopwatch _stopWatch = new Stopwatch();
    private SKColor _fillColor = new SKColor(20, 20, 40);
    private double x;
    private double y;
    private double vx;
    private double vy;
    private int r = 50;
    public MainPage()
    {
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Init();
    }
    private void Init()
    {
        var ms = 10;
        var ts = TimeSpan.FromMilliseconds(ms);

        // Create a timer that triggers roughly every 100 milliseconds
        Device.StartTimer(ts, TimerLoop);

        // Initialize position
        x = 300.0;
        y = 200.0;

        // Initialize velocity
        vx = 100.0;
        vy = 200.0;
    }

    private bool TimerLoop()
    {
        // Get the elapsed time from the stopwatch;
        var dt = _stopWatch.Elapsed.TotalSeconds;

        // Restart the time measurement for the next time this method is called
        _stopWatch.Restart();

        var width = canvasView.CanvasSize.Width - r;
        var height = canvasView.CanvasSize.Height - r;

        // Update position based on velocity and the delta time
        x += dt * vx;
        y += dt * vy;

        // Check collision with side of the screen and reverse velocity
        // We also use the velocity component to see if the circle is moving in the direction of the boundary
        // Otherwise it could try to reverse the direction again while the circle is moving away from the wall.
        if ((x < r && vx < 0) || (x > width && vx > 0))
        {
            vx = -vx;
        }
        if ((y < r && vy < 0) || (y > height && vy > 0))
        {
            vy = -vy;
        }

        // Trigger the redrawing of the view
        canvasView.InvalidateSurface();

        return true;
    }

    private async void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var surface = e.Surface;
        var canvas = surface.Canvas;

        canvas.Clear(_fillColor);

        // Draw a circle
        using (SKPaint skPaint = new SKPaint())
        {
            skPaint.Style = SKPaintStyle.Fill;
            skPaint.IsAntialias = true;
            skPaint.Color = SKColors.Blue;
            skPaint.StrokeWidth = 10;

            canvas.DrawCircle((float)x, (float)y, r, skPaint);
        }
    }
}

