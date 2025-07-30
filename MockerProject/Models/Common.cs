using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace MockerProject.Models
{
    internal class Common
    {
    }

    public enum CONTROL_TYPE
    {
        NONE,
        AREA,
        CONTAINERBOX,
        TABS,
        TREEVIEW,
        BUTTON,
        TEXTBOX,
        LABEL,
        TITLE,
        IMAGE,
        CHECK,
        PASSWORD,
        MULTIBUTTON,
        TEXTAREA,
        LINK,
        ICON,
        HOTSPOT,
        RADIO,
        SLIDER,
        PROGRESS,

        LISTBOX,
        DROPDOWN,
        REPEATER,
        TABLE,
        LINE,
        TRIANGLE,
        RECTANGLE,
        CIRCLE,
        CUSTOMSHAPE,
        CHART,
        MAP,
        GAUGE,
        COVERFLOW,
        WEBCAM,
        VIDEOPLAYER,
        NOTE,
        CALLOUT,
        ARROW
    }

    public class DeviceInfo
    {
        public string Device { get; set; }
        public int DeviceID { get; set; }
        public int SubID { get; set; }
        public stSize size { get; set; }
        public string MainPage { get; set; }
        public int PageCount { get; set; }
        public List<string> Pages { get; set; }
    }

    public class PageInfo
    {
        public bool Orientation { get; set; }
        public stSize size { get; set; }
        public IBrush background { get; set; }
        public double Opacity { get; set; }
        public List<object> Contents { get; set; }
    }

    public class CustomItem : INotifyPropertyChanged
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

        public bool _Visible;
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                if (_Visible != value)
                {
                    _Visible = value;
                    OnPropertyChanged(nameof(Visible));
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

    public class Node : INotifyPropertyChanged
    {
        public ObservableCollection<Node> SubItems { get; set; }

        public string strNodeText { get; }
        public string strFullPath { get; }
        public Node parent;

        public Node(string _strFullPath, Node p)
        {
            strFullPath = _strFullPath;
            strNodeText = Path.GetFileName(_strFullPath);
            Visible = true;
            text = _strFullPath;
            SubItems = new ObservableCollection<Node>();
            parent = p;
        }


        public void addSubItem(Node Item)
        {
            SubItems.Add(Item);
        }
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

        public bool _Visible;
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                if (_Visible != value)
                {
                    _Visible = value;
                    OnPropertyChanged(nameof(Visible));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ControlInfo
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public CONTROL_TYPE Type { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        public string Text { get; set; }
        public double Opacity { get; set; }
        public bool isDisable { get; set; }
        public string Tooltip { get; set; }
        public IBrush Background { get; set; }
        public IBrush Foreground { get; set; }
        public IBrush BorderColor { get; set; }
        public stRect BorderThickness { get; set; }
        public stRect BorderRound { get; set; }
        public string fontFamily { get; set; }
        public bool isFitWidth { get; set; }
        public bool isFitHeight { get; set; }
        public string src { get; set; }
        public int fontSize { get; set; }
        public bool isBold { get; set; }
        public bool isItalic { get; set; }
        public string TapEvent { get; set; }
        public string DTapEvent { get; set; }
        public string HPressEvent { get; set; }
        public string SwipeLeftEvent { get; set; }
        public string SwipeRightEvent { get; set; }
        public string SwipeUpEvent { get; set; }
        public string SwipeDownEvent { get; set; }
    }

    public class ListControlInfo : ControlInfo
    {
        public List<CustomItem> Items { get; set; }
        public int SeletedIndex { get; set; }
        public IBrush ItemBackground { get; set; }
        public int itemHeight { get; set; }

    }

    public class TreeItemInfo
    {
        public List<Object> Items { get; set; }
        public CustomItem item { get; set; }
    }

    public class ContainterControlInfo : ControlInfo
    {
        public List<Object> Items { get; set; }
        //public int index { get; set; }
        //public IBrush ItemBackground { get; set; }
        //public int itemHeight { get; set; }

    }
    public class TabControlInfo : ContainterControlInfo
    {
        public List<string> Headers { get; set; }

        public int SeletedIndex { get; set; } = 0;
        //public int index { get; set; }
        //public IBrush ItemBackground { get; set; }
        //public int itemHeight { get; set; }

    }
    public class TreeViewControlInfo : ControlInfo
    {
        public List<Object> Items { get; set; }
        //public int index { get; set; }
        public IBrush ItemBackground { get; set; }
        public int itemHeight { get; set; }

    }
    public struct stSize
    {
        public int W;
        public int H;
        public stSize(int w, int h)
        {
            this.W = w;
            this.H = h;
        }
    }
    public struct stRect
    {
        public int X;
        public int Y;
        public int W;
        public int H;
        public stRect(int x, int y, int w, int h)
        {
            this.X = x;
            this.Y = y;
            this.W = w;
            this.H = h;
        }
        public stRect(Thickness thickness)
        {
            this.X = (int)thickness.Left;
            this.Y = (int)thickness.Top;
            this.W = (int)thickness.Right;
            this.H = (int)thickness.Bottom;
        }
        public stRect(CornerRadius round)
        {
            this.X = (int)round.TopLeft;
            this.Y = (int)round.TopRight;
            this.W = (int)round.BottomRight;
            this.H = (int)round.BottomLeft;
        }
        public Thickness getThickness()
        {
            return new Thickness(this.X, this.Y, this.W, this.H);
        }
        public CornerRadius GetCornerRadius()
        {
            return new CornerRadius(this.X, this.Y, this.W, this.H);
        }
    }

    public struct stPlatFormPosInfo
    {

        public string Type; //PlatForm_Type
        public List<stSize> PF_Size;//PlatForm_Size
        public int PG_X;    //page_X
        public int PG_Y;    //page_Y
        public List<stSize> PG_Size;//page_W, page_H

        public int PF_TH;    //Platform-TOP-Height
        public int PF_BH;    //Platform-BOTTOM-Height
        public int PF_LW;    //Platform-Left-Width
        public int PF_RW;    //Platform-Right-Width

        public List<stSize> L_Size;//Label_W, Label_H
        public stPlatFormPosInfo(string type, List<stSize> PF_Size, int PG_X, int PG_Y, List<stSize> PG_Size, int PF_TH, int PF_BH, int PF_LW, int PF_RW, List<stSize> L_Size)
        {
            this.Type = type;
            this.PF_Size = new List<stSize>(PF_Size);
            this.PG_X = PG_X;
            this.PG_Y = PG_Y;
            this.PG_Size = new List<stSize>(PG_Size);
            this.PF_TH = PF_TH;
            this.PF_BH = PF_BH;
            this.PF_LW = PF_LW;
            this.PF_RW = PF_RW;
            this.L_Size = new List<stSize>(L_Size);
        }
    }

    public enum EventType
    {
        EVENT_TAP,
        EVENT_DOUBLETAP,
        EVENT_PRESSHOLD,
        EVENT_SWIPELEFT,
        EVENT_SWIPERIGHT,
        EVENT_SWIPEUP,
        EVENT_SWIPEDOWN,
        EVENT_SELECTITEM,
    }
}