using DatabaseProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProject.Repository
{
    public static class OrderDatabase
    {
        const string ConnectionString = @"Data Source=127.0.0.1,1433;Database=Northwind;User Id=sa;Password=Aluno@123;";

        public static ObservableCollection<Order> GetAllOrders(string customerID)
        {
            ObservableCollection<Order> ordersList = new ObservableCollection<Order>();
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand getCustomersQuery = new SqlCommand($"SELECT * FROM Orders WHERE CustomerID = '{customerID}'", connection);
                        SqlDataReader queryResponse = getCustomersQuery.ExecuteReader();

                        while (queryResponse.Read())
                        {
                            DateTime orderRequiredDate = queryResponse.IsDBNull(4) ? DateTime.MinValue : queryResponse.GetDateTime(4);
                            DateTime orderShippedDate = queryResponse.IsDBNull(5) ? DateTime.MinValue : queryResponse.GetDateTime(5);
                            string orderRegion = queryResponse.IsDBNull(11) ? null : queryResponse.GetString(11);
                            string orderPostalCode = queryResponse.IsDBNull(12) ? null : queryResponse.GetString(12);

                            Order order = new Order
                            {
                                OrderID = queryResponse.GetInt32(0),
                                CustomerID = queryResponse.GetString(1),
                                EmployeeID = queryResponse.GetInt32(2),
                                OrderDate = queryResponse.GetDateTime(3),
                                RequiredDate = orderRequiredDate,
                                ShippedDate = orderShippedDate,
                                ShipVia = queryResponse.GetInt32(6),
                                Freight = queryResponse.GetDecimal(7),
                                ShipName = queryResponse.GetString(8),
                                ShipAddress = queryResponse.GetString(9),
                                ShipCity = queryResponse.GetString(10),
                                ShipRegion = orderRegion,
                                ShipPostalCode = orderPostalCode,
                                ShipCountry = queryResponse.GetString(13)
                            };

                            ordersList.Add(order);
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine($"Exception: {eSql.Message}");
            }

            return ordersList;
        }

        public static void GetOrderDetails(string orderID)
        {

        }
    }
}
