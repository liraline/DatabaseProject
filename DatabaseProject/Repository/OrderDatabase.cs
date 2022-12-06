using System;
using System.Diagnostics;
using System.Data.SqlClient;
using DatabaseProject.Model;
using DatabaseProject.Utils;
using System.Collections.ObjectModel;

namespace DatabaseProject.Repository
{
    public static class OrderDatabase
    {
        const string ConnectionString = @"Data Source=127.0.0.1,1433;Database=Northwind;User Id=sa;Password=Aluno@123;";

        public static void CreateOrder(Order order)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand insertOrderQuery = new SqlCommand("INSERT INTO Orders VALUES " +
                        "(@customerID, @employeeID, @orderDate, @requiredDate, @shippedDate, @shipVia, " +
                        "@freight, @shipName, @shipAddress, @shipCity, @shipRegion, @shipPostalCode, @shipCountry)", connection);

                    insertOrderQuery.Parameters.AddWithValue("@customerID", order.CustomerID);
                    insertOrderQuery.Parameters.AddWithValue("@employeeID", order.EmployeeID);
                    insertOrderQuery.Parameters.AddWithValue("@orderDate", order.OrderDate);
                    insertOrderQuery.Parameters.AddWithValue("@requiredDate", order.RequiredDate);
                    insertOrderQuery.Parameters.AddWithValue("@shippedDate", order.ShippedDate);
                    insertOrderQuery.Parameters.AddWithValue("@shipVia", order.ShipVia);
                    insertOrderQuery.Parameters.AddWithValue("@freight", order.Freight);
                    insertOrderQuery.Parameters.AddWithValue("@shipName", order.ShipName);
                    insertOrderQuery.Parameters.AddWithValue("@shipAddress", order.ShipAddress);
                    insertOrderQuery.Parameters.AddWithValue("@shipCity", order.ShipCity);
                    insertOrderQuery.Parameters.AddWithValue("@shipRegion", order.ShipRegion);
                    insertOrderQuery.Parameters.AddWithValue("@shipPostalCode", order.ShipPostalCode);
                    insertOrderQuery.Parameters.AddWithValue("@shipCountry", order.ShipCountry);

                    insertOrderQuery.ExecuteNonQuery();
                }
            }
        }

        public static void CreateOrderDetails(DateTime orderDate, OrderDetails orderDetails)
        {
            int orderID = GetOrderIDFromOrderDate(orderDate);

            if (orderID != -1)
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand insertOrderDetailsQuery = new SqlCommand("INSERT INTO [Order Details] VALUES " +
                            "(@orderID, @productID, @unitPrice, @quantity, @discount)", connection);

                        insertOrderDetailsQuery.Parameters.AddWithValue("@orderID", orderID);
                        insertOrderDetailsQuery.Parameters.AddWithValue("@productID", orderDetails.ProductID);
                        insertOrderDetailsQuery.Parameters.AddWithValue("@unitPrice", orderDetails.UnitPrice);
                        insertOrderDetailsQuery.Parameters.AddWithValue("@quantity", orderDetails.Quantity);
                        insertOrderDetailsQuery.Parameters.AddWithValue("@discount", orderDetails.Discount);

                        insertOrderDetailsQuery.ExecuteNonQuery();
                    }
                }

            }
        }

        private static int GetOrderIDFromOrderDate(DateTime orderDate)
        {
            int orderID = -1;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand getOrderIDQuery = new SqlCommand($"SELECT OrderID FROM Orders WHERE OrderDate = '{orderDate:yyyy-MM-dd HH:mm:ss:fff}'", connection);
                    SqlDataReader queryResponse = getOrderIDQuery.ExecuteReader();

                    if (queryResponse.HasRows)
                    {
                        queryResponse.Read();
                        orderID = NullChecker.CheckIntField(queryResponse, 0);
                    }
                }
            }
            return orderID;
        }

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
                            Order order = new Order
                            {
                                OrderID = NullChecker.CheckIntField(queryResponse, 0),
                                CustomerID = NullChecker.CheckStringField(queryResponse, 1),
                                EmployeeID = NullChecker.CheckIntField(queryResponse, 2),
                                OrderDate = NullChecker.CheckDateTimeField(queryResponse, 3),
                                RequiredDate = NullChecker.CheckDateTimeField(queryResponse, 4),
                                ShippedDate = NullChecker.CheckDateTimeField(queryResponse, 5),
                                ShipVia = NullChecker.CheckIntField(queryResponse, 6),
                                Freight = NullChecker.CheckDecimalField(queryResponse, 7),
                                ShipName = NullChecker.CheckStringField(queryResponse, 8),
                                ShipAddress = NullChecker.CheckStringField(queryResponse, 9),
                                ShipCity = NullChecker.CheckStringField(queryResponse, 10),
                                ShipRegion = NullChecker.CheckStringField(queryResponse, 11),
                                ShipPostalCode = NullChecker.CheckStringField(queryResponse, 12),
                                ShipCountry = NullChecker.CheckStringField(queryResponse, 13)
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

        public static void DeleteOrders(string customerID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand deleteOrdersQuery = new SqlCommand($"DELETE FROM Orders WHERE CustomerID = '{customerID}'", connection);
                    deleteOrdersQuery.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteOrderDetails(string customerID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand deleteOrderDetailsQuery = new SqlCommand($"DELETE FROM [Order Details] WHERE OrderID IN " +
                        $"(SELECT OrderID FROM Orders WHERE CustomerID = '{customerID}')", connection);
                    deleteOrderDetailsQuery.ExecuteNonQuery();
                }
            }
        }

        public static ObservableCollection<OrderDetails> GetAllOrderDetails(int orderID)
        {
            ObservableCollection<OrderDetails> orderDetailsList = new ObservableCollection<OrderDetails>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand getOrderDetailsQuery = new SqlCommand($"SELECT * FROM [Order Details] WHERE OrderID = '{orderID}'", connection);
                    SqlDataReader queryResponse = getOrderDetailsQuery.ExecuteReader();

                    while (queryResponse.Read())
                    {
                        OrderDetails orderDetails = new OrderDetails
                        {
                            OrderID = NullChecker.CheckIntField(queryResponse, 0),
                            ProductID = NullChecker.CheckIntField(queryResponse, 1),
                            UnitPrice = NullChecker.CheckDecimalField(queryResponse, 2),
                            Quantity = NullChecker.CheckSmallIntField(queryResponse, 3),
                            Discount = queryResponse.GetFloat(4)
                        };

                        orderDetailsList.Add(orderDetails);
                    }
                }
            }
            return orderDetailsList;
        }
    }
}
