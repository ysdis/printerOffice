using MySql.Data.MySqlClient;

namespace PrinterOffice {
    public class Response {
        private readonly bool _isError;
        private readonly MySqlException _exception;
        public int affectedRows { get; }
        public string result { get; }
        public string lastInsertedId { get; set; }

        /// <summary>
        /// Класс ответа от БД
        /// </summary>
        public Response(int affectedRows) {
            this.affectedRows = affectedRows;
        }

        public Response(string result) {
            this.result = result;
        }

        public Response(MySqlException ex) {
            _exception = ex;
            _isError = true;
        }

        /// <summary>
        /// Возникла ли ошибка при выполнении SQL запроса
        /// </summary>
        /// <returns></returns>
        public bool isFailed() {
            return _isError;
        }

        /// <summary>
        /// Успешно ли выполнился SQL-запрос
        /// </summary>
        /// <returns></returns>
        public bool isSuccess() {
            return !_isError;
        }

        /// <summary>
        /// Получить код MySql ошибки
        /// </summary>
        /// <returns></returns>
        public int getErrorCode() {
            return Utilities.getMySqlExceptionErrorCode(_exception);
        }

        public string getErrorMsg() {
            return _exception.Message;
        }
    }
}
