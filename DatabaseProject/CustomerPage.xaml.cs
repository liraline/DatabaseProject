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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DatabaseProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerPage : Page
    {
        public CustomerPage()
        {
            this.InitializeComponent();
            PopulateList();
        }

        private void PopulateList()
        {
            CustomerList.ItemsSource = CustomerDatabase.GetAllCustomers();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateOrUpdateCustomerPage));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerList.SelectedItem != null)
            {
                Customer selectedCustomer = (Customer)CustomerList.SelectedItem;
                CustomerDatabase.DeleteCustomer(selectedCustomer.CustomerID);
                PopulateList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerList.SelectedItem != null)
            {
                Customer selectedCustomer = (Customer)CustomerList.SelectedItem;
                this.Frame.Navigate(typeof(CreateOrUpdateCustomerPage), selectedCustomer);
            }
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerList.SelectedItem != null)
            {
                Customer selectedCustomer = (Customer)CustomerList.SelectedItem;
                this.Frame.Navigate(typeof(OrdersPage), selectedCustomer);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
