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
    public partial class MainWindow : Window
    {
        public ListView myListView;
        public MainWindow()
        {
            InitializeComponent();
            myListView = new ListView();

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

            myListView.View = myGridView;
            dockPanel.Children.Add(myListView);
            myListView.ItemsSource = GetList();
            myListView.SelectionChanged += MyListView_SelectionChanged;
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myListView.SelectedItem != null)
            {
                editButton.IsEnabled = true;
                deleteButton.IsEnabled = true;
            }
        }

        public AuthorsList GetList()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "NIKITOS";
            builder.InitialCatalog = "Library";
            builder.IntegratedSecurity = true;

            AuthorsList authors = new AuthorsList();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Opened");               

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Authors";

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    authors.Add(new Author((String)reader["FirstName"], (String)reader["LastName"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ooops", MessageBoxButton.OK);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return authors;
        }

        public class Author
        {
            private String _firstName;
            private String _lastName;

            public Author(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public string FirstName { get => _firstName; set => _firstName = value; }
            public string LastName { get => _lastName; set => _lastName = value; }
        }

        public class AuthorsList : ObservableCollection<Author>
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "addButton":
                    AddAuthor();
                    break;
                case "editButton":
                    EditAuthor();
                    break;
                case "deleteButton":
                    DeleteAuthor();
                    break;
                default:
                    break;
            }
        }

        public void AddAuthor()
        {
            additionalWindow window = new additionalWindow();
            bool? res = window.ShowDialog();

            if (res != null && res == true)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "NIKITOS";
                builder.InitialCatalog = "Library";
                builder.IntegratedSecurity = true;

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                try
                {
                    connection.Open();
                    Console.WriteLine("Opened");

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO Authors(FirstName, LastName) VALUES (@FirstName, @LastName)";

                    SqlParameter firstNameParam = command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 15);

                    SqlParameter lastNameParam = command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 25);

                    firstNameParam.Value = window.Author.FirstName;
                    lastNameParam.Value = window.Author.LastName;

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ooops", MessageBoxButton.OK);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            myListView.ItemsSource = GetList();
        }

        public void EditAuthor()
        {
            Author oldAuthor = new Author(GetSelecetedAuthor().FirstName, GetSelecetedAuthor().LastName);
            additionalWindow window = new additionalWindow(GetSelecetedAuthor());
            bool? res = window.ShowDialog();

            if (res != null && res == true)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "NIKITOS";
                builder.InitialCatalog = "Library";
                builder.IntegratedSecurity = true;                

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                try
                {
                    connection.Open();
                    Console.WriteLine("Opened");

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Authors SET FirstName = @newFirstName, LastName = @newLastName WHERE FirstName = @FirstName AND LastName = @LastName";

                    SqlParameter firstNameParam = command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 15);

                    SqlParameter lastNameParam = command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 25);

                    SqlParameter newFirstNameParam = command.Parameters.Add("@newFirstName", System.Data.SqlDbType.NVarChar, 15);

                    SqlParameter newLastNameParam = command.Parameters.Add("@newLastName", System.Data.SqlDbType.NVarChar, 25);

                    firstNameParam.Value = oldAuthor.FirstName;
                    lastNameParam.Value = oldAuthor.LastName;

                    newFirstNameParam.Value = window.Author.FirstName;
                    newLastNameParam.Value = window.Author.LastName;

                    command.ExecuteNonQuery();                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ooops", MessageBoxButton.OK);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            myListView.ItemsSource = GetList();
        }

        public void DeleteAuthor()
        {
            MessageBoxResult res = MessageBox.Show("Are u sure?", "Really?", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "NIKITOS";
                builder.InitialCatalog = "Library";
                builder.IntegratedSecurity = true;

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                try
                {
                    connection.Open();
                    Console.WriteLine("Opened");

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM Authors WHERE FirstName = @FirstName AND LastName = @LastName";

                    SqlParameter firstNameParam = command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 15);

                    SqlParameter lastNameParam = command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 25);
                    
                    firstNameParam.Value = GetSelecetedAuthor().FirstName;
                    lastNameParam.Value = GetSelecetedAuthor().LastName;

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ooops", MessageBoxButton.OK);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            myListView.ItemsSource = GetList();
        }

        public Author GetSelecetedAuthor()
        {
            Author author = (Author)myListView.SelectedItem;
            return author;
        }
    }
}
