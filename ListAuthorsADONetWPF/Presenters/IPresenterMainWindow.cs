using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListAuthorsADONetWPF.Presenters
{
    interface IPresenterMainWindow
    {
        void AddAuthor();
        void EditAuthor();
        void DeleteAuthor();
    }
}
