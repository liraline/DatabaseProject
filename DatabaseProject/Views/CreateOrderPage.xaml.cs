using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DatabaseProject.Repository;
using DatabaseProject.Model;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DatabaseProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateOrderPage : Page
    {
        ObservableCollection<Product> productsList = new ObservableCollection<Product>();
        ObservableCollection<Cart> cartList = new ObservableCollection<Cart>();
        private Customer selectedCustomer;

        public CreateOrderPage()
        {
            this.InitializeComponent();
            ProductList.ItemsSource = ProductDatabase.GetAllProducts();
            PopulateCart();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                selectedCustomer = (Customer)e.Parameter;
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductList.SelectedItem != null)
            {
                Product selectedProduct = (Product)ProductList.SelectedItem;
                productsList.Add(selectedProduct);
                cartList.Add(new Cart
                {
                    ProductName = selectedProduct.ProductName,
                    Quantity = 1
                });
                PopulateCart();
            }
        }

        private void PopulateCart()
        {
            CartList.ItemsSource = cartList;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CustomerPage));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CartList.SelectedItem != null)
            {
                Cart cartItem = (Cart)CartList.SelectedItem;
                cartList.Remove(cartItem);
                productsList.Remove(productsList.First(p => p.ProductName.Equals(cartItem.ProductName)));
                PopulateCart();
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (cartList.Count > 0)
            {
                Order newOrder = new Order {
                    CustomerID = selectedCustomer.CustomerID,
                    EmployeeID = 1,
                    OrderDate = DateTime.Now,
                    RequiredDate = DateTime.Now,
                    ShippedDate = DateTime.Today,
                    ShipVia = 1,
                    Freight = 12,
                    ShipName = "Fedex",
                    ShipAddress = selectedCustomer.Address,
                    ShipCity = selectedCustomer.City,
                    ShipRegion = selectedCustomer.Region,
                    ShipPostalCode = selectedCustomer.PostalCode,
                    ShipCountry = selectedCustomer.PostalCode
                };

                OrderDatabase.CreateOrder(newOrder);

                foreach (Product product in productsList)
                {
                    OrderDetails newOrderDetails = new OrderDetails
                    {
                        ProductID = product.ProductID,
                        UnitPrice = product.UnitPrice,
                        Quantity = productsList.Where(p => p.ProductID.Equals(product.ProductID)).Count(),
                        Discount = 0
                    };

                    OrderDatabase.CreateOrderDetails(newOrder.OrderDate, newOrderDetails);
                }
            }
        }
    }
}
