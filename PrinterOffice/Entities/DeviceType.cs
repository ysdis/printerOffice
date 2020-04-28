using System.Data;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    internal class DeviceType {
        public int id { get; }
        public string title { get; set; }

        public DeviceType(string title) { // Когда нам необходимо создать новую
            this.title = title;
        }

        public DeviceType() { // Когда нам необходимо создать новую
        }

        public DeviceType(int id) { // Когда нам необходимо обновить
            this.id = id;
            if (!download()) {
                new NotificationWindow("Ошибка загрузки данных", Brushes.Tomato).Show();
            }
        }

        private bool download() {
            var dt = new DataTable();
            if (Utilities.fillDataTable(dt, "SELECT * FROM deviceTypes WHERE id = @ID", new MySqlParameter("@ID", id))) {
                return false;
            } // Заполнение полей данными об услуге из БД
            title = dt.Rows[0].Field<string>("title");
            return true;
        }

        public Response insert() {
            return Database.execute("INSERT INTO deviceTypes(title) VALUES(@TITLE);", new MySqlParameter("@TITLE", title));
        }

        public Response update() {
            return Database.execute("UPDATE deviceTypes SET title = @TITLE WHERE id = @ID;", new[] {
                new MySqlParameter("@TITLE", title),
                new MySqlParameter("@ID", id)
            });
        }

        public Response delete() {
            return Database.execute("DELETE FROM deviceTypes WHERE id = @ID;", new[] {
                new MySqlParameter("@TITLE", title),
                new MySqlParameter("@ID", id)
            });
        }
    }
}
