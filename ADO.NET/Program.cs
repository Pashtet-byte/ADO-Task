using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdoNet30Tasks
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public UserStatus Status { get; set; }
        public string JsonData { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public enum UserStatus
    {
        Active = 1,
        Inactive = 2,
        Suspended = 3,
        Pending = 4
    }

    class Program
    {
        // Измените строку подключения если нужно
        private const string ConnectionString = @"Server=.\SQLEXPRESS;Database=AdoNetDemoDB;Trusted_Connection=True;Encrypt=False;";
        private const string MasterConnectionString = @"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True;Encrypt=False;";

        // ============================================
        // ПРОСТАЯ РЕАЛИЗАЦИЯ КЭША
        // ============================================
        private static readonly Dictionary<string, (object data, DateTime expiry)> _cache =
            new Dictionary<string, (object, DateTime)>();
        private static readonly object _cacheLock = new object();

        private static T GetFromCache<T>(string cacheKey)
        {
            lock (_cacheLock)
            {
                if (_cache.ContainsKey(cacheKey))
                {
                    var (data, expiry) = _cache[cacheKey];
                    if (expiry > DateTime.Now)
                    {
                        Console.WriteLine($"Данные получены из кэша (ключ: {cacheKey})");
                        return (T)data;
                    }
                    else
                    {
                        Console.WriteLine($"Кэш устарел (ключ: {cacheKey})");
                        _cache.Remove(cacheKey);
                    }
                }
                return default(T);
            }
        }

        private static void AddToCache(string cacheKey, object data, TimeSpan duration)
        {
            lock (_cacheLock)
            {
                _cache[cacheKey] = (data, DateTime.Now.Add(duration));
                Console.WriteLine($"Данные добавлены в кэш (ключ: {cacheKey}, срок: {duration})");
            }
        }

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== 30 ЗАДАНИЙ ПО ADO.NET ===\n");

            try
            {
                Console.WriteLine("Инициализация базы данных...");
                await InitializeDatabaseAsync();

                Console.WriteLine("База данных готова! Нажмите любую клавишу для продолжения...");
                Console.ReadKey();

                int choice;
                do
                {
                    Console.Clear();
                    DisplayMenu();

                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        await ExecuteTaskAsync(choice);
                    }

                    if (choice != 0)
                    {
                        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                    }
                } while (choice != 0);

                Console.WriteLine("\nПрограмма завершена. До свидания!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Критическая ошибка: {ex.Message}");
                Console.WriteLine($"Тип ошибки: {ex.GetType().Name}");
                Console.ReadKey();
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("=== ВЫБЕРИТЕ ЗАДАНИЕ (1-30) ===\n");

            string[] tasks = {
                "1. Первое подключение к БД",
                "2. Простой SELECT запрос",
                "3. SqlDataReader для чтения данных",
                "4. INSERT запрос с параметрами",
                "5. UPDATE запрос",
                "6. DELETE запрос",
                "7. Параметризованные запросы vs SQL Injection",
                "8. SqlDataAdapter и DataTable",
                "9. DataSet с несколькими таблицами",
                "10. Обработка исключений при работе с БД",
                "11. Работа с хранимыми процедурами",
                "12. Хранимая процедура с выходными параметрами",
                "13. Транзакции в ADO.NET",
                "14. Уровни изоляции транзакций",
                "15. Пакетные операции (SqlBulkCopy)",
                "16. Асинхронные операции с БД",
                "17. Кэширование результатов запросов",
                "18. Полнотекстовый поиск",
                "19. Работа с NULL значениями",
                "20. Логирование SQL запросов",
                "21. DAL (Data Access Layer)",
                "22. Маппинг результатов на объекты",
                "23. Unit of Work паттерн",
                "24. Repository паттерн с фильтрацией",
                "25. Работа с Enum в БД",
                "26. JSON данные в SQL Server",
                "27. Оптимизация запросов",
                "28. Connection Pooling",
                "29. Миграции БД",
                "30. Интеграционные тесты"
            };

            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
            Console.WriteLine("\n0. Выход");
            Console.Write("\nВаш выбор: ");
        }

        static async Task ExecuteTaskAsync(int taskNumber)
        {
            Console.Clear();
            Console.WriteLine($"=== ЗАДАНИЕ {taskNumber} ===\n");

            try
            {
                switch (taskNumber)
                {
                    case 1: await Task1_FirstConnection(); break;
                    case 2: await Task2_SimpleSelect(); break;
                    case 3: await Task3_SqlDataReader(); break;
                    case 4: await Task4_InsertWithParameters(); break;
                    case 5: await Task5_UpdateQuery(); break;
                    case 6: await Task6_DeleteQuery(); break;
                    case 7: await Task7_SqlInjectionDemo(); break;
                    case 8: await Task8_SqlDataAdapter(); break;
                    case 9: await Task9_DataSetMultipleTables(); break;
                    case 10: await Task10_ExceptionHandling(); break;
                    case 11: await Task11_StoredProcedures(); break;
                    case 12: await Task12_StoredProcedureOutput(); break;
                    case 13: await Task13_Transactions(); break;
                    case 14: await Task14_IsolationLevels(); break;
                    case 15: await Task15_BulkOperations(); break;
                    case 16: await Task16_AsyncOperations(); break;
                    case 17: await Task17_Caching(); break;
                    case 18: await Task18_FullTextSearch(); break;
                    case 19: await Task19_NullValues(); break;
                    case 20: await Task20_QueryLogging(); break;
                    case 21: await Task21_DAL(); break;
                    case 22: await Task22_ObjectMapping(); break;
                    case 23: await Task23_UnitOfWork(); break;
                    case 24: await Task24_RepositoryPattern(); break;
                    case 25: await Task25_EnumInDatabase(); break;
                    case 26: await Task26_JsonInSqlServer(); break;
                    case 27: await Task27_QueryOptimization(); break;
                    case 28: await Task28_ConnectionPooling(); break;
                    case 29: await Task29_Migrations(); break;
                    case 30: await Task30_IntegrationTests(); break;
                    default: Console.WriteLine("Неверный выбор"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка в задании {taskNumber}: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 1: Первое подключение
        // ============================================
        static async Task Task1_FirstConnection()
        {
            Console.WriteLine("Задание 1: Создание первого подключения к БД\n");

            try
            {
                Console.WriteLine("Пытаемся подключиться к БД...");

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("✅ Подключение установлено!");
                    Console.WriteLine($"   Сервер: {connection.DataSource}");
                    Console.WriteLine($"   База данных: {connection.Database}");
                    Console.WriteLine($"   Версия сервера: {connection.ServerVersion}");
                    Console.WriteLine($"   Состояние: {connection.State}");
                    Console.WriteLine($"   Connection Timeout: {connection.ConnectionTimeout} сек");

                    // Проверяем таблицы
                    var cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IN ('Users', 'Orders')",
                        connection);

                    int tableCount = (int)await cmd.ExecuteScalarAsync();
                    Console.WriteLine($"   Таблиц Users и Orders: {tableCount} из 2");

                    await connection.CloseAsync();
                    Console.WriteLine($"   Состояние после закрытия: {connection.State}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"❌ Ошибка SQL Server (код {ex.Number}): {ex.Message}");

                if (ex.Number == 4060)
                {
                    Console.WriteLine("\nРешение проблемы:");
                    Console.WriteLine("1. Проверьте, существует ли база данных 'AdoNetDemoDB'");
                    Console.WriteLine("2. Проверьте права доступа пользователя");
                    Console.WriteLine("3. Проверьте, запущен ли SQL Server");
                }
                else if (ex.Number == -1 || ex.Number == 53)
                {
                    Console.WriteLine("\nРешение проблемы:");
                    Console.WriteLine("1. Убедитесь, что SQL Server запущен");
                    Console.WriteLine("2. Проверьте имя сервера в строке подключения");
                    Console.WriteLine("3. Проверьте брандмауэр");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Общая ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 2: Простой SELECT
        // ============================================
        static async Task Task2_SimpleSelect()
        {
            Console.WriteLine("Задание 2: Выполнение простого SELECT запроса\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    // Проверяем, есть ли данные
                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users", connection);
                    int userCount = (int)await checkCmd.ExecuteScalarAsync();

                    if (userCount == 0)
                    {
                        Console.WriteLine("Таблица Users пуста. Добавляем тестовые данные...");
                        await AddTestDataAsync(connection);
                    }

                    string query = "SELECT Id, Name, Email, CreatedDate FROM Users ORDER BY Id";

                    using (var cmd = new SqlCommand(query, connection))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        Console.WriteLine("Результаты запроса:");
                        Console.WriteLine(new string('-', 70));
                        Console.WriteLine($"| {"ID",4} | {"Имя",-20} | {"Email",-25} | {"Дата создания",-12} |");
                        Console.WriteLine(new string('-', 70));

                        if (!reader.HasRows)
                        {
                            Console.WriteLine("|                   ТАБЛИЦА ПУСТА                              |");
                        }
                        else
                        {
                            int count = 0;
                            while (await reader.ReadAsync())
                            {
                                var id = reader.GetInt32(0);
                                var name = reader.IsDBNull(1) ? "NULL" : reader.GetString(1);
                                var email = reader.IsDBNull(2) ? "NULL" : reader.GetString(2);
                                var createdDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);

                                Console.WriteLine($"| {id,4} | {name,-20} | {email,-25} | {createdDate:dd.MM.yyyy} |");
                                count++;
                            }
                            Console.WriteLine($"\nВсего записей: {count}");
                        }
                        Console.WriteLine(new string('-', 70));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 3: SqlDataReader
        // ============================================
        static async Task Task3_SqlDataReader()
        {
            Console.WriteLine("Задание 3: SqlDataReader для чтения данных\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    // Проверяем данные
                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users", connection);
                    int userCount = (int)await checkCmd.ExecuteScalarAsync();

                    if (userCount == 0)
                    {
                        await AddTestDataAsync(connection);
                    }

                    string query = "SELECT Id, Name, Email, Status FROM Users ORDER BY Id";

                    using (var cmd = new SqlCommand(query, connection))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        Console.WriteLine("Чтение данных через SqlDataReader...\n");

                        int recordCount = 0;
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine($"Запись #{recordCount + 1}:");

                            // Используем GetOrdinal для получения индекса колонки
                            int idIndex = reader.GetOrdinal("Id");
                            int nameIndex = reader.GetOrdinal("Name");
                            int emailIndex = reader.GetOrdinal("Email");
                            int statusIndex = reader.GetOrdinal("Status");

                            int id = reader.GetInt32(idIndex);
                            string name = reader.IsDBNull(nameIndex) ? "Не указано" : reader.GetString(nameIndex);
                            string email = reader.IsDBNull(emailIndex) ? "Не указан" : reader.GetString(emailIndex);
                            int status = reader.GetInt32(statusIndex);

                            Console.WriteLine($"  ID: {id}");
                            Console.WriteLine($"  Имя: {name}");
                            Console.WriteLine($"  Email: {email}");
                            Console.WriteLine($"  Статус: {status}\n");

                            recordCount++;
                        }

                        Console.WriteLine($"✅ Прочитано записей: {recordCount}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 4: INSERT запрос
        // ============================================
        static async Task Task4_InsertWithParameters()
        {
            Console.WriteLine("Задание 4: INSERT запрос в ADO.NET\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                Console.Write("Введите имя пользователя: ");
                string name = Console.ReadLine();

                Console.Write("Введите email: ");
                string email = Console.ReadLine();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        INSERT INTO Users (Name, Email, CreatedDate, Status)
                        VALUES (@Name, @Email, @CreatedDate, @Status);
                        SELECT SCOPE_IDENTITY();";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Status", 1);

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null)
                        {
                            int newId = Convert.ToInt32(result);
                            Console.WriteLine($"\n✅ Пользователь успешно добавлен!");
                            Console.WriteLine($"   ID новой записи: {newId}");
                        }
                        else
                        {
                            Console.WriteLine($"\n❌ Не удалось получить ID новой записи");
                        }
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                Console.WriteLine($"\n❌ Ошибка: Пользователь с таким email уже существует!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 5: UPDATE запрос
        // ============================================
        static async Task Task5_UpdateQuery()
        {
            Console.WriteLine("Задание 5: UPDATE запрос\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    // Сначала покажем существующих пользователей
                    Console.WriteLine("Текущие пользователи:");
                    await ShowUsersBriefAsync(connection);

                    Console.Write("\nВведите ID пользователя для обновления: ");
                    if (!int.TryParse(Console.ReadLine(), out int userId))
                    {
                        Console.WriteLine("❌ Неверный ID!");
                        return;
                    }

                    // Проверяем существование пользователя
                    bool userExists = await CheckUserExistsAsync(userId, connection);
                    if (!userExists)
                    {
                        Console.WriteLine($"❌ Пользователь с ID {userId} не найден!");
                        return;
                    }

                    Console.Write("Введите новый email: ");
                    string newEmail = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newEmail))
                    {
                        Console.WriteLine("❌ Email не может быть пустым!");
                        return;
                    }

                    string updateQuery = "UPDATE Users SET Email = @Email WHERE Id = @Id";

                    using (var cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", userId);
                        cmd.Parameters.AddWithValue("@Email", newEmail);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"\n✅ Email пользователя обновлен!");

                            // Показываем обновленные данные
                            var selectCmd = new SqlCommand(
                                "SELECT Name, Email FROM Users WHERE Id = @Id",
                                connection);
                            selectCmd.Parameters.AddWithValue("@Id", userId);

                            using (var reader = await selectCmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    Console.WriteLine($"   Имя: {reader["Name"]}");
                                    Console.WriteLine($"   Новый email: {reader["Email"]}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n⚠️ Пользователь не найден!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 6: DELETE запрос
        // ============================================
        static async Task Task6_DeleteQuery()
        {
            Console.WriteLine("Задание 6: DELETE запрос\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    // Показываем пользователей
                    Console.WriteLine("Текущие пользователи:");
                    await ShowUsersBriefAsync(connection);

                    Console.Write("\nВведите ID пользователя для удаления: ");
                    if (!int.TryParse(Console.ReadLine(), out int userId))
                    {
                        Console.WriteLine("❌ Неверный ID!");
                        return;
                    }

                    // Проверяем существование пользователя
                    bool userExists = await CheckUserExistsAsync(userId, connection);
                    if (!userExists)
                    {
                        Console.WriteLine($"❌ Пользователь с ID {userId} не найден!");
                        return;
                    }

                    Console.Write($"Вы уверены, что хотите удалить пользователя {userId}? (y/n): ");
                    string confirmation = Console.ReadLine()?.ToLower();

                    if (confirmation != "y")
                    {
                        Console.WriteLine("Удаление отменено.");
                        return;
                    }

                    string deleteQuery = "DELETE FROM Users WHERE Id = @Id";
                    using (var cmd = new SqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", userId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"\n✅ Пользователь удален!");
                            Console.WriteLine($"   Удалено записей: {rowsAffected}");
                        }
                        else
                        {
                            Console.WriteLine("\n⚠️ Пользователь не найден!");
                        }
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                Console.WriteLine($"\n❌ Ошибка ссылочной целостности!");
                Console.WriteLine("   Невозможно удалить запись, так как на нее ссылаются другие таблицы.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 7: SQL Injection демонстрация
        // ============================================
        static async Task Task7_SqlInjectionDemo()
        {
            Console.WriteLine("Задание 7: Параметризованные запросы против SQL Injection\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("1. БЕЗОПАСНЫЙ МЕТОД (параметризованные запросы):");
                    Console.Write("Введите имя для поиска: ");
                    string searchName = Console.ReadLine();

                    string safeQuery = "SELECT * FROM Users WHERE Name LIKE @SearchName";
                    Console.WriteLine($"\nВыполняемый запрос: {safeQuery}");
                    Console.WriteLine($"Параметр: @SearchName = '%{searchName}%'");

                    using (var safeCmd = new SqlCommand(safeQuery, connection))
                    {
                        safeCmd.Parameters.AddWithValue("@SearchName", $"%{searchName}%");

                        using (var reader = await safeCmd.ExecuteReaderAsync())
                        {
                            Console.WriteLine("\nРезультаты:");
                            int count = 0;
                            while (await reader.ReadAsync())
                            {
                                Console.WriteLine($"  Найден: {reader["Name"]} - {reader["Email"]}");
                                count++;
                            }
                            Console.WriteLine($"  Всего найдено: {count}");
                        }
                    }

                    Console.WriteLine("\n2. ДЕМОНСТРАЦИЯ УЯЗВИМОСТИ:");
                    Console.WriteLine("   Если бы мы использовали конкатенацию строк:");
                    Console.WriteLine($"   string dangerousQuery = \"SELECT * FROM Users WHERE Name LIKE '%{searchName}%'\";");
                    Console.WriteLine("   И злоумышленник ввел бы: ' OR 1=1 --");
                    Console.WriteLine($"   Запрос стал бы: SELECT * FROM Users WHERE Name LIKE '%' OR 1=1 --%'");
                    Console.WriteLine("   Что показало бы ВСЕХ пользователей!");

                    Console.WriteLine("\n⚠️ ВНИМАНИЕ: Никогда не используйте конкатенацию строк для SQL запросов!");
                    Console.WriteLine("✅ ВСЕГДА используйте параметризованные запросы!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 8: SqlDataAdapter и DataTable
        // ============================================
        static async Task Task8_SqlDataAdapter()
        {
            Console.WriteLine("Задание 8: Использование SqlDataAdapter и DataTable\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    // Проверяем данные
                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users", connection);
                    int userCount = (int)await checkCmd.ExecuteScalarAsync();

                    if (userCount == 0)
                    {
                        await AddTestDataAsync(connection);
                    }

                    var dataTable = new DataTable("Users");

                    using (var adapter = new SqlDataAdapter("SELECT * FROM Users", connection))
                    {
                        int rowsFilled = adapter.Fill(dataTable);

                        Console.WriteLine($"Загружено строк: {rowsFilled}");
                        Console.WriteLine($"Колонок в DataTable: {dataTable.Columns.Count}");

                        Console.WriteLine("\nСтруктура DataTable:");
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            Console.WriteLine($"  {column.ColumnName}: {column.DataType.Name}");
                        }

                        Console.WriteLine("\nДанные в DataTable (первые 5 строк):");
                        Console.WriteLine(new string('-', 80));
                        Console.WriteLine($"| {"ID",4} | {"Имя",-15} | {"Email",-20} | {"Статус",-10} |");
                        Console.WriteLine(new string('-', 80));

                        for (int i = 0; i < Math.Min(5, dataTable.Rows.Count); i++)
                        {
                            DataRow row = dataTable.Rows[i];
                            Console.WriteLine($"| {row["Id"],4} | " +
                                            $"{(row.IsNull("Name") ? "NULL" : row["Name"]),-15} | " +
                                            $"{(row.IsNull("Email") ? "NULL" : row["Email"]),-20} | " +
                                            $"{row["Status"],-10} |");
                        }
                        Console.WriteLine(new string('-', 80));

                        // Работа с DataRow
                        if (dataTable.Rows.Count > 0)
                        {
                            Console.WriteLine("\nРабота с DataRow:");
                            DataRow firstRow = dataTable.Rows[0];
                            Console.WriteLine($"Первая строка: ID={firstRow["Id"]}, Имя={firstRow["Name"]}");

                            // Изменение данных
                            string originalName = firstRow["Name"].ToString();
                            firstRow["Name"] = "Измененное Имя";
                            Console.WriteLine($"Имя изменено с '{originalName}' на '{firstRow["Name"]}'");
                            Console.WriteLine($"Состояние строки: {firstRow.RowState}");

                            // Отмена изменений
                            firstRow.RejectChanges();
                            Console.WriteLine($"После отмены: Имя={firstRow["Name"]}, Состояние={firstRow.RowState}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 9: DataSet с несколькими таблицами
        // ============================================
        static async Task Task9_DataSetMultipleTables()
        {
            Console.WriteLine("Задание 9: DataSet с несколькими таблицами\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    var dataSet = new DataSet("ShopData");

                    using (var usersAdapter = new SqlDataAdapter("SELECT * FROM Users", connection))
                    using (var ordersAdapter = new SqlDataAdapter("SELECT * FROM Orders", connection))
                    {
                        int usersRows = usersAdapter.Fill(dataSet, "Users");
                        int ordersRows = ordersAdapter.Fill(dataSet, "Orders");

                        Console.WriteLine($"DataSet создан: {dataSet.DataSetName}");
                        Console.WriteLine($"Таблиц в DataSet: {dataSet.Tables.Count}");

                        Console.WriteLine("\nИнформация о таблицах:");
                        foreach (DataTable table in dataSet.Tables)
                        {
                            Console.WriteLine($"  Таблица '{table.TableName}':");
                            Console.WriteLine($"    Строк: {table.Rows.Count}");
                            Console.WriteLine($"    Колонок: {table.Columns.Count}");

                            if (table.Rows.Count > 0)
                            {
                                Console.WriteLine($"    Первая запись: ");
                                DataRow firstRow = table.Rows[0];
                                foreach (DataColumn col in table.Columns)
                                {
                                    Console.WriteLine($"      {col.ColumnName}: {firstRow[col]}");
                                }
                            }
                        }

                        // Создаем отношение между таблицами
                        if (dataSet.Tables.Contains("Users") && dataSet.Tables.Contains("Orders"))
                        {
                            var relation = new DataRelation(
                                "UserOrders",
                                dataSet.Tables["Users"].Columns["Id"],
                                dataSet.Tables["Orders"].Columns["UserId"]);

                            dataSet.Relations.Add(relation);

                            Console.WriteLine("\nСоздано отношение: UserOrders");
                            Console.WriteLine($"  Родительская таблица: {relation.ParentTable.TableName}");
                            Console.WriteLine($"  Дочерняя таблица: {relation.ChildTable.TableName}");

                            // Показываем связанные данные
                            Console.WriteLine("\nСвязанные данные:");
                            foreach (DataRow userRow in dataSet.Tables["Users"].Rows)
                            {
                                var userOrders = userRow.GetChildRows(relation);
                                Console.WriteLine($"Пользователь {userRow["Name"]}: {userOrders.Length} заказов");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 10: Обработка исключений
        // ============================================
        static async Task Task10_ExceptionHandling()
        {
            Console.WriteLine("Задание 10: Обработка исключений при работе с БД\n");

            Console.WriteLine("=== ДЕМОНСТРАЦИЯ РАЗЛИЧНЫХ ТИПОВ ИСКЛЮЧЕНИЙ ===\n");

            Console.WriteLine("1. ОБРАБОТКА SqlException (неверное имя таблицы):");
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    string invalidSql = "SELECT * FROM NonExistentTable";
                    using (var cmd = new SqlCommand(invalidSql, connection))
                    {
                        await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"  SqlException поймана:");
                Console.WriteLine($"    Номер ошибки: {ex.Number}");
                Console.WriteLine($"    Сообщение: {ex.Message}");

                switch (ex.Number)
                {
                    case 208: // Invalid object name
                        Console.WriteLine("    Действие: Проверьте имя таблицы");
                        break;
                    case 4060: // Cannot open database
                        Console.WriteLine("    Действие: Проверьте имя базы данных");
                        break;
                    default:
                        Console.WriteLine("    Действие: Обратитесь к администратору");
                        break;
                }
            }

            Console.WriteLine("\n2. ОБРАБОТКА TimeoutException:");
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new SqlCommand("WAITFOR DELAY '00:00:05'", connection))
                    {
                        cmd.CommandTimeout = 1; // 1 секунда
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == -2) // Timeout
            {
                Console.WriteLine($"  TimeoutException поймана:");
                Console.WriteLine($"    Сообщение: {ex.Message}");
                Console.WriteLine($"    Действие: Увеличьте CommandTimeout или оптимизируйте запрос");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Исключение: {ex.GetType().Name}");
                Console.WriteLine($"    Сообщение: {ex.Message}");
            }

            Console.WriteLine("\n3. ОБРАБОТКА InvalidOperationException:");
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    // Не открываем соединение!
                    using (var cmd = new SqlCommand("SELECT 1", connection))
                    {
                        await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  InvalidOperationException поймана:");
                Console.WriteLine($"    Сообщение: {ex.Message}");
                Console.WriteLine($"    Действие: Убедитесь, что соединение открыто перед использованием");
            }

            Console.WriteLine("\n4. ОБЩАЯ ОБРАБОТКА ИСКЛЮЧЕНИЙ:");
            try
            {
                using (var connection = new SqlConnection("InvalidConnectionString"))
                {
                    await connection.OpenAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Исключение типа {ex.GetType().Name} поймано:");
                Console.WriteLine($"    Сообщение: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 11: Хранимые процедуры
        // ============================================
        static async Task Task11_StoredProcedures()
        {
            Console.WriteLine("Задание 11: Работа с хранимыми процедурами\n");

            try
            {
                await EnsureDatabaseExistsAsync();
                await CreateStoredProceduresAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("1. ВЫЗОВ ХРАНИМОЙ ПРОЦЕДУРЫ БЕЗ ПАРАМЕТРОВ:");
                    using (var cmd = new SqlCommand("GetAllUsers", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            Console.WriteLine("Пользователи:");
                            int count = 0;
                            while (await reader.ReadAsync())
                            {
                                Console.WriteLine($"  {reader["Id"]}: {reader["Name"]} - {reader["Email"]}");
                                count++;
                            }
                            Console.WriteLine($"Всего пользователей: {count}");
                        }
                    }

                    Console.WriteLine("\n2. ВЫЗОВ ХРАНИМОЙ ПРОЦЕДУРЫ С ПАРАМЕТРАМИ:");
                    Console.Write("Введите минимальный статус (1-4): ");
                    if (int.TryParse(Console.ReadLine(), out int minStatus))
                    {
                        using (var cmd = new SqlCommand("GetUsersByStatus", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MinStatus", minStatus);

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                Console.WriteLine($"Пользователи со статусом >= {minStatus}:");
                                int count = 0;
                                while (await reader.ReadAsync())
                                {
                                    Console.WriteLine($"  {reader["Name"]} - Статус: {reader["Status"]}");
                                    count++;
                                }
                                Console.WriteLine($"Найдено: {count} пользователей");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 12: Хранимая процедура с выходными параметрами
        // ============================================
        static async Task Task12_StoredProcedureOutput()
        {
            Console.WriteLine("Задание 12: Хранимая процедура с выходными параметрами\n");

            try
            {
                await EnsureDatabaseExistsAsync();
                await CreateStoredProceduresAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("Вызов процедуры с OUTPUT параметром:");

                    using (var cmd = new SqlCommand("GetUserStatistics", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Входные параметры
                        cmd.Parameters.AddWithValue("@Status", 1);

                        // Выходные параметры
                        var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(totalCountParam);

                        var activeCountParam = new SqlParameter("@ActiveCount", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(activeCountParam);

                        await cmd.ExecuteNonQueryAsync();

                        int totalCount = totalCountParam.Value != DBNull.Value
                            ? (int)totalCountParam.Value
                            : 0;
                        int activeCount = activeCountParam.Value != DBNull.Value
                            ? (int)activeCountParam.Value
                            : 0;

                        Console.WriteLine($"Статистика пользователей:");
                        Console.WriteLine($"  Всего пользователей: {totalCount}");
                        Console.WriteLine($"  Активных пользователей: {activeCount}");
                        Console.WriteLine($"  Неактивных: {totalCount - activeCount}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 13: Транзакции в ADO.NET
        // ============================================
        static async Task Task13_Transactions()
        {
            Console.WriteLine("Задание 13: Транзакции в ADO.NET\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("Начало транзакции...");
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            Console.WriteLine("1. Добавляем нового пользователя...");
                            var insertCmd = new SqlCommand(
                                "INSERT INTO Users (Name, Email, Status) VALUES (@Name, @Email, @Status)",
                                connection, transaction);

                            insertCmd.Parameters.AddWithValue("@Name", "Транзакционный Пользователь");
                            insertCmd.Parameters.AddWithValue("@Email", "transaction@example.com");
                            insertCmd.Parameters.AddWithValue("@Status", 1);

                            int userId = Convert.ToInt32(await insertCmd.ExecuteScalarAsync());
                            Console.WriteLine($"   Добавлен пользователь с ID: {userId}");

                            Console.WriteLine("2. Добавляем заказ для пользователя...");
                            var orderCmd = new SqlCommand(
                                @"INSERT INTO Orders (UserId, Product, Amount) 
                                  VALUES (@UserId, 'Тестовый товар', 1000.00)",
                                connection, transaction);

                            orderCmd.Parameters.AddWithValue("@UserId", userId);
                            await orderCmd.ExecuteNonQueryAsync();
                            Console.WriteLine("   Заказ добавлен");

                            Console.WriteLine("3. Проверяем данные перед коммитом...");
                            var checkCmd = new SqlCommand(
                                "SELECT COUNT(*) FROM Users WHERE Email = 'transaction@example.com'",
                                connection, transaction);

                            int userCount = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());
                            Console.WriteLine($"   Найдено пользователей: {userCount}");

                            Console.Write("Зафиксировать транзакцию? (y/n): ");
                            string answer = Console.ReadLine()?.ToLower();

                            if (answer == "y")
                            {
                                transaction.Commit();
                                Console.WriteLine("✅ Транзакция успешно зафиксирована!");
                            }
                            else
                            {
                                transaction.Rollback();
                                Console.WriteLine("✅ Транзакция откачена!");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"❌ Ошибка в транзакции: {ex.Message}");
                            Console.WriteLine("Откатываем транзакцию...");
                            transaction.Rollback();
                            Console.WriteLine("Транзакция откачена");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 14: Уровни изоляции транзакций
        // ============================================
        static async Task Task14_IsolationLevels()
        {
            Console.WriteLine("Задание 14: Уровни изоляции транзакций\n");

            Console.WriteLine("Доступные уровни изоляции в SQL Server:");
            Console.WriteLine("1. ReadUncommitted - Чтение неподтвержденных данных");
            Console.WriteLine("2. ReadCommitted   - Чтение подтвержденных данных (по умолчанию)");
            Console.WriteLine("3. RepeatableRead  - Повторяемое чтение");
            Console.WriteLine("4. Serializable    - Сериализуемый");
            Console.WriteLine("5. Snapshot        - Снимок состояния");

            try
            {
                await EnsureDatabaseExistsAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("\nДемонстрация уровня ReadCommitted:");
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        var cmd = new SqlCommand("SELECT COUNT(*) FROM Users", connection, transaction);
                        int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        Console.WriteLine($"Количество пользователей: {count}");

                        // Демонстрация блокировок
                        Console.WriteLine("\nПытаемся обновить данные в той же транзакции...");
                        var updateCmd = new SqlCommand(
                            "UPDATE Users SET Email = 'test@example.com' WHERE Id = 1",
                            connection, transaction);

                        int rowsUpdated = await updateCmd.ExecuteNonQueryAsync();
                        Console.WriteLine($"Обновлено строк: {rowsUpdated}");

                        transaction.Commit();
                        Console.WriteLine("Транзакция завершена");
                    }

                    Console.WriteLine("\nДемонстрация уровня Serializable:");
                    using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                    {
                        var cmd = new SqlCommand("SELECT * FROM Users WITH (UPDLOCK)", connection, transaction);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            Console.WriteLine("Заблокированы данные таблицы Users для обновления");
                        }

                        transaction.Commit();
                        Console.WriteLine("Блокировки сняты");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 15: Пакетные операции
        // ============================================
        static async Task Task15_BulkOperations()
        {
            Console.WriteLine("Задание 15: Пакетные операции (SqlBulkCopy)\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("1. Создаем тестовую таблицу для bulk insert...");
                    var createTableCmd = new SqlCommand(@"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BulkTest')
                        BEGIN
                            CREATE TABLE BulkTest (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Name NVARCHAR(100),
                                Value INT,
                                CreatedDate DATETIME DEFAULT GETDATE()
                            )
                            PRINT 'Таблица BulkTest создана'
                        END
                        ELSE
                        BEGIN
                            PRINT 'Таблица BulkTest уже существует'
                            TRUNCATE TABLE BulkTest
                            PRINT 'Таблица BulkTest очищена'
                        END", connection);

                    await createTableCmd.ExecuteNonQueryAsync();

                    Console.WriteLine("2. Подготавливаем данные для массовой вставки...");
                    var dataTable = new DataTable("BulkTest");
                    dataTable.Columns.Add("Name", typeof(string));
                    dataTable.Columns.Add("Value", typeof(int));

                    int recordCount = 1000;
                    for (int i = 1; i <= recordCount; i++)
                    {
                        dataTable.Rows.Add($"Тестовая запись {i}", i * 10);
                    }

                    Console.WriteLine($"   Подготовлено {recordCount} записей");

                    Console.WriteLine("3. Выполняем массовую вставку через SqlBulkCopy...");
                    var stopwatch = Stopwatch.StartNew();

                    using (var bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = "BulkTest";
                        bulkCopy.BatchSize = 100; // 100 записей за раз
                        bulkCopy.BulkCopyTimeout = 60; // 60 секунд
                        bulkCopy.NotifyAfter = 100; // Уведомлять каждые 100 записей

                        bulkCopy.SqlRowsCopied += (sender, e) =>
                        {
                            Console.WriteLine($"   Скопировано записей: {e.RowsCopied}");
                        };

                        await bulkCopy.WriteToServerAsync(dataTable);

                        stopwatch.Stop();
                        Console.WriteLine($"✅ Вставлено {recordCount} записей за {stopwatch.ElapsedMilliseconds} мс");
                    }

                    Console.WriteLine("\n4. Сравнение с обычными INSERT:");
                    Console.WriteLine("   SqlBulkCopy значительно быстрее при больших объемах данных");
                    Console.WriteLine("   Преимущества:");
                    Console.WriteLine("   - Минимальные логирование и блокировки");
                    Console.WriteLine("   - Пакетная обработка");
                    Console.WriteLine("   - Автоматическое сопоставление колонок");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 16: Асинхронные операции
        // ============================================
        static async Task Task16_AsyncOperations()
        {
            Console.WriteLine("Задание 16: Асинхронные операции с БД\n");

            Console.WriteLine("Демонстрация асинхронных методов:");
            Console.WriteLine("1. OpenAsync() - асинхронное открытие соединения");
            Console.WriteLine("2. ExecuteReaderAsync() - асинхронное чтение");
            Console.WriteLine("3. ExecuteNonQueryAsync() - асинхронное выполнение команд");
            Console.WriteLine("4. ExecuteScalarAsync() - асинхронное получение скалярного значения");

            try
            {
                Console.WriteLine("\nЗапускаем несколько асинхронных операций...");

                var stopwatch = Stopwatch.StartNew();

                var tasks = new List<Task>();
                for (int i = 0; i < 3; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using (var connection = new SqlConnection(ConnectionString))
                        {
                            await connection.OpenAsync();

                            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Users", connection))
                            {
                                var result = await cmd.ExecuteScalarAsync();
                                Console.WriteLine($"  Поток: {result} пользователей");
                            }
                        }
                    }));
                }

                await Task.WhenAll(tasks);

                stopwatch.Stop();
                Console.WriteLine($"\n✅ Все операции завершены за {stopwatch.ElapsedMilliseconds} мс");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 17: Кэширование результатов
        // ============================================
        static async Task Task17_Caching()
        {
            Console.WriteLine("Задание 17: Кэширование результатов запросов\n");

            try
            {
                string cacheKey = "all_users";

                Console.WriteLine("Проверяем кэш...");
                var cachedData = GetFromCache<List<User>>(cacheKey);

                if (cachedData != null)
                {
                    Console.WriteLine("✅ Данные получены из кэша");
                    Console.WriteLine($"Количество пользователей в кэше: {cachedData.Count}");
                }
                else
                {
                    Console.WriteLine("❌ Данных нет в кэше, загружаем из БД...");

                    var users = new List<User>();
                    using (var connection = new SqlConnection(ConnectionString))
                    {
                        await connection.OpenAsync();

                        using (var cmd = new SqlCommand("SELECT * FROM Users", connection))
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                users.Add(new User
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Email = reader.GetString(2)
                                });
                            }
                        }
                    }

                    AddToCache(cacheKey, users, TimeSpan.FromMinutes(5));
                    Console.WriteLine($"✅ Данные загружены из БД и сохранены в кэш");
                    Console.WriteLine($"Количество пользователей: {users.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 18: Полнотекстовый поиск
        // ============================================
        static async Task Task18_FullTextSearch()
        {
            Console.WriteLine("Задание 18: Полнотекстовый поиск\n");

            Console.Write("Введите ключевое слово для поиска: ");
            string keyword = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("❌ Ключевое слово не может быть пустым");
                return;
            }

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine($"Поиск пользователей по ключевому слову: '{keyword}'");

                    string query = @"
                        SELECT Id, Name, Email 
                        FROM Users 
                        WHERE Name LIKE @Keyword 
                           OR Email LIKE @Keyword
                        ORDER BY Name";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            Console.WriteLine("Результаты поиска:");
                            Console.WriteLine(new string('-', 60));

                            int count = 0;
                            while (await reader.ReadAsync())
                            {
                                Console.WriteLine($"  {reader["Name"]} - {reader["Email"]}");
                                count++;
                            }

                            if (count == 0)
                            {
                                Console.WriteLine("  Ничего не найдено");
                            }

                            Console.WriteLine(new string('-', 60));
                            Console.WriteLine($"Найдено записей: {count}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 19: Работа с NULL значениями
        // ============================================
        static async Task Task19_NullValues()
        {
            Console.WriteLine("Задание 19: Работа с NULL значениями\n");

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("Добавляем пользователя с NULL значениями...");

                    var insertCmd = new SqlCommand(@"
                        INSERT INTO Users (Name, Email, Status) 
                        VALUES (@Name, @Email, @Status)", connection);

                    insertCmd.Parameters.AddWithValue("@Name", "Тестовый NULL");
                    insertCmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@Status", 1);

                    await insertCmd.ExecuteNonQueryAsync();

                    Console.WriteLine("Читаем данные с NULL значениями...");
                    var selectCmd = new SqlCommand(
                        "SELECT TOP 5 Name, Email, CreatedDate FROM Users ORDER BY Id DESC",
                        connection);

                    using (var reader = await selectCmd.ExecuteReaderAsync())
                    {
                        Console.WriteLine("\nДанные (обработка NULL):");
                        Console.WriteLine(new string('-', 70));

                        while (await reader.ReadAsync())
                        {
                            string name = reader.IsDBNull(0) ? "[NULL]" : reader.GetString(0);
                            string email = reader.IsDBNull(1) ? "[NULL]" : reader.GetString(1);
                            string date = reader.IsDBNull(2)
                                ? "[NULL]"
                                : reader.GetDateTime(2).ToString("dd.MM.yyyy");

                            Console.WriteLine($"Имя: {name,-20} | Email: {email,-25} | Дата: {date}");
                        }

                        Console.WriteLine(new string('-', 70));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 20: Логирование SQL запросов
        // ============================================
        static async Task Task20_QueryLogging()
        {
            Console.WriteLine("Задание 20: Логирование SQL запросов\n");

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    var stopwatch = Stopwatch.StartNew();

                    string query = "SELECT * FROM Users WHERE Status = @Status";
                    Console.WriteLine($"Выполняем запрос: {query}");

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Status", 1);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            int count = 0;
                            while (await reader.ReadAsync())
                            {
                                count++;
                            }

                            stopwatch.Stop();

                            Console.WriteLine("\n=== ЛОГ ЗАПРОСА ===");
                            Console.WriteLine($"Время: {DateTime.Now:HH:mm:ss.fff}");
                            Console.WriteLine($"Запрос: {query}");
                            Console.WriteLine($"Параметр: Status = 1");
                            Console.WriteLine($"Сервер: {connection.DataSource}");
                            Console.WriteLine($"База данных: {connection.Database}");
                            Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
                            Console.WriteLine($"Возвращено строк: {count}");
                            Console.WriteLine("=== КОНЕЦ ЛОГА ===");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 21: DAL (Data Access Layer)
        // ============================================
        static async Task Task21_DAL()
        {
            Console.WriteLine("Задание 21: Создание DAL (Data Access Layer)\n");

            Console.WriteLine("Пример простого репозитория для работы с пользователями:");

            try
            {
                var userRepository = new UserRepository(ConnectionString);

                Console.WriteLine("\n1. Получаем всех пользователей:");
                var allUsers = await userRepository.GetAllAsync();
                Console.WriteLine($"   Всего пользователей: {allUsers.Count()}");

                Console.WriteLine("\n2. Ищем пользователя по email:");
                var user = await userRepository.GetByEmailAsync("ivan@example.com");
                if (user != null)
                {
                    Console.WriteLine($"   Найден: {user.Name} (ID: {user.Id})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 22: Маппинг результатов на объекты
        // ============================================
        static async Task Task22_ObjectMapping()
        {
            Console.WriteLine("Задание 22: Маппинг результатов на объекты (ORM подход)\n");

            try
            {
                Console.WriteLine("Демонстрация маппинга DataReader на объекты User...");

                var users = new List<User>();
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT Id, Name, Email, Status FROM Users";

                    using (var cmd = new SqlCommand(query, connection))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Status = (UserStatus)reader.GetInt32(3)
                            };
                            users.Add(user);
                        }
                    }
                }

                Console.WriteLine($"\nСопоставлено объектов: {users.Count}");
                foreach (var user in users)
                {
                    Console.WriteLine($"  {user.Id}: {user.Name} ({user.Email}) - {user.Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 23: Unit of Work паттерн
        // ============================================
        static async Task Task23_UnitOfWork()
        {
            Console.WriteLine("Задание 23: Unit of Work паттерн\n");

            Console.WriteLine("Паттерн Unit of Work обеспечивает:");
            Console.WriteLine("✓ Управление несколькими операциями в одной транзакции");
            Console.WriteLine("✓ Автоматический откат при ошибках");
            Console.WriteLine("✓ Согласованность данных");

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            Console.WriteLine("Выполняем несколько операций в одной транзакции...");

                            var cmd1 = new SqlCommand(
                                "UPDATE Users SET Email = 'test1@example.com' WHERE Id = 1",
                                connection, transaction);
                            await cmd1.ExecuteNonQueryAsync();

                            var cmd2 = new SqlCommand(
                                "UPDATE Users SET Email = 'test2@example.com' WHERE Id = 2",
                                connection, transaction);
                            await cmd2.ExecuteNonQueryAsync();

                            transaction.Commit();
                            Console.WriteLine("✅ Все операции успешно выполнены в одной транзакции!");
                        }
                        catch
                        {
                            transaction.Rollback();
                            Console.WriteLine("❌ Ошибка! Все изменения откачены.");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 24: Repository паттерн с фильтрацией
        // ============================================
        static async Task Task24_RepositoryPattern()
        {
            Console.WriteLine("Задание 24: Repository паттерн с фильтрацией\n");

            try
            {
                var repository = new UserRepository(ConnectionString);

                Console.WriteLine("Демонстрация различных сценариев фильтрации:");

                Console.WriteLine("\n1. Поиск по ключевому слову 'Иван':");
                var ivanUsers = await repository.SearchAsync("Иван");
                Console.WriteLine($"   Найдено: {ivanUsers.Count()} пользователей");

                Console.WriteLine("\n2. Получение пользователя по ID:");
                var userById = await repository.GetByIdAsync(1);
                if (userById != null)
                {
                    Console.WriteLine($"   Найден: {userById.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 25: Работа с Enum в БД
        // ============================================
        static async Task Task25_EnumInDatabase()
        {
            Console.WriteLine("Задание 25: Работа с Enum в БД\n");

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("Демонстрация сохранения и загрузки Enum значений:");

                    Console.WriteLine("\n1. Сохраняем Enum как число:");
                    var insertCmd = new SqlCommand(
                        "INSERT INTO Users (Name, Email, Status) VALUES (@Name, @Email, @Status)",
                        connection);

                    insertCmd.Parameters.AddWithValue("@Name", "Enum Тест");
                    insertCmd.Parameters.AddWithValue("@Email", "enum@example.com");
                    insertCmd.Parameters.AddWithValue("@Status", (int)UserStatus.Active);

                    await insertCmd.ExecuteNonQueryAsync();
                    Console.WriteLine("   Enum сохранен как число в БД");

                    Console.WriteLine("\n2. Загружаем и преобразуем обратно в Enum:");
                    var selectCmd = new SqlCommand(
                        "SELECT TOP 1 Name, Status FROM Users WHERE Email = 'enum@example.com'",
                        connection);

                    using (var reader = await selectCmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int statusValue = reader.GetInt32(reader.GetOrdinal("Status"));
                            UserStatus status = (UserStatus)statusValue;

                            Console.WriteLine($"   Загружено: {reader["Name"]}");
                            Console.WriteLine($"   Статус как число: {statusValue}");
                            Console.WriteLine($"   Статус как Enum: {status}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 26: JSON данные в SQL Server
        // ============================================
        static async Task Task26_JsonInSqlServer()
        {
            Console.WriteLine("Задание 26: JSON данные в SQL Server\n");

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("Демонстрация работы с JSON в SQL Server:");

                    Console.WriteLine("\n1. Сохраняем JSON данные:");
                    var userPreferences = new
                    {
                        Theme = "dark",
                        Language = "ru-RU",
                        Notifications = true
                    };

                    string jsonData = JsonSerializer.Serialize(userPreferences);

                    var insertCmd = new SqlCommand(
                        "INSERT INTO Users (Name, Email, Status, JsonData) VALUES (@Name, @Email, @Status, @JsonData)",
                        connection);

                    insertCmd.Parameters.AddWithValue("@Name", "JSON Тест");
                    insertCmd.Parameters.AddWithValue("@Email", "json@example.com");
                    insertCmd.Parameters.AddWithValue("@Status", 1);
                    insertCmd.Parameters.AddWithValue("@JsonData", jsonData);

                    await insertCmd.ExecuteNonQueryAsync();
                    Console.WriteLine("   JSON данные сохранены в БД");

                    Console.WriteLine("\n2. Извлекаем и парсим JSON:");
                    var selectCmd = new SqlCommand(
                        "SELECT Name, JsonData FROM Users WHERE Email = 'json@example.com'",
                        connection);

                    using (var reader = await selectCmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string storedJson = reader.GetString(reader.GetOrdinal("JsonData"));
                            var restoredPreferences = JsonSerializer.Deserialize<Dictionary<string, object>>(storedJson);

                            Console.WriteLine($"   Пользователь: {reader["Name"]}");
                            Console.WriteLine($"   JSON данные: {storedJson}");
                            Console.WriteLine($"   Тема: {restoredPreferences["Theme"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 27: Оптимизация запросов
        // ============================================
        static async Task Task27_QueryOptimization()
        {
            Console.WriteLine("Задание 27: Оптимизация запросов\n");

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("Демонстрация оптимизации SQL запросов:");

                    Console.WriteLine("\n1. Создаем индексы для оптимизации:");
                    var createIndexCmd = new SqlCommand(@"
                        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Email')
                        CREATE INDEX IX_Users_Email ON Users(Email);
                        
                        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Status')
                        CREATE INDEX IX_Users_Status ON Users(Status);", connection);

                    await createIndexCmd.ExecuteNonQueryAsync();
                    Console.WriteLine("   Индексы созданы/проверены");

                    Console.WriteLine("\n2. Сравнение запросов:");
                    Console.WriteLine("   Медленный запрос: SELECT * FROM Users");
                    Console.WriteLine("   Быстрый запрос: SELECT * FROM Users WHERE Email = @Email");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 28: Connection Pooling
        // ============================================
        static async Task Task28_ConnectionPooling()
        {
            Console.WriteLine("Задание 28: Connection Pooling\n");

            Console.WriteLine("Демонстрация пула соединений ADO.NET:");

            Console.WriteLine("\n1. Настройка пула соединений в строке подключения:");
            string pooledConnectionString = ConnectionString +
                ";Pooling=true;Min Pool Size=5;Max Pool Size=100;Connection Lifetime=300";

            Console.WriteLine($"   Min Pool Size: 5");
            Console.WriteLine($"   Max Pool Size: 100");
            Console.WriteLine($"   Connection Lifetime: 300 секунд");

            try
            {
                Console.WriteLine("\n2. Многократное открытие соединений (используется пул):");
                var stopwatch = Stopwatch.StartNew();

                var tasks = new List<Task>();
                for (int i = 0; i < 5; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using (var connection = new SqlConnection(pooledConnectionString))
                        {
                            await connection.OpenAsync();

                            using (var cmd = new SqlCommand("SELECT 1", connection))
                            {
                                await cmd.ExecuteScalarAsync();
                            }
                        }
                    }));
                }

                await Task.WhenAll(tasks);
                stopwatch.Stop();

                Console.WriteLine($"\n   Все соединения выполнены за: {stopwatch.ElapsedMilliseconds} мс");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 29: Миграции БД
        // ============================================
        static async Task Task29_Migrations()
        {
            Console.WriteLine("Задание 29: Миграции и управление схемой БД\n");

            Console.WriteLine("Демонстрация системы миграций БД:");

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine("\n1. Создаем таблицу для отслеживания миграций:");
                    var createMigrationTableCmd = new SqlCommand(@"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__MigrationHistory')
                        BEGIN
                            CREATE TABLE __MigrationHistory (
                                MigrationId NVARCHAR(150) PRIMARY KEY,
                                ProductVersion NVARCHAR(32) NOT NULL,
                                AppliedAt DATETIME DEFAULT GETDATE(),
                                Description NVARCHAR(MAX)
                            )
                        END", connection);

                    await createMigrationTableCmd.ExecuteNonQueryAsync();
                    Console.WriteLine("   Таблица миграций создана/проверена");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 30: Интеграционные тесты
        // ============================================
        static async Task Task30_IntegrationTests()
        {
            Console.WriteLine("Задание 30: Интеграционные тесты\n");

            Console.WriteLine("Демонстрация интеграционных тестов для DAL:");

            try
            {
                var repository = new UserRepository(ConnectionString);

                Console.WriteLine("\nТест 1: Получение всех пользователей");
                try
                {
                    var allUsers = await repository.GetAllAsync();
                    Console.WriteLine($"     ✅ Успешно. Получено: {allUsers.Count()} пользователей");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"     ❌ Ошибка: {ex.Message}");
                }

                Console.WriteLine("\nТест 2: Добавление нового пользователя");
                try
                {
                    var newUser = new User
                    {
                        Name = "Интеграционный Тест",
                        Email = $"test_{Guid.NewGuid()}@example.com",
                        Status = UserStatus.Active
                    };

                    var newId = await repository.AddAsync(newUser);
                    Console.WriteLine($"     ✅ Успешно. ID новой записи: {newId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"     ❌ Ошибка: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        // ============================================
        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
        // ============================================

        private static async Task InitializeDatabaseAsync()
        {
            try
            {
                Console.WriteLine("Проверяем наличие базы данных...");

                using (var masterConnection = new SqlConnection(MasterConnectionString))
                {
                    await masterConnection.OpenAsync();

                    // Проверяем, существует ли база данных
                    var checkDbCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM sys.databases WHERE name = 'AdoNetDemoDB'",
                        masterConnection);

                    int dbExists = (int)await checkDbCmd.ExecuteScalarAsync();

                    if (dbExists == 0)
                    {
                        Console.WriteLine("Создаем базу данных AdoNetDemoDB...");
                        var createDbCmd = new SqlCommand(
                            "CREATE DATABASE AdoNetDemoDB",
                            masterConnection);
                        await createDbCmd.ExecuteNonQueryAsync();
                        Console.WriteLine("✅ База данных создана");
                    }
                    else
                    {
                        Console.WriteLine("✅ База данных уже существует");
                    }
                }

                // Теперь создаем таблицы в новой базе данных
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    string createTablesSql = @"
                        -- Создаем таблицу Users если её нет
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
                        BEGIN
                            CREATE TABLE Users (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Name NVARCHAR(100) NOT NULL,
                                Email NVARCHAR(100) UNIQUE,
                                Status INT DEFAULT 1,
                                CreatedDate DATETIME DEFAULT GETDATE(),
                                JsonData NVARCHAR(MAX)
                            );
                            PRINT 'Таблица Users создана';
                        END
                        ELSE
                        BEGIN
                            PRINT 'Таблица Users уже существует';
                        END

                        -- Создаем таблицу Orders если её нет
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Orders')
                        BEGIN
                            CREATE TABLE Orders (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                UserId INT NOT NULL,
                                Product NVARCHAR(100) NOT NULL,
                                Amount DECIMAL(10,2) NOT NULL,
                                OrderDate DATETIME DEFAULT GETDATE(),
                                CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                            );
                            PRINT 'Таблица Orders создана';
                        END
                        ELSE
                        BEGIN
                            PRINT 'Таблица Orders уже существует';
                        END";

                    using (var cmd = new SqlCommand(createTablesSql, connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }

                    // Проверяем, есть ли данные в таблице Users
                    var checkDataCmd = new SqlCommand("SELECT COUNT(*) FROM Users", connection);
                    int userCount = (int)await checkDataCmd.ExecuteScalarAsync();

                    if (userCount == 0)
                    {
                        Console.WriteLine("Добавляем тестовые данные...");
                        await AddTestDataAsync(connection);
                    }

                    Console.WriteLine("✅ База данных инициализирована успешно");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"❌ Ошибка SQL при инициализации БД: {ex.Message}");
                Console.WriteLine($"Код ошибки: {ex.Number}");

                if (ex.Number == 18456)
                {
                    Console.WriteLine("\nПроблема с аутентификацией!");
                    Console.WriteLine("Проверьте строку подключения:");
                    Console.WriteLine($"Сервер: {ConnectionString}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при инициализации БД: {ex.Message}");
                Console.WriteLine($"Тип ошибки: {ex.GetType().Name}");
            }
        }

        private static async Task EnsureDatabaseExistsAsync()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                }
            }
            catch (SqlException ex) when (ex.Number == 4060)
            {
                // База данных не существует, создаем её
                Console.WriteLine("База данных не найдена, создаем...");
                await InitializeDatabaseAsync();
            }
        }

        private static async Task AddTestDataAsync(SqlConnection connection)
        {
            try
            {
                // Добавляем тестовых пользователей
                string insertUsers = @"
                    INSERT INTO Users (Name, Email, Status) VALUES
                    ('Иван Иванов', 'ivan@example.com', 1),
                    ('Мария Петрова', 'maria@example.com', 1),
                    ('Алексей Сидоров', 'alex@example.com', 2),
                    ('Ольга Николаева', 'olga@example.com', 1),
                    ('Петр Васильев', 'petr@example.com', 3);
                    
                    INSERT INTO Orders (UserId, Product, Amount) VALUES
                    (1, 'Ноутбук', 75000.00),
                    (1, 'Мышь', 2500.00),
                    (2, 'Клавиатура', 4500.00),
                    (2, 'Монитор', 32000.00),
                    (3, 'Наушники', 8500.00),
                    (4, 'Смартфон', 45000.00),
                    (5, 'Планшет', 28000.00);
                ";

                using (var cmd = new SqlCommand(insertUsers, connection))
                {
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    Console.WriteLine($"✅ Добавлено тестовых данных: {rowsAffected} строк");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении тестовых данных: {ex.Message}");
            }
        }

        private static async Task<bool> CheckUserExistsAsync(int userId, SqlConnection connection)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);

                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private static async Task ShowUsersBriefAsync(SqlConnection connection)
        {
            try
            {
                string query = "SELECT Id, Name, Email FROM Users ORDER BY Id";
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    Console.WriteLine(new string('-', 60));
                    Console.WriteLine($"| {"ID",4} | {"Имя",-20} | {"Email",-30} |");
                    Console.WriteLine(new string('-', 60));

                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"| {reader["Id"],4} | {reader["Name"],-20} | {reader["Email"],-30} |");
                    }
                    Console.WriteLine(new string('-', 60));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static async Task CreateStoredProceduresAsync()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    string proceduresSql = @"
                        -- Процедура для получения всех пользователей
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetAllUsers')
                        EXEC('
                        CREATE PROCEDURE GetAllUsers
                        AS
                        BEGIN
                            SELECT Id, Name, Email, Status, CreatedDate 
                            FROM Users 
                            ORDER BY Name;
                        END');

                        -- Процедура для получения пользователей по статусу
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetUsersByStatus')
                        EXEC('
                        CREATE PROCEDURE GetUsersByStatus
                            @MinStatus INT
                        AS
                        BEGIN
                            SELECT Id, Name, Email, Status, CreatedDate 
                            FROM Users 
                            WHERE Status >= @MinStatus
                            ORDER BY Status DESC, Name;
                        END');

                        -- Процедура с выходными параметрами
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetUserStatistics')
                        EXEC('
                        CREATE PROCEDURE GetUserStatistics
                            @Status INT,
                            @TotalCount INT OUTPUT,
                            @ActiveCount INT OUTPUT
                        AS
                        BEGIN
                            SELECT @TotalCount = COUNT(*) FROM Users;
                            SELECT @ActiveCount = COUNT(*) FROM Users WHERE Status = @Status;
                        END');
                    ";

                    using (var cmd = new SqlCommand(proceduresSql, connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                        Console.WriteLine("✅ Хранимые процедуры созданы/проверены");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка создания процедур: {ex.Message}");
            }
        }

        // Вспомогательный метод для маппинга пользователей
        private static User MapUserFromReader(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                Status = (UserStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                JsonData = reader.IsDBNull(reader.GetOrdinal("JsonData")) ? null : reader.GetString(reader.GetOrdinal("JsonData"))
            };
        }
    }

    // ============================================
    // РЕАЛИЗАЦИЯ UserRepository
    // ============================================
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Получение всех пользователей
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT Id, Name, Email, CreatedDate, Status, JsonData 
                    FROM Users 
                    ORDER BY Id";

                using (var cmd = new SqlCommand(query, connection))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var user = MapUserFromReader(reader);
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        // Поиск пользователя по email
        public async Task<User> GetByEmailAsync(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT Id, Name, Email, CreatedDate, Status, JsonData 
                    FROM Users 
                    WHERE Email = @Email";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapUserFromReader(reader);
                        }
                    }
                }
            }

            return null;
        }

        // Поиск пользователя по ID
        public async Task<User> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT Id, Name, Email, CreatedDate, Status, JsonData 
                    FROM Users 
                    WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapUserFromReader(reader);
                        }
                    }
                }
            }

            return null;
        }

        // Поиск пользователей по ключевому слову
        public async Task<IEnumerable<User>> SearchAsync(string keyword)
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT Id, Name, Email, CreatedDate, Status, JsonData 
                    FROM Users 
                    WHERE Name LIKE @Keyword OR Email LIKE @Keyword
                    ORDER BY Name";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = MapUserFromReader(reader);
                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        // Добавление нового пользователя
        public async Task<int> AddAsync(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    INSERT INTO Users (Name, Email, Status, JsonData) 
                    VALUES (@Name, @Email, @Status, @JsonData);
                    SELECT SCOPE_IDENTITY();";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", user.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", (int)user.Status);
                    cmd.Parameters.AddWithValue("@JsonData", user.JsonData ?? (object)DBNull.Value);

                    object result = await cmd.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        // Обновление пользователя
        public async Task<bool> UpdateAsync(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    UPDATE Users 
                    SET Name = @Name, 
                        Email = @Email, 
                        Status = @Status, 
                        JsonData = @JsonData
                    WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Name", user.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", (int)user.Status);
                    cmd.Parameters.AddWithValue("@JsonData", user.JsonData ?? (object)DBNull.Value);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Удаление пользователя
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM Users WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Получение пользователей со статусом
        public async Task<IEnumerable<User>> GetByStatusAsync(UserStatus status)
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT Id, Name, Email, CreatedDate, Status, JsonData 
                    FROM Users 
                    WHERE Status = @Status
                    ORDER BY Name";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Status", (int)status);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = MapUserFromReader(reader);
                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        // Вспомогательный метод для маппинга данных из DataReader
        private User MapUserFromReader(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                Status = (UserStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                JsonData = reader.IsDBNull(reader.GetOrdinal("JsonData")) ? null : reader.GetString(reader.GetOrdinal("JsonData"))
            };
        }
    }
}