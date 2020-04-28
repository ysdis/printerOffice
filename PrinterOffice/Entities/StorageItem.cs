using System.Data;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    internal class StorageItem {
        public string sn { get; set; }
        public string deviceId { get; set; }
        public string destinationId { get; set; }
        public string price { get; set; }

        public StorageItem(string sn) {
            this.sn = sn;
            download();
        }

        public StorageItem(string sn, string deviceId, string destinationId, string price) {
            this.sn = sn;
            this.deviceId = deviceId;
            this.destinationId = destinationId;
            this.price = price;
        }

        private bool download() {
            var dt = new DataTable();
            if (Utilities.fillDataTable(dt, "SELECT * FROM storage WHERE sn = @SN", new MySqlParameter("@SN", sn))) {
                return false;
            } // Заполнение полей данными об услуге из БД
            var row = dt.Rows[0];
            deviceId = row.Field<int>("deviceId").ToString();
            destinationId = row.Field<int>("destinationId").ToString();
            price = row.Field<int>("price").ToString();
            return true;
        }

        public Response insert() {
            return Database.execute("INSERT INTO storage(sn, deviceId, destinationId, price) VALUES(@SN, @DEVICE, @DESTINATION, @PRICE);", new[] {
                new MySqlParameter("@SN", sn),
                new MySqlParameter("@DEVICE", deviceId),
                new MySqlParameter("@DESTINATION", destinationId),
                new MySqlParameter("@PRICE", price)
            });
        }
        public Response update() {
            return Database.execute("UPDATE storage SET deviceId = @DEVICE, destinationId = @DESTINATION, price = @PRICE WHERE sn = @SN;", new[] {
                new MySqlParameter("@SN", sn),
                new MySqlParameter("@DEVICE", deviceId),
                new MySqlParameter("@DESTINATION", destinationId),
                new MySqlParameter("@PRICE", price)
            });
        }

        public Response delete() {
            return Database.execute("DELETE FROM storage WHERE sn = @SN;", new MySqlParameter("@SN", sn));
        }
    }
}
