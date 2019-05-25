using System.Collections.Generic;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Skia;
using SkiaSharp;

namespace EvaFrame.Visualization.WindowVisualization
{
    class FloorTab : UserControl
    {
        private SKPaint indicatorPaint;
        private SKPaint stairPaint;
        private SKPaint exitPaint;
        private SKPaint corridorPaint;
        private SKPaint densityPaint;

        private Floor target;
        private List<SKPoint> inhabitantLocations;
        private List<SKPaint> inhabitantPaints;

        public FloorTab(Floor target)
        {
            this.Width = 1280;
            this.Height = 720;
            this.IsHitTestVisible = false;

            SetPaintColor(ref indicatorPaint, 3, 146, 207);
            SetPaintColor(ref stairPaint, 253, 244, 152);
            SetPaintColor(ref exitPaint, 123, 192, 67);
            SetPaintColor(ref densityPaint, 51, 51, 51);
            corridorPaint = new SKPaint();

            this.target = target;
            this.inhabitantLocations = new List<SKPoint>();
            this.inhabitantPaints = new List<SKPaint>();
        }

        public void ClearInhabitantIcons()
        {
            inhabitantLocations.Clear();
            inhabitantPaints.Clear();
        }

        public void AddInhabitantIcon(Person person, SKPaint paint)
        {
            inhabitantPaints.Add(paint);
            if (person.Location == null)
            {
                inhabitantLocations.Add(new SKPoint(person.Following.X, person.Following.Y));
                return;
            }
            Indicator from = person.Following;
            Indicator to = person.Location.To(from);
            double iconX = from.X + (to.X - from.X) * person.CompletedPercentage;
            double iconY = from.Y + (to.Y - from.Y) * person.CompletedPercentage;
            inhabitantLocations.Add(new SKPoint((float)iconX, (float)iconY));
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            Bitmap bitmap = GetSituationBitmap(target, (int)Width, (int)Height);
            context.DrawImage(
                bitmap, 1,
                new Rect(0, 0, bitmap.PixelSize.Width, bitmap.PixelSize.Height),
                new Rect(0, 0, Width, Height)
            );
        }

        private void SetPaintColor(ref SKPaint paint, byte r, byte g, byte b)
        {
            paint = new SKPaint();
            paint.Color = new SKColor(r, g, b, 255);
            paint.Shader = SKShader.CreateColor(paint.Color);
        }

        private Bitmap GetSituationBitmap(Floor target, int width, int height)
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


                foreach (Corridor cor in target.Corridors)
                    if (!cor.IsStairway)
                    {
                        SKPoint p1 = new SKPoint(cor.I1.X, cor.I1.Y);
                        SKPoint p2 = new SKPoint(cor.I2.X, cor.I2.Y);
                        byte red = (byte)(255 * (1.0 - cor.Trustiness));
                        byte green = (byte)(255 * cor.Trustiness);
                        corridorPaint.Color = new SKColor(red, green, 0, 255);
                        corridorPaint.Shader = SKShader.CreateColor(corridorPaint.Color);
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

                for (int i = 0; i < inhabitantLocations.Count; i++)
                    surface.Canvas.DrawCircle(inhabitantLocations[i], 5, inhabitantPaints[i]);

                foreach (Corridor cor in target.Corridors)
                    if (!cor.IsStairway)
                    {
                        SKPoint p = new SKPoint((cor.I1.X + cor.I2.X) / 2, (cor.I1.Y + cor.I2.Y) / 2);
                        string text = cor.Density.ToString();
                        surface.Canvas.DrawText(text, p, densityPaint);
                    }
            }
            return bitmap;
        }
    }
}