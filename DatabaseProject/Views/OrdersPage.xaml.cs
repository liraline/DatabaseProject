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
    public sealed partial class OrdersPage : Page
    {
        private Customer selectedCustomer;

        public OrdersPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                selectedCustomer = (Customer)e.Parameter;
                OrderList.ItemsSource = OrderDatabase.GetAllOrders(selectedCustomer.CustomerID);
            }
        }
        private void CreateNewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateOrderPage), selectedCustomer);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CustomerPage));
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderList.SelectedItem != null)
            {
                this.Frame.Navigate(typeof(OrderDetailsPage), (Order)OrderList.SelectedItem);
            }
        }
    }
}
