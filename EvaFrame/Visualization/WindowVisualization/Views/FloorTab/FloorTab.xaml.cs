using System;
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
        private FloorTabHelper helper;
        private Bitmap background;

        public FloorTab(Floor target)
        {
            this.Width = 1280;
            this.Height = 720;
            this.helper = new FloorTabHelper();
            this.background = this.helper.GetBackgroundBitmap(target, (int) Width, (int) Height);
            this.background.Save("img.png");
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            context.DrawImage(
                background, 1, 
                new Rect(0, 0, background.PixelSize.Width, background.PixelSize.Height),
                new Rect(0, 0, Width, Height)
            );
        }
    }
}