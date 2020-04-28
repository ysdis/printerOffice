using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Interaction logic for EmployeeControl.xaml
    /// </summary>
    public partial class EmployeeControl : Window {

        private readonly bool isEditMode;
        private Employee entity;
        public bool logout = false; // Если пользователь отредактировал сам себя, то его выкинет на страницу входа 

        public EmployeeControl() {
            InitializeComponent();
            entity = new Employee();
        }

        // Конструктор редактирования клиента
        public EmployeeControl(string login) {
            InitializeComponent();

            isEditMode = true;

            Header.Content = "Редактирование сотрудника";
            AcceptClientBtn.Content = "Обновить";
            
            entity = new Employee(login);

            LoginBox.Text = entity.login;
            LastNameBox.Text = entity.lastName;
            FirstNameBox.Text = entity.firstName;
            MiddleNameBox.Text = entity.middleName;
            PasswordBox.Text = entity.password;
            IsAdminCheckBox.IsChecked = entity.isAdmin;
        }

        private void exitClick(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }

        private void createBtnClick(object sender, RoutedEventArgs e) {
            if(LastNameBox.Text == "" || FirstNameBox.Text == "" || PasswordBox.Text == "" || LoginBox.Text == "") {
                MessageBox.Show("Проверьте, все ли поля заполнены правильно.");
            } else {

                entity.lastName = LastNameBox.Text;
                entity.firstName = FirstNameBox.Text;
                entity.middleName = (MiddleNameBox.Text == "") ? "-" : MiddleNameBox.Text;
                entity.login = LoginBox.Text;
                entity.password = PasswordBox.Text;
                entity.isAdmin = Convert.ToBoolean(IsAdminCheckBox.IsChecked);

                if(isEditMode) {
                    if(entity.update().isFailed()) {
                        MessageBox.Show("Проверьте правильность введённых данных!");
                    } else {
                        if (entity.login == Convert.ToString(Application.Current.Properties["emplLogin"])) {
                            MessageBox.Show($"Вы только что изменили свои учётные данные,{Environment.NewLine}пожалуйста, пройдите аутентификацию заново!");
                            logout = true;
                        }
                        DialogResult = true;
                    }
                } else {
                    if(entity.insert().isFailed()) {
                        MessageBox.Show("Проверьте правильность введённых данных!");
                    } else { DialogResult = true; }
                }
            }
        }
    }
}
