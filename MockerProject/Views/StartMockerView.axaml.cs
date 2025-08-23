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
    private void SearchBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is MainWindowViewModel vm && vm.onSearchProject.CanExecute(null))
            {
                vm.onSearchProject.Execute(null);
            }
        }
    }

    private async void SearchBar_Pressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm && vm.onSearchProject.CanExecute(null))
        {
            await ((AsyncRelayCommand)vm.onSearchProject).ExecuteAsync(null);
        }
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