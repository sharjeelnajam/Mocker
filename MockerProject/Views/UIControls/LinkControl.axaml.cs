using Avalonia.Controls;
using Avalonia;
using Avalonia.Input;
using MockerProject.ViewModels;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace MockerProject.Views
{
    public partial class LinkControl : UIControl
    {
        public LinkControl()
        {
            InitializeComponent();
            this.DataContext = m_ControlViewModel;
        }
    }
}
