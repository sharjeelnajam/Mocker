using Avalonia.Controls;

namespace MockerProject.Views.UIControls
{
    public partial class RepeaterRunControl : UserControl
    {
        public RepeaterRunControl()
        {
            InitializeComponent();
            Canvas canvas = new Canvas();
            Canvas.SetLeft(canvas, 0);
            Canvas.SetTop(canvas, 0);
            Canvas.SetRight(canvas, 100);
            Canvas.SetBottom(canvas, 100);
        }
    }
}
