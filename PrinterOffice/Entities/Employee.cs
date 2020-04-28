using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace PrinterOffice {
    internal class Employee {
        public string login { get; set; }
        public string password { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public bool fired { get; set; }
        public bool isAdmin { get; set; }

        private string oldLogin { get; }

        public Employee() {

        }

        public Employee(string login, string password, string lastName, string firstName, string middleName, bool isAdmin) {
            this.login = login;
            this.password = password;
            this.lastName = lastName;
            this.firstName = firstName;
            this.middleName = middleName;
            this.isAdmin = isAdmin;
        }

        public Employee(string login) {
            this.login = login;
            this.oldLogin = login;
            download();
        }

        private bool download() {
            var dt = new DataTable();
            if (Utilities.fillDataTable(dt, "SELECT * FROM employees WHERE login = @LOGIN", new MySqlParameter("@LOGIN", login))) {
                return false;
            } // Заполнение полей данными об услуге из БД
            var row = dt.Rows[0];
            password = row.Field<string>("password");
            lastName = row.Field<string>("lastName");
            firstName = row.Field<string>("firstName");
            middleName = row.Field<string>("middleName");
            fired = Convert.ToBoolean(row.Field<SByte>("fired"));
            isAdmin = Convert.ToBoolean(row.Field<SByte>("isAdmin"));
            return true;
        }

        public Response insert() {
            return Database.execute("INSERT INTO employees(login, password, lastName, firstName, middleName, isAdmin) VALUES(@LOGIN, @PASS, @LAST, @FIRST, @MIDDLE, @ADMIN);", new[] {
                new MySqlParameter("@LOGIN", login),
                new MySqlParameter("@PASS", password),
                new MySqlParameter("@LAST", lastName),
                new MySqlParameter("@FIRST", firstName),
                new MySqlParameter("@MIDDLE", middleName),
                new MySqlParameter("@ADMIN", isAdmin)
            });
        }

        public Response update() {
            return Database.execute("UPDATE employees SET login = @LOGIN, password = @PASS, lastName = @LAST, firstName = @FIRST, middleName = @MIDDLE, fired = @FIRED, isAdmin = @ADMIN WHERE login = @OLDLOGIN;", new[] {
                new MySqlParameter("@LOGIN", login),
                new MySqlParameter("@OLDLOGIN", oldLogin),
                new MySqlParameter("@PASS", password),
                new MySqlParameter("@LAST", lastName),
                new MySqlParameter("@FIRST", firstName),
                new MySqlParameter("@MIDDLE", middleName),
                new MySqlParameter("@FIRED", fired),
                new MySqlParameter("@ADMIN", isAdmin)
            });
        }

        public Response delete() {
            return Database.execute("DELETE FROM employees WHERE login = @LOGIN;", new MySqlParameter("@LOGIN", login));
        }

        public Response fire() {
            return Database.execute("UPDATE employees SET fired = 1 WHERE login = @LOGIN;", new MySqlParameter("@LOGIN", login));
        }

        public Response hire() {
            return Database.execute("UPDATE employees SET fired = 0 WHERE login = @LOGIN;", new MySqlParameter("@LOGIN", login));
        }
    }
}
