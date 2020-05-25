using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PrinterOffice
{
    /// <summary>
    /// Окно просмотра и редактирования записей в БД
    /// </summary>
    public partial class Browser : Window {

        public const int TYPE_EMPLOYEES = -2;
        public const int TYPE_STORAGE = -1;
        public const int TYPE_CLIENTS = 0;
        public const int TYPE_DEVICES = 1;
        public const int TYPE_DEVICE_TYPES = 2;
        public const int TYPE_DEVICE_MANUFACS = 3;

        private bool _computeSelection;
        private readonly string _sqlQuery;
        private readonly string _successUpdateString;
        private readonly string _successInsertString;

        private readonly int browserType;

        public Browser(string title, string query, int type) {
            InitializeComponent();

            browserType = type;
            _sqlQuery = query;

            Title.Content = title;

            switch(browserType) {
                case TYPE_CLIENTS: {
                        _successUpdateString = "Клиент успешно обновлён!";
                        _successInsertString = "Клиент успешно добавлен!";
                        break;
                    }
                case TYPE_DEVICES: {
                        _successUpdateString = "Модель успешно обновлена!";
                        _successInsertString = "Модель успешно добавлена!";
                        break;
                    }
                case TYPE_DEVICE_TYPES: {
                        _successUpdateString = "Тип устройств успешно обновлен!";
                        _successInsertString = "Тип устройств успешно создан!";
                        break;
                    }
                case TYPE_DEVICE_MANUFACS: {
                    _successUpdateString = "Производитель успешно обновлен!";
                    _successInsertString = "Производитель успешно создан!";
                    break;
                }
                case TYPE_STORAGE: {
                    _successUpdateString = "Позиция успешно обновлена!";
                    _successInsertString = "Позиция успешно создана!";
                    break;
                }
                case TYPE_EMPLOYEES: {
                    _successUpdateString = "Сотрудник успешно обновлен!";
                    _successInsertString = "Сотрудник успешно создан!";
                    break;
                }
            }

            refresh();
        }

        /// <summary>
        /// Реакция на выделение строки в таблице для разных групп данных
        /// </summary>
        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (_computeSelection) return;

            var isSuccess = false;

            switch(browserType) {
                case TYPE_CLIENTS: {
                    var dialog = new ClientControl(((DataRowView)ClientsList.SelectedItem).Row.Field<string>("Номер телефона"));
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successUpdateString);
                        isSuccess = true;
                    }
                    break;
                }
                case TYPE_DEVICES: {
                    var dialog = new DeviceControl(((DataRowView)ClientsList.SelectedItem).Row.Field<int>("id"));
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successUpdateString);
                        isSuccess = true;
                    }
                    break;
                }
                case TYPE_DEVICE_TYPES: {
                    var dialog = new DeviceTypeControl(((DataRowView)ClientsList.SelectedItem).Row.Field<int>("id"));
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successUpdateString);
                        isSuccess = true;
                    }
                    break;
                }
                case TYPE_DEVICE_MANUFACS: {
                    var dialog = new DeviceManufacControl(((DataRowView)ClientsList.SelectedItem).Row.Field<int>("id"));
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successUpdateString);
                        isSuccess = true;
                    }
                    break;
                }
                case TYPE_STORAGE: {
                    var dialog = new StorageItemControl(((DataRowView)ClientsList.SelectedItem).Row.Field<string>("sn"));
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successUpdateString);
                        isSuccess = true;
                    }
                    break;
                }
                case TYPE_EMPLOYEES: {
                    var dialog = new EmployeeControl(((DataRowView)ClientsList.SelectedItem).Row.Field<string>("login"));
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        if (dialog.logout) {
                            new MainWindow().Show();
                            ((AdminPanel)Owner).Close();
                            this.Close();
                        } else {
                            MessageBox.Show(_successUpdateString);
                        }
                        
                        isSuccess = true;
                    }
                    break;
                }
            }

            if(isSuccess) {
                refresh();
                ((AdminPanel)Owner).refresh();
            }
        }

        /// <summary>
        /// Актуализирует информацию в окне
        /// </summary>
        private void refresh() {
            _computeSelection = true;
            var dt = new DataTable();
            Utilities.fillDataTable(dt, _sqlQuery);
            ClientsList.ItemsSource = dt.DefaultView;
            _computeSelection = false;
        }

        /// <summary>
        /// Во время закрытия браузера вызывается метод актуализации родительского окна
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e) {
            ((AdminPanel)Owner).refresh();
        }

        /// <summary>
        /// Скрытие мешающихся столбцов у таблиц для разных групп данных, во время их отображения в таблице
        /// </summary>
        private void ClientsList_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            if (browserType <= TYPE_CLIENTS) return;

            if(e.PropertyName == "id") {
                e.Cancel = true;
            }
        }

        // Действия кнопки "Создать"
        private void createNewSomething(object sender, RoutedEventArgs e) {
            var isSuccess = false;

            switch(browserType) {
                case TYPE_CLIENTS: {
                    var dialog = new ClientControl();
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successInsertString);
                        isSuccess = true;
                    }
                    break;
                }
                case TYPE_DEVICES: {
                    var dialog = new DeviceControl();
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successInsertString);
                        isSuccess = true;
                    }
                    break;
                }
                case TYPE_DEVICE_TYPES: {
                    var dialog = new DeviceTypeControl();
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successInsertString);
                        isSuccess = true;
                    }
                    break;
                }
                case TYPE_DEVICE_MANUFACS: {
                    var dialog = new DeviceManufacControl();
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successInsertString);
                        isSuccess = true;
                    }
                    break;
                }
                case TYPE_STORAGE: {
                    var dialog = new StorageItemControl();
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successInsertString);
                        isSuccess = true;
                    }
                    break;
                }
                case TYPE_EMPLOYEES: {
                    var dialog = new EmployeeControl();
                    dialog.ShowDialog();
                    if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                        MessageBox.Show(_successInsertString);
                        isSuccess = true;
                    }
                    break;
                }
            }

            if(isSuccess) {
                refresh();
                ((AdminPanel)Owner).refresh();
            }
        }
    }
}
