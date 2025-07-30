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

        private Node _SelectedItem;
        public Node SelectedItem
        {
            get { return _SelectedItem; }
            set { this.RaiseAndSetIfChanged(ref _SelectedItem, value); }
        }

        public TreeViewViewModel(UIControl uiControl) : base(uiControl)
        {
            m_UIControl = uiControl;
            Items = new ObservableCollection<Node>();
            Items.Add(rootNode);
            AddItems = ReactiveCommand.Create(ExecuteAddItems);
            InsertAfter = ReactiveCommand.Create(InsertAfterItems);
            InsertBefore = ReactiveCommand.Create(InsertBeforeItems);
        }

        private void InsertAfterItems()
        {
            int index = Items.IndexOf(SelectedItem);
            Node newNode = new Node("Item" + Items.Count, SelectedItem.parent);
            newNode.iteration = "None";
            Items.Insert(index+1, newNode);
            // Handle the click event logic here
        }

        private void InsertBeforeItems()
        {
            int index = Items.IndexOf(SelectedItem);
            Node newNode = new Node("Item" + Items.Count, SelectedItem.parent);
            newNode.iteration = "None";
            Items.Insert(index, newNode);
            // Handle the click event logic here
        }

        private void ExecuteAddItems()
        {
            Node newNode = new Node("Item" + Items.Count, null);
            newNode.iteration = "None";
            Items.Add( newNode);
            // Handle the click event logic here
        }
    }
}