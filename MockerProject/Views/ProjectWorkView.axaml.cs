using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Interactivity;
using Avalonia;
using Avalonia.VisualTree;
using Avalonia.Threading;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MockerProject.Views;

public partial class ProjectWorkView : UserControl
{
    public ProjectWorkView()
    {
        InitializeComponent();
    }

    private async void OnCameraButtonClick(object sender, RoutedEventArgs e)
    {
        try
        {
            // Get the main window to access the ViewModel
            var mainWindow = this.FindAncestorOfType<Window>();
            if (mainWindow?.DataContext is ViewModels.MainWindowViewModel viewModel)
            {
                // Take screenshot of the current work screen
                await TakeScreenshotAsync(viewModel);
            }
        }
        catch (Exception ex)
        {
            // For now, just silently handle errors
            // In a production app, you might want to log this or show a notification
            System.Diagnostics.Debug.WriteLine($"Screenshot error: {ex.Message}");
        }
    }

    private async Task TakeScreenshotAsync(ViewModels.MainWindowViewModel viewModel)
    {
        try
        {
            // Check if we have a work screen
            if (viewModel.WorkScreen == null)
            {
                System.Diagnostics.Debug.WriteLine("No work screen available");
                return;
            }

            // Get the screen dimensions from the view model
            var screenWidth = (int)viewModel.WorkScreen.m_Size.Width;
            var screenHeight = (int)viewModel.WorkScreen.m_Size.Height;

            // Create render target bitmap
            var renderTargetBitmap = new RenderTargetBitmap(new PixelSize(screenWidth, screenHeight));
            
            // Try to render the current view content
            // Since we're in ProjectWorkView, we'll try to render the Screen content control
            var screenContent = this.FindControl<ContentControl>("Screen");
            if (screenContent?.Content is Visual visualContent)
            {
                renderTargetBitmap.Render(visualContent);
            }
            else
            {
                // Fallback: try to render the entire view
                renderTargetBitmap.Render(this);
            }

            // Get desktop path
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var projectName = viewModel.strProjectTitle ?? "MockerProject";
            
            // Create filename with timestamp
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
            var fileName = $"{projectName}_Screenshot_{timestamp}.png";
            var filePath = Path.Combine(desktopPath, fileName);

            // Save the screenshot
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                renderTargetBitmap.Save(stream);
            }

            // Store the file path for the "Open folder" button
            _lastScreenshotPath = filePath;

            // Show flash effect
            await ShowFlashEffectAsync();
            
            // Show notification popup
            ShowNotificationPopup();

            // Log success
            System.Diagnostics.Debug.WriteLine($"Screenshot saved successfully to: {filePath}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to save screenshot: {ex.Message}");
        }
    }

    private string _lastScreenshotPath = string.Empty;

    private async Task ShowFlashEffectAsync()
    {
        var flashOverlay = this.FindControl<Border>("FlashOverlay");
        if (flashOverlay != null)
        {
            // Show flash overlay
            flashOverlay.IsVisible = true;
            flashOverlay.Opacity = 0.3;

            // Animate flash effect
            var animation = new Avalonia.Animation.Animation
            {
                Duration = TimeSpan.FromMilliseconds(200)
            };

            // Fade out the flash
            await Task.Delay(100);
            flashOverlay.Opacity = 0;
            await Task.Delay(100);
            
            // Hide the overlay
            flashOverlay.IsVisible = false;
        }
    }

    private void ShowNotificationPopup()
    {
        var notificationPopup = this.FindControl<Border>("NotificationPopup");
        if (notificationPopup != null)
        {
            notificationPopup.IsVisible = true;
            
            // Auto-hide after 5 seconds
            _ = Task.Delay(5000).ContinueWith(_ =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    notificationPopup.IsVisible = false;
                });
            });
        }
    }

    private void OnCloseNotificationClick(object sender, RoutedEventArgs e)
    {
        var notificationPopup = this.FindControl<Border>("NotificationPopup");
        if (notificationPopup != null)
        {
            notificationPopup.IsVisible = false;
        }
    }

    private void OnOpenFolderClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(_lastScreenshotPath) && File.Exists(_lastScreenshotPath))
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{_lastScreenshotPath}\"");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to open folder: {ex.Message}");
        }
    }
}