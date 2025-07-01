using Avalonia.Controls;
using MockerProject.ViewModels.UIViewModels;
using MockerProject.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace MockerProject.Views.UIProperties
{
    public partial class UIContainerBoxProperty : Window
    {
        [AllowNull] private UIControl m_UIControl;
        [AllowNull] private ListBoxViewModel m_ControlModel;
        public UIContainerBoxProperty()
        {
            InitializeComponent();

        }
        //set model
        public void setModel(UIControlViewModel model, UIControl uIControl)
        {

            ListBoxViewModel listBoxViewModel = (ListBoxViewModel)uIControl.DataContext;
            m_UIControl = uIControl;
            m_ControlModel = (ListBoxViewModel)uIControl.DataContext;

            this.DataContext = m_ControlModel;
            BaseProperty.DataContext = m_ControlModel;
            BaseProperty.setModel((UIControlViewModel)m_ControlModel, m_UIControl);

            //Event select Page
          

        }


    }
}
