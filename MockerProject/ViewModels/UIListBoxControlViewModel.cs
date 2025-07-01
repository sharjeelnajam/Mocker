using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using MockerProject.Views;
using MockerProject.Views.UIControls;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using MockerProject.Models;


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
