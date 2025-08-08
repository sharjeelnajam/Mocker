using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.Input;
using MockerProject.Action;
using MockerProject.Models;
using MockerProject.Services;
using MockerProject.ViewModels.UIViewModels;
using MockerProject.Views;
using MockerProject.Views.UIControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

//using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Color = Avalonia.Media.Color;
using Path = System.IO.Path;

namespace MockerProject.ViewModels
{

    public class MainWindowViewModel : ReactiveObject
    {
        private bool _isDarkMode = true;
        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                this.RaiseAndSetIfChanged(ref _isDarkMode, value);
                ThemeChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<bool>? ThemeChanged;

        public CONTROL_TYPE m_UIControlType = CONTROL_TYPE.NONE;
        public CONTROL_TYPE m_nSelectedUIControl = CONTROL_TYPE.NONE;
        public UIPropertyWindow m_wndUIProperty;
        public PlatformView m_PlatformView;
        private SaveProjectWindow m_SaveWindow;

        private Stack<IAction> _undoStack = new Stack<IAction>();
        private Stack<IAction> _redoStack = new Stack<IAction>();

        public void ExecuteAction(IAction action)
        {
            action.Execute();
            _undoStack.Push(action);
            _redoStack.Clear();
        }

        public void Undo()
        {
            //if (_undoStack.Any())
            //{
            //    var action = _undoStack.Pop();
            //    action.UnExecute();
            //    _redoStack.Push(action);
            //}
            int w_Count = WorkScreen.m_UndoList.Count;
            if (w_Count > 0)
            {
                stControlHistory w_ControlHistory = WorkScreen.m_UndoList[w_Count - 1];
                string w_Cmd = WorkScreen.m_UndoList[w_Count - 1].Cmd;
                int w_Index = WorkScreen.m_UndoList[w_Count - 1].Index;

                if (w_Cmd == "New")
                {
                    Control w_Control = WorkScreen.screenCanvas.Children[w_Index];
                    WorkScreen.screenCanvas.Children.Remove(w_Control);
                    WorkScreen.m_UndoList.Remove(w_ControlHistory);
                }
                WorkScreen.m_RedoList.Add(w_ControlHistory);
                if (ContainerFlag > 0)
                    ContainerFlag = 0;
            }
        }

        public void Redo()
        {
            //if (_redoStack.Any())
            //{
            //    var action = _redoStack.Pop();
            //    action.Execute();
            //    _undoStack.Push(action);
            //}

            int w_Count = WorkScreen.m_RedoList.Count;
            if (w_Count > 0)
            {
                stControlHistory w_ControlHistory = WorkScreen.m_RedoList[w_Count - 1];
                string w_Cmd = WorkScreen.m_RedoList[w_Count - 1].Cmd;
                int w_Index = WorkScreen.m_RedoList[w_Count - 1].Index;
                if (w_Cmd == "New")
                {
                    Control w_Control = w_ControlHistory.curInfo;
                    WorkScreen.screenCanvas.Children.Insert(w_Index, w_Control);
                    WorkScreen.m_RedoList.Remove(w_ControlHistory);
                }
                WorkScreen.m_UndoList.Add(w_ControlHistory);
            }
        }

        public bool m_IsProjectPath = false;

        public int w_nRunWindowWidth = 1000;
        public int RunWindowWidth
        {
            get => w_nRunWindowWidth;
            set => this.RaiseAndSetIfChanged(ref w_nRunWindowWidth, value);
        }
        public int w_nRunWindowHeight = 1000;
        public int RunWindowHeight
        {
            get => w_nRunWindowHeight;
            set => this.RaiseAndSetIfChanged(ref w_nRunWindowHeight, value);
        }

        public int ContainerFlag = 0;

        public ObservableCollection<Canvas> ContainerCanvas = new ObservableCollection<Canvas>();

        public MainWindow m_MainWindow;

        public int m_nSelectedPlatFormIndex = 0;
        public List<ScreenView> m_lstWorkScreen = new List<ScreenView>();
        public List<stPlatFormPosInfo> m_PlatFormInfo = new List<stPlatFormPosInfo>();

        public ProjectTaskbarView m_ProjectTaskbarView = null;
        public ObservableCollection<ScreenSmallView> m_ScreenSmallView = new ObservableCollection<ScreenSmallView>();

        public bool m_IsScreenVisible = false;
        public ScreenView m_WorkScreen = null;

        //private SampleViewModel CurrentSample;
        private IStorageFile? _openCodeFile;
        public ICommand onMenuOpen { get; }
        public ICommand onMenuClose { get; }
        public ICommand onNewProject { get; }
        public ICommand onSetProjectPath { get; }
        public ICommand onSave { get; }
        public ICommand onSaveCancel { get; }
        public ICommand onCloseProject { get; }
        public ICommand onSaveProject { get; }
        public ICommand onSaveAllProject { get; }
        public ICommand onOpenProject { get; }
        public ICommand onSearchProject { get; }
        public ICommand onNewScreen { get; }
        public ICommand onDeleteScreen { get; }
        public ICommand onUndo { get; }
        public ICommand onRedo { get; }
        public ICommand onRun { get; }
        public ICommand onShare { get; }
        public ICommand onSmallCanvas { get; }
        public ICommand onPlatForm1 { get; }
        public ICommand onPlatForm2 { get; }
        public ICommand onPlatForm3 { get; }
        public ICommand onPlatForm4 { get; }
        public ICommand onPlatForm5 { get; }
        public ICommand onPlatForm6 { get; }
        public ICommand onPlatForm7 { get; }
        public ICommand onHorViewPort { get; }
        public ICommand onVerViewPort { get; }

        public ObservableCollection<RecentProject> RecentProjects { get; } = new();

        public bool w_IsStartMockerState = true;
        public bool w_IsMenuOpenState = false;
        public bool w_IsProjectState = false;
        public bool w_IsWorkView = false;
        public bool w_IsToolbarView = false;

        /// <ScreenView>
        public double w_nPG_OPT = 0.33; public double PG_OPT { get => w_nPG_OPT; set { WorkScreen.m_Opacity = value; this.RaiseAndSetIfChanged(ref w_nPG_OPT, value); } }
        public int w_nPG_X = 150; public int PG_X { get => w_nPG_X; set => this.RaiseAndSetIfChanged(ref w_nPG_X, value); }
        public int w_nPG_Y = 150; public int PG_Y { get => w_nPG_Y; set => this.RaiseAndSetIfChanged(ref w_nPG_Y, value); }
        public int w_nPG_W = 375; public int PG_W { get => w_nPG_W; set { this.RaiseAndSetIfChanged(ref w_nPG_W, value); } }
        public int w_nPG_H = 647; public int PG_H { get => w_nPG_H; set { this.RaiseAndSetIfChanged(ref w_nPG_H, value); } }
        public string w_strPageResize = "Resize(375, 647)"; public string PageResize { get => w_strPageResize; set => this.RaiseAndSetIfChanged(ref w_strPageResize, value); }
        public int w_nPG_RW = 375; public int PG_RW
        {
            get => w_nPG_RW;
            set
            {
                PG_RBX = value + PG_X - 15;
                PageResize = "Resize(" + value + ", " + PG_RH + ")";
                if (WorkScreen != null) WorkScreen.m_Size.Width = value;
                this.RaiseAndSetIfChanged(ref w_nPG_RW, value);
            }
        }
        public int w_nPG_RH = 647; public int PG_RH
        {
            get => w_nPG_RH;
            set
            {
                PG_RBY = value + PG_Y - 15;
                PageResize = "Resize(" + PG_RW + ", " + value + ")";
                if (WorkScreen != null) WorkScreen.m_Size.Height = value;
                this.RaiseAndSetIfChanged(ref w_nPG_RH, value);
            }
        }
        public int w_nPG_RBX = 375; public int PG_RBX { get => w_nPG_RBX; set => this.RaiseAndSetIfChanged(ref w_nPG_RBX, value); }
        public int w_nPG_RBY = 647; public int PG_RBY { get => w_nPG_RBY; set => this.RaiseAndSetIfChanged(ref w_nPG_RBY, value); }
        public int w_nPF_X = 125; public int PF_X { get => w_nPF_X; set => this.RaiseAndSetIfChanged(ref w_nPF_X, value); }
        public int w_nPF_Y = 65; public int PF_Y { get => w_nPF_Y; set => this.RaiseAndSetIfChanged(ref w_nPF_Y, value); }
        public double w_nPF_OPT = 0.33; public double PF_OPT { get => w_nPF_OPT; set => this.RaiseAndSetIfChanged(ref w_nPF_OPT, value); }
        public int w_nPF_Ang = 0; public int PF_Ang { get => w_nPF_Ang; set => this.RaiseAndSetIfChanged(ref w_nPF_Ang, value); }
        public int w_nPF_W = 425; public int PF_W { get => w_nPF_W; set => this.RaiseAndSetIfChanged(ref w_nPF_W, value); }
        public int w_nPF_H = 817; public int PF_H { get => w_nPF_H; set => this.RaiseAndSetIfChanged(ref w_nPF_H, value); }
        public int w_nPF_TH = 85; public int PF_TH { get => w_nPF_TH; set => this.RaiseAndSetIfChanged(ref w_nPF_TH, value); }
        public int w_nPF_BH = 85; public int PF_BH { get => w_nPF_BH; set => this.RaiseAndSetIfChanged(ref w_nPF_BH, value); }
        public int w_nPF_LW = 25; public int PF_LW { get => w_nPF_LW; set => this.RaiseAndSetIfChanged(ref w_nPF_LW, value); }
        public int w_nPF_RW = 25; public int PF_RW { get => w_nPF_RW; set => this.RaiseAndSetIfChanged(ref w_nPF_RW, value); }
        public int w_nPF_TLX0 = 165; public int PF_TLX0 { get => w_nPF_TLX0; set => this.RaiseAndSetIfChanged(ref w_nPF_TLX0, value); }
        public int w_nPF_TLY = 35; public int PF_TLY { get => w_nPF_TLY; set => this.RaiseAndSetIfChanged(ref w_nPF_TLY, value); }
        public int w_nPF_BLX = 182; public int PF_BLX { get => w_nPF_BLX; set => this.RaiseAndSetIfChanged(ref w_nPF_BLX, value); }
        public int w_nPF_BLY = 745; public int PF_BLY { get => w_nPF_BLY; set => this.RaiseAndSetIfChanged(ref w_nPF_BLY, value); }
        public Avalonia.Media.Imaging.Bitmap w_strImg_PF_TL = new Avalonia.Media.Imaging.Bitmap("./Assets/Platforms/iPhone 7/Top_Left.png");
        public Avalonia.Media.Imaging.Bitmap Img_PF_TL { get => w_strImg_PF_TL; set => this.RaiseAndSetIfChanged(ref w_strImg_PF_TL, value); }
        public Avalonia.Media.Imaging.Bitmap w_strImg_PF_TM = new Avalonia.Media.Imaging.Bitmap("./Assets/Platforms/iPhone 7/Top_Middle.png");
        public Avalonia.Media.Imaging.Bitmap Img_PF_TM { get => w_strImg_PF_TM; set => this.RaiseAndSetIfChanged(ref w_strImg_PF_TM, value); }
        public Avalonia.Media.Imaging.Bitmap w_strImg_PF_TR = new Avalonia.Media.Imaging.Bitmap("./Assets/Platforms/iPhone 7/Top_Right.png");
        public Avalonia.Media.Imaging.Bitmap Img_PF_TR { get => w_strImg_PF_TR; set => this.RaiseAndSetIfChanged(ref w_strImg_PF_TR, value); }
        public Avalonia.Media.Imaging.Bitmap w_strImg_PF_BL = new Avalonia.Media.Imaging.Bitmap("./Assets/Platforms/iPhone 7/Bottom_Left.png");
        public Avalonia.Media.Imaging.Bitmap Img_PF_BL { get => w_strImg_PF_BL; set => this.RaiseAndSetIfChanged(ref w_strImg_PF_BL, value); }
        public Avalonia.Media.Imaging.Bitmap w_strImg_PF_BM = new Avalonia.Media.Imaging.Bitmap("./Assets/Platforms/iPhone 7/Bottom_Middle.png");
        public Avalonia.Media.Imaging.Bitmap Img_PF_BM { get => w_strImg_PF_BM; set => this.RaiseAndSetIfChanged(ref w_strImg_PF_BM, value); }
        public Avalonia.Media.Imaging.Bitmap w_strImg_PF_BR = new Avalonia.Media.Imaging.Bitmap("./Assets/Platforms/iPhone 7/Bottom_Right.png");
        public Avalonia.Media.Imaging.Bitmap Img_PF_BR { get => w_strImg_PF_BR; set => this.RaiseAndSetIfChanged(ref w_strImg_PF_BR, value); }
        public Avalonia.Media.Imaging.Bitmap w_strImg_PF_L = new Avalonia.Media.Imaging.Bitmap("./Assets/Platforms/iPhone 7/Left.png");
        public Avalonia.Media.Imaging.Bitmap Img_PF_L { get => w_strImg_PF_L; set => this.RaiseAndSetIfChanged(ref w_strImg_PF_L, value); }
        public Avalonia.Media.Imaging.Bitmap w_strImg_PF_R = new Avalonia.Media.Imaging.Bitmap("./Assets/Platforms/iPhone 7/Right.png");
        public Avalonia.Media.Imaging.Bitmap Img_PF_R { get => w_strImg_PF_R; set => this.RaiseAndSetIfChanged(ref w_strImg_PF_R, value); }
        public Avalonia.Media.Imaging.Bitmap w_strImg_PF_TL0 = new Avalonia.Media.Imaging.Bitmap("./Assets/Platforms/iPhone 7/Top_Label.png");
        public Avalonia.Media.Imaging.Bitmap Img_PF_TL0 { get => w_strImg_PF_TL0; set => this.RaiseAndSetIfChanged(ref w_strImg_PF_TL0, value); }
        public Avalonia.Media.Imaging.Bitmap w_strImg_PF_BL0 = new Avalonia.Media.Imaging.Bitmap("./Assets/Platforms/iPhone 7/Bottom_Label.png");
        public Avalonia.Media.Imaging.Bitmap Img_PF_BL0 { get => w_strImg_PF_BL0; set => this.RaiseAndSetIfChanged(ref w_strImg_PF_BL0, value); }

