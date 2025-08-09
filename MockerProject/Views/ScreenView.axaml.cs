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
            this.AttachedToVisualTree += (_, _) => this.Focus(); // or screenCanvas.Focus();
            this.AddHandler(KeyUpEvent, OnKeyDown, RoutingStrategies.Tunnel);
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && selectedElement != null)
            {
                var deleteAction = new DeleteCommand(screenCanvas, selectedElement);
                ViewModel.ExecuteAction(deleteAction);
                selectedElement = null;
            }
            else if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.Z)
            {
                ViewModel.Undo();
            }
            else if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.Y)
            {
                ViewModel.Redo();
            }
            if (e.Key == Key.S && e.KeyModifiers == KeyModifiers.Control)
            {
                // Execute the Save command
                var vm = this.DataContext as MainWindowViewModel;
                vm?.onSaveProject?.Execute(null);
                e.Handled = true;
            }
        }

        public void OnElementSelected(object sender, PointerPressedEventArgs e)
        {
            if (e.Source is Control clickedControl)
            {
                // Traverse up to find the draggable container (e.g. LayoutTransformControl)
                var rootControl = FindDraggableParent(clickedControl);

                if (rootControl != null)
                {
                    selectedElement = rootControl;
                    e.Handled = true;
                }
            }
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

        private Control? FindDraggableParent(Control control)
        {
            Visual? current = control;

            while (current != null)
            {
                // If it's your custom controls, stop here
                if (current is ButtonControl || current is CheckControl || current is EditControl)
                    return (Control)current;

                current = current.GetVisualParent();
                return (Control)current;
            }

            return null;
        }
    }
}