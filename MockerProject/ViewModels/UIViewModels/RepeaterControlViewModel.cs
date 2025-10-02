using Avalonia.Media;
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

        // Override background property to update tab content background
        public new SolidColorBrush background
        {
            get => base.background;
            set 
            { 
                base.background = value;
                // Update tab content background when background changes
                if (m_UIControl is TabViewControl tabControl)
                {
                    tabControl.UpdateTabContentBackground(value);
                }
            }
        }

        public void InsertAfterItems()
        {
            System.Diagnostics.Debug.WriteLine("InsertAfterItems called!");
            // Insert a new tab after the currently selected tab
            int insertIndex = SelectedTabIndex + 1;
            if (insertIndex > Items.Count) insertIndex = Items.Count;
            
            TabHeaders.Insert(insertIndex, "Item " + (Items.Count + 1));
            ContainerBoxControl containerBoxControl = new ContainerBoxControl();
            containerBoxControl.setMainVM(this.m_MainVM);
            containerBoxControl.m_nUIControlType = Models.CONTROL_TYPE.CONTAINERBOX;
            Items.Insert(insertIndex, containerBoxControl);
            
            // Select the newly created tab
            SelectedTabIndex = insertIndex;
        }

        public void InsertBeforeItems()
        {
            System.Diagnostics.Debug.WriteLine("InsertBeforeItems called!");
            // Insert a new tab after the currently selected tab (same as InsertAfter)
            int insertIndex = SelectedTabIndex + 1;
            if (insertIndex > Items.Count) insertIndex = Items.Count;
            
            TabHeaders.Insert(insertIndex, "Item " + (Items.Count + 1));
            ContainerBoxControl containerBoxControl = new ContainerBoxControl();
            containerBoxControl.setMainVM(this.m_MainVM);
            containerBoxControl.m_nUIControlType = Models.CONTROL_TYPE.CONTAINERBOX;
            Items.Insert(insertIndex, containerBoxControl);
            
            // Select the newly created tab
            SelectedTabIndex = insertIndex;
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