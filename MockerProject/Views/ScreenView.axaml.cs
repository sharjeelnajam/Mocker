using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using MockerProject.Action;
using MockerProject.Models;
//using Avalonia.Remote.Protocol.Input;
using MockerProject.ViewModels;
using MockerProject.Views.UIControls;
using System;
using System.Collections.Generic;
using System.Linq;
using Size = System.Drawing.Size;

namespace MockerProject.Views
{
    public struct stControlHistory
    {
        public int Index;
        public string Cmd;

        public CONTROL_TYPE id;
        public Type type;
        public Control oldInfo;
        public Control curInfo;
    }

    /// <summary>
    /// ScreenView provides the main design canvas for UI controls with multi-selection support.
    /// 
    /// Keyboard Shortcuts:
    /// - Ctrl+A: Select all controls
    /// - Ctrl+C: Copy selected controls (placeholder)
    /// - Ctrl+V: Paste controls (placeholder)
    /// - Delete: Delete selected controls
    /// - Escape: Clear selection
    /// - Ctrl+Z: Undo
    /// - Ctrl+Y: Redo
    /// 
    /// Mouse Selection:
    /// - Click: Select single control
    /// - Ctrl+Click: Add/remove control from selection
    /// - Shift+Click: Select range between last selected and clicked control
    /// - Click empty space: Clear selection (unless Ctrl is held)
    /// </summary>
    public partial class ScreenView : UserControl
    {
        private MainWindowViewModel m_MainViewModel;
        public List<stControlHistory> m_UndoList = new List<stControlHistory>();
        public List<stControlHistory> m_RedoList = new List<stControlHistory>();
        public string m_strName = "Page1";
        public bool m_Orientation = true;
        public Size m_Size = new Size(375, 647);
        public SolidColorBrush m_background = new SolidColorBrush(new Color(255, 255, 255, 255));
        public double m_Opacity = 0.33;
        
        // Multi-selection support
        private List<Control> selectedElements = new List<Control>();
        private Control? lastSelectedElement;
        private bool isMultiSelectMode = false;
        private bool isDragging = false;
        private Point dragStartPoint;
        
        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

        public ScreenView()
        {
            InitializeComponent();
            //this.PointerMoved += onMouseMove;
            this.AttachedToVisualTree += (_, _) =>
            {
                this.Focus(); // or screenCanvas.Focus();
            };
            this.AddHandler(KeyUpEvent, OnKeyDown, RoutingStrategies.Tunnel);
        }

        public void UpdateSelectionHighlight(Control? element)
        {
            // Clear previous selection
            ClearAllSelections();

            if (element != null)
            {
                selectedElements.Add(element);
                lastSelectedElement = element;
                HighlightControl(element);
                Console.WriteLine($"Selected: {element.GetType().Name}");
                UpdateSelectionStatus();
            }
        }

        public void AddToSelection(Control element)
        {
            if (element != null && !selectedElements.Contains(element))
            {
                selectedElements.Add(element);
                lastSelectedElement = element;
                HighlightControl(element);
                Console.WriteLine($"Added to selection: {element.GetType().Name}");
                UpdateSelectionStatus();
            }
        }

        public void RemoveFromSelection(Control element)
        {
            if (element != null && selectedElements.Contains(element))
            {
                selectedElements.Remove(element);
                UnhighlightControl(element);
                if (lastSelectedElement == element)
                {
                    lastSelectedElement = selectedElements.Count > 0 ? selectedElements.Last() : null;
                }
                Console.WriteLine($"Removed from selection: {element.GetType().Name}");
                UpdateSelectionStatus();
            }
        }

        public void SelectAllControls()
        {
            var canvas = this.FindControl<Canvas>("screenCanvas");
            if (canvas != null)
            {
                ClearAllSelections();
                
                foreach (var child in canvas.Children)
                {
                    if (child is Control control && IsSelectableControl(control))
                    {
                        selectedElements.Add(control);
                        HighlightControl(control);
                    }
                }
                
                if (selectedElements.Count > 0)
                {
                    lastSelectedElement = selectedElements.Last();
                    Console.WriteLine($"Selected all {selectedElements.Count} controls");
                    UpdateSelectionStatus();
                }
            }
        }

