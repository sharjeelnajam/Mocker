using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using MockerProject.Models;
using MockerProject.ViewModels.UIViewModels;
using MockerProject.Views.UIProperties;
using System.Diagnostics.CodeAnalysis;
using static MockerProject.ViewModels.UIControlViewModel;

namespace MockerProject.Views.UIControls
{
    public partial class TreeViewControl : UIControl
    {
        [AllowNull] public UITreeViewProperty wind;
        public TreeViewControl()
        {
            InitializeComponent();
            m_ControlViewModel = new TreeViewViewModel(this);
            this.DataContext = m_ControlViewModel;
            setName("TreeView");
            IterationItem item = new IterationItem
            {
                text = "Selects",
                type = EventType.EVENT_SELECTITEM,
                iteration = "None",
            };
            m_ControlViewModel.iterationItems.Insert(0, item);
            setWidth(300);
            setHeight(400);
            setBackground(new SolidColorBrush(new Color(0,0 , 200, 200)));
            setForeground(new SolidColorBrush(new Color(255, 33, 33, 33)));
            setBorderColor(new SolidColorBrush(new Color(255, 77, 77, 77)));
            setBorderThickness(1);
            setBorderRound(5);
            wind = null;
            this.AddHandler(Control.KeyDownEvent, (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    Node item = (Node)((Control)e.Source).DataContext;
                    item.Visible = !item.Visible;
                }
                if (e.Key == Key.Right && e.KeyModifiers == KeyModifiers.Shift)
                {
                    Node item = (Node)((Control)e.Source).DataContext;
                    if(item.parent != null)
                    {
                        int index = item.parent.SubItems.IndexOf(item);
                        if (index > 0)
                        {
                            Node selItem = item.parent.SubItems[index - 1];
                            item.parent.SubItems.Remove(item);
                            item.parent = selItem;
                            selItem.addSubItem(item);
                            var treeViewItem = (TreeViewItem)treeView.ItemContainerGenerator.Index.ContainerFromItem(selItem);
                            treeView.ExpandSubTree(treeViewItem);
                        }
                    }
                    else
                    {
                        int index = ((TreeViewViewModel)m_ControlViewModel).Items.IndexOf(item);
                        if(index > 0)
                        {
                            Node selItem = ((TreeViewViewModel)m_ControlViewModel).Items[index - 1];
                            ((TreeViewViewModel)m_ControlViewModel).Items.Remove(item);
                            item.parent = selItem;
                            selItem.addSubItem(item);
                            var treeViewItem = (TreeViewItem)treeView.ItemContainerGenerator.Index.ContainerFromItem(selItem);
                            treeView.ExpandSubTree(treeViewItem);
                        }
                    }
                }
                if (e.Key == Key.Left && e.KeyModifiers == KeyModifiers.Shift)
                {
                    Node item = (Node)((Control)e.Source).DataContext;
                    if (item.parent != null)
                    {
                        Node selItem = item.parent;

                        selItem.SubItems.Remove(item);
                        
                        if(selItem.parent != null)
                        {
                            int index = selItem.parent.SubItems.IndexOf(selItem);
                            item.parent = selItem.parent;
                            selItem.parent.SubItems.Insert(index + 1, item);
                            var treeViewItem = (TreeViewItem)treeView.ItemContainerGenerator.Index.ContainerFromItem(selItem);
                            treeView.ExpandSubTree(treeViewItem);
                        }
                        else
                        {
                            int index = ((TreeViewViewModel)m_ControlViewModel).Items.IndexOf(selItem);
                            ((TreeViewViewModel)m_ControlViewModel).Items.Insert(index+1,item);
                            item.parent = selItem.parent;
                            var treeViewItem = (TreeViewItem)treeView.ItemContainerGenerator.Index.ContainerFromItem(selItem);
                            treeView.ExpandSubTree(treeViewItem);
                        }
                    }
                }
            }, handledEventsToo: true);
        }

        private TreeViewItem GetParentTreeViewItem(TreeViewItem treeViewItem)
        {
            var parent = treeViewItem.GetVisualParent<TreeViewItem>();
            return parent;
        }

        private void TreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public override void doubleClickHandler(object sender, TappedEventArgs e)
        {
            Point cP = e.GetPosition(this);
            Point mP = e.GetPosition(m_MainViewModel.m_MainWindow);
            PixelPoint cPP = new PixelPoint((int)(mP.X - cP.X + m_nWidth), (int)(mP.Y - cP.Y));
            PixelPoint nPP = m_MainViewModel.m_MainWindow.Position;

            if (wind != null)
            {
                wind.Close();
            }
            wind = new UITreeViewProperty();

            wind.setModel((TreeViewViewModel)m_ControlViewModel, this);
            wind.Position = nPP + cPP;
            wind.Show();
        }
    }
}