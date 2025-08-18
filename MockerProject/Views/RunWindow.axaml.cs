using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using MockerProject.Models;
using MockerProject.ViewModels;
using MockerProject.ViewModels.UIViewModels;
using MockerProject.Views;
using MockerProject.Views.UIControls;
using static MockerProject.ViewModels.UIControlViewModel;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using FontStyle = Avalonia.Media.FontStyle;
using Image = Avalonia.Controls.Image;

namespace MockerProject
{
    public partial class RunWindow : Window
    {
        private MainWindowViewModel m_MainVM;

        public RunWindow()
        {
            InitializeComponent();
        }

        public void setMainViewModel(MainWindowViewModel mainViewModel)
        {
            this.DataContext = mainViewModel;
            m_MainVM = mainViewModel;
            Control w_Control = null;

            SetControltoCanvas(0);
        }

        public void Init()
        {
            for (int i = 1; i < this.screenCanvas.Children.Count;)
                this.screenCanvas.Children.RemoveAt(1);
        }

        private int GetIdtoPageName(string name)
        {
            int id = -1;
            for (int i = 0; i < m_MainVM.m_lstWorkScreen.Count; i++)
            {
                if (name == m_MainVM.m_lstWorkScreen[i].m_strName)
                    return i;
            }

            return id;
        }

        private void setBaseProperty(TemplatedControl m_control, UIControlViewModel model)
        {
            (m_control).Opacity = model.opacity;
            ToolTip.SetTip(m_control, model.Control_Tip);
            (m_control).Background = model.background;
            (m_control).BorderBrush = model.borderColor;
            (m_control).BorderThickness = model.borderThickness;
            (m_control).CornerRadius = model.borderRound;
            (m_control).Foreground = model.foreground;
            (m_control).FontSize = model.fontSize;
            (m_control).FontFamily = model.fontFamily;
            if (model.textBold)
                (m_control).FontWeight = FontWeight.Bold;
            if (model.textItalic)
                (m_control).FontStyle = FontStyle.Italic;
            (m_control).IsEnabled = model.isEnabled;
        }

        private void SetControltoCanvas(int id)
        {
            Control w_Control = null;

            this.rectangle.Width = m_MainVM.m_lstWorkScreen[id].m_Size.Width;
            this.rectangle.Height = m_MainVM.m_lstWorkScreen[id].m_Size.Height;
            this.rectangle.Fill = m_MainVM.m_lstWorkScreen[id].m_background;
            this.rectangle.Opacity = m_MainVM.m_lstWorkScreen[id].m_Opacity;
            int width = m_MainVM.m_lstWorkScreen[id].m_Size.Width;
            int height = m_MainVM.m_lstWorkScreen[id].m_Size.Height;

            BuidControl(m_MainVM.m_lstWorkScreen[id].screenCanvas, this.screenCanvas, id, width, height, 3);
        }

