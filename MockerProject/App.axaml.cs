using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using MockerProject.ViewModels;
using MockerProject.Views;
using System;

namespace MockerProject
{
    public partial class App : Application
    {
        private ResourceInclude? _currentTheme;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var viewModel = new MainWindowViewModel();
                var window = new MainWindow
                {
                    DataContext = viewModel
                };

                viewModel.setMainWindow(window);
                viewModel.ThemeChanged += OnThemeChanged;
                LoadTheme(viewModel.IsDarkMode);
                desktop.MainWindow = window;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void LoadTheme(bool isDark)
        {
            var themePath = isDark
                ? "avares://MockerProject/Styles/Colors.Dark.axaml"
                : "avares://MockerProject/Styles/Colors.Light.axaml";

            var newTheme = new ResourceInclude(new Uri("avares://MockerProject/"))
            {
                Source = new Uri(themePath, UriKind.Absolute)
            };

            if (_currentTheme != null)
                Resources.MergedDictionaries.Remove(_currentTheme);

            _currentTheme = newTheme;
            Resources.MergedDictionaries.Add(_currentTheme);
        }

        private void OnThemeChanged(object? sender, bool isDark)
        {
            RequestedThemeVariant = isDark ? ThemeVariant.Dark : ThemeVariant.Light;
            LoadTheme(isDark);
        }
    }
}