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
using DatabaseProject.Model;
using DatabaseProject.Repository;
using DatabaseProject.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DatabaseProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateOrUpdateCustomerPage : Page
    {
        public CreateOrUpdateCustomerPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                Customer selectedCustomer = (Customer)e.Parameter;
                ID.Text = NullChecker.CheckStringProperty(selectedCustomer.CustomerID);
                CompanyName.Text = NullChecker.CheckStringProperty(selectedCustomer.CompanyName);
                ContactName.Text = NullChecker.CheckStringProperty(selectedCustomer.ContactName);
                ContactTitle.Text = NullChecker.CheckStringProperty(selectedCustomer.ContactTitle);
                Address.Text = NullChecker.CheckStringProperty(selectedCustomer.Address);
                City.Text = NullChecker.CheckStringProperty(selectedCustomer.City);
                Region.Text = NullChecker.CheckStringProperty(selectedCustomer.Region);
                PostalCode.Text = NullChecker.CheckStringProperty(selectedCustomer.PostalCode);
                Country.Text = NullChecker.CheckStringProperty(selectedCustomer.Country);
                Phone.Text = NullChecker.CheckStringProperty(selectedCustomer.Phone);
                Fax.Text = NullChecker.CheckStringProperty(selectedCustomer.Fax);

                SaveButton.Click -= SaveButton_Click;
                SaveButton.Click += UpdateButton_Click;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CustomerPage));
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerDatabase.UpdateCustomer(CreateCustomerFromTextBox());
            this.Frame.Navigate(typeof(CustomerPage));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerDatabase.CreateCustomer(CreateCustomerFromTextBox());
            this.Frame.Navigate(typeof(CustomerPage));
        }

        private Customer CreateCustomerFromTextBox()
        {
            Customer customer = new Customer
            {
                CustomerID = ID.Text,
                CompanyName = CompanyName.Text,
                ContactName = ContactName.Text,
                ContactTitle = ContactTitle.Text,
                Address = Address.Text,
                City = City.Text,
                Region = Region.Text,
                PostalCode = PostalCode.Text,
                Country = Country.Text,
                Phone = Phone.Text,
                Fax = Fax.Text
            };

            return customer;
        }
    }
}
