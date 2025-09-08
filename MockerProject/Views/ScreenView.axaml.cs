using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using MockerProject.Action;
using MockerProject.Models;
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
        public object? CustomData; // For storing background color changes
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
        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

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
        
        // Hover support
        private Control? currentlyHoveredElement;

        public ScreenView()
        {
            InitializeComponent();
            //this.PointerMoved += onMouseMove;
            this.AttachedToVisualTree += (_, _) =>
            {
                this.Focus(); // or screenCanvas.Focus();
            };
            this.AddHandler(KeyUpEvent, OnKeyDown, RoutingStrategies.Tunnel);

            // Add pointer pressed event handler to canvas for clearing selection when clicking outside
            this.AttachedToVisualTree += (_, _) =>
            {
                var canvas = this.FindControl<Canvas>("screenCanvas");
                if (canvas != null)
                {
                    canvas.AddHandler(PointerPressedEvent, OnCanvasPointerPressed, RoutingStrategies.Tunnel);
                    canvas.AddHandler(PointerMovedEvent, OnCanvasPointerMoved, RoutingStrategies.Tunnel);
                }
            };
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
            
            // Restore hover state if there's a currently hovered element
            if (currentlyHoveredElement != null)
            {
                AddHoverHighlight(currentlyHoveredElement);
            }
        }

        private void HighlightControl(Control control)
        {
            // Clear any hover highlighting first
            if (currentlyHoveredElement == control)
            {
                currentlyHoveredElement = null;
            }
            
            control.SetValue(Border.BorderBrushProperty, Brushes.Blue);
            control.SetValue(Border.BorderThicknessProperty, new Thickness(2));
        }

        private void UnhighlightControl(Control control)
        {
            control.ClearValue(Border.BorderBrushProperty);
            control.ClearValue(Border.BorderThicknessProperty);
            
            // Restore hover state if this control is currently being hovered
            if (currentlyHoveredElement == control)
            {
                AddHoverHighlight(control);
            }
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

        private void OnCanvasPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            // Check if the click was on the canvas itself (not on a control)
            var canvas = this.FindControl<Canvas>("screenCanvas");
            if (canvas == null) return;

            // Get the position of the click relative to the canvas
            var clickPoint = e.GetPosition(canvas);

            // Check if the click was on empty space (not on any control)
            bool clickedOnControl = false;
            foreach (var child in canvas.Children)
            {
                if (child is Control control)
                {
                    var bounds = control.Bounds;
                    if (bounds.Contains(clickPoint))
                    {
                        clickedOnControl = true;
                        break;
                    }
                }
            }

            // If clicked on empty space and Ctrl is not held, clear selection
            if (!clickedOnControl && !e.KeyModifiers.HasFlag(KeyModifiers.Control))
            {
                ClearSelection();
            }
        }

        private void OnCanvasPointerMoved(object? sender, PointerEventArgs e)
        {
            var canvas = this.FindControl<Canvas>("screenCanvas");
            if (canvas == null) return;

            // Get the position of the pointer relative to the canvas
            var pointerPoint = e.GetPosition(canvas);

            // Find the control under the pointer
            Control? hoveredControl = null;
            foreach (var child in canvas.Children)
            {
                if (child is Control control && IsSelectableControl(control))
                {
                    var bounds = control.Bounds;
                    if (bounds.Contains(pointerPoint))
                    {
                        hoveredControl = control;
                        break;
                    }
                }
            }

            // Handle hover state changes
            if (hoveredControl != currentlyHoveredElement)
            {
                // Remove hover from previous element
                if (currentlyHoveredElement != null && !selectedElements.Contains(currentlyHoveredElement))
                {
                    RemoveHoverHighlight(currentlyHoveredElement);
                }

                // Add hover to new element
                if (hoveredControl != null && !selectedElements.Contains(hoveredControl))
                {
                    AddHoverHighlight(hoveredControl);
                }

                currentlyHoveredElement = hoveredControl;
            }
        }

        private void AddHoverHighlight(Control control)
        {
            // Apply hover border - darker blue for better visibility
            control.SetValue(Border.BorderBrushProperty, Brushes.DarkBlue);
            control.SetValue(Border.BorderThicknessProperty, new Thickness(2));
        }

        private void RemoveHoverHighlight(Control control)
        {
            // Remove hover border
            control.ClearValue(Border.BorderBrushProperty);
            control.ClearValue(Border.BorderThicknessProperty);
        }
    }
}