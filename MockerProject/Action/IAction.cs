using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockerProject.Action
{
    public interface IAction
    {
        void Execute();
        void UnExecute();
    }
}
