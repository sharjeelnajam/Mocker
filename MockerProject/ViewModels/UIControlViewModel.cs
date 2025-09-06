using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using MockerProject.Models;
using MockerProject.Views;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Color = Avalonia.Media.Color;
using FontFamily = Avalonia.Media.FontFamily;
using FontStyle = Avalonia.Media.FontStyle;
using Size = Avalonia.Size;

namespace MockerProject.ViewModels;

public class UIControlViewModel : ReactiveObject
{
    public UIControl m_UIControl;
    public MainWindowViewModel m_MainVM;
    private string m_strControl_Name = "Label";
    public string Control_Name
    {
        get => m_strControl_Name;
        set { this.RaiseAndSetIfChanged(ref m_strControl_Name, value); if (m_UIControl != null) m_UIControl.m_strName = Control_Name; }
    } //Text
    public string m_text = "Label";
    public string text
    {
        get => m_text;
        set { this.RaiseAndSetIfChanged(ref m_text, value); if (m_UIControl != null) m_UIControl.m_strText=text;  setFitSize();}
    }//Text

    // Slider value property for proper binding
    private double m_sliderValue = 50.0;
    public double sliderValue
    {
        get => m_sliderValue;
        set 
        { 
            m_sliderValue = Math.Round(value, 2); // Round to 2 decimal places
            this.RaiseAndSetIfChanged(ref m_sliderValue, m_sliderValue);
            
            // Update the text property with formatted value
            if (m_UIControl != null && m_UIControl.m_nUIControlType == CONTROL_TYPE.SLIDER)
            {
                m_text = m_sliderValue.ToString("F2");
                this.RaisePropertyChanged(nameof(text));
                m_UIControl.m_strText = m_text;
            }
        }
    }

    private string m_passwordChar = "";
    public string passwordChar
    {
        get => m_passwordChar;
        set { this.RaiseAndSetIfChanged(ref m_passwordChar, value); if (m_UIControl != null) m_UIControl.m_strPasswordChar = passwordChar; setFitSize(); }
    }//Text
    private bool m_isEnabled = true;
    public bool isEnabled
    {
        get => m_isEnabled;
        set => this.RaiseAndSetIfChanged(ref m_isEnabled, value);
    }//Disable
    
    private bool m_isChecked = false;
    public bool isChecked
    {
        get => m_isChecked;
        set => this.RaiseAndSetIfChanged(ref m_isChecked, value);
    }//Checked state for Radio and Checkbox controls
    private string m_strControl_Tip;
    public string Control_Tip
    {
        get => m_strControl_Tip;
        set
        {
            this.RaiseAndSetIfChanged(ref m_strControl_Tip, value);
            if (m_UIControl != null) m_UIControl.m_Tooltip = Control_Tip;
        }
    }//Tooltip
    public double m_opacity= 1;
    public double opacity
    {
        get => m_opacity;
        set
        {
            this.RaiseAndSetIfChanged(ref m_opacity, value);
            if (m_UIControl != null) m_UIControl.m_Opacity = opacity;
        }
    }//Opacity
    private int m_nControl_sX;
    public int Control_sX
    {
        get => m_nControl_sX;
        set { this.RaiseAndSetIfChanged(ref m_nControl_sX, value); setLinePosition(); }
    }//???
    private int m_nControl_sY;
    public int Control_sY
    {
        get => m_nControl_sY;
        set { this.RaiseAndSetIfChanged(ref m_nControl_sY, value); setLinePosition(); }
    }//???
    

    private int m_nOffsetX;
    public int OffsetX
    {
        get => m_nOffsetX;
        set { this.RaiseAndSetIfChanged(ref m_nOffsetX, value); }
    }//???

    private int m_nOffsetY;
    public int OffsetY
    {
        get => m_nOffsetY;
        set { this.RaiseAndSetIfChanged(ref m_nOffsetY, value); }
    }//???


    private int m_width;
    public int width
    {
        get => m_width;
        set { this.RaiseAndSetIfChanged(ref m_width, value); if (m_UIControl != null) m_UIControl.m_nWidth = width; }
    } //Width
    private int m_height;
    public int height
    {
        get => m_height;
        set { this.RaiseAndSetIfChanged(ref m_height, value); if (m_UIControl != null) m_UIControl.m_nHeight = m_height; }
    } //Height


