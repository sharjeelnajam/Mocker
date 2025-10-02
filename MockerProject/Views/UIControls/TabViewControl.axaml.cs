using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
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

                // Create main container for the tab header with top positioning
                var mainGrid = new Grid();
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Top row for + buttons
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Tab content row
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Tab content

                // Left + button (top-left) - Using Border with TextBlock for better control
                var leftAddButton = new Border
                {
                    Width = 14,
                    Height = 14,
                    Margin = new Thickness(1, 1, 0, 0),
                    Background = new SolidColorBrush(Color.FromRgb(255, 165, 0)), // Orange background
                    CornerRadius = new CornerRadius(2),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
                    IsEnabled = true,
                    IsVisible = true,
                    Child = new TextBlock
                    {
                        Text = "+",
                        Foreground = new SolidColorBrush(Colors.Black), // Black + sign
                        FontWeight = FontWeight.Bold,
                        FontSize = 10,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                    }
                };
                
                // Add click event to the border
                leftAddButton.PointerPressed += (s, args) => 
                {
                    System.Diagnostics.Debug.WriteLine("Left + button clicked!");
                    vm.SelectedTabIndex = e.Index;
                    vm.InsertBeforeItems();
                };

                // Right + button (top-right) - Using Border with TextBlock for better control
                var rightAddButton = new Border
                {
                    Width = 14,
                    Height = 14,
                    Margin = new Thickness(0, 1, 1, 0),
                    Background = new SolidColorBrush(Color.FromRgb(255, 165, 0)), // Orange background
                    CornerRadius = new CornerRadius(2),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
                    IsEnabled = true,
                    IsVisible = true,
                    Child = new TextBlock
                    {
                        Text = "+",
                        Foreground = new SolidColorBrush(Colors.Black), // Black + sign
                        FontWeight = FontWeight.Bold,
                        FontSize = 10,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                    }
                };
                
                // Add click event to the border
                rightAddButton.PointerPressed += (s, args) => 
                {
                    System.Diagnostics.Debug.WriteLine("Right + button clicked!");
                    vm.SelectedTabIndex = e.Index;
                    vm.InsertAfterItems();
                };

                // Tab content area
                var contentGrid = new Grid();
                contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var textBlock = new TextBlock
                {
                    Text = headerText,
                    Foreground = new SolidColorBrush(new Color(255, 0, 0, 0)),
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                };

                var textBox = new TextBox
                {
                    Text = headerText,
                    IsVisible = false,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
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

                // Add elements to content grid
                Grid.SetColumn(textBlock, 0);
                Grid.SetColumn(textBox, 0);
                
                contentGrid.Children.Add(textBlock);
                contentGrid.Children.Add(textBox);

                // Add elements to main grid
                Grid.SetRow(leftAddButton, 0);
                Grid.SetColumn(leftAddButton, 0);
                Grid.SetRow(rightAddButton, 0);
                Grid.SetColumn(rightAddButton, 0);
                Grid.SetRow(contentGrid, 1);
                Grid.SetColumn(contentGrid, 0);
                
                mainGrid.Children.Add(leftAddButton);
                mainGrid.Children.Add(rightAddButton);
                mainGrid.Children.Add(contentGrid);

                tabItem.Header = mainGrid;
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

        public void UpdateTabContentBackground(SolidColorBrush color)
        {
            if (tabControl != null)
            {
                // The background binding in XAML will handle updating the selected tab content area
                // This method is called when the background property changes
                // The XAML binding will automatically update the selected tab's background
            }
        }
    }
}
