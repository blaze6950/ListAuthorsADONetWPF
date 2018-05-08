using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static ListAuthorsADONetWPF.MainWindow;

namespace ListAuthorsADONetWPF
{
    /// <summary>
    /// Interaction logic for additionalWindow.xaml
    /// </summary>
    public partial class additionalWindow : Window
    {
        private Author _author;
        public additionalWindow()
        {
            InitializeComponent();
        }

        public additionalWindow(Author author)
        {            
            InitializeComponent();
            Author = author;
        }

        public Author Author { get => _author;
            set
            {
                _author = value;
                firstNameTB.Text = _author.FirstName;
                lastNameTB.Text = _author.LastName;
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "okButton":
                    PressedOKButton();
                    break;
                case "cancelButton":
                    PressedCancelButton();
                    break;               
                default:
                    break;
            }
        }

        private void PressedOKButton()
        {            
            if (firstNameTB.Text.Count() > 0 && lastNameTB.Text.Count() > 0)
            {
                if (_author == null)
                {
                    _author = new Author(firstNameTB.Text, lastNameTB.Text);
                }
                else
                {
                    _author.FirstName = firstNameTB.Text;
                    _author.LastName = lastNameTB.Text;
                }
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please, fill all fields", "Ooops", MessageBoxButton.OK);
            }
        }

        private void PressedCancelButton()
        {
            DialogResult = false;
            this.Close();
        }
    }
}
