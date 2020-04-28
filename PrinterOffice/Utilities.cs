using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    internal class Utilities {

        public const int STATUS_CANCELED = 1;
        public const int STATUS_DONE = 2;
        public const int STATUS_PENDING = 3; 
        public const string UNITED_DATE_FORMAT = "yyyy-MM-dd HH:mm";

        /// <summary>
        /// Возвращает числовой код переданного MySqlException
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static int getMySqlExceptionErrorCode(MySqlException ex) {
            var exception = ex.InnerException as MySqlException;
            var number = exception?.Number ?? ex.Number;
            return number;
        }

        /// <summary>
        /// Заполняет ComboBox данными из БД.
        /// </summary>
        /// <param name="query">SELECT-запрос к БД. Максимальный ответ - 2 стобца</param>
        /// <param name="combo">ComboBox который нужно заполнить</param>
        /// <param name="valueMem">Название столбца значений</param>
        /// <param name="displayMem">Название столбца, который будет отображаться</param>
        /// <param name="firstAll">Наличие первого элемента "Все"</param>
        /// <returns>Вернёт ложь только в случае успешного выполнения</returns>
        public static bool fillDropDown(string query, ComboBox combo, string valueMem, string displayMem, bool firstAll = false) {
            using (var conn = Database.getConnection()) {
                using (var adapter = new MySqlDataAdapter(new MySqlCommand(query, conn))) {
                    try {
                        var dt = new DataTable();

                        conn.Open();
                        adapter.Fill(dt);

                        if(firstAll) {
                            var newRow = dt.NewRow();
                            newRow[0] = "-1";
                            newRow[1] = "Все";
                            dt.Rows.InsertAt(newRow, 0);
                        }

                        if (dt.Rows.Count == 0) return true;

                        combo.ItemsSource = dt.DefaultView;
                        combo.DisplayMemberPath = displayMem;
                        combo.SelectedValuePath = valueMem;
                        combo.SelectedIndex = 0;
                    } catch(MySqlException ex) {
                        MessageBox.Show($"Возникла ошибка: {ex.Message}. Обратитесь к администратору");
                        return true;
                    }
                    return false;   
                }
            }
        }

        /// <summary>
        /// Заполняет DataTable данными из БД
        /// </summary>
        /// <param name="dt">Экземпляр DataTable</param>
        /// <param name="query">SELECT-запрос к БД</param>
        /// <param name="parameters">Список параметров для запроса</param>
        /// <returns>Вернёт ложь только в случае успешного выполнения</returns>
        public static bool fillDataTable(DataTable dt, string query, MySqlParameter[] parameters = null) {
            using (var conn = Database.getConnection()) {
                using (var command = new MySqlCommand(query, conn)) {
                    try {
                        if (parameters != null) command.Parameters.AddRange(parameters);
                        var adapter = new MySqlDataAdapter(command);
                        conn.Open();
                        adapter.Fill(dt);
                    } catch (MySqlException ex) {
                        MessageBox.Show($"Возникла ошибка: {ex.Message}. Обратитесь к администратору");
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Заполняет DataTable данными из БД
        /// </summary>
        /// <param name="dt">Экземпляр DataTable</param>
        /// <param name="query">SELECT-запрос к БД</param>
        /// <param name="parameter">Список параметров для запроса</param>
        /// <returns>Вернёт ложь только в случае успешного выполнения</returns>
        public static bool fillDataTable(DataTable dt, string query, MySqlParameter parameter) {
            using (var conn = Database.getConnection()) {
                using (var command = new MySqlCommand(query, conn)) {
                    try {
                        command.Parameters.Add(parameter);
                        var adapter = new MySqlDataAdapter(command);
                        conn.Open();
                        adapter.Fill(dt);
                    } catch (MySqlException ex) {
                        MessageBox.Show(
                            $"Возникла ошибка с кодом {ex.Message}. Проверьте подключение к интернету либо обратитесь к администратору");
                        return true;
                    }
                    return false;
                }
            }
        }
    }
}