        public void SelectRange(Control startControl, Control endControl)
        {
            var canvas = this.FindControl<Canvas>("screenCanvas");
            if (canvas == null) return;

            ClearAllSelections();
            
            bool inRange = false;
            bool foundStart = false;
            bool foundEnd = false;
            
            foreach (var child in canvas.Children)
            {
                if (child is Control control && IsSelectableControl(control))
                {
                    if (control == startControl)
                    {
                        foundStart = true;
                        inRange = true;
                    }
                    else if (control == endControl)
                    {
                        foundEnd = true;
                        inRange = true;
                    }
                    
                    if (inRange)
                    {
                        selectedElements.Add(control);
                        HighlightControl(control);
                    }
                    
                    // If we've found both start and end, we're done
                    if (foundStart && foundEnd)
                    {
                        break;
                    }
                }
            }
            
            if (selectedElements.Count > 0)
            {
                lastSelectedElement = endControl;
                Console.WriteLine($"Range selected: {selectedElements.Count} controls");
                UpdateSelectionStatus();
            }
        }

        public void ClearSelection()
        {
            ClearAllSelections();
            UpdateSelectionStatus();
        }

        public List<Control> GetSelectedElements()
        {
            return new List<Control>(selectedElements);
        }

        public int GetSelectedCount()
        {
            return selectedElements.Count;
        }

        private void UpdateSelectionStatus()
        {
            if (selectedElements.Count > 1)
            {
                Console.WriteLine($"Multiple selection: {selectedElements.Count} controls selected");
            }
            else if (selectedElements.Count == 1)
            {
                Console.WriteLine($"Single selection: {selectedElements[0].GetType().Name} selected");
            }
            else
            {
                Console.WriteLine("No controls selected");
            }
        }

        private void ClearAllSelections()
        {
            foreach (var element in selectedElements)
            {
                UnhighlightControl(element);
            }
            selectedElements.Clear();
            lastSelectedElement = null;
        }

        private void HighlightControl(Control control)
        {
            control.SetValue(Border.BorderBrushProperty, Brushes.Blue);
            control.SetValue(Border.BorderThicknessProperty, new Thickness(2));
        }

        private void UnhighlightControl(Control control)
        {
            control.ClearValue(Border.BorderBrushProperty);
            control.ClearValue(Border.BorderThicknessProperty);
        }

        private bool IsSelectableControl(Control control)
        {
            return control is ButtonControl || control is CheckControl || control is EditControl ||
                   control is RadioControl || control is TabViewControl || control is UIControl ||
                   control is RepeaterControl || control is ListBoxControl || control is SliderControl ||
                   control is ImageControl || control is ContainerBoxControl || control is TreeViewControl ||
                   control is LabelControl || control is LinkControl || control is ProgressControl ||
                   control is DropDownControl;
        }

        // Test method to verify selection is working
        public void TestSelection()
        {
            var canvas = this.FindControl<Canvas>("screenCanvas");
            if (canvas != null && canvas.Children.Count > 0)
            {
                // Try to select the first child
                var firstChild = canvas.Children[0];
                if (firstChild is Control control)
                {
                    UpdateSelectionHighlight(control);
                }
            }
        }

        // Debug method to help identify issues
        public void DebugSelection()
        {
            var canvas = this.FindControl<Canvas>("screenCanvas");
            if (canvas != null)
            {
                Console.WriteLine($"Canvas found with {canvas.Children.Count} children");
                for (int i = 0; i < canvas.Children.Count; i++)
                {
                    var child = canvas.Children[i];
                    Console.WriteLine($"Child {i}: {child.GetType().Name}");
                }
            }
            else
            {
                Console.WriteLine("Canvas not found!");
            }
        }

        // Test multi-selection functionality
        public void TestMultiSelection()
        {
            var canvas = this.FindControl<Canvas>("screenCanvas");
            if (canvas != null && canvas.Children.Count >= 2)
            {
                Console.WriteLine("Testing multi-selection...");
                
                // Clear any existing selections
                ClearAllSelections();
                
                // Select the first two controls
                var firstControl = canvas.Children[0] as Control;
                var secondControl = canvas.Children[1] as Control;
                
                if (firstControl != null && IsSelectableControl(firstControl))
                {
                    AddToSelection(firstControl);
                    Console.WriteLine($"Added first control: {firstControl.GetType().Name}");
                }
                
                if (secondControl != null && IsSelectableControl(secondControl))
                {
                    AddToSelection(secondControl);
                    Console.WriteLine($"Added second control: {secondControl.GetType().Name}");
                }
                
                Console.WriteLine($"Multi-selection test complete. Selected: {selectedElements.Count} controls");
            }
            else
            {
                Console.WriteLine("Cannot test multi-selection - need at least 2 controls on canvas");
            }
        }

