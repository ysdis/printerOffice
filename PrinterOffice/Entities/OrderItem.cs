using System.Data;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    public class OrderItem {
        public int id { get; }
        public int orderId { get; set; }
        public string storageSn { get; set; }
        public int price { get; set; }

        public OrderItem(int orderId, string storageSn, int price = 0, int id = -1) { // Когда нам необходимо создать новое устройство в заказе/ или мы имеем полные данные о нём
            this.orderId = orderId;
            this.storageSn = storageSn;
            this.price = price;
            if (id != -1) this.id = id;
        }

        public OrderItem(string storageSn, int price = 0) { // Просто храним информацию об устройстве в заказе
            this.storageSn = storageSn;
            this.price = price;
        }

        public OrderItem(int id) { // Когда нам необходимо получить услугу в заказе
            this.id = id;
            download();
        }

        /// <summary>
        /// Загрузка услуги из заказа
        /// </summary>
        /// <returns></returns>
        private bool download() {
            var dt = new DataTable();
            if (Utilities.fillDataTable(dt, "SELECT * FROM orderItems WHERE id = @ID", new MySqlParameter("@ID", id))) {
                return false;
            } // Заполнение полей данными об услуге из БД
            var row = dt.Rows[0];
            orderId = row.Field<int>("orderId");
            storageSn = row.Field<string>("storageSn");
            price = row.Field<int>("price");
            return true;
        }

        public Response insert() {
            return Database.execute("INSERT INTO orderitems(orderId, storageSn) VALUES(@ORDERID, @STORAGESN);", new[] {
                new MySqlParameter("@ORDERID", orderId),
                new MySqlParameter("@STORAGESN", storageSn)
            });
        }

        public Response update() {
            return Database.execute("UPDATE orderitems SET price = @PRICE WHERE id = @ID;", new[] {
                new MySqlParameter("@PRICE", price),
                new MySqlParameter("@ID", id)
            });
        }

        /// <summary>
        /// Удаление услуги из заказа
        /// </summary>
        /// <returns></returns>
        public Response delete() {
            return Database.execute("DELETE FROM orderitems WHERE id = @ID);", new MySqlParameter("@ID", id));
        }
    }
}
