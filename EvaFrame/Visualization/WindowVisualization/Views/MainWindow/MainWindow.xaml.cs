using System;
using EvaFrame.Models;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace EvaFrame.Visualization.WindowVisualization
{
    /// <summary>
    /// Cửa sổ chính trong mô phỏng đồ họa của chương trình. 
    /// Tại đây, các thông tin cơ bản như số lượng người còn lại trong tòa nhà, hoặc thông tin về người 
    /// đang ở gần lối thoát nhất sẽ được hiển thị.
    /// </summary>
    public class MainWindow : Window
    {
        private DynamicTabControl tabControl;
        private GeneralTab generalTab;

        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            this.tabControl = new DynamicTabControl();
            this.generalTab = new GeneralTab();
            this.tabControl.AddTab(generalTab, "General");
            this.Content = this.tabControl;
        }

        public void UpdateContent(double timeElapsed, int remainingCount, Person displayedPerson)
        {
            generalTab.UpdateContent(timeElapsed, remainingCount, displayedPerson);
        }
    }
}