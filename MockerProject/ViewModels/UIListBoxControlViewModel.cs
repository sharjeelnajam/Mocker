using Avalonia.Controls;
using MockerProject.Views;
using MockerProject.Views.UIControls;
using ReactiveUI;

namespace MockerProject.ViewModels
{
    internal class UIListBoxControlViewModel : UIControlViewModel
    {
        public ListBox listBox;
        public string text
        {
            get => m_text;
            set
            {
                this.RaiseAndSetIfChanged(ref m_text, value);
                if (listBox.SelectedIndex >= 0)
                    listBox.Items[listBox.SelectedIndex] = text;
            }
        }//Text

        public UIListBoxControlViewModel(UIControl uiControl) : base(uiControl)
        {
            m_UIControl = uiControl;
            listBox = ((ListBoxControl)m_UIControl).listBox;
        }
    }
}