using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Interaction logic for DeviceControl.xaml
    /// </summary>
    public partial class DeviceControl : Window {
        private readonly bool isEditMode;
        private Device device;
        public int id;

        // Конструктор создания клиента
        public DeviceControl() {
            InitializeComponent();
            init();
            device = new Device();
        }

        // Конструктор редактирования клиента
        public DeviceControl(int id) {
            InitializeComponent();
            init();
            isEditMode = true;
            this.id = id;

            header.Content = "Изменение существующей модели";
            AcceptClientBtn.Content = "Применить";
            DeleteBtn.Visibility = Visibility.Visible;

            var dt = new DataTable();
            Utilities.fillDataTable(dt, "SELECT * FROM devices WHERE id = @ID", new MySqlParameter("@ID", id));
            var row = dt.Rows[0];

            ModelBox.Text = row.Field<string>("model");
            ManufacComboBox.SelectedValue = row.Field<int>("deviceManufacId").ToString();
            TypeComboBox.SelectedValue = row.Field<int>("deviceTypeId").ToString();

            device = new Device(id);
        }

        private void init() {
            Utilities.fillDropDown("SELECT * FROM deviceManufacs;", ManufacComboBox, "id", "title");
            Utilities.fillDropDown("SELECT * FROM deviceTypes;", TypeComboBox, "id", "title");
            ManufacComboBox.SelectedIndex = 0;
            TypeComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Проверяет информацию в полях ввода, загружает информацию о клиенте в БД
        /// </summary>
        private void createBtnClick(object sender, RoutedEventArgs e) {
            if(ModelBox.Text == "") {
                MessageBox.Show("Проверьте, все ли поля заполнены правильно.");
            } else {
                device.model = ModelBox.Text;
                device.deviceManufacId = Convert.ToInt16(ManufacComboBox.SelectedValue.ToString());
                device.deviceTypeId = Convert.ToInt16(TypeComboBox.SelectedValue.ToString());

                if(isEditMode) {
                    if(device.update().isFailed()) {
                        MessageBox.Show("Проверьте правильность введённых данных!");
                    } else { DialogResult = true; }
                } else {
                    var response = device.insert();
                    if(response.isFailed()) {
                        MessageBox.Show("Проверьте правильность введённых данных!");
                    } else {
                        id = Convert.ToInt16(response.lastInsertedId);
                        DialogResult = true;
                    }
                }
            }
        }


        private void deleteAction(object sender, RoutedEventArgs e) {
            if(device.delete().isFailed()) {
                MessageBox.Show("Удаление невозможно!");
            } else { DialogResult = true; }
        }
    }
}
