using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using MockerProject.Models;

namespace MockerProject.Views.UIControls
{
    public partial class SliderControl : UIControl
    {
        public SliderControl()
        {
            InitializeComponent();
            this.DataContext = m_ControlViewModel;
            setType(CONTROL_TYPE.SLIDER);
            setName("Slider");
            setText("50.00");
            m_ControlViewModel.sliderValue = 50.0;
            setSize(100, 50);
            setBackground(new SolidColorBrush(new Color(255, 0, 0, 255)));
            setForeground(new SolidColorBrush(new Color(0, 0, 0, 0)));
            setBorderColor(new SolidColorBrush(new Color(255, 0, 0, 255)));
            //m_ControlViewModel.IsBorderColorVisible = false;
            m_ControlViewModel.IsBorderVisible = false;
            m_ControlViewModel.ReadOnlyHeight = true;
            m_ControlViewModel.IsTextPropertiesVisible = false;

            this.AddHandler(InputElement.PointerPressedEvent, OnSliderPressed, RoutingStrategies.Tunnel);
        }

        private void OnSliderPressed(object? sender, PointerPressedEventArgs e)
        {
            e.Handled = false;
        }
    }
}
