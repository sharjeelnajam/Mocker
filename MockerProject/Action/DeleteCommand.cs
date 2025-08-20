using Avalonia.Controls;
using MockerProject.ViewModels.UIViewModels;
using MockerProject.Views.UIControls;
using System.Linq;
using Avalonia.VisualTree;
using Avalonia;

namespace MockerProject.Action
{
    internal class DeleteCommand : IAction
    {
        private Panel _canvas;
        private Control _element;
        private int _originalIndex;

        public DeleteCommand(Panel canvas, Control element)
        {
            _canvas = canvas;
            _element = element;
            _originalIndex = canvas.Children.IndexOf(element);
        }

        public void Execute()
        {
            // Handle TabControl deletion
            if (_element is TabViewControl tabControl)
            {
                // Remove the entire TabControl
                _canvas.Children.Remove(_element);
                
                // Also remove any associated run controls
                var runControls = _canvas.Children.OfType<TabViewRunControl>().ToList();
                foreach (var runControl in runControls)
                {
                    _canvas.Children.Remove(runControl);
                }
                return;
            }

            // Handle ListBoxControl deletion
            if (_element is ListBoxControl listBoxControl)
            {
                // Remove the entire ListBoxControl
                _canvas.Children.Remove(_element);
                return;
            }

            // Handle Border elements (inner content) by finding their parent ContainerBoxControl
            if (_element is Border border)
            {
                var containerBoxCtrl = FindAncestor<ContainerBoxControl>(border);
                if (containerBoxCtrl != null)
                {
                    // Check if this container is part of a TabViewControl
                    var tabViewControlCtrl = FindAncestor<TabViewControl>(containerBoxCtrl);
                    if (tabViewControlCtrl != null)
                    {
                        var viewModel = tabViewControlCtrl.DataContext as RepeaterControlViewModel;
                        if (viewModel != null)
                        {
                            // Find the index of the container in the Items collection
                            int itemIndex = viewModel.Items.IndexOf(containerBoxCtrl);
                            if (itemIndex >= 0)
                            {
                                // Remove from Items collection
                                viewModel.Items.RemoveAt(itemIndex);
                                
                                // Remove corresponding header
                                if (itemIndex < viewModel.TabHeaders.Count)
                                {
                                    viewModel.TabHeaders.RemoveAt(itemIndex);
                                }
                                
                                // Remove from parent canvas
                                var pCanvas = containerBoxCtrl.Parent as Canvas;
                                if (pCanvas != null)
                                {
                                    pCanvas.Children.Remove(containerBoxCtrl);
                                }
                                
                                // If this was the last tab, remove the entire TabControl
                                if (viewModel.Items.Count == 0)
                                {
                                    var mainCanvas = tabViewControlCtrl.Parent as Canvas;
                                    if (mainCanvas != null)
                                    {
                                        mainCanvas.Children.Remove(tabViewControlCtrl);
                                        
                                        // Also remove any associated run controls
                                        var runControls = mainCanvas.Children.OfType<TabViewRunControl>().ToList();
                                        foreach (var runControl in runControls)
                                        {
                                            mainCanvas.Children.Remove(runControl);
                                        }
                                    }
                                }
                                return;
                            }
                        }
                    }
                    
                    // If not part of TabViewControl, just remove the container
                    var containerParent = containerBoxCtrl.Parent as Panel;
                    if (containerParent != null)
                    {
                        containerParent.Children.Remove(containerBoxCtrl);
                    }
                    return;
                }
            }

            // Handle individual tab deletion within TabControl
            if (_element is ContainerBoxControl containerBox && 
                containerBox.Parent is Canvas parentCanvas &&
                parentCanvas.Parent is TabViewControl tabViewControl)
            {
                var viewModel = tabViewControl.DataContext as RepeaterControlViewModel;
                if (viewModel != null)
                {
                    // Find the index of the container in the Items collection
                    int itemIndex = viewModel.Items.IndexOf(containerBox);
                    if (itemIndex >= 0)
                    {
                        // Remove from Items collection
                        viewModel.Items.RemoveAt(itemIndex);
                        
                        // Remove corresponding header
                        if (itemIndex < viewModel.TabHeaders.Count)
                        {
                            viewModel.TabHeaders.RemoveAt(itemIndex);
                        }
                        
                        // Remove from parent canvas
                        parentCanvas.Children.Remove(containerBox);
                        
                        // If this was the last tab, remove the entire TabControl
                        if (viewModel.Items.Count == 0)
                        {
                            var mainCanvas = tabViewControl.Parent as Canvas;
                            if (mainCanvas != null)
                            {
                                mainCanvas.Children.Remove(tabViewControl);
                                
                                // Also remove any associated run controls
                                var runControls = mainCanvas.Children.OfType<TabViewRunControl>().ToList();
                                foreach (var runControl in runControls)
                                {
                                    mainCanvas.Children.Remove(runControl);
                                }
                            }
                        }
                        return;
                    }
                }
            }

            // Safe removal that works with any type of parent container
            if (_element.Parent is Panel panel)
            {
                panel.Children.Remove(_element);
                _canvas.Children.Remove(panel);
            }
            else if (_element.Parent is Decorator decorator)
            {
                decorator.Child = null;
                _canvas.Children.Remove(decorator);
            }
            else if (_element.Parent is ContentControl contentControl)
            {
                contentControl.Content = null;
                _canvas.Children.Remove(contentControl);
            }
            else if (_element.Parent != null)
            {
                // Fallback for other types of parents
                var parentProp = _element.Parent.GetType().GetProperty("Children");
                parentProp?.GetValue(_element.Parent)?.GetType()
                    .GetMethod("Remove")?
                    .Invoke(parentProp.GetValue(_element.Parent), new object[] { _element });
            }

            _canvas.Children.Remove(_element);

            if (_element.DataContext is TreeViewViewModel treeVm)
            {
                var runControl = _canvas.Children
                  .OfType<TreeViewControl>()
                  .FirstOrDefault(c => c.DataContext == treeVm);

                if (runControl != null)
                {
                    _canvas.Children.Remove(runControl);
                }
            }
        }

        private T FindAncestor<T>(Visual visual) where T : Visual
        {
            while (visual != null)
            {
                if (visual is T result)
                    return result;
                visual = visual.GetVisualParent();
            }
            return null;
        }

        public void UnExecute()
        {
            if (_originalIndex >= 0 && _originalIndex <= _canvas.Children.Count)
            {
                _canvas.Children.Insert(_originalIndex, _element);
            }
            else
            {
                _canvas.Children.Add(_element);
            }
        }
    }
}
