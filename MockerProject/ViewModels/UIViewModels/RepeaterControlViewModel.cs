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
        public ReactiveCommand<Unit, Unit> RemoveTab { get; }
        public ReactiveCommand<string, Unit> RenameTab { get; }

        public RepeaterControlViewModel(UIControl uiControl) : base(uiControl)
        {
            m_UIControl = uiControl;
            AddItems = ReactiveCommand.Create(ExecuteAddItems);
            InsertAfter = ReactiveCommand.Create(InsertAfterItems);
            InsertBefore = ReactiveCommand.Create(InsertBeforeItems);
            RemoveTab = ReactiveCommand.Create(ExecuteRemoveTab);
            RenameTab = ReactiveCommand.Create<string>(ExecuteRenameTab);
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

        private void ExecuteRemoveTab()
        {
            if (Items.Count > 1 && SelectedTabIndex >= 0 && SelectedTabIndex < Items.Count)
            {
                // Remove the selected tab
                Items.RemoveAt(SelectedTabIndex);
                TabHeaders.RemoveAt(SelectedTabIndex);
                
                // Adjust selected index if necessary
                if (SelectedTabIndex >= Items.Count)
                {
                    SelectedTabIndex = Items.Count - 1;
                }
            }
        }

        private void ExecuteRenameTab(string newText)
        {
            if (SelectedTabIndex >= 0 && SelectedTabIndex < TabHeaders.Count && !string.IsNullOrEmpty(newText))
            {
                TabHeaders[SelectedTabIndex] = newText;
                SelectedTabText = newText;
            }
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

        private int _SelectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get { return _SelectedTabIndex; }
            set { 
                this.RaiseAndSetIfChanged(ref _SelectedTabIndex, value);
                // Update selected tab text when index changes
                if (value >= 0 && value < TabHeaders.Count)
                {
                    SelectedTabText = TabHeaders[value];
                    // Also update the base text property to sync with the UI
                    text = TabHeaders[value];
                }
            }
        }

        private string _SelectedTabText = "";
        public string SelectedTabText
        {
            get { return _SelectedTabText; }
            set { this.RaiseAndSetIfChanged(ref _SelectedTabText, value); }
        }

        // Handle text changes for tab renaming
        public void UpdateTabText(string newText)
        {
            if (SelectedTabIndex >= 0 && SelectedTabIndex < TabHeaders.Count)
            {
                TabHeaders[SelectedTabIndex] = newText ?? "";
                SelectedTabText = newText ?? "";
                // Keep the base text property in sync
                text = newText ?? "";
            }
        }

        // Add a new tab with specific text
        public void AddNewTabWithText(string tabText)
        {
            if (!string.IsNullOrEmpty(tabText))
            {
                TabHeaders.Add(tabText);
                ContainerBoxControl containerBoxControl = new ContainerBoxControl();
                containerBoxControl.setMainVM(this.m_MainVM);
                containerBoxControl.m_nUIControlType = Models.CONTROL_TYPE.CONTAINERBOX;
                Items.Add(containerBoxControl);
                
                // Select the newly created tab
                SelectedTabIndex = Items.Count - 1;
                SelectedTabText = tabText;
                text = tabText;
            }
        }
    }
}