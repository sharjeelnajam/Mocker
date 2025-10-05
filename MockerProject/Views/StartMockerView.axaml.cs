using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using MockerProject.ViewModels;
using Avalonia.Interactivity;

namespace MockerProject.Views;

public partial class StartMockerView : UserControl
{
    public StartMockerView()
    {
        InitializeComponent();
    }

    private async void RecentProject_Tapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm && sender is Border border && border.DataContext is ViewModels.RecentProject project)
        {
            if (vm.onOpenRecentProject.CanExecute(project))
            {
                await ((AsyncRelayCommand<ViewModels.RecentProject>)vm.onOpenRecentProject).ExecuteAsync(project);
            }
        }
    }

}