using Avalonia.Controls;
using Avalonia.Input;
using MockerProject.ViewModels;

namespace MockerProject.Views;

public partial class MenuBoxView : UserControl
{
    public MenuBoxView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.S && e.KeyModifiers == KeyModifiers.Control)
        {
            // Execute the Save command
            var vm = this.DataContext as MainWindowViewModel;
            vm?.onSaveProject?.Execute(null);

            e.Handled = true;
        }
    }
}