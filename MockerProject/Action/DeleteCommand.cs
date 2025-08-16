using Avalonia.Controls;

namespace MockerProject.Action
{
    internal class DeleteCommand : IAction
    {
        private Panel _canvas;
        private Control _element;

        public DeleteCommand(Panel canvas, Control element)
        {
            _canvas = canvas;
            _element = element;
        }

        public void Execute()
        {
            // Safe removal that works with any type of parent container
            if (_element.Parent is Panel panel)
            {
                panel.Children.Remove(_element);
            }
            else if (_element.Parent is Decorator decorator)
            {
                decorator.Child = null;
            }
            else if (_element.Parent is ContentControl contentControl)
            {
                contentControl.Content = null;
            }
            else if (_element.Parent != null)
            {
                // Fallback for other types of parents
                var parentProp = _element.Parent.GetType().GetProperty("Children");
                parentProp?.GetValue(_element.Parent)?.GetType()
                    .GetMethod("Remove")?
                    .Invoke(parentProp.GetValue(_element.Parent), new object[] { _element });
            }

            _canvas.Children.Remove(_element);
        }

        public void UnExecute()
        {
            _canvas.Children.Add(_element);
        }
    }
}
