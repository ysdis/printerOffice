using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Interaction logic for DeviceManufacControl.xaml
    /// </summary>
    public partial class DeviceManufacControl : Window {
        private readonly bool isEditMode;
        private readonly DeviceManufac entity;

        public DeviceManufacControl() {
            InitializeComponent();
            entity = new DeviceManufac();
        }

        public DeviceManufacControl(int id) {
            InitializeComponent();
            isEditMode = true;

            Header.Content = "Изменение существующего производителя";
            AcceptClientBtn.Content = "Применить";
            DeleteBtn.Visibility = Visibility.Visible;

            var dt = new DataTable();
            Utilities.fillDataTable(dt, "SELECT * FROM deviceManufacs WHERE id = @ID", new MySqlParameter("@ID", id));

            TitleBox.Text = dt.Rows[0].Field<string>("title");

            entity = new DeviceManufac(id);
        }

        /// <summary>
        /// Проверяет информацию в полях ввода, загружает информацию в БД
        /// </summary>
        private void createSomethingBtnClick(object sender, RoutedEventArgs e) {
            if(TitleBox.Text == "") {
                MessageBox.Show("Проверьте, все ли поля заполнены правильно.");
            } else {
                entity.title = TitleBox.Text;

                if(isEditMode) {
                    if(entity.update().isFailed()) {
                        MessageBox.Show("Проверьте правильность введённых данных!");
                    } else { DialogResult = true; }
                } else {
                    if(entity.insert().isFailed()) {
                        MessageBox.Show("Проверьте правильность введённых данных!");
                    } else { DialogResult = true; }
                }
            }
        }


        private void deleteAction(object sender, RoutedEventArgs e) {
            if(entity.delete().isFailed()) {
                MessageBox.Show("Удаление невозможно!");
            } else { DialogResult = true; }
        }
    }
}
