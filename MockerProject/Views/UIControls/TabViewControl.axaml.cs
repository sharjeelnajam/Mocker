using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using MockerProject.ViewModels.UIViewModels;

namespace MockerProject.Views.UIControls
{
    public partial class TabViewControl : UIControl
    {
        public TabViewControl()
        {
            InitializeComponent();
            m_ControlViewModel = new RepeaterControlViewModel(this);
            this.DataContext = m_ControlViewModel;
            setName("TabView");
            setWidth(310);
            setHeight(270);
            setForeground(new SolidColorBrush(new Color(255, 0, 0, 0)));
        }

        private void tabControl_ContainerPrepared(object? sender, ContainerPreparedEventArgs e)
        {
            if (e.Container is TabItem tabItem)
            {
                var vm = DataContext as RepeaterControlViewModel;
                if (vm == null) return;

                var item = vm.Items[e.Index];
                string headerText = vm.TabHeaders[e.Index];

                TextBlock textBlock = new TextBlock();
                textBlock.Text = headerText;
                textBlock.Foreground = new SolidColorBrush(new Color(255, 0, 0, 0));
                tabItem.Header = textBlock;
            }
        }

        public override void doubleClickHandler(object sender, TappedEventArgs e) { }

        private void Binding(object? sender, Avalonia.Interactivity.RoutedEventArgs e) { }

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
        }
    }
}
