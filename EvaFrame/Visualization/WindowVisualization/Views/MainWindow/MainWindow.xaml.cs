using System;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using Avalonia.Controls;
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
        private FloorTab[] floorTabs;

        public MainWindow()
        {
            this.Title = "EvaFrame";
            this.Width = 1280;
            this.Height = 720;

            this.tabControl = new DynamicTabControl();
            this.generalTab = new GeneralTab();
            this.tabControl.AddTab(generalTab, "General");
            this.Content = this.tabControl;
        }

        public void Initialize(Building target)
        {
            floorTabs = new FloorTab[target.Floors.Count];
            for (int i = 0; i < floorTabs.Length; i ++)
            {
                floorTabs[i] = new FloorTab(target.Floors[i]);
                tabControl.AddTab(floorTabs[i], String.Format("Floor {0}", i + 1));
            }
        }

        public void UpdateContent(double timeElapsed, int remainingCount, Person displayedPerson)
        {
            generalTab.UpdateContent(timeElapsed, remainingCount, displayedPerson);
            foreach (FloorTab tab in floorTabs)
                tab.ClearInhabitantIcon();
        }
    }
}