using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DuoPinballCore9.Form1;
using System.Windows.Forms;

namespace DuoPinballCore9
{
    public class Log : ILog
    {
        private static BindingSource _logitems = new BindingSource();

        public static BindingSource LogItems
        {
            get
            {
                return _logitems;
            }
            private set
            {
                _logitems = value;
            }
        }

        public static void StaticAdd(string message)
        {

            LogItems.Add(new LogItem(message));


        }

        public void Add(string message)
        {

            LogItems.Add(new LogItem(message));


        }
    }
}
