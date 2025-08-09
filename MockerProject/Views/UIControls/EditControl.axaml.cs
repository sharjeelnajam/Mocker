using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using MockerProject.ViewModels;

namespace MockerProject.Views
{
    public partial class EditControl : UIControl
    {
        public EditControl()
        {
            InitializeComponent();
            this.DataContext = m_ControlViewModel;
            m_ControlViewModel.IsFitVisible = m_ControlViewModel.IsBorderVisible = false;
            //m_ControlViewModel.ReadOnlyHeight = true;
            setName("TextBox");
            setText("This is TextBox");
            setWidth(200);
            setFontSizeID(3);
            setBackground(new SolidColorBrush(new Color(255,255,255,255)));
            setForeground(new SolidColorBrush(new Color(255, 33, 33, 33)));
            setBorderColor(new SolidColorBrush(new Color(255, 77, 77, 77)));
            setBorderThickness(1);
            edit.AddHandler(TextBox.DoubleTappedEvent, (sender, e) =>
            {
                //m_IsDoubleTapped = true;
                base.doubleClickHandler(sender, e);
            }, handledEventsToo: true);

            var editControl = this.FindControl<TextBox>("edit");
            if (editControl != null)
            {
                editControl.AddHandler(PointerPressedEvent, (sender, e) =>
                {
                    this.RaiseEvent(e);
                }, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
            }

            /*edit.AddHandler(Border.TappedEvent, (sender, e) =>
            {
                //m_MainViewModel.m_nSelectedUIControl = m_nIndex;
            }, handledEventsToo: true);*/
        }

        
        /*private void onMousePressed(object sender, PointerPressedEventArgs e)
        {
            var properties = e.GetCurrentPoint(this).Properties;
            if (properties.IsLeftButtonPressed)
            {
                sX = e.GetPosition(this).X;
                sY = e.GetPosition(this).Y;
                m_MainViewModel.m_nSelectedUIControl = m_nIndex;
            }
            else
            {
                m_MainViewModel.m_nSelectedUIControl = 0;
            }
        }
        private void onMouseReleased(object sender, PointerReleasedEventArgs e)
        {
            m_MainViewModel.m_nSelectedUIControl = 0;
            if (m_IsDoubleTapped)
            {
                m_IsDoubleTapped = false;
                return;
            }
            if (m_MainViewModel.m_wndUIProperty==null || !m_MainViewModel.m_wndUIProperty.IsVisible)
                m_MainViewModel.m_wndUIProperty = new UIPropertyWindow();
            else
            {
                m_MainViewModel.m_wndUIProperty.Close();
                m_MainViewModel.m_wndUIProperty = new UIPropertyWindow();
            }
            Point cP = e.GetPosition(this);
            Point mP = e.GetPosition(m_MainViewModel.m_MainWindow);
            PixelPoint cPP = new PixelPoint((int)(mP.X-cP.X+m_nWidth), (int)(mP.Y-cP.Y));
            PixelPoint nPP = m_MainViewModel.m_MainWindow.Position;
            m_MainViewModel.m_wndUIProperty.Position = nPP+cPP;
            ((UIPropertyWindow)m_MainViewModel.m_wndUIProperty).setMainViewModel(m_MainViewModel);
            ((UIPropertyWindow)m_MainViewModel.m_wndUIProperty).setControlInfo(m_ControlViewModel, this);
            m_MainViewModel.m_wndUIProperty.Show();
        }*/
    }
}