        // Simulate Ctrl+Click for testing
        public void SimulateCtrlClick(Control control)
        {
            Console.WriteLine($"Simulating Ctrl+Click on {control.GetType().Name}");
            
            if (selectedElements.Contains(control))
            {
                // Remove from selection if already selected
                Console.WriteLine("Removing from selection");
                RemoveFromSelection(control);
            }
            else
            {
                // Add to selection
                Console.WriteLine("Adding to selection");
                AddToSelection(control);
            }
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && selectedElements.Count > 0)
            {
                var canvas = this.FindControl<Canvas>("screenCanvas");
                if (canvas != null)
                {
                    // Delete all selected elements
                    foreach (var element in selectedElements.ToList())
                    {
                        var deleteAction = new DeleteCommand(canvas, element);
                        ViewModel.ExecuteAction(deleteAction);
                    }
                    ClearSelection();
                }
            }
            else if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.Z)
            {
                ViewModel.Undo();
            }
            else if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.Y)
            {
                ViewModel.Redo();
            }
            else if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.A)
            {
                // Select all controls
                SelectAllControls();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                // Clear selection when Escape is pressed
                ClearSelection();
                e.Handled = true;
            }
            else if (e.Key == Key.S && e.KeyModifiers == KeyModifiers.Control)
            {
                // Execute the Save command
                var vm = this.DataContext as MainWindowViewModel;
                vm?.onSaveProject?.Execute(null);
                e.Handled = true;
            }
            else if (e.Key == Key.T)
            {
                // Test selection with T key
                TestSelection();
                e.Handled = true;
            }
            else if (e.Key == Key.D)
            {
                // Debug selection with D key
                DebugSelection();
                e.Handled = true;
            }
            else if (e.Key == Key.M)
            {
                // Test multi-selection with M key
                TestMultiSelection();
                e.Handled = true;
            }
            else if (e.Key == Key.N)
            {
                // Test Ctrl+Click simulation with N key
                var canvas = this.FindControl<Canvas>("screenCanvas");
                if (canvas != null && canvas.Children.Count >= 2)
                {
                    var control = canvas.Children[1] as Control;
                    if (control != null && IsSelectableControl(control))
                    {
                        SimulateCtrlClick(control);
                    }
                }
                e.Handled = true;
            }
            else if (e.Key == Key.C && e.KeyModifiers == KeyModifiers.Control && selectedElements.Count > 0)
            {
                // Copy selected controls (placeholder for future implementation)
                Console.WriteLine($"Copy {selectedElements.Count} selected controls");
                e.Handled = true;
            }
            else if (e.Key == Key.V && e.KeyModifiers == KeyModifiers.Control)
            {
                // Paste controls (placeholder for future implementation)
                Console.WriteLine("Paste controls");
                e.Handled = true;
            }
        }



        private Control? FindTopLevelUIControl(Control control)
        {
            // First, check if the control itself is a UI control
            if (IsSelectableControl(control))
            {
                return control;
            }

            // Look for UI controls in the ancestry
            var current = control;
            while (current != null)
            {
                if (IsSelectableControl(current))
                {
                    return current;
                }

                current = current.GetVisualParent() as Control;
            }

            return null;
        }

        private Control? FindDraggableParent(Control control)
        {
            Visual? current = control;

            while (current != null)
            {
                // if (current is LayoutTransformControl || current is Border) // or any root drag container
                // {
                //return (Control)current;
                // }

                //current = current.GetVisualParent();
                return (Control)current;
            }

            return null;
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

        private void onMousePressed(object sender, PointerPressedEventArgs e)
        {
            m_MainViewModel = (MainWindowViewModel)this.DataContext;
            var properties = e.GetCurrentPoint(this).Properties;
            if (properties.IsLeftButtonPressed)
            {
                m_MainViewModel.m_UIControlType = CONTROL_TYPE.AREA;
            }
            else
            {
                m_MainViewModel.m_UIControlType = CONTROL_TYPE.NONE;
            }
        }

        private void onMouseReleased(object sender, PointerReleasedEventArgs e)
        {
            m_MainViewModel.m_UIControlType = 0;
        }
    }
}