using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using MockerProject.Models;
using MockerProject.ViewModels.UIViewModels;
using MockerProject.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockerProject.ViewModels.UIPropertyModels
{
    internal class UIPropertyBaseModel : ViewModelBase
    {
        public UIPropertyBaseModel() {
            List_FontSize = new ObservableCollection<int>();
            List_Thickness = new ObservableCollection<int>();
            List_Round = new ObservableCollection<int>();
           

        }

       
        public ObservableCollection<int> List_Thickness { get; set; }
        public ObservableCollection<int> List_Round { get; set; }
        public ObservableCollection<int> List_FontSize { get; set; }
        private string m_strControl_Tip;
        public string Control_Tip
        {
            get => m_strControl_Tip;
            set
            {
                this.RaiseAndSetIfChanged(ref m_strControl_Tip, value);
                //m_UIControl.m_Tooltip = Control_Tip;
            }
        }//Tooltip


       

        public double m_opacity = 1;
        public double opacity
        {
            get => m_opacity;
            set
            {
                this.RaiseAndSetIfChanged(ref m_opacity, value);
                //m_UIControl.m_Opacity = opacity;
            }
        }//Opacity
        private int m_nControl_sX;
        public int Control_sX
        {
            get => m_nControl_sX;
            set { this.RaiseAndSetIfChanged(ref m_nControl_sX, value); }
        }//???
        private int m_nControl_sY;
        public int Control_sY
        {
            get => m_nControl_sY;
            set { this.RaiseAndSetIfChanged(ref m_nControl_sY, value); }
        }//???
        private int m_width;
        public int width
        {
            get => m_width;
            set { this.RaiseAndSetIfChanged(ref m_width, value); }
        } //Width
        private int m_height;
        public int height
        {
            get => m_height;
            set { this.RaiseAndSetIfChanged(ref m_height, value); }
        } //Height
        private SolidColorBrush m_background = new SolidColorBrush(new Color(0, 255, 255, 255));
        public SolidColorBrush background
        {
            get => m_background;
            set
            {
                this.RaiseAndSetIfChanged(ref m_background, value);
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
            set { this.RaiseAndSetIfChanged(ref m_borderColor, value); }
        } //Border_Color
        public string ControlName { get; set; }

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
                    borderThickness = new Thickness(List_Thickness[value], List_Thickness[value], List_Thickness[value], List_Thickness[value]);
                }
                this.RaiseAndSetIfChanged(ref w_nThicknessID, value);
            }
        }
        private Thickness m_borderThickness;
        public Thickness borderThickness
        {
            get => m_borderThickness;
            set { this.RaiseAndSetIfChanged(ref m_borderThickness, value);  }
        } //Border Thickness
     
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
                    borderRound = new CornerRadius(List_Round[value], List_Round[value], List_Round[value], List_Round[value]);
                } 
                this.RaiseAndSetIfChanged(ref w_nRoundID, value);
            }
        }
        private CornerRadius m_borderRound = new CornerRadius(3, 3, 3, 3);
        public CornerRadius borderRound
        {
            get => m_borderRound;
            set { this.RaiseAndSetIfChanged(ref m_borderRound, value); }
        } //Border Rounding
        private SolidColorBrush m_foreground = new SolidColorBrush(new Color(255, 33, 33, 33));
        public SolidColorBrush foreground
        {
            get => m_foreground;
            set => this.RaiseAndSetIfChanged(ref m_foreground, value);
        }//Foreground
       
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
                    //fontSize = List_FontSize[value];
                }
                this.RaiseAndSetIfChanged(ref w_nfontSizeID, value);
            }
        }
        private int m_fontSize = 14;
        public int fontSize
        {
            get => m_fontSize;
            set
            {
                this.RaiseAndSetIfChanged(ref m_fontSize, value); //m_UIControl.m_nFontSize = fontSize;
                //setFitSize();
            }
        }//Text_FontSize

        private FontFamily m_fontFamily = "arial";
        public FontFamily fontFamily
        {
            get => m_fontFamily;
            set { this.RaiseAndSetIfChanged(ref m_fontFamily, value);  }
        }//Text_FontFamily
        private bool m_bTextBold = false;
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
            set { this.RaiseAndSetIfChanged(ref m_FontWeight, value);  }
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
            set { this.RaiseAndSetIfChanged(ref m_FontStyle, value);  }
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
            set { this.RaiseAndSetIfChanged(ref m_TextDecorations, value);  }
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

        public int w_nPageID = 0;
        public int PageID
        {
            get => w_nPageID;
            set => this.RaiseAndSetIfChanged(ref w_nPageID, value);
        }
        public object IsTextPropertiesVisible { get; set; }
        public bool m_IsMultiItemVisible = false;

    }

    
}
