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

                var grid = new Grid();

                var textBlock = new TextBlock
                {
                    Text = headerText,
                    Foreground = new SolidColorBrush(new Color(255, 0, 0, 0))
                };

                var textBox = new TextBox
                {
                    Text = headerText,
                    IsVisible = false
                };

                textBlock.DoubleTapped += (s, args) =>
                {
                    textBlock.IsVisible = false;
                    textBox.IsVisible = true;
                    textBox.Focus();
                    textBox.CaretIndex = textBox.Text?.Length ?? 0;
                };

                textBox.KeyDown += (s, args) =>
                {
                    if (args.Key == Key.Enter)
                    {
                        textBlock.Text = textBox.Text;
                        vm.TabHeaders[e.Index] = textBox.Text;
                        textBox.IsVisible = false;
                        textBlock.IsVisible = true;
                    }
                };

                textBox.LostFocus += (s, args) =>
                {
                    textBlock.Text = textBox.Text;
                    vm.TabHeaders[e.Index] = textBox.Text;
                    textBox.IsVisible = false;
                    textBlock.IsVisible = true;
                };

                grid.Children.Add(textBlock);
                grid.Children.Add(textBox);

                tabItem.Header = grid;
            }
        }

        public override void doubleClickHandler(object sender, TappedEventArgs e)
        {
            // Call base method to show property window like other controls
            base.doubleClickHandler(sender, e);
        }

        private void Binding(object? sender, Avalonia.Interactivity.RoutedEventArgs e) { }

        private void TabControl_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is TabControl tabControl && m_ControlViewModel is RepeaterControlViewModel viewModel)
            {
                // Update the SelectedTabIndex when the user selects a different tab
                viewModel.SelectedTabIndex = tabControl.SelectedIndex;
            }
        }

        public override void MousePressEvent(object sender, PointerEventArgs e)
        {
            foreach (ContainerBoxControl item in ((RepeaterControlViewModel)m_ControlViewModel).Items)
            {
                if (item.closeBtn.IsVisible) return;
            }
            base.MousePressEvent(sender, e);
        }

        public void RefreshTabHeaders()
        {
            if (m_ControlViewModel is RepeaterControlViewModel viewModel)
            {
                // Force the TabControl to refresh by temporarily clearing and restoring the ItemsSource
                var items = viewModel.Items;
                var selectedIndex = viewModel.SelectedTabIndex;
                
                // Temporarily clear the ItemsSource
                tabControl.ItemsSource = null;
                
                // Restore the ItemsSource
                tabControl.ItemsSource = items;
                
                // Restore the selected index
                tabControl.SelectedIndex = selectedIndex;
            }
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
