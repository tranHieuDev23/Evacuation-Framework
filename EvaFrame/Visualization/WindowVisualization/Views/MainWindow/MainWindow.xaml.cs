using System;
using System.Threading;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using Avalonia.Controls;
using ReactiveUI;

namespace EvaFrame.Visualization.WindowVisualization
{
    class MainWindow : Window
    {
        private DynamicTabControl tabControl;
        private GeneralTab generalTab;
        private FloorTab[] floorTabs;
        private Building target;
        private Mutex mutex;

        public MainWindow()
        {
            this.Title = "EvaFrame";
            this.Width = 1280;
            this.Height = 720;

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
            for (int i = 0; i < floorTabs.Length; i++)
            {
                floorTabs[i] = new FloorTab(target.Floors[i]);
                tabControl.AddTab(floorTabs[i], String.Format("Floor {0}", i + 1));
            }
        }

        public void UpdateContent(double timeElapsed, int remainingCount, Person displayedPerson)
        {
            generalTab.UpdateContent(timeElapsed, remainingCount, displayedPerson);
            foreach (FloorTab tab in floorTabs)
                tab.ClearInhabitantIcons();
            mutex.WaitOne();
            foreach (Person p in target.Inhabitants)
            {
                if (p.Location != null)
                    if (p.Location.IsStairway)
                        continue;
                floorTabs[p.Following.FloorId - 1].AddInhabitantIcon(p);
            }
            mutex.ReleaseMutex();
            foreach (FloorTab tab in floorTabs)
                tab.InvalidateVisual();
        }
    }
}