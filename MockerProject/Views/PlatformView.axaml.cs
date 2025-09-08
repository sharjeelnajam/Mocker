using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MockerProject.Models;
using MockerProject.ViewModels;
using static MockerProject.ViewModels.MainWindowViewModel;

namespace MockerProject.Views
{
    public partial class PlatformView : UserControl
    {
        private MainWindowViewModel m_MainViewModel;
        public PlatformView()
        {
            InitializeComponent(); 

            colorButton.PropertyChanged += (s, e) =>
            {
                if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty)
                {
                    m_MainViewModel = (MainWindowViewModel)this.DataContext;
                    m_MainViewModel.m_PlatformView = this;
                    
                    // Store the old background color for undo
                    var oldBackground = m_MainViewModel.PageBackground;
                    
                    //Do something
                    Color w_Color = colorButton.Color;
                    if(w_Color.A != 255)
                        m_MainViewModel.PG_OPT = (double)w_Color.A / 255.0;
                    //colorButton.Color = new Color(255, w_Color.R, w_Color.G, w_Color.B);
                    SolidColorBrush w_IBrush = new SolidColorBrush(new Color(255, w_Color.R, w_Color.G, w_Color.B));
                    m_MainViewModel.PageBackground = w_IBrush;
                    
                    // Add background color change to undo list
                    if (m_MainViewModel.WorkScreen != null)
                    {
                        stControlHistory w_ControlHistory = new stControlHistory();
                        w_ControlHistory.Index = -1; // Special index for background color changes
                        w_ControlHistory.Cmd = "BackgroundColor";
                        w_ControlHistory.id = CONTROL_TYPE.NONE;
                        w_ControlHistory.type = typeof(SolidColorBrush);
                        w_ControlHistory.oldInfo = null;
                        w_ControlHistory.curInfo = null;
                        
                        // Store the background color change data directly in the history entry
                        w_ControlHistory.CustomData = new BackgroundColorChange
                        {
                            OldColor = oldBackground,
                            NewColor = w_IBrush
                        };
                        
                        m_MainViewModel.WorkScreen.m_UndoList.Add(w_ControlHistory);
                    }
                }
            };
        }
    }
}
