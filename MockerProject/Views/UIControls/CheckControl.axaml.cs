using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System.Linq;

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

            var stackPanel = this.FindControl<StackPanel>("check");
            if (stackPanel != null)
            {
                var checkBox = stackPanel.GetVisualDescendants()
                                         .OfType<CheckBox>()
                                         .FirstOrDefault();
                if (checkBox != null)
                {
                    checkBox.TemplateApplied += (_, __) =>
                    {
                        var labelPresenter = checkBox.GetTemplateChildren()
                                                     .OfType<ContentPresenter>()
                                                     .FirstOrDefault(cp => cp.Name == "PART_TextPresenter");

                        if (labelPresenter != null)
                        {
                            labelPresenter.AddHandler(PointerPressedEvent, (sender, e) =>
                            {
                                this.RaiseEvent(e);
                            }, RoutingStrategies.Bubble);
                        }
                    };
                }
            }
        }
    }
}