    private SolidColorBrush m_itemBackground = new SolidColorBrush(new Color(0, 255, 255, 255));
    public SolidColorBrush itemBackground
    {
        get => m_itemBackground;
        set
        {
            this.RaiseAndSetIfChanged(ref m_itemBackground, value);
            //backgroundOver = new ImmutableSolidColorBrush(m_itemBackground);
        }
    } //ItemBackground

    private bool m_iterationVisible = false;
    public bool IterationVisible
    {
        get => m_iterationVisible;
        set
        {
            this.RaiseAndSetIfChanged(ref m_iterationVisible, value);
           
        }
    }//IterationVisible

    private SolidColorBrush m_background = new SolidColorBrush(new Color(0, 255, 255, 255));
    public SolidColorBrush background
    {
        get => m_background;
        set { this.RaiseAndSetIfChanged(ref m_background, value);
            backgroundOver = new ImmutableSolidColorBrush(background);
        }
    } //Background



    public IImmutableSolidColorBrush m_backgroundOver = new ImmutableSolidColorBrush(new Color(0, 255, 255, 255));
    public IImmutableSolidColorBrush backgroundOver
    {
        get => m_backgroundOver;
        set { this.RaiseAndSetIfChanged(ref m_backgroundOver, value); }
    } //Background
    private SolidColorBrush m_borderColor = new SolidColorBrush(new Color(255, 77, 77, 77));
    public SolidColorBrush borderColor
    {
        get => m_borderColor;
        set { this.RaiseAndSetIfChanged(ref m_borderColor, value);}
    } //Border_Color
    public ObservableCollection<int> List_Thickness { get; set; }
    public int w_nThicknessID = 0;
    public int ThicknessID
    {
        get
        {
            return w_nThicknessID;
        }
        set
        {
            if (value > -1)
            {
                borderThickness = new Thickness(List_Thickness[value],List_Thickness[value],List_Thickness[value],List_Thickness[value]);
            }
            this.RaiseAndSetIfChanged(ref w_nThicknessID, value);
        }
    }
    private Thickness m_borderThickness;
    public Thickness borderThickness
    {
        get => m_borderThickness;
        set { this.RaiseAndSetIfChanged(ref m_borderThickness, value); if (m_UIControl != null) m_UIControl.m_BorderThickness = borderThickness; }
    } //Border Thickness
    public ObservableCollection<int> List_Round { get; set; }
    public int w_nRoundID = 0;
    public int RoundID
    {
        get
        {
            return w_nRoundID;
        }
        set
        {
            if (value > -1)
            {
                borderRound = new CornerRadius(List_Round[value],List_Round[value],List_Round[value],List_Round[value]);
            }
            this.RaiseAndSetIfChanged(ref w_nRoundID, value);
        }
    }
    private CornerRadius m_borderRound = new CornerRadius(3, 3, 3, 3);
    public CornerRadius borderRound
    {
        get => m_borderRound;
        set { this.RaiseAndSetIfChanged(ref m_borderRound, value); if (m_UIControl != null) m_UIControl.m_BorderRound = borderRound; }
    } //Border Rounding
    private SolidColorBrush m_foreground = new SolidColorBrush(new Color(255, 33, 33, 33));
    public SolidColorBrush foreground
    {
        get => m_foreground;
        set => this.RaiseAndSetIfChanged(ref m_foreground, value);
    }//Foreground
    public ObservableCollection<int> List_FontSize { get; set; }
    public int w_nfontSizeID = 0;
    public int fontSizeID
    {
        get
        {
            return w_nfontSizeID;
        }
        set
        {
            if (value > -1)
            {
                fontSize = List_FontSize[value];
            }
            this.RaiseAndSetIfChanged(ref w_nfontSizeID, value);
        }
    }
    private int m_fontSize = 14 ;
    public int  fontSize
    {
        get => m_fontSize;
        set { 
            this.RaiseAndSetIfChanged(ref m_fontSize, value); if (m_UIControl != null) m_UIControl.m_nFontSize = fontSize; 
            setFitSize(); }
    }//Text_FontSize
    
