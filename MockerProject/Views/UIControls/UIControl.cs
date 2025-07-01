using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using MockerProject.ViewModels;
using MockerProject.Views.UIControls;
using MockerProject.Models;
using System.Security.Cryptography.X509Certificates;
using Avalonia.Input;

namespace MockerProject.Views;

public class UIControl : UserControl
{
    public MainWindowViewModel m_MainViewModel;
    [AllowNull]  public UIControlViewModel m_ControlViewModel;
    public CONTROL_TYPE m_nUIControlType = CONTROL_TYPE.NONE;
    public int m_nIndex = 0;
    public string m_strName;
    public string m_strText;
    public string m_strPasswordChar;
    public double m_Opacity = 1.0;
    public double m_nPositionX;
    public double m_nPositionY;
    public double m_nOffsetX;
    public double m_nOffsetY;
    public double m_nWidth;
    public double m_nHeight;
    public string m_Tooltip;
    public bool m_bFitWidth;
    public bool m_bFitHeight;
    public SolidColorBrush m_Background;
    
    public SolidColorBrush m_Foreground;
    public SolidColorBrush m_BorderColor;
    public Thickness m_BorderThickness;
    public CornerRadius m_BorderRound;
    public FontFamily m_FontFamily = "arial";
    public int m_nFontSize;
    public bool m_bBold;
    public bool m_bItalic;
    public bool m_bUnderline;
    public bool m_bDisable;
    public string m_strSrc;
    public string m_TapEvent;
    public string m_DTapEvent;
    public string m_HPressEvent;
    public string m_SwipeLeftEvent;
    public string m_SwipeRightEvent;
    public string m_SwipeUpEvent;
    public string m_SwipeDownEvent;
    
    public bool m_IsDoubleTapped = false;
    public List<string> subItems = new List<string>();
    public double sX;
    public double sY;
    public void setType(CONTROL_TYPE uiType)
    {
        m_nUIControlType = uiType;
    }
    public void setName(string name)
    {
        m_strName = name;
        m_ControlViewModel.Control_Name = m_strName;
    }
    public void setText(string text)
    {
        m_strText = text;
        m_ControlViewModel.text = m_strText;
            
    }

    public void setPasswordChar(string passwordChar)
    {
        m_strPasswordChar = passwordChar;
        m_ControlViewModel.passwordChar = m_strPasswordChar;

    }

    
    public void setOpacity(double opacity)
    {
        m_Opacity = opacity;
        m_ControlViewModel.opacity = m_Opacity;
    }
    public void setTooltip(string text)
    {
        m_Tooltip = text;
    }
    public void setPositionX(double x)
    {
        m_nPositionX = x;
        Canvas.SetLeft(this, m_nPositionX+ m_ControlViewModel.OffsetX);
       
       // m_ControlViewModel.Control_sX = (int)m_nPositionX ;
    }
    public void setPositionY(double y )
    {
        m_nPositionY = y;
        Canvas.SetTop(this, m_nPositionY+ m_ControlViewModel.OffsetY);
        
        //m_ControlViewModel.Control_sY = (int)m_nPositionY ;
    }
    public void setPosition(double x, double y,double ox=150,double oy=150)
    {
        //m_currentControlPoint = new Point(x, y);
        m_nPositionX = (int)x;
        m_nPositionY = (int)y;
        m_ControlViewModel.OffsetX = (int)ox;
        m_ControlViewModel.OffsetY = (int)oy;
        Canvas.SetLeft(this,(int) x + m_ControlViewModel.OffsetX);
        Canvas.SetTop(this,(int) y + m_ControlViewModel.OffsetY);
        m_ControlViewModel.Control_sX = (int)x;
        m_ControlViewModel.Control_sY = (int)y;
    }
    public void setWidth(double width)
    {
        m_nWidth = width;
        m_ControlViewModel.width = (int)m_nWidth;
    }
    public void setHeight(double height)
    {
        m_nHeight = height;
        m_ControlViewModel.height = (int)m_nHeight;
    }
    public void setSize(double width, double height)
    {
        //m_currentControlSize = new Size(width, height);
        m_nWidth = width;
        m_nHeight = height;
        m_ControlViewModel.width = (int)m_nWidth;
        m_ControlViewModel.height = (int)m_nHeight;
        Canvas.SetLeft(this, m_nPositionX);
        Canvas.SetTop(this, m_nPositionY);
    }

    public void setFitWidth(bool fitWidth)
    {
        m_bFitWidth = fitWidth;
        m_ControlViewModel.IsFitWidth = fitWidth;
    }
    public void setFitHeight(bool fitHeight)
    {
        m_bFitHeight = fitHeight;
        m_ControlViewModel.IsFitHeight = fitHeight;
    }

