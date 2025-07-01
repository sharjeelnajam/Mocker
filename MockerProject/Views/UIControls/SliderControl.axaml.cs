using Avalonia.Controls;
using Avalonia.Media;
using MockerProject.ViewModels;

namespace MockerProject.Views.UIControls
{
    public partial class SliderControl : UIControl
    {
        public SliderControl()
        {
            InitializeComponent();
            this.DataContext = m_ControlViewModel;
            setName("Slider");
            setText("50");
            setSize(100, 50);
            setBackground(new SolidColorBrush(new Color(255, 0, 0, 255)));
            setForeground(new SolidColorBrush(new Color(0, 0, 0, 0)));
            setBorderColor(new SolidColorBrush(new Color(255, 0, 0, 255)));
            //m_ControlViewModel.IsBorderColorVisible = false;
            m_ControlViewModel.IsBorderVisible = false;
            m_ControlViewModel.ReadOnlyHeight = true;
            m_ControlViewModel.IsTextPropertiesVisible = false;
        }
    }
}
