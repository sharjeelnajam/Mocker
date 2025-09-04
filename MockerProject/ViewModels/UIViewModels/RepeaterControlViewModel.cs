using MockerProject.Views;
using MockerProject.Views.UIControls;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace MockerProject.ViewModels.UIViewModels
{
    internal class RepeaterControlViewModel : UIControlViewModel
    {
        public ReactiveCommand<Unit, Unit> AddItems { get; }
        public ReactiveCommand<Unit, Unit> InsertAfter { get; }
        public ReactiveCommand<Unit, Unit> InsertBefore { get; }

        public RepeaterControlViewModel(UIControl uiControl) : base(uiControl)
        {
            m_UIControl = uiControl;
            AddItems = ReactiveCommand.Create(ExecuteAddItems);
            InsertAfter = ReactiveCommand.Create(InsertAfterItems);
            InsertBefore = ReactiveCommand.Create(InsertBeforeItems);
            ExecuteAddItems();
        }

        private void InsertAfterItems()
        {
            // Handle the click event logic here
        }

        private void InsertBeforeItems()
        {
            // Handle the click event logic here
        }

        private void ExecuteAddItems()
        {
            TabHeaders.Add("Item " + (Items.Count + 1));
            ContainerBoxControl containerBoxControl = new ContainerBoxControl();
            containerBoxControl.setMainVM(this.m_MainVM);
            containerBoxControl.m_nUIControlType = Models.CONTROL_TYPE.CONTAINERBOX;
            Items.Add(containerBoxControl);
        }

        public ObservableCollection<ContainerBoxControl> _Items = new ObservableCollection<ContainerBoxControl>();
        public ObservableCollection<ContainerBoxControl> Items
        {
            get { return _Items; }
            set { this.RaiseAndSetIfChanged(ref _Items, value); }
        }

        public ObservableCollection<string> _TabHeaders = new ObservableCollection<string>();
        public ObservableCollection<string> TabHeaders
        {
            get { return _TabHeaders; }
            set { this.RaiseAndSetIfChanged(ref _TabHeaders, value); }
        }
    }
}