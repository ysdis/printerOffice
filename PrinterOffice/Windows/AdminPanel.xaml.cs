using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Логика взаимодействия с панелью администратора.
    /// </summary>
    public partial class AdminPanel : Window {
        private DataTable _items;
        private bool _computeSelection; // Выделяет ли компьютер автоматически в данный момент

        public AdminPanel() {
            InitializeComponent();
            if (Utilities.fillDropDown("SELECT id, title FROM orderStatuses;",
                OrderStatusComboBox,
                "id",
                "title",
                true)) {
                MessageBox.Show("Критическая ошибка. Приложение экстренно завершает работу");
                Application.Current.Shutdown();
            } else {
                Title = Convert.ToBoolean(Application.Current.Properties["isAdmin"]) ? "Панель администратора " : "Панель сотрудника "; // Администратор ли сотрудник
                Title += Convert.ToString(Application.Current.Properties["emplLogin"]);
                if(Convert.ToBoolean(Application.Current.Properties["isAdmin"])) EmployeeMenuItem.Visibility = Visibility.Visible;
                refresh();
            }
        }

        /// <summary>
        /// Показывает заказ при клике на строку в списке ближайших записей
        /// </summary>
        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (_computeSelection) return;
            if (DetailsStack.Children.Count > 0) {
                DetailsStack.Children.RemoveAt(0);
            }
            LabelHint.Visibility = Visibility.Hidden;
            var card = new OrderControlP(((DataRowView)OrdersDataGrid.SelectedItem).Row.Field<int>("id").ToString());
            DetailsStack.Children.Add(card);
        }

        /// <summary>
        /// Актуализирует информацию в окне
        /// </summary>
        public void refresh() {
            _computeSelection = true;
            _items = new DataTable();
            Utilities.fillDataTable(_items, SqlQueries.GET_ALL_ORDERS);
            OrdersDataGrid.DataContext = _items.DefaultView;
            _items = new DataTable();
            if (OrderStatusComboBox.SelectedValue.ToString() == "-1") {
                Utilities.fillDataTable(_items, SqlQueries.GET_ALL_ORDERS);
            } else {
                Utilities.fillDataTable(_items, SqlQueries.GET_ORDERS_BY_STATUS, new MySqlParameter("@STATUSTEXT", ((DataRowView)OrderStatusComboBox.SelectedItem).Row.ItemArray[1].ToString()));
            }
            OrdersDataGrid.ItemsSource = _items.DefaultView;
            _computeSelection = false;
        }

        private void ClosestOrders_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            // Скрытие столбца с идентификатором записи в таблице с записями
            if(e.PropertyName == "id") {
                e.Cancel = true;
            }
        }

        private void exit(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void changeUser(object sender, RoutedEventArgs e) {
            new MainWindow().Show();
            Close();
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(_computeSelection) return;
            refresh();
        }

        private void createOrder(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void openCustomersControl(object sender, RoutedEventArgs e) {
            var window = new Browser("Клиенты", SqlQueries.GET_ALL_CUSTOMERS, Browser.TYPE_CLIENTS) {Owner = this};
            window.Show();
        }

        private void openDevicesBrowser(object sender, RoutedEventArgs e) {
            var window = new Browser("Устройства", SqlQueries.GET_ALL_DEVICES, Browser.TYPE_DEVICES) {Owner = this};
            window.Show();
        }

        private void openDeviceTypeBrowser(object sender, RoutedEventArgs e) {
            var window = new Browser("Типы устройств", SqlQueries.GET_ALL_DEVICE_TYPES, Browser.TYPE_DEVICE_TYPES) {Owner = this};
            window.Show();
        }

        private void openDevicesManufacsBrowser(object sender, RoutedEventArgs e) {
            var window = new Browser("Производители", SqlQueries.GET_ALL_DEVICE_MANUFACS, Browser.TYPE_DEVICE_MANUFACS) {Owner = this};
            window.Show();
        }

        private void openAdditionToStorage(object sender, RoutedEventArgs e) {
            var dialog = new StorageItemControl();
            dialog.ShowDialog();
            if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                MessageBox.Show("Устройство успешно добавлено на склад!");
            }
        }

        private void openStorageBrowser(object sender, RoutedEventArgs e) {
            var window = new Browser("Склад", SqlQueries.GET_STORAGE, Browser.TYPE_STORAGE) {Owner = this};
            window.Show();
        }

        private void openEmployeeBrowser(object sender, RoutedEventArgs e) {
            var window = new Browser("Сотрудники", SqlQueries.GET_EMPLOYEES, Browser.TYPE_EMPLOYEES) {Owner = this};
            window.Show();
        }

        private void openFiringWindow(object sender, RoutedEventArgs e) {
            var dialog = new EmployeeJobStatusControl();
            dialog.ShowDialog();
            if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                MessageBox.Show("Статус сотрудника успешно обновлён!");
            }
        }

        private void openHiringWindow(object sender, RoutedEventArgs e) {
            var dialog = new EmployeeJobStatusControl(true);
            dialog.ShowDialog();
            if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                MessageBox.Show("Статус сотрудника успешно обновлён!");
            }
        }
    }
}
