using Avalonia.Controls;
using MockerProject.ViewModels.UIViewModels;
using MockerProject.ViewModels;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using MockerProject.Models;

namespace MockerProject.Views.UIProperties
{
    public partial class UITreeViewProperty : Window
    {
        [AllowNull] private TreeViewViewModel m_ControlModel;
        [AllowNull] private UIControl m_UIControl;
        public UITreeViewProperty()
        {
            InitializeComponent();
            //ItemBGButton.PropertyChanged += (s, e) =>
            //{
            //    if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty)
            //    {

            //        Color w_Color = ItemBGButton.Color;
            //        SolidColorBrush w_Brush = new SolidColorBrush(w_Color);
            //        if (m_ControlModel == null) return;
            //        m_ControlModel.itemBackground = w_Brush;

            //    }
            //};
        }

        public void setModel(UIControlViewModel model, UIControl uIControl)
        {
            TreeViewViewModel listBoxViewModel = (TreeViewViewModel)uIControl.DataContext;
            m_UIControl = uIControl;
            m_ControlModel = (TreeViewViewModel)uIControl.DataContext;

            this.DataContext = m_ControlModel;
            BaseProperty.DataContext = m_ControlModel;
            BaseProperty.setModel((UIControlViewModel)m_ControlModel, m_UIControl);
            BaseProperty.ListPage.AddHandler(ComboBox.SelectionChangedEvent, (sender, e) =>
            {
                int index = BaseProperty.Event.SelectedIndex;
                if (index != 0) return;

                Node selectedItem = (Node)SelectItems.SelectedItem;
                if (selectedItem == null) return;

                ComboBoxItem item = (ComboBoxItem)BaseProperty.ListPage.SelectedItem;
                selectedItem.iteration = (string)item.Content;
            });
            BaseProperty.Event.AddHandler(ComboBox.SelectionChangedEvent, (sender, e) =>
            {
                int index = BaseProperty.Event.SelectedIndex;
                //check Event is Selects
                if (index < 1)
                {
                    SelectStack.IsVisible = true;
                    Node SelectedItem = (Node)SelectItems.SelectedItem;

                    if (SelectedItem == null) return;
                    ItemCollection items = BaseProperty.ListPage.Items;

                    foreach (ComboBoxItem item in items)
                    {
                        if (item.Content == SelectedItem.iteration)
                        {
                            BaseProperty.ListPage.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    SelectStack.IsVisible = false;
                }
            });
        }

        private void onSelectItems(object? sender, SelectionChangedEventArgs e)
        {
            //check Event is Selects
            int index = BaseProperty.Event.SelectedIndex;
            if (index != 0) return;

            Node selectedItem = (Node)SelectItems.SelectedItem;
            if (selectedItem == null) return;

            ItemCollection items = BaseProperty.ListPage.Items;
            foreach (ComboBoxItem item in items)
            {
                if (item.Content == selectedItem.iteration)
                {
                    BaseProperty.ListPage.SelectedItem = item;
                    break;
                }
            }
        }
    }
}