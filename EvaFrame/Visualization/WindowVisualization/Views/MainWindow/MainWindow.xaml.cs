using System.Threading;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace EvaFrame.Visualization.WindowVisualization
{
    public class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            this.viewModel = new MainWindowViewModel();
            this.DataContext = this.viewModel;
            AvaloniaXamlLoader.Load(this);

            Thread drawThread = new Thread(() =>
            {
                while (true)
                    OnUpdate();
            });
            drawThread.IsBackground = true;
            drawThread.Start();
        }

        private void OnUpdate()
        {

        }
    }

    public class MainWindowViewModel : ReactiveObject
    {
        private string content = "Hello My Friends!";

        public string Content
        {
            get => content;
            set
            {
                this.RaisePropertyChanging();
                content = value;
                this.RaisePropertyChanged();
            }
        }
    }
}