using Avalonia.Controls;
using MockerProject.ViewModels;
using MockerProject.ViewModels.UIViewModels;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using MockerProject.Models;
using static MockerProject.ViewModels.UIControlViewModel;

namespace MockerProject.Views.UIProperties
{
    public partial class UIListBoxProperty : Window 
    {
        [AllowNull] private ListBoxViewModel m_ControlModel;
        [AllowNull] private UIControl m_UIControl;
        public UIListBoxProperty()
        {
            InitializeComponent();

            //Item Background Color Button setting
            ItemBGButton.PropertyChanged += (s, e) =>
            {
                if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty)
                {

                    Color w_Color = ItemBGButton.Color;
                    SolidColorBrush w_Brush = new SolidColorBrush(w_Color);
                    if (m_ControlModel == null) return;
                    m_ControlModel.itemBackground = w_Brush;

                }
            };

        }
        //set model
        public void setModel(UIControlViewModel model, UIControl uIControl)
        {
            
            ListBoxViewModel listBoxViewModel = (ListBoxViewModel)uIControl.DataContext;
            m_UIControl = uIControl;
            m_ControlModel =(ListBoxViewModel)uIControl.DataContext;
            
            this.DataContext = m_ControlModel;
            BaseProperty.DataContext = m_ControlModel;
            BaseProperty.setModel((UIControlViewModel) m_ControlModel, m_UIControl);

            //Event select Page
            BaseProperty.ListPage.AddHandler(ComboBox.SelectionChangedEvent, (sender, e) =>
            {
                int index = BaseProperty.Event.SelectedIndex;
                if (index != 0) return;
                index = SelectItems.SelectedIndex;
                ComboBoxItem item = (ComboBoxItem)BaseProperty.ListPage.SelectedItem;
                m_ControlModel.Items[index].iteration = (string)item.Content;
            });

            //Event Select listItem
            BaseProperty.Event.AddHandler(ComboBox.SelectionChangedEvent, (sender, e) =>
            {
                int index = BaseProperty.Event.SelectedIndex;
                if (index < 1)
                {
                    SelectStack.IsVisible = true;
                    index = SelectItems.SelectedIndex;
                    ItemCollection items = BaseProperty.ListPage.Items;
                    foreach (ComboBoxItem item in items)
                    {
                        if (item.Content == m_ControlModel.Items[index].iteration)
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
            int index = BaseProperty.Event.SelectedIndex;
            if (index != 0) return;
            index = SelectItems.SelectedIndex;
            ItemCollection items = BaseProperty.ListPage.Items;
            foreach (ComboBoxItem item in items)
            {
                if (item.Content == m_ControlModel.Items[index].iteration)
                {
                    BaseProperty.ListPage.SelectedItem = item;
                    break;
                }
            }
        }

    }
}