        /// </ScreenView>
        /// <PlatformView>
        public bool w_IsPlatForm1 = false; public bool IsPlatForm1 { get => w_IsPlatForm1; set => this.RaiseAndSetIfChanged(ref w_IsPlatForm1, value); }
        public bool w_IsPlatForm2 = false; public bool IsPlatForm2 { get => w_IsPlatForm2; set => this.RaiseAndSetIfChanged(ref w_IsPlatForm2, value); }
        public bool w_IsPlatForm3 = false; public bool IsPlatForm3 { get => w_IsPlatForm3; set => this.RaiseAndSetIfChanged(ref w_IsPlatForm3, value); }
        public bool w_IsPlatForm4 = false; public bool IsPlatForm4 { get => w_IsPlatForm4; set => this.RaiseAndSetIfChanged(ref w_IsPlatForm4, value); }
        public bool w_IsPlatForm5 = false; public bool IsPlatForm5 { get => w_IsPlatForm5; set => this.RaiseAndSetIfChanged(ref w_IsPlatForm5, value); }
        public bool w_IsPlatForm6 = false; public bool IsPlatForm6 { get => w_IsPlatForm6; set => this.RaiseAndSetIfChanged(ref w_IsPlatForm6, value); }
        public bool w_IsPlatForm7 = false; public bool IsPlatForm7 { get => w_IsPlatForm7; set => this.RaiseAndSetIfChanged(ref w_IsPlatForm7, value); }
        public string w_strPlatFormTitle = "iPhone 7"; public string strPlatFormTitle { get => w_strPlatFormTitle; set => this.RaiseAndSetIfChanged(ref w_strPlatFormTitle, value); }
        public ObservableCollection<string> SubPlatform { get; }
        public int w_nSubPlatformId = 0;
        public int SubPlatformId
        {
            get
            {
                return w_nSubPlatformId;
            }
            set
            {
                if (value > -1)
                {
                    PG_W = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[value].W;//375;
                    PG_H = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[value].H;//647;
                    PF_W = PG_W + PF_LW + PF_RW;
                    PF_H = PG_H + PF_TH + PF_BH;
                    PF_TLX0 = (PF_W - m_PlatFormInfo[m_nSelectedPlatFormIndex].L_Size[0].W) / 2;
                    PF_BLX = (PF_W - m_PlatFormInfo[m_nSelectedPlatFormIndex].L_Size[1].W) / 2;
                    PF_BLY = PF_H - (PF_BH + m_PlatFormInfo[m_nSelectedPlatFormIndex].L_Size[1].H) / 2;
                    PG_RW = PG_W;
                    PG_RH = PG_H;
                    if (PF_Ang > 0)
                    {
                        PG_RW = PG_H;
                        PG_RH = PG_W;
                    }
                }
                this.RaiseAndSetIfChanged(ref w_nSubPlatformId, value);
            }
        }
        //public Color w_nPageBackground;// Color.Parse("#00ff00");//Color.FromRgb(0,250,0);
        public bool w_IsHorViewEnabled = true;
        public bool w_IsVerViewEnabled = true;
        public bool w_IsResponseVisible = false;

        public SolidColorBrush w_nPageBackground = new SolidColorBrush(new Color(255, 255, 255, 255));//Color.FromRgb(0,250,0);
        public SolidColorBrush PageBackground
        {
            get
            {
                return w_nPageBackground;
            }
            set
            {
                WorkScreen.m_background = value;
                this.RaiseAndSetIfChanged(ref w_nPageBackground, value);
            }
        }

        public bool IsHorViewEnabled { get => w_IsHorViewEnabled; set => this.RaiseAndSetIfChanged(ref w_IsHorViewEnabled, value); }
        public bool IsVerViewEnabled { get => w_IsVerViewEnabled; set => this.RaiseAndSetIfChanged(ref w_IsVerViewEnabled, value); }
        public bool IsResponseVisible { get => w_IsResponseVisible; set => this.RaiseAndSetIfChanged(ref w_IsResponseVisible, value); }
        public bool w_Orientation = true;
        public bool Orientation { get => w_Orientation; set { this.RaiseAndSetIfChanged(ref w_Orientation, value); if (WorkScreen != null) WorkScreen.m_Orientation = Orientation; } }
        /// </PlatformView>

        public int w_SelectedTabIndex { get; set; }
        public IEnumerable<TabItemModel> DataContent { get; set; }

        public ObservableCollection<ScreenSmallView> SmallScreens
        {
            get => m_ScreenSmallView;
            set => this.RaiseAndSetIfChanged(ref m_ScreenSmallView, value);
        }

        public ScreenView WorkScreen
        {
            get => m_WorkScreen;
            set => this.RaiseAndSetIfChanged(ref m_WorkScreen, value);
        }
        public bool IsScreenVisible
        {
            get => m_IsScreenVisible;
            set => this.RaiseAndSetIfChanged(ref m_IsScreenVisible, value);
        }
        public bool m_IsProjectView = false;
        public bool IsProjectView
        {
            get => m_IsProjectView;
            set
            {
                this.RaiseAndSetIfChanged(ref m_IsProjectView, value);
                if (IsProjectView)
                    SmallScreenID = -1;
            }
        }
        public int m_nSmallScreenID = -1;
        public int SmallScreenID
        {
            get
            {

                return m_nSmallScreenID;
            }
            set
            {
                if (ContainerFlag > 0)
                {
                    for (int i = ContainerCanvas.Count - 1; i >= 0; i--)
                    {
                        ContainerBoxControl ctl = ContainerCanvas[i].FindAncestorOfType<ContainerBoxControl>();
                        if (ctl != null)
                        {
                            ctl.Click_CloseButton(ctl.Click_CloseButton, new RoutedEventArgs());
                        }
                    }

                }
                if (value >= 0 && m_lstWorkScreen.Count > value)
                {

                    IsProjectView = false;


                    WorkScreen = m_lstWorkScreen[value];

                    PG_RW = WorkScreen.m_Size.Width;
                    PG_RH = WorkScreen.m_Size.Height;
                    setOrientation(WorkScreen.m_Orientation);
                    //Orientation = WorkScreen.m_Orientation;
                    PageBackground = WorkScreen.m_background;
                    PG_OPT = WorkScreen.m_Opacity;
                    if (m_PlatformView != null)
                    {
                        m_PlatformView.colorButton.Color = WorkScreen.m_background.Color;
                    }

                    setScreenSmallView(value);
                }
                this.RaiseAndSetIfChanged(ref m_nSmallScreenID, value);
            }
        }
        public string m_strProjectLocation = "D:\\";
        public string strProjectLocation
        {
            get => m_strProjectLocation;
            set => this.RaiseAndSetIfChanged(ref m_strProjectLocation, value);
        }
        public string m_strProjectTitle = "My First Project";
        public string strProjectTitle
        {
            get => m_strProjectTitle;
            set
            {
                this.RaiseAndSetIfChanged(ref m_strProjectTitle, value);
                if (m_IsProjectPath) return;
                strProjectPath = strProjectLocation + "\\" + strProjectTitle;
                int id = 1;
                while (Directory.Exists(strProjectPath))
                {
                    strProjectPath = strProjectLocation + "\\" + strProjectTitle + "-" + id.ToString();
                    id++;
                }
            }
        }
        public string m_strProjectPath;
        public string strProjectPath
        {
            get => m_strProjectPath;
            set => this.RaiseAndSetIfChanged(ref m_strProjectPath, value);
        }
        public bool m_IsProjectUnSaved = true;
        public bool IsProjectUnSaved
        {
            get => m_IsProjectUnSaved;
            set => this.RaiseAndSetIfChanged(ref m_IsProjectUnSaved, value);
        }
        public bool IsStartMocker
        {
            get => w_IsStartMockerState;
            set => this.RaiseAndSetIfChanged(ref w_IsStartMockerState, value);
        }
        public bool IsMenuOpened
        {
            get => w_IsMenuOpenState;
            set => this.RaiseAndSetIfChanged(ref w_IsMenuOpenState, value);
        }

