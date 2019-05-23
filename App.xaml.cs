using Avalonia;
using Avalonia.Markup.Xaml;

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
