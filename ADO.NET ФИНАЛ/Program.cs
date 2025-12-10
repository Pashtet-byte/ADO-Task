using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace AdoNetPracticeTasks
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
        private const string ConnectionString = @"Server=.\SQLEXPRESS;Database=AdoNetDemoDB;Trusted_Connection=True;Encrypt=False;";
        private const string MasterConnectionString = @"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True;Encrypt=False;";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== ПРАКТИЧЕСКИЕ ЗАДАНИЯ ПО ADO.NET ===\n");

            try
            {
                Console.WriteLine("Инициализация базы данных...");
                await InitializeDatabaseAsync();

                Console.WriteLine("База данных готова! Нажмите любую клавишу для продолжения...");
                Console.ReadKey();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=== ВЫБЕРИТЕ ЗАДАНИЕ ===\n");
                    Console.WriteLine("1. Задание 13.1: Асинхронный метод чтения данных");
                    Console.WriteLine("2. Задание 13.2: Параллельные асинхронные запросы");
                    Console.WriteLine("3. Синхронный метод чтения (для сравнения)");
                    Console.WriteLine("0. Выход");
                    Console.Write("\nВаш выбор: ");

                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        if (choice == 0) break;

                        switch (choice)
                        {
                            case 1:
                                await Task13_1_AsyncDataReading();
                                break;
                            case 2:
                                await Task13_2_ParallelAsyncQueries();
                                break;
                            case 3:
                                await SyncDataReading();
                                break;
                            default:
                                Console.WriteLine("Неверный выбор!");
                                break;
                        }

                        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                    }
                }

                Console.WriteLine("\nПрограмма завершена. До свидания!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Критическая ошибка: {ex.Message}");
                Console.ReadKey();
            }
        }

        // ============================================
        // ЗАДАНИЕ 13.1: Асинхронный метод чтения данных
        // ============================================
        static async Task Task13_1_AsyncDataReading()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАДАНИЕ 13.1: АСИНХРОННЫЙ МЕТОД ЧТЕНИЯ ДАННЫХ ===\n");

            Console.WriteLine("Цель: Переделать синхронный метод чтения данных в асинхронный, используя async/await.\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                Console.WriteLine("1. Старый синхронный метод (для сравнения):");
                var syncStopwatch = Stopwatch.StartNew();
                var syncUsers = ReadUsersSync();
                syncStopwatch.Stop();
                Console.WriteLine($"   Прочитано пользователей синхронно: {syncUsers.Count}");
                Console.WriteLine($"   Время выполнения: {syncStopwatch.ElapsedMilliseconds} мс");

                Console.WriteLine("\n2. Новый асинхронный метод:");
                var asyncStopwatch = Stopwatch.StartNew();
                var asyncUsers = await ReadUsersAsync();
                asyncStopwatch.Stop();
                Console.WriteLine($"   Прочитано пользователей асинхронно: {asyncUsers.Count}");
                Console.WriteLine($"   Время выполнения: {asyncStopwatch.ElapsedMilliseconds} мс");

                Console.WriteLine("\n3. Детали асинхронной реализации:");
                Console.WriteLine("   Использованные асинхронные методы:");
                Console.WriteLine("   - connection.OpenAsync() вместо connection.Open()");
                Console.WriteLine("   - cmd.ExecuteReaderAsync() вместо cmd.ExecuteReader()");
                Console.WriteLine("   - reader.ReadAsync() вместо reader.Read()");

                Console.WriteLine("\n4. Преимущества асинхронного подхода:");
                Console.WriteLine("   ✓ Не блокирует основной поток приложения");
                Console.WriteLine("   ✓ Лучшая масштабируемость при большом количестве запросов");
                Console.WriteLine("   ✓ Возможность параллельного выполнения операций");
                Console.WriteLine("   ✓ Более отзывчивый UI в клиентских приложениях");

                // Демонстрация асинхронного чтения с подробным выводом
                Console.WriteLine("\n5. Демонстрация детального асинхронного чтения:");
                await DemonstrateDetailedAsyncReading();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }
        }

        // Старый синхронный метод чтения (для сравнения)
        static List<User> ReadUsersSync()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open(); // Синхронное открытие соединения

                string query = "SELECT Id, Name, Email, Status, CreatedDate FROM Users";

                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader()) // Синхронное чтение
                {
                    while (reader.Read()) // Синхронное чтение строк
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Status = (UserStatus)reader.GetInt32(3),
                            CreatedDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                        };
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        // Новый асинхронный метод чтения
        static async Task<List<User>> ReadUsersAsync()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync(); // Асинхронное открытие соединения

                string query = "SELECT Id, Name, Email, Status, CreatedDate FROM Users";

                using (var cmd = new SqlCommand(query, connection))
                using (var reader = await cmd.ExecuteReaderAsync()) // Асинхронное чтение
                {
                    while (await reader.ReadAsync()) // Асинхронное чтение строк
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Status = (UserStatus)reader.GetInt32(3),
                            CreatedDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                        };
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        // Демонстрация детального асинхронного чтения
        static async Task DemonstrateDetailedAsyncReading()
        {
            Console.WriteLine("\n6. Пошаговая демонстрация асинхронного чтения:");

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    Console.WriteLine("   Шаг 1: Открываем соединение асинхронно...");
                    await connection.OpenAsync();

                    string query = "SELECT TOP 3 Id, Name, Email FROM Users ORDER BY Id";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        Console.WriteLine("   Шаг 2: Выполняем запрос асинхронно...");
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            Console.WriteLine("   Шаг 3: Читаем данные асинхронно...");
                            int rowCount = 0;

                            while (await reader.ReadAsync())
                            {
                                rowCount++;
                                Console.WriteLine($"\n   Строка {rowCount}:");

                                // Получаем значения
                                var id = reader.GetInt32(0);
                                var name = reader.IsDBNull(1) ? "[NULL]" : reader.GetString(1);
                                var email = reader.IsDBNull(2) ? "[NULL]" : reader.GetString(2);

                                Console.WriteLine($"     ID: {id}");
                                Console.WriteLine($"     Имя: {name}");
                                Console.WriteLine($"     Email: {email}");
                            }

                            Console.WriteLine($"\n   Всего прочитано строк: {rowCount}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ Ошибка при демонстрации: {ex.Message}");
            }
        }

        // ============================================
        // ЗАДАНИЕ 13.2: Параллельные асинхронные запросы
        // ============================================
        static async Task Task13_2_ParallelAsyncQueries()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАДАНИЕ 13.2: ПАРАЛЛЕЛЬНЫЕ АСИНХРОННЫЕ ЗАПРОСЫ ===\n");

            Console.WriteLine("Цель: Создать приложение, которое параллельно выполняет несколько асинхронных запросов к БД.\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                Console.WriteLine("1. Последовательное выполнение запросов:");
                await DemonstrateSequentialQueries();

                Console.WriteLine("\n\n2. Параллельное выполнение запросов:");
                await DemonstrateParallelQueries();

                Console.WriteLine("\n\n3. Сравнение производительности:");
                await ComparePerformance();

                Console.WriteLine("\n\n4. Демонстрация с использованием Task.WhenAll():");
                await DemonstrateTaskWhenAll();

                Console.WriteLine("\n\n5. Демонстрация с отменой операций:");
                await DemonstrateWithCancellation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }
        }

        // Демонстрация последовательного выполнения
        static async Task DemonstrateSequentialQueries()
        {
            Console.WriteLine("   Последовательное выполнение 3 запросов:");

            var stopwatch = Stopwatch.StartNew();

            // Запрос 1: количество пользователей
            using (var conn1 = new SqlConnection(ConnectionString))
            {
                await conn1.OpenAsync();
                var cmd1 = new SqlCommand("SELECT COUNT(*) FROM Users", conn1);
                var count = (int)await cmd1.ExecuteScalarAsync();
                Console.WriteLine($"   Запрос 1: Количество пользователей = {count}");
            }

            // Запрос 2: средняя сумма заказов
            using (var conn2 = new SqlConnection(ConnectionString))
            {
                await conn2.OpenAsync();
                var cmd2 = new SqlCommand("SELECT AVG(Amount) FROM Orders", conn2);
                var avgObj = await cmd2.ExecuteScalarAsync();
                var avg = avgObj == DBNull.Value ? 0m : (decimal)avgObj;
                Console.WriteLine($"   Запрос 2: Средняя сумма заказа = {avg:F2}");
            }

            // Запрос 3: последний пользователь
            using (var conn3 = new SqlConnection(ConnectionString))
            {
                await conn3.OpenAsync();
                var cmd3 = new SqlCommand("SELECT MAX(Name) FROM Users", conn3);
                var lastName = await cmd3.ExecuteScalarAsync();
                Console.WriteLine($"   Запрос 3: Последний пользователь = {lastName}");
            }

            stopwatch.Stop();
            Console.WriteLine($"   Общее время: {stopwatch.ElapsedMilliseconds} мс");
        }

        // Демонстрация параллельного выполнения
        static async Task DemonstrateParallelQueries()
        {
            Console.WriteLine("   Параллельное выполнение 3 запросов:");

            var stopwatch = Stopwatch.StartNew();

            var task1 = Task.Run(async () =>
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("SELECT COUNT(*) FROM Users", conn);
                    return (int)await cmd.ExecuteScalarAsync();
                }
            });

            var task2 = Task.Run(async () =>
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("SELECT AVG(Amount) FROM Orders", conn);
                    var result = await cmd.ExecuteScalarAsync();
                    return result == DBNull.Value ? 0m : (decimal)result;
                }
            });

            var task3 = Task.Run(async () =>
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("SELECT MAX(Name) FROM Users", conn);
                    return await cmd.ExecuteScalarAsync() as string;
                }
            });

            // Ожидаем завершения всех задач
            await Task.WhenAll(task1, task2, task3);

            Console.WriteLine($"   Запрос 1: Количество пользователей = {task1.Result}");
            Console.WriteLine($"   Запрос 2: Средняя сумма заказа = {task2.Result:F2}");
            Console.WriteLine($"   Запрос 3: Последний пользователь = {task3.Result}");

            stopwatch.Stop();
            Console.WriteLine($"   Общее время: {stopwatch.ElapsedMilliseconds} мс");
        }

        // Сравнение производительности
        static async Task ComparePerformance()
        {
            Console.WriteLine("   Сравнение производительности:");

            // Последовательное выполнение
            var sequentialStopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 5; i++)
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("WAITFOR DELAY '00:00:00.1'; SELECT @i", conn);
                    cmd.Parameters.AddWithValue("@i", i);
                    await cmd.ExecuteScalarAsync();
                }
            }
            sequentialStopwatch.Stop();

            // Параллельное выполнение
            var parallelStopwatch = Stopwatch.StartNew();
            var tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    {
                        await conn.OpenAsync();
                        var cmd = new SqlCommand("WAITFOR DELAY '00:00:00.1'; SELECT 1", conn);
                        await cmd.ExecuteScalarAsync();
                    }
                }));
            }
            await Task.WhenAll(tasks);
            parallelStopwatch.Stop();

            Console.WriteLine($"   Последовательное выполнение: {sequentialStopwatch.ElapsedMilliseconds} мс");
            Console.WriteLine($"   Параллельное выполнение: {parallelStopwatch.ElapsedMilliseconds} мс");
            if (parallelStopwatch.ElapsedMilliseconds > 0)
            {
                double speedup = (double)sequentialStopwatch.ElapsedMilliseconds / parallelStopwatch.ElapsedMilliseconds;
                Console.WriteLine($"   Ускорение: {speedup:F2}x");
            }
        }

        // Демонстрация Task.WhenAll
        static async Task DemonstrateTaskWhenAll()
        {
            Console.WriteLine("   Использование Task.WhenAll() для 4 разных запросов:");

            var queries = new[]
            {
                "SELECT COUNT(*) FROM Users",
                "SELECT COUNT(*) FROM Orders",
                "SELECT MIN(CreatedDate) FROM Users",
                "SELECT MAX(OrderDate) FROM Orders"
            };

            var tasks = queries.Select(async (query, index) =>
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand(query, conn);
                    var result = await cmd.ExecuteScalarAsync();
                    return (Index: index + 1, Query: query, Result: result);
                }
            }).ToList();

            Console.WriteLine("   Запущено 4 параллельных запроса...");
            var results = await Task.WhenAll(tasks);

            foreach (var result in results)
            {
                Console.WriteLine($"   Запрос {result.Index}: {result.Query}");
                Console.WriteLine($"     Результат: {result.Result}");
            }
        }

        // Демонстрация с отменой операций
        static async Task DemonstrateWithCancellation()
        {
            Console.WriteLine("   Демонстрация с возможностью отмены:");

            var cancellationTokenSource = new CancellationTokenSource();

            // Запускаем длительную операцию
            var longTask = Task.Run(async () =>
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    {
                        await conn.OpenAsync();
                        var cmd = new SqlCommand("WAITFOR DELAY '00:00:05'; SELECT 'Долгая операция завершена'", conn);

                        // Проверяем токен отмены
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();

                        return await cmd.ExecuteScalarAsync() as string;
                    }
                }
                catch (OperationCanceledException)
                {
                    return "Операция отменена!";
                }
            });

            // Быстрая операция
            var fastTask = Task.Run(async () =>
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("SELECT 'Быстрая операция завершена'", conn);
                    return await cmd.ExecuteScalarAsync() as string;
                }
            });

            // Ожидаем быструю операцию
            var fastResult = await fastTask;
            Console.WriteLine($"   {fastResult}");

            // Предлагаем отменить долгую операцию
            Console.WriteLine("\n   Долгая операция выполняется...");
            Console.WriteLine("   Нажмите 'c' для отмены или любую другую клавишу для продолжения...");

            if (Console.ReadKey().KeyChar == 'c')
            {
                cancellationTokenSource.Cancel();
                Console.WriteLine("\n   Отмена запрошена...");
            }

            try
            {
                var longResult = await longTask;
                Console.WriteLine($"   {longResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ Ошибка: {ex.Message}");
            }
        }

        // Синхронный метод чтения для сравнения
        static async Task SyncDataReading()
        {
            Console.Clear();
            Console.WriteLine("=== СИНХРОННЫЙ МЕТОД ЧТЕНИЯ ДАННЫХ ===\n");

            Console.WriteLine("Демонстрация синхронного подхода (для сравнения с заданием 13.1):\n");

            try
            {
                await EnsureDatabaseExistsAsync();

                var stopwatch = Stopwatch.StartNew();

                // Имитация синхронного блока
                Console.WriteLine("Выполнение синхронных операций...");

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open(); // Блокирующий вызов

                    string query = "SELECT Id, Name, Email FROM Users";

                    using (var cmd = new SqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader()) // Блокирующий вызов
                    {
                        Console.WriteLine("Результаты:");
                        Console.WriteLine(new string('-', 50));

                        while (reader.Read()) // Блокирующий вызов
                        {
                            Console.WriteLine($"  {reader["Id"]}: {reader["Name"]} - {reader["Email"]}");
                        }

                        Console.WriteLine(new string('-', 50));
                    }
                }

                stopwatch.Stop();
                Console.WriteLine($"\nВремя выполнения: {stopwatch.ElapsedMilliseconds} мс");

                Console.WriteLine("\nНедостатки синхронного подхода:");
                Console.WriteLine("✓ Блокирует основной поток приложения");
                Console.WriteLine("✓ Плохая масштабируемость");
                Console.WriteLine("✓ Может вызывать зависание UI");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
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

                // Создаем таблицы
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    string createTablesSql = @"
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
                        END";

                    using (var cmd = new SqlCommand(createTablesSql, connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }

                    // Добавляем тестовые данные
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
                if (ex.Number == 18456)
                {
                    Console.WriteLine("\nПроблема с аутентификацией!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при инициализации БД: {ex.Message}");
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
                    ('Петр Васильев', 'petr@example.com', 3),
                    ('Анна Смирнова', 'anna@example.com', 1),
                    ('Дмитрий Попов', 'dmitry@example.com', 4),
                    ('Екатерина Волкова', 'ekaterina@example.com', 1);
                    
                    INSERT INTO Orders (UserId, Product, Amount) VALUES
                    (1, 'Ноутбук', 75000.00),
                    (1, 'Мышь', 2500.00),
                    (2, 'Клавиатура', 4500.00),
                    (2, 'Монитор', 32000.00),
                    (3, 'Наушники', 8500.00),
                    (4, 'Смартфон', 45000.00),
                    (5, 'Планшет', 28000.00),
                    (6, 'Принтер', 15000.00),
                    (7, 'Сканер', 12000.00),
                    (8, 'Веб-камера', 5000.00);";

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
    }
}