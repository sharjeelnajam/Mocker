using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using MockerProject.Models;
using MockerProject.ViewModels;
using MockerProject.ViewModels.UIViewModels;
using MockerProject.Views.UIProperties;
using static MockerProject.ViewModels.UIControlViewModel;

namespace MockerProject.Views.UIControls
{
    public partial class ListBoxControl : UIControl
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

        public ListBoxControl()
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
            setSize(150, 200);
            setName("ListBox");
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
        
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}