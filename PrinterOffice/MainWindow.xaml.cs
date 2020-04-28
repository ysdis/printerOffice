using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Вход в систему
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void exitApp(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void login(object sender, RoutedEventArgs e) {
            var response = Database.get("SELECT COUNT(*) FROM employees WHERE login = @LOGIN and password = @PASS and fired = 0",
                new[] {
                    new MySqlParameter("@LOGIN", LoginBox.Text),
                    new MySqlParameter("@PASS", PasswordBox.Text)
                });
            if (response.isSuccess()) { // Если не произошло ошибок
                if (response.result == "1") { // Учетные данные сотрудника совпали и сотрудник не уволен
                    response = Database.get("SELECT COUNT(*) FROM employees WHERE login = @LOGIN and isAdmin = 1",
                        new[] {
                            new MySqlParameter("@LOGIN", LoginBox.Text)
                        });
                    if (response.isSuccess()) {
                        Application.Current.Properties["isAdmin"] = (response.result == "1") ? true : false; // Администратор ли сотрудник
                        Application.Current.Properties["emplLogin"] = LoginBox.Text;
                        new AdminPanel().Show();
                        Close();
                    } else {
                        new NotificationWindow(response.getErrorMsg(), Brushes.Tomato).Show();
                    }
                } else {
                    new NotificationWindow("Неверный логин и/или пароль!", Brushes.Tomato).Show();
                }
            } else {
                new NotificationWindow(response.getErrorMsg(), Brushes.Tomato).Show();
            }
        }
    }
}
