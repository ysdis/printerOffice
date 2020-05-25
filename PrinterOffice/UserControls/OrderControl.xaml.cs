using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Interaction logic for OrderControlP.xaml
    /// </summary>
    public partial class OrderControl : UserControl {
        private readonly AdminPanel _adminPanel;
        private DataTable _servicesOrderedTable;
        public string id { get; }

        private Order _order;

        private bool _isEditMode;
        private bool _firstSight = true;

        private readonly Window _parentWindow;
        
        
        public OrderControl(Window parentWindow) {
            InitializeComponent();
            _isEditMode = false;

            if (Utilities.fillDropDown("SELECT id, title FROM services;", ServiceTypeComboBox, "id", "title")) {
                new NotificationWindow("Ошибка загрузки данных", Brushes.Tomato).Show();
            }

            _parentWindow = parentWindow;
            _order = new Order();

            AcceptBtn.Content = "Создать";
            MakeCanceledBtn.Visibility = Visibility.Hidden;
            MakeDoneBtn.Visibility = Visibility.Hidden;

            RespEmpl.Text = Application.Current.Properties["emplLogin"].ToString();

            if ((bool) Application.Current.Properties["isAdmin"]) {
                SearchEmployeeBtn.Visibility = Visibility.Visible;
            }

            _loading = false;
        }

        /// <param name="orderId">Идентификатор редактируемой записи</param>
        /// <param name="adminPanel">Ссылка на панель администратора</param>
        public OrderControl(string orderId, AdminPanel adminPanel) {
            InitializeComponent();
            _adminPanel = adminPanel;
            
            id = orderId;
            if (orderId == "") {
                new NotificationWindow("Заказ не найден", Brushes.Tomato).Show();
                return;
            }

            if (Utilities.fillDropDown("SELECT id, title FROM services;", ServiceTypeComboBox, "id", "title")) {
                new NotificationWindow("Ошибка загрузки данных", Brushes.Tomato).Show();
            }

            ServiceTypeComboBox.IsEnabled = false;

            loadOrder(id);
        }

        public void loadOrder(string orderId) {
            _isEditMode = true;
            _loading = true;

            CustomerTel.IsEnabled = false;
            SearchCustomer.Visibility = Visibility.Hidden;
            RespEmpl.IsEnabled = false;
            SearchEmployeeBtn.Visibility = Visibility.Hidden;

            _order = new Order(orderId);
            if (!_order.download()) {
                new NotificationWindow("Ошибка загрузки данных", Brushes.Tomato).Show();
                return;
            }

            CustomerTel.Text = _order.custId;
            RespEmpl.Text = _order.emplLogin;
            ServiceTypeComboBox.SelectedValue = _order.serviceId.ToString();

            _servicesOrderedTable = new DataTable();
            Utilities.fillDataTable(_servicesOrderedTable, "SELECT storageSn AS 'sn', price AS 'Стоимость' FROM orderitems WHERE orderId = @ORDERID", new MySqlParameter("@ORDERID", orderId));
            OrderedDevices.ItemsSource = _servicesOrderedTable.DefaultView;

            CommentBox.Text = _order.comment;
            PriceBox.Text = _order.totalPrice.ToString();

            switch (_order.statusId) { // В зависимости от статуса изменяем видимость и доступность элементов интерфейса
                case 1: // Отменён
                case 2: // Завершён
                    disableAllControls();
                    break;
                case 3: // Выполняется
                    AcceptBtn.Content = "Сохранить";
                    MakeDoneBtn.Visibility = Visibility.Visible;
                    MakeCanceledBtn.Visibility = Visibility.Visible;
                    break;
            }
            

            calculate();
            _loading = false;
        }

        /// Listener Click. Запускает процесс загрузки новой записи в БД/изменения существующей
        private void acceptChanges(object sender, RoutedEventArgs e) {
            _order.orderItems.Clear();
            _order.comment = CommentBox.Text;
            if (_servicesOrderedTable == null) {
                new NotificationWindow("Устройства не были выбраны!", Brushes.Tomato).Show();
                return;
            }

            foreach(DataRow row in _servicesOrderedTable.Rows) {
                _order.orderItems.Add(new OrderItem(row.Field<string>("sn"), row.Field<int>("Стоимость")));
            }

            if(_isEditMode) { // Обновление записи
                if(_order.update()) { 
                    new NotificationWindow("Изменения сохранены!", Brushes.DarkSeaGreen).Show();
                    _adminPanel.refresh();
                } else {
                    MessageBox.Show("Проверьте правильность введённых данных и подключение к интернету!");
                }
            } else { // Создание записи
                if(_order.upload()) {
                    if (((DataRowView) ServiceTypeComboBox.SelectedItem).Row.ItemArray[1].ToString() == "Возврат") {
                        makeDone(sender, e);
                        new NotificationWindow("Возврат произведен успешно!", Brushes.DarkSeaGreen).Show();
                    } else {
                        new NotificationWindow("Заказ создан!", Brushes.DarkSeaGreen).Show();
                    }
                    _parentWindow.DialogResult = true;
                    _parentWindow.Close();
                } else {
                    new NotificationWindow("Заказ не был создан!", Brushes.Tomato).Show();
                }
            }
        }

        /// Listener Click. Открывает окно поиска клиента
        private void openClientSearch(object sender, RoutedEventArgs e) {
            var dialog = new QuickSearch(SqlQueries.GET_ALL_CUSTOMERS,
                "Номер телефона",
                new List<string> {"Номер телефона", "ФИО Клиента"},
                new List<string> {"Номер телефона"});
            dialog.ShowDialog();
            if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                CustomerTel.Text = dialog.SelectedItem;
            }
        }

        private void openEmployeeSearch(object sender, RoutedEventArgs e) {
            var dialog = new QuickSearch(SqlQueries.GET_ALL_WORKING_EMPLOYEES,
                "login",
                new List<string> {"ФИО Сотрудника"},
                new List<string> {"login"},
                QuickSearch.TYPE_EMPLOYEES);
            dialog.ShowDialog();
            if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                RespEmpl.Text = dialog.SelectedItem;
            }
        }

        /// Listener Click. Открывает окно поиска устройства
        private void openDeviceAddition(object sender, RoutedEventArgs e) {
            var status = "";
            var destination = "";
            var selectedOrderTypeText = ((DataRowView) ServiceTypeComboBox.SelectedItem).Row.ItemArray[1].ToString();
            var sqlSnExclude = "";

            switch (selectedOrderTypeText) {
                case "Заправка":
                case "Ремонт":
                    status = "Отсутствует";
                    destination = "Хранение";
                    break;
                case "Продажа":
                    status = "В наличии";
                    destination = "Реализация";
                    break;
                case "Возврат":
                    status = "Отсутствует";
                    destination = "Реализация";
                    break;
            }

            var sqlQuery = $"SELECT * FROM devicesSelection WHERE `Статус` LIKE '%{status}%' and `Предназначение` LIKE '%{destination}%'";

            if (_servicesOrderedTable != null) {
                if (_servicesOrderedTable.Rows.Count > 0) {
                    sqlSnExclude += " and sn NOT IN(";
                    for(var i = 0; i < _servicesOrderedTable.Rows.Count; ++i) {
                        sqlSnExclude += "'" + _servicesOrderedTable.Rows[i].Field<string>("sn") + "'";
                        if (i < _servicesOrderedTable.Rows.Count - 1) {
                            sqlSnExclude += ",";
                        } else {
                            sqlSnExclude += ")";
                        }
                    }
                } 
            }

            sqlSnExclude += ";";

            var dialog = new QuickSearch(sqlQuery + sqlSnExclude,
                "sn",
                new List<string> {"Модель"},
                new List<string>(),
                QuickSearch.TYPE_DEVICES);
            dialog.ShowDialog();

            if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                if(_servicesOrderedTable == null) {
                    _servicesOrderedTable = dialog.SelectedRowView.DataView.Table.Clone();
                }

                _servicesOrderedTable.Rows.Add(dialog.SelectedRowView.Row.ItemArray);
                OrderedDevices.ItemsSource = _servicesOrderedTable.DefaultView;
                calculate();
            }
        }

        /// Listener AutoGeneratingColumn. Скрывает первый столбец с идентификатором услуги
        private void OrderedServices_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            if(e.PropertyName == "orderId" || e.PropertyName == "id") {
                e.Cancel = true;
            }
        }

        /// Удаляет выделенную услугу в списке услуг записи
        private void deleteSelectedService(object sender, RoutedEventArgs e) {
            if (OrderedDevices.SelectedIndex == -1) return;
            _servicesOrderedTable.Rows.RemoveAt(OrderedDevices.SelectedIndex);
            OrderedDevices.ItemsSource = _servicesOrderedTable.DefaultView;

            calculate();
        }

        private bool _loading = true;
        /// Производит расчет стоимости и продолжительности процедур
        private void calculate() {
            var totalPrice = 0;

            if (_loading) {
                totalPrice = _order.totalPrice;
            } else {
                foreach(DataRow row in _servicesOrderedTable.Rows) {
                    totalPrice += row.Field<int>("Стоимость");
                }
                _order.totalPrice = totalPrice;
            }

            PriceBox.Text = $"{totalPrice:#,0}";
        }

        /// Изменяет статус записи на "Отменен"
        private void cancelOrder(object sender, RoutedEventArgs e) {
            var response = _order.updateStatus(Order.STATUS_CANCELLED);
            if (response.isSuccess()) {
                new NotificationWindow("Заказ отменён!", Brushes.DarkSeaGreen).Show();
                disableAllControls();
                _adminPanel.refresh();
            } else {
                new NotificationWindow(response.getErrorMsg(), Brushes.Tomato).Show();
            }
        }

        /// Изменяет статус записи на "Выполнен"
        private void makeDone(object sender, RoutedEventArgs e) {
            var response = _order.updateStatus(Order.STATUS_DONE);
            if (response.isSuccess()) {
                if (((DataRowView) ServiceTypeComboBox.SelectedItem).Row.ItemArray[1].ToString() == "Возврат") return;
                
                new NotificationWindow("Заказ завершён!", Brushes.DarkSeaGreen).Show();
                disableAllControls();
                _adminPanel.refresh();
            } else {
                new NotificationWindow(response.getErrorMsg(), Brushes.Tomato).Show();
            }
        }
        private void disableAllControls() {
            MakeDoneBtn.Visibility = Visibility.Hidden;
            MakeCanceledBtn.Visibility = Visibility.Hidden;
            AddDeviceBtn.Visibility = Visibility.Hidden;
            DeleteSelectedBtn.Visibility = Visibility.Hidden;
            AcceptBtn.Visibility = Visibility.Hidden;
            CommentBox.IsEnabled = false;
            PriceBox.IsEnabled = false;
            OrderedDevices.IsEnabled = false;
        }

        private void priceLabel_KeyUp(object sender, KeyEventArgs e) {
            try {
                _order.totalPrice = Convert.ToInt16(((TextBox) sender).Text);
            }
            catch (Exception) {
                _order.totalPrice = 0;
            }
        }

        private void priceLabel_GotFocus(object sender, RoutedEventArgs e) {
            ((TextBox) sender).Text = _order.totalPrice.ToString();
        }

        private void priceLabel_LostFocus(object sender, RoutedEventArgs e) {
            ((TextBox) sender).Text = $"{_order.totalPrice:#,0}";
        }
        
        /// <summary>
        /// Изменение вида услуги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(_loading) return;
            _order.serviceId = Convert.ToInt16(ServiceTypeComboBox.SelectedValue);
        }

        private void CustomerTel_TextChanged(object sender, TextChangedEventArgs e) {
            _order.custId = ((TextBox) sender).Text;
        }

        private void RespEmpl_TextChanged(object sender, TextChangedEventArgs e) {
            _order.emplLogin = ((TextBox) sender).Text;
        }
    }
}
