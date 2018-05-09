using ListAuthorsADONetWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static ListAuthorsADONetWPF.MainWindow;

namespace ListAuthorsADONetWPF.Presenters
{
    class MainWindowPresenter : IPresenterMainWindow
    {
        private DbLibrary _model;
        private IViewMainWindow _view;

        public MainWindowPresenter(IViewMainWindow view)
        {
            _view = view;
            _model = new DbLibrary();
            _view.MyListView.ItemsSource = _model.GetList();
        }

        public void AddAuthor()
        {
            additionalWindow window = new additionalWindow();
            bool? res = window.ShowDialog();

            if (res != null && res == true)
            {
                Author author = window.Author;
                _model.AddAuthor(author);
            }
            _view.MyListView.ItemsSource = _model.GetList();
        }

        public void DeleteAuthor()
        {
            MessageBoxResult res = MessageBox.Show("Are u sure?", "Really?", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                _model.DeleteAuthor(_view.GetSelecetedAuthor());
            }
            _view.MyListView.ItemsSource = _model.GetList();
        }

        public void EditAuthor()
        {
            Author oldAuthor = new Author(_view.GetSelecetedAuthor().FirstName, _view.GetSelecetedAuthor().LastName);
            additionalWindow window = new additionalWindow(_view.GetSelecetedAuthor());
            bool? res = window.ShowDialog();

            if (res != null && res == true)
            {
                _model.EditAuthor(oldAuthor, window.Author);
            }
            _view.MyListView.ItemsSource = _model.GetList();
        }
    }
}
