using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EvaFrame.Visualization.WindowVisualization
{
    public class DynamicTabControl : UserControl
    {
        public ObservableCollection<TabItem> Items { get; }

        public DynamicTabControl()
        {
            this.Items = new ObservableCollection<TabItem>();
            this.DataContext = this.Items;
            AvaloniaXamlLoader.Load(this);
        }

        public void AddTab(UserControl content, string tabHeader)
        {
            TabItem item = new TabItem();
            item.Header = tabHeader;
            item.Content = content;
            Items.Add(item);
        }
    }
}