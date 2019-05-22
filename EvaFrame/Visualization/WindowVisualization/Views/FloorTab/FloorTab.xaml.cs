using System;
using System.Collections.Generic;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Markup.Xaml;
using Avalonia.Skia;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using Avalonia.Rendering;
using SkiaSharp;

namespace EvaFrame.Visualization.WindowVisualization
{
    public class FloorTab : UserControl
    {
        private Pen inhabitantPen = new Pen(Brushes.Red);
        private Brush inhabitantBrush = new SolidColorBrush(Colors.Red);

        private FloorTabHelper helper;
        private Bitmap backgroundBitmap;
        private List<Point> inhabitantLocations;
        private System.Threading.Mutex mutex;

        public FloorTab(Floor target)
        {
            this.Width = 1280;
            this.Height = 720;
            this.IsHitTestVisible = false;

            this.helper = new FloorTabHelper();
            this.backgroundBitmap = this.helper.GetBackgroundBitmap(target, (int) Width, (int) Height);
            this.inhabitantLocations = new List<Point>();
            this.mutex = new System.Threading.Mutex();
        }

        public void ClearInhabitantIcons() {inhabitantLocations.Clear();}

        public void AddInhabitantIcon(Person person)
        {
            if (person.Location == null)
            {
                inhabitantLocations.Add(new Point(person.Following.X, person.Following.Y));
                return;
            }
            Indicator from = person.Location.From;
            Indicator to = person.Location.To;
            double iconX = from.X + (to.X - from.X) * person.CompletedPercentage;
            double iconY = from.Y + (to.Y - from.Y) * person.CompletedPercentage;
            inhabitantLocations.Add(new Point(iconX, iconY));
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            context.DrawImage(
                backgroundBitmap, 1, 
                new Rect(0, 0, backgroundBitmap.PixelSize.Width, backgroundBitmap.PixelSize.Height),
                new Rect(0, 0, Width, Height)
            );

            foreach (Point icon in inhabitantLocations)
                context.DrawGeometry(inhabitantBrush, inhabitantPen, helper.GetInhabitantIcon(icon));
        }
    }
}