using System;
using System.Diagnostics;
using System.Data.SqlClient;
using DatabaseProject.Model;
using DatabaseProject.Utils;
using System.Collections.ObjectModel;

namespace DatabaseProject.Repository
{
    public static class ProductDatabase
    {
        const string ConnectionString = @"Data Source=127.0.0.1,1433;Database=Northwind;User Id=sa;Password=Aluno@123;";

        public static ObservableCollection<Product> GetAllProducts()
        {
            ObservableCollection<Product> productsList = new ObservableCollection<Product>();
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand getProductsQuery = new SqlCommand("SELECT Products.ProductID, Products.ProductName, Categories.CategoryName, " +
                            "Products.QuantityPerUnit, Products.UnitPrice, Products.UnitsInStock " +
                            " FROM Products INNER JOIN Categories ON Products.CategoryID = Categories.CategoryID " +
                            " WHERE Products.Discontinued = 0", connection);
                        SqlDataReader queryResponse = getProductsQuery.ExecuteReader();

                        while (queryResponse.Read())
                        {
                            Product product = new Product
                            {
                                ProductID = NullChecker.CheckIntField(queryResponse, 0),
                                ProductName = NullChecker.CheckStringField(queryResponse, 1),
                                CategoryName = NullChecker.CheckStringField(queryResponse, 2),
                                QuantityPerUnit = NullChecker.CheckStringField(queryResponse, 3),
                                UnitPrice = NullChecker.CheckDecimalField(queryResponse, 4),
                                UnitsInStock = NullChecker.CheckSmallIntField(queryResponse, 5)
                            };

                            productsList.Add(product);
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine($"Exception: {eSql.Message}");
            }

            return productsList;
        }
    }
}
