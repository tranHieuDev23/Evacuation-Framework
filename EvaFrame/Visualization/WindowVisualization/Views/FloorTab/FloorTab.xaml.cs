using System;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EvaFrame.Visualization.WindowVisualization
{
    public class FloorTab : UserControl
    {
        
        public FloorTab(Floor target)
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void ClearInhabitantIcon()
        {

        }

        public void DrawInhabitantIcon(int x, int y)
        {

        }
    }
}