    private FontFamily m_fontFamily = "arial";
    public FontFamily fontFamily
    {
        get => m_fontFamily;
        set { this.RaiseAndSetIfChanged(ref m_fontFamily, value); if (m_UIControl != null) m_UIControl.m_FontFamily = fontFamily; setFitSize(); }
    }//Text_FontFamily
    private bool m_bTextBold= false;
    public bool textBold
    {
        get => m_bTextBold;
        set
        {
            if (value)
                fontWeight = FontWeight.Bold;
            else
                fontWeight = FontWeight.Normal;
            this.RaiseAndSetIfChanged(ref m_bTextBold, value);
        }
    } //Text_Bold
    public FontWeight m_FontWeight = FontWeight.Normal;
    public FontWeight fontWeight
    {
        get => m_FontWeight;
        set { this.RaiseAndSetIfChanged(ref m_FontWeight, value); setFitSize(); }
    }//FontWeight
    private bool m_bTextItalic = false;
    public bool textItalic
    {
        get => m_bTextItalic;
        set
        {
            if (value)
                fontStyle = FontStyle.Italic;
            else
                fontStyle = FontStyle.Normal;
            this.RaiseAndSetIfChanged(ref m_bTextItalic, value);
        }
    } //Text_Italic

    public FontStyle m_FontStyle = FontStyle.Normal;
    public FontStyle fontStyle
    {
        get => m_FontStyle;
        set { this.RaiseAndSetIfChanged(ref m_FontStyle, value); setFitSize(); }
    }//FontStyle


    private bool m_bTextUnderline = false;
    public bool textUnderline
    {
        get => m_bTextUnderline;
        set
        {
            if (value)
                textDecorations = TextDecorations.Underline;
            else
                textDecorations = null;
            this.RaiseAndSetIfChanged(ref m_bTextItalic, value);
        }
    } //Text_Underline

    public TextDecorationCollection m_TextDecorations = null;
    public TextDecorationCollection textDecorations
    {
        get => m_TextDecorations;
        set { this.RaiseAndSetIfChanged(ref m_TextDecorations, value); setFitSize(); }
    }//FontStyle


    public bool m_IsFitVisible = false;
    public bool IsFitVisible
    {
        get => m_IsFitVisible;
        set => this.RaiseAndSetIfChanged(ref m_IsFitVisible, value);
    }//IsVisible_Fit

    public bool m_IsMultiEnable = false;
    public bool IsMultiEnable
    {
        get => m_IsMultiEnable;
        set => this.RaiseAndSetIfChanged(ref m_IsMultiEnable, value);
    }//IsVisible_Fit

    public bool m_IsTextMultiEnable = false;
    public bool IsTextMultiEnable
    {
        get => m_IsTextMultiEnable;
        set => this.RaiseAndSetIfChanged(ref m_IsTextMultiEnable, value);
    }//IsVisible_Fit
    public bool m_IsBorderVisible = true;
    public bool IsBorderVisible
    {
        get => m_IsBorderVisible;
        set => this.RaiseAndSetIfChanged(ref m_IsBorderVisible, value);
    }//IsVisible_Border
    public bool m_IsBorderColorVisible = true;
    public bool IsBorderColorVisible
    {
        get => m_IsBorderColorVisible;
        set => this.RaiseAndSetIfChanged(ref m_IsBorderColorVisible, value);
    }//IsVisible_BorderColor
    public bool m_IsBackgroundVisibel = true;
    public bool IsBackgroundVisible
    {
        get => m_IsBackgroundVisibel;
        set => this.RaiseAndSetIfChanged(ref m_IsBackgroundVisibel, value);
    }//IsVisible_BorderColor
    public bool m_IsFitWidth = false;
    public bool IsFitWidth
    {
        get => m_IsFitWidth;
        set => this.RaiseAndSetIfChanged(ref m_IsFitWidth, value);
    }//IsEnable_FitWidth
    public bool m_IsFitHeight = false;
    public bool IsFitHeight
    {
        get => m_IsFitHeight;
        set => this.RaiseAndSetIfChanged(ref m_IsFitHeight, value);
    }//IsEnable_FitHeight
    public bool m_ReadOnlyHeight = false;
    public bool ReadOnlyHeight
    {
        get => m_ReadOnlyHeight;
        set => this.RaiseAndSetIfChanged(ref m_ReadOnlyHeight, value);
    }//IsReadOnly_Height

