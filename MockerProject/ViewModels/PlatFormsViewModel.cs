using MockerProject.Views;
using ReactiveUI;

namespace MockerProject.ViewModels
{
    public  class PlatFormsViewModel : ReactiveObject
    {
        public ScreenView m_WorkScreen = null;
        public string m_WorkText = "Hello world!";
        public string WorkText
        {  
            get=>m_WorkText;
            set=>this.RaiseAndSetIfChanged(ref m_WorkText,value); 
        }
        public ScreenView WorkScreen
        {
            get => m_WorkScreen;
            set => this.RaiseAndSetIfChanged(ref m_WorkScreen, value);
        }
    }
}