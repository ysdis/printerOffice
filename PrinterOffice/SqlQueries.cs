namespace PrinterOffice {
    /// <summary>
    /// Содержит различные SQL-Запросы к БД
    /// </summary>
    internal class SqlQueries {
        public const string UNITED_DATE_FORMAT = "yyyy-MM-dd HH:mm";
        public const string GET_ALL_ORDERS = "SET lc_time_names = 'ru_RU'; SELECT * FROM ordersDetailed";
        public const string GET_ORDERS_BY_STATUS = "SET lc_time_names = 'ru_RU'; SELECT * FROM ordersDetailed WHERE `Статус` = @STATUSTEXT;";
        public const string GET_ALL_CUSTOMERS = "SET lc_time_names = 'ru_RU'; SELECT * FROM customersDetailed;";
        public const string GET_ALL_DEVICES = "SELECT * FROM devicesdetailed;";
        public const string GET_ALL_DEVICE_TYPES = "SELECT id, title AS 'Название' FROM deviceTypes;";
        public const string GET_ALL_DEVICE_MANUFACS = "SELECT id, title AS 'Название' FROM deviceManufacs;";
        public const string GET_STORAGE = "SELECT * FROM storagedetail;";
        public const string GET_ALL_EMPLOYEES = "SELECT * FROM employeesDetailed;";
        public const string GET_ALL_WORKING_EMPLOYEES = "SELECT * FROM employeesDetailed WHERE `Статус` = 'Работает';";
        public const string GET_JUST_CREATED_ID = "SELECT LAST_INSERT_ID() AS 'latest';";
    }
}
