using System;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Содержит всю информацию о клиенте, отправляет и актуализирует информацию о клиенте в БД
    /// </summary>
    internal class Client {
        public string   tel { get; set; }
        public string   lastName { get; set; }
        public string   firstName { get; set; }
        public string   middleName { get; set; }
        public DateTime birthDate { get; set; }
        public string   address { get; set; }
        private readonly string oldTel; // Хранит телефон, по которому загрузились

        public Client(string tel, string lN, string fN, string mN, string addr, DateTime dt) {
            this.tel = tel;
            lastName = lN;
            firstName = fN;
            middleName = mN;
            address = addr;
            birthDate = dt;
        }

        public Client() {

        }

        public Client(string tel) {
            this.tel = tel;
            oldTel = tel;
        }

        /// <summary>
        /// Создаёт пользователя в БД, если такого не существует. Вернет истину, при возникновении ошибки.
        /// </summary>
        /// <returns></returns>
        public Response insert() {
            return Database.execute(
                    "INSERT INTO customers VALUES(@TEL, @LASTNAME, @FIRSTNAME, @MIDDLENAME, @BIRTHDATE, @ADDRESS);",
                    new[] {
                        new MySqlParameter("@TEL", tel),
                        new MySqlParameter("@FIRSTNAME", firstName),
                        new MySqlParameter("@LASTNAME", lastName),
                        new MySqlParameter("@MIDDLENAME", middleName),
                        new MySqlParameter("@BIRTHDATE", birthDate.ToString("yyyy-MM-dd")),
                        new MySqlParameter("@ADDRESS", address)
                    });
        }

        /// <summary>
        /// Обновляет информацию о клиенте в БД. Вернет истину, при возникновении ошибки.
        /// </summary>
        /// <returns></returns>
        public Response update() {
            return Database.execute(
                "UPDATE customers SET tel = @TEL, lastname =  @LASTNAME, firstname = @FIRSTNAME, middlename = @MIDDLENAME, birthdate = @BIRTHDATE, address = @ADDRESS WHERE tel = @OLDTEL;",
                new[] {
                    new MySqlParameter("@TEL", tel),
                    new MySqlParameter("@OLDTEL", oldTel),
                    new MySqlParameter("@FIRSTNAME", firstName),
                    new MySqlParameter("@LASTNAME", lastName),
                    new MySqlParameter("@MIDDLENAME", middleName),
                    new MySqlParameter("@BIRTHDATE", birthDate.ToString("yyyy-MM-dd")),
                    new MySqlParameter("@ADDRESS", address)
                });
        }
    }
}
