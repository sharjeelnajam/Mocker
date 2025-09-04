using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using System.Diagnostics.CodeAnalysis;

namespace MockerProject.Views.UIControls
{
    public partial class ContainerBoxControl : UIControl
    {
        public TappedEventArgs t_event;
        public object t_sender;
        [AllowNull] public UIPropertyWindow wind;
        public ContainerBoxControl()
        {
            InitializeComponent();
            //m_ControlViewModel = new UIControlViewModel(this);
            this.DataContext = m_ControlViewModel;
            setSize(300, 200);
            setName("ContainerBox");
            setBorderThickness(1);
            setBorderRound(0);
            setBackground(new SolidColorBrush(new Color(255, 255, 255, 255)));
            setForeground(new SolidColorBrush(new Color(255, 255, 255, 255)));
            setBorderColor(new SolidColorBrush(new Color(255, 200, 155, 15)));
            wind = null;
            
            m_ControlViewModel.IsTextPropertiesVisible = false;
            m_ControlViewModel.IsMultiEnable = false;
        }

        public override void doubleClickHandler(object sender, TappedEventArgs e)
        {
            t_event = e;
            t_sender = sender;
            if (closeBtn.IsVisible)
            {
                //base.doubleClickHandler(sender, e);
            } 
            else 
            {
                closeBtn.IsVisible = true;
                editBtn.IsVisible = true;
                Canvas.SetLeft(closeBtn, -closeBtn.Width);
                Canvas.SetLeft(editBtn, closeBtn.Width);
                double left = Canvas.GetLeft(this);
                double top = Canvas.GetTop(this);
                System.Type type = this.Parent.GetType();

                this.Width = 2000;// ((Canvas)this.Parent).Width;
                this.Height = 2000;// ((Canvas)this.Parent).Height;
                backCanvas.Width = 2000;//((Canvas)this.Parent).Width;
                backCanvas.Height = 2000;//((Canvas)this.Parent).Height;
                int width = (int) container.Width;
                int height = (int)container.Height;

                Canvas.SetLeft(this, 0);
                Canvas.SetTop(this, 0);

                backCanvas.Background = new SolidColorBrush(new Color(255, 100, 100, 100));

                Canvas.SetLeft(border, left);
                Canvas.SetTop(border, top);
                
                if (((ContainerBoxControl)sender).closeBtn.IsVisible)
                {
                    m_ControlViewModel.m_MainVM.ContainerFlag++;
                    m_ControlViewModel.m_MainVM.ContainerCanvas.Add(container);
                }
            }
        }

        public override void KeyPressEvent(object sender, KeyEventArgs e)
        {
        }

        public override void MousePressEvent(object sender, PointerEventArgs e)
        {
            if (!closeBtn.IsVisible)
            {
                base.MousePressEvent(sender, e);
            }
            
        }

        public override void MouseMoveEvent(object sender, PointerEventArgs e)
        {
            if (!closeBtn.IsVisible)
            {
                base.MouseMoveEvent(sender, e);
            }
        }

        public void Click_CloseButton(object sender, RoutedEventArgs e)
        {
            closeBtn.IsVisible = false;
            editBtn.IsVisible = false;
            m_ControlViewModel.m_MainVM.ContainerFlag -=1;
            m_ControlViewModel.m_MainVM.ContainerCanvas.RemoveAt(m_ControlViewModel.m_MainVM.ContainerFlag);

            double left = Canvas.GetLeft(border);
            double top = Canvas.GetTop(border);
            this.Width = container.Width;
            this.Height = container.Height;
            backCanvas.Width = container.Width;
            backCanvas.Height = container.Height;

            backCanvas.Background = new SolidColorBrush(new Color(0, 100, 100, 100));

            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
            Canvas.SetLeft(border, 0);
            Canvas.SetTop(border, 0);
        }

        public void Click_EditButton(object sender, RoutedEventArgs e)
        {
            var ttv = this.TransformToVisual(m_ControlViewModel.m_MainVM.WorkScreen.screenCanvas);
            Point screenCoords = (new Point(0, 0)).Transform((Matrix)ttv);
            PixelPoint cPP = new PixelPoint((int)(screenCoords.X + m_nWidth), (int)(screenCoords.Y));
            PixelPoint nPP = m_MainViewModel.m_MainWindow.Position;

            if (wind != null)
            {
                wind.Close();
            }
            wind = new UIPropertyWindow();

            wind.DataContext = m_ControlViewModel;
            wind.setMainViewModel(m_MainViewModel);
            wind.setControlInfo(m_ControlViewModel, this);
            wind.Position = nPP + cPP;
            wind.Show();

        }
    }
}