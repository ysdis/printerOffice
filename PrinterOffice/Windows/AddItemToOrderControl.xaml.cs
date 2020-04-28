using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrinterOffice {
    /// <summary>
    /// Interaction logic for OrderItemControl.xaml
    /// </summary>
    public partial class AddItemToOrderControl : Window {
        private bool _isComputerSelection = false;
        private DataTable _items;

        public AddItemToOrderControl() { // Создание новой услуги в заказе
            InitializeComponent();
            init();
        }

        public AddItemToOrderControl(int orderItemId) { // Изменение существующей оказываемой услуги в заказе
            InitializeComponent();
            init();
        }

        private void init() {
            //if (Utilities.fillDropDown(
            //    "SELECT id, title FROM servicesTypes;",
            //    ServiceComboBox, "id", "title")) { // Заполняем выпадающий список
            //    new NotificationWindow("Невозможно загрузить данные!", Brushes.Tomato).Show();
            //} else {
            //    ServiceComboBox.SelectedIndex = 0;
            //}
            //if(Utilities.fillDataTable(DevicesDataGrid, "SELECT id, model, deviceManufac"))
        }

        private void loadDevices(int mode) {
            switch (mode) {
                case 1: { // Ремонт
                    
                    break;
                }
                case 2: { // Заправка
                    break;
                }
                case 3: { // Поставка
                    break;
                }
            }
        }

        private void onKeyUpSearchBox(object sender, KeyEventArgs e) {
            var dv = _items.DefaultView;
            dv.RowFilter = $"`Модель` like '%{SearchBox.Text}%' or `Производитель` like '%{SearchBox.Text}%'";
            DevicesDataGrid.ItemsSource = dv;
        }

        private void openModelCreation(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            throw new NotImplementedException();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            

        }

        private void markDoneOrderItem(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void markCanceledOrderItem(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void exitWindow(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        // Реакция на изменение выбора фильтра
        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            throw new NotImplementedException();
        }
    }
}
