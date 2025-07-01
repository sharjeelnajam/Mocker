using Avalonia.Controls;
using Avalonia.Media;

namespace MockerProject.Views.UIControls
{
    public partial class ProgressControl : UIControl
    {
        public ProgressControl()
        {
            InitializeComponent();
            this.DataContext = m_ControlViewModel;
            setName("Progress");
            setText("50");
            setSize(150, 20);
            setBackground(new SolidColorBrush(new Color(255, 200, 200, 200)));
            setForeground(new SolidColorBrush(new Color(0, 0, 0, 0)));
            setBorderColor(new SolidColorBrush(new Color(255, 77, 77, 200)));
            m_ControlViewModel.IsBorderVisible = false;
            //m_ControlViewModel.ReadOnlyHeight = true;
            //m_ControlViewModel.IsTextPropertiesVisible = false;
        }
    }
}
