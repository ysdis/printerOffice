using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace PrinterOffice {
    public partial class QuickSearch : Window {

        public const int TYPE_EMPLOYEES = -1;
        public const int TYPE_CLIENTS = 0;
        public const int TYPE_DEVICES = 1;

        private bool _isComputerSelection = false;
        private DataTable _items;

        public string SelectedItem;
        public DataRowView SelectedRowView;

        private readonly string _query;
        private readonly string _dataReturnColumn;
        private readonly List<string> _filterByColumns;
        private readonly List<string> _hideColumnName;
        private readonly int _type;

        public QuickSearch(string query, string dataReturnColumn, List<string> filterByColumns, List<string> hideColumnName, int type = 0) { // Изменение существующей оказываемой услуги в заказе
            InitializeComponent();

            _query = query;
            _dataReturnColumn = dataReturnColumn;
            _filterByColumns = filterByColumns;
            _hideColumnName = hideColumnName;
            _type = type;

            switch (type) {
                case TYPE_EMPLOYEES: {
                    QuickCreationBtn.Visibility = Visibility.Collapsed;
                    break;
                }
                case TYPE_CLIENTS: {
                    QuickCreationBtn.Visibility = Visibility.Collapsed;
                    break;
                }
                case TYPE_DEVICES: {
                    
                    break;
                }
            }
            
            // Заполнение DataTable и DataGrid данными из базы
            refresh();
        }

        private void refresh() {
            _isComputerSelection = true;
            _items = new DataTable();
            if(Utilities.fillDataTable(_items, _query)) {
                DialogResult = false;
                return;
            }
            OptionsDataGrid.ItemsSource = _items.DefaultView;
            _isComputerSelection = false;
        }

        private void onKeyUpSearchBox(object sender, KeyEventArgs e) {
            var dv = _items.DefaultView;

            // Формирование строки фильтрации элементов DataGrid
            var searchQuery = $"`{_filterByColumns[0]}` like '%{SearchBox.Text}%'";
            if (_filterByColumns.Count > 1) {
                for(var i = 1; i < _filterByColumns.Count; ++i) {
                    searchQuery += $" or `{_filterByColumns[i]}` like '%{SearchBox.Text}%'";
                }
            }

            dv.RowFilter = searchQuery;
            OptionsDataGrid.ItemsSource = dv;
        }

        private void openModelCreation(object sender, RoutedEventArgs e) {
            var dialog = new DeviceControl();
            dialog.ShowDialog();
            if(dialog.DialogResult.HasValue && dialog.DialogResult.Value) {
                SelectedItem = dialog.id.ToString();
                
                refresh();

                var dv = _items.DefaultView;
                var searchQuery = $"`{_dataReturnColumn}` like '%{SelectedItem}%'";
                dv.RowFilter = searchQuery;
                OptionsDataGrid.ItemsSource = dv;

                OptionsDataGrid.SelectedIndex = 0;
                SelectedRowView = ((DataRowView) OptionsDataGrid.SelectedItem);

                DialogResult = true;
            }
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            if(_hideColumnName.Exists(element => element == e.PropertyName)) {
                e.Cancel = true;
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(_isComputerSelection) return;
            if(OptionsDataGrid.SelectedItem != null) {
                SelectedItem = ((DataRowView) OptionsDataGrid.SelectedItem).Row[_dataReturnColumn].ToString();
                if (_type == TYPE_DEVICES) {
                    _items.Columns.Remove("Модель");
                    _items.Columns.Remove("Тип");
                    _items.Columns.Remove("Статус");
                    _items.Columns.Remove("Предназначение");
                    OptionsDataGrid.ItemsSource = _items.DefaultView;
                }
                SelectedRowView = ((DataRowView) OptionsDataGrid.SelectedItem);
                DialogResult = true;
            }
        }

        private void exitWindow(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }

        // Реакция на изменение выбора фильтра
        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            throw new NotImplementedException();
        }
    }
}
