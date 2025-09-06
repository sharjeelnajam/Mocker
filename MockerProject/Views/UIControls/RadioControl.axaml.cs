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

            m_ControlViewModel.IsBorderVisible = true;
            m_ControlViewModel.IsBackgroundVisible = true;
            m_ControlViewModel.IsBorderColorVisible = true;
        }
    }
}