using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Interaction logic for FireEmployeeControl.xaml
    /// </summary>
    public partial class EmployeeJobStatusControl : Window {
        private readonly bool isRecover;
        private readonly Employee entity;

        // Конструктор редактирования клиента
        public EmployeeJobStatusControl(bool isRecover = false) {
            InitializeComponent();
            this.isRecover = isRecover;
            entity = new Employee();

            Header.Content = (isRecover) ? "Восстановление": "Увольнение";
            AcceptClientBtn.Content = (isRecover) ? "Восстановить" : "Уволить";
        }
        
        private void exitClick(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }

        private void acceptBtnClick(object sender, RoutedEventArgs e) {
            if(LoginBox.Text == "") {
                MessageBox.Show("Проверьте, все ли поля заполнены правильно.");
            } else {

                if (LoginBox.Text == Convert.ToString(Application.Current.Properties["emplLogin"]) && !isRecover) {
                    MessageBox.Show("Увольнение собственного аккаунта запрещено!");
                    return;
                }

                entity.login = LoginBox.Text;

                if(isRecover) {
                    if(entity.hire().isFailed()) {
                        MessageBox.Show("Ошибка, сотрудник не может быть восстановлён!");
                    } else { DialogResult = true; }
                } else {
                    if(entity.fire().isFailed()) {
                        MessageBox.Show("Ошибка, сотрудник не может быть уволен!");
                    } else { DialogResult = true; }
                }
            }
        }
    }
}