    private int _SelectedIterationIndex;
    public int SelectedIterationIndex
    {
        get { return _SelectedIterationIndex; }
        set { this.RaiseAndSetIfChanged(ref _SelectedIterationIndex, value);  }
    }// Selected Iteration Index

    
  


    public int w_nPageID = 0;
    public int PageID
    {
        get => w_nPageID;
        set => this.RaiseAndSetIfChanged(ref w_nPageID, value);
    }

    public object IsTextPropertiesVisible { get; set; }
    public bool m_IsMultiItemVisible = false;
    public bool IsMultiItemVisible { get=>m_IsMultiItemVisible; set => this.RaiseAndSetIfChanged(ref m_IsMultiItemVisible, value); }
    public ObservableCollection<IterationItem> iterationItems { get; set; }


    public void setMainVM(MainWindowViewModel mainVM)
    {
        m_MainVM = mainVM;
    }
    private int[] m_listTextSize = new int[]{6,7,8,9,10,11,12,14,16,18,21,24,36,48,60,72};
    public UIControlViewModel(UIControl uiControl)
    {
        m_UIControl = uiControl;
        List_FontSize =new ObservableCollection<int>();
        List_Thickness =new ObservableCollection<int>();
        List_Round =new ObservableCollection<int>();
        
        // Initialize position properties with default values
        m_nControl_sX = (int)uiControl.m_nPositionX;
        m_nControl_sY = (int)uiControl.m_nPositionY;
        
        //if (uiControl.GetType() == typeof(LabelControl))
        {
            for (int i = 0; i < m_listTextSize.Length; i++)
            {
                List_FontSize.Add(m_listTextSize[i]);
            }
            fontSizeID = 7;
            for (int i = 0; i < 20; i++)
            {
                List_Thickness.Add(i);
                List_Round.Add(i);
            }
            ThicknessID = 0;
            RoundID = 0;
        }

        iterationItems = new ObservableCollection<IterationItem>();

        IterationItem item = new IterationItem
        {
            text = "Tapped",
            type = EventType.EVENT_TAP,
            iteration = "None",

        };
        iterationItems.Add(item);

        item = new IterationItem
        {
            text = "Double Tapped",
            type = EventType.EVENT_DOUBLETAP,
            iteration = "None",

        };
        iterationItems.Add(item);
        item = new IterationItem
        {
            text = "Presses and Holds",
            type = EventType.EVENT_PRESSHOLD,
            iteration = "None",

        };
        iterationItems.Add(item);
        item = new IterationItem
        {
            text = "SwipeLeft",
            type = EventType.EVENT_SWIPELEFT,
            iteration = "None",

        };
        iterationItems.Add(item);
        item = new IterationItem
        {
            text = "SwipeRight",
            type = EventType.EVENT_SWIPERIGHT,
            iteration = "None",

        };
        iterationItems.Add(item);
        item = new IterationItem
        {
            text = "SwipeUp",
            type = EventType.EVENT_SWIPEUP,
            iteration = "None",

        };
        iterationItems.Add(item);
        item = new IterationItem
        {
            text = "SwipeDown",
            type = EventType.EVENT_SWIPEDOWN,
            iteration = "None",

        };
        iterationItems.Add(item);
    }
    public void setLinePosition()
    {
        if (m_MainVM == null || m_MainVM.WorkScreen == null) return;
        
        m_MainVM.WorkScreen.lineY.StartPoint = new Avalonia.Point(Control_sX+OffsetX, 0);
        m_MainVM.WorkScreen.lineY.EndPoint = new Avalonia.Point(Control_sX + OffsetX, Control_sY+OffsetY);
        m_MainVM.WorkScreen.PosY.Text = (Control_sY ).ToString();
        Canvas.SetTop(m_MainVM.WorkScreen.PosY, Control_sY + OffsetY - 30);
        Canvas.SetLeft(m_MainVM.WorkScreen.PosY, Control_sX + OffsetX - 7);
        m_MainVM.WorkScreen.lineX.StartPoint = new Avalonia.Point(0, Control_sY + OffsetY);
        m_MainVM.WorkScreen.lineX.EndPoint = new Avalonia.Point(Control_sX + OffsetX, Control_sY + OffsetY);
        m_MainVM.WorkScreen.PosX.Text = (Control_sX ).ToString();
        Canvas.SetTop(m_MainVM.WorkScreen.PosX, Control_sY + OffsetY - 5);
        Canvas.SetLeft(m_MainVM.WorkScreen.PosX, Control_sX + OffsetX - 30);

        if(Control_sX+width/2-150 == m_MainVM.PG_RW / 2)
        {
            m_MainVM.WorkScreen.align1.IsVisible = true;
            m_MainVM.WorkScreen.align1.StartPoint = new Avalonia.Point(Control_sX + width / 2,0);
            m_MainVM.WorkScreen.align1.EndPoint = new Avalonia.Point(Control_sX + width / 2, m_MainVM.PG_RH);
        }
        else
        {
            m_MainVM.WorkScreen.align1.IsVisible = false;
        }
        if (Control_sY + height / 2 - 150 == m_MainVM.PG_RH / 2)
        {
            m_MainVM.WorkScreen.align2.IsVisible = true;
            m_MainVM.WorkScreen.align2.StartPoint = new Avalonia.Point(0, Control_sY + height / 2);
            m_MainVM.WorkScreen.align2.EndPoint = new Avalonia.Point(m_MainVM.PG_RW , Control_sY + height / 2);
        }
        else
        {
            m_MainVM.WorkScreen.align2.IsVisible = false;
        }
    }
    public void setFitSize()
    {
        TextBlock w_text = new TextBlock();
        w_text.Text = m_text;
        w_text.FontFamily = m_fontFamily;
        w_text.FontSize = m_fontSize;
        w_text.FontWeight = m_FontWeight;
        w_text.FontStyle = m_FontStyle;
        w_text.TextDecorations = m_TextDecorations;
        w_text.TextWrapping = TextWrapping.Wrap;
        w_text.Width = double.NaN;
        w_text.Measure(Size.Infinity);
        if (ReadOnlyHeight)
        {
            m_UIControl.m_nHeight = m_UIControl.DesiredSize.Height;
        }
        else
        {
            if(IsFitWidth)
            {
                m_UIControl.m_nWidth = w_text.DesiredSize.Width;
                width = (int)m_UIControl.m_nWidth;
                if (m_MainVM.m_wndUIProperty != null)
                    m_MainVM.m_wndUIProperty.Size_W.Text = m_UIControl.m_nWidth.ToString();
            }
            if (IsFitHeight && IsFitWidth)
            {
                m_UIControl.m_nHeight = w_text.DesiredSize.Height;
                height = (int)m_UIControl.m_nHeight;
                if (m_MainVM.m_wndUIProperty != null)
                {
                    m_MainVM.m_wndUIProperty.Size_H.Text = m_UIControl.m_nHeight.ToString();
                    m_MainVM.m_wndUIProperty.Size_W.Text = m_UIControl.m_nWidth.ToString();
                }
            }
            else if (IsFitHeight)
            {
                w_text.Width = m_width;
                w_text.Height = double.NaN;
                w_text.Measure(Size.Infinity);
                m_UIControl.m_nHeight= w_text.DesiredSize.Height;
                height = (int)m_UIControl.m_nHeight;
                if (m_MainVM.m_wndUIProperty != null)
                    m_MainVM.m_wndUIProperty.Size_H.Text = m_UIControl.m_nHeight.ToString();
            }
        }
        //TextReader.MeasureText(m_text, new Font(m_fontFamily.ToString(), m_fontSize));
    }

    public class IterationItem : INotifyPropertyChanged
    {
        public string _text { get; set; }
        public string text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(text));
                }
            }
        }

        public EventType _type { get; set; }
        public EventType type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged(nameof(type));
                }
            }
        }

        public string _iteration = "";
        public string iteration
        {
            get { return _iteration; }
            set
            {
                if (_iteration != value)
                {
                    _iteration = value;
                    OnPropertyChanged(nameof(iteration));
                }
            }
        }

        // Add more properties as needed
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}