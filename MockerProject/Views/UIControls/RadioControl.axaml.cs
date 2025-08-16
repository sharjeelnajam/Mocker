using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System.Linq;

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

            var stackPanel = this.FindControl<StackPanel>("radio");
            if (stackPanel != null)
            {
                var radioButton = stackPanel.GetVisualDescendants()
                                            .OfType<RadioButton>()
                                            .FirstOrDefault();
                if (radioButton != null)
                {
                    radioButton.TemplateApplied += (_, __) =>
                    {
                        var labelPresenter = radioButton.GetTemplateChildren()
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