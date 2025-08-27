namespace MockerProject.Views.UIControls
{
    public partial class CheckControl : UIControl
    {
        public CheckControl()
        {
            InitializeComponent();
            this.DataContext = m_ControlViewModel;
            setSize(100, 30);
            setName("Check");
            setFontSize(14);
            setText("Check");
        }
    }
}