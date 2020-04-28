using System.Data;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    internal class Device {
        public int id { get; }
        public string model { get; set; }
        public int deviceManufacId { get; set; }
        public int deviceTypeId { get; set; }

        public Device() { // Когда нам необходимо создать новую модель
        }

        public Device(int id) { // Когда нам необходимо обновить
            this.id = id;
            if (!download()) {
                new NotificationWindow("Ошибка загрузки данных", Brushes.Tomato).Show();
            }
        }

        /// <summary>
        /// Загрузка модели
        /// </summary>
        /// <returns></returns>
        private bool download() {
            var dt = new DataTable();
            if (Utilities.fillDataTable(dt, "SELECT * FROM devices WHERE id = @ID", new MySqlParameter("@ID", id))) {
                return false;
            } // Заполнение полей данными об услуге из БД
            var row = dt.Rows[0];
            model = row.Field<string>("model");
            deviceManufacId = row.Field<int>("deviceManufacId");
            deviceTypeId = row.Field<int>("deviceTypeId");
            return true;
        }

        public Response insert() {
            return Database.execute("INSERT INTO devices(model, deviceManufacId, deviceTypeId) VALUES(@MODEL, @DEVMANUFAC, @TYPE);", new[] {
                new MySqlParameter("@MODEL", model),
                new MySqlParameter("@DEVMANUFAC", deviceManufacId),
                new MySqlParameter("@TYPE", deviceTypeId)
            }, true);
        }

        public Response update() {
            return Database.execute("UPDATE devices SET model = @MODEL, deviceManufacId = @DEVMANUFAC, deviceTypeId = @TYPE WHERE id = @ID;", new[] {
                new MySqlParameter("@MODEL", model),
                new MySqlParameter("@DEVMANUFAC", deviceManufacId),
                new MySqlParameter("@TYPE", deviceTypeId),
                new MySqlParameter("@ID", id)
            });
        }

        public Response delete() {
            return Database.execute("DELETE FROM devices WHERE id = @ID;", new MySqlParameter("@ID", id));
        }
    }
}
