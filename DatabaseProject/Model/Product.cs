using System.ComponentModel;

namespace DatabaseProject.Model
{
    public class Product : INotifyPropertyChanged
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitPriceString => UnitPrice.ToString("######.00");
        public int UnitsInStock { get; set; }
        public string UnitsInStockString => UnitsInStock.ToString("#####0");

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
