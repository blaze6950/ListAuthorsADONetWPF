using ListAuthorsADONetWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static ListAuthorsADONetWPF.MainWindow;

namespace ListAuthorsADONetWPF
{
    interface IViewMainWindow
    {
        ListView MyListView
        {
            get;
            set;            
        }

        Author GetSelecetedAuthor();
    }
}
