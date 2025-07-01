using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using MockerProject.ViewModels;
using MockerProject.ViewModels.UIViewModels;

namespace MockerProject.Views.UIControls
{
    public partial class RepeaterControl : UIControl
    {
        public RepeaterControl()
        {
            InitializeComponent();
            m_ControlViewModel = new RepeaterControlViewModel(this);
            this.DataContext = m_ControlViewModel;
            setName("Repeater");
            setWidth(240);
            setHeight(300);
            setBackground(new SolidColorBrush(new Color(0, 200, 200, 200)));
            //setForeground(new SolidColorBrush(new Color(255, 33, 33, 33)));
            //setBorderColor(new SolidColorBrush(new Color(255, 77, 77, 77)));
            //setBorderThickness(1);
            //setBorderRound(5);
        }
        public override void doubleClickHandler(object sender, TappedEventArgs e)
        {
            
        }

        private void Binding(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

        }

        public override void MousePressEvent(object sender, PointerEventArgs e)
        {
            foreach (ContainerBoxControl item in ((RepeaterControlViewModel)m_ControlViewModel).Items)
            {
                if (item.closeBtn.IsVisible) return;
            }
            base.MousePressEvent(sender, e);
           

        }

        public override void MouseMoveEvent(object sender, PointerEventArgs e)
        {
            foreach (ContainerBoxControl item in ((RepeaterControlViewModel)m_ControlViewModel).Items)
            {
                if (item.closeBtn.IsVisible) return;
            }
            base.MouseMoveEvent(sender, e);
            //if (!closeBtn.IsVisible)
            //{
            //   
            //}
        }
    }
}
    