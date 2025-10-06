using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mime;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MockerProject.Models;
using MockerProject.ViewModels;
using MockerProject.ViewModels.UIViewModels;
using MockerProject.Views.UIControls;
using ReactiveUI;

namespace MockerProject.Views
{
    public partial class UIPropertyWindow : Window
    {
        [AllowNull] private MainWindowViewModel m_MainVM;
        //private UIControlViewModel m_UIControlVM;
        [AllowNull] private UIControl m_UIControl;
        public UIPropertyWindow()
        {
            InitializeComponent();
            BGButton.PropertyChanged += (s, e) =>
            {
                if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty)
                {
                    //Do something
                    Color w_Color = BGButton.Color;
                    //colorButton.Color = new Color(255, w_Color.R, w_Color.G, w_Color.B);
                    SolidColorBrush w_Brush = new SolidColorBrush(w_Color);
                    if (m_UIControl == null) return;
                    m_UIControl.setBackground(w_Brush);
                }
            };
            BCButton.PropertyChanged += (s, e) =>
            {
                if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty)
                {
                    //Do something
                    Color w_Color = BCButton.Color;
                    //colorButton.Color = new Color(255, w_Color.R, w_Color.G, w_Color.B);
                    SolidColorBrush w_Brush = new SolidColorBrush(w_Color);
                    if (m_UIControl == null) return;
                    m_UIControl.setBorderColor(w_Brush);
                }
            };
            FGButton.PropertyChanged += (s, e) =>
            {
                if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty)
                {
                    //Do something
                    Color w_Color = FGButton.Color;
                    //colorButton.Color = new Color(255, w_Color.R, w_Color.G, w_Color.B);
                    SolidColorBrush w_Brush = new SolidColorBrush(w_Color);
                    if (m_UIControl == null) return;
                    m_UIControl.setForeground(w_Brush);
                }
            };
            this.PointerPressed += (sender, e) =>
            {
                
            };
            AddItem.AddHandler(TextBlock.TappedEvent, (sender, e) =>
            {
                if (m_UIControl.GetType() == typeof(ListBoxControl))
                    ((ListBoxControl)m_UIControl).setAddItem();
            });
            
        }

        private void OnMainTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (m_UIControl is TabViewControl tabViewControl && sender is TextBox textBox)
            {
                var viewModel = (RepeaterControlViewModel)tabViewControl.DataContext;
                
                // Update tab text when the main text field changes
                if (textBox.Text != null && textBox.Text != viewModel.SelectedTabText)
                {
                    // If we have a selected tab, update it (allow empty text)
                    if (viewModel.SelectedTabIndex >= 0 && viewModel.SelectedTabIndex < viewModel.TabHeaders.Count)
                    {
                        viewModel.UpdateTabText(textBox.Text);
                        // Refresh the tab headers to update the UI
                        tabViewControl.RefreshTabHeaders();
                    }
                    else if (textBox.Text != null && !viewModel.TabHeaders.Contains(textBox.Text))
                    {
                        // If no tab is selected but we have text that doesn't exist, create a new tab
                        // Only create new tab if text is not empty
                        if (!string.IsNullOrEmpty(textBox.Text))
                        {
                            viewModel.AddNewTabWithText(textBox.Text);
                            // Refresh the tab headers to update the UI
                            tabViewControl.RefreshTabHeaders();
                        }
                    }
                    
                    // Update the base text property to keep it in sync
                    viewModel.text = textBox.Text;
                }
            }
        }

        private void OnMainTextLostFocus(object? sender, RoutedEventArgs e)
        {
            if (m_UIControl is TabViewControl tabViewControl && sender is TextBox textBox)
            {
                var viewModel = (RepeaterControlViewModel)tabViewControl.DataContext;
                
                // Apply text changes when focus is lost
                if (textBox.Text != null && textBox.Text != viewModel.SelectedTabText)
                {
                    // If we have a selected tab, update it (allow empty text)
                    if (viewModel.SelectedTabIndex >= 0 && viewModel.SelectedTabIndex < viewModel.TabHeaders.Count)
                    {
                        viewModel.UpdateTabText(textBox.Text);
                        // Refresh the tab headers to update the UI
                        tabViewControl.RefreshTabHeaders();
                    }
                    else if (textBox.Text != null && !viewModel.TabHeaders.Contains(textBox.Text))
                    {
                        // If no tab is selected but we have text that doesn't exist, create a new tab
                        // Only create new tab if text is not empty
                        if (!string.IsNullOrEmpty(textBox.Text))
                        {
                            viewModel.AddNewTabWithText(textBox.Text);
                            // Refresh the tab headers to update the UI
                            tabViewControl.RefreshTabHeaders();
                        }
                    }
                    
                    // Update the base text property to keep it in sync
                    viewModel.text = textBox.Text;
                }
            }
        }

        public void setMainViewModel(MainWindowViewModel mainViewModel)
        {
            m_MainVM = mainViewModel;
            
            // Subscribe to theme changes from the main view model
            if (m_MainVM != null)
            {
                m_MainVM.ThemeChanged += OnThemeChanged;
            }
        }

        private void OnThemeChanged(object? sender, bool isDark)
        {
            // Force the window to refresh its theme
            this.InvalidateVisual();
        }

        public override void Show()
        {
            Pos_X.Text = m_UIControl.m_nPositionX.ToString();
            Pos_Y.Text = m_UIControl.m_nPositionY.ToString();
            Size_W.Text = m_UIControl.m_nWidth.ToString();
            if (m_UIControl.m_nUIControlType == Models.CONTROL_TYPE.CONTAINERBOX || m_UIControl.m_nUIControlType == Models.CONTROL_TYPE.PROGRESS)
            {
                Size_H.Text = m_UIControl.m_nHeight.ToString();
            }
            else
            {
                Size_H.Text = m_UIControl.DesiredSize.Height.ToString();
            }
            Disable.IsChecked = m_UIControl.m_bDisable;

            base.Show();
        }
        public void setControlInfo(UIControlViewModel controlViewModel, UIControl control)
        {
            m_UIControl = control;
            //m_UIControlVM = controlViewModel;
            this.DataContext = controlViewModel;
            BGButton.Color = controlViewModel.background.Color;
            BCButton.Color = controlViewModel.borderColor.Color;
            FGButton.Color = controlViewModel.foreground.Color;

            // Remove Tab button visibility is now controlled by data binding to CanRemoveTab property
            
            // Set up interaction dropdown based on control type
            SetupInteractionDropdown(control);
            
            // Set up text binding for TabView controls
            if (control.GetType() == typeof(TabViewControl))
            {
                var tabViewModel = (RepeaterControlViewModel)controlViewModel;
                // Initialize the text field with the selected tab text
                if (tabViewModel.SelectedTabIndex >= 0 && tabViewModel.SelectedTabIndex < tabViewModel.TabHeaders.Count)
                {
                    tabViewModel.SelectedTabText = tabViewModel.TabHeaders[tabViewModel.SelectedTabIndex];
                    // Set the text property to show in the text field
                    controlViewModel.text = tabViewModel.SelectedTabText;
                }
                // Show text properties for TabView controls
                controlViewModel.IsTextPropertiesVisible = true;
                
                // Subscribe to tab selection changes to update the text field
                tabViewModel.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(tabViewModel.SelectedTabIndex))
                    {
                        if (tabViewModel.SelectedTabIndex >= 0 && tabViewModel.SelectedTabIndex < tabViewModel.TabHeaders.Count)
                        {
                            controlViewModel.text = tabViewModel.TabHeaders[tabViewModel.SelectedTabIndex];
                        }
                    }
                };
            }

            int index = 0;
            ComboBoxItem item0 = new ComboBoxItem();
            item0.Content = "None";
            ListPage.Items.Add(item0);
            item0.PointerPressed += onSetEvent;
            for (int i = 0; i < m_MainVM.m_lstWorkScreen.Count; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = m_MainVM.m_lstWorkScreen[i].m_strName;
                ListPage.Items.Add(item);
                item.PointerPressed += onSetEvent;
                if (m_UIControl.m_TapEvent == m_MainVM.m_lstWorkScreen[i].m_strName)
                    index = i + 1;
            }
            ListPage.SelectedIndex = index;
            onShowEvent();
        }
        private void onNewScreen(object? sender, RoutedEventArgs routedEventArgs)
        {
            m_MainVM.createPage("");
            int index = m_MainVM.m_lstWorkScreen.Count;
            ComboBoxItem item = new ComboBoxItem();
            item.Content = m_MainVM.m_lstWorkScreen[index-1].m_strName;
            ListPage.Items.Add(item);
            item.PointerPressed += onSetEvent;
            ListPage.SelectedIndex = index;
            onSetEvent(m_MainVM.m_lstWorkScreen[index-1].m_strName);
        }
        private void onSetEvent(object? sender, PointerPressedEventArgs pointerPressedEventArgs)
        {
            if (sender == null) return;
            ComboBoxItem item = (ComboBoxItem)sender;
            onSetEvent(item.Content.ToString());
        }
        private void onSetEvent([AllowNull] string strPage)
        {
            if (strPage == "None" || strPage == m_MainVM.WorkScreen.m_strName)
                strPage = null;
            if(Event.SelectedIndex == 0)
                m_UIControl.setTapEvent(strPage);
            else if(Event.SelectedIndex == 1)
                m_UIControl.setDTapEvent(strPage);
            else if(Event.SelectedIndex == 2)
                m_UIControl.setHPressEvent(strPage);
            else if(Event.SelectedIndex == 3)
                m_UIControl.setSwipeLeftEvent(strPage);
            else if(Event.SelectedIndex == 4)
                m_UIControl.setSwipeRightEvent(strPage);
            else if(Event.SelectedIndex == 5)
                m_UIControl.setSwipeUpEvent(strPage);
            else
                m_UIControl.setSwipeDownEvent(strPage);
            onShowEvent();
        }
        private void onShowEvent()
        {
            if (m_UIControl.m_TapEvent != null)
            {
                TapBorder.IsVisible = true;
                Tap.Text = m_UIControl.m_TapEvent;
            }
            else TapBorder.IsVisible = false;
            if (m_UIControl.m_DTapEvent != null)
            {
                DTapBorder.IsVisible = true;
                DTap.Text = m_UIControl.m_DTapEvent;
            }
            else DTapBorder.IsVisible = false;
            if (m_UIControl.m_HPressEvent != null)
            {
                HPressBorder.IsVisible = true;
                HPressTap.Text = m_UIControl.m_HPressEvent;
            }
            else HPressBorder.IsVisible = false;
        }
        private void onSelectEvent(object? sender, SelectionChangedEventArgs e)
        {
            if(m_UIControl==null) return;
            ComboBox comboBox = (ComboBox)sender;
            int index = 0;
            
            // Handle checkbox-specific interactions
            if (m_UIControl.m_nUIControlType == CONTROL_TYPE.CHECK)
            {
                if (comboBox.SelectedIndex == 0) // "Checks"
                {
                    // Set checkbox to checked
                    m_UIControl.m_ControlViewModel.isChecked = true;
                }
                else if (comboBox.SelectedIndex == 1) // "Unchecks"
                {
                    // Set checkbox to unchecked
                    m_UIControl.m_ControlViewModel.isChecked = false;
                }
                return;
            }
            
            // Handle standard interactions for other controls
            if (comboBox.SelectedIndex == 0)
                index = selectPage(m_UIControl.m_TapEvent);
            else if(comboBox.SelectedIndex == 1)
                index = selectPage(m_UIControl.m_DTapEvent);
            else if(comboBox.SelectedIndex == 2)
                index = selectPage(m_UIControl.m_HPressEvent);
            else if(comboBox.SelectedIndex == 3)
                index = selectPage(m_UIControl.m_SwipeLeftEvent);
            else if(comboBox.SelectedIndex == 4)
                index = selectPage(m_UIControl.m_SwipeRightEvent);
            else if(comboBox.SelectedIndex == 5)
                index = selectPage(m_UIControl.m_SwipeUpEvent);
            else if(comboBox.SelectedIndex == 6)
                index = selectPage(m_UIControl.m_SwipeDownEvent);
            ListPage.SelectedIndex = index;
        }
        private int selectPage(string page)
        {
            for (int i = 0; i < m_MainVM.m_lstWorkScreen.Count; i++)
            {
               if (page == m_MainVM.m_lstWorkScreen[i].m_strName)
                    return i + 1;
            }
            return 0;
        }
        private void setTextBold(object? sender, RoutedEventArgs e)
        {
            m_UIControl.setTextBold((bool)BoldButton.IsChecked);
        }
        private void setTextItalic(object? sender, RoutedEventArgs e)
        {
            m_UIControl.setTextItalic((bool)ItalicButton.IsChecked);
        }
        private void onADDACTION(object sender, RoutedEventArgs e)
        {
            Base.IsVisible = false;
            AddAction.IsVisible = true;
        }
        private void onBack(object sender, RoutedEventArgs e)
        {
            Base.IsVisible = true;
            AddAction.IsVisible = false;
        }
        /*private void onSetFontSizeButton(object? sender, RoutedEventArgs e)
        {*/
            
            //ppopup.Topmost = false;
            //ppopup.StaysOpen = "False";
            //ppopup.PlacementMode = PlacementMode.AnchorAndGravity;
            //ppopup.PlacementGravity = Avalonia.Controls.Primitives.PopupPositioning.PopupGravity.Bottom;
            //ppopup.PlacementAnchor = Avalonia.Controls.Primitives.PopupPositioning.PopupAnchor.Bottom;
            //ppopup.PlacementConstraintAdjustment = Avalonia.Controls.Primitives.PopupPositioning.PopupPositionerConstraintAdjustment.FlipY;
            //ppopup.PlacementTarget = FontSizeButton;
            //ppopup.Open();
        //}
        private void SetControlPostionX(object? sender, RoutedEventArgs routedEventArgs)
        {
            if (sender is TextBox textBox && m_UIControl != null)
            {
                try
                {
                    int w_nControlX = int.Parse(textBox.Text);
                    if (w_nControlX >= 0)
                        m_UIControl.setPositionX(w_nControlX);
                }
                catch (Exception e)
                {
                    textBox.Text = m_UIControl.m_nPositionX.ToString();
                }
            }
        }
        private void SetControlPostionY(object? sender, RoutedEventArgs routedEventArgs)
        {
            if (sender is TextBox textBox && m_UIControl != null)
            {
                try
                {
                    int w_nControlY = int.Parse(textBox.Text);
                    if (w_nControlY >= 0)
                        m_UIControl.setPositionY(w_nControlY);
                }
                catch (Exception e)
                {
                    textBox.Text = m_UIControl.m_nPositionY.ToString();
                }
            }
        }
        private void OnXTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Handle null or empty text
                string originalText = textBox.Text ?? "";
                
                // Remove non-numeric characters - handle empty string case
                string numericOnly = string.IsNullOrEmpty(originalText) ? "" : 
                    new string(originalText.Where(c => char.IsDigit(c)).ToArray());
                
                if (originalText != numericOnly)
                {
                    int cursorPosition = textBox.CaretIndex;
                    textBox.Text = numericOnly;
                    textBox.CaretIndex = Math.Min(cursorPosition, numericOnly.Length);
                }
                
                // Apply min/max limits only if we have a valid number
                if (!string.IsNullOrEmpty(numericOnly) && int.TryParse(numericOnly, out int value))
                {
                    const int minX = 0;
                    const int maxX = 2000;
                    
                    if (value < minX)
                    {
                        textBox.Text = minX.ToString();
                    }
                    else if (value > maxX)
                    {
                        textBox.Text = maxX.ToString();
                    }
                }
            }
        }
        
        private void OnYTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Handle null or empty text
                string originalText = textBox.Text ?? "";
                
                // Remove non-numeric characters - handle empty string case
                string numericOnly = string.IsNullOrEmpty(originalText) ? "" : 
                    new string(originalText.Where(c => char.IsDigit(c)).ToArray());
                
                if (originalText != numericOnly)
                {
                    int cursorPosition = textBox.CaretIndex;
                    textBox.Text = numericOnly;
                    textBox.CaretIndex = Math.Min(cursorPosition, numericOnly.Length);
                }
                
                // Apply min/max limits only if we have a valid number
                if (!string.IsNullOrEmpty(numericOnly) && int.TryParse(numericOnly, out int value))
                {
                    const int minY = 0;
                    const int maxY = 2000;
                    
                    if (value < minY)
                    {
                        textBox.Text = minY.ToString();
                    }
                    else if (value > maxY)
                    {
                        textBox.Text = maxY.ToString();
                    }
                }
            }
        }
        
        private void OnWidthTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Handle null or empty text
                string originalText = textBox.Text ?? "";
                
                // Remove non-numeric characters - handle empty string case
                string numericOnly = string.IsNullOrEmpty(originalText) ? "" : 
                    new string(originalText.Where(c => char.IsDigit(c)).ToArray());
                
                if (originalText != numericOnly)
                {
                    int cursorPosition = textBox.CaretIndex;
                    textBox.Text = numericOnly;
                    textBox.CaretIndex = Math.Min(cursorPosition, numericOnly.Length);
                }
                
                // Apply min/max limits only if we have a valid number
                if (!string.IsNullOrEmpty(numericOnly) && int.TryParse(numericOnly, out int value))
                {
                    const int minWidth = 1;
                    const int maxWidth = 2000;
                    
                    if (value < minWidth)
                    {
                        textBox.Text = minWidth.ToString();
                    }
                    else if (value > maxWidth)
                    {
                        textBox.Text = maxWidth.ToString();
                    }
                }
            }
        }
        
        private void SetControlWidth(object? sender, RoutedEventArgs routedEventArgs)
        {
            if (sender is TextBox textBox && m_UIControl != null)
            {
                try
                {
                    int w_nControlWidth = int.Parse(textBox.Text);
                    if (fitWidth.IsChecked == true && m_UIControl.m_nWidth != w_nControlWidth)
                        fitWidth.IsChecked = false;
                    if (w_nControlWidth > 0)
                        m_UIControl.setWidth(w_nControlWidth);
                }
                catch (Exception e)
                {
                    textBox.Text = m_UIControl.m_nWidth.ToString();
                }
            }
        }
        private void OnHeightTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Handle null or empty text
                string originalText = textBox.Text ?? "";
                
                // Remove non-numeric characters - handle empty string case
                string numericOnly = string.IsNullOrEmpty(originalText) ? "" : 
                    new string(originalText.Where(c => char.IsDigit(c)).ToArray());
                
                if (originalText != numericOnly)
                {
                    int cursorPosition = textBox.CaretIndex;
                    textBox.Text = numericOnly;
                    textBox.CaretIndex = Math.Min(cursorPosition, numericOnly.Length);
                }
                
                // Apply min/max limits only if we have a valid number
                if (!string.IsNullOrEmpty(numericOnly) && int.TryParse(numericOnly, out int value))
                {
                    const int minHeight = 1;
                    const int maxHeight = 2000;
                    
                    if (value < minHeight)
                    {
                        textBox.Text = minHeight.ToString();
                    }
                    else if (value > maxHeight)
                    {
                        textBox.Text = maxHeight.ToString();
                    }
                }
            }
        }
        
        private void SetControlHeight(object? sender, RoutedEventArgs routedEventArgs)
        {
            if (sender is TextBox textBox && m_UIControl != null)
            {
                try
                {
                    int w_nControlHeight = int.Parse(textBox.Text);
                    if (fitHeight.IsChecked == true && m_UIControl.m_nHeight != w_nControlHeight)
                        fitHeight.IsChecked = false;
                    if (w_nControlHeight > 0)
                        m_UIControl.setHeight(w_nControlHeight);
                }
                catch (Exception e)
                {
                    textBox.Text = m_UIControl.m_nHeight.ToString();
                }
            }
        }
        private void onSetFitWidth(object? sender, RoutedEventArgs e)
        {
            if (m_UIControl.GetType() == typeof(LabelControl) && fitWidth.IsChecked==true)
                ((LabelControl)m_UIControl).setFitWidth();
            Size_W.Text = m_UIControl.m_nWidth.ToString();
            //if (fitHeight.IsChecked == true)
            //    onSetFitHeight(sender, e);
        }
        private void onSetFitHeight(object? sender, RoutedEventArgs e)
        {
            if (m_UIControl.GetType() == typeof(LabelControl) && fitHeight.IsChecked==true)
                ((LabelControl)m_UIControl).setFitHeight();
            Size_H.Text = m_UIControl.m_nHeight.ToString();
        }

        private void onDisable(object? sender, RoutedEventArgs e)
        {
            m_UIControl.m_bDisable = (Disable.IsChecked == true);
        }

        private void RemoveTab_Click(object? sender, RoutedEventArgs e)
        {
            if (m_UIControl is TabViewControl tabViewControl)
            {
                var viewModel = (RepeaterControlViewModel)tabViewControl.DataContext;
                viewModel.RemoveTab.Execute().Subscribe();
            }
        }

        private void SetupInteractionDropdown(UIControl control)
        {
            // Clear existing items
            Event.Items.Clear();
            
            // Add items based on control type
            if (control.m_nUIControlType == CONTROL_TYPE.CHECK)
            {
                // Add checkbox-specific interactions
                Event.Items.Add(new ComboBoxItem { Content = "Checks", Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)) });
                Event.Items.Add(new ComboBoxItem { Content = "Unchecks", Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)) });
            }
            else
            {
                // Add standard interactions for other controls
                Event.Items.Add(new ComboBoxItem { Content = "Tapped", Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)) });
                Event.Items.Add(new ComboBoxItem { Content = "Double Tapped", Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)) });
                Event.Items.Add(new ComboBoxItem { Content = "Presses and holds", Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)) });
                Event.Items.Add(new ComboBoxItem { Content = "SwipeLeft", Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)) });
                Event.Items.Add(new ComboBoxItem { Content = "SwipeRight", Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)) });
                Event.Items.Add(new ComboBoxItem { Content = "SwipeUp", Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)) });
                Event.Items.Add(new ComboBoxItem { Content = "SwipeDown", Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)) });
            }
            
            // Set default selection
            Event.SelectedIndex = 0;
        }

        
    }
}
