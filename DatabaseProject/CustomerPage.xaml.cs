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
            //CustomerList.ItemsSource = CustomerDatabase.GetAllCustomers();
            Customer Aline = new Customer
            {
                CustomerID = "ALIN",
                CompanyName = "Company",
                ContactName = "Aline",
                ContactTitle = "Owner",
                Address = "One Street, 1",
                City = "City",
                Region = "A Region",
                PostalCode = "0000 000",
                Country = "Spain",
                Phone = "030 000-0000",
                Fax = "030 000-0000"
            };

            OrderDatabase.GetAllOrders("LEHMS");

        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(ListViewItem), new PropertyMetadata(null, new PropertyChangedCallback(OnIsSelectedPropertyChanged)));

        private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (ListViewItem)d;
            if (self != null)
            {
                self.IsSelected = !self.IsSelected;
            }
        }
    }
}
