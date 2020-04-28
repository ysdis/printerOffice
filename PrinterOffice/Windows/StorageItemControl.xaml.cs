using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrinterOffice {
    /// <summary>
    /// Interaction logic for AddDeviceToStorage.xaml
    /// </summary>
    public partial class StorageItemControl : Window {
        private DataTable _items = new DataTable();
        private bool _computeSelection; // Выделяет ли компьютер автоматически в данный момент
        private int totalPrice { get; set; }
        public bool isEditMode { get; }

        private StorageItem item;

        public StorageItemControl(string sn) {
            InitializeComponent();
            isEditMode = true;
            init();

            MarkDoneBtn.Content = "Обновить";
            DeleteBtn.Visibility = Visibility.Visible;
            Title = "Изменение устройства на складе";
            item = new StorageItem(sn);

            SnBox.Text = item.sn;
            SnBox.IsEnabled = false;

            ModelIdBox.Text = item.deviceId;
            DestinationComboBox.SelectedValue = item.destinationId;

            totalPrice = Convert.ToInt16(item.price);
            PriceBox.Text = totalPrice.ToString();
        }

        public StorageItemControl() {
            InitializeComponent();
            isEditMode = false;
            init();
        }

        public void init() {
            _computeSelection = true;

            if (Utilities.fillDropDown("SELECT * FROM destinations;", DestinationComboBox, "id", "title") ||
                Utilities.fillDropDown("SELECT * FROM deviceManufacs;", DeviceManufacComboBox, "id", "title") || 
                Utilities.fillDataTable(_items, SqlQueries.GET_ALL_DEVICES)
            ) {
                MessageBox.Show("Ошибка загрузки данных!");
            }
            _computeSelection = false;
            DevicesDataGrid.DataContext = _items.DefaultView;
            DevicesDataGrid.ItemsSource = _items.DefaultView;
            PriceBox.Text = totalPrice.ToString();
        }

        public void refresh() {
            _computeSelection = true;
            _items = new DataTable();
            if (Utilities.fillDataTable(_items, SqlQueries.GET_ALL_DEVICES)) {
                MessageBox.Show("Ошибка загрузки данных!");
            }
            DevicesDataGrid.DataContext = _items.DefaultView;
            DevicesDataGrid.ItemsSource = _items.DefaultView;
            _computeSelection = false;
        }

        private void addToStorage(object sender, RoutedEventArgs e) {
            if (ModelIdBox.Text == "" || SnBox.Text == "" || PriceBox.Text == "") {
                MessageBox.Show("Проверьте, все ли данные заполнены верно..");
                return;
            }

            Response response;
            if (isEditMode) {
                item.deviceId = ModelIdBox.Text;
                item.destinationId = DestinationComboBox.SelectedValue.ToString();
                item.price = totalPrice.ToString();

                response = item.update();
                if (response.isFailed()) {
                    MessageBox.Show($"Произошла ошибка при обновлении склада, данные не были обновлены!{Environment.NewLine}{response.getErrorMsg()}");
                } else {
                    DialogResult = true;
                }
            } else {
                var entity = new StorageItem(SnBox.Text, ModelIdBox.Text, DestinationComboBox.SelectedValue.ToString(), totalPrice.ToString());
                response = entity.insert();
                if (response.isFailed()) {
                    MessageBox.Show($"Произошла ошибка при обновлении склада, данные не были загружены!{Environment.NewLine}{response.getErrorMsg()}");
                } else {
                    DialogResult = true;
                }
            }
            
        }

        private void exitAddition(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }

        private void openModelCreation(object sender, RoutedEventArgs e) {
            var dialog = new DeviceControl();
            dialog.ShowDialog();
            if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                ModelIdBox.Text = dialog.id.ToString();
                refresh();
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (_computeSelection) return;
            var dv = _items.DefaultView;
            dv.RowFilter = $"`Производитель` like '%{((DataRowView)DeviceManufacComboBox.SelectedItem).Row.ItemArray[1]}%' and `Модель` like '%{SearchBox.Text}%'";
            DevicesDataGrid.ItemsSource = dv;
        }

        private void onKeyUpSearchBox(object sender, KeyEventArgs e) {
            var dv = _items.DefaultView;
            dv.RowFilter = $"`Модель` like '%{SearchBox.Text}%'";
            DevicesDataGrid.ItemsSource = dv;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (_computeSelection) return;
            if (DevicesDataGrid.SelectedItem != null)
                ModelIdBox.Text = ((DataRowView) DevicesDataGrid.SelectedItem).Row.Field<int>("id").ToString();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            // Скрытие столбца с идентификатором записи в таблице с записями
            if(e.PropertyName == "id") {
                e.Cancel = true;
            }
        }

        private void priceLabel_KeyUp(object sender, KeyEventArgs e) {
            try {
                totalPrice = Convert.ToInt16(((TextBox) sender).Text);
            }
            catch (Exception) {
                totalPrice = 0;
            }
        }

        private void priceLabel_GotFocus(object sender, RoutedEventArgs e) {
            if (((TextBox) sender).Text == "0") {
                ((TextBox) sender).Text = "";
            } else {
                ((TextBox) sender).Text = totalPrice.ToString();
            }
        }

        private void priceLabel_LostFocus(object sender, RoutedEventArgs e) {
            ((TextBox) sender).Text = $"{totalPrice:#,0}";
        }

        private void deleteItem(object sender, RoutedEventArgs e) {
            if (item.delete().isFailed()) {
                MessageBox.Show("Удаление невозможно!");
            } else {
                DialogResult = true;
            }
        }
    }
}
