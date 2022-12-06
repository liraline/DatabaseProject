using System;
using System.Diagnostics;
using System.Data.SqlClient;
using DatabaseProject.Model;
using DatabaseProject.Utils;
using System.Collections.ObjectModel;

namespace DatabaseProject.Repository
{
    public static class CustomerDatabase
    {
        const string ConnectionString = @"Data Source=127.0.0.1,1433;Database=Northwind;User Id=sa;Password=Aluno@123;";

        public static void CreateCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand insertCustomerQuery = new SqlCommand("INSERT INTO Customers VALUES " +
                        "(@customerID, @companyName, @contactName, @contactTitle, @address, @city, @region, " +
                        "@postalCode, @country, @phone, @fax)", connection);

                    insertCustomerQuery.Parameters.AddWithValue("@customerID", customer.CustomerID);
                    insertCustomerQuery.Parameters.AddWithValue("@companyName", customer.CompanyName);
                    insertCustomerQuery.Parameters.AddWithValue("@contactName", customer.ContactName);
                    insertCustomerQuery.Parameters.AddWithValue("@contactTitle", customer.ContactTitle);
                    insertCustomerQuery.Parameters.AddWithValue("@address", customer.Address);
                    insertCustomerQuery.Parameters.AddWithValue("@city", customer.City);
                    insertCustomerQuery.Parameters.AddWithValue("@region", customer.Region);
                    insertCustomerQuery.Parameters.AddWithValue("@postalCode", customer.PostalCode);
                    insertCustomerQuery.Parameters.AddWithValue("@country", customer.Country);
                    insertCustomerQuery.Parameters.AddWithValue("@phone", customer.Phone);
                    insertCustomerQuery.Parameters.AddWithValue("@fax", customer.Fax);

                    insertCustomerQuery.ExecuteNonQuery();
                }
            }

        }

        public static Customer GetCustomer(string customerID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                Customer foundCustomer = null;

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand getCustomerQuery = new SqlCommand($"SELECT * FROM Customers WHERE CustomerID = '{customerID}'", connection);
                    SqlDataReader queryResponse = getCustomerQuery.ExecuteReader();

                    if (queryResponse.HasRows)
                    {
                        queryResponse.Read();

                        string customerRegion = queryResponse.IsDBNull(6) ? null : queryResponse.GetString(6);
                        string customerFax = queryResponse.IsDBNull(10) ? null : queryResponse.GetString(10);

                        foundCustomer = new Customer
                        {
                            CustomerID = queryResponse.GetString(0),
                            CompanyName = queryResponse.GetString(1),
                            ContactName = queryResponse.GetString(2),
                            ContactTitle = queryResponse.GetString(3),
                            Address = queryResponse.GetString(4),
                            City = queryResponse.GetString(5),
                            Region = customerRegion,
                            PostalCode = queryResponse.GetString(7),
                            Country = queryResponse.GetString(8),
                            Phone = queryResponse.GetString(9),
                            Fax = customerFax
                        };
                    }
                }

                return foundCustomer;
            }
        }

        public static ObservableCollection<Customer> GetAllCustomers()
        {
            ObservableCollection<Customer> customersList = new ObservableCollection<Customer>();
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand getCustomersQuery = new SqlCommand("SELECT * FROM Customers", connection);
                        SqlDataReader queryResponse = getCustomersQuery.ExecuteReader();

                        while (queryResponse.Read())
                        {
                            Customer customer = new Customer
                            {
                                CustomerID = NullChecker.CheckStringField(queryResponse, 0),
                                CompanyName = NullChecker.CheckStringField(queryResponse, 1),
                                ContactName = NullChecker.CheckStringField(queryResponse, 2),
                                ContactTitle = NullChecker.CheckStringField(queryResponse, 3),
                                Address = NullChecker.CheckStringField(queryResponse, 4),
                                City = NullChecker.CheckStringField(queryResponse, 5),
                                Region = NullChecker.CheckStringField(queryResponse, 6),
                                PostalCode = NullChecker.CheckStringField(queryResponse, 7),
                                Country = NullChecker.CheckStringField(queryResponse, 8),
                                Phone = NullChecker.CheckStringField(queryResponse, 9),
                                Fax = NullChecker.CheckStringField(queryResponse, 10)
                            };

                            customersList.Add(customer);
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine($"Exception: {eSql.Message}");
            }

            return customersList;
        }

        public static void UpdateCustomer(Customer updatedCustomer)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    Customer outdatedCustomer = GetCustomer(updatedCustomer.CustomerID);

                    if (outdatedCustomer != null)
                    {
                        SqlCommand updateCustomerQuery = new SqlCommand($"UPDATE Customers SET " +
                            $"CompanyName = '{updatedCustomer.CompanyName}', ContactName = '{updatedCustomer.ContactName}', ContactTitle = '{updatedCustomer.ContactTitle}', " +
                            $"Address = '{updatedCustomer.Address}', City = '{updatedCustomer.City}', Region = '{updatedCustomer.Region}', PostalCode = '{updatedCustomer.PostalCode}', " +
                            $"Country = '{updatedCustomer.Country}', Phone = '{updatedCustomer.Phone}', Fax = '{updatedCustomer.Fax}' WHERE CustomerID = '{updatedCustomer.CustomerID}'", connection);

                        updateCustomerQuery.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void DeleteCustomer(string customerID)
        {
            OrderDatabase.DeleteOrderDetails(customerID);
            OrderDatabase.DeleteOrders(customerID);

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand deleteCustomerQuery = new SqlCommand($"DELETE FROM Customers WHERE CustomerID = '{customerID}'", connection);
                    deleteCustomerQuery.ExecuteNonQuery();
                }
            }
        }

        private static string CheckNullField(SqlDataReader queryResponse, int currentIndex)
        {
            return queryResponse.IsDBNull(currentIndex) ? null : queryResponse.GetString(currentIndex);
        }
    }
}
