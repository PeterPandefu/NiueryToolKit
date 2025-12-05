using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiueryToolKit.Base.Interface
{
    public interface IViewOperator
    {
        public void ShowWindow(IMVVMController controller);

        public void Message(string message, string tiele);

        public void SwitchLanguage();

        public string[] OpenFolder();
        public string[] OpenFile();
    }
}
