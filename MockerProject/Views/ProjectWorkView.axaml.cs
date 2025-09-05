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
using Avalonia.Input;
using Avalonia.Controls.Shapes;
using Avalonia;
using IOPath = System.IO.Path;
using SysFileStream = System.IO.FileStream;
using SysFileMode = System.IO.FileMode;

namespace MockerProject.Views;

public partial class ProjectWorkView : UserControl
{
	private const double RulerThickness = 24.0;
	private const double TimelineHeight = 48.0;
	private bool _guidesActive = false;
	
	public ProjectWorkView()
	{
		InitializeComponent();
		this.AttachedToVisualTree += OnAttachedToVisualTree;
		this.SizeChanged += (_, __) => RenderRulers();
		this.AddHandler(PointerMovedEvent, OnPointerMoved, RoutingStrategies.Tunnel);
		this.AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
		this.AddHandler(PointerReleasedEvent, OnPointerReleased, RoutingStrategies.Tunnel);
		this.AddHandler(KeyUpEvent, OnKeyDown, RoutingStrategies.Tunnel);
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

	private void OnKeyDown(object? sender, KeyEventArgs e)
	{
		// Handle Ctrl+R shortcut for taking screenshot
		if (e.Key == Key.R && e.KeyModifiers == KeyModifiers.Control)
		{
			// Trigger the same functionality as the camera button click
			OnCameraButtonClick(this, new RoutedEventArgs());
			e.Handled = true;
		}
		// Handle Ctrl+L shortcut for toggling ruler visibility
		else if (e.Key == Key.L && e.KeyModifiers == KeyModifiers.Control)
		{
			// Get the main window to access the ViewModel
			var mainWindow = this.FindAncestorOfType<Window>();
			if (mainWindow?.DataContext is ViewModels.MainWindowViewModel viewModel)
			{
				// Toggle the ruler visibility
				viewModel.IsRulerVisible = !viewModel.IsRulerVisible;
			}
			e.Handled = true;
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
			
			// Create fixed filename (will overwrite existing file)
			var fileName = $"{projectName}_Screenshot.png";
			var filePath = IOPath.Combine(desktopPath, fileName);

			// Save the screenshot
			using (var stream = new SysFileStream(filePath, SysFileMode.Create))
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

	private void OnCenterScreenClick(object sender, RoutedEventArgs e)
	{
		try
		{
			// Get the main window to access the ViewModel
			var mainWindow = this.FindAncestorOfType<Window>();
			if (mainWindow?.DataContext is ViewModels.MainWindowViewModel viewModel)
			{
				// Call the centering method in the ViewModel
				viewModel.CenterScreen();
				
				// Also center the scroll viewer to show the centered content
				var scrollViewer = this.FindControl<ScrollViewer>("WorkScroll");
				if (scrollViewer != null)
				{
					// Use Dispatcher with a small delay to ensure UI is updated before scrolling
					Dispatcher.UIThread.InvokeAsync(async () =>
					{
						// Small delay to ensure UI updates are complete
						await Task.Delay(50);
						{
							// Get the screen content to calculate proper centering
							var screenContent = this.FindControl<ContentControl>("Screen");
							if (screenContent?.Content is Visual visualContent)
							{
								// Calculate the center position for the mobile device frame
								var contentWidth = scrollViewer.Extent.Width;
								var contentHeight = scrollViewer.Extent.Height;
								var viewportWidth = scrollViewer.Viewport.Width;
								var viewportHeight = scrollViewer.Viewport.Height;

								// Calculate the position of the mobile device frame within the content
								var deviceFrameX = viewModel.PF_X;
								var deviceFrameY = viewModel.PF_Y;
								var deviceFrameWidth = viewModel.PF_W;
								var deviceFrameHeight = viewModel.PF_H;

								// Calculate center offsets to show the mobile device frame in the center
								var centerX = Math.Max(0, deviceFrameX - (viewportWidth - deviceFrameWidth) / 2);
								var centerY = Math.Max(0, deviceFrameY - (viewportHeight - deviceFrameHeight) / 2);

								// Scroll to center the mobile device frame
								scrollViewer.Offset = new Vector(centerX, centerY);
							}
							else
							{
								// Fallback to simple centering if screen content is not available
								var contentWidth = scrollViewer.Extent.Width;
								var contentHeight = scrollViewer.Extent.Height;
								var viewportWidth = scrollViewer.Viewport.Width;
								var viewportHeight = scrollViewer.Viewport.Height;

								// Calculate center offsets
								var centerX = Math.Max(0, (contentWidth - viewportWidth) / 2);
								var centerY = Math.Max(0, (contentHeight - viewportHeight) / 2);

								// Scroll to center
								scrollViewer.Offset = new Vector(centerX, centerY);
							}
						}
					});
				}
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to center screen: {ex.Message}");
		}
	}

	private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
	{
		// Defer to next layout pass to ensure we have valid sizes
		Dispatcher.UIThread.Post(RenderRulers, DispatcherPriority.Background);
	}

	private void RenderRulers()
	{
		var overlay = this.FindControl<Canvas>("RulerOverlay");
		var top = this.FindControl<Canvas>("TopRuler");
		var left = this.FindControl<Canvas>("LeftRuler");
		if (overlay == null || top == null || left == null) return;

		var width = Bounds.Width;
		var height = Math.Max(0, Bounds.Height - TimelineHeight); // keep clear of the timeline row
		overlay.Width = width;
		overlay.Height = height;

		// position bars
		Canvas.SetLeft(top, RulerThickness);
		Canvas.SetTop(top, 0);
		top.Width = Math.Max(0, width - RulerThickness);
		top.Height = RulerThickness;

		Canvas.SetLeft(left, 0);
		Canvas.SetTop(left, RulerThickness);
		left.Width = RulerThickness;
		left.Height = Math.Max(0, height - RulerThickness);

		// clear existing
		top.Children.Clear();
		left.Children.Clear();

		DrawRuler(top, true);
		DrawRuler(left, false);
	}

	private void DrawRuler(Canvas canvas, bool isHorizontal)
	{
		// Use the width/height we just set to guarantee a length
		double length = isHorizontal ? canvas.Width : canvas.Height;
		if (double.IsNaN(length) || length <= 0) length = isHorizontal ? canvas.Bounds.Width : canvas.Bounds.Height;
		double step = 5;
		double medium = 25;
		double major = 50;

		for (double p = 0; p < length; p += step)
		{
			double tick = 4;
			if (Math.Abs(p % major) < 0.001) tick = 12;
			else if (Math.Abs(p % medium) < 0.001) tick = 8;

			if (isHorizontal)
			{
				var line = new Line
				{
					StartPoint = new Point(p, canvas.Height),
					EndPoint = new Point(p, canvas.Height - tick),
					StrokeThickness = 1,
					Stroke = Brushes.Gray
				};
				canvas.Children.Add(line);
				if (Math.Abs(p % major) < 0.001)
				{
					var tb = new TextBlock
					{
						Text = ((int)p).ToString(),
						Foreground = Brushes.Gray,
						FontSize = 10
					};
					Canvas.SetLeft(tb, p + 2);
					Canvas.SetTop(tb, canvas.Height - 18);
					canvas.Children.Add(tb);
				}
			}
			else
			{
				var line = new Line
				{
					StartPoint = new Point(canvas.Width, p),
					EndPoint = new Point(canvas.Width - tick, p),
					StrokeThickness = 1,
					Stroke = Brushes.Gray
				};
				canvas.Children.Add(line);
				if (Math.Abs(p % major) < 0.001)
				{
					var tb = new TextBlock
					{
						Text = ((int)p).ToString(),
						Foreground = Brushes.Gray,
						FontSize = 10
					};
					Canvas.SetLeft(tb, 2);
					Canvas.SetTop(tb, p - 8);
					canvas.Children.Add(tb);
				}
			}
		}
	}

	private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		var overlay = this.FindControl<Canvas>("RulerOverlay");
		var top = this.FindControl<Canvas>("TopRuler");
		var left = this.FindControl<Canvas>("LeftRuler");
		
		if (overlay == null || top == null || left == null) return;

		var pt = e.GetPosition(overlay);
		
		// Check if click is on top ruler
		var topBounds = new Rect(Canvas.GetLeft(top), Canvas.GetTop(top), top.Width, top.Height);
		var leftBounds = new Rect(Canvas.GetLeft(left), Canvas.GetTop(left), left.Width, left.Height);
		
		if (topBounds.Contains(pt) || leftBounds.Contains(pt))
		{
			_guidesActive = true;
			ShowGuides(pt);
		}
	}

	private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		_guidesActive = false;
		HideGuides();
	}

	private void OnPointerMoved(object? sender, PointerEventArgs e)
	{
		if (!_guidesActive) return;
		
		var overlay = this.FindControl<Canvas>("RulerOverlay");
		if (overlay == null) return;

		var pt = e.GetPosition(overlay);
		ShowGuides(pt);
	}

	private void ShowGuides(Point pt)
	{
		var overlay = this.FindControl<Canvas>("RulerOverlay");
		var guideV = this.FindControl<Line>("GuideV");
		var guideH = this.FindControl<Line>("GuideH");
		var measurementDisplay = this.FindControl<Border>("MeasurementDisplay");
		var measurementText = this.FindControl<TextBlock>("MeasurementText");
		
		if (overlay == null || guideV == null || guideH == null || measurementDisplay == null || measurementText == null) return;

		// Clamp point to overlay bounds
		pt = new Point(Math.Clamp(pt.X, 0, overlay.Bounds.Width), Math.Clamp(pt.Y, 0, overlay.Bounds.Height));
		
		// Show guides
		guideV.IsVisible = true;
		guideH.IsVisible = true;
		
		// Vertical guide
		guideV.StartPoint = new Point(pt.X, RulerThickness);
		guideV.EndPoint = new Point(pt.X, overlay.Bounds.Height);
		
		// Horizontal guide
		guideH.StartPoint = new Point(RulerThickness, pt.Y);
		guideH.EndPoint = new Point(overlay.Bounds.Width, pt.Y);
		
		// Show measurement at intersection
		measurementDisplay.IsVisible = true;
		measurementText.Text = $"{(int)pt.X}, {(int)pt.Y}";
		
		// Position measurement display near the intersection
		var displayX = Math.Min(pt.X + 10, overlay.Bounds.Width - 50);
		var displayY = Math.Min(pt.Y + 10, overlay.Bounds.Height - 25);
		Canvas.SetLeft(measurementDisplay, displayX);
		Canvas.SetTop(measurementDisplay, displayY);
	}

	private void HideGuides()
	{
		var guideV = this.FindControl<Line>("GuideV");
		var guideH = this.FindControl<Line>("GuideH");
		var measurementDisplay = this.FindControl<Border>("MeasurementDisplay");
		
		if (guideV != null) guideV.IsVisible = false;
		if (guideH != null) guideH.IsVisible = false;
		if (measurementDisplay != null) measurementDisplay.IsVisible = false;
	}
}