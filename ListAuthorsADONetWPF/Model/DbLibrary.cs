﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static ListAuthorsADONetWPF.MainWindow;

namespace ListAuthorsADONetWPF.Model
{
    class DbLibrary : IDisposable
    {
        private String _connectionString = null;
        private SqlConnection _connection = null;

        public DbLibrary()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "NIKITOS";
            builder.InitialCatalog = "Library";
            builder.IntegratedSecurity = true;

            _connectionString = builder.ConnectionString;
            _connection = new SqlConnection(_connectionString);
            try
            {
                _connection.Open();                
                MessageBox.Show("Connection opened", "Successfully", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ooops", MessageBoxButton.OK);
            }            
        }

        public AuthorsList GetList()
        {            
            AuthorsList authors = new AuthorsList();
            SqlDataReader reader = null;
            try
            {                
                SqlCommand command = _connection.CreateCommand();
                command.CommandText = "SELECT * FROM Authors";

                reader = command.ExecuteReader();
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
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return authors;
        }

        public void AddAuthor(Author author)
        {
            try
            {
                SqlCommand command = _connection.CreateCommand();
                command.CommandText = "INSERT INTO Authors(FirstName, LastName) VALUES (@FirstName, @LastName)";

                SqlParameter firstNameParam = command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 15);

                SqlParameter lastNameParam = command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 25);

                firstNameParam.Value = author.FirstName;
                lastNameParam.Value = author.LastName;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ooops", MessageBoxButton.OK);
            }
        }

        public void EditAuthor(Author oldAuthor, Author newAuthor)
        {
            try
            {
                SqlCommand command = _connection.CreateCommand();
                command.CommandText = "UPDATE Authors SET FirstName = @newFirstName, LastName = @newLastName WHERE FirstName = @FirstName AND LastName = @LastName";

                SqlParameter firstNameParam = command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 15);

                SqlParameter lastNameParam = command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 25);

                SqlParameter newFirstNameParam = command.Parameters.Add("@newFirstName", System.Data.SqlDbType.NVarChar, 15);

                SqlParameter newLastNameParam = command.Parameters.Add("@newLastName", System.Data.SqlDbType.NVarChar, 25);

                firstNameParam.Value = oldAuthor.FirstName;
                lastNameParam.Value = oldAuthor.LastName;

                newFirstNameParam.Value = newAuthor.FirstName;
                newLastNameParam.Value = newAuthor.LastName;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ooops", MessageBoxButton.OK);
            }
        }

        public void DeleteAuthor(Author selectedAuthor)
        {
            try
            {
                SqlCommand command = _connection.CreateCommand();
                command.CommandText = "DELETE FROM Authors WHERE FirstName = @FirstName AND LastName = @LastName";

                SqlParameter firstNameParam = command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 15);

                SqlParameter lastNameParam = command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 25);

                firstNameParam.Value = selectedAuthor.FirstName;
                lastNameParam.Value = selectedAuthor.LastName;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ooops", MessageBoxButton.OK);
            }
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
