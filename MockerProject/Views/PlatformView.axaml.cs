using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MockerProject.Models;
using MockerProject.ViewModels;
using static MockerProject.ViewModels.MainWindowViewModel;
using System.Linq;
using System;
using Avalonia.Input;

namespace MockerProject.Views
{
    public partial class PlatformView : UserControl
    {
        private MainWindowViewModel m_MainViewModel;
        public PlatformView()
        {
            InitializeComponent(); 

            // Initialize the TextBox values when the control loads
            this.Loaded += (s, e) =>
            {
                if (DataContext is MainWindowViewModel viewModel)
                {
                    m_MainViewModel = viewModel;
                    WidthTextBox.Text = viewModel.PG_RW.ToString();
                    HeightTextBox.Text = viewModel.PG_RH.ToString();
                    
                    // Subscribe to property changes to update TextBox values
                    viewModel.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == nameof(viewModel.PG_RW))
                        {
                            WidthTextBox.Text = viewModel.PG_RW.ToString();
                        }
                        else if (args.PropertyName == nameof(viewModel.PG_RH))
                        {
                            HeightTextBox.Text = viewModel.PG_RH.ToString();
                        }
                    };
                }
            };

            colorButton.PropertyChanged += (s, e) =>
            {
                if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty)
                {
                    m_MainViewModel = (MainWindowViewModel)this.DataContext;
                    m_MainViewModel.m_PlatformView = this;
                    
                    // Store the old background color for undo
                    var oldBackground = m_MainViewModel.PageBackground;
                    
                    //Do something
                    Color w_Color = colorButton.Color;
                    if(w_Color.A != 255)
                        m_MainViewModel.PG_OPT = (double)w_Color.A / 255.0;
                    //colorButton.Color = new Color(255, w_Color.R, w_Color.G, w_Color.B);
                    SolidColorBrush w_IBrush = new SolidColorBrush(new Color(255, w_Color.R, w_Color.G, w_Color.B));
                    m_MainViewModel.PageBackground = w_IBrush;
                    
                    // Add background color change to undo list
                    if (m_MainViewModel.WorkScreen != null)
                    {
                        stControlHistory w_ControlHistory = new stControlHistory();
                        w_ControlHistory.Index = -1; // Special index for background color changes
                        w_ControlHistory.Cmd = "BackgroundColor";
                        w_ControlHistory.id = CONTROL_TYPE.NONE;
                        w_ControlHistory.type = typeof(SolidColorBrush);
                        w_ControlHistory.oldInfo = null;
                        w_ControlHistory.curInfo = null;
                        
                        // Store the background color change data directly in the history entry
                        w_ControlHistory.CustomData = new BackgroundColorChange
                        {
                            OldColor = oldBackground,
                            NewColor = w_IBrush
                        };
                        
                        m_MainViewModel.WorkScreen.m_UndoList.Add(w_ControlHistory);
                    }
                }
            };
        }

        private void OnWidthTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Handle null or empty text
                string originalText = textBox.Text ?? "";
                
                // Only filter non-numeric characters, don't apply limits during typing
                string numericOnly = string.IsNullOrEmpty(originalText) ? "" : 
                    new string(originalText.Where(c => char.IsDigit(c)).ToArray());
                
                if (originalText != numericOnly)
                {
                    int cursorPosition = textBox.CaretIndex;
                    textBox.Text = numericOnly;
                    textBox.CaretIndex = Math.Min(cursorPosition, numericOnly.Length);
                }
            }
        }

        private void OnHeightTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Handle null or empty text
                string originalText = textBox.Text ?? "";
                
                // Only filter non-numeric characters, don't apply limits during typing
                string numericOnly = string.IsNullOrEmpty(originalText) ? "" : 
                    new string(originalText.Where(c => char.IsDigit(c)).ToArray());
                
                if (originalText != numericOnly)
                {
                    int cursorPosition = textBox.CaretIndex;
                    textBox.Text = numericOnly;
                    textBox.CaretIndex = Math.Min(cursorPosition, numericOnly.Length);
                }
            }
        }

        private void OnWidthLostFocus(object? sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && m_MainViewModel != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        // If empty, restore the current value
                        textBox.Text = m_MainViewModel.PG_RW.ToString();
                    }
                    else if (int.TryParse(textBox.Text, out int width))
                    {
                        // Validate and set the width - allow values from 50 to 3000
                        const int minWidth = 50;
                        const int maxWidth = 3000;
                        
                        if (width < minWidth)
                        {
                            width = minWidth;
                            textBox.Text = width.ToString();
                        }
                        else if (width > maxWidth)
                        {
                            width = maxWidth;
                            textBox.Text = width.ToString();
                        }
                        
                        // Update the ViewModel with the new width
                        if (m_MainViewModel.PG_RW != width)
                        {
                            m_MainViewModel.PG_RW = width;
                            System.Diagnostics.Debug.WriteLine($"Width updated to: {width}");
                        }
                    }
                    else
                    {
                        // Invalid input, restore the current value
                        textBox.Text = m_MainViewModel.PG_RW.ToString();
                    }
                }
                catch (Exception ex)
                {
                    // If any error occurs, restore the current value
                    textBox.Text = m_MainViewModel.PG_RW.ToString();
                    System.Diagnostics.Debug.WriteLine($"Error in OnWidthLostFocus: {ex.Message}");
                }
            }
        }

        private void OnHeightLostFocus(object? sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && m_MainViewModel != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        // If empty, restore the current value
                        textBox.Text = m_MainViewModel.PG_RH.ToString();
                    }
                    else if (int.TryParse(textBox.Text, out int height))
                    {
                        // Validate and set the height - allow values from 50 to 3000
                        const int minHeight = 50;
                        const int maxHeight = 3000;
                        
                        if (height < minHeight)
                        {
                            height = minHeight;
                            textBox.Text = height.ToString();
                        }
                        else if (height > maxHeight)
                        {
                            height = maxHeight;
                            textBox.Text = height.ToString();
                        }
                        
                        // Update the ViewModel with the new height
                        if (m_MainViewModel.PG_RH != height)
                        {
                            m_MainViewModel.PG_RH = height;
                            System.Diagnostics.Debug.WriteLine($"Height updated to: {height}");
                        }
                    }
                    else
                    {
                        // Invalid input, restore the current value
                        textBox.Text = m_MainViewModel.PG_RH.ToString();
                    }
                }
                catch (Exception ex)
                {
                    // If any error occurs, restore the current value
                    textBox.Text = m_MainViewModel.PG_RH.ToString();
                    System.Diagnostics.Debug.WriteLine($"Error in OnHeightLostFocus: {ex.Message}");
                }
            }
        }

        private void OnWidthKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Apply the same logic as LostFocus when Enter is pressed
                OnWidthLostFocus(sender, new RoutedEventArgs());
                
                // Remove focus from the TextBox
                if (sender is TextBox textBox)
                {
                    textBox.IsEnabled = false;
                    textBox.IsEnabled = true;
                }
            }
        }

        private void OnHeightKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Apply the same logic as LostFocus when Enter is pressed
                OnHeightLostFocus(sender, new RoutedEventArgs());
                
                // Remove focus from the TextBox
                if (sender is TextBox textBox)
                {
                    textBox.IsEnabled = false;
                    textBox.IsEnabled = true;
                }
            }
        }
    }
}
