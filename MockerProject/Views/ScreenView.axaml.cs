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
        private Control? selectedElement;
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
            this.AddHandler(InputElement.PointerPressedEvent, OnElementSelected, RoutingStrategies.Bubble, handledEventsToo: false);
        }

        public void UpdateSelectionHighlight(Control? element)
        {
            // Clear previous selection
            if (selectedElement != null)
            {
                selectedElement.ClearValue(Border.BorderBrushProperty);
                selectedElement.ClearValue(Border.BorderThicknessProperty);
            }

            selectedElement = element;

            if (selectedElement != null)
            {
                selectedElement.SetValue(Border.BorderBrushProperty, Brushes.Blue);
                selectedElement.SetValue(Border.BorderThicknessProperty, new Thickness(2));

                Console.WriteLine($"Selected: {selectedElement.GetType().Name}");
            }
        }

        public void ClearSelection()
        {
            if (selectedElement != null)
            {
                selectedElement.ClearValue(Border.BorderBrushProperty);
                selectedElement.ClearValue(Border.BorderThicknessProperty);
                selectedElement = null;
            }
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

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && selectedElement != null)
            {
                var canvas = this.FindControl<Canvas>("screenCanvas");
                if (canvas != null)
                {
                    var deleteAction = new DeleteCommand(canvas, selectedElement);
                    ViewModel.ExecuteAction(deleteAction);
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
        }

        private void OnElementSelected(object sender, PointerPressedEventArgs e)
        {
            // Don't handle double-clicks - let them pass through to the UIControl
            if (e.ClickCount > 1) return;

            if (e.Source is Control clickedControl)
            {
                // Find the top-level UI control that should be selected
                var rootControl = FindTopLevelUIControl(clickedControl);

                if (rootControl != null && rootControl is not Canvas)
                {
                    // Update the selection highlight
                    UpdateSelectionHighlight(rootControl);
                    e.Handled = true;
                }
                else
                {
                    // Clear selection if clicking on empty space
                    ClearSelection();
                }
            }
        }

        private Control? FindTopLevelUIControl(Control control)
        {
            // First, check if the control itself is a UI control
            if (control is ButtonControl || control is CheckControl || control is EditControl ||
                control is RadioControl || control is TabViewControl || control is UIControl ||
                control is RepeaterControl || control is ListBoxControl || control is SliderControl ||
                control is ImageControl || control is ContainerBoxControl || control is TreeViewControl)
            {
                return control;
            }

            // Look for UI controls in the ancestry
            var current = control;
            while (current != null)
            {
                if (current is ButtonControl || current is CheckControl || current is EditControl ||
                    current is RadioControl || current is TabViewControl || current is UIControl ||
                    current is RepeaterControl || current is ListBoxControl || current is SliderControl ||
                    current is ImageControl || current is ContainerBoxControl || current is TreeViewControl)
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