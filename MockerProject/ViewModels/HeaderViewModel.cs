using ReactiveUI;
using System.Windows.Input;


namespace MockerProject.ViewModels
{
    public  class HeaderViewModel : ViewModelBase
    {
        public HeaderViewModel()
        {
            onMenuOpen = ReactiveCommand.Create(() =>
            {
                var window = new RunWindow();
                window.Show();
                // Code here will be executed when the button is clicked.
                IsMenuOpened = true;
            });
        }

        public ICommand onMenuOpen { get; }
        public bool w_IsMenuOpenState = false;
        public bool IsMenuOpened
        {
            get => w_IsMenuOpenState;
            set => this.RaiseAndSetIfChanged(ref w_IsMenuOpenState, value);
        }
    }
}