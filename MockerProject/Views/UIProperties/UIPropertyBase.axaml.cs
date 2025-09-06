using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using HarfBuzzSharp;
using MockerProject.ViewModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static MockerProject.ViewModels.UIControlViewModel;

namespace MockerProject.Views.UIProperties
{
    public partial class UIPropertyBase : UserControl
    {
        [AllowNull] private UIControl m_UIControl;
        [AllowNull] public UIControlViewModel m_ControlModel;
        public UIPropertyBase()
        {
            InitializeComponent();

            BGButton.PropertyChanged += (s, e) =>
            {
                if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty)
                {

                    Color w_Color = BGButton.Color;
                    SolidColorBrush w_Brush = new SolidColorBrush(w_Color);
                    if (m_ControlModel == null) return;
                    m_ControlModel.background = w_Brush;
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
                    if (m_ControlModel == null) return;
                    m_ControlModel.borderColor = w_Brush;
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
                    if (m_ControlModel == null) return;
                    m_ControlModel.foreground = (w_Brush);
                    m_UIControl.setForeground(w_Brush);
                }
            };
        }

        public virtual void setModel(UIControlViewModel model, UIControl uIControl)
        {
            m_ControlModel = model;
            this.DataContext = m_ControlModel;
            m_UIControl = uIControl;
            BGButton.Color = model.background.Color;
            BCButton.Color = model.borderColor.Color;
            FGButton.Color = model.foreground.Color;

            int index = 0;
            ComboBoxItem item0 = new ComboBoxItem();
            item0.Content = "None";
            ListPage.Items.Add(item0);

            for (int i = 0; i < m_UIControl.m_MainViewModel.m_lstWorkScreen.Count; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = m_UIControl.m_MainViewModel.m_lstWorkScreen[i].m_strName;
                ListPage.Items.Add(item);
            }

            index = Event.SelectedIndex;
            ItemCollection items = ListPage.Items;
            foreach (ComboBoxItem item in items)
            {
                if (item.Content == m_ControlModel.iterationItems[index].iteration)
                {
                    ListPage.SelectedItem = item;
                    break;
                }
            }
        }

        private void onADDACTION(object sender, RoutedEventArgs e)
        {
            if (m_ControlModel == null) return;
            m_ControlModel.IterationVisible = true;
        }

        private void onBack(object sender, RoutedEventArgs e)
        {
            if (m_ControlModel == null) return;
            m_ControlModel.IterationVisible = false;
        }

        private void onSelectEvent(object? sender, SelectionChangedEventArgs e)
        {
            int index = Event.SelectedIndex;
            IterationItem selectedItem = ((IterationItem)Event.SelectedItem);
            if (selectedItem == null) return;
            if (selectedItem.text == "Selects") return;
            
            ItemCollection items = ListPage.Items;
            foreach (ComboBoxItem item in items)
            {
                if (item.Content == m_ControlModel.iterationItems[index].iteration)
                {
                    ListPage.SelectedItem = item;
                    break;
                }
            }
        }

        private void onSelectPage(object? sender, SelectionChangedEventArgs e)
        {
            int index = Event.SelectedIndex;
            ComboBoxItem item = (ComboBoxItem)ListPage.SelectedItem;
            m_ControlModel.iterationItems[index].iteration = (string)item.Content;
        }

        private void onNewScreen(object? sender, RoutedEventArgs routedEventArgs)
        {
            m_UIControl.m_MainViewModel.createPage("");
            int index = m_UIControl.m_MainViewModel.m_lstWorkScreen.Count;
            ComboBoxItem item = new ComboBoxItem();
            item.Content = m_UIControl.m_MainViewModel.m_lstWorkScreen[index - 1].m_strName;
            ListPage.Items.Add(item);
            item.PointerPressed += onSetEvent;
            ListPage.SelectedIndex = index;
            onSetEvent(m_UIControl.m_MainViewModel.m_lstWorkScreen[index - 1].m_strName);
        }

        private void SetControlPostionX(object? sender, RoutedEventArgs routedEventArgs)
        {
            if (sender is TextBox textBox)
            {
                try
                {
                    int w_nControlX = int.Parse(textBox.Text);
                    if (w_nControlX >= 0)
                    {

                        m_UIControl.setPositionX(w_nControlX);
                    }
                }

                catch (Exception e)
                {
                    textBox.Text = m_UIControl.m_nPositionX.ToString();
                }
            }
        }
        
        private void SetControlPostionY(object? sender, RoutedEventArgs routedEventArgs)
        {
            if (sender is TextBox textBox)
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
        private void OnWidthTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Remove non-numeric characters
                string originalText = textBox.Text;
                string numericOnly = new string(originalText.Where(c => char.IsDigit(c)).ToArray());
                
                if (originalText != numericOnly)
                {
                    int cursorPosition = textBox.CaretIndex;
                    textBox.Text = numericOnly;
                    textBox.CaretIndex = Math.Min(cursorPosition, numericOnly.Length);
                }
                
                // Apply min/max limits
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
                
                // Update the binding
                if (m_ControlModel != null && int.TryParse(textBox.Text, out int widthValue))
                {
                    m_ControlModel.width = widthValue;
                }
            }
        }
        
        private void OnHeightTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Remove non-numeric characters
                string originalText = textBox.Text;
                string numericOnly = new string(originalText.Where(c => char.IsDigit(c)).ToArray());
                
                if (originalText != numericOnly)
                {
                    int cursorPosition = textBox.CaretIndex;
                    textBox.Text = numericOnly;
                    textBox.CaretIndex = Math.Min(cursorPosition, numericOnly.Length);
                }
                
                // Apply min/max limits
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
                
                // Update the binding
                if (m_ControlModel != null && int.TryParse(textBox.Text, out int heightValue))
                {
                    m_ControlModel.height = heightValue;
                }
            }
        }
        //private void onSetFitWidth(object? sender, RoutedEventArgs e)
        //{
        //}
        //private void onSetFitHeight(object? sender, RoutedEventArgs e)
        //{

        //}

        private void onDisable(object? sender, RoutedEventArgs e)
        {
            m_ControlModel.isEnabled = (Disable.IsChecked == false);
        }

        private void onSetEvent(object? sender, PointerPressedEventArgs pointerPressedEventArgs)
        {
            if (sender == null) return;
            ComboBoxItem item = (ComboBoxItem)sender;
            onSetEvent(item.Content.ToString());
        }
        private void onSetEvent([AllowNull] string strPage)
        {

            onShowEvent();
        }
        private void onShowEvent()
        {

        }
    }
}