        private void BuidControl(Canvas sourceCanvas, Canvas destCanvas, int id, int width, int height, int start = 0)
        {
            Control w_Control = null;
            for (int i = start; i < sourceCanvas.Children.Count; i++)
            {
                UIControl w_UIControl = (UIControl)sourceCanvas.Children[i];
                w_UIControl.m_MainViewModel = this.m_MainVM;

                if (w_UIControl.m_nUIControlType == CONTROL_TYPE.BUTTON || w_UIControl.m_nUIControlType == CONTROL_TYPE.MULTIBUTTON)
                {
                    var buttonControl = new ButtonControl
                    {
                        DataContext = w_UIControl.m_ControlViewModel
                    };

                    var innerButton = buttonControl.FindControl<Button>("button");

                    if (innerButton != null)
                    {
                        if (w_UIControl.m_TapEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                            if (w_EventID != -1 && w_EventID != id)
                                innerButton.AddHandler(Button.ClickEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                        if (w_UIControl.m_DTapEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                            if (w_EventID != -1 && w_EventID != id)
                                innerButton.AddHandler(Button.DoubleTappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                        if (w_UIControl.m_HPressEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                            if (w_EventID != -1 && w_EventID != id)
                                innerButton.AddHandler(Button.HoldingEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                        if (w_UIControl.m_SwipeLeftEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                            if (w_EventID != -1 && w_EventID != id)
                                innerButton.AddHandler(Button.PointerMovedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                    }
                    w_Control = buttonControl;
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.TEXTBOX || w_UIControl.m_nUIControlType == CONTROL_TYPE.PASSWORD || w_UIControl.m_nUIControlType == CONTROL_TYPE.TEXTAREA)
                {
                    var editControl = new EditControl();
                    editControl.DataContext = w_UIControl.m_ControlViewModel;
                    var innerTextBox = editControl.FindControl<TextBox>("textBox");

                    if (innerTextBox != null)
                    {
                        if (w_UIControl.m_TapEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                            if (w_EventID != -1 && w_EventID != id)
                                innerTextBox.AddHandler(TextBox.TappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                        if (w_UIControl.m_DTapEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                            if (w_EventID != -1 && w_EventID != id)
                                innerTextBox.AddHandler(TextBox.DoubleTappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                        if (w_UIControl.m_HPressEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                            if (w_EventID != -1 && w_EventID != id)
                                innerTextBox.AddHandler(TextBox.HoldingEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                        if (w_UIControl.m_SwipeLeftEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                            if (w_EventID != -1 && w_EventID != id)
                                innerTextBox.AddHandler(TextBox.PointerMovedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                    }

                    w_Control = editControl;
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.LABEL || w_UIControl.m_nUIControlType == CONTROL_TYPE.LINK || w_UIControl.m_nUIControlType == CONTROL_TYPE.TITLE)
                {
                    w_Control = new TextBlock();
                    ((TextBlock)w_Control).Text = w_UIControl.m_strText;
                    ((TextBlock)w_Control).FontSize = w_UIControl.m_nFontSize;
                    ((TextBlock)w_Control).Opacity = w_UIControl.m_Opacity;
                    ToolTip.SetTip(w_Control, w_UIControl.m_Tooltip);
                    ((TextBlock)w_Control).Background = w_UIControl.m_Background;
                    ((TextBlock)w_Control).Foreground = w_UIControl.m_Foreground;
                    ((TextBlock)w_Control).FontFamily = w_UIControl.m_FontFamily;
                    if (w_UIControl.m_bFitWidth)
                    {
                        ((TextBlock)w_Control).TextWrapping = TextWrapping.Wrap;
                        ((TextBlock)w_Control).Width = double.NaN;
                        ((TextBlock)w_Control).Measure(Avalonia.Size.Infinity);
                        ((TextBlock)w_Control).Width = ((TextBlock)w_Control).DesiredSize.Width;

                    }
                    if (w_UIControl.m_bFitHeight)
                    {
                        ((TextBlock)w_Control).TextWrapping = TextWrapping.Wrap;
                        ((TextBlock)w_Control).Height = double.NaN;
                        ((TextBlock)w_Control).Measure(Avalonia.Size.Infinity);
                        ((TextBlock)w_Control).Width = ((TextBlock)w_Control).DesiredSize.Width;
                    }
                    if (w_UIControl.m_nUIControlType == CONTROL_TYPE.LINK) ((TextBlock)w_Control).TextDecorations = TextDecorations.Underline;
                    if (w_UIControl.m_bBold)
                        ((TextBlock)w_Control).FontWeight = FontWeight.Bold;
                    if (w_UIControl.m_bItalic)
                        ((TextBlock)w_Control).FontStyle = FontStyle.Italic;
                    ((TextBlock)w_Control).IsEnabled = !w_UIControl.m_bDisable;
                    if (w_UIControl.m_TapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((TextBlock)w_Control).AddHandler(TextBlock.TappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_DTapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((TextBlock)w_Control).AddHandler(TextBlock.DoubleTappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_HPressEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((TextBlock)w_Control).AddHandler(TextBlock.HoldingEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_SwipeLeftEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((TextBlock)w_Control).AddHandler(TextBlock.PointerMovedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.IMAGE)
                {
                    w_Control = new Image();
                    ((Image)w_Control).Opacity = w_UIControl.m_Opacity;

                    ToolTip.SetTip(w_Control, w_UIControl.m_Tooltip);
                    ((Image)w_Control).Source = new Bitmap(w_UIControl.m_strSrc);
                    //((Image)w_Control).Text = w_UIControl.m_strText;
                    ((Image)w_Control).IsEnabled = !w_UIControl.m_bDisable;
                    ((Image)w_Control).Stretch = Stretch.Fill;
                    if (w_UIControl.m_TapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Image)w_Control).AddHandler(Image.TappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_DTapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Image)w_Control).AddHandler(Image.DoubleTappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_HPressEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Image)w_Control).AddHandler(Image.HoldingEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_SwipeLeftEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Image)w_Control).AddHandler(Image.PointerMovedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.CHECK)
                {
                    var checkControl = new CheckControl
                    {
                        DataContext = w_UIControl.m_ControlViewModel
                    };

                    var innerCheckBox = checkControl.FindControl<CheckBox>("checkBox");

                    if (innerCheckBox != null)
                    {
                        if (w_UIControl.m_TapEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                            if (w_EventID != -1 && w_EventID != id)
                            {
                                innerCheckBox.AddHandler(CheckBox.TappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                            }
                        }
                        if (w_UIControl.m_DTapEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                            if (w_EventID != -1 && w_EventID != id)
                            {
                                innerCheckBox.AddHandler(CheckBox.DoubleTappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                            }
                        }
                        if (w_UIControl.m_HPressEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                            if (w_EventID != -1 && w_EventID != id)
                            {
                                innerCheckBox.AddHandler(CheckBox.HoldingEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                            }
                        }
                        if (w_UIControl.m_SwipeLeftEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                            if (w_EventID != -1 && w_EventID != id)
                            {
                                innerCheckBox.AddHandler(CheckBox.PointerMovedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                            }
                        }
                    }
                    w_Control = checkControl;
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.DROPDOWN)
                {
                    w_Control = new ComboBox();

                    setBaseProperty((TemplatedControl)w_Control, (UIControlViewModel)w_UIControl.m_ControlViewModel);

                    foreach (CustomItem subItem in ((ListBoxViewModel)w_UIControl.m_ControlViewModel).Items)
                    {
                        ((ComboBox)w_Control).Items.Add(subItem.text);

                    }
                    ((ComboBox)w_Control).SelectedIndex = ((DropDownControl)w_UIControl).listBox.SelectedIndex;
                    ((ComboBox)w_Control).AddHandler(ComboBox.SelectionChangedEvent, (sender, e) =>
                    {
                        int index = ((ComboBox)sender).SelectedIndex;
                        int w_EventID = GetIdtoPageName(((ListBoxViewModel)w_UIControl.m_ControlViewModel).Items[index].iteration);
                        if (w_EventID != -1 && w_EventID != id)
                        {
                            Init();
                            SetControltoCanvas(w_EventID);
                        }

                    }, handledEventsToo: true);

                    foreach (IterationItem iterationItem in ((ListBoxViewModel)w_UIControl.m_ControlViewModel).iterationItems)
                    {
                        if (iterationItem.text == "Tapped")
                        {
                            int w_EventID = GetIdtoPageName(iterationItem.iteration);
                            if (w_EventID != -1 && w_EventID != id)
                            {
                                ((ComboBox)w_Control).AddHandler(ComboBox.TappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                            }
                        }
                        if (iterationItem.text == "Double Tapped")
                        {
                            int w_EventID = GetIdtoPageName(iterationItem.iteration);
                            if (w_EventID != -1 && w_EventID != id)
                                ((ComboBox)w_Control).AddHandler(ComboBox.DoubleTappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                    }
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.CONTAINERBOX)
                {
                    w_Control = new ContainerBoxRunControl();

                    setBaseProperty((TemplatedControl)w_Control, (UIControlViewModel)w_UIControl.m_ControlViewModel);
                    int w = (int)((ContainerBoxControl)w_UIControl).container.Width;
                    int h = (int)((ContainerBoxControl)w_UIControl).container.Height;
                    BuidControl(((ContainerBoxControl)w_UIControl).container, ((ContainerBoxRunControl)w_Control).container, id, w, h);
                    //((ContainerBoxRunControl)w_Control).container = ((ContainerBoxControl)w_UIControl).container;

                    //foreach (IterationItem iterationItem in ((ListBoxViewModel)w_UIControl.m_ControlViewModel).iterationItems)
                    //{
                    //    if (iterationItem.text == "Tapped")
                    //    {
                    //        int w_EventID = GetIdtoPageName(iterationItem.iteration);
                    //        if (w_EventID != -1 && w_EventID != id)
                    //        {
                    //            ((ComboBox)w_Control).AddHandler(ComboBox.TappedEvent, (sender, e) =>
                    //            {
                    //                Init();
                    //                SetControltoCanvas(w_EventID);
                    //            }, handledEventsToo: true);
                    //        }
                    //    }
                    //    if (iterationItem.text == "Double Tapped")
                    //    {
                    //        int w_EventID = GetIdtoPageName(iterationItem.iteration);
                    //        if (w_EventID != -1 && w_EventID != id)
                    //            ((ComboBox)w_Control).AddHandler(ComboBox.DoubleTappedEvent, (sender, e) =>
                    //            {
                    //                Init();
                    //                SetControltoCanvas(w_EventID);
                    //            }, handledEventsToo: true);
                    //    }
                    //}
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.REPEATER)
                {
                    w_Control = new RepeaterRunControl();
                    setBaseProperty((TemplatedControl)w_Control, (UIControlViewModel)w_UIControl.m_ControlViewModel);
                    foreach (ContainerBoxControl item in ((RepeaterControlViewModel)w_UIControl.m_ControlViewModel).Items)
                    {
                        ContainerBoxRunControl containerItem = new ContainerBoxRunControl();

                        int w = (int)(item.container).Width;
                        int h = (int)(item.container).Height;
                        BuidControl(item.container, containerItem.container, id, w, h);
                        setBaseProperty((TemplatedControl)containerItem, (UIControlViewModel)item.m_ControlViewModel);
                        containerItem.Width = w;
                        containerItem.Height = h;
                        ((RepeaterRunControl)w_Control).itemControl.Items.Add(containerItem);
                    }
                    w_UIControl.m_nWidth = 300;
                    w_UIControl.m_nHeight = 500;
                    //((ContainerBoxRunControl)w_Control).container = ((ContainerBoxControl)w_UIControl).container;
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.TABS)
                {
                    w_Control = new TabViewRunControl();

                    setBaseProperty((TemplatedControl)w_Control, (UIControlViewModel)w_UIControl.m_ControlViewModel);
                    int index = 0;
                    foreach (ContainerBoxControl item in ((RepeaterControlViewModel)w_UIControl.m_ControlViewModel).Items)
                    {
                        ContainerBoxRunControl containerItem = new ContainerBoxRunControl();

                        int w = (int)(item.container).Width;
                        int h = (int)(item.container).Height;
                        BuidControl(item.container, containerItem.container, id, w, h);
                        setBaseProperty((TemplatedControl)containerItem, (UIControlViewModel)item.m_ControlViewModel);
                        containerItem.Width = w;
                        containerItem.Height = h;
                        TabItem tabItem1 = new TabItem();
                        tabItem1.Header = ((RepeaterControlViewModel)w_UIControl.m_ControlViewModel).TabHeaders[index];
                        tabItem1.Content = containerItem;
                        tabItem1.Foreground = new SolidColorBrush(new Color(255, 0, 0, 0));

                        ((TabViewRunControl)w_Control).tabControl.Items.Add(tabItem1);
                        index++;
                    }
                    w_UIControl.m_nWidth = 300;
                    w_UIControl.m_nHeight = 500;
                    //((ContainerBoxRunControl)w_Control).container = ((ContainerBoxControl)w_UIControl).container;
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.LISTBOX)
                {
                    w_Control = new ListBox();

                    setBaseProperty((TemplatedControl)w_Control, (UIControlViewModel)w_UIControl.m_ControlViewModel);

                    foreach (CustomItem subItem in ((ListBoxViewModel)w_UIControl.m_ControlViewModel).Items)
                    {

                        ListBoxItem itm = new ListBoxItem();
                        itm.Content = subItem.text;
                        ((ListBox)w_Control).Items.Add(itm);
                        itm.Background = ((ListBoxViewModel)w_UIControl.m_ControlViewModel).itemBackground;

                    }
                   ((ListBox)w_Control).SelectedIndex = ((ListBoxControl)w_UIControl).listBox.SelectedIndex;


                    ((ListBox)w_Control).SelectionChanged += (sender, e) =>
                    {
                        int index = (int)((ListBox)sender).SelectedIndex;

                        int w_EventID = GetIdtoPageName(((ListBoxViewModel)w_UIControl.m_ControlViewModel).Items[index].iteration);
                        if (w_EventID != -1 && w_EventID != id)
                        {
                            Init();
                            SetControltoCanvas(w_EventID);
                        }

                    };
                    foreach (IterationItem iterationItem in ((ListBoxViewModel)w_UIControl.m_ControlViewModel).iterationItems)
                    {
                        if (iterationItem.text == "Tapped")
                        {
                            int w_EventID = GetIdtoPageName(iterationItem.iteration);
                            if (w_EventID != -1 && w_EventID != id)
                            {
                                ((ListBox)w_Control).AddHandler(ListBox.TappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                            }
                        }
                        if (iterationItem.text == "Double Tapped")
                        {
                            int w_EventID = GetIdtoPageName(iterationItem.iteration);
                            if (w_EventID != -1 && w_EventID != id)
                                ((ListBox)w_Control).AddHandler(ListBox.DoubleTappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                    }




                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.TREEVIEW)
                {
                    w_Control = new TreeViewRunControl();

                    setBaseProperty((TemplatedControl)w_Control, (UIControlViewModel)w_UIControl.m_ControlViewModel);

                    w_Control.DataContext = (TreeViewViewModel)w_UIControl.m_ControlViewModel;

                    (((TreeViewRunControl)w_Control).treeView).SelectionChanged += (sender, e) =>
                    {
                        Node selectedItem = (Node)((TreeView)sender).SelectedItem;
                        int w_EventID = GetIdtoPageName(selectedItem.iteration);
                        if (w_EventID != -1 && w_EventID != id)
                        {
                            Init();
                            SetControltoCanvas(w_EventID);
                        }
                    };

                    foreach (IterationItem iterationItem in ((TreeViewViewModel)w_UIControl.m_ControlViewModel).iterationItems)
                    {
                        if (iterationItem.text == "Tapped")
                        {
                            int w_EventID = GetIdtoPageName(iterationItem.iteration);
                            if (w_EventID != -1 && w_EventID != id)
                                (((TreeViewRunControl)w_Control).treeView).AddHandler(TreeView.TappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                        if (iterationItem.text == "Double Tapped")
                        {
                            int w_EventID = GetIdtoPageName(iterationItem.iteration);
                            if (w_EventID != -1 && w_EventID != id)
                                (((TreeViewRunControl)w_Control).treeView).AddHandler(TreeView.DoubleTappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                        }
                    }
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.SLIDER)
                {
                    w_Control = new Slider();
                    ((Slider)w_Control).Opacity = w_UIControl.m_Opacity;
                    ToolTip.SetTip(w_Control, w_UIControl.m_Tooltip);
                    ((Slider)w_Control).Minimum = 0;
                    ((Slider)w_Control).Maximum = 100;
                    ((Slider)w_Control).TickFrequency = 1;
                    ((Slider)w_Control).Value = int.Parse(w_UIControl.m_strText.Substring(0, 2));
                    ((Slider)w_Control).Background = w_UIControl.m_Background;
                    ((Slider)w_Control).Foreground = w_UIControl.m_BorderColor;
                    ((Slider)w_Control).IsEnabled = !w_UIControl.m_bDisable;
                    //((Image)w_Control).Text = w_UIControl.m_strText;
                    if (w_UIControl.m_TapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Slider)w_Control).AddHandler(Slider.TappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_DTapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Slider)w_Control).AddHandler(Slider.DoubleTappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_HPressEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Slider)w_Control).AddHandler(Slider.HoldingEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_SwipeLeftEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Slider)w_Control).AddHandler(Slider.PointerMovedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }

                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.PROGRESS)
                {
                    w_Control = new ProgressBar();
                    ((ProgressBar)w_Control).Opacity = w_UIControl.m_Opacity;
                    ToolTip.SetTip(w_Control, w_UIControl.m_Tooltip);
                    ((ProgressBar)w_Control).Minimum = 0;
                    ((ProgressBar)w_Control).Maximum = 100;
                    ((ProgressBar)w_Control).ShowProgressText = true;
                    ((ProgressBar)w_Control).Value = int.Parse(w_UIControl.m_strText.Substring(0, 2));
                    ((ProgressBar)w_Control).Background = w_UIControl.m_Background;
                    ((ProgressBar)w_Control).Foreground = w_UIControl.m_BorderColor;
                    ((ProgressBar)w_Control).IsEnabled = !w_UIControl.m_bDisable;
                    //((Image)w_Control).Text = w_UIControl.m_strText;
                    if (w_UIControl.m_TapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((ProgressBar)w_Control).AddHandler(ProgressBar.TappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_DTapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((ProgressBar)w_Control).AddHandler(ProgressBar.DoubleTappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_HPressEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((ProgressBar)w_Control).AddHandler(ProgressBar.HoldingEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_SwipeLeftEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((ProgressBar)w_Control).AddHandler(ProgressBar.PointerMovedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.RADIO)
                {
                    var radioControl = new RadioControl
                    {
                        DataContext = w_UIControl.m_ControlViewModel
                    };

                    // find the inner radio button
                    var innerRadio = radioControl.FindControl<RadioButton>("radioButton");

                    if (innerRadio != null)
                    {
                        if (w_UIControl.m_TapEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                            if (w_EventID != -1 && w_EventID != id)
                            {
                                innerRadio.AddHandler(RadioButton.TappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                            }
                        }
                        if (w_UIControl.m_DTapEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                            if (w_EventID != -1 && w_EventID != id)
                            {
                                innerRadio.AddHandler(RadioButton.DoubleTappedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                            }
                        }
                        if (w_UIControl.m_HPressEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                            if (w_EventID != -1 && w_EventID != id)
                            {
                                innerRadio.AddHandler(RadioButton.HoldingEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                            }
                        }
                        if (w_UIControl.m_SwipeLeftEvent != null)
                        {
                            int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                            if (w_EventID != -1 && w_EventID != id)
                            {
                                innerRadio.AddHandler(RadioButton.PointerMovedEvent, (sender, e) =>
                                {
                                    Init();
                                    SetControltoCanvas(w_EventID);
                                }, handledEventsToo: true);
                            }
                        }
                    }

                    w_Control = radioControl;
                }
                else continue;
                w_Control.Width = w_UIControl.m_nWidth;
                w_Control.Height = w_UIControl.m_nHeight;

                //Canvas.SetTop(w_Control,-20);
                Canvas.SetTop(w_Control, w_UIControl.m_nPositionY - w_UIControl.m_nOffsetY);
                Canvas.SetLeft(w_Control, w_UIControl.m_nPositionX - w_UIControl.m_nOffsetX);
                destCanvas.Children.Add(w_Control);
                destCanvas.Width = width;
                destCanvas.Height = height;
            }
        }
    }
}