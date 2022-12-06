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
                            " WHERE Products.Discontinued = 0");
                        SqlDataReader queryResponse = getProductsQuery.ExecuteReader();

                        while (queryResponse.Read())
                        {
                            Product product = new Product
                            {
                                ProductID = queryResponse.GetInt32(0),
                                ProductName = queryResponse.GetString(1),
                                CategoryName = queryResponse.GetString(2),
                                QuantityPerUnit = queryResponse.GetString(3),
                                UnitPrice = queryResponse.GetDecimal(4),
                                UnitsInStock = queryResponse.GetInt16(5)
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
