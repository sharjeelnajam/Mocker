using MockerProject.Models;
using MockerProject.Views;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace MockerProject.ViewModels.UIViewModels
{
    internal class TreeViewViewModel : UIControlViewModel
    {
        public ObservableCollection<Node> Items { get; set; }
        Node rootNode = new Node("Root", null);

        public ObservableCollection<Node> SelectedItems { get; }
        public string strFolder { get; set; }
        public ReactiveCommand<Unit, Unit> AddItems { get; }
        public ReactiveCommand<Unit, Unit> InsertAfter { get; }
        public ReactiveCommand<Unit, Unit> InsertBefore { get; }
        public ReactiveCommand<Unit, Unit> AddSubItem { get; }
        public ReactiveCommand<Unit, Unit> RemoveItem { get; }

        private Node _SelectedItem;
        public Node SelectedItem
        {
            get { return _SelectedItem; }
            set { this.RaiseAndSetIfChanged(ref _SelectedItem, value); }
        }

        // Override property to control Remove button visibility - only show when there are items other than root
        public override bool CanRemoveItem => Items.Count > 1;

        public TreeViewViewModel(UIControl uiControl) : base(uiControl)
        {
            m_UIControl = uiControl;
            Items = new ObservableCollection<Node>();
            Items.Add(rootNode);
            AddItems = ReactiveCommand.Create(ExecuteAddItems);
            InsertAfter = ReactiveCommand.Create(InsertAfterItems);
            InsertBefore = ReactiveCommand.Create(InsertBeforeItems);
            AddSubItem = ReactiveCommand.Create(ExecuteAddSubItem);
            RemoveItem = ReactiveCommand.Create(ExecuteRemoveItem);
        }

        private void InsertAfterItems()
        {
            int index = Items.IndexOf(SelectedItem);
            Node newNode = new Node("Item" + Items.Count, SelectedItem.parent);
            newNode.iteration = "None";
            Items.Insert(index+1, newNode);
            
            // Raise CanRemoveItem property change
            this.RaisePropertyChanged(nameof(CanRemoveItem));
            // Handle the click event logic here
        }

        private void InsertBeforeItems()
        {
            int index = Items.IndexOf(SelectedItem);
            Node newNode = new Node("Item" + Items.Count, SelectedItem.parent);
            newNode.iteration = "None";
            Items.Insert(index, newNode);
            
            // Raise CanRemoveItem property change
            this.RaisePropertyChanged(nameof(CanRemoveItem));
            // Handle the click event logic here
        }

        private void ExecuteAddItems()
        {
            Node newNode = new Node("Item" + Items.Count, null);
            newNode.iteration = "None";
            Items.Add( newNode);
            
            // Raise CanRemoveItem property change
            this.RaisePropertyChanged(nameof(CanRemoveItem));
            // Handle the click event logic here
        }

        private void ExecuteAddSubItem()
        {
            if (SelectedItem != null)
            {
                Node newNode = new Node("SubItem" + (SelectedItem.SubItems.Count + 1), SelectedItem);
                newNode.iteration = "None";
                SelectedItem.addSubItem(newNode);
            }
        }

        private void ExecuteRemoveItem()
        {
            if (SelectedItem != null && SelectedItem != rootNode)
            {
                // If the selected item has a parent, remove it from parent's SubItems
                if (SelectedItem.parent != null)
                {
                    SelectedItem.parent.SubItems.Remove(SelectedItem);
                }
                else
                {
                    // If it's a root level item, remove it from Items collection
                    Items.Remove(SelectedItem);
                }
                
                // Clear the selected item
                SelectedItem = null;
                
                // Raise CanRemoveItem property change
                this.RaisePropertyChanged(nameof(CanRemoveItem));
            }
        }

        public void RemoveSpecificItem(Node itemToRemove)
        {
            if (itemToRemove != null && itemToRemove != rootNode)
            {
                // If the item has a parent, remove it from parent's SubItems
                if (itemToRemove.parent != null)
                {
                    itemToRemove.parent.SubItems.Remove(itemToRemove);
                }
                else
                {
                    // If it's a root level item, remove it from Items collection
                    Items.Remove(itemToRemove);
                }
                
                // Clear the selected item if it was the one being removed
                if (SelectedItem == itemToRemove)
                {
                    SelectedItem = null;
                }
                
                // Raise CanRemoveItem property change
                this.RaisePropertyChanged(nameof(CanRemoveItem));
            }
        }
    }
}