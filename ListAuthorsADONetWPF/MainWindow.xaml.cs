using ListAuthorsADONetWPF.Model;
using ListAuthorsADONetWPF.Presenters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ListAuthorsADONetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewMainWindow
    {
        private IPresenterMainWindow _presenter;
        private ListView _myListView;

        public ListView MyListView {
            get {
                return _myListView;
            }
            set {
                _myListView = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            _myListView = new ListView();

            GridView myGridView = new GridView();
            myGridView.AllowsColumnReorder = true;
            myGridView.ColumnHeaderToolTip = "Authors Information";

            GridViewColumn gvc1 = new GridViewColumn();
            gvc1.DisplayMemberBinding = new Binding("FirstName");
            gvc1.Header = "FirstName";
            gvc1.Width = 100;
            myGridView.Columns.Add(gvc1);

            GridViewColumn gvc2 = new GridViewColumn();
            gvc2.DisplayMemberBinding = new Binding("LastName");
            gvc2.Header = "Last Name";
            gvc2.Width = 100;
            myGridView.Columns.Add(gvc2);

            _myListView.View = myGridView;
            dockPanel.Children.Add(_myListView);
            _myListView.SelectionChanged += MyListView_SelectionChanged;

            _presenter = new MainWindowPresenter(this);
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_myListView.SelectedItem != null)
            {
                editButton.IsEnabled = true;
                deleteButton.IsEnabled = true;
            }
            else
            {
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
            }
        }        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "addButton":
                    _presenter.AddAuthor();
                    break;
                case "editButton":
                    _presenter.EditAuthor();
                    break;
                case "deleteButton":
                    _presenter.DeleteAuthor();
                    break;
                default:
                    break;
            }
        }

        public Author GetSelecetedAuthor()
        {
            Author author = (Author)_myListView.SelectedItem;
            return author;
        }                
    }
}
