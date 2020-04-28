using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Interaction logic for OrderControlP.xaml
    /// </summary>
    public partial class OrderControlP : UserControl {
        private AdminPanel _parentWindow;
        private DataTable _servicesOrderedTable;

        public string id { get; }

        private Order _order;

        private bool _isEditMode;
        private bool _firstSight = true;
        
        
        public OrderControlP() {
            InitializeComponent();
            _order = new Order();
        }

        /// <param name="orderId">Идентификатор редактируемой записи</param>
        public OrderControlP(string orderId) {
            InitializeComponent();
            id = orderId;
            if (orderId == "") {
                new NotificationWindow("Заказ не найден", Brushes.Tomato).Show();
                return;
            }

            if (Utilities.fillDropDown("SELECT id, title FROM services;", ServiceTypeComboBox, "id", "title")) {
                new NotificationWindow("Ошибка загрузки данных", Brushes.Tomato).Show();
            }
        }

        private void loadedEvent(object sender, RoutedEventArgs e) {
            _parentWindow = Window.GetWindow(this) as AdminPanel;
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
            Utilities.fillDataTable(_servicesOrderedTable, "SELECT * FROM orderitems WHERE orderId = @ORDERID", new MySqlParameter("@ORDERID", orderId));
            OrderedDevices.ItemsSource = _servicesOrderedTable.DefaultView;

            CommentBox.Text = _order.comment;
            PriceBox.Text = _order.totalPrice.ToString();

            switch (_order.statusId) { // В зависимости от статуса изменяем видимость и доступность элементов интерфейса
                case 1: // Отменён
                case 2: // Завершён
                    MakeDoneBtn.Visibility = Visibility.Hidden;
                    MakeCanceledBtn.Visibility = Visibility.Hidden;
                    AddService.Visibility = Visibility.Hidden;
                    DeleteSelectedBtn.Visibility = Visibility.Hidden;
                    OrderedDevices.IsEnabled = false;
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
            if(_isEditMode) { // Обновление записи
                if(_order.update()) { 
                    new NotificationWindow("Изменения сохранены!", Brushes.DarkSeaGreen).Show();
                } else {
                    MessageBox.Show("Проверьте правильность введённых данных и подключение к интернету!");
                }
            } else { // Создание записи
                if (_order.upload()) {
                    new NotificationWindow("Заказ создан!", Brushes.DarkSeaGreen).Show();
                }
            }
        }

        /// Listener Click. Открывает окно поиска клиента
        private void openClientSearch(object sender, RoutedEventArgs e) {
            
        }

        /// Listener Click. Открывает окно поиска услуги
        private void openServiceAddition(object sender, RoutedEventArgs e) {
            //var window = new OrderItemControl() {Owner = _parentWindow};
            //window.Show();
        }

        /// Добавляет услугу в список услуг записи
        public void addServiceToDataTable(DataRowView rowSelected) {
            if(_servicesOrderedTable == null) {
                _servicesOrderedTable = rowSelected.DataView.Table.Clone();
            }

            _servicesOrderedTable.Rows.Add(rowSelected.Row.ItemArray);
            OrderedDevices.ItemsSource = _servicesOrderedTable.DefaultView;
            calculate();
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
            //_totalPrice = 0;
            //_totalDurationInMinutes = 0;
            //_idsList = new List<int>();
            //foreach(DataRow row in _servicesOrderedTable.Rows) {
            //    _totalPrice += row.Field<int>("Стоимость");
            //    _totalDurationInMinutes += row.Field<int>("Длительность");

            //    _idsList.Add(row.Field<int>("id"));
            //}

            //if(_isEditMode) {
            //    if (_loading) {
            //        _totalPrice = _loadedOrder.totalPrice;
            //        _totalDurationInMinutes = _loadedOrder.totalDuration;
            //    }
            //    _loadedOrder.totalPrice = _totalPrice;
            //    if(!_firstSight) {
            //        _loadedOrder.totalDuration = _totalDurationInMinutes;
            //    }
            //    _loadedOrder.orderItems = _idsList;
            //}

            //priceLabel.Text = $"{_totalPrice:#,0}";
        }

        /// Изменяет статус записи на "Отменен"
        private void cancelOrder(object sender, RoutedEventArgs e) {
            //if(Database.execute(_loadedOrder.id, 1).isSuccess()) {
            //    new NotificationWindow("Заказ отменён!", Brushes.DarkSeaGreen).Show();
            //}
        }

        /// Изменяет статус записи на "Выполнен"
        private void makeDone(object sender, RoutedEventArgs e) {
            //if(Database.updateOrderStatus(_loadedOrder.id, 2).isSuccess()) {
            //    new NotificationWindow("Заказ выполнен!", Brushes.DarkSeaGreen).Show();
            //}
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
