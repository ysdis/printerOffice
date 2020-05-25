using MySql.Data.MySqlClient;

namespace PrinterOffice {
    /// <summary>
    /// Содержит SQL связанные методы для обмена информацией с БД
    /// </summary>
    internal class Database {

        public static MySqlConnection getConnection() {
            return new MySqlConnection(ConnStr);
        }

        /// ConnStr cодержит информацию для подключению к БД
        private const string ConnStr = "Database=printeroffice;Server=localhost;User=root;Password=qwerty78;Port=3305";

        //--------------------------------  МЕТОДЫ  ------------------------------------//

        /// <summary>
        /// Получает значение после выполнения SELECT-запроса к БД с параметрами
        /// </summary>
        /// <param name="query">SELECT запрос с параметрами и placeholder'ами к ним</param>
        /// <param name="parameters">Список параметров для запроса</param>
        /// <returns>Вернёт Response</returns>
        public static Response get(string query, MySqlParameter[] parameters = null) {
            using (var conn = getConnection()) {
                using (var command = new MySqlCommand(query, conn)) {
                    try {
                        if (parameters != null) command.Parameters.AddRange(parameters);
                        conn.Open();
                        return new Response(command.ExecuteScalar().ToString());
                    } catch (MySqlException ex) {
                        return new Response(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Выполняет SQL-запрос, изменяющий/удаляющий записи в БД
        /// </summary>
        /// <param name="query">UPDATE/INSERT/DELETE запрос</param>
        /// <param name="parameters">Список параметров для запроса</param>
        /// <param name="getLastInsertId">Добавить ли в Response ID, только что вставленной записи</param>
        /// <returns>Вернёт Response</returns>
        public static Response execute(string query, MySqlParameter[] parameters = null, bool getLastInsertId = false) {
            using (var conn = getConnection()) {
                using (var command = new MySqlCommand(query, conn)) {
                    try {
                        if (parameters != null) command.Parameters.AddRange(parameters);
                        conn.Open();
                        var response = new Response(command.ExecuteNonQuery());
                        
                        if (!getLastInsertId) return response;
                        
                        using (var commandSecond = new MySqlCommand(SqlQueries.GET_JUST_CREATED_ID, conn)) {
                            response.lastInsertedId = commandSecond.ExecuteScalar().ToString();
                        }
                        return response;
                    } catch(MySqlException ex) {
                        return new Response(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Выполняет SQL-запрос, изменяющий/удаляющий записи в БД
        /// </summary>
        /// <param name="query">UPDATE/INSERT/DELETE запрос</param>
        /// <param name="parameter">Список параметров для запроса</param>
        /// <param name="getLastInsertId">Добавить ли в Response ID, только что вставленной записи</param>
        /// <returns>Вернёт Response</returns>
        public static Response execute(string query, MySqlParameter parameter, bool getLastInsertId = false) {
            using (var conn = getConnection()) {
                using (var command = new MySqlCommand(query, conn)) {
                    try {
                        command.Parameters.Add(parameter);
                        conn.Open();
                        var response = new Response(command.ExecuteNonQuery());
                        
                        if (!getLastInsertId) return response;
                        
                        using (var commandSecond = new MySqlCommand(SqlQueries.GET_JUST_CREATED_ID, conn)) {
                            response.lastInsertedId = commandSecond.ExecuteScalar().ToString();
                        }

                        return response;
                    } catch(MySqlException ex) {
                        return new Response(ex);
                    }
                }
            }
        }
    }
}
