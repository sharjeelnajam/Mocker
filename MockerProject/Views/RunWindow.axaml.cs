using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MockerProject.ViewModels;
using MockerProject.Views;
using System.Drawing;
using Avalonia.Styling;
using MockerProject.Views.UIControls;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using FontStyle = Avalonia.Media.FontStyle;
using Image = Avalonia.Controls.Image;
using System;
using MockerProject.Models;
using MockerProject.ViewModels.UIViewModels;
using static MockerProject.ViewModels.UIControlViewModel;
using System.Reflection;
using Avalonia.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;



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
            for(int i=1; i<this.screenCanvas.Children.Count;)
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
            int width =  m_MainVM.m_lstWorkScreen[id].m_Size.Width;
            int height = m_MainVM.m_lstWorkScreen[id].m_Size.Height;
            
            BuidControl(m_MainVM.m_lstWorkScreen[id].screenCanvas,this.screenCanvas, id,width,height,3);
            
        }

        private void BuidControl(Canvas sourceCanvas, Canvas destCanvas,int id,int width,int height,int start = 0)
        {
            Control w_Control = null;
            for (int i = start; i < sourceCanvas.Children.Count; i++)
            {
                UIControl w_UIControl = (UIControl)sourceCanvas.Children[i];
                if (w_UIControl.m_nUIControlType == CONTROL_TYPE.BUTTON || w_UIControl.m_nUIControlType == CONTROL_TYPE.MULTIBUTTON)
                {
                    w_Control = new Button();
                    ((Button)w_Control).HorizontalContentAlignment = HorizontalAlignment.Center;
                    ((Button)w_Control).VerticalContentAlignment = VerticalAlignment.Center;
                    ((Button)w_Control).Padding = new Thickness(0);
                    ((Button)w_Control).Content = w_UIControl.m_strText;
                    ((Button)w_Control).FontSize = w_UIControl.m_nFontSize;
                    ((Button)w_Control).Opacity = w_UIControl.m_Opacity;
                    ToolTip.SetTip(w_Control, w_UIControl.m_Tooltip);
                    ((Button)w_Control).Background = w_UIControl.m_Background;
                    ((Button)w_Control).Foreground = w_UIControl.m_Foreground;
                    ((Button)w_Control).BorderThickness = w_UIControl.m_BorderThickness;
                    ((Button)w_Control).CornerRadius = w_UIControl.m_BorderRound;
                    ((Button)w_Control).BorderBrush = w_UIControl.m_BorderColor;
                    ((Button)w_Control).FontFamily = w_UIControl.m_FontFamily;


                    if (w_UIControl.m_bBold)
                        ((Button)w_Control).FontWeight = FontWeight.Bold;
                    if (w_UIControl.m_bItalic)
                        ((Button)w_Control).FontStyle = FontStyle.Italic;
                    ((Button)w_Control).IsEnabled = !w_UIControl.m_bDisable;

                    if (w_UIControl.m_TapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Button)w_Control).AddHandler(Button.ClickEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_DTapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Button)w_Control).AddHandler(Button.DoubleTappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_HPressEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Button)w_Control).AddHandler(Button.HoldingEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_SwipeLeftEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((Button)w_Control).AddHandler(Button.PointerMovedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.TEXTBOX || w_UIControl.m_nUIControlType == CONTROL_TYPE.PASSWORD || w_UIControl.m_nUIControlType == CONTROL_TYPE.TEXTAREA)
                {
                    w_Control = new TextBox();
                    ((TextBox)w_Control).Text = w_UIControl.m_strText;
                    ((TextBox)w_Control).FontSize = w_UIControl.m_nFontSize;
                    ((TextBox)w_Control).Opacity = w_UIControl.m_Opacity;
                    ToolTip.SetTip(w_Control, w_UIControl.m_Tooltip);
                    ((TextBox)w_Control).Background = w_UIControl.m_Background;
                    ((TextBox)w_Control).Foreground = w_UIControl.m_Foreground;
                    ((TextBox)w_Control).BorderThickness = w_UIControl.m_BorderThickness;
                    ((TextBox)w_Control).CornerRadius = w_UIControl.m_BorderRound;
                    ((TextBox)w_Control).BorderBrush = w_UIControl.m_BorderColor;
                    ((TextBox)w_Control).FontFamily = w_UIControl.m_FontFamily;
                    if (w_UIControl.m_nUIControlType == CONTROL_TYPE.PASSWORD) ((TextBox)w_Control).PasswordChar = w_UIControl.m_strPasswordChar.ToCharArray()[0];
                    if (w_UIControl.m_bBold)
                        ((TextBox)w_Control).FontWeight = FontWeight.Bold;
                    if (w_UIControl.m_bItalic)
                        ((TextBox)w_Control).FontStyle = FontStyle.Italic;
                    ((TextBox)w_Control).IsEnabled = !w_UIControl.m_bDisable;
                    if (w_UIControl.m_TapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((TextBox)w_Control).AddHandler(TextBox.TappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_DTapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((TextBox)w_Control).AddHandler(TextBox.DoubleTappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_HPressEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((TextBox)w_Control).AddHandler(TextBox.HoldingEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_SwipeLeftEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((TextBox)w_Control).AddHandler(TextBox.PointerMovedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }

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
                    if(w_UIControl.m_bFitWidth)
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
                    ((Image)w_Control).Stretch =Stretch.Fill;
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
                    w_Control = new CheckBox();
                    ((CheckBox)w_Control).HorizontalContentAlignment = HorizontalAlignment.Center;
                    ((CheckBox)w_Control).VerticalContentAlignment = VerticalAlignment.Center;
                    ((CheckBox)w_Control).Padding = new Thickness(0);
                    ((CheckBox)w_Control).Content = w_UIControl.m_strText;
                    ((CheckBox)w_Control).FontSize = w_UIControl.m_nFontSize;
                    ((CheckBox)w_Control).Opacity = w_UIControl.m_Opacity;
                    ToolTip.SetTip(w_Control, w_UIControl.m_Tooltip);
                    ((CheckBox)w_Control).Background = w_UIControl.m_Background;
                    ((CheckBox)w_Control).Foreground = w_UIControl.m_Foreground;
                    ((CheckBox)w_Control).BorderThickness = w_UIControl.m_BorderThickness;
                    ((CheckBox)w_Control).CornerRadius = w_UIControl.m_BorderRound;
                    ((CheckBox)w_Control).BorderBrush = w_UIControl.m_BorderColor;
                    ((CheckBox)w_Control).FontFamily = w_UIControl.m_FontFamily;
                    if (w_UIControl.m_bBold)
                        ((CheckBox)w_Control).FontWeight = FontWeight.Bold;
                    if (w_UIControl.m_bItalic)
                        ((CheckBox)w_Control).FontStyle = FontStyle.Italic;
                    ((CheckBox)w_Control).IsEnabled = !w_UIControl.m_bDisable;
                    if (w_UIControl.m_TapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((CheckBox)w_Control).AddHandler(CheckBox.TappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_DTapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((CheckBox)w_Control).AddHandler(CheckBox.DoubleTappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_HPressEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((CheckBox)w_Control).AddHandler(CheckBox.HoldingEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_SwipeLeftEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((CheckBox)w_Control).AddHandler(CheckBox.PointerMovedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
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
                    int w = (int) ((ContainerBoxControl)w_UIControl).container.Width;
                    int h =(int) ((ContainerBoxControl)w_UIControl).container.Height;
                    BuidControl(((ContainerBoxControl)w_UIControl).container, ((ContainerBoxRunControl)w_Control).container, id,w,h);
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
                    w_Control = new RadioButton();
                    ((RadioButton)w_Control).Opacity = w_UIControl.m_Opacity;
                    ToolTip.SetTip(w_Control, w_UIControl.m_Tooltip);
                    ((RadioButton)w_Control).Content = w_UIControl.m_strText;
                    ((RadioButton)w_Control).Foreground = w_UIControl.m_Foreground;
                    ((RadioButton)w_Control).FontSize = w_UIControl.m_nFontSize;
                    ((RadioButton)w_Control).FontFamily = w_UIControl.m_FontFamily;
                    if (w_UIControl.m_bBold)
                        ((RadioButton)w_Control).FontWeight = FontWeight.Bold;
                    if (w_UIControl.m_bItalic)
                        ((RadioButton)w_Control).FontStyle = FontStyle.Italic;
                    ((RadioButton)w_Control).IsEnabled = !w_UIControl.m_bDisable;
                    //((Image)w_Control).Text = w_UIControl.m_strText;
                    if (w_UIControl.m_TapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_TapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((RadioButton)w_Control).AddHandler(RadioButton.TappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_DTapEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_DTapEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((RadioButton)w_Control).AddHandler(RadioButton.DoubleTappedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_HPressEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_HPressEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((RadioButton)w_Control).AddHandler(RadioButton.HoldingEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                    if (w_UIControl.m_SwipeLeftEvent != null)
                    {
                        int w_EventID = GetIdtoPageName(w_UIControl.m_SwipeLeftEvent);
                        if (w_EventID != -1 && w_EventID != id)
                            ((RadioButton)w_Control).AddHandler(RadioButton.PointerMovedEvent, (sender, e) =>
                            {
                                Init();
                                SetControltoCanvas(w_EventID);
                            }, handledEventsToo: true);
                    }
                }
                else continue;
                w_Control.Width = w_UIControl.m_nWidth;
                w_Control.Height = w_UIControl.m_nHeight;
                
                //Canvas.SetTop(w_Control,-20);
                Canvas.SetTop(w_Control, w_UIControl.m_nPositionY- w_UIControl.m_nOffsetY);
                Canvas.SetLeft(w_Control, w_UIControl.m_nPositionX- w_UIControl.m_nOffsetX);
                destCanvas.Children.Add(w_Control);
                destCanvas.Width = width;
                destCanvas.Height = height;
            }
        }
    }

}
