using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Содержит всю информацию о заказе, отправляет и обновляет информацию о нём в БД
    /// </summary>
    internal class Order {
        public string    id { get; set; }
        public string    emplLogin { get; set; }
        public string    custId { get; set; }
        public int       totalPrice { get; set; }
        public int       statusId { get; set; }
        public int       serviceId { get; set; }
        public DateTime  startDateTime { get; set; }
        public DateTime  lastModifiedDateTime { get; set; }
        public List<OrderItem> orderItems { get; set; }
        public string    comment { get; set; }

        public const int STATUS_CANCELLED = 1;
        public const int STATUS_DONE = 2;
        public const int STATUS_PENDING = 3;

        private List<int> orderItemsIdsOnLoad; // Идентификаторы устройств в заказе, на момент загрузки из БД
        
        public Order() {
            //custId = tel;
            orderItems = new List<OrderItem>();
            totalPrice = 0; // Вычисляется
            //this.statusId = statusId;
            //this.serviceId = serviceId;
            emplLogin = Convert.ToString(Application.Current.Properties["emplLogin"]);
        }

        public Order(string id) {
            this.id = id;
            orderItems = new List<OrderItem>();
            emplLogin = Convert.ToString(Application.Current.Properties["emplLogin"]);
            download();
        }

        public bool updateItems() {
            var response = Database.execute(
                "DELETE FROM orderItems WHERE orderId = @ORDERID;", 
                new[] {
                    new MySqlParameter("@ORDERID", id)
            });

            return response.isSuccess() && uploadItems(id);
        }

        public bool uploadItems(string newOrderId) {
            foreach (var item in orderItems) {
                item.orderId = Convert.ToInt16(newOrderId);
            }

            return orderItems.All(item => Database.execute(
                "INSERT INTO orderItems(orderId, storageSn, price) VALUES(@ORDERID, @STORAGESN, @PRICE);", 
                new[] {
                    new MySqlParameter("@STORAGESN", item.storageSn),
                    new MySqlParameter("@ORDERID", newOrderId),
                    new MySqlParameter("@PRICE", item.price)
                }).isSuccess());
        }

        /// <summary>
        /// Создаёт заказ в БД
        /// </summary>
        /// <returns>Возвращает -1 в случае ошибки</returns>
        public bool upload() {
            var response = Database.execute(
                "INSERT INTO orders(emplLogin, custId, serviceId, comment) VALUES(@EMPLOYEE, @CUST, @SERVICE, @COMMENT);",
                new[] {
                    new MySqlParameter("@EMPLOYEE", emplLogin),
                    new MySqlParameter("@CUST", custId),
                    new MySqlParameter("@SERVICE", serviceId),
                    new MySqlParameter("@COMMENT", (comment == "") ? "-" : comment)
                }, true);

            if(response.isSuccess()) {
                id = response.lastInsertedId;
                if (uploadItems(id)) {
                    return Database.execute(
                        "UPDATE orders SET totalPrice = @PRICE WHERE id = @ID;",
                        new [] {
                            new MySqlParameter("@PRICE", totalPrice),
                            new MySqlParameter("@ID", id)
                        }).isSuccess();
                }
            }

            return false;
        }

        /// <summary>
        /// Обновляет информацию в комментарии
        /// </summary>
        /// <returns>Возвращает истину только в случае успешного выполнения</returns>
        public bool update() {
            return updateItems() && Database.execute(
                "UPDATE orders SET comment = @COMMENT, totalPrice = @TOTALPRICE WHERE id = @ID;",
                new[] {
                    new MySqlParameter("@COMMENT", (comment == "") ? "-" : comment),
                    new MySqlParameter("@ID", id),
                    new MySqlParameter("@TOTALPRICE", totalPrice)
                }).isSuccess();
        }

        /// <summary>
        /// Обновляет статус заказа
        /// </summary>
        /// <returns>Возвращает Response</returns>
        public Response updateStatus(int status) {
            return Database.execute(
                "UPDATE orders SET statusId = @STATUS WHERE id = @ID;",
                new[] {
                    new MySqlParameter("@STATUS", status),
                    new MySqlParameter("@ID", id)
                });
        }

        /// <summary>
        /// Загружает информацию о заказе из БД
        /// </summary>
        /// <returns>Возвращает истину только в случае успешного выполнения</returns>
        public bool download() {
            var orderDetails = new DataTable();

            if(Utilities.fillDataTable(orderDetails, $"SELECT * FROM orders WHERE id = {id};")) { // Загрузка информации о заказе
                new NotificationWindow("Ошибка загрузки заказа!", Brushes.Tomato).Show();
                return false;
            }

            {
                // Заполнение полей данными о заказе из БД
                var row = orderDetails.Rows[0];
                emplLogin = row.Field<string>("emplLogin");
                custId = row.Field<string>("custId");
                serviceId = row.Field<int>("serviceId");
                startDateTime = row.Field<DateTime>("startDateTime");
                lastModifiedDateTime = row.Field<DateTime>("lastModifiedDateTime");
                statusId = row.Field<int>("statusId");
                comment = row.Field<string>("comment");
                totalPrice = row.Field<int>("totalPrice");
            }

            orderDetails = new DataTable();
            if(Utilities.fillDataTable(orderDetails, "SELECT * FROM orderItems WHERE orderId = @ID;", new MySqlParameter("@ID", id))) { // Получаем идентификаторы услуг заказа
                return false;
            }

            foreach(DataRow row in orderDetails.Rows) { // Заполняем массив устройств в заказе
                orderItems.Add(new OrderItem(
                    row.Field<int>("orderId"),
                    row.Field<string>("storageSn"),
                    row.Field<int>("price"),
                    row.Field<int>("id"))
                );
            }

            return true;
        }
    }
}