        public bool IsProjectOpened
        {
            get => w_IsProjectState;
            set => this.RaiseAndSetIfChanged(ref w_IsProjectState, value);
        }
        public bool IsWorkView
        {
            get => w_IsWorkView;
            set => this.RaiseAndSetIfChanged(ref w_IsWorkView, value);
        }
        public bool IsToolbarView
        {
            get => w_IsToolbarView;
            set => this.RaiseAndSetIfChanged(ref w_IsToolbarView, value);
        }
        public MainWindowViewModel()
        {
            List<stSize> w_PF_Size = new List<stSize>();
            List<stSize> w_PG_Size = new List<stSize>();
            List<stSize> w_L_Size = new List<stSize>();
            w_PF_Size.Add(new stSize(431, 880));
            w_PF_Size.Add(new stSize(478, 431));
            w_PG_Size.Add(new stSize(375, 647));
            w_PG_Size.Add(new stSize(414, 716));
            w_PG_Size.Add(new stSize(667, 375));
            w_PG_Size.Add(new stSize(736, 414));
            w_L_Size.Add(new stSize(95, 15));
            w_L_Size.Add(new stSize(60, 60));

            m_PlatFormInfo.Add(new stPlatFormPosInfo("iPhone 7", w_PF_Size, 150, 150, w_PG_Size, 85, 85, 25, 25, w_L_Size));
            w_PF_Size.Clear();
            w_PF_Size.Add(new stSize(380, 802));
            w_PG_Size.Clear();
            w_PG_Size.Add(new stSize(320, 548));
            w_PG_Size.Add(new stSize(568, 300));
            w_L_Size.Clear();
            w_L_Size.Add(new stSize(70, 50));
            w_L_Size.Add(new stSize(60, 60));
            m_PlatFormInfo.Add(new stPlatFormPosInfo("iPhone SE", w_PF_Size, 150, 150, w_PG_Size, 120, 115, 31, 26, w_L_Size));
            w_PF_Size.Clear();
            w_PF_Size.Add(new stSize(36, 156));
            w_PG_Size.Clear();
            w_PG_Size.Add(new stSize(320, 480));
            w_PG_Size.Add(new stSize(360, 640));
            w_PG_Size.Add(new stSize(412, 690));
            w_PG_Size.Add(new stSize(480, 320));
            w_PG_Size.Add(new stSize(640, 360));
            w_PG_Size.Add(new stSize(690, 412));
            w_L_Size.Clear();
            w_L_Size.Add(new stSize(66, 10));
            w_L_Size.Add(new stSize(60, 60));
            m_PlatFormInfo.Add(new stPlatFormPosInfo("Other iPhone", w_PF_Size, 150, 150, w_PG_Size, 60, 60, 13, 13, w_L_Size));
            w_PF_Size.Clear();
            w_PF_Size.Add(new stSize(866, 1250));
            w_PG_Size.Clear();
            w_PG_Size.Add(new stSize(748, 1024));
            w_PG_Size.Add(new stSize(1004, 768));
            w_L_Size.Clear();
            w_L_Size.Add(new stSize(13, 13));
            w_L_Size.Add(new stSize(60, 60));
            m_PlatFormInfo.Add(new stPlatFormPosInfo("iPad", w_PF_Size, 150, 150, w_PG_Size, 111, 111, 50, 50, w_L_Size));
            w_PF_Size.Clear();
            w_PF_Size.Add(new stSize(140, 140));
            w_PG_Size.Clear();
            w_PG_Size.Add(new stSize(768, 1024));
            w_PG_Size.Add(new stSize(1024, 1440));
            w_PG_Size.Add(new stSize(1024, 768));
            w_PG_Size.Add(new stSize(1440, 1024));
            w_L_Size.Clear();
            w_L_Size.Add(new stSize(13, 13));
            w_L_Size.Add(new stSize(60, 60));
            m_PlatFormInfo.Add(new stPlatFormPosInfo("Other Tablet", w_PF_Size, 150, 150, w_PG_Size, 70, 70, 70, 70, w_L_Size));
            w_PF_Size.Clear();
            w_PF_Size.Add(new stSize(1, 20));
            w_PG_Size.Clear();
            w_PG_Size.Add(new stSize(320, 548));
            w_PG_Size.Add(new stSize(640, 480));
            w_PG_Size.Add(new stSize(800, 600));
            w_PG_Size.Add(new stSize(1024, 768));
            w_PG_Size.Add(new stSize(1280, 1024));
            w_L_Size.Clear();
            w_L_Size.Add(new stSize(55, 23));
            w_L_Size.Add(new stSize(55, 23));
            m_PlatFormInfo.Add(new stPlatFormPosInfo("Browser", w_PF_Size, 150, 150, w_PG_Size, 40, 3, 3, 3, w_L_Size));
            w_PF_Size.Clear();
            w_PF_Size.Add(new stSize(1, 1));
            w_PG_Size.Clear();
            w_PG_Size.Add(new stSize(640, 480));
            w_PG_Size.Add(new stSize(800, 600));
            w_PG_Size.Add(new stSize(1024, 768));
            w_PG_Size.Add(new stSize(1280, 1024));
            w_L_Size.Clear();
            w_L_Size.Add(new stSize(13, 13));
            w_L_Size.Add(new stSize(60, 60));
            m_PlatFormInfo.Add(new stPlatFormPosInfo("Generic", w_PF_Size, 150, 150, w_PG_Size, 85, 85, 20, 20, w_L_Size));

            /*m_PlatFormInfo.Add(new stPlatFormPosInfo("./Assets/iPhoneSE.png", 200,50,123,133,368,743,227,155,320,548));
            m_PlatFormInfo.Add(new stPlatFormPosInfo("./Assets/lumia920.png", 200,50,120,130,356,660,218,147,320,480));
            m_PlatFormInfo.Add(new stPlatFormPosInfo("./Assets/ipad.png", 200,50,135,93,869,1251,250,164,748,1024));
            m_PlatFormInfo.Add(new stPlatFormPosInfo("./Assets/surfacePro_3.png", 200,70,262,5,1759,1188,358,166,1440,1024));
            m_PlatFormInfo.Add(new stPlatFormPosInfo("./Assets/browser.png", 50,50,97,150,642,531,51,68,640,510));
            m_PlatFormInfo.Add(new stPlatFormPosInfo(null, 50,50,97,150,800,600,50,50,800,600));*/



            /*ScreenSmallView w_ScreenSmallView = new ScreenSmallView();
            w_ScreenSmallView = new ScreenSmallView();
            m_ScreenSmallView.Add(w_ScreenSmallView);
            MyItems = m_ScreenSmallView;*/


            SubPlatform = new ObservableCollection<string>();
            SubPlatform.Add("iPhone 7 - 375X647");
            SubPlatform.Add("iPhone 7 Plus - 414X716");
            SubPlatformId = 0;
            onSaveProject = new AsyncRelayCommand(async () => await SaveProjFile());
            onOpenProject = new AsyncRelayCommand(async () => await OpenProjFile());
            onSearchProject = new AsyncRelayCommand(async () => await OpenProjFile());
           // RecentProjects = new AsyncRelayCommand(async () => await GetAllRecentProjects());
            _ = InitializeRecentProjects();
            DataContent = new TabItemModel[] {
                new TabItemModel("One", "first"),
                new TabItemModel("Two", "second"),
            };
            onMenuOpen = ReactiveCommand.Create(() =>
            {
                // Code here will be executed when the button is clicked.
                IsMenuOpened = true;
            });
            onMenuClose = ReactiveCommand.Create(() =>
            {
                IsMenuOpened = false;
            });
            onNewProject = ReactiveCommand.Create(() =>
            {
                IsMenuOpened = false;
                init();
                //OpenFolderPickerAsync()
            });
            onSaveProject = ReactiveCommand.Create(() =>
            {
                IsMenuOpened = false;
                if (m_IsProjectPath)
                {
                    savePage(strProjectPath);
                    if (m_IsProjectView)
                    {
                        IsProjectUnSaved = false;
                        for (int i = 0; i < m_ScreenSmallView.Count; i++)
                            m_ScreenSmallView[i].ScreenUnSaved.IsVisible = false;
                    }
                    else
                    {
                        //screen saved
                        m_ScreenSmallView[m_nSmallScreenID].ScreenUnSaved.IsVisible = false;
                        bool w_allunSaved = false;
                        for (int i = 0; i < m_ScreenSmallView.Count; i++)
                        {
                            if (m_ScreenSmallView[i].ScreenUnSaved.IsVisible == true)
                            {
                                w_allunSaved = true;
                                break;
                            }
                        }
                        IsProjectUnSaved = w_allunSaved;
                    }
                    return;
                }
                strProjectPath = strProjectLocation + "\\" + strProjectTitle;
                int id = 1;
                while (Directory.Exists(strProjectPath))
                {
                    strProjectPath = strProjectLocation + "\\" + strProjectTitle + "-" + id.ToString();
                    id++;
                }
                m_SaveWindow = new SaveProjectWindow();
                m_SaveWindow.ShowDialog(m_MainWindow);
                m_SaveWindow.setMainVM(this);
            });
            onSaveAllProject = ReactiveCommand.Create(() =>
            {
                IsMenuOpened = false;
                if (m_IsProjectPath)
                {
                    saveAllPages(strProjectPath);
                    IsProjectUnSaved = false;
                    for (int i = 0; i < m_ScreenSmallView.Count; i++)
                        m_ScreenSmallView[i].ScreenUnSaved.IsVisible = false;
                    return;
                }
                strProjectPath = strProjectLocation + "\\" + strProjectTitle;
                int id = 1;
                while (Directory.Exists(strProjectPath))
                {
                    strProjectPath = strProjectLocation + "\\" + strProjectTitle + "-" + id.ToString();
                    id++;
                }
                m_SaveWindow = new SaveProjectWindow();
                m_SaveWindow.ShowDialog(m_MainWindow);
                m_SaveWindow.save.Content = "Save All";
                m_SaveWindow.setMainVM(this);
            });
            /*onSetProjectPath = ReactiveCommand.Create(() =>
            {
                //OpenFolderPickerAsync()
            });*/
            onSetProjectPath = new AsyncRelayCommand(async () => await OpenProjFolder());
            onSave = ReactiveCommand.Create(() =>
            {
                IsMenuOpened = false;
                if (m_SaveWindow == null) return;
                createFolder(strProjectPath);
                savePage(strProjectPath);
                m_IsProjectPath = true;
                if (m_IsProjectView || m_SaveWindow.save.Content == "Save All")
                {
                    IsProjectUnSaved = false;
                    for (int i = 0; i < m_ScreenSmallView.Count; i++)
                        m_ScreenSmallView[i].ScreenUnSaved.IsVisible = false;
                }
                else
                {
                    //screen saved
                    m_ScreenSmallView[m_nSmallScreenID].ScreenUnSaved.IsVisible = false;
                    bool w_allunSaved = false;
                    for (int i = 0; i < m_ScreenSmallView.Count; i++)
                    {
                        if (m_ScreenSmallView[i].ScreenUnSaved.IsVisible == true)
                        {
                            w_allunSaved = true;
                            break;
                        }
                    }
                    IsProjectUnSaved = w_allunSaved;
                }
                m_SaveWindow.Close();
            });
            onSaveCancel = ReactiveCommand.Create(() =>
            {
                if (m_SaveWindow == null) return;
                m_SaveWindow.Close();
            });
            onCloseProject = ReactiveCommand.Create(() =>
            {
                IsStartMocker = true;
                IsMenuOpened = false;
                IsProjectOpened = false;
            });
            onPlatForm1 = ReactiveCommand.Create(() =>
            {
                setPlatform(0);
            });
            onPlatForm2 = ReactiveCommand.Create(() =>
            {
                setPlatform(1);
            });
            onPlatForm3 = ReactiveCommand.Create(() =>
            {
                setPlatform(2);
            });
            onPlatForm4 = ReactiveCommand.Create(() =>
            {
                setPlatform(3);
            });
            onPlatForm5 = ReactiveCommand.Create(() =>
            {
                setPlatform(4);
            });
            onPlatForm6 = ReactiveCommand.Create(() =>
            {
                setPlatform(5);
            });
            onPlatForm7 = ReactiveCommand.Create(() =>
            {
                setPlatform(6);
            });
            onHorViewPort = ReactiveCommand.Create(() =>
            {
                setOrientation(true);

            });
            onVerViewPort = ReactiveCommand.Create(() =>
            {
                setOrientation(false);
            });
            onNewScreen = ReactiveCommand.Create(() =>
            {
                createPage(null);
            });
            onDeleteScreen = ReactiveCommand.Create(() =>
            {
                int id = SmallScreenID;
                m_lstWorkScreen.RemoveAt(SmallScreenID);
                SmallScreens.RemoveAt(SmallScreenID);
                if (SmallScreens.Count == 0)
                    IsProjectView = true;
                else if (SmallScreens.Count > id)
                    SmallScreenID = id;
                else
                    SmallScreenID = 0;
            });
            onUndo = ReactiveCommand.Create(() =>
            {
                int w_Count = WorkScreen.m_UndoList.Count;
                if (w_Count > 0)
                {

                    stControlHistory w_ControlHistory = WorkScreen.m_UndoList[w_Count - 1];
                    string w_Cmd = WorkScreen.m_UndoList[w_Count - 1].Cmd;
                    int w_Index = WorkScreen.m_UndoList[w_Count - 1].Index;

                    if (w_Cmd == "New")
                    {
                        Control w_Control = WorkScreen.screenCanvas.Children[w_Index];
                        WorkScreen.screenCanvas.Children.Remove(w_Control);
                        WorkScreen.m_UndoList.Remove(w_ControlHistory);
                    }
                    WorkScreen.m_RedoList.Add(w_ControlHistory);
                    if (ContainerFlag > 0)
                        ContainerFlag = 0;
                }
            });
            onRedo = ReactiveCommand.Create(() =>
            {
                int w_Count = WorkScreen.m_RedoList.Count;
                if (w_Count > 0)
                {
                    stControlHistory w_ControlHistory = WorkScreen.m_RedoList[w_Count - 1];
                    string w_Cmd = WorkScreen.m_RedoList[w_Count - 1].Cmd;
                    int w_Index = WorkScreen.m_RedoList[w_Count - 1].Index;
                    if (w_Cmd == "New")
                    {
                        Control w_Control = w_ControlHistory.curInfo;
                        WorkScreen.screenCanvas.Children.Insert(w_Index, w_Control);
                        WorkScreen.m_RedoList.Remove(w_ControlHistory);
                    }
                    WorkScreen.m_UndoList.Add(w_ControlHistory);
                }
            });
            onRun = ReactiveCommand.Create(() =>
            {
                var window = new RunWindow();
                window.setMainViewModel(this);
                if (!Orientation)
                {
                    RunWindowHeight = w_nPF_W + w_nPF_X * 2;
                    RunWindowWidth = w_nPF_H + w_nPF_Y * 2;
                }
                window.Show();
            });
            onShare = ReactiveCommand.Create(() =>
            {
                var window = new ShareWindow();
                window.ShowDialog(m_MainWindow);
                window.setMainVM(this);
            });
            onSmallCanvas = ReactiveCommand.Create(() =>
            {

            });
        }
        public void setMainWindow(Window window)
        {
            m_MainWindow = (MainWindow)window;
        }
        private void init(bool flag = true)
        {
            m_IsProjectPath = false;
            strProjectTitle = "My First Project";
            IsProjectUnSaved = true;
            IsStartMocker = false;
            IsMenuOpened = false;
            IsProjectOpened = true;
            IsScreenVisible = false;
            IsWorkView = false;
            IsToolbarView = false;
            IsProjectView = true;
            for (int i = 0; i < SmallScreens.Count;)
            {
                SmallScreens.RemoveAt(0);
                m_lstWorkScreen.RemoveAt(0);
            }
            if(flag)
            createPage(null);
        }
        public void setPlatform(int platformId)
        {
            IsPlatForm1 = false;
            IsPlatForm2 = false;
            IsPlatForm3 = false;
            IsPlatForm4 = false;
            IsPlatForm5 = false;
            IsPlatForm6 = false;
            IsPlatForm7 = false;
            m_nSelectedPlatFormIndex = platformId;
            if (m_nSelectedPlatFormIndex == 0)
            {
                IsPlatForm1 = true;
                SubPlatform.Clear();
                SubPlatform.Add("iPhone 7 - 375X647");
                SubPlatform.Add("iPhone 7 Plus - 414X716");
                SubPlatformId = 0;
            }
            else if (m_nSelectedPlatFormIndex == 1)
            {
                IsPlatForm2 = true;
                SubPlatform.Clear();
                SubPlatform.Add("iPhone SE - 320X548");
                SubPlatformId = 0;
            }
            else if (m_nSelectedPlatFormIndex == 2)
            {
                IsPlatForm3 = true;
                SubPlatform.Clear();
                SubPlatform.Add("Lumia 920 - 320X480");
                SubPlatform.Add("Nexus 5 - 360X640");
                SubPlatform.Add("Nexus 6 - 412X690");
                SubPlatform.Add("Custom - resize");
                SubPlatformId = 0;
            }
            else if (m_nSelectedPlatFormIndex == 3)
            {
                IsPlatForm4 = true;
                SubPlatform.Clear();
                SubPlatform.Add("iPad - 1024X748");
                SubPlatformId = 0;
            }
            else if (m_nSelectedPlatFormIndex == 4)
            {
                IsPlatForm5 = true;
                SubPlatform.Clear();
                SubPlatform.Add("Nexus 9 - 1024X768");
                SubPlatform.Add("Surface Pro 3 - 1440X1024");
                SubPlatform.Add("Custom - resize");
                SubPlatformId = 0;
            }
            else if (m_nSelectedPlatFormIndex == 5)
            {
                IsPlatForm6 = true;
                SubPlatform.Clear();
                SubPlatform.Add("320X548");
                SubPlatform.Add("640X480");
                SubPlatform.Add("800X600");
                SubPlatform.Add("1024X768");
                SubPlatform.Add("1280X1024");
                SubPlatform.Add("Custom");
                SubPlatformId = 0;
            }
            else if (m_nSelectedPlatFormIndex == 6)
            {
                IsPlatForm7 = true;
                SubPlatform.Clear();
                SubPlatform.Add("640X480");
                SubPlatform.Add("800X600");
                SubPlatform.Add("1024X768");
                SubPlatform.Add("1280X1024");
                SubPlatform.Add("Custom");
                SubPlatformId = 0;
            }
            PF_Ang = 0;
            Orientation = true;
            IsVerViewEnabled = IsHorViewEnabled = true;
            IsResponseVisible = false;

            strPlatFormTitle = m_PlatFormInfo[m_nSelectedPlatFormIndex].Type;

            string m_strImgPath = "./Assets/Platforms/" + strPlatFormTitle + "/";
            Img_PF_TL = LoadBitmapOrError("Top_Left.png", m_strImgPath);
            Img_PF_TM = LoadBitmapOrError("Top_Middle.png", m_strImgPath);
            Img_PF_TR = LoadBitmapOrError("Top_Right.png", m_strImgPath);
            Img_PF_BL = LoadBitmapOrError("Bottom_Left.png", m_strImgPath);
            Img_PF_BM = LoadBitmapOrError("Bottom_Middle.png", m_strImgPath);
            Img_PF_BR = LoadBitmapOrError("Bottom_Right.png", m_strImgPath);
            Img_PF_L = LoadBitmapOrError("Left.png", m_strImgPath);
            Img_PF_R = LoadBitmapOrError("Right.png", m_strImgPath);
            Img_PF_TL0 = LoadBitmapOrError("Top_Label.png", m_strImgPath);
            Img_PF_BL0 = LoadBitmapOrError("Bottom_Label.png", m_strImgPath);


            PG_W = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[0].W;
            PG_H = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[0].H;
            PG_X = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_X;
            PG_Y = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Y;
            PF_LW = m_PlatFormInfo[m_nSelectedPlatFormIndex].PF_LW;
            PF_RW = m_PlatFormInfo[m_nSelectedPlatFormIndex].PF_RW;
            PF_TH = m_PlatFormInfo[m_nSelectedPlatFormIndex].PF_TH;
            PF_BH = m_PlatFormInfo[m_nSelectedPlatFormIndex].PF_BH;
            PF_X = PG_X - PF_LW;
            PF_Y = PG_Y - PF_TH;
            PF_W = PG_W + PF_LW + PF_RW;
            PF_H = PG_H + PF_TH + PF_BH;
            PF_TLX0 = (PF_W - m_PlatFormInfo[m_nSelectedPlatFormIndex].L_Size[0].W) / 2;
            PF_TLY = (PF_TH - m_PlatFormInfo[m_nSelectedPlatFormIndex].L_Size[0].H) / 2;
            PF_BLX = (PF_W - m_PlatFormInfo[m_nSelectedPlatFormIndex].L_Size[1].W) / 2;
            PF_BLY = PF_H - (PF_BH + m_PlatFormInfo[m_nSelectedPlatFormIndex].L_Size[1].H) / 2;
            PG_RW = PG_W;
            PG_RH = PG_H;
        }

        private Bitmap? LoadBitmapOrError(string fileName, string imagePath)
        {
            string path = Path.Combine(imagePath, fileName);

            if (File.Exists(path))
            {
                return new Bitmap(path);
            }
            else
            {

                return null; // or return fallback image if you have one
            }
        }
        public void setOrientation(bool p_Orientation)
        {

            Orientation = p_Orientation;
            PG_X = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_X;
            PG_Y = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Y;
            PF_LW = m_PlatFormInfo[m_nSelectedPlatFormIndex].PF_LW;
            PF_RW = m_PlatFormInfo[m_nSelectedPlatFormIndex].PF_RW;
            PF_TH = m_PlatFormInfo[m_nSelectedPlatFormIndex].PF_TH;

            if (Orientation)
            {
                PF_Ang = 0;
                PF_X = PG_X - PF_LW;
                PF_Y = PG_Y - PF_TH;
                if (PG_RW < m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[SubPlatformId].W || PG_RW == m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[SubPlatformId].H)
                    PG_RW = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[SubPlatformId].W;
                if (PG_RH < m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[SubPlatformId].H)
                    PG_RH = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[SubPlatformId].H;
            }
            else
            {
                PF_Ang = 270;
                PF_X = PG_X - PF_TH;
                PF_Y = PG_Y - PF_RW;
                if (PG_RW < m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[SubPlatformId].H)
                    PG_RW = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[SubPlatformId].H;
                if (PG_RH < m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[SubPlatformId].W || PG_RH == m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[SubPlatformId].H)
                    PG_RH = m_PlatFormInfo[m_nSelectedPlatFormIndex].PG_Size[SubPlatformId].W;
            }
        }

        public IImage getImage(int id)
        {
            if (id < 0)
                return null;
            int w_height = 0;
            int w_width = 0;
            int w_crop_W = 0;
            int w_crop_H = 0;
            if (PF_Ang == 0)
            {
                w_height = w_nPF_H + 100;
                w_width = w_nPF_W + 300;
                w_crop_W = w_nPG_W;
                w_crop_H = w_nPG_H;
            }
            else
            {
                w_width = w_nPF_H + 100;
                w_height = w_nPF_W + 300;
                w_crop_W = w_nPG_H;
                w_crop_H = w_nPG_W;
            }

            var renderTargetBitmap = new RenderTargetBitmap(new PixelSize(w_width, w_height));

            renderTargetBitmap.Render(m_lstWorkScreen[id].screenCanvas);
            IImage result = new CroppedBitmap(renderTargetBitmap, new PixelRect(150, 150, w_crop_W, w_crop_H));
            return result;
        }
        public void setScreenSmallView(int id)
        {
            /*if(id<0)
                return;
            int height = 0;
            int width = 0;
            if (PF_Ang == 0)
            {
                height = w_nPF_H + 100;
                width = w_nPF_W + 300;
            }
            else
            {
                width = w_nPF_H + 100;
                height = w_nPF_W + 300;
            }

            var renderTargetBitmap = new RenderTargetBitmap(new PixelSize(width, height));
            renderTargetBitmap.Render(m_lstWorkScreen[id].screenCanvas);*/
            /*Image image = new Image();// This is a Image
            image.Source = renderTargetBitmap;
            image.Width = 180;
            image.Height = 150;
            image.Stretch = Stretch.Fill;*/
            m_ScreenSmallView[id].iamge.Source = getImage(id); //renderTargetBitmap;
        }
        private static List<FilePickerFileType> GetCodeFileTypes()
        {
            return new List<FilePickerFileType>
            {
                StorageService.Proj,
                //StorageService.All
            };
        }
        private async Task SaveProjFile()
        {
            IsMenuOpened = false;
            if (IsStartMocker)
            {
                return;
            }
            var storageProvider = StorageService.GetStorageProvider();
            if (storageProvider is null)
            {
                return;
            }
            var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save Project",
                FileTypeChoices = GetCodeFileTypes(),
                SuggestedFileName = Path.GetFileNameWithoutExtension(strProjectTitle),
                DefaultExtension = "dsproj",
                ShowOverwritePrompt = true
            });
            if (file is not null)
            {
                try
                {
                    _openCodeFile = file;
                    await using var stream = await _openCodeFile.OpenWriteAsync();
                    await using var writer = new StreamWriter(stream);
                    await writer.WriteAsync("!---Avalonia&c#Project---!");

                    string w_strName = file.Name;
                    int w_len = w_strName.Length;
                    if (w_strName.Length > 8 &&
                        w_strName.Substring(w_len - 7, 7) == ".dsproj")
                    {
                        strProjectTitle = w_strName.Substring(0, w_len - 7);
                        IsProjectUnSaved = false;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
        private async Task OpenProjFolder()
        {
            try
            {
                // Check if the default directory exists, if not use a sensible fallback
                string initialDirectory = strProjectLocation;
                if (!Directory.Exists(initialDirectory))
                {
                    initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                var openFileDialog = new OpenFolderDialog
                {
                    Title = "Select Folder",
                    Directory = initialDirectory
                };

                var result = await openFileDialog.ShowAsync(m_MainWindow);
                if (string.IsNullOrEmpty(result)) return;

                // Combine paths properly using Path.Combine
                strProjectPath = Path.Combine(result, strProjectTitle);

                int id = 1;
                while (Directory.Exists(strProjectPath))
                {
                    strProjectPath = Path.Combine(result, $"{strProjectTitle}-{id}");
                    id++;
                }

                strProjectLocation = Path.GetDirectoryName(strProjectPath);
            }
            catch (Exception ex)
            {
                // Handle or log the exception appropriately
                Console.WriteLine($"Error opening project folder: {ex.Message}");
                // You might want to show a message to the user here
            }
        }
        private async Task GetAllRecentProjects()
        {
            //var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            //{
            //    Title = "Open Project",
            //    FileTypeFilter = GetCodeFileTypes(),
            //    AllowMultiple = true
            //});

            //var files = result.TakeLast(2);
            RecentProjects.Clear();
            RecentProjects.Add(new RecentProject { Name = "Test Demo", LastOpened = DateTime.Now });
            RecentProjects.Add(new RecentProject { Name = "Final test", LastOpened = DateTime.Now });
        }
        private async Task InitializeRecentProjects()
        {
            try
            {
                // Wait for the UI to be fully initialized if needed
                await Task.Delay(100); // Small delay to ensure services are ready

                var storageProvider = StorageService.GetStorageProvider();
                if (storageProvider != null)
                {
                    await GetAllRecentProjects();
                }
                else
                {
                    Debug.WriteLine("Storage provider not available yet");
                    // You might want to retry after a delay
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing recent projects: {ex.Message}");
            }
        }
        private async Task OpenProjFile()
        {
            IsMenuOpened = false;
            //// if (!IsStartMocker)
            // if (IsStartMocker)
            // {
            //     return;
            // }
            var storageProvider = StorageService.GetStorageProvider();
            if (storageProvider is null)
            {
                return;
            }

            var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Project",
                FileTypeFilter = GetCodeFileTypes(),
                AllowMultiple = false
            });

            var file = result.FirstOrDefault();
            DeviceInfo w_DeviceInfo = await creatProject(file);
            if (w_DeviceInfo == null) return;
            for (int i = 0; i < w_DeviceInfo.PageCount; i++)
                createPage(getPageName(w_DeviceInfo.Pages[i]));
        }
        private async Task<DeviceInfo> creatProject(IStorageFile file)
        {
            if (file is not null)
            {
                try
                {
                    _openCodeFile = file;
                    await using var stream = await _openCodeFile.OpenReadAsync();
                    using var reader = new StreamReader(stream);
                    var fileContent = await reader.ReadToEndAsync();
                    var objects = JArray.Parse(fileContent);
                    JObject items = objects[0].ToObject<JObject>();
                    if (items.Count != 7) return null;
                    List<DeviceInfo> deviceInfo = JsonConvert.DeserializeObject<List<DeviceInfo>>(fileContent);
                    if (deviceInfo == null || deviceInfo[0] == null || deviceInfo[0].Device == null || deviceInfo[0].size.W <= 0 || deviceInfo[0].size.H <= 0) return null;

                    string w_strPath = file.Path.LocalPath;
                    string w_strName = Path.GetFileNameWithoutExtension(w_strPath);
                    string w_strExtension = Path.GetExtension(w_strPath);
                    if (w_strExtension != ".dsproj") return null;
                    init(false);
                    strProjectTitle = w_strName;
                    strProjectPath = Path.GetDirectoryName(w_strPath);
                    strProjectLocation = Path.GetDirectoryName(strProjectPath);
                    if (strProjectLocation == null)
                        strProjectLocation = strProjectPath;
                    IsStartMocker = false;
                    IsProjectOpened = true;
                    m_IsProjectPath = true;
                    IsProjectUnSaved = false;
                    setPlatform(deviceInfo[0].DeviceID);
                    SubPlatformId = deviceInfo[0].SubID;
                    return deviceInfo[0];
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }

            return null;
        }
        public string getPageName(string pageName)
        {
            string w_strPageName = pageName;
            bool w_bExist = true;
            int i = 0;
            while (w_bExist)
            {
                w_bExist = false; i++;
                foreach (ScreenView screen in m_lstWorkScreen)
                {
                    if (w_strPageName == screen.m_strName)
                    {
                        w_strPageName = pageName + i.ToString();
                        if (w_strPageName == "Page0") w_strPageName = "Page";
                        w_bExist = true;
                        break;
                    }
                }
            }
            return w_strPageName;
        }
        public void createPage(string pageName)
        {
            ScreenView w_WorkScreen = new ScreenView();
            ScreenSmallView w_ScreenSmallView = new ScreenSmallView();
            if (pageName == "")
            {
                pageName = getPageName("Page");
                m_lstWorkScreen.Add(w_WorkScreen);

                m_ScreenSmallView.Add(w_ScreenSmallView);
                SmallScreens = m_ScreenSmallView;
                IsProjectUnSaved = true;
            }
            else if (pageName is not null)
            {
               // if(pageName == "Page1") pageName = "Page";
                if (!File.Exists(strProjectPath + "\\" + pageName + ".dspage")) return;
                string json = File.ReadAllText(strProjectPath + "\\" + pageName + ".dspage");
                List<PageInfo> pageInfos = JsonConvert.DeserializeObject<List<PageInfo>>(json);

                IsWorkView = true;
                IsToolbarView = true;
                IsScreenVisible = true;

                w_WorkScreen.m_Orientation = pageInfos[0].Orientation;
                ImmutableSolidColorBrush brush = pageInfos[0].background as ImmutableSolidColorBrush;
                w_WorkScreen.m_background = new SolidColorBrush(brush.Color);
                w_WorkScreen.m_Opacity = pageInfos[0].Opacity;

                m_lstWorkScreen.Add(w_WorkScreen);
                WorkScreen = w_WorkScreen;
                if (m_PlatformView != null)
                {
                    m_PlatformView.colorButton.Color = WorkScreen.m_background.Color;
                }

                int controlCount = pageInfos[0].Contents.Count;
                foreach (Object obj in pageInfos[0].Contents)
                {

                    ControlInfo control = JsonConvert.DeserializeObject<ControlInfo>(JsonConvert.SerializeObject(obj));

                    UIControl uiControl;
                    if (control.Name == "Button")
                        uiControl = new ButtonControl();
                    else if (control.Name == "TextBox")
                        uiControl = new EditControl();
                    else if (control.Name == "Label")
                        uiControl = new LabelControl();
                    else if (control.Name == "Title")
                        uiControl = new LabelControl();
                    else if (control.Name == "Image")
                        uiControl = new ImageControl();
                    else if (control.Name == "Link")
                        uiControl = new LinkControl();
                    else if (control.Name == "Check")
                        uiControl = new CheckControl();
                    else if (control.Name == "Radio")
                        uiControl = new RadioControl();
                    else if (control.Name == "MultilineButton")
                    {
                        uiControl = new ButtonControl();
                        uiControl.setType(CONTROL_TYPE.MULTIBUTTON);
                    }
                    else if (control.Name == "TextArea")
                    {
                        uiControl = new EditControl();
                        uiControl.setType(CONTROL_TYPE.MULTIBUTTON);
                    }
                    else if (control.Name == "Slider")
                        uiControl = new SliderControl();
                    else if (control.Name == "Progress")
                        uiControl = new ProgressControl();
                    else if (control.Name == "Password")
                    {
                        uiControl = new EditControl();
                        uiControl.setType(CONTROL_TYPE.PASSWORD);
                        uiControl.setPasswordChar("*");
                    }
                    else if (control.Name == "DropDown")
                    {
                        ListControlInfo control1 = JsonConvert.DeserializeObject<ListControlInfo>(JsonConvert.SerializeObject(obj));

                        uiControl = new DropDownControl();
                        ((ListBoxViewModel)((DropDownControl)uiControl).DataContext).Items.Clear();
                        foreach (CustomItem item in control1.Items)
                        {
                            ((ListBoxViewModel)((DropDownControl)uiControl).DataContext).Items.Add(item);
                        }
                        brush = control1.ItemBackground as ImmutableSolidColorBrush;
                        ((ListBoxViewModel)((DropDownControl)uiControl).DataContext).itemBackground = new SolidColorBrush(brush.Color);

                    }
                    else if (control.Name == "ListBox")
                    {
                        ListControlInfo control1 = JsonConvert.DeserializeObject<ListControlInfo>(JsonConvert.SerializeObject(obj));
                        uiControl = new ListBoxControl();

                        ((ListBoxViewModel)((ListBoxControl)uiControl).DataContext).Items.Clear();
                        foreach (CustomItem item in control1.Items)
                        {
                            ((ListBoxViewModel)((ListBoxControl)uiControl).DataContext).Items.Add(item);
                        }
                        brush = control1.ItemBackground as ImmutableSolidColorBrush;
                        ((ListBoxViewModel)((ListBoxControl)uiControl).DataContext).itemBackground = new SolidColorBrush(brush.Color);

                    }
                    else
                        continue;
                    uiControl.setMainVM(this);
                    uiControl.m_nIndex = control.Index;
                    uiControl.setType(control.Type);
                    uiControl.setName(control.Name);
                    uiControl.setText(control.Text);
                    uiControl.setWidth(control.w);
                    uiControl.setHeight(control.h);
                    uiControl.setPosition(control.x, control.y);
                    //uiControl.setPositionX(control.x);
                    //uiControl.setPositionY(control.y);

                    uiControl.setFitWidth(control.isFitWidth);
                    uiControl.setFitHeight(control.isFitHeight);
                    if (control.isFitWidth && (control.Name == "Label" || control.Name == "Title"))
                    {
                        ((LabelControl)uiControl).setFitWidth();
                    }
                    if (control.isFitHeight && (control.Name == "Label" || control.Name == "Title"))
                    {
                        ((LabelControl)uiControl).setFitHeight();
                    }

                    uiControl.setImageSrc(strProjectPath, control.src);
                    uiControl.setOpacity(control.Opacity);
                    uiControl.setTooltip(control.Tooltip);
                    uiControl.m_bDisable = control.isDisable;
                    if (control.Background != null)
                    {
                        brush = control.Background as ImmutableSolidColorBrush;
                        uiControl.setBackground(new SolidColorBrush(brush.Color));
                    }
                    if (control.Foreground != null)
                    {
                        brush = control.Foreground as ImmutableSolidColorBrush;
                        uiControl.setForeground(new SolidColorBrush(brush.Color));
                    }
                    if (control.BorderColor != null)
                    {
                        brush = control.BorderColor as ImmutableSolidColorBrush;
                        uiControl.setBorderColor(new SolidColorBrush(brush.Color));
                    }
                    uiControl.setBorderThickness(control.BorderThickness);
                    uiControl.setBorderRound(control.BorderRound);
                    uiControl.setFontFamily(control.fontFamily);
                    uiControl.setFontSize(control.fontSize);
                    uiControl.setTextItalic(control.isItalic);
                    uiControl.setTextBold(control.isBold);
                    uiControl.setTapEvent(control.TapEvent);
                    uiControl.setDTapEvent(control.DTapEvent);
                    uiControl.setHPressEvent(control.HPressEvent);
                    uiControl.setSwipeLeftEvent(control.SwipeLeftEvent);
                    uiControl.setSwipeRightEvent(control.SwipeRightEvent);
                    uiControl.setSwipeUpEvent(control.SwipeUpEvent);
                    uiControl.setSwipeDownEvent(control.SwipeDownEvent);

                    WorkScreen.screenCanvas.Children.Add(uiControl);
                }



                m_ScreenSmallView.Add(w_ScreenSmallView);

                SmallScreens = m_ScreenSmallView;
                SmallScreenID = SmallScreens.Count - 1;

                w_ScreenSmallView.ScreenUnSaved.IsVisible = false;
            }
            else
            {
                IsWorkView = true;
                IsToolbarView = true;
                IsScreenVisible = true;
                IsProjectUnSaved = true;

                pageName = getPageName("Page");
                m_lstWorkScreen.Add(w_WorkScreen);
                WorkScreen = w_WorkScreen;

                m_ScreenSmallView.Add(w_ScreenSmallView);
                SmallScreens = m_ScreenSmallView;
                SmallScreenID = SmallScreens.Count - 1;

                if (m_wndUIProperty != null)
                    m_wndUIProperty.Hide();
            }
            w_ScreenSmallView.SmallCanvasText.Text = pageName;
            w_WorkScreen.m_strName = pageName;
            setScreenSmallView(SmallScreenID);
        }
        public void makeAssets(string path)
        {
            createFolder(path);
            UIControl w_uiControl;
            foreach (ScreenView screenView in m_lstWorkScreen)
            {
                for (int i = 3; i < screenView.screenCanvas.Children.Count; i++)
                {
                    w_uiControl = (UIControl)screenView.screenCanvas.Children[i];
                    if (w_uiControl.GetType() == typeof(ImageControl))
                    {
                        string w_path = Path.Combine(path, Path.GetFileName(w_uiControl.m_strSrc));
                        if (!File.Exists(w_uiControl.m_strSrc) || path == w_uiControl.m_strSrc) continue;
                        File.Copy(w_uiControl.m_strSrc, w_path);
                    }
                }
            }
        }
        public void createFolder(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path);
                Directory.CreateDirectory(path);//Create Project Folder
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
        public void saveProject(string path)
        {
            string w_strDevice = strPlatFormTitle;
            int w_nDeviceID = m_nSelectedPlatFormIndex;
            int w_nSubID = SubPlatformId;
            stSize w_Size;
            ////////////////////w_Size = new stSize(PF_W, PF_H);
            if (w_nDeviceID == 2 || w_nDeviceID == 4)
            {
                w_Size = new stSize(m_PlatFormInfo[w_nDeviceID].PF_Size[0].W, m_PlatFormInfo[w_nDeviceID].PF_Size[0].H);
            }
            else if (w_nDeviceID > 4)
                w_Size = new stSize(PF_W, PF_H);
            else w_Size = m_PlatFormInfo[w_nDeviceID].PF_Size[w_nSubID];//new Size(431, 880);
            ///////////////////////
            string w_strMainPage = null;
            int w_nPageCount = m_lstWorkScreen.Count;
            List<string> w_strPages = new List<string>();
            if (w_nPageCount > 0)
                w_strMainPage = m_lstWorkScreen[0].m_strName;
            foreach (var screen in m_lstWorkScreen)
            {
                w_strPages.Add(screen.m_strName);
            }
            List<DeviceInfo> w_DeviceInfo = new List<DeviceInfo>();
            string filePath = Path.Combine(path, strProjectTitle + ".dsproj");
            try
            {
                w_DeviceInfo.Add(new DeviceInfo { Device = w_strDevice, DeviceID = w_nDeviceID, SubID = w_nSubID, size = w_Size, MainPage = w_strMainPage, PageCount = w_nPageCount, Pages = w_strPages });
                string json = JsonConvert.SerializeObject(w_DeviceInfo.ToArray());
                System.IO.File.WriteAllText(filePath, json);
                makeAssets(Path.Combine(path, "assets"));
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
        public void saveAllPages(string path)
        {
            saveProject(path);
            for (int i = 0; i < m_lstWorkScreen.Count; i++)
            {
                savePage(path, i);
            }
        }
        public void savePage(string path)
        {
            if (IsProjectView)//save_all
            {
                saveAllPages(strProjectPath);
            }
            else
            {
                //SmallScreenID
                saveProject(path);
                savePage(path, SmallScreenID);
            }
        }
        public void savePage(string path, int id)
        {
            //SmallScreenID
            if (id < 0) return;
            string filePath = path + "\\" + m_lstWorkScreen[id].m_strName + ".dspage";
            try
            {
                List<Object> w_ControlInfo = new List<Object>();
                List<PageInfo> w_PageInfo = new List<PageInfo>();

                makeControl(w_ControlInfo, m_lstWorkScreen[id].screenCanvas, 3);
                w_PageInfo.Add(new PageInfo { Orientation = m_lstWorkScreen[id].m_Orientation, size = new stSize(m_lstWorkScreen[id].m_Size.Width, m_lstWorkScreen[id].m_Size.Height), background = m_lstWorkScreen[id].m_background, Opacity = m_lstWorkScreen[id].m_Opacity, Contents = w_ControlInfo });

                string json = JsonConvert.SerializeObject(w_PageInfo.ToArray());
                System.IO.File.WriteAllText(filePath, json);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        public List<Object> NodeToList(List<Node> nodes)
        {
            List<Object> list = new List<Object>();
            foreach (Node node in nodes)
            {
                list.Add(new TreeItemInfo
                {
                    item = new CustomItem
                    {
                        text = node.text,
                        Visible = node.Visible,
                        iteration = node.iteration,
                    },
                    Items = NodeToList(node.SubItems.ToList())
                });
            }
            return list;
        }
        public void makeControl(List<Object> controlInfo, Canvas canvas, int startChild = 0)
        {
            for (int i = 0; i < canvas.Children.Count - startChild; i++)
            {
                UIControl w_UIControl = (UIControl)canvas.Children[i + startChild];
                string w_imgSrc = null;
                if (w_UIControl.m_strSrc != null)
                    w_imgSrc = Path.Combine("assets", Path.GetFileName(w_UIControl.m_strSrc));
                if (w_UIControl.m_nUIControlType == CONTROL_TYPE.DROPDOWN)
                {
                    int index = 0; // Index of the item you want to get the height for
                    ListBoxItem listBoxItem = (ListBoxItem)((DropDownControl)w_UIControl).listBox.ContainerFromIndex(index);
                    int itemHeight = (int)listBoxItem.Bounds.Height;

                    controlInfo.Add(new ListControlInfo
                    {
                        Name = w_UIControl.m_strName,
                        Index = w_UIControl.m_nIndex,
                        Type = w_UIControl.m_nUIControlType,
                        Text = w_UIControl.m_strText,
                        Opacity = w_UIControl.m_Opacity,
                        x = (int)w_UIControl.m_nPositionX,
                        y = (int)w_UIControl.m_nPositionY,
                        w = (int)w_UIControl.m_nWidth,
                        h = (int)w_UIControl.m_nHeight,
                        isFitWidth = w_UIControl.m_bFitWidth,
                        isFitHeight = w_UIControl.m_bFitHeight,
                        Tooltip = w_UIControl.m_Tooltip,
                        Background = w_UIControl.m_Background,
                        Foreground = w_UIControl.m_Foreground,
                        BorderColor = w_UIControl.m_BorderColor,
                        BorderThickness = new stRect(w_UIControl.m_BorderThickness),
                        BorderRound = new stRect(w_UIControl.m_BorderRound),
                        fontFamily = w_UIControl.m_FontFamily.Name,
                        fontSize = w_UIControl.m_nFontSize,
                        isBold = w_UIControl.m_bBold,
                        isItalic = w_UIControl.m_bItalic,
                        isDisable = w_UIControl.m_bDisable,
                        src = w_imgSrc,
                        TapEvent = w_UIControl.m_TapEvent,
                        DTapEvent = w_UIControl.m_DTapEvent,
                        HPressEvent = w_UIControl.m_HPressEvent,
                        SwipeLeftEvent = w_UIControl.m_SwipeLeftEvent,
                        SwipeRightEvent = w_UIControl.m_SwipeRightEvent,
                        SwipeUpEvent = w_UIControl.m_SwipeUpEvent,
                        SwipeDownEvent = w_UIControl.m_SwipeDownEvent,
                        Items = ((ListBoxViewModel)w_UIControl.m_ControlViewModel).Items.ToList(),
                        SeletedIndex = ((DropDownControl)w_UIControl).listBox.SelectedIndex,
                        ItemBackground = ((ListBoxViewModel)w_UIControl.m_ControlViewModel).itemBackground,
                        itemHeight = itemHeight

                    });
                    continue;
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.LISTBOX)
                {
                    int index = 0; // Index of the item you want to get the height for
                    ListBoxItem listBoxItem = (ListBoxItem)((ListBoxControl)w_UIControl).listBox.ContainerFromIndex(index);
                    int itemHeight = (int)listBoxItem.Bounds.Height;
                    controlInfo.Add(new ListControlInfo
                    {
                        Name = w_UIControl.m_strName,
                        Index = w_UIControl.m_nIndex,
                        Type = w_UIControl.m_nUIControlType,
                        Text = w_UIControl.m_strText,
                        Opacity = w_UIControl.m_Opacity,
                        x = (int)w_UIControl.m_nPositionX,
                        y = (int)w_UIControl.m_nPositionY,
                        w = (int)w_UIControl.m_nWidth,
                        h = (int)w_UIControl.m_nHeight,
                        isFitWidth = w_UIControl.m_bFitWidth,
                        isFitHeight = w_UIControl.m_bFitHeight,
                        Tooltip = w_UIControl.m_Tooltip,
                        Background = w_UIControl.m_Background,
                        Foreground = w_UIControl.m_Foreground,
                        BorderColor = w_UIControl.m_BorderColor,
                        BorderThickness = new stRect(w_UIControl.m_BorderThickness),
                        BorderRound = new stRect(w_UIControl.m_BorderRound),
                        fontFamily = w_UIControl.m_FontFamily.Name,
                        fontSize = w_UIControl.m_nFontSize,
                        isBold = w_UIControl.m_bBold,
                        isItalic = w_UIControl.m_bItalic,
                        isDisable = w_UIControl.m_bDisable,
                        src = w_imgSrc,
                        TapEvent = w_UIControl.m_TapEvent,
                        DTapEvent = w_UIControl.m_DTapEvent,
                        HPressEvent = w_UIControl.m_HPressEvent,
                        SwipeLeftEvent = w_UIControl.m_SwipeLeftEvent,
                        SwipeRightEvent = w_UIControl.m_SwipeRightEvent,
                        SwipeUpEvent = w_UIControl.m_SwipeUpEvent,
                        SwipeDownEvent = w_UIControl.m_SwipeDownEvent,
                        Items = ((ListBoxViewModel)w_UIControl.m_ControlViewModel).Items.ToList(),
                        SeletedIndex = ((ListBoxControl)w_UIControl).listBox.SelectedIndex,
                        ItemBackground = ((ListBoxViewModel)w_UIControl.m_ControlViewModel).itemBackground,
                        itemHeight = itemHeight


                    });
                    continue;
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.TREEVIEW)
                {
                    int index = 0; // Index of the item you want to get the height for
                    TreeViewItem treeViewItem = (TreeViewItem)((TreeViewControl)w_UIControl).treeView.ContainerFromIndex(0);
                    //((TreeViewControl)w_UIControl).treeView.CollapseSubTree(treeViewItem);

                    TreeViewItem treeViewItem1 = (TreeViewItem)((TreeViewControl)w_UIControl).treeView.ContainerFromIndex(0);

                    int itemHeight = 46;// (int)treeViewItem1.MinHeight;
                    controlInfo.Add(new TreeViewControlInfo
                    {
                        Name = w_UIControl.m_strName,
                        Index = w_UIControl.m_nIndex,
                        Type = w_UIControl.m_nUIControlType,
                        Text = w_UIControl.m_strText,
                        Opacity = w_UIControl.m_Opacity,
                        x = (int)w_UIControl.m_nPositionX,
                        y = (int)w_UIControl.m_nPositionY,
                        w = (int)w_UIControl.m_nWidth,
                        h = (int)w_UIControl.m_nHeight,
                        isFitWidth = w_UIControl.m_bFitWidth,
                        isFitHeight = w_UIControl.m_bFitHeight,
                        Tooltip = w_UIControl.m_Tooltip,
                        Background = w_UIControl.m_Background,
                        Foreground = w_UIControl.m_Foreground,
                        BorderColor = w_UIControl.m_BorderColor,
                        BorderThickness = new stRect(w_UIControl.m_BorderThickness),
                        BorderRound = new stRect(w_UIControl.m_BorderRound),
                        fontFamily = w_UIControl.m_FontFamily.Name,
                        fontSize = w_UIControl.m_nFontSize,
                        isBold = w_UIControl.m_bBold,
                        isItalic = w_UIControl.m_bItalic,
                        isDisable = w_UIControl.m_bDisable,
                        src = w_imgSrc,
                        TapEvent = w_UIControl.m_TapEvent,
                        DTapEvent = w_UIControl.m_DTapEvent,
                        HPressEvent = w_UIControl.m_HPressEvent,
                        SwipeLeftEvent = w_UIControl.m_SwipeLeftEvent,
                        SwipeRightEvent = w_UIControl.m_SwipeRightEvent,
                        SwipeUpEvent = w_UIControl.m_SwipeUpEvent,
                        SwipeDownEvent = w_UIControl.m_SwipeDownEvent,
                        Items = NodeToList(((TreeViewViewModel)w_UIControl.m_ControlViewModel).Items.ToList()),
                        ItemBackground = ((TreeViewViewModel)w_UIControl.m_ControlViewModel).itemBackground,
                        itemHeight = itemHeight


                    });
                    continue;
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.CONTAINERBOX)
                {
                    List<Object> canvasInfo = new List<Object>();
                    makeControl(canvasInfo, ((ContainerBoxControl)w_UIControl).container);
                    controlInfo.Add(new ContainterControlInfo
                    {
                        Name = w_UIControl.m_strName,
                        Index = w_UIControl.m_nIndex,
                        Type = w_UIControl.m_nUIControlType,
                        Text = w_UIControl.m_strText,
                        Opacity = w_UIControl.m_Opacity,
                        x = (int)w_UIControl.m_nPositionX,
                        y = (int)w_UIControl.m_nPositionY,
                        w = (int)w_UIControl.m_nWidth,
                        h = (int)w_UIControl.m_nHeight,
                        isFitWidth = w_UIControl.m_bFitWidth,
                        isFitHeight = w_UIControl.m_bFitHeight,
                        Tooltip = w_UIControl.m_Tooltip,
                        Background = w_UIControl.m_Background,
                        Foreground = w_UIControl.m_Foreground,
                        BorderColor = w_UIControl.m_BorderColor,
                        BorderThickness = new stRect(w_UIControl.m_BorderThickness),
                        BorderRound = new stRect(w_UIControl.m_BorderRound),
                        fontFamily = w_UIControl.m_FontFamily.Name,
                        fontSize = w_UIControl.m_nFontSize,
                        isBold = w_UIControl.m_bBold,
                        isItalic = w_UIControl.m_bItalic,
                        isDisable = w_UIControl.m_bDisable,
                        src = w_imgSrc,
                        TapEvent = w_UIControl.m_TapEvent,
                        DTapEvent = w_UIControl.m_DTapEvent,
                        HPressEvent = w_UIControl.m_HPressEvent,
                        SwipeLeftEvent = w_UIControl.m_SwipeLeftEvent,
                        SwipeRightEvent = w_UIControl.m_SwipeRightEvent,
                        SwipeUpEvent = w_UIControl.m_SwipeUpEvent,
                        SwipeDownEvent = w_UIControl.m_SwipeDownEvent,
                        Items = canvasInfo
                    });
                    continue;
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.REPEATER)
                {
                    List<Object> repeaterInfo = new List<Object>();
                    ObservableCollection<ContainerBoxControl> items = ((RepeaterControlViewModel)w_UIControl.m_ControlViewModel).Items;

                    foreach (ContainerBoxControl item in items)
                    {
                        List<Object> canvasInfo = new List<Object>();
                        makeControl(canvasInfo, ((ContainerBoxControl)item).container);
                        repeaterInfo.Add(new ContainterControlInfo
                        {
                            Name = item.m_strName,
                            Index = item.m_nIndex,
                            Type = item.m_nUIControlType,
                            Text = item.m_strText,
                            Opacity = item.m_Opacity,
                            x = (int)item.m_nPositionX,
                            y = (int)item.m_nPositionY,
                            w = (int)item.m_nWidth,
                            h = (int)item.m_nHeight,
                            isFitWidth = item.m_bFitWidth,
                            isFitHeight = item.m_bFitHeight,
                            Tooltip = item.m_Tooltip,
                            Background = item.m_Background,
                            Foreground = item.m_Foreground,
                            BorderColor = item.m_BorderColor,
                            BorderThickness = new stRect(item.m_BorderThickness),
                            BorderRound = new stRect(item.m_BorderRound),
                            fontFamily = item.m_FontFamily.Name,
                            fontSize = item.m_nFontSize,
                            isBold = item.m_bBold,
                            isItalic = item.m_bItalic,
                            isDisable = item.m_bDisable,
                            src = w_imgSrc,
                            TapEvent = item.m_TapEvent,
                            DTapEvent = item.m_DTapEvent,
                            HPressEvent = item.m_HPressEvent,
                            SwipeLeftEvent = item.m_SwipeLeftEvent,
                            SwipeRightEvent = item.m_SwipeRightEvent,
                            SwipeUpEvent = item.m_SwipeUpEvent,
                            SwipeDownEvent = item.m_SwipeDownEvent,
                            Items = canvasInfo
                        });
                        continue;
                    }

                    controlInfo.Add(new ContainterControlInfo
                    {
                        Name = w_UIControl.m_strName,
                        Index = w_UIControl.m_nIndex,
                        Type = w_UIControl.m_nUIControlType,
                        Text = w_UIControl.m_strText,
                        Opacity = w_UIControl.m_Opacity,
                        x = (int)w_UIControl.m_nPositionX,
                        y = (int)w_UIControl.m_nPositionY,
                        w = (int)w_UIControl.m_nWidth,
                        h = (int)w_UIControl.m_nHeight,
                        isFitWidth = w_UIControl.m_bFitWidth,
                        isFitHeight = w_UIControl.m_bFitHeight,
                        Tooltip = w_UIControl.m_Tooltip,
                        Background = w_UIControl.m_Background,
                        Foreground = w_UIControl.m_Foreground,
                        BorderColor = w_UIControl.m_BorderColor,
                        BorderThickness = new stRect(w_UIControl.m_BorderThickness),
                        BorderRound = new stRect(w_UIControl.m_BorderRound),
                        fontFamily = w_UIControl.m_FontFamily.Name,
                        fontSize = w_UIControl.m_nFontSize,
                        isBold = w_UIControl.m_bBold,
                        isItalic = w_UIControl.m_bItalic,
                        isDisable = w_UIControl.m_bDisable,
                        src = w_imgSrc,
                        TapEvent = w_UIControl.m_TapEvent,
                        DTapEvent = w_UIControl.m_DTapEvent,
                        HPressEvent = w_UIControl.m_HPressEvent,
                        SwipeLeftEvent = w_UIControl.m_SwipeLeftEvent,
                        SwipeRightEvent = w_UIControl.m_SwipeRightEvent,
                        SwipeUpEvent = w_UIControl.m_SwipeUpEvent,
                        SwipeDownEvent = w_UIControl.m_SwipeDownEvent,
                        Items = repeaterInfo
                    });
                }
                else if (w_UIControl.m_nUIControlType == CONTROL_TYPE.TABS)
                {
                    List<Object> repeaterInfo = new List<Object>();
                    ObservableCollection<ContainerBoxControl> items = ((RepeaterControlViewModel)w_UIControl.m_ControlViewModel).Items;

                    foreach (ContainerBoxControl item in items)
                    {
                        List<Object> canvasInfo = new List<Object>();
                        makeControl(canvasInfo, ((ContainerBoxControl)item).container);
                        repeaterInfo.Add(new ContainterControlInfo
                        {
                            Name = item.m_strName,
                            Index = item.m_nIndex,
                            Type = item.m_nUIControlType,
                            Text = item.m_strText,
                            Opacity = item.m_Opacity,
                            x = (int)item.m_nPositionX,
                            y = (int)item.m_nPositionY,
                            w = (int)item.m_nWidth,
                            h = (int)item.m_nHeight,
                            isFitWidth = item.m_bFitWidth,
                            isFitHeight = item.m_bFitHeight,
                            Tooltip = item.m_Tooltip,
                            Background = item.m_Background,
                            Foreground = item.m_Foreground,
                            BorderColor = item.m_BorderColor,
                            BorderThickness = new stRect(item.m_BorderThickness),
                            BorderRound = new stRect(item.m_BorderRound),
                            fontFamily = item.m_FontFamily.Name,
                            fontSize = item.m_nFontSize,
                            isBold = item.m_bBold,
                            isItalic = item.m_bItalic,
                            isDisable = item.m_bDisable,
                            src = w_imgSrc,
                            TapEvent = item.m_TapEvent,
                            DTapEvent = item.m_DTapEvent,
                            HPressEvent = item.m_HPressEvent,
                            SwipeLeftEvent = item.m_SwipeLeftEvent,
                            SwipeRightEvent = item.m_SwipeRightEvent,
                            SwipeUpEvent = item.m_SwipeUpEvent,
                            SwipeDownEvent = item.m_SwipeDownEvent,
                            Items = canvasInfo
                        });
                        continue;
                    }

                    controlInfo.Add(new TabControlInfo
                    {
                        Name = w_UIControl.m_strName,
                        Index = w_UIControl.m_nIndex,
                        Type = w_UIControl.m_nUIControlType,
                        Text = w_UIControl.m_strText,
                        Opacity = w_UIControl.m_Opacity,
                        x = (int)w_UIControl.m_nPositionX,
                        y = (int)w_UIControl.m_nPositionY,
                        w = (int)w_UIControl.m_nWidth,
                        h = (int)w_UIControl.m_nHeight,
                        isFitWidth = w_UIControl.m_bFitWidth,
                        isFitHeight = w_UIControl.m_bFitHeight,
                        Tooltip = w_UIControl.m_Tooltip,
                        Background = w_UIControl.m_Background,
                        Foreground = w_UIControl.m_Foreground,
                        BorderColor = w_UIControl.m_BorderColor,
                        BorderThickness = new stRect(w_UIControl.m_BorderThickness),
                        BorderRound = new stRect(w_UIControl.m_BorderRound),
                        fontFamily = w_UIControl.m_FontFamily.Name,
                        fontSize = w_UIControl.m_nFontSize,
                        isBold = w_UIControl.m_bBold,
                        isItalic = w_UIControl.m_bItalic,
                        isDisable = w_UIControl.m_bDisable,
                        src = w_imgSrc,
                        TapEvent = w_UIControl.m_TapEvent,
                        DTapEvent = w_UIControl.m_DTapEvent,
                        HPressEvent = w_UIControl.m_HPressEvent,
                        SwipeLeftEvent = w_UIControl.m_SwipeLeftEvent,
                        SwipeRightEvent = w_UIControl.m_SwipeRightEvent,
                        SwipeUpEvent = w_UIControl.m_SwipeUpEvent,
                        SwipeDownEvent = w_UIControl.m_SwipeDownEvent,
                        Items = repeaterInfo,
                        Headers = ((RepeaterControlViewModel)w_UIControl.m_ControlViewModel).TabHeaders.ToList(),
                        SeletedIndex = ((TabViewControl)w_UIControl).tabControl.SelectedIndex
                    });
                }
                else
                {
                    controlInfo.Add(new ControlInfo
                    {
                        Name = w_UIControl.m_strName,
                        Index = w_UIControl.m_nIndex,
                        Type = w_UIControl.m_nUIControlType,
                        Text = w_UIControl.m_strText,
                        Opacity = w_UIControl.m_Opacity,
                        x = (int)w_UIControl.m_nPositionX,
                        y = (int)w_UIControl.m_nPositionY,
                        w = (int)w_UIControl.m_nWidth,
                        h = (int)w_UIControl.m_nHeight,
                        isFitWidth = w_UIControl.m_bFitWidth,
                        isFitHeight = w_UIControl.m_bFitHeight,
                        Tooltip = w_UIControl.m_Tooltip,
                        Background = w_UIControl.m_Background,
                        Foreground = w_UIControl.m_Foreground,
                        BorderColor = w_UIControl.m_BorderColor,
                        BorderThickness = new stRect(w_UIControl.m_BorderThickness),
                        BorderRound = new stRect(w_UIControl.m_BorderRound),
                        fontFamily = w_UIControl.m_FontFamily.Name,
                        fontSize = w_UIControl.m_nFontSize,
                        isBold = w_UIControl.m_bBold,
                        isItalic = w_UIControl.m_bItalic,
                        isDisable = w_UIControl.m_bDisable,
                        src = w_imgSrc,
                        TapEvent = w_UIControl.m_TapEvent,
                        DTapEvent = w_UIControl.m_DTapEvent,
                        HPressEvent = w_UIControl.m_HPressEvent,
                        SwipeLeftEvent = w_UIControl.m_SwipeLeftEvent,
                        SwipeRightEvent = w_UIControl.m_SwipeRightEvent,
                        SwipeUpEvent = w_UIControl.m_SwipeUpEvent,
                        SwipeDownEvent = w_UIControl.m_SwipeDownEvent
                    });
                }


            }
        }
    }
    public class RecentProject
    {
        public string Name { get; set; }
        public DateTime LastOpened { get; set; }
    }
    public class TabItemModel
    {
        public string Header { get; }
        public string Content { get; }
        public TabItemModel(string header, string content)
        {
            Header = header;
            Content = content;
        }
    }
}