    public void setImageSrc([AllowNull]string dirPath, [AllowNull] string absPath)
    {
        m_strSrc = null;
        if(dirPath != null && absPath != null)
        {
            m_strSrc = Path.Combine(dirPath, absPath);
            if (this.GetType() == typeof(ImageControl))
                ((ImageControl)this).setImage(m_strSrc);
        }
    }
    public void setBackground(SolidColorBrush color)
    {
        m_Background = color;
        m_ControlViewModel.background = color;
    }

    public void setMultiEnable(bool flag)
    {
        m_ControlViewModel.IsMultiEnable = flag;
    }
    public void setTextMultiEnable(bool flag)
    {
        m_ControlViewModel.IsTextMultiEnable = flag;
    }
    public void setBorderColor(SolidColorBrush color)
    {
        m_BorderColor = color;
        m_ControlViewModel.borderColor = color;
    }
    public void setBorderThickness(int thickness)
    {
        m_ControlViewModel.ThicknessID = thickness;
    }
    public void setBorderThickness(stRect thickness)
    {
        setBorderThickness(thickness.X);
        //m_BorderThickness = thickness.getThickness();
        //m_ControlViewModel.borderThickness = m_BorderThickness;
    }
    public void setBorderRound(int round)
    {
       m_ControlViewModel.RoundID = round;
    }
    public void setBorderRound(stRect round)
    {
        setBorderRound(round.X);
        //m_BorderRound = round.GetCornerRadius();
        //m_ControlViewModel.borderRound = m_BorderRound;
    }
    public void setForeground(SolidColorBrush color)
    {
        m_Foreground = color;
        m_ControlViewModel.foreground = color;
    }
    public void setFontSizeID(int fontSizeID)
    {
        int w_nFontSizeID = 0;
        if (m_ControlViewModel.List_FontSize.Count > fontSizeID)
        {
            w_nFontSizeID = fontSizeID;
        }
        m_ControlViewModel.fontSizeID = w_nFontSizeID;
        m_nFontSize = m_ControlViewModel.List_FontSize[w_nFontSizeID];
        m_ControlViewModel.fontSize = m_nFontSize;
    }
    public void setFontSize(int fontSize)
    {
        int id = 0;
        foreach (var size in m_ControlViewModel.List_FontSize)
        {
            if (fontSize == size)
                break;
            id++;
        }
        setFontSizeID(id);
        //m_nFontSize = fontSize;
        //m_ControlViewModel.fontSize = m_nFontSize;
    }
    public void setFontFamily(FontFamily fontFamily)
    {
        m_FontFamily = fontFamily;
        m_ControlViewModel.fontFamily = m_FontFamily;
    }
    public void setTextBold(bool isBold)
    {
        m_bBold = isBold;
        m_ControlViewModel.textBold = m_bBold;
    }
    public void setTextItalic(bool isItalic)
    {
        m_bItalic = isItalic;
        m_ControlViewModel.textItalic = m_bItalic;
    }

    public void setTextUnderline(bool isUnderline)
    {
        m_bUnderline = isUnderline;
        m_ControlViewModel.textUnderline = m_bUnderline;
    }

    

    public void setTapEvent(string page)
    {
        m_TapEvent = page;
    }
    public void setDTapEvent(string page)
    {
        m_DTapEvent = page;
    }
    public void setHPressEvent(string page)
    {
        m_HPressEvent = page;
    }
    public void setSwipeLeftEvent(string page)
    {
        m_SwipeLeftEvent = page;
    }
    public void setSwipeRightEvent(string page)
    {
        m_SwipeRightEvent = page;
    }
    public void setSwipeUpEvent(string page)
    {
        m_SwipeUpEvent = page;
    }
    public void setSwipeDownEvent(string page)
    {
        m_SwipeDownEvent = page;
    }
    public void setDisable(bool disable)
    {
        m_bDisable = disable;
        m_ControlViewModel.isEnabled = !disable;
    }
    public Rect getRect()
    {
        Rect w_Rect=new Rect(m_nPositionX, m_nPositionY, m_nWidth, m_nHeight);
        return w_Rect;
    }
    public void setMainVM(MainWindowViewModel mainViewModel)
    {
        if (mainViewModel != null)
        {
            m_MainViewModel = mainViewModel;
            m_nIndex = m_MainViewModel.WorkScreen.screenCanvas.Children.Count;
            m_ControlViewModel.setMainVM(mainViewModel);
        }
    }
    public virtual void doubleClickHandler(object sender,TappedEventArgs e)
    {
        if (m_MainViewModel.ContainerFlag == 0 && this.Parent != m_MainViewModel.WorkScreen.screenCanvas) return;
        if (m_MainViewModel.ContainerFlag > 0 && this.Parent != m_MainViewModel.ContainerCanvas[m_MainViewModel.ContainerFlag-1]) return;

        m_MainViewModel.WorkScreen.LinePosition.IsVisible = false;
        if (m_IsDoubleTapped == true)
        {
            m_IsDoubleTapped = false;
            m_MainViewModel.m_wndUIProperty.Hide();
            return;
        }
        m_MainViewModel.m_nSelectedUIControl = 0;
        if (m_MainViewModel.m_wndUIProperty == null || !m_MainViewModel.m_wndUIProperty.IsVisible)
            m_MainViewModel.m_wndUIProperty = new UIPropertyWindow();
        else
        {
            m_MainViewModel.m_wndUIProperty.Close();
            m_MainViewModel.m_wndUIProperty = new UIPropertyWindow();
        }
        Point cP = e.GetPosition(this);
        Point mP = e.GetPosition(m_MainViewModel.m_MainWindow);
        PixelPoint cPP = new PixelPoint((int)(mP.X - cP.X + m_nWidth), (int)(mP.Y - cP.Y));
        PixelPoint nPP = m_MainViewModel.m_MainWindow.Position;
        m_MainViewModel.m_wndUIProperty.Position = nPP + cPP;
        m_MainViewModel.m_wndUIProperty.setMainViewModel(m_MainViewModel);
        m_MainViewModel.m_wndUIProperty.setControlInfo(m_ControlViewModel, this);
        m_MainViewModel.m_wndUIProperty.Show();
    }

    public virtual void MousePressEvent(object sender, PointerEventArgs e)
    {
        if (m_MainViewModel == null) return;
        m_MainViewModel.WorkScreen.LinePosition.IsVisible = false;
        sX = e.GetPosition(this).X;
        sY = e.GetPosition(this).Y;
        //if (m_MainViewModel.ContainerFlag > 0)
        //{
        //    for (var i = 0; i < m_MainViewModel.ContainerFlag; i++)
        //    {
        //        sX += Canvas.GetLeft(m_MainViewModel.ContainerCanvas[i].Parent);
        //        sY += Canvas.GetTop(m_MainViewModel.ContainerCanvas[i].Parent);
        //    }

        //}
        if (m_MainViewModel.ContainerFlag > 0)
        {
            var ttv = m_MainViewModel.ContainerCanvas[m_MainViewModel.ContainerFlag - 1].TransformToVisual(m_MainViewModel.WorkScreen.screenCanvas);
            Point screenCoords = (new Point(0, 0)).Transform((Matrix)ttv);
            sX += screenCoords.X;
            sY += screenCoords.Y;
        }
        m_MainViewModel.m_nSelectedUIControl = (CONTROL_TYPE)m_nIndex;
    }

    public virtual void MouseMoveEvent(object sender, PointerEventArgs e)
    {

        var properties = e.GetCurrentPoint(this).Properties;
        if (m_MainViewModel == null) return;
        if (m_MainViewModel.ContainerFlag == 0 && this.Parent != m_MainViewModel.WorkScreen.screenCanvas) return;
        if (properties.IsLeftButtonPressed )
        {

            m_MainViewModel.WorkScreen.LinePosition.IsVisible = true;
            if (m_MainViewModel.ContainerFlag > 0) m_MainViewModel.WorkScreen.LinePosition.IsVisible = false;
            var X = e.GetPosition(m_MainViewModel.WorkScreen).X;
            var Y = e.GetPosition(m_MainViewModel.WorkScreen).Y;

            double w_CsX = 0, w_CsY = 0;
            w_CsX = X - sX - m_ControlViewModel.OffsetX;
            w_CsY = Y - sY - m_ControlViewModel.OffsetY;
            if( m_MainViewModel.ContainerFlag>0) setPosition(w_CsX, w_CsY,0,0);
            else setPosition(w_CsX, w_CsY);
        }
    }

    public virtual void KeyPressEvent(object sender, KeyEventArgs e)
    {
        
    }

    public UIControl()
    {
        m_ControlViewModel = new UIControlViewModel(this);
        this.AddHandler(Control.PointerPressedEvent, (sender, e) =>
        {
            MousePressEvent(sender, e);
        }, handledEventsToo: true);

        this.AddHandler(Control.PointerMovedEvent, (sender, e) =>
        {
            MouseMoveEvent(sender, e);
        }, handledEventsToo: true);
       
        this.AddHandler(Control.DoubleTappedEvent, (sender, e) =>
        {
            doubleClickHandler(sender,e);

        }, handledEventsToo: true);

        this.AddHandler(Control.KeyDownEvent, (sender, e) =>
        {
            KeyPressEvent(sender, e);

        }, handledEventsToo: true);
    }
}