namespace MockerProject.Views
{
    public partial class RadioControl : UIControl
    {
        public RadioControl()
        {
            InitializeComponent();
            this.DataContext = m_ControlViewModel;
            setSize(100, 30);
            setName("Radio");
            setFontSize(14);
            setText("Option");
            setFontSizeID(7);

            m_ControlViewModel.IsBorderVisible = false;
            m_ControlViewModel.IsBackgroundVisible = false;
            m_ControlViewModel.IsBorderColorVisible = false;
        }
    }
}