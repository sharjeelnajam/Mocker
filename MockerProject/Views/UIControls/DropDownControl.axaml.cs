using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using MockerProject.Models;
using MockerProject.ViewModels;
using MockerProject.ViewModels.UIViewModels;
using MockerProject.Views.UIProperties;
using static MockerProject.ViewModels.UIControlViewModel;

namespace MockerProject.Views.UIControls
{
    public partial class DropDownControl : UIControl
    {
        [AllowNull] public UIListBoxProperty wind;

        public void setAddItem(string m_text = "New Item")
        {
            CustomItem item = new CustomItem
            {
                text = m_text,
                Visible = true,
            };
            listBox.Items.Add(item);
        }
        public DropDownControl()
        {
            InitializeComponent();
            m_ControlViewModel = new ListBoxViewModel(this);
            this.DataContext = m_ControlViewModel;
            IterationItem item = new IterationItem
            {
                text = "Selects",
                type = EventType.EVENT_SELECTITEM,
                iteration = "None",

            };
            m_ControlViewModel.iterationItems.Insert(0, item);
            //setWidth(150);
            //setHeight(200);
            setSize(150, 50);
            setName("DropDown");
            setBackground(new SolidColorBrush(new Color(0, 200, 200, 200)));
            setForeground(new SolidColorBrush(new Color(255, 0, 33, 33)));
            setBorderColor(new SolidColorBrush(new Color(255, 77, 77, 77)));
            setBorderThickness(1);
            setBorderRound(5);
            wind = null;
            //m_ControlViewModel.IsMultiItemVisible = true;
            // Canvas.SetTop(listBox, 50);

            this.AddHandler(Control.KeyDownEvent, (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    CustomItem item = (CustomItem)listBox.SelectedItem;
                    if (item != null)
                        item.Visible = !item.Visible;
                }
            }, handledEventsToo: true);

            this.AddHandler(Control.TappedEvent, (sender, e) =>
            {
                CustomItem item = (CustomItem)listBox.SelectedItem;
                if (item != null)
                    item.Visible = !item.Visible;
            }, handledEventsToo: true);
        }
      

        public override void doubleClickHandler(object sender, TappedEventArgs e)
        {
            // Mark the event as handled to prevent the base class from also opening a window
            e.Handled = true;
            
            Point cP = e.GetPosition(this);
            Point mP = e.GetPosition(m_MainViewModel.m_MainWindow);
            PixelPoint cPP = new PixelPoint((int)(mP.X - cP.X + m_nWidth), (int)(mP.Y - cP.Y));
            PixelPoint nPP = m_MainViewModel.m_MainWindow.Position;

            if (wind != null)
            {
                wind.Close();
            }
            wind = new UIListBoxProperty();

            wind.setModel((ListBoxViewModel)m_ControlViewModel, this);
            wind.Position = nPP + cPP;
            wind.Show();

        }
        private void ClickDropDown(object sender, RoutedEventArgs e)
        {
            listBox.IsVisible = !listBox.IsVisible;
        }

        private void Selected_Item(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                SelectText.Text = ((CustomItem)listBox.SelectedItem).text;
            }
        }
    }
}
