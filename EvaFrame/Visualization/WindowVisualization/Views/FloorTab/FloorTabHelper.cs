using EvaFrame.Models.Building;
using Avalonia;
using Avalonia.Media;
using Avalonia.Skia;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using SkiaSharp;

namespace EvaFrame.Visualization.WindowVisualization
{
    class FloorTabHelper
    {
        private SKPaint indicatorPaint;
        private SKPaint stairPaint;
        private SKPaint exitPaint;
        private SKPaint corridorPaint;

        public FloorTabHelper()
        {
            SetPaintColor(ref indicatorPaint, 0, 0, 255);
            SetPaintColor(ref stairPaint, 255, 255, 0);
            SetPaintColor(ref exitPaint, 0, 255, 0);
            SetPaintColor(ref corridorPaint, 0, 255, 255);
        }

        public Bitmap GetBackgroundBitmap(Floor target, int width, int height)
        {
            WriteableBitmap bitmap = new WriteableBitmap(
                new PixelSize(width, height),
                new Vector(96, 96),
                PixelFormat.Rgba8888
            );

            using (var lockedBitmap = bitmap.Lock())
            {
                SKImageInfo info = new SKImageInfo(
                    lockedBitmap.Size.Width,
                    lockedBitmap.Size.Height,
                    lockedBitmap.Format.ToSkColorType()
                );

                SKSurface surface = SKSurface.Create(info, lockedBitmap.Address, lockedBitmap.RowBytes);

                foreach (Indicator ind in target.Indicators)
                    foreach (Corridor cor in ind.Neighbors)
                        if (!cor.IsStairway)
                        {
                            SKPoint p1 = new SKPoint(cor.From.X, cor.From.Y);
                            SKPoint p2 = new SKPoint(cor.To.X, cor.To.Y);
                            surface.Canvas.DrawLine(p1, p2, corridorPaint);
                        }

                foreach (Indicator ind in target.Indicators)
                {
                    if (ind.IsExitNode)
                        surface.Canvas.DrawCircle(ind.X, ind.Y, 5, exitPaint);
                    else if (ind.IsStairNode)
                        surface.Canvas.DrawCircle(ind.X, ind.Y, 5, stairPaint);
                    else 
                        surface.Canvas.DrawCircle(ind.X, ind.Y, 5, indicatorPaint);
                }
            }
            return bitmap;
        }

        public Geometry GetInhabitantIcon(Point position)
        {
            Size iconSize = new Size(10, 10);
            Point center = new Point(
                position.X - iconSize.Width / 2, 
                position.Y - iconSize.Height / 2
            );
            return new EllipseGeometry(new Rect(center, iconSize));
        }

        private void SetPaintColor(ref SKPaint paint, byte r, byte g, byte b)
        {
            paint = new SKPaint();
            paint.Color = new SKColor(r, g, b, 255);
            paint.Shader = SKShader.CreateColor(paint.Color);
        }
    }
}