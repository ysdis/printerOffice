using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace PrinterOffice
{
    /// <summary>
    /// Окно создания новых и редактирования существующих клиентов
    /// </summary>
    public partial class ClientControl : Window {
        private readonly bool isEditMode;
        private Client client;

        // Конструктор создания клиента
        public ClientControl() {
            InitializeComponent();
            client = new Client();
        }

        // Конструктор редактирования клиента
        public ClientControl(string tel) {
            InitializeComponent();

            isEditMode = true;

            header.Content = "Изменение существующего клиента";
            AcceptClientBtn.Content = "Применить";

            var dt = new DataTable();
            Utilities.fillDataTable(dt, "SELECT * FROM customers WHERE tel = @TEL", new MySqlParameter("@TEL", tel));
            var row = dt.Rows[0];

            TelBox.Text = row.Field<string>("tel");
            LastNameBox.Text = row.Field<string>("lastname");
            FirstNameBox.Text = row.Field<string>("firstname");
            MiddleNameBox.Text = row.Field<string>("middlename");
            AddressBox.Text = row.Field<string>("address");
            BirthDatePicker.SelectedDate = row.Field<DateTime>("birthdate");

            client = new Client(tel);
        }

        /// <summary>
        /// Проверяет информацию в полях ввода, загружает информацию о клиенте в БД
        /// </summary>
        private void createCustomerBtnClick(object sender, RoutedEventArgs e) {
            DateTime birthDateTime;
            if (BirthDatePicker.SelectedDate != null) {
                birthDateTime = BirthDatePicker.SelectedDate.Value.Date;
            } else {
                MessageBox.Show("Проверьте, все ли поля заполнены правильно.");
                return;
            }

            if(LastNameBox.Text == "" || FirstNameBox.Text == "" || AddressBox.Text == "" || TelBox.Text == "" || BirthDatePicker.Text == "" || birthDateTime > DateTime.Today) {
                MessageBox.Show("Проверьте, все ли поля заполнены правильно.");
            } else {
                client.tel = TelBox.Text;
                client.lastName = LastNameBox.Text;
                client.firstName = FirstNameBox.Text;
                client.middleName = MiddleNameBox.Text;
                client.birthDate = birthDateTime;
                client.address = AddressBox.Text;

                if(isEditMode) {
                    if(client.update().isFailed()) {
                        MessageBox.Show("Проверьте правильность введённых данных!");
                    } else { DialogResult = true; }
                } else {
                    if(client.insert().isFailed()) {
                        MessageBox.Show("Проверьте правильность введённых данных!");
                    } else { DialogResult = true; }
                }
            }
        }

        /// <summary>
        /// Закрывает окно
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }
    }
}
