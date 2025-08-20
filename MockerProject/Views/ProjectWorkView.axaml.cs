using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Interactivity;
using Avalonia;
using Avalonia.VisualTree;
using Avalonia.Threading;
using Avalonia.Media;
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

            // Get the actual visual bounds of the current view to capture everything
            var visualBounds = this.Bounds;
            var actualWidth = (int)visualBounds.Width;
            var actualHeight = (int)visualBounds.Height;
            
            // Use the larger of the actual bounds or the platform dimensions
            var captureWidth = Math.Max(actualWidth, viewModel.PF_W + 200);  // Add generous padding
            var captureHeight = Math.Max(actualHeight, viewModel.PF_H + 200); // Add generous padding
            
            System.Diagnostics.Debug.WriteLine($"Visual bounds: {actualWidth}x{actualHeight}");
            System.Diagnostics.Debug.WriteLine($"Platform dimensions: {viewModel.PF_W}x{viewModel.PF_H}");
            System.Diagnostics.Debug.WriteLine($"Final capture dimensions: {captureWidth}x{captureHeight}");

            // Create render target bitmap with the larger dimensions
            var renderTargetBitmap = new RenderTargetBitmap(new PixelSize(captureWidth, captureHeight));
            
            // Simple approach: Capture the entire ScreenView directly
            var screenContent = this.FindControl<ContentControl>("Screen");
            if (screenContent?.Content is Visual visualContent)
            {
                // Render the entire ScreenView content (this should include the complete mobile phone)
                renderTargetBitmap.Render(visualContent);
                System.Diagnostics.Debug.WriteLine($"Successfully rendered ScreenView: {visualContent.GetType().Name}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No ScreenView content found");
                return;
            }

            // Debug: Log the dimensions being captured
            System.Diagnostics.Debug.WriteLine($"Capturing screenshot with actual visual bounds: {actualWidth}x{actualHeight}");
            System.Diagnostics.Debug.WriteLine($"Capture dimensions with padding: {captureWidth}x{captureHeight}");
            System.Diagnostics.Debug.WriteLine($"Page dimensions: {viewModel.PG_W}x{viewModel.PG_H}");
            System.Diagnostics.Debug.WriteLine($"Platform dimensions: {viewModel.PF_W}x{viewModel.PF_H}");
            System.Diagnostics.Debug.WriteLine($"RenderTargetBitmap size: {renderTargetBitmap.PixelSize.Width}x{renderTargetBitmap.PixelSize.Height}");

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