using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using MockerProject.Models;
using MockerProject.ViewModels;
using MockerProject.Views.UIControls;

namespace MockerProject.Views;

public partial class BodyView : UserControl
{
    UIControl m_Control = null;
    public BodyView()
    {
        InitializeComponent();
        this.Toolbar.UIControl.Button.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Button.PointerExited += onMouseExit;
        this.Toolbar.UIControl.TextBox.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.TextBox.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Label.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Label.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Title.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Title.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Image.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Image.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Radio.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Radio.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Check.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Check.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Password.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Password.PointerExited += onMouseExit;
        this.Toolbar.UIControl.MultilineButton.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.MultilineButton.PointerExited += onMouseExit;
        this.Toolbar.UIControl.TextArea.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.TextArea.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Link.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Link.PointerExited += onMouseExit;
		this.Toolbar.UIControl.listBox.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.listBox.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Repeater.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Repeater.PointerExited += onMouseExit;
        this.Toolbar.UIControl.DropDown.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.DropDown.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Slider.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Slider.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Progress.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Progress.PointerExited += onMouseExit;
        this.Toolbar.UIControl.TreeView.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.TreeView.PointerExited += onMouseExit;
        this.Toolbar.UIControl.ContainerBox.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.ContainerBox.PointerExited += onMouseExit;
        this.Toolbar.UIControl.Tabs.PointerPressed += onMouseUp;
        this.Toolbar.UIControl.Tabs.PointerExited += onMouseExit;

        this.PointerMoved += onMouseMove;
        this.PointerReleased += onMouseDown;
    }

    private void onMouseExit(object sender, PointerEventArgs e)
    {
        MainWindowViewModel mainViewModel = (MainWindowViewModel)this.DataContext;
        mainViewModel.m_UIControlType = 0;
    }

    private void onMouseUp(object sender, PointerPressedEventArgs e)
    {
        MainWindowViewModel mainViewModel = (MainWindowViewModel)this.DataContext;
        var properties = e.GetCurrentPoint(this).Properties;
        mainViewModel.WorkScreen.LinePosition.IsVisible = false;
        if (properties.IsLeftButtonPressed && sender.GetType() == typeof(Image))
        {
            Control w_Control = (Control)sender;
            if (w_Control.Name == "Button")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.BUTTON;
            }
            else if (w_Control.Name == "TextBox")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.TEXTBOX;
            }
            else if (w_Control.Name == "Label")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.LABEL;
            }
            else if (w_Control.Name == "Title")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.TITLE;
            }
            else if (w_Control.Name == "Image")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.IMAGE;
            }
            else if (w_Control.Name == "Check")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.CHECK;
            }
            else if (w_Control.Name == "Password")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.PASSWORD;
            }
            else if (w_Control.Name == "TreeView")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.TREEVIEW;
            }
            else if (w_Control.Name == "MultilineButton")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.MULTIBUTTON;
            }
            else if (w_Control.Name == "TextArea")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.TEXTAREA;
            }
            else if (w_Control.Name == "Link")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.LINK;
            }
            else if (w_Control.Name == "Radio")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.RADIO;
            }
			else if (w_Control.Name == "listBox")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.LISTBOX;
            }
            else if (w_Control.Name == "Repeater")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.REPEATER;
            }
            else if (w_Control.Name == "DropDown")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.DROPDOWN;
            }
            else if (w_Control.Name == "Slider")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.SLIDER;
            }
            else if (w_Control.Name == "Progress")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.PROGRESS;
            }
            else if (w_Control.Name == "TreeView")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.TREEVIEW;
            }
            else if (w_Control.Name == "ContainerBox")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.CONTAINERBOX;
            }
            else if (w_Control.Name == "Tabs")
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.TABS;
            }
            else
            {
                mainViewModel.m_UIControlType = CONTROL_TYPE.NONE;
            }
        }
        else
        {
            mainViewModel.m_UIControlType = CONTROL_TYPE.NONE;
        }
        return;
    }
    
    private void onMouseMove(object sender, PointerEventArgs e)
    {
        MainWindowViewModel mainViewModel = (MainWindowViewModel)this.DataContext;
        CONTROL_TYPE w_UIControlType = mainViewModel.m_UIControlType;
        CONTROL_TYPE w_nSelectedUIControl = mainViewModel.m_nSelectedUIControl;
        var properties = e.GetCurrentPoint(this).Properties;
        if (properties.IsLeftButtonPressed)
        {

            double sX = e.GetPosition(this.Work.Screen).X;
            double sY = e.GetPosition(this.Work.Screen).Y;
            if (mainViewModel.ContainerFlag >0)
            {
                var ttv = mainViewModel.ContainerCanvas[mainViewModel.ContainerFlag - 1].TransformToVisual(mainViewModel.WorkScreen.screenCanvas);
                Point screenCoords =(new Point(0,0)).Transform((Matrix)ttv);
                sX -= screenCoords.X;
                sY -= screenCoords.Y;
            }
            if (w_UIControlType > 0)
            {
                if (m_Control == null)
                {
                    if (w_UIControlType == CONTROL_TYPE.AREA)
                    {
                        mainViewModel.PG_RW = (int)sX - mainViewModel.PG_X;
                        mainViewModel.PG_RH = (int)sY - mainViewModel.PG_Y;
                        if (mainViewModel.PF_Ang == 0)
                        {
                            if (mainViewModel.PG_RW < mainViewModel.PG_W)
                                mainViewModel.PG_RW = mainViewModel.PG_W;
                            if (mainViewModel.PG_RH < mainViewModel.PG_H)
                                mainViewModel.PG_RH = mainViewModel.PG_H;
                        }
                        else
                        {
                            if (mainViewModel.PG_RW < mainViewModel.PG_H)
                                mainViewModel.PG_RW = mainViewModel.PG_H;
                            if (mainViewModel.PG_RH < mainViewModel.PG_W)
                                mainViewModel.PG_RH = mainViewModel.PG_W;
                        }
                        
                    }
                    else if (w_UIControlType == CONTROL_TYPE.BUTTON || w_UIControlType == CONTROL_TYPE.MULTIBUTTON)
                    {
                        m_Control = new ButtonControl();
                        m_Control.setMainVM(mainViewModel);
                        if(w_UIControlType == CONTROL_TYPE.BUTTON)
                        {
                            m_Control.setName("Button");
                            m_Control.setText("Button");
                            m_Control.setSize(100, 30);
                            m_Control.setFontSizeID(7);
                            m_Control.setType(w_UIControlType);
                        }
                       
                        if (w_UIControlType == CONTROL_TYPE.MULTIBUTTON)
                        {
                            m_Control.setName("MultilineButton");
                            m_Control.setText("Multiline\nButton");
                            m_Control.setFontSizeID(7);
                            m_Control.setType(w_UIControlType);
                            m_Control.setTextMultiEnable(true);
                            m_Control.setSize(165, 70);
                        }
                            //m_Control.setBackground(new SolidColorBrush(new Color(255, 200, 200, 200)));
                        }
                    else if (w_UIControlType == CONTROL_TYPE.TEXTBOX || w_UIControlType == CONTROL_TYPE.TEXTAREA)
                    {
                        m_Control = new EditControl();
                        
                        m_Control.setMainVM(mainViewModel);
                       if(w_UIControlType == CONTROL_TYPE.TEXTBOX) m_Control.setName("TextBox");
                       if(w_UIControlType == CONTROL_TYPE.TEXTAREA) m_Control.setName("TextArea");
                        m_Control.setFontSizeID(3);


                        m_Control.setType(w_UIControlType);
                        m_Control.setPasswordChar("");
                        if ( w_UIControlType == CONTROL_TYPE.TEXTBOX)
                        {
                            
                            m_Control.setSize(200,24);
                            m_Control.setText("This is TextBox");
                        }
                        if (w_UIControlType == CONTROL_TYPE.TEXTAREA)
                        {
                            m_Control.setText("This is \nTextArea");
                            m_Control.setTextMultiEnable(true);
                            m_Control.setSize(200,70);
                        }
                       
                        //m_Control.setBackground(new SolidColorBrush(new Color(255,255,255,255)));
                    }
                    else if(w_UIControlType == CONTROL_TYPE.LABEL)
                    {
                        m_Control = new LabelControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setName("Label");
                        m_Control.setText("This is label");
                        m_Control.setSize(100, 30);
                        m_Control.setFontSizeID(7);
                        m_Control.setType(w_UIControlType);
                    }
                    else if(w_UIControlType == CONTROL_TYPE.TITLE)
                    {
                        m_Control = new LabelControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setName("Title");
                        m_Control.setText("TITLE OF DESIGN");
                        m_Control.setSize(300, 50);
                        m_Control.setFontSizeID(10);
                        m_Control.setType(w_UIControlType);
                    }
                    else if(w_UIControlType == CONTROL_TYPE.IMAGE)
                    {
                        m_Control = new ImageControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setName("Image");
                        
                        m_Control.setSize(100, 100);
                        m_Control.setText("Image");
                        m_Control.setType(w_UIControlType);
                    }
                    else if (w_UIControlType == CONTROL_TYPE.CHECK)
                    {
                        m_Control = new CheckControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setType(w_UIControlType);
                    }
                    else if (w_UIControlType == CONTROL_TYPE.PASSWORD)
                    {
                        m_Control = new EditControl();

                        m_Control.setMainVM(mainViewModel);
                        m_Control.setName("Password");
                        m_Control.setText("This is Password");
                        m_Control.setSize(200,24);
                        m_Control.setFontSizeID(4);
                        m_Control.setType(w_UIControlType);
                        m_Control.setPasswordChar("*");
                        //m_Control.setBackground(new SolidColorBrush(new Color(255,255,255,255)));
                    }
                    else if (w_UIControlType == CONTROL_TYPE.LINK)
                    {
                        m_Control = new LabelControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setName("Link");
                        m_Control.setText("Link");
                        m_Control.setForeground(new SolidColorBrush(new Color(255, 33, 33, 255)));
                        m_Control.setTextUnderline(true);
                        m_Control.setSize(100, 50);
                        m_Control.setFontSizeID(10);
                        
                        m_Control.setType(w_UIControlType);
                    }					
                    else if(w_UIControlType == CONTROL_TYPE.RADIO)
                    {
                        m_Control = new RadioControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setType(w_UIControlType);
                    }
					else if (w_UIControlType == CONTROL_TYPE.LISTBOX)
                    {
                        m_Control = new ListBoxControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setType(w_UIControlType);

                    }
                    else if (w_UIControlType == CONTROL_TYPE.REPEATER)
                    {
                        m_Control = new RepeaterControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setType(w_UIControlType);
                    }
                    else if (w_UIControlType == CONTROL_TYPE.TABS)
                    {
                        m_Control = new TabViewControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setType(w_UIControlType);
                    }
                    else if (w_UIControlType == CONTROL_TYPE.DROPDOWN)
                    {
                        m_Control = new DropDownControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setType(w_UIControlType);
                    }
                    else if (w_UIControlType == CONTROL_TYPE.SLIDER)
                    {
                        m_Control = new SliderControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setType(w_UIControlType);
                    }
                    else if (w_UIControlType == CONTROL_TYPE.PROGRESS)
                    {
                        m_Control = new ProgressControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setType(w_UIControlType);
                    }
                    else if (w_UIControlType == CONTROL_TYPE.TREEVIEW)
                    {
                        m_Control = new TreeViewControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setType(w_UIControlType);
                    }
                    else if (w_UIControlType == CONTROL_TYPE.CONTAINERBOX)
                    {
                        m_Control = new ContainerBoxControl();
                        m_Control.setMainVM(mainViewModel);
                        m_Control.setType(w_UIControlType);

                    }
                    if (m_Control != null)
                    {
                        if(mainViewModel.ContainerFlag > 0)
                        {
                            mainViewModel.ContainerCanvas[mainViewModel.ContainerFlag-1].Children.Add(m_Control);
                        }
                        else
                        {
                            mainViewModel.WorkScreen.screenCanvas.Children.Add(m_Control);
                        }
                        
                    }
                        
                }

                if (m_Control != null)
                {
                    
                    if (mainViewModel.ContainerFlag > 0)
                    {
                        mainViewModel.WorkScreen.LinePosition.IsVisible = false;
                        double w_CsX = 0, w_CsY = 0;
                        Rect w_CRect = m_Control.getRect();
                        w_CsX = sX - w_CRect.Width / 2;
                        w_CsY = sY - w_CRect.Height / 2;
                        m_Control.setPosition(w_CsX, w_CsY,0,0);
                    }
                    else
                    {
                        mainViewModel.WorkScreen.LinePosition.IsVisible = true;
                        double w_CsX = 0, w_CsY = 0;
                        Rect w_CRect = m_Control.getRect();
                        w_CsX = sX - w_CRect.Width / 2 -150;
                        w_CsY = sY - w_CRect.Height / 2 - 150;
                        m_Control.setPosition(w_CsX, w_CsY);
                    }
                    
                   
                }
              
            }
            
        }
        
    }
    
    private void onMouseDown(object sender, PointerEventArgs e)
    {
        MainWindowViewModel mainViewModel = (MainWindowViewModel)this.DataContext;
        CONTROL_TYPE w_UIControlType = mainViewModel.m_UIControlType;
        CONTROL_TYPE w_nSelectedUIControl = mainViewModel.m_nSelectedUIControl;
        var properties = e.GetCurrentPoint(this).Properties;
        mainViewModel.WorkScreen.LinePosition.IsVisible = false;
        //if (properties.IsLeftButtonPressed)
        {
            var sX = e.GetPosition(this.Work.Screen).X;
            var sY = e.GetPosition(this.Work.Screen).Y;
            if (w_UIControlType > 0)
            {
                if (m_Control != null)
                {
                    
                    stControlHistory w_ControlHistory = new stControlHistory();
                    w_ControlHistory.Index = mainViewModel.WorkScreen.screenCanvas.Children.Count-1;
                    w_ControlHistory.type = m_Control.GetType();
                    w_ControlHistory.Cmd = "New";
                    w_ControlHistory.id = w_UIControlType;
                    w_ControlHistory.curInfo = m_Control;
                    mainViewModel.WorkScreen.m_UndoList.Add(w_ControlHistory);
                }
            }
            else if (w_nSelectedUIControl > 0)
            {
                if (m_Control != null)
                {
                    stControlHistory w_ControlHistory = new stControlHistory();
                    w_ControlHistory.Index =(int) w_nSelectedUIControl;
                    w_ControlHistory.Cmd = "Change";
                    w_ControlHistory.type = m_Control.GetType();
                    //w_ControlHistory.id = w_UIControlType;
                    w_ControlHistory.curInfo = m_Control;
                    mainViewModel.WorkScreen.m_UndoList.Add(w_ControlHistory);
                }
            }
        }
        mainViewModel.setScreenSmallView(mainViewModel.SmallScreenID);
        m_Control = null;
    }
}