using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
          //  _element.contro
            _canvas.Children.Remove(_element);
        }

        public void UnExecute()
        {
            _canvas.Children.Add(_element);
        }
    }
}
