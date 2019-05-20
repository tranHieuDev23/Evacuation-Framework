using Avalonia;
using Avalonia.Markup.Xaml;

namespace EvaFrame.Visualization.BasicGraphicalVisualization
{
    /// <summary>
    /// 
    /// </summary>
    class App : Application
    {
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}