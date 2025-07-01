using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using MockerProject.ViewModels;

namespace MockerProject.Views
{
    public partial class RadioControl : UIControl
    {
        public RadioControl()
        {
            InitializeComponent();
            this.DataContext = m_ControlViewModel;
            setSize(100, 30);
            setName("Radio");
            setFontSize(14);
            setText("Option");
            setForeground(new SolidColorBrush(new Color(255, 0, 255, 255)));
            setFontSizeID(7);
            m_ControlViewModel.IsBorderVisible = false;
            m_ControlViewModel.IsBackgroundVisible = false;
            m_ControlViewModel.IsBorderColorVisible = false;
        }
    }
}
