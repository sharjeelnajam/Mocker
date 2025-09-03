using MockerProject.Models;
using MockerProject.Views;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace MockerProject.ViewModels.UIViewModels
{
    internal class ListBoxViewModel : UIControlViewModel
    {
        public ReactiveCommand<Unit, Unit> AddItems { get; }
        public ReactiveCommand<Unit, Unit> InsertAfter { get; }
        public ReactiveCommand<Unit, Unit> InsertBefore { get; }
        public ListBoxViewModel(UIControl uiControl) : base(uiControl)
        {
            m_UIControl = uiControl;
            CustomItem item = new CustomItem
            {
                text = "Item1",
                Visible = true,
                iteration = "None",
            };
            Items.Add(item);
            AddItems = ReactiveCommand.Create(ExecuteAddItems);
            InsertAfter = ReactiveCommand.Create(InsertAfterItems);
            InsertBefore = ReactiveCommand.Create(InsertBeforeItems);
        }

        private void InsertAfterItems()
        {
            CustomItem item = new CustomItem
            {
                text = "New Item",
                Visible = true,
                iteration = "None",

            };
            Items.Insert(SelectedIndex + 1, item);
            // Handle the click event logic here
        }

        private void InsertBeforeItems()
        {

            CustomItem item = new CustomItem
            {
                text = "New Item",
                Visible = true,
                iteration = "None",

            };
            Items.Insert(SelectedIndex, item);
            // Handle the click event logic here
        }

        private void ExecuteAddItems()
        {
            CustomItem item = new CustomItem
            {
                text = "New Item",
                Visible = true,
                iteration = "None",
            };
            Items.Add(item);
            // Handle the click event logic here
        }

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set { this.RaiseAndSetIfChanged(ref _SelectedIndex, value); }
        }

        public string SelectText
        {
            get 
            { 
                if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
                    return Items[SelectedIndex].text;
                return string.Empty;
            }
        }

        public ObservableCollection<CustomItem> _Items = new ObservableCollection<CustomItem>();
        public ObservableCollection<CustomItem> Items
        {
            get { return _Items; }
            set { this.RaiseAndSetIfChanged(ref _Items, value); }
        }
    }
}