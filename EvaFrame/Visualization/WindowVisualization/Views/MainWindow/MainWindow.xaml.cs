using System;
using System.Collections.Generic;
using System.Threading;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using Avalonia.Controls;
using ReactiveUI;
using SkiaSharp;

namespace EvaFrame.Visualization.WindowVisualization
{
    class MainWindow : Window
    {
        private DynamicTabControl tabControl;
        private GeneralTab generalTab;
        private FloorTab[] floorTabs;
        private Building target;
        private Mutex mutex;

        private Dictionary<string, SKPaint> colors;
        private SKColor[] DEFAULT_COLORS = {
            new SKColor(199, 0, 0),
            new SKColor(243, 119, 54),
            new SKColor(46, 0, 62),
            new SKColor(0, 160, 176),
            new SKColor(7, 30, 34),
        };

        public MainWindow(double width, double height)
        {
            this.Title = "EvaFrame";
            this.Width = width;
            this.Height = height;

            this.tabControl = new DynamicTabControl();
            this.generalTab = new GeneralTab();
            this.tabControl.AddTab(generalTab, "General");
            this.Content = this.tabControl;

            this.mutex = new Mutex();
        }

        public void Initialize(Building target)
        {
            this.target = target;
            this.floorTabs = new FloorTab[target.Floors.Count];

            Random rnd = new Random(12345);
            List<SKPaint> floorColors = new List<SKPaint>();
            for (int i = 0; i < floorTabs.Length; i++)
            {
                floorTabs[i] = new FloorTab(target.Floors[i], this.Width, this.Height);
                tabControl.AddTab(floorTabs[i], String.Format("Floor {0}", i + 1));

                SKColor color;
                if (i < DEFAULT_COLORS.Length)
                    color = DEFAULT_COLORS[i];
                else
                {
                    byte red = (byte)rnd.Next(0, 256);
                    byte green = (byte)rnd.Next(0, 256);
                    byte blue = (byte)rnd.Next(0, 256);
                    color = new SKColor(red, green, blue);
                }
                SKPaint paint = new SKPaint();
                paint.Color = color;
                paint.Shader = SKShader.CreateColor(color);
                floorColors.Add(paint);
            }

            this.colors = new Dictionary<string, SKPaint>();
            foreach (Person p in target.Inhabitants)
                colors[p.Id] = floorColors[p.Following.FloorId - 1];
        }

        public void UpdateContent(double timeElapsed, int remainingCount, Person displayedPerson)
        {
            foreach (FloorTab tab in floorTabs)
                tab.ClearInhabitantIcons();
            mutex.WaitOne();
            generalTab.UpdateContent(timeElapsed, remainingCount, displayedPerson);
            foreach (Person p in target.Inhabitants)
            {
                if (p.Location != null)
                    if (p.Location.IsStairway)
                        continue;
                floorTabs[p.Following.FloorId - 1].AddInhabitantIcon(p, colors[p.Id]);
            }
            mutex.ReleaseMutex();
            foreach (FloorTab tab in floorTabs)
                tab.InvalidateVisual();
        }
    }
}