using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DataViewCompleteCourse
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ПОЛНЫЙ КУРС ПО DATAVIEW И МЕТОДАМ ПОИСКА (30 ЗАДАНИЙ) ===\n");

            while (true)
            {
                Console.WriteLine("\nВыберите задание для запуска (1-30):");
                for (int i = 1; i <= 30; i++)
                {
                    Console.WriteLine($"{i,2}. Задание {i}");
                }
                Console.WriteLine(" 0. Выход");
                Console.Write("\nВаш выбор: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    continue;
                }

                if (choice == 0)
                {
                    Console.WriteLine("Выход из программы.");
                    return;
                }

                if (choice < 1 || choice > 30)
                {
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    continue;
                }

                Console.Clear();
                Console.WriteLine($"=== ЗАДАНИЕ {choice} ===\n");

                switch (choice)
                {
                    case 1: Task1_DataViewAdvantages(); break;
                    case 2: Task2_FindMethod(); break;
                    case 3: Task3_SelectMethod(); break;
                    case 4: Task4_CreateDataView(); break;
                    case 5: Task5_SortFilterDataView(); break;
                    case 6: Task6_DataViewRowState(); break;
                    case 7: Task7_FindInDataView(); break;
                    case 8: Task8_AddThroughDataView(); break;
                    case 9: Task9_EditThroughDataView(); break;
                    case 10: Task10_DeleteThroughDataView(); break;
                    case 11: Task11_CreateTableFromView(); break;
                    case 12: Task12_CombinedSearch(); break;
                    case 13: Task13_MultiLevelFiltering(); break;
                    case 14: Task14_MultiColumnSorting(); break;
                    case 15: Task15_MultipleDataViews(); break;
                    case 16: Task16_DynamicFiltering(); break;
                    case 17: Task17_DataViewForCharts(); break;
                    case 18: Task18_FindNearestValues(); break;
                    case 19: Task19_ValidationInDataView(); break;
                    case 20: Task20_ExportFromDataView(); break;
                    case 21: Task21_HierarchicalViews(); break;
                    case 22: Task22_AdvancedFiltering(); break;
                    case 23: Task23_GroupingReports(); break;
                    case 24: Task24_CalculatedColumns(); break;
                    case 25: Task25_FindDuplicates(); break;
                    case 26: Task26_SyncDataViews(); break;
                    case 27: Task27_PerformanceOptimization(); break;
                    case 28: Task28_CustomSearchEngine(); break;
                    case 29: Task29_UserSpecificViews(); break;
                    case 30: Task30_ProjectManagementSystem(); break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        #region Задание 1: Основные преимущества DataView
        static void Task1_DataViewAdvantages()
        {
            Console.WriteLine("ЗАДАНИЕ 1: Основные преимущества использования DataView\n");
            Console.WriteLine("Цель: Демонстрация преимуществ DataView по сравнению с прямой работой с DataTable\n");

            // 1. Создание DataTable
            DataTable products = CreateProductsTable();
            FillProductsData(products, 20);

            Console.WriteLine("1. Исходная таблица (первые 5 записей):");
            PrintProductsTable(products, 5);

            // 2. Сравнение фильтрации
            Console.WriteLine("\n2. Сравнение фильтрации:");
            Console.WriteLine("   DataTable.Select() vs DataView с фильтром");

            // Использование DataTable.Select()
            Stopwatch sw1 = Stopwatch.StartNew();
            DataRow[] filteredRows = products.Select("Price > 5000 AND Quantity > 10");
            sw1.Stop();
            Console.WriteLine($"\nDataTable.Select():");
            Console.WriteLine($"  Найдено: {filteredRows.Length} записей");
            Console.WriteLine($"  Время: {sw1.ElapsedMilliseconds} мс");

            // Использование DataView
            Stopwatch sw2 = Stopwatch.StartNew();
            DataView view1 = new DataView(products);
            view1.RowFilter = "Price > 5000 AND Quantity > 10";
            sw2.Stop();
            Console.WriteLine($"\nDataView с фильтром:");
            Console.WriteLine($"  Найдено: {view1.Count} записей");
            Console.WriteLine($"  Время создания: {sw2.ElapsedMilliseconds} мс");

            // 3. Сортировка без изменения исходной таблицы
            Console.WriteLine("\n3. Сортировка в DataView без изменения исходной таблицы:");

            DataView sortedView = new DataView(products);
            sortedView.Sort = "Price DESC";

            Console.WriteLine("DataView (отсортирован по убыванию цены):");
            for (int i = 0; i < Math.Min(5, sortedView.Count); i++)
            {
                Console.WriteLine($"  {sortedView[i]["Name"],-20} {sortedView[i]["Price"],10:C}");
            }

            Console.WriteLine("\nИсходная таблица (не изменилась):");
            for (int i = 0; i < Math.Min(5, products.Rows.Count); i++)
            {
                Console.WriteLine($"  {products.Rows[i]["Name"],-20} {products.Rows[i]["Price"],10:C}");
            }

            // 4. Динамическое изменение фильтра
            Console.WriteLine("\n4. Динамическое изменение фильтра:");

            DataView dynamicView = new DataView(products);
            Console.WriteLine($"Исходный фильтр: нет");
            Console.WriteLine($"Количество записей: {dynamicView.Count}");

            dynamicView.RowFilter = "Category = 'Электроника'";
            Console.WriteLine($"\nПосле фильтра: Category = 'Электроника'");
            Console.WriteLine($"Количество записей: {dynamicView.Count}");

            dynamicView.RowFilter = "Category = 'Электроника' AND Price > 10000";
            Console.WriteLine($"\nПосле добавления фильтра по цене:");
            Console.WriteLine($"Количество записей: {dynamicView.Count}");

            // 5. Несколько представлений одной таблицы
            Console.WriteLine("\n5. Несколько DataView одной таблицы:");

            DataView cheapProducts = new DataView(products);
            cheapProducts.RowFilter = "Price < 5000";
            cheapProducts.Sort = "Price ASC";

            DataView expensiveProducts = new DataView(products);
            expensiveProducts.RowFilter = "Price > 10000";
            expensiveProducts.Sort = "Price DESC";

            Console.WriteLine($"Дешевые товары (< 5000): {cheapProducts.Count} записей");
            Console.WriteLine($"Дорогие товары (> 10000): {expensiveProducts.Count} записей");

            // 6. Автоматическое отражение изменений
            Console.WriteLine("\n6. Автоматическое отражение изменений в DataView:");

            DataView autoUpdateView = new DataView(products);
            autoUpdateView.RowFilter = "Category = 'Электроника'";

            Console.WriteLine($"До добавления: {autoUpdateView.Count} электронных товаров");

            // Добавляем новый товар
            DataRow newRow = products.NewRow();
            newRow["ProductID"] = 999;
            newRow["Name"] = "Новый планшет";
            newRow["Price"] = 25000;
            newRow["Quantity"] = 15;
            newRow["Category"] = "Электроника";
            products.Rows.Add(newRow);

            Console.WriteLine($"После добавления: {autoUpdateView.Count} электронных товаров");

            // 7. Сравнение производительности
            Console.WriteLine("\n7. Сравнение производительности:");
            ComparePerformanceForTask1(products);

            // 8. Отчёт о преимуществах
            Console.WriteLine("\n8. Отчёт о преимуществах DataView:");
            PrintDataViewAdvantagesReport();
        }

        static DataTable CreateProductsTable()
        {
            DataTable table = new DataTable("Товары");

            table.Columns.Add("ProductID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Category", typeof(string));

            return table;
        }

        static void FillProductsData(DataTable table, int count)
        {
            string[] categories = { "Электроника", "Книги", "Одежда", "Бытовая техника", "Спорт" };
            string[] electronics = { "Ноутбук", "Смартфон", "Планшет", "Наушники", "Монитор" };
            string[] books = { "Программирование", "Базы данных", "Дизайн", "Маркетинг", "Философия" };
            string[] clothes = { "Футболка", "Джинсы", "Куртка", "Обувь", "Головной убор" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                string category = categories[rand.Next(categories.Length)];
                string name = "";
                decimal price = 0;

                switch (category)
                {
                    case "Электроника":
                        name = electronics[rand.Next(electronics.Length)] + " " + i;
                        price = rand.Next(5000, 50000);
                        break;
                    case "Книги":
                        name = books[rand.Next(books.Length)] + " " + i;
                        price = rand.Next(500, 3000);
                        break;
                    case "Одежда":
                        name = clothes[rand.Next(clothes.Length)] + " " + i;
                        price = rand.Next(1000, 10000);
                        break;
                    default:
                        name = "Товар " + i;
                        price = rand.Next(100, 20000);
                        break;
                }

                table.Rows.Add(
                    i,
                    name,
                    price,
                    rand.Next(1, 100),
                    category
                );
            }
        }

        static void PrintProductsTable(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-5} {"Название",-20} {"Цена",-10} {"Кол-во",-8} {"Категория",-15}");
            Console.WriteLine(new string('-', 60));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["ProductID"],-5} " +
                                $"{table.Rows[i]["Name"],-20} " +
                                $"{table.Rows[i]["Price"],10:C} " +
                                $"{table.Rows[i]["Quantity"],8} " +
                                $"{table.Rows[i]["Category"],-15}");
            }
        }

        static void ComparePerformanceForTask1(DataTable table)
        {
            int iterations = 1000;
            Random rand = new Random();

            // Тест DataTable.Select()
            Stopwatch swSelect = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                decimal minPrice = rand.Next(1000, 10000);
                DataRow[] rows = table.Select($"Price > {minPrice}");
            }
            swSelect.Stop();

            // Тест DataView с изменением фильтра
            Stopwatch swViewCreate = Stopwatch.StartNew();
            DataView view = new DataView(table);
            swViewCreate.Stop();

            Stopwatch swViewFilter = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                decimal minPrice = rand.Next(1000, 10000);
                view.RowFilter = $"Price > {minPrice}";
                int count = view.Count;
            }
            swViewFilter.Stop();

            Console.WriteLine($"{"Метод",-25} {"Время (мс)",-15} {"Операций/сек",-15}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"Select()",-25} {swSelect.ElapsedMilliseconds,-15} {iterations * 1000 / swSelect.ElapsedMilliseconds,-15:F0}");
            Console.WriteLine($"{"DataView создание",-25} {swViewCreate.ElapsedMilliseconds,-15} {"-",-15}");
            Console.WriteLine($"{"DataView фильтрация",-25} {swViewFilter.ElapsedMilliseconds,-15} {iterations * 1000 / swViewFilter.ElapsedMilliseconds,-15:F0}");

            Console.WriteLine("\nВыводы:");
            Console.WriteLine("• Select() быстрее для разовых запросов");
            Console.WriteLine("• DataView эффективнее при частых изменениях фильтра");
            Console.WriteLine("• DataView сохраняет сортировку и состояние между запросами");
        }

        static void PrintDataViewAdvantagesReport()
        {
            Console.WriteLine("Преимущества DataView:");
            Console.WriteLine("1. Фильтрация без изменения исходных данных");
            Console.WriteLine("2. Сортировка без изменения порядка в таблице");
            Console.WriteLine("3. Динамическое изменение фильтров и сортировки");
            Console.WriteLine("4. Возможность создания нескольких представлений");
            Console.WriteLine("5. Автоматическое обновление при изменении данных");
            Console.WriteLine("6. Кэширование результатов для быстрого доступа");
            Console.WriteLine("7. Поддержка DataViewRowState для работы с разными состояниями строк");
            Console.WriteLine("8. Возможность редактирования через представление");
        }
        #endregion

        #region Задание 2: Метод Find()
        static void Task2_FindMethod()
        {
            Console.WriteLine("ЗАДАНИЕ 2: Поиск по первичному ключу с использованием метода Find()\n");
            Console.WriteLine("Цель: Демонстрация метода Find() при поиске по первичному ключу\n");

            // 1. Создание таблицы сотрудников
            DataTable employees = CreateProjectEmployeesTable();
            FillEmployeesData(employees, 50);

            Console.WriteLine("1. Создана таблица сотрудников (50 записей)");
            Console.WriteLine($"   Первичный ключ: EmployeeID");

            // 2. Поиск одного сотрудника по ID
            Console.WriteLine("\n2. Поиск сотрудника по ID:");

            int[] searchIds = { 1015, 1025, 1035, 1045 };
            foreach (int id in searchIds)
            {
                DataRow employee = employees.Rows.Find(id);

                if (employee != null)
                {
                    Console.WriteLine($"\n✓ Найден сотрудник ID: {id}");
                    PrintEmployeeDetails(employee);
                }
                else
                {
                    Console.WriteLine($"\n✗ Сотрудник с ID {id} не найден");
                }
            }

            // 3. Поиск несуществующего ID
            Console.WriteLine("\n3. Поиск несуществующего ID:");

            int nonExistentId = 9999;
            DataRow notFound = employees.Rows.Find(nonExistentId);

            if (notFound == null)
            {
                Console.WriteLine($"✓ Правильно: ID {nonExistentId} не существует (вернулся null)");
            }

            // 4. Сравнение производительности Find() vs Select()
            Console.WriteLine("\n4. Сравнение производительности Find() vs Select():");
            CompareFindVsSelect(employees);

            // 5. Поиск с использованием DataView и BinarySearch()
            Console.WriteLine("\n5. Поиск с BinarySearch() в DataView:");
            DemoBinarySearchInDataView(employees);

            // 6. Поиск диапазона ID
            Console.WriteLine("\n6. Поиск диапазона ID (1050-1060):");
            DataRow[] rangeResults = FindRange(employees, 1050, 1060);
            Console.WriteLine($"   Найдено сотрудников: {rangeResults.Length}");
            foreach (DataRow emp in rangeResults.Take(3))
            {
                Console.WriteLine($"   • {emp["EmployeeID"]}: {emp["FullName"]} - {emp["Department"]}");
            }

            // 7. Поиск с обработкой исключений
            Console.WriteLine("\n7. Поиск с обработкой исключений:");
            DemoSearchWithExceptions(employees);

            // 8. Вывод статистики
            Console.WriteLine("\n8. Статистика поиска:");
            PrintFindMethodStatistics();
        }

        static DataTable CreateEmployeesTable()
        {
            DataTable table = new DataTable("Сотрудники");

            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("FullName", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("Salary", typeof(decimal));

            // Установка первичного ключа
            table.PrimaryKey = new DataColumn[] { table.Columns["EmployeeID"] };

            return table;
        }

        static void FillEmployeesData(DataTable table, int count)
        {
            string[] departments = { "IT", "HR", "Finance", "Sales", "Marketing", "Operations", "Support", "R&D" };
            string[] firstNames = { "Иван", "Мария", "Петр", "Анна", "Дмитрий", "Светлана", "Алексей", "Елена",
                                   "Николай", "Ольга", "Сергей", "Татьяна", "Андрей", "Юлия", "Михаил" };
            string[] lastNames = { "Петров", "Иванов", "Сидоров", "Смирнов", "Кузнецов", "Попов", "Васильев",
                                  "Соколов", "Михайлов", "Новиков", "Федоров", "Морозов", "Волков", "Алексеев" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];
                string email = $"{firstName.ToLower()}.{lastName.ToLower()}@company.com";
                string department = departments[rand.Next(departments.Length)];
                decimal salary = Math.Round(30000 + (decimal)(rand.NextDouble() * 120000), 2);

                table.Rows.Add(
                    1000 + i,                    // EmployeeID
                    $"{firstName} {lastName}",   // FullName
                    email,                       // Email
                    department,                  // Department
                    salary                       // Salary
                );
            }
        }

        static void PrintEmployeeDetails(DataRow employee)
        {
            Console.WriteLine($"   Имя: {employee["FullName"]}");
            Console.WriteLine($"   Email: {employee["Email"]}");
            Console.WriteLine($"   Отдел: {employee["Department"]}");
            Console.WriteLine($"   Зарплата: {employee["Salary"]:C}");
        }

        static void CompareFindVsSelect(DataTable table)
        {
            int iterations = 10000;
            Random rand = new Random();

            Stopwatch swFind = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                int id = 1001 + (rand.Next() % 50);
                DataRow row = table.Rows.Find(id);
            }
            swFind.Stop();

            Stopwatch swSelect = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                int id = 1001 + (rand.Next() % 50);
                DataRow[] rows = table.Select($"EmployeeID = {id}");
            }
            swSelect.Stop();

            Console.WriteLine($"{"Метод",-20} {"Время (мс)",-15} {"Операций/сек",-15}");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"{"Find()",-20} {swFind.ElapsedMilliseconds,-15} {iterations * 1000 / swFind.ElapsedMilliseconds,-15:F0}");
            Console.WriteLine($"{"Select()",-20} {swSelect.ElapsedMilliseconds,-15} {iterations * 1000 / swSelect.ElapsedMilliseconds,-15:F0}");

            double ratio = (double)swSelect.ElapsedMilliseconds / swFind.ElapsedMilliseconds;
            Console.WriteLine($"\nFind() быстрее Select() в {ratio:F1} раза для поиска по PK");
        }

        static void DemoBinarySearchInDataView(DataTable table)
        {
            DataView sortedView = new DataView(table);
            sortedView.Sort = "EmployeeID ASC";

            int[] testIds = { 1010, 1020, 1030, 1040 };

            foreach (int id in testIds)
            {
                int index = sortedView.Find(id);
                if (index >= 0)
                {
                    Console.WriteLine($"   ID {id}: найден на позиции {index} ({sortedView[index]["FullName"]})");
                }
            }
        }

        static DataRow[] FindRange(DataTable table, int startId, int endId)
        {
            List<DataRow> results = new List<DataRow>();

            for (int id = startId; id <= endId; id++)
            {
                DataRow row = table.Rows.Find(id);
                if (row != null)
                {
                    results.Add(row);
                }
            }

            return results.ToArray();
        }

        static void DemoSearchWithExceptions(DataTable table)
        {
            Console.WriteLine("   а) Поиск с неверным типом данных:");
            try
            {
                DataRow row = table.Rows.Find("не число");
                Console.WriteLine("      ✗ Ожидалось исключение, но его не было");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"      ✓ Правильно обработано: {ex.Message}");
            }

            Console.WriteLine("   б) Поиск с null ID:");
            try
            {
                DataRow row = table.Rows.Find(null);
                Console.WriteLine("      ✓ Правильно вернул null для null ключа");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      ✗ Неожиданная ошибка: {ex.Message}");
            }
        }

        static void PrintFindMethodStatistics()
        {
            Console.WriteLine("Преимущества метода Find():");
            Console.WriteLine("• Самый быстрый способ поиска по первичному ключу");
            Console.WriteLine("• Использует внутренние индексы таблицы");
            Console.WriteLine("• Возвращает одну строку или null");
            Console.WriteLine("• Идеален для точного поиска по уникальным ключам");
            Console.WriteLine("• Не требует синтаксического анализа выражений");
        }
        #endregion

        #region Задание 3: Метод Select()
        static void Task3_SelectMethod()
        {
            Console.WriteLine("ЗАДАНИЕ 3: Фильтрация данных с использованием метода Select()\n");
            Console.WriteLine("Цель: Демонстрация различных фильтров через Select()\n");

            // 1. Создание таблицы заказов
            DataTable orders = CreateOrdersTable();
            FillOrdersData(orders, 150);

            Console.WriteLine("1. Создана таблица заказов (150 записей)");

            // 2. Заказы с суммой больше X
            Console.WriteLine("\n2. Заказы с суммой > 5000:");
            DataRow[] expensiveOrders = orders.Select("Amount > 5000");
            Console.WriteLine($"   Найдено: {expensiveOrders.Length} заказов");
            PrintOrdersSample(expensiveOrders, 3);

            // 3. Заказы определённого статуса
            Console.WriteLine("\n3. Заказы со статусом 'Shipped':");
            DataRow[] shippedOrders = orders.Select("Status = 'Shipped'");
            Console.WriteLine($"   Найдено: {shippedOrders.Length} заказов");

            // 4. Заказы в диапазоне дат
            Console.WriteLine("\n4. Заказы за январь 2024:");
            DataRow[] januaryOrders = orders.Select("OrderDate >= '2024-01-01' AND OrderDate <= '2024-01-31'");
            Console.WriteLine($"   Найдено: {januaryOrders.Length} заказов");

            // 5. Комбинированные фильтры
            Console.WriteLine("\n5. Комбинированный фильтр: статус 'Delivered' AND сумма > 3000:");
            DataRow[] combined = orders.Select("Status = 'Delivered' AND Amount > 3000");
            Console.WriteLine($"   Найдено: {combined.Length} заказов");
            PrintOrdersSample(combined, 3);

            // 6. Заказы определённого клиента
            Console.WriteLine("\n6. Заказы клиента с ID 101:");
            DataRow[] clientOrders = orders.Select("CustomerID = 101");
            Console.WriteLine($"   Найдено: {clientOrders.Length} заказов");

            // 7. Поиск с использованием LIKE
            Console.WriteLine("\n7. Заказы с адресом содержащим 'Москва':");
            DataRow[] moscowOrders = orders.Select("ShippingAddress LIKE '%Москва%'");
            Console.WriteLine($"   Найдено: {moscowOrders.Length} заказов");

            // 8. Сортировка результатов
            Console.WriteLine("\n8. Заказы отсортированные по сумме (убывание):");
            DataRow[] sortedOrders = orders.Select("", "Amount DESC");
            Console.WriteLine("   Топ-3 самых дорогих заказов:");
            for (int i = 0; i < Math.Min(3, sortedOrders.Length); i++)
            {
                Console.WriteLine($"   • {sortedOrders[i]["OrderID"]}: {sortedOrders[i]["Amount"]:C} - {sortedOrders[i]["Status"]}");
            }

            // 9. Сравнение с LINQ
            Console.WriteLine("\n9. Сравнение Select() с LINQ:");
            CompareSelectWithLinq(orders);

            // 10. Динамическое построение фильтра
            Console.WriteLine("\n10. Динамическое построение фильтра:");
            DemoDynamicFilterBuilder(orders);

            // 11. Вывод производительности
            Console.WriteLine("\n11. Производительность для большого числа записей:");
            MeasureSelectPerformance(orders);
        }

        static DataTable CreateOrdersTable()
        {
            DataTable table = new DataTable("Заказы");

            table.Columns.Add("OrderID", typeof(int));
            table.Columns.Add("CustomerID", typeof(int));
            table.Columns.Add("OrderDate", typeof(DateTime));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("ShippingAddress", typeof(string));

            return table;
        }

        static void FillOrdersData(DataTable table, int count)
        {
            string[] statuses = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
            string[] cities = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань" };
            string[] streets = { "ул. Ленина", "ул. Пушкина", "ул. Гагарина", "пр. Мира", "ул. Советская" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2024, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                int daysOffset = rand.Next(0, 365);
                DateTime orderDate = startDate.AddDays(daysOffset);

                string city = cities[rand.Next(cities.Length)];
                string address = $"{city}, {streets[rand.Next(streets.Length)]}, д. {rand.Next(1, 100)}";

                table.Rows.Add(
                    i,                                    // OrderID
                    rand.Next(100, 120),                  // CustomerID
                    orderDate,                            // OrderDate
                    Math.Round((decimal)rand.NextDouble() * 10000, 2), // Amount
                    statuses[rand.Next(statuses.Length)], // Status
                    address                               // ShippingAddress
                );
            }
        }

        static void PrintOrdersSample(DataRow[] orders, int maxCount)
        {
            for (int i = 0; i < Math.Min(maxCount, orders.Length); i++)
            {
                Console.WriteLine($"   • ID: {orders[i]["OrderID"]}, " +
                                $"Дата: {((DateTime)orders[i]["OrderDate"]):dd.MM.yyyy}, " +
                                $"Сумма: {orders[i]["Amount"]:C}, " +
                                $"Статус: {orders[i]["Status"]}");
            }
            if (orders.Length > maxCount)
            {
                Console.WriteLine($"   ... и ещё {orders.Length - maxCount} заказов");
            }
        }

        static void CompareSelectWithLinq(DataTable table)
        {
            // Использование Select()
            Stopwatch swSelect = Stopwatch.StartNew();
            DataRow[] selectResult = table.Select("Status = 'Delivered' AND Amount > 5000");
            swSelect.Stop();

            // Использование LINQ
            Stopwatch swLinq = Stopwatch.StartNew();
            var linqResult = table.AsEnumerable()
                .Where(row => row.Field<string>("Status") == "Delivered" &&
                             row.Field<decimal>("Amount") > 5000)
                .ToArray();
            swLinq.Stop();

            Console.WriteLine($"{"Метод",-20} {"Время (мс)",-15} {"Результатов",-15}");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"{"DataTable.Select()",-20} {swSelect.ElapsedMilliseconds,-15} {selectResult.Length,-15}");
            Console.WriteLine($"{"LINQ",-20} {swLinq.ElapsedMilliseconds,-15} {linqResult.Length,-15}");
        }

        static void DemoDynamicFilterBuilder(DataTable table)
        {
            // Пример динамического фильтра
            var filterParams = new Dictionary<string, object>
            {
                { "Status", "Delivered" },
                { "MinAmount", 1000m },
                { "MaxAmount", 5000m }
            };

            string filter = BuildDynamicFilter(filterParams);
            Console.WriteLine($"   Сгенерированный фильтр: {filter}");

            DataRow[] results = table.Select(filter);
            Console.WriteLine($"   Найдено заказов: {results.Length}");
        }

        static string BuildDynamicFilter(Dictionary<string, object> parameters)
        {
            var conditions = new List<string>();

            foreach (var param in parameters)
            {
                switch (param.Key)
                {
                    case "Status":
                        conditions.Add($"Status = '{param.Value}'");
                        break;
                    case "MinAmount":
                        conditions.Add($"Amount >= {param.Value}");
                        break;
                    case "MaxAmount":
                        conditions.Add($"Amount <= {param.Value}");
                        break;
                    case "CustomerID":
                        conditions.Add($"CustomerID = {param.Value}");
                        break;
                }
            }

            return string.Join(" AND ", conditions);
        }

        static void MeasureSelectPerformance(DataTable table)
        {
            int iterations = 1000;
            Random rand = new Random();

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                decimal minAmount = rand.Next(1000, 5000);
                string status = rand.NextDouble() > 0.5 ? "Delivered" : "Shipped";
                DataRow[] results = table.Select($"Amount > {minAmount} AND Status = '{status}'");
            }
            sw.Stop();

            Console.WriteLine($"   {iterations} операций Select() выполнены за {sw.ElapsedMilliseconds} мс");
            Console.WriteLine($"   Среднее время: {(double)sw.ElapsedMilliseconds / iterations:F2} мс на операцию");
        }
        #endregion

        #region Задание 4: Создание DataView
        static void Task4_CreateDataView()
        {
            Console.WriteLine("ЗАДАНИЕ 4: Создание объекта DataView различными способами\n");
            Console.WriteLine("Цель: Демонстрация различных способов создания DataView\n");

            // 1. Создание таблицы студентов
            DataTable students = CreateStudentsTable();
            FillStudentsData(students, 100);

            Console.WriteLine("1. Создана таблица студентов (100 записей)");

            // 2. Способ 1: new DataView(table)
            Console.WriteLine("\n2. Способ 1: new DataView(table)");
            DataView view1 = new DataView(students);
            Console.WriteLine($"   Количество строк: {view1.Count}");
            Console.WriteLine($"   Фильтр: {view1.RowFilter ?? "(нет)"}");
            Console.WriteLine($"   Сортировка: {view1.Sort ?? "(нет)"}");
            Console.WriteLine("   Первые 3 студента:");
            for (int i = 0; i < Math.Min(3, view1.Count); i++)
            {
                Console.WriteLine($"   • {view1[i]["Name"]} - GPA: {view1[i]["GPA"]}");
            }

            // 3. Способ 2: new DataView(table, filter, sort, rowState)
            Console.WriteLine("\n3. Способ 2: new DataView(table, filter, sort, rowState)");
            DataView view2 = new DataView(students, "GPA > 3.5", "Name ASC", DataViewRowState.CurrentRows);
            Console.WriteLine($"   Количество строк: {view2.Count}");
            Console.WriteLine($"   Фильтр: {view2.RowFilter}");
            Console.WriteLine($"   Сортировка: {view2.Sort}");
            Console.WriteLine("   Первые 3 студента (GPA > 3.5, отсортированы по имени):");
            for (int i = 0; i < Math.Min(3, view2.Count); i++)
            {
                Console.WriteLine($"   • {view2[i]["Name"]} - GPA: {view2[i]["GPA"]}, {view2[i]["Speciality"]}");
            }

            // 4. Способ 3: Через таблицу.DefaultView
            Console.WriteLine("\n4. Способ 3: table.DefaultView");
            DataView view3 = students.DefaultView;
            Console.WriteLine($"   Количество строк: {view3.Count}");
            Console.WriteLine("   Изменение DefaultView влияет на все связанные представления");

            // 5. Способ 4: Создание и настройка после
            Console.WriteLine("\n5. Способ 4: Создание с последующей настройкой");
            DataView view4 = new DataView(students);
            view4.RowFilter = "EnrollmentYear >= 2021";
            view4.Sort = "GPA DESC";
            Console.WriteLine($"   Количество строк: {view4.Count}");
            Console.WriteLine($"   Фильтр: {view4.RowFilter}");
            Console.WriteLine($"   Сортировка: {view4.Sort}");
            Console.WriteLine("   Топ-3 студента (зачислены с 2021, по GPA):");
            for (int i = 0; i < Math.Min(3, view4.Count); i++)
            {
                Console.WriteLine($"   • {view4[i]["Name"]} - GPA: {view4[i]["GPA"]}, {view4[i]["EnrollmentYear"]}");
            }

            // 6. Сравнение производительности
            Console.WriteLine("\n6. Сравнение производительности:");
            CompareDataViewCreationMethods(students);
        }

        static DataTable CreateStudentsTable()
        {
            DataTable table = new DataTable("Студенты");

            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("GPA", typeof(double));
            table.Columns.Add("Speciality", typeof(string));
            table.Columns.Add("EnrollmentYear", typeof(int));

            return table;
        }

        static void FillStudentsData(DataTable table, int count)
        {
            string[] specialities = { "Информатика", "Математика", "Физика", "Химия", "Биология", "Экономика", "Юриспруденция" };
            string[] firstNames = { "Александр", "Мария", "Дмитрий", "Екатерина", "Михаил", "Анна", "Сергей", "Ольга",
                                   "Андрей", "Наталья", "Павел", "Ирина", "Владимир", "Светлана", "Николай" };
            string[] lastNames = { "Смирнов", "Иванов", "Кузнецов", "Попов", "Соколов", "Лебедев", "Козлов",
                                  "Новиков", "Морозов", "Петров", "Волков", "Соловьев", "Васильев", "Зайцев" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];
                double gpa = Math.Round(2.0 + rand.NextDouble() * 2.5, 2);
                string speciality = specialities[rand.Next(specialities.Length)];
                int year = rand.Next(2018, 2024);

                table.Rows.Add(
                    i,
                    $"{firstName} {lastName}",
                    gpa,
                    speciality,
                    year
                );
            }
        }

        static void CompareDataViewCreationMethods(DataTable table)
        {
            int iterations = 10000;

            // Способ 1
            Stopwatch sw1 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                DataView view = new DataView(table);
            }
            sw1.Stop();

            // Способ 2
            Stopwatch sw2 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                DataView view = new DataView(table, "GPA > 3.0", "Name ASC", DataViewRowState.CurrentRows);
            }
            sw2.Stop();

            Console.WriteLine($"{"Способ",-30} {"Время (мс)",-15} {"Операций/сек",-15}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"new DataView(table)",-30} {sw1.ElapsedMilliseconds,-15} {iterations * 1000 / sw1.ElapsedMilliseconds,-15:F0}");
            Console.WriteLine($"{"с фильтром и сортировкой",-30} {sw2.ElapsedMilliseconds,-15} {iterations * 1000 / sw2.ElapsedMilliseconds,-15:F0}");
        }
        #endregion

        #region Задание 5: Сортировка и фильтрация в DataView
        static void Task5_SortFilterDataView()
        {
            Console.WriteLine("ЗАДАНИЕ 5: Сортировка и фильтрация в DataView\n");
            Console.WriteLine("Цель: Демонстрация комплексной фильтрации и сортировки\n");

            // 1. Создание таблицы продаж
            DataTable sales = CreateSalesTable();
            FillSalesData(sales, 200);

            Console.WriteLine("1. Создана таблица продаж (200 записей)");

            // 2. Создание DataView и базовая фильтрация
            DataView salesView = new DataView(sales);

            Console.WriteLine("\n2. Фильтрация по регионам:");
            salesView.RowFilter = "Region = 'Северный'";
            Console.WriteLine($"   Продажи в Северном регионе: {salesView.Count} записей");
            PrintSalesSample(salesView, 3);

            // 3. Фильтрация по диапазону дат
            Console.WriteLine("\n3. Фильтрация по диапазону дат:");
            salesView.RowFilter = "SalesDate >= '2024-01-01' AND SalesDate <= '2024-03-31'";
            Console.WriteLine($"   Продажи за 1 квартал 2024: {salesView.Count} записей");

            // 4. Фильтрация по минимальной сумме
            Console.WriteLine("\n4. Фильтрация по минимальной сумме продажи:");
            decimal minAmount = 5000;
            salesView.RowFilter = $"(Quantity * Price) > {minAmount}";
            Console.WriteLine($"   Продажи с суммой > {minAmount:C}: {salesView.Count} записей");
            PrintSalesSample(salesView, 3);

            // 5. Комбинированные фильтры
            Console.WriteLine("\n5. Комбинированные фильтры:");
            salesView.RowFilter = "Region = 'Центральный' AND Quantity > 10";
            Console.WriteLine($"   Продажи в Центральном регионе с количеством > 10: {salesView.Count} записей");

            // 6. Сортировка по дате
            Console.WriteLine("\n6. Сортировка по дате (возрастание):");
            salesView.RowFilter = "";
            salesView.Sort = "SalesDate ASC";
            Console.WriteLine("   Первые 3 записи:");
            for (int i = 0; i < Math.Min(3, salesView.Count); i++)
            {
                Console.WriteLine($"   • {((DateTime)salesView[i]["SalesDate"]):dd.MM.yyyy} - {salesView[i]["ProductName"]}");
            }

            // 7. Сортировка по продавцу
            Console.WriteLine("\n7. Сортировка по продавцу:");
            salesView.Sort = "Salesperson ASC";
            Console.WriteLine("   Первые 3 продавца:");
            var uniqueSalespersons = new HashSet<string>();
            for (int i = 0; i < salesView.Count && uniqueSalespersons.Count < 3; i++)
            {
                string salesperson = salesView[i]["Salesperson"].ToString();
                if (uniqueSalespersons.Add(salesperson))
                {
                    Console.WriteLine($"   • {salesperson}");
                }
            }

            // 8. Многоуровневая сортировка
            Console.WriteLine("\n8. Многоуровневая сортировка (Region ASC, SalesDate DESC):");
            salesView.Sort = "Region ASC, SalesDate DESC";
            Console.WriteLine("   Первые 5 записей:");
            for (int i = 0; i < Math.Min(5, salesView.Count); i++)
            {
                Console.WriteLine($"   • {salesView[i]["Region"]} - {((DateTime)salesView[i]["SalesDate"]):dd.MM.yyyy} - {salesView[i]["ProductName"]}");
            }

            // 9. Динамическое изменение сортировки
            Console.WriteLine("\n9. Динамическое изменение сортировки:");
            string[] sortOptions = { "ProductName ASC", "Quantity DESC", "Price ASC", "Salesperson ASC, SalesDate DESC" };
            foreach (string sortOption in sortOptions)
            {
                salesView.Sort = sortOption;
                Console.WriteLine($"   Сортировка: {sortOption}");
                Console.WriteLine($"   Первая запись: {salesView[0]["ProductName"]} - {salesView[0]["Quantity"]} шт.");
            }

            // 10. Сброс фильтров и сортировки
            Console.WriteLine("\n10. Сброс фильтров и сортировки:");
            salesView.RowFilter = "";
            salesView.Sort = "";
            Console.WriteLine($"   Количество записей после сброса: {salesView.Count}");

            // 11. Производительность сортировки
            Console.WriteLine("\n11. Производительность сортировки:");
            MeasureSortPerformance(sales);
        }

        static DataTable CreateSalesTable()
        {
            DataTable table = new DataTable("Продажи");

            table.Columns.Add("SalesID", typeof(int));
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("SalesDate", typeof(DateTime));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Salesperson", typeof(string));
            table.Columns.Add("Region", typeof(string));

            return table;
        }

        static void FillSalesData(DataTable table, int count)
        {
            string[] products = { "Ноутбук", "Смартфон", "Планшет", "Монитор", "Принтер", "Клавиатура", "Мышь", "Наушники" };
            string[] salespersons = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Смирнов", "Попов", "Лебедев", "Козлов" };
            string[] regions = { "Центральный", "Северный", "Южный", "Западный", "Восточный" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2023, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                int daysOffset = rand.Next(0, 730);
                DateTime salesDate = startDate.AddDays(daysOffset);

                table.Rows.Add(
                    i,
                    products[rand.Next(products.Length)],
                    salesDate,
                    rand.Next(1, 100),
                    Math.Round((decimal)rand.NextDouble() * 50000, 2),
                    salespersons[rand.Next(salespersons.Length)],
                    regions[rand.Next(regions.Length)]
                );
            }
        }

        static void PrintSalesSample(DataView view, int maxCount)
        {
            for (int i = 0; i < Math.Min(maxCount, view.Count); i++)
            {
                decimal total = (int)view[i]["Quantity"] * (decimal)view[i]["Price"];
                Console.WriteLine($"   • {view[i]["ProductName"]}: {view[i]["Quantity"]} × {view[i]["Price"]:C} = {total:C}");
            }
            if (view.Count > maxCount)
            {
                Console.WriteLine($"   ... и ещё {view.Count - maxCount} записей");
            }
        }

        static void MeasureSortPerformance(DataTable table)
        {
            DataView view = new DataView(table);

            string[] sortColumns = { "ProductName", "Quantity", "Price", "SalesDate", "Region" };

            Console.WriteLine($"{"Сортировка по",-20} {"Время (мс)",-15} {"Кол-во записей",-15}");
            Console.WriteLine(new string('-', 50));

            foreach (string column in sortColumns)
            {
                Stopwatch sw = Stopwatch.StartNew();
                view.Sort = column + " ASC";
                sw.Stop();
                Console.WriteLine($"{"'" + column + "'",-20} {sw.ElapsedMilliseconds,-15} {view.Count,-15}");
            }
        }
        #endregion

        #region Задание 6: Использование DataViewRowState
        static void Task6_DataViewRowState()
        {
            Console.WriteLine("ЗАДАНИЕ 6: Использование DataViewRowState для фильтрации по состоянию строк\n");
            Console.WriteLine("Цель: Демонстрация работы с DataViewRowState\n");

            // 1. Создание таблицы счетов
            DataTable invoices = CreateInvoicesTable();
            FillInvoicesData(invoices, 10);

            Console.WriteLine("1. Создана таблица счетов (10 записей):");
            PrintInvoicesTable(invoices);

            // 2. Добавление новых счетов
            Console.WriteLine("\n2. Добавление 5 новых счетов:");
            for (int i = 11; i <= 15; i++)
            {
                DataRow newRow = invoices.NewRow();
                newRow["InvoiceID"] = i;
                newRow["CustomerName"] = $"Новый клиент {i}";
                newRow["Amount"] = 1000 + i * 100;
                newRow["Status"] = "New";
                newRow["DueDate"] = DateTime.Now.AddDays(30);
                invoices.Rows.Add(newRow);
            }
            Console.WriteLine($"   Всего счетов после добавления: {invoices.Rows.Count}");

            // 3. Модификация существующих счетов
            Console.WriteLine("\n3. Модификация 3 существующих счетов:");
            invoices.Rows[0]["Status"] = "Modified";
            invoices.Rows[1]["Amount"] = 9999;
            invoices.Rows[2]["CustomerName"] = "Изменённый клиент";
            Console.WriteLine("   Изменены счета ID: 1, 2, 3");

            // 4. Удаление счетов
            Console.WriteLine("\n4. Удаление 2 счетов:");
            invoices.Rows[5].Delete();
            invoices.Rows[6].Delete();
            Console.WriteLine("   Удалены счета ID: 6, 7");

            // 5. DataViewRowState.CurrentRows
            Console.WriteLine("\n5. DataViewRowState.CurrentRows:");
            DataView currentView = new DataView(invoices, "", "", DataViewRowState.CurrentRows);
            Console.WriteLine($"   Количество строк: {currentView.Count}");
            Console.WriteLine("   Текущие счета (без удалённых):");
            for (int i = 0; i < Math.Min(3, currentView.Count); i++)
            {
                Console.WriteLine($"   • {currentView[i]["InvoiceID"]}: {currentView[i]["CustomerName"]} - {currentView[i]["Amount"]:C}");
            }

            // 6. DataViewRowState.OriginalRows
            Console.WriteLine("\n6. DataViewRowState.OriginalRows:");
            DataView originalView = new DataView(invoices, "", "", DataViewRowState.OriginalRows);
            Console.WriteLine($"   Количество строк: {originalView.Count}");

            // 7. DataViewRowState.Added
            Console.WriteLine("\n7. DataViewRowState.Added:");
            DataView addedView = new DataView(invoices, "", "", DataViewRowState.Added);
            Console.WriteLine($"   Количество добавленных: {addedView.Count}");
            if (addedView.Count > 0)
            {
                Console.WriteLine("   Добавленные счета:");
                for (int i = 0; i < Math.Min(3, addedView.Count); i++)
                {
                    Console.WriteLine($"   • {addedView[i]["InvoiceID"]}: {addedView[i]["CustomerName"]}");
                }
            }

            // 8. DataViewRowState.ModifiedCurrent
            Console.WriteLine("\n8. DataViewRowState.ModifiedCurrent:");
            DataView modifiedCurrentView = new DataView(invoices, "", "", DataViewRowState.ModifiedCurrent);
            Console.WriteLine($"   Количество изменённых: {modifiedCurrentView.Count}");
            if (modifiedCurrentView.Count > 0)
            {
                Console.WriteLine("   Изменённые счета (текущие значения):");
                for (int i = 0; i < modifiedCurrentView.Count; i++)
                {
                    Console.WriteLine($"   • {modifiedCurrentView[i]["InvoiceID"]}: {modifiedCurrentView[i]["CustomerName"]} - {modifiedCurrentView[i]["Status"]}");
                }
            }

            // 9. DataViewRowState.Deleted
            Console.WriteLine("\n9. DataViewRowState.Deleted:");
            DataView deletedView = new DataView(invoices, "", "", DataViewRowState.Deleted);
            Console.WriteLine($"   Количество удалённых: {deletedView.Count}");
            if (deletedView.Count > 0)
            {
                Console.WriteLine("   Удалённые счета:");
                for (int i = 0; i < deletedView.Count; i++)
                {
                    Console.WriteLine($"   • {deletedView[i]["InvoiceID"]}: {deletedView[i]["CustomerName"]}");
                }
            }

            // 10. Комбинированные состояния
            Console.WriteLine("\n10. Комбинированные состояния (Added | Modified):");
            DataView addedModifiedView = new DataView(invoices, "", "", DataViewRowState.Added | DataViewRowState.ModifiedCurrent);
            Console.WriteLine($"   Количество добавленных или изменённых: {addedModifiedView.Count}");

            // 11. Отчёт об изменениях
            Console.WriteLine("\n11. Отчёт об изменениях перед AcceptChanges():");
            PrintChangesReport(invoices);

            // 12. Откат изменений
            Console.WriteLine("\n12. Откат изменений (RejectChanges):");
            invoices.RejectChanges();
            Console.WriteLine($"   Количество счетов после отката: {invoices.Rows.Count}");
        }

        static DataTable CreateInvoicesTable()
        {
            DataTable table = new DataTable("Счета");

            table.Columns.Add("InvoiceID", typeof(int));
            table.Columns.Add("CustomerName", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("DueDate", typeof(DateTime));

            return table;
        }

        static void FillInvoicesData(DataTable table, int count)
        {
            string[] customers = { "ООО Ромашка", "ИП Сидоров", "ЗАЯ Весна", "ООО ТехноПро", "АО Северсталь" };
            string[] statuses = { "Pending", "Paid", "Overdue", "Cancelled" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                table.Rows.Add(
                    i,
                    customers[rand.Next(customers.Length)],
                    Math.Round((decimal)rand.NextDouble() * 10000, 2),
                    statuses[rand.Next(statuses.Length)],
                    DateTime.Now.AddDays(rand.Next(-30, 60))
                );
            }
        }

        static void PrintInvoicesTable(DataTable table)
        {
            Console.WriteLine($"{"ID",-5} {"Клиент",-20} {"Сумма",-10} {"Статус",-10} {"Срок",-12}");
            Console.WriteLine(new string('-', 60));

            for (int i = 0; i < Math.Min(5, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["InvoiceID"],-5} " +
                                $"{table.Rows[i]["CustomerName"],-20} " +
                                $"{table.Rows[i]["Amount"],10:C} " +
                                $"{table.Rows[i]["Status"],-10} " +
                                $"{((DateTime)table.Rows[i]["DueDate"]):dd.MM.yyyy}");
            }
        }

        static void PrintChangesReport(DataTable table)
        {
            Console.WriteLine($"{"ID",-5} {"Состояние",-15} {"Текущее значение",-30}");
            Console.WriteLine(new string('-', 50));

            foreach (DataRow row in table.Rows)
            {
                string state = GetRowStateDescription(row.RowState);
                Console.WriteLine($"{row["InvoiceID"],-5} {state,-15} {row["CustomerName"],-30}");
            }
        }

        static string GetRowStateDescription(DataRowState state)
        {
            switch (state)
            {
                case DataRowState.Unchanged: return "Не изменён";
                case DataRowState.Added: return "Добавлен";
                case DataRowState.Modified: return "Изменён";
                case DataRowState.Deleted: return "Удалён";
                case DataRowState.Detached: return "Отсоединён";
                default: return "Неизвестно";
            }
        }
        #endregion

        #region Задание 7: Поиск в DataView с использованием Find()
        static void Task7_FindInDataView()
        {
            Console.WriteLine("ЗАДАНИЕ 7: Поиск данных в DataView с использованием метода Find()\n");
            Console.WriteLine("Цель: Демонстрация поиска в DataView\n");

            // 1. Создание таблицы книг
            DataTable books = CreateBooksTable();
            books.PrimaryKey = new DataColumn[] { books.Columns["BookID"] };
            FillBooksData(books, 50);

            Console.WriteLine("1. Создана таблица книг (50 записей)");
            Console.WriteLine($"   Первичный ключ: BookID");

            // 2. Создание DataView с фильтром по категории
            Console.WriteLine("\n2. Создание DataView с фильтром по категории:");
            DataView programmingBooks = new DataView(books);
            programmingBooks.RowFilter = "Category = 'Программирование'";
            programmingBooks.Sort = "BookID ASC";
            Console.WriteLine($"   Книг по программированию: {programmingBooks.Count}");

            // 3. Поиск книги по ISBN в DataTable
            Console.WriteLine("\n3. Поиск книги по ISBN в DataTable:");
            string searchISBN = "978-5-8459-1959-1";
            DataRow[] foundRows = books.Select($"ISBN = '{searchISBN}'");
            if (foundRows.Length > 0)
            {
                Console.WriteLine($"   Найдена книга: {foundRows[0]["Title"]} - {foundRows[0]["Author"]}");
            }
            else
            {
                Console.WriteLine($"   Книга с ISBN {searchISBN} не найдена");
            }

            // 4. Поиск в DataView с использованием Find()
            Console.WriteLine("\n4. Поиск в DataView с использованием Find():");
            programmingBooks.Sort = "BookID ASC";
            int bookIdToFind = 15;
            int index = programmingBooks.Find(bookIdToFind);
            if (index >= 0)
            {
                Console.WriteLine($"   Книга с ID {bookIdToFind} найдена на позиции {index}");
                Console.WriteLine($"   Название: {programmingBooks[index]["Title"]}");
                Console.WriteLine($"   Автор: {programmingBooks[index]["Author"]}");
            }
            else
            {
                Console.WriteLine($"   Книга с ID {bookIdToFind} не найдена в DataView");
            }

            // 5. Сравнение производительности Find() в DataTable vs DataView
            Console.WriteLine("\n5. Сравнение производительности:");
            CompareFindInTableVsView(books);

            // 6. Поиск по нескольким критериям
            Console.WriteLine("\n6. Поиск по автору и году:");
            DataView searchView = new DataView(books);
            searchView.RowFilter = "Author LIKE '%Кнут%' AND Year > 1990";
            Console.WriteLine($"   Найдено книг: {searchView.Count}");
            if (searchView.Count > 0)
            {
                for (int i = 0; i < Math.Min(3, searchView.Count); i++)
                {
                    Console.WriteLine($"   • {searchView[i]["Title"]} ({searchView[i]["Year"]})");
                }
            }

            // 7. Поиск с использованием BinarySearch()
            Console.WriteLine("\n7. Поиск с использованием BinarySearch() в отсортированном DataView:");
            DataView sortedByYear = new DataView(books);
            sortedByYear.Sort = "Year ASC";

            // Создаем массив для BinarySearch
            int[] years = new int[sortedByYear.Count];
            for (int i = 0; i < sortedByYear.Count; i++)
            {
                years[i] = (int)sortedByYear[i]["Year"];
            }

            int yearToFind = 2000;
            int yearIndex = Array.BinarySearch(years, yearToFind);
            if (yearIndex >= 0)
            {
                Console.WriteLine($"   Книга {yearToFind} года найдена на позиции {yearIndex}");
                Console.WriteLine($"   {sortedByYear[yearIndex]["Title"]} - {sortedByYear[yearIndex]["Author"]}");
            }

            // 8. Обработка случаев, когда книга не найдена
            Console.WriteLine("\n8. Обработка случаев, когда книга не найдена:");
            int nonExistentId = 999;
            int notFoundIndex = programmingBooks.Find(nonExistentId);
            if (notFoundIndex == -1)
            {
                Console.WriteLine($"   Правильно: книга с ID {nonExistentId} не найдена (вернулся -1)");
            }

            // 9. Поиск с разными сортировками
            Console.WriteLine("\n9. Поиск с разными сортировками:");
            Console.WriteLine("   а) Сортировка по названию:");
            DataView sortedByTitle = new DataView(books);
            sortedByTitle.Sort = "Title ASC";
            int titleIndex = sortedByTitle.Find("Программирование на C#");
            if (titleIndex >= 0)
            {
                Console.WriteLine($"      Найдена на позиции {titleIndex}");
            }
        }

        static DataTable CreateBooksTable()
        {
            DataTable table = new DataTable("Книги");

            table.Columns.Add("BookID", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Author", typeof(string));
            table.Columns.Add("ISBN", typeof(string));
            table.Columns.Add("Year", typeof(int));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Category", typeof(string));

            return table;
        }

        static void FillBooksData(DataTable table, int count)
        {
            string[] categories = { "Программирование", "Базы данных", "Алгоритмы", "Математика", "Физика" };
            string[] programmingBooks = { "Программирование на C#", "Язык программирования Python", "Искусство программирования",
                                         "Совершенный код", "Паттерны проектирования", "Чистый код", "Рефакторинг" };
            string[] authors = { "Роберт Мартин", "Дональд Кнут", "Стив Макконнелл", "Эрик Фримен", "Кэти Сьерра",
                                "Берт Бейтс", "Эндрю Таненбаум", "Брайан Керниган" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                string category = categories[rand.Next(categories.Length)];
                string title = "";
                string author = "";

                if (category == "Программирование")
                {
                    title = programmingBooks[rand.Next(programmingBooks.Length)];
                    author = authors[rand.Next(authors.Length)];
                }
                else
                {
                    title = $"Книга {i} по {category}";
                    author = $"Автор {i}";
                }

                table.Rows.Add(
                    i,
                    title,
                    author,
                    $"978-5-8459-{1950 + rand.Next(100):0000}-{rand.Next(10)}",
                    1990 + rand.Next(30),
                    Math.Round((decimal)rand.NextDouble() * 5000, 2),
                    category
                );
            }
        }

        static void CompareFindInTableVsView(DataTable table)
        {
            int iterations = 10000;
            Random rand = new Random();

            // Find в DataTable
            Stopwatch swTableFind = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                int id = rand.Next(1, 50);
                DataRow row = table.Rows.Find(id);
            }
            swTableFind.Stop();

            // Find в DataView
            DataView view = new DataView(table);
            view.Sort = "BookID ASC";

            Stopwatch swViewFind = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                int id = rand.Next(1, 50);
                int index = view.Find(id);
            }
            swViewFind.Stop();

            Console.WriteLine($"{"Метод",-25} {"Время (мс)",-15} {"Операций/сек",-15}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"DataTable.Find()",-25} {swTableFind.ElapsedMilliseconds,-15} {iterations * 1000 / swTableFind.ElapsedMilliseconds,-15:F0}");
            Console.WriteLine($"{"DataView.Find()",-25} {swViewFind.ElapsedMilliseconds,-15} {iterations * 1000 / swViewFind.ElapsedMilliseconds,-15:F0}");
        }
        #endregion

        #region Задание 8: Добавление данных через DataView
        static void Task8_AddThroughDataView()
        {
            Console.WriteLine("ЗАДАНИЕ 8: Добавление данных через DataView\n");
            Console.WriteLine("Цель: Демонстрация добавления данных через DataView\n");

            // 1. Создание таблицы клиентов
            DataTable customers = CreateCustomersTable();
            FillCustomersData(customers, 20);

            Console.WriteLine("1. Создана таблица клиентов (20 записей):");
            PrintCustomersTable(customers, 5);

            // 2. Создание DataView с фильтром по статусу
            DataView activeCustomersView = new DataView(customers);
            activeCustomersView.RowFilter = "Status = 'Active'";

            Console.WriteLine("\n2. DataView с фильтром Status = 'Active':");
            Console.WriteLine($"   Активных клиентов: {activeCustomersView.Count}");

            // 3. Добавление через DataView.AddNew()
            Console.WriteLine("\n3. Добавление нового клиента через DataView.AddNew():");
            try
            {
                DataRowView newRowView = activeCustomersView.AddNew();
                newRowView["CustomerID"] = 999;
                newRowView["Name"] = "Новый клиент через DataView";
                newRowView["Email"] = "new.view@example.com";
                newRowView["Phone"] = "+7 (999) 123-45-67";
                newRowView["City"] = "Москва";
                newRowView["Status"] = "Active";
                newRowView.EndEdit();

                Console.WriteLine("   ✓ Клиент успешно добавлен через DataView");
                Console.WriteLine($"   Теперь активных клиентов: {activeCustomersView.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Ошибка при добавлении: {ex.Message}");
            }

            // 4. Добавление через исходную таблицу
            Console.WriteLine("\n4. Добавление клиента через исходную таблицу:");
            DataRow newTableRow = customers.NewRow();
            newTableRow["CustomerID"] = 1000;
            newTableRow["Name"] = "Новый клиент через таблицу";
            newTableRow["Email"] = "new.table@example.com";
            newTableRow["Phone"] = "+7 (999) 987-65-43";
            newTableRow["City"] = "Санкт-Петербург";
            newTableRow["Status"] = "Active";
            customers.Rows.Add(newTableRow);

            Console.WriteLine("   ✓ Клиент успешно добавлен через таблицу");
            Console.WriteLine($"   Активных клиентов в DataView: {activeCustomersView.Count}");

            // 5. Добавление клиента с нарушением фильтра
            Console.WriteLine("\n5. Добавление клиента с Status = 'Inactive':");
            DataRow inactiveRow = customers.NewRow();
            inactiveRow["CustomerID"] = 1001;
            inactiveRow["Name"] = "Неактивный клиент";
            inactiveRow["Email"] = "inactive@example.com";
            inactiveRow["Phone"] = "+7 (999) 111-11-11";
            inactiveRow["City"] = "Казань";
            inactiveRow["Status"] = "Inactive";
            customers.Rows.Add(inactiveRow);

            Console.WriteLine($"   Всего клиентов в таблице: {customers.Rows.Count}");
            Console.WriteLine($"   Активных клиентов в DataView: {activeCustomersView.Count}");
            Console.WriteLine("   Неактивный клиент не отображается в DataView");

            // 6. Валидация при добавлении
            Console.WriteLine("\n6. Валидация при добавлении:");
            Console.WriteLine("   а) Проверка email:");
            try
            {
                DataRowView invalidRow = activeCustomersView.AddNew();
                invalidRow["CustomerID"] = 1002;
                invalidRow["Name"] = "Клиент с неверным email";
                invalidRow["Email"] = "invalid-email"; // Неверный формат
                invalidRow["Phone"] = "+7 (999) 222-22-22";
                invalidRow["City"] = "Екатеринбург";
                invalidRow["Status"] = "Active";
                invalidRow.EndEdit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      ✗ Ошибка валидации: {ex.Message}");
            }

            Console.WriteLine("   б) Проверка уникальности ID:");
            try
            {
                DataRowView duplicateRow = activeCustomersView.AddNew();
                duplicateRow["CustomerID"] = 1; // Дублирующий ID
                duplicateRow["Name"] = "Дубликат";
                duplicateRow["Email"] = "duplicate@example.com";
                duplicateRow["Phone"] = "+7 (999) 333-33-33";
                duplicateRow["City"] = "Новосибирск";
                duplicateRow["Status"] = "Active";
                duplicateRow.EndEdit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      ✗ Ошибка уникальности: {ex.Message}");
            }

            // 7. Автоматическое обновление ID
            Console.WriteLine("\n7. Автоматическое обновление ID:");
            int maxId = customers.AsEnumerable()
                .Select(row => row.Field<int>("CustomerID"))
                .DefaultIfEmpty(0)
                .Max();

            DataRowView autoIdRow = activeCustomersView.AddNew();
            autoIdRow["CustomerID"] = maxId + 1;
            autoIdRow["Name"] = "Клиент с авто ID";
            autoIdRow["Email"] = $"client{maxId + 1}@example.com";
            autoIdRow["Phone"] = $"+7 (999) {444 + maxId:000}-00-00";
            autoIdRow["City"] = "Владивосток";
            autoIdRow["Status"] = "Active";
            autoIdRow.EndEdit();

            Console.WriteLine($"   Добавлен клиент с автоматическим ID: {maxId + 1}");

            // 8. Влияние на другие DataView
            Console.WriteLine("\n8. Влияние добавления на другие DataView:");
            DataView allCustomersView = new DataView(customers);
            DataView moscowCustomersView = new DataView(customers);
            moscowCustomersView.RowFilter = "City = 'Москва'";

            Console.WriteLine($"   Всех клиентов: {allCustomersView.Count}");
            Console.WriteLine($"   Клиентов в Москве: {moscowCustomersView.Count}");

            // 9. Отображение результатов
            Console.WriteLine("\n9. Итоговые данные:");
            Console.WriteLine("   Последние 5 добавленных клиентов:");
            DataView recentView = new DataView(customers);
            recentView.Sort = "CustomerID DESC";

            for (int i = 0; i < Math.Min(5, recentView.Count); i++)
            {
                Console.WriteLine($"   • {recentView[i]["CustomerID"]}: {recentView[i]["Name"]} - {recentView[i]["Status"]}");
            }
        }

        static DataTable CreateCustomersTable()
        {
            DataTable table = new DataTable("Клиенты");

            table.Columns.Add("CustomerID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Phone", typeof(string));
            table.Columns.Add("City", typeof(string));
            table.Columns.Add("Status", typeof(string));

            return table;
        }

        static void FillCustomersData(DataTable table, int count)
        {
            string[] cities = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань", "Нижний Новгород" };
            string[] firstNames = { "Алексей", "Мария", "Дмитрий", "Елена", "Сергей", "Ольга", "Андрей", "Наталья" };
            string[] lastNames = { "Иванов", "Петров", "Сидоров", "Смирнов", "Кузнецов", "Попов", "Васильев" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];
                string status = rand.NextDouble() > 0.3 ? "Active" : "Inactive";

                table.Rows.Add(
                    i,
                    $"{lastName} {firstName}",
                    $"{firstName.ToLower()}.{lastName.ToLower()}@example.com",
                    $"+7 ({900 + rand.Next(100)}) {rand.Next(100, 1000)}-{rand.Next(10, 100)}-{rand.Next(10, 100)}",
                    cities[rand.Next(cities.Length)],
                    status
                );
            }
        }

        static void PrintCustomersTable(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-5} {"Имя",-20} {"Email",-25} {"Город",-15} {"Статус",-10}");
            Console.WriteLine(new string('-', 80));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["CustomerID"],-5} " +
                                $"{table.Rows[i]["Name"],-20} " +
                                $"{table.Rows[i]["Email"],-25} " +
                                $"{table.Rows[i]["City"],-15} " +
                                $"{table.Rows[i]["Status"],-10}");
            }
        }
        #endregion

        #region Задание 9: Редактирование данных через DataView
        static void Task9_EditThroughDataView()
        {
            Console.WriteLine("ЗАДАНИЕ 9: Редактирование данных через DataView\n");
            Console.WriteLine("Цель: Демонстрация редактирования данных через DataView\n");

            // 1. Создание таблицы сотрудников
            DataTable employees = CreateEmployeesTableForEditing();
            FillEmployeesDataForEditing(employees, 30);

            Console.WriteLine("1. Создана таблица сотрудников (30 записей):");
            PrintEmployeesTable(employees, 5);

            // 2. Создание DataView с фильтром по отделу
            Console.WriteLine("\n2. Создание DataView для IT отдела:");
            DataView itEmployeesView = new DataView(employees);
            itEmployeesView.RowFilter = "Department = 'IT'";
            itEmployeesView.Sort = "EmployeeID ASC";

            Console.WriteLine($"   Сотрудников в IT отделе: {itEmployeesView.Count}");
            PrintEmployeesView(itEmployeesView, 3);

            // 3. Поиск и редактирование конкретного сотрудника
            Console.WriteLine("\n3. Поиск и редактирование сотрудника:");
            int employeeIdToEdit = 1010;
            int index = itEmployeesView.Find(employeeIdToEdit);

            if (index >= 0)
            {
                Console.WriteLine($"   Найден сотрудник ID {employeeIdToEdit}:");
                Console.WriteLine($"   До редактирования: {itEmployeesView[index]["Name"]}, " +
                                $"Зарплата: {itEmployeesView[index]["Salary"]:C}, " +
                                $"Статус: {itEmployeesView[index]["Status"]}");

                // Редактирование через DataView
                itEmployeesView[index]["Salary"] = (decimal)itEmployeesView[index]["Salary"] * 1.15m; // Повышение на 15%
                itEmployeesView[index]["Status"] = "Senior";
                itEmployeesView[index].EndEdit();

                Console.WriteLine($"\n   После редактирования:");
                Console.WriteLine($"   Имя: {itEmployeesView[index]["Name"]}");
                Console.WriteLine($"   Зарплата: {itEmployeesView[index]["Salary"]:C}");
                Console.WriteLine($"   Статус: {itEmployeesView[index]["Status"]}");

                // Проверка, что изменения отразились в исходной таблице
                DataRow originalRow = employees.Rows.Find(employeeIdToEdit);
                Console.WriteLine($"\n   Проверка в исходной таблице:");
                Console.WriteLine($"   Зарплата: {originalRow["Salary"]:C}, Статус: {originalRow["Status"]}");
            }
            else
            {
                Console.WriteLine($"   Сотрудник с ID {employeeIdToEdit} не найден в IT отделе");
            }

            // 4. Валидация при редактировании
            Console.WriteLine("\n4. Валидация при редактировании:");
            Console.WriteLine("   а) Проверка зарплаты > 0:");
            try
            {
                if (itEmployeesView.Count > 0)
                {
                    itEmployeesView[0]["Salary"] = -1000; // Неверное значение
                    itEmployeesView[0].EndEdit();
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"      ✓ Правильно обработано: {ex.Message}");
            }

            Console.WriteLine("   б) Проверка допустимых статусов:");
            try
            {
                if (itEmployeesView.Count > 1)
                {
                    itEmployeesView[1]["Status"] = "InvalidStatus"; // Неверный статус
                    itEmployeesView[1].EndEdit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      ✓ Ошибка валидации: {ex.Message}");
            }

            // 5. Массовое редактирование
            Console.WriteLine("\n5. Массовое редактирование (повышение зарплаты на 10% всем в IT):");
            decimal totalSalaryBefore = 0;
            decimal totalSalaryAfter = 0;

            for (int i = 0; i < itEmployeesView.Count; i++)
            {
                totalSalaryBefore += (decimal)itEmployeesView[i]["Salary"];
                itEmployeesView[i]["Salary"] = (decimal)itEmployeesView[i]["Salary"] * 1.10m;
                itEmployeesView[i].EndEdit();
                totalSalaryAfter += (decimal)itEmployeesView[i]["Salary"];
            }

            Console.WriteLine($"   ФОТ до повышения: {totalSalaryBefore:C}");
            Console.WriteLine($"   ФОТ после повышения: {totalSalaryAfter:C}");
            Console.WriteLine($"   Увеличение на: {totalSalaryAfter - totalSalaryBefore:C}");

            // 6. Редактирование с проверкой RowState
            Console.WriteLine("\n6. Редактирование с проверкой RowState:");
            DataView allEmployeesView = new DataView(employees);
            int editedCount = 0;

            for (int i = 0; i < allEmployeesView.Count; i++)
            {
                if (allEmployeesView[i].Row.RowState == DataRowState.Modified)
                {
                    editedCount++;
                    Console.WriteLine($"   Изменён: {allEmployeesView[i]["Name"]} (ID: {allEmployeesView[i]["EmployeeID"]})");
                }
            }
            Console.WriteLine($"   Всего изменённых записей: {editedCount}");

            // 7. Обработка исключений при редактировании удалённой строки
            Console.WriteLine("\n7. Попытка редактирования удалённой строки:");
            try
            {
                // Удаляем строку, затем пытаемся редактировать
                if (itEmployeesView.Count > 2)
                {
                    DataRow rowToDelete = itEmployeesView[2].Row;
                    rowToDelete.Delete();

                    // Попытка редактирования удалённой строки
                    itEmployeesView[2]["Salary"] = 99999;
                    itEmployeesView[2].EndEdit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✓ Ошибка при редактировании удалённой строки: {ex.Message}");
            }

            // 8. Создание отчёта об изменениях
            Console.WriteLine("\n8. Отчёт об изменениях:");
            CreateEditReport(employees);

            // 9. Влияние изменений на другие DataView
            Console.WriteLine("\n9. Влияние изменений на другие DataView:");
            DataView hrEmployeesView = new DataView(employees);
            hrEmployeesView.RowFilter = "Department = 'HR'";

            Console.WriteLine($"   Сотрудников в IT: {itEmployeesView.Count}");
            Console.WriteLine($"   Сотрудников в HR: {hrEmployeesView.Count}");
            Console.WriteLine($"   Изменения в IT отражаются во всех представлениях");
        }

        static DataTable CreateEmployeesTableForEditing()
        {
            DataTable table = new DataTable("Сотрудники");

            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Salary", typeof(decimal));
            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("HireDate", typeof(DateTime));
            table.Columns.Add("Status", typeof(string));

            // Установка первичного ключа
            table.PrimaryKey = new DataColumn[] { table.Columns["EmployeeID"] };

            return table;
        }

        static void FillEmployeesDataForEditing(DataTable table, int count)
        {
            string[] departments = { "IT", "HR", "Finance", "Sales", "Marketing" };
            string[] statuses = { "Junior", "Middle", "Senior", "Lead", "Manager" };
            string[] firstNames = { "Александр", "Екатерина", "Максим", "Анна", "Дмитрий", "Мария", "Сергей", "Ольга" };
            string[] lastNames = { "Иванов", "Петрова", "Сидоров", "Смирнова", "Кузнецов", "Попова", "Васильев", "Николаева" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];
                string department = departments[rand.Next(departments.Length)];
                string status = statuses[rand.Next(statuses.Length)];

                table.Rows.Add(
                    1000 + i,
                    $"{lastName} {firstName}",
                    Math.Round(30000 + (decimal)(rand.NextDouble() * 120000), 2),
                    department,
                    DateTime.Now.AddDays(-rand.Next(1, 3650)),
                    status
                );
            }
        }

        static void PrintEmployeesTable(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-5} {"Имя",-20} {"Зарплата",-12} {"Отдел",-10} {"Статус",-10}");
            Console.WriteLine(new string('-', 60));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["EmployeeID"],-5} " +
                                $"{table.Rows[i]["Name"],-20} " +
                                $"{table.Rows[i]["Salary"],12:C} " +
                                $"{table.Rows[i]["Department"],-10} " +
                                $"{table.Rows[i]["Status"],-10}");
            }
        }

        static void PrintEmployeesView(DataView view, int maxRows)
        {
            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"   • {view[i]["EmployeeID"]}: {view[i]["Name"]} - {view[i]["Salary"]:C}");
            }
        }

        static void CreateEditReport(DataTable table)
        {
            DataView modifiedView = new DataView(table, "", "", DataViewRowState.ModifiedCurrent);

            Console.WriteLine($"   Изменённых записей: {modifiedView.Count}");

            if (modifiedView.Count > 0)
            {
                Console.WriteLine($"   {"ID",-5} {"Имя",-15} {"Изменения",-30}");
                Console.WriteLine($"   {new string('-', 50)}");

                foreach (DataRowView rowView in modifiedView)
                {
                    // Получаем оригинальные значения через версию данных
                    string changes = "";
                    DataRow row = rowView.Row;

                    if (row.HasVersion(DataRowVersion.Original) && row.HasVersion(DataRowVersion.Current))
                    {
                        var originalSalary = row["Salary", DataRowVersion.Original];
                        var currentSalary = row["Salary", DataRowVersion.Current];

                        var originalStatus = row["Status", DataRowVersion.Original];
                        var currentStatus = row["Status", DataRowVersion.Current];

                        if (!object.Equals(originalSalary, currentSalary))
                        {
                            changes += $"Зарплата: {originalSalary:C} → {currentSalary:C} ";
                        }

                        if (!object.Equals(originalStatus, currentStatus))
                        {
                            changes += $"Статус: {originalStatus} → {currentStatus}";
                        }
                    }

                    Console.WriteLine($"   {rowView["EmployeeID"],-5} {rowView["Name"],-15} {changes,-30}");
                }
            }
        }
        #endregion

        #region Задание 10: Удаление данных через DataView
        static void Task10_DeleteThroughDataView()
        {
            Console.WriteLine("ЗАДАНИЕ 10: Удаление данных через DataView\n");
            Console.WriteLine("Цель: Демонстрация удаления данных через DataView\n");

            // 1. Создание таблицы заказов
            DataTable orders = CreateOrdersTableForDeletion();
            FillOrdersDataForDeletion(orders, 50);

            Console.WriteLine("1. Создана таблица заказов (50 записей):");
            PrintOrdersTable(orders, 5);

            // 2. Создание DataView с фильтром по статусу
            Console.WriteLine("\n2. Создание DataView для заказов со статусом 'Cancelled':");
            DataView cancelledOrdersView = new DataView(orders);
            cancelledOrdersView.RowFilter = "Status = 'Cancelled'";

            Console.WriteLine($"   Отменённых заказов: {cancelledOrdersView.Count}");

            // 3. Удаление конкретного заказа через DataView
            Console.WriteLine("\n3. Удаление конкретного заказа через DataView.Delete():");
            if (cancelledOrdersView.Count > 0)
            {
                int orderIdToDelete = (int)cancelledOrdersView[0]["OrderID"];
                string customerName = (string)cancelledOrdersView[0]["CustomerName"];
                decimal amount = (decimal)cancelledOrdersView[0]["Amount"];

                Console.WriteLine($"   Заказ для удаления:");
                Console.WriteLine($"   ID: {orderIdToDelete}");
                Console.WriteLine($"   Клиент: {customerName}");
                Console.WriteLine($"   Сумма: {amount:C}");

                // Подтверждение удаления
                Console.Write("\n   Подтвердите удаление (да/нет): ");
                string confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "да")
                {
                    cancelledOrdersView.Delete(0);
                    Console.WriteLine("   ✓ Заказ успешно удалён");
                    Console.WriteLine($"   Осталось отменённых заказов: {cancelledOrdersView.Count}");

                    // Проверка отражения в исходной таблице
                    DataRow deletedRow = orders.Rows.Find(orderIdToDelete);
                    if (deletedRow != null && deletedRow.RowState == DataRowState.Deleted)
                    {
                        Console.WriteLine($"   В исходной таблице: строка помечена как удалённая");
                    }
                }
                else
                {
                    Console.WriteLine("   ✗ Удаление отменено");
                }
            }

            // 4. Массовое удаление
            Console.WriteLine("\n4. Массовое удаление всех отменённых заказов:");
            int cancelledCount = cancelledOrdersView.Count;
            Console.WriteLine($"   К заказов для массового удаления: {cancelledCount}");

            if (cancelledCount > 0)
            {
                Console.Write("   Подтвердите массовое удаление (да/нет): ");
                string massConfirmation = Console.ReadLine();

                if (massConfirmation?.ToLower() == "да")
                {
                    // Создаем список ID для удаления (нельзя удалять в цикле по изменяющейся коллекции)
                    List<int> idsToDelete = new List<int>();
                    for (int i = 0; i < cancelledOrdersView.Count; i++)
                    {
                        idsToDelete.Add((int)cancelledOrdersView[i]["OrderID"]);
                    }

                    // Удаляем по ID
                    foreach (int id in idsToDelete)
                    {
                        DataRow row = orders.Rows.Find(id);
                        if (row != null)
                        {
                            row.Delete();
                        }
                    }

                    Console.WriteLine($"   ✓ Удалено {idsToDelete.Count} заказов");
                }
                else
                {
                    Console.WriteLine("   ✗ Массовое удаление отменено");
                }
            }

            // 5. Удаление с подтверждением информации
            Console.WriteLine("\n5. Удаление с выводом информации перед подтверждением:");
            DataView pendingOrdersView = new DataView(orders);
            pendingOrdersView.RowFilter = "Status = 'Pending'";

            if (pendingOrdersView.Count > 0)
            {
                Console.WriteLine("\n   Заказ для удаления:");
                PrintOrderDetails(pendingOrdersView[0]);

                Console.Write("\n   Подтвердите удаление (да/нет): ");
                string confirm = Console.ReadLine();

                if (confirm?.ToLower() == "да")
                {
                    pendingOrdersView.Delete(0);
                    Console.WriteLine("   ✓ Заказ удалён");
                }
            }

            // 6. Функция отката удаления
            Console.WriteLine("\n6. Функция отката удаления (RejectChanges):");
            DataRow[] deletedRows = orders.Select("", "", DataViewRowState.Deleted);
            Console.WriteLine($"   Удалённых заказов (помеченных как удалённые): {deletedRows.Length}");

            if (deletedRows.Length > 0)
            {
                Console.Write("   Откатить последнее удаление? (да/нет): ");
                string rollback = Console.ReadLine();

                if (rollback?.ToLower() == "да")
                {
                    // Находим первую удалённую строку
                    if (deletedRows.Length > 0)
                    {
                        deletedRows[0].RejectChanges();
                        Console.WriteLine($"   ✓ Удаление отменено для заказа ID: {deletedRows[0]["OrderID", DataRowVersion.Original]}");
                    }
                }
            }

            // 7. Обработка исключений
            Console.WriteLine("\n7. Обработка исключений при удалении:");
            Console.WriteLine("   а) Попытка удаления несуществующего заказа:");
            try
            {
                DataRow nonExistentRow = orders.Rows.Find(9999);
                if (nonExistentRow != null)
                {
                    nonExistentRow.Delete();
                }
                else
                {
                    Console.WriteLine("      ✓ Заказ не существует, удаление не требуется");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      ✗ Ошибка: {ex.Message}");
            }

            Console.WriteLine("   б) Нарушение ограничений при удалении:");
            try
            {
                // Создаем связанную таблицу для демонстрации ограничений
                DataTable orderDetails = CreateOrderDetailsTableForDeletion();
                // Здесь могла бы быть проверка внешних ключей
                Console.WriteLine("      (В реальном приложении здесь была бы проверка внешних ключей)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      ✗ Ошибка ограничений: {ex.Message}");
            }

            // 8. Создание отчёта об удалённых заказах
            Console.WriteLine("\n8. Отчёт об удалённых заказах:");
            CreateDeletionReport(orders);

            // 9. Статистика перед AcceptChanges
            Console.WriteLine("\n9. Статистика перед AcceptChanges():");
            PrintDeletionStatistics(orders);

            // 10. Влияние на другие DataView
            Console.WriteLine("\n10. Влияние удаления на другие DataView:");
            DataView allOrdersView = new DataView(orders);
            DataView deliveredOrdersView = new DataView(orders);
            deliveredOrdersView.RowFilter = "Status = 'Delivered'";

            Console.WriteLine($"   Всех заказов: {allOrdersView.Count}");
            Console.WriteLine($"   Доставленных заказов: {deliveredOrdersView.Count}");
            Console.WriteLine($"   Удаление отражается во всех представлениях");
        }

        static DataTable CreateOrdersTableForDeletion()
        {
            DataTable table = new DataTable("Заказы");

            table.Columns.Add("OrderID", typeof(int));
            table.Columns.Add("CustomerID", typeof(int));
            table.Columns.Add("CustomerName", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("CreationDate", typeof(DateTime));

            table.PrimaryKey = new DataColumn[] { table.Columns["OrderID"] };

            return table;
        }

        static DataTable CreateOrderDetailsTableForDeletion()
        {
            DataTable table = new DataTable("Детали заказов");
            table.Columns.Add("DetailID", typeof(int));
            table.Columns.Add("OrderID", typeof(int));
            table.Columns.Add("ProductName", typeof(string));
            return table;
        }

        static void FillOrdersDataForDeletion(DataTable table, int count)
        {
            string[] statuses = { "Pending", "Processing", "Delivered", "Cancelled", "Shipped" };
            string[] customers = { "Иванов Иван", "Петров Петр", "Сидорова Анна", "Кузнецов Дмитрий", "Смирнова Мария" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                table.Rows.Add(
                    i,
                    100 + rand.Next(10),
                    customers[rand.Next(customers.Length)],
                    Math.Round((decimal)rand.NextDouble() * 10000, 2),
                    statuses[rand.Next(statuses.Length)],
                    DateTime.Now.AddDays(-rand.Next(0, 365))
                );
            }
        }

        static void PrintOrdersTable(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-5} {"Клиент",-20} {"Сумма",-12} {"Статус",-12} {"Дата",-12}");
            Console.WriteLine(new string('-', 65));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["OrderID"],-5} " +
                                $"{table.Rows[i]["CustomerName"],-20} " +
                                $"{table.Rows[i]["Amount"],12:C} " +
                                $"{table.Rows[i]["Status"],-12} " +
                                $"{((DateTime)table.Rows[i]["CreationDate"]):dd.MM.yyyy}");
            }
        }

        static void PrintOrderDetails(DataRowView order)
        {
            Console.WriteLine($"   ID заказа: {order["OrderID"]}");
            Console.WriteLine($"   Клиент: {order["CustomerName"]}");
            Console.WriteLine($"   Сумма: {order["Amount"]:C}");
            Console.WriteLine($"   Статус: {order["Status"]}");
            Console.WriteLine($"   Дата создания: {((DateTime)order["CreationDate"]):dd.MM.yyyy}");
        }

        static void CreateDeletionReport(DataTable table)
        {
            DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);

            Console.WriteLine($"   Всего удалённых заказов: {deletedRows.Length}");

            if (deletedRows.Length > 0)
            {
                Console.WriteLine($"   {"ID",-5} {"Клиент",-20} {"Сумма",-12} {"Дата удаления",-20}");
                Console.WriteLine($"   {new string('-', 60)}");

                foreach (DataRow row in deletedRows.Take(5))
                {
                    if (row.HasVersion(DataRowVersion.Original))
                    {
                        Console.WriteLine($"   {row["OrderID", DataRowVersion.Original],-5} " +
                                        $"{row["CustomerName", DataRowVersion.Original],-20} " +
                                        $"{((decimal)row["Amount", DataRowVersion.Original]),12:C} " +
                                        $"{DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    }
                }

                if (deletedRows.Length > 5)
                {
                    Console.WriteLine($"   ... и ещё {deletedRows.Length - 5} заказов");
                }
            }
        }

        static void PrintDeletionStatistics(DataTable table)
        {
            DataRow[] currentRows = table.Select("", "", DataViewRowState.CurrentRows);
            DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);
            DataRow[] modifiedRows = table.Select("", "", DataViewRowState.ModifiedCurrent);

            decimal totalCurrentAmount = currentRows.Sum(row => row.Field<decimal>("Amount"));
            decimal totalDeletedAmount = 0;

            foreach (DataRow row in deletedRows)
            {
                if (row.HasVersion(DataRowVersion.Original))
                {
                    totalDeletedAmount += row.Field<decimal>("Amount", DataRowVersion.Original);
                }
            }

            Console.WriteLine($"   Текущих заказов: {currentRows.Length} на сумму {totalCurrentAmount:C}");
            Console.WriteLine($"   Удалённых заказов: {deletedRows.Length} на сумму {totalDeletedAmount:C}");
            Console.WriteLine($"   Изменённых заказов: {modifiedRows.Length}");
            Console.WriteLine($"   Всего строк в таблице: {table.Rows.Count}");

            if (deletedRows.Length > 0)
            {
                Console.WriteLine($"\n   Внимание! Есть {deletedRows.Length} несохранённых удалений.");
                Console.WriteLine($"   Используйте AcceptChanges() для сохранения или RejectChanges() для отката.");
            }
        }
        #endregion

        #region Задание 11: Создание новой DataTable из DataView
        static void Task11_CreateTableFromView()
        {
            Console.WriteLine("ЗАДАНИЕ 11: Создание новой DataTable из DataView\n");
            Console.WriteLine("Цель: Демонстрация создания таблицы на основе DataView\n");

            // Создание исходной таблицы продуктов
            DataTable products = CreateProductsTableForConversion();
            FillProductsDataForConversion(products, 100);

            Console.WriteLine("1. Исходная таблица продуктов (100 записей):");
            PrintProductsTableSimple(products, 3);

            // Создание DataView с фильтром
            DataView expensiveProductsView = new DataView(products);
            expensiveProductsView.RowFilter = "Price > 5000 AND InStock = true";
            expensiveProductsView.Sort = "Price DESC";

            Console.WriteLine($"\n2. DataView с фильтром (Price > 5000, InStock = true):");
            Console.WriteLine($"   Записей в DataView: {expensiveProductsView.Count}");

            // Способ 1: Использование ToTable()
            Console.WriteLine("\n3. Способ 1: Использование DataView.ToTable():");
            try
            {
                DataTable tableFromView = expensiveProductsView.ToTable();
                Console.WriteLine($"   Создана таблица с {tableFromView.Rows.Count} записями");
                Console.WriteLine($"   Колонки: {string.Join(", ", tableFromView.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ToTable() не доступен: {ex.Message}");
                Console.WriteLine("   Используем альтернативный метод...");
            }

            // Способ 2: Ручное копирование данных
            Console.WriteLine("\n4. Способ 2: Ручное копирование данных:");
            DataTable manualTable = CreateTableFromViewManually(expensiveProductsView);
            Console.WriteLine($"   Создана таблица с {manualTable.Rows.Count} записями");
            PrintProductsTableSimple(manualTable, 3);

            // Способ 3: Копирование только некоторых колонок
            Console.WriteLine("\n5. Способ 3: Копирование только некоторых колонок:");
            DataTable partialTable = CreatePartialTableFromView(expensiveProductsView,
                new string[] { "ProductID", "Name", "Price", "Supplier" });
            Console.WriteLine($"   Создана таблица с {partialTable.Rows.Count} записей и {partialTable.Columns.Count} колонками");
            Console.WriteLine($"   Колонки: {string.Join(", ", partialTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");

            // Способ 4: Копирование с изменением имён колонок
            Console.WriteLine("\n6. Способ 4: Копирование с изменением имён колонок:");
            DataTable renamedTable = CreateTableWithRenamedColumns(expensiveProductsView);
            Console.WriteLine($"   Создана таблица с переименованными колонками:");
            foreach (DataColumn col in renamedTable.Columns)
            {
                Console.WriteLine($"   • {col.ColumnName} ({col.DataType.Name})");
            }

            // Создание таблицы с преобразованием данных
            Console.WriteLine("\n7. Создание таблицы с преобразованием данных:");
            DataTable convertedTable = CreateTableWithConvertedData(expensiveProductsView);
            Console.WriteLine($"   Создана таблица с преобразованными данными (цена в USD):");
            PrintConvertedTable(convertedTable, 3);

            // Создание с добавлением новых рассчитываемых колонок
            Console.WriteLine("\n8. Создание с добавляемыми колонками:");
            DataTable extendedTable = CreateTableWithCalculatedColumns(expensiveProductsView);
            Console.WriteLine($"   Создана таблица с дополнительными колонками:");
            foreach (DataColumn col in extendedTable.Columns)
            {
                Console.WriteLine($"   • {col.ColumnName}");
            }

            // Сравнение исходной и созданной таблиц
            Console.WriteLine("\n9. Сравнение таблиц:");
            CompareTables(products, manualTable);

            // Проверка независимости таблиц
            Console.WriteLine("\n10. Проверка независимости таблиц:");
            TestTableIndependence(products, manualTable);
        }

        static DataTable CreateProductsTableForConversion()
        {
            DataTable table = new DataTable("Продукты");

            table.Columns.Add("ProductID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("InStock", typeof(bool));
            table.Columns.Add("Supplier", typeof(string));

            return table;
        }

        static void FillProductsDataForConversion(DataTable table, int count)
        {
            string[] categories = { "Электроника", "Одежда", "Продукты", "Книги", "Спорт" };
            string[] suppliers = { "Поставщик А", "Поставщик Б", "Поставщик В", "Поставщик Г", "Поставщик Д" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                table.Rows.Add(
                    i,
                    $"Товар {i}",
                    categories[rand.Next(categories.Length)],
                    Math.Round((decimal)rand.NextDouble() * 20000, 2),
                    rand.NextDouble() > 0.3,
                    suppliers[rand.Next(suppliers.Length)]
                );
            }
        }

        static DataTable CreateTableFromViewManually(DataView view)
        {
            // Создаём новую таблицу с такой же структурой
            DataTable newTable = view.Table.Clone();

            // Копируем данные
            foreach (DataRowView rowView in view)
            {
                DataRow newRow = newTable.NewRow();
                foreach (DataColumn column in newTable.Columns)
                {
                    newRow[column.ColumnName] = rowView[column.ColumnName];
                }
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }

        static DataTable CreatePartialTableFromView(DataView view, string[] columnNames)
        {
            DataTable newTable = new DataTable();

            // Добавляем только нужные колонки
            foreach (string colName in columnNames)
            {
                if (view.Table.Columns.Contains(colName))
                {
                    DataColumn originalCol = view.Table.Columns[colName];
                    newTable.Columns.Add(originalCol.ColumnName, originalCol.DataType);
                }
            }

            // Копируем данные
            foreach (DataRowView rowView in view)
            {
                DataRow newRow = newTable.NewRow();
                foreach (DataColumn column in newTable.Columns)
                {
                    newRow[column.ColumnName] = rowView[column.ColumnName];
                }
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }

        static DataTable CreateTableWithRenamedColumns(DataView view)
        {
            DataTable newTable = new DataTable();

            // Добавляем колонки с новыми именами
            Dictionary<string, string> columnMappings = new Dictionary<string, string>
            {
                { "ProductID", "КодТовара" },
                { "Name", "Название" },
                { "Category", "Категория" },
                { "Price", "Цена" },
                { "Supplier", "Поставщик" }
            };

            foreach (var mapping in columnMappings)
            {
                if (view.Table.Columns.Contains(mapping.Key))
                {
                    DataColumn originalCol = view.Table.Columns[mapping.Key];
                    newTable.Columns.Add(mapping.Value, originalCol.DataType);
                }
            }

            // Копируем данные
            foreach (DataRowView rowView in view)
            {
                DataRow newRow = newTable.NewRow();
                foreach (var mapping in columnMappings)
                {
                    if (view.Table.Columns.Contains(mapping.Key))
                    {
                        newRow[mapping.Value] = rowView[mapping.Key];
                    }
                }
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }

        static DataTable CreateTableWithConvertedData(DataView view)
        {
            DataTable newTable = view.Table.Clone();
            // Переименовываем колонку
            newTable.Columns["Price"].ColumnName = "PriceUSD";

            decimal exchangeRate = 0.011m; // Пример курса RUB к USD

            foreach (DataRowView rowView in view)
            {
                DataRow newRow = newTable.NewRow();
                foreach (DataColumn column in newTable.Columns)
                {
                    if (column.ColumnName == "PriceUSD")
                    {
                        // Конвертируем цену в USD
                        decimal priceInRub = (decimal)rowView["Price"];
                        decimal priceInUsd = priceInRub * exchangeRate;
                        newRow["PriceUSD"] = Math.Round(priceInUsd, 2);
                    }
                    else if (view.Table.Columns.Contains(column.ColumnName))
                    {
                        newRow[column.ColumnName] = rowView[column.ColumnName];
                    }
                }
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }

        static DataTable CreateTableWithCalculatedColumns(DataView view)
        {
            // Создаём таблицу с исходными колонками
            DataTable newTable = CreatePartialTableFromView(view,
                new string[] { "ProductID", "Name", "Price", "Category" });

            // Добавляем рассчитываемые колонки
            newTable.Columns.Add("PriceWithVAT", typeof(decimal));
            newTable.Columns.Add("PriceCategory", typeof(string));
            newTable.Columns.Add("IsExpensive", typeof(bool));

            decimal vatRate = 0.20m; // НДС 20%

            // Заполняем рассчитываемые колонки
            for (int i = 0; i < newTable.Rows.Count; i++)
            {
                decimal price = (decimal)newTable.Rows[i]["Price"];
                decimal priceWithVat = price * (1 + vatRate);

                newTable.Rows[i]["PriceWithVAT"] = Math.Round(priceWithVat, 2);
                newTable.Rows[i]["IsExpensive"] = price > 10000;
                newTable.Rows[i]["PriceCategory"] = price switch
                {
                    < 1000 => "Дешёвый",
                    < 5000 => "Средний",
                    < 10000 => "Дорогой",
                    _ => "Очень дорогой"
                };
            }

            return newTable;
        }

        static void PrintProductsTableSimple(DataTable table, int maxRows)
        {
            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"   • {table.Rows[i]["ProductID"]}: {table.Rows[i]["Name"]} - {table.Rows[i]["Price"]:C}");
            }
            if (table.Rows.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {table.Rows.Count - maxRows} товаров");
            }
        }

        static void PrintConvertedTable(DataTable table, int maxRows)
        {
            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"   • {table.Rows[i]["ProductID"]}: {table.Rows[i]["Name"]} - {table.Rows[i]["PriceUSD"]:C} USD");
            }
        }

        static void CompareTables(DataTable originalTable, DataTable newTable)
        {
            Console.WriteLine($"   Исходная таблица: {originalTable.Rows.Count} записей, {originalTable.Columns.Count} колонок");
            Console.WriteLine($"   Новая таблица: {newTable.Rows.Count} записей, {newTable.Columns.Count} колонок");
            Console.WriteLine($"   Совпадающих записей: {CountMatchingRows(originalTable, newTable)}");
        }

        static int CountMatchingRows(DataTable table1, DataTable table2)
        {
            int count = 0;
            if (table1.Columns.Contains("ProductID") && table2.Columns.Contains("ProductID"))
            {
                foreach (DataRow row1 in table1.Rows)
                {
                    int id1 = (int)row1["ProductID"];
                    foreach (DataRow row2 in table2.Rows)
                    {
                        int id2 = (int)row2["ProductID"];
                        if (id1 == id2)
                        {
                            count++;
                            break;
                        }
                    }
                }
            }
            return count;
        }

        static void TestTableIndependence(DataTable originalTable, DataTable newTable)
        {
            Console.WriteLine("   Тест 1: Изменение в новой таблице:");
            if (newTable.Rows.Count > 0)
            {
                decimal oldPrice = (decimal)newTable.Rows[0]["Price"];
                newTable.Rows[0]["Price"] = 99999;
                Console.WriteLine($"      Цена в новой таблице изменена: {oldPrice:C} → 99999:C");

                // Проверяем, что исходная таблица не изменилась
                int productId = (int)newTable.Rows[0]["ProductID"];
                DataRow originalRow = originalTable.Select($"ProductID = {productId}").FirstOrDefault();
                if (originalRow != null && (decimal)originalRow["Price"] != 99999)
                {
                    Console.WriteLine($"      ✓ Исходная таблица не изменилась: цена осталась {originalRow["Price"]:C}");
                }
            }

            Console.WriteLine("   Тест 2: Добавление строки в исходную таблицу:");
            DataRow newRow = originalTable.NewRow();
            newRow["ProductID"] = 99999;
            newRow["Name"] = "Тестовый товар";
            newRow["Price"] = 1000;
            originalTable.Rows.Add(newRow);

            Console.WriteLine($"      В исходную таблицу добавлена 1 строка");
            Console.WriteLine($"      Исходная таблица: {originalTable.Rows.Count} записей");
            Console.WriteLine($"      Новая таблица: {newTable.Rows.Count} записей");
            Console.WriteLine($"      ✓ Новая таблица независима (не изменилась)");
        }
        #endregion

        #region Задание 12: Сочетание Find(), Select() и DataView для комплексного поиска
        static void Task12_CombinedSearch()
        {
            Console.WriteLine("ЗАДАНИЕ 12: Сочетание Find(), Select() и DataView для комплексного поиска\n");
            Console.WriteLine("Цель: Демонстрация совместного использования методов поиска\n");

            // 1. Создание таблицы банковских счетов
            DataTable accounts = CreateBankAccountsTable();
            FillBankAccountsData(accounts, 200);

            Console.WriteLine("1. Создана таблица банковских счетов (200 записей):");
            PrintBankAccountsTable(accounts, 5);

            // 2. Умный поиск с автоматическим выбором метода
            Console.WriteLine("\n2. Умный поиск с автоматическим выбором метода:");

            Console.WriteLine("   а) Поиск по PK (AccountID = 1025) → используем Find():");
            SmartSearch(accounts, "AccountID = 1025");

            Console.WriteLine("\n   б) Поиск с одним критерием (Balance > 50000) → используем Select():");
            SmartSearch(accounts, "Balance > 50000");

            Console.WriteLine("\n   в) Поиск с несколькими критериями (Status = 'Active' AND Balance > 100000) → используем DataView:");
            SmartSearch(accounts, "Status = 'Active' AND Balance > 100000 AND AccountType = 'Savings'");

            Console.WriteLine("\n   г) Поиск по первым буквам имени (LIKE 'Иванов%') → используем Select():");
            SmartSearch(accounts, "CustomerName LIKE 'Иванов%'");

            // 3. Поиск по первичному ключу (Find)
            Console.WriteLine("\n3. Поиск по первичному ключу (Find):");
            int[] accountIds = { 1005, 1015, 1025, 1035 };
            foreach (int id in accountIds)
            {
                DataRow account = accounts.Rows.Find(id);
                if (account != null)
                {
                    Console.WriteLine($"   ✓ Счёт {id}: {account["CustomerName"]} - {account["Balance"]:C}");
                }
            }

            // 4. Поиск с использованием Select()
            Console.WriteLine("\n4. Поиск с использованием Select():");
            Console.WriteLine("   Счета с балансом > 75000 и статусом 'Active':");
            DataRow[] selectedAccounts = accounts.Select("Balance > 75000 AND Status = 'Active'");
            Console.WriteLine($"   Найдено: {selectedAccounts.Length} счетов");
            foreach (DataRow acc in selectedAccounts.Take(3))
            {
                Console.WriteLine($"   • {acc["AccountID"]}: {acc["CustomerName"]} - {acc["Balance"]:C}");
            }

            // 5. Поиск с использованием DataView
            Console.WriteLine("\n5. Поиск с использованием DataView:");
            DataView accountsView = new DataView(accounts);
            accountsView.RowFilter = "AccountType = 'Savings' AND OpenDate >= '2023-01-01'";
            accountsView.Sort = "Balance DESC";

            Console.WriteLine($"   Сберегательные счета с 2023 года: {accountsView.Count}");
            for (int i = 0; i < Math.Min(3, accountsView.Count); i++)
            {
                Console.WriteLine($"   • {accountsView[i]["CustomerName"]}: {accountsView[i]["Balance"]:C} ({accountsView[i]["OpenDate"]:dd.MM.yyyy})");
            }

            // 6. Поиск с автодополнением
            Console.WriteLine("\n6. Поиск с автодополнением:");
            string searchTerm = "Ива";
            Console.WriteLine($"   Поиск клиентов, начинающихся на '{searchTerm}':");
            DataRow[] autoCompleteResults = accounts.Select($"CustomerName LIKE '{searchTerm}%'", "CustomerName ASC");
            Console.WriteLine($"   Найдено: {autoCompleteResults.Length} клиентов");
            foreach (DataRow result in autoCompleteResults.Take(5))
            {
                Console.WriteLine($"   • {result["CustomerName"]} ({result["AccountType"]})");
            }

            // 7. Сравнение производительности
            Console.WriteLine("\n7. Сравнение производительности методов поиска:");
            CompareSearchMethodsPerformance(accounts);

            // 8. Отчёт о найденных счетах
            Console.WriteLine("\n8. Отчёт о найденных счетах:");
            GenerateBankSearchReport(accounts);
        }

        static DataTable CreateBankAccountsTable()
        {
            DataTable table = new DataTable("Банковские счета");

            table.Columns.Add("AccountID", typeof(int));
            table.Columns.Add("CustomerName", typeof(string));
            table.Columns.Add("Balance", typeof(decimal));
            table.Columns.Add("AccountType", typeof(string));
            table.Columns.Add("OpenDate", typeof(DateTime));
            table.Columns.Add("Status", typeof(string));

            table.PrimaryKey = new DataColumn[] { table.Columns["AccountID"] };

            return table;
        }

        static void FillBankAccountsData(DataTable table, int count)
        {
            string[] accountTypes = { "Checking", "Savings", "Deposit", "Investment" };
            string[] statuses = { "Active", "Inactive", "Frozen", "Closed" };
            string[] lastNames = { "Иванов", "Петров", "Сидоров", "Смирнов", "Кузнецов", "Попов",
                          "Васильев", "Николаев", "Алексеев", "Морозов", "Волков", "Зайцев" };
            string[] firstNames = { "Александр", "Иван", "Сергей", "Дмитрий", "Андрей",
                           "Мария", "Елена", "Ольга", "Наталья", "Светлана" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2020, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                string lastName = lastNames[rand.Next(lastNames.Length)];
                string firstName = firstNames[rand.Next(firstNames.Length)];

                table.Rows.Add(
                    1000 + i,                            // AccountID
                    $"{lastName} {firstName}",          // CustomerName
                    Math.Round((decimal)rand.NextDouble() * 200000, 2), // Balance
                    accountTypes[rand.Next(accountTypes.Length)], // AccountType
                    startDate.AddDays(rand.Next(0, 1460)), // OpenDate
                    statuses[rand.Next(statuses.Length)]  // Status
                );
            }
        }

        static void PrintBankAccountsTable(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-6} {"Клиент",-20} {"Баланс",-12} {"Тип",-12} {"Статус",-10}");
            Console.WriteLine(new string('-', 65));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["AccountID"],-6} " +
                                $"{table.Rows[i]["CustomerName"],-20} " +
                                $"{table.Rows[i]["Balance"],12:C} " +
                                $"{table.Rows[i]["AccountType"],-12} " +
                                $"{table.Rows[i]["Status"],-10}");
            }
        }

        static void SmartSearch(DataTable table, string searchCriteria)
        {
            try
            {
                // Анализируем критерий поиска
                if (searchCriteria.StartsWith("AccountID ="))
                {
                    // Поиск по PK → используем Find
                    int accountId = int.Parse(searchCriteria.Split('=')[1].Trim());
                    DataRow result = table.Rows.Find(accountId);

                    if (result != null)
                    {
                        Console.WriteLine($"      Найден через Find(): {result["CustomerName"]} - {result["Balance"]:C}");
                    }
                    else
                    {
                        Console.WriteLine($"      Счёт не найден");
                    }
                }
                else if (searchCriteria.Split(new[] { "AND", "OR" }, StringSplitOptions.RemoveEmptyEntries).Length <= 2)
                {
                    // Простой критерий → используем Select
                    Stopwatch sw = Stopwatch.StartNew();
                    DataRow[] results = table.Select(searchCriteria);
                    sw.Stop();

                    Console.WriteLine($"      Найдено через Select(): {results.Length} записей за {sw.ElapsedMilliseconds} мс");
                    if (results.Length > 0)
                    {
                        Console.WriteLine($"      Пример: {results[0]["CustomerName"]}");
                    }
                }
                else
                {
                    // Сложный критерий → используем DataView
                    Stopwatch sw = Stopwatch.StartNew();
                    DataView view = new DataView(table);
                    view.RowFilter = searchCriteria;
                    sw.Stop();

                    Console.WriteLine($"      Найдено через DataView: {view.Count} записей за {sw.ElapsedMilliseconds} мс");
                    if (view.Count > 0)
                    {
                        Console.WriteLine($"      Пример: {view[0]["CustomerName"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"      Ошибка: {ex.Message}");
            }
        }

        static void CompareSearchMethodsPerformance(DataTable table)
        {
            int iterations = 10000;
            Random rand = new Random();

            Console.WriteLine($"{"Метод",-25} {"Время (мс)",-15} {"Операций",-10}");
            Console.WriteLine(new string('-', 50));

            // Тест Find()
            Stopwatch swFind = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                int id = 1001 + rand.Next(table.Rows.Count);
                DataRow row = table.Rows.Find(id);
            }
            swFind.Stop();
            Console.WriteLine($"{"Find()",-25} {swFind.ElapsedMilliseconds,-15} {iterations,-10}");

            // Тест Select() с простым фильтром
            Stopwatch swSelectSimple = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                decimal minBalance = rand.Next(10000, 50000);
                DataRow[] rows = table.Select($"Balance > {minBalance}");
            }
            swSelectSimple.Stop();
            Console.WriteLine($"{"Select() простой",-25} {swSelectSimple.ElapsedMilliseconds,-15} {iterations,-10}");

            // Тест Select() со сложным фильтром
            Stopwatch swSelectComplex = Stopwatch.StartNew();
            for (int i = 0; i < iterations / 10; i++) // Меньше итераций из-за сложности
            {
                decimal minBalance = rand.Next(10000, 50000);
                string status = rand.NextDouble() > 0.5 ? "Active" : "Inactive";
                DataRow[] rows = table.Select($"Balance > {minBalance} AND Status = '{status}' AND AccountType = 'Savings'");
            }
            swSelectComplex.Stop();
            Console.WriteLine($"{"Select() сложный",-25} {swSelectComplex.ElapsedMilliseconds,-15} {iterations / 10,-10}");

            // Тест DataView
            Stopwatch swViewCreate = Stopwatch.StartNew();
            DataView view = new DataView(table);
            swViewCreate.Stop();

            Stopwatch swViewFilter = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                decimal minBalance = rand.Next(10000, 50000);
                view.RowFilter = $"Balance > {minBalance}";
                int count = view.Count;
            }
            swViewFilter.Stop();

            Console.WriteLine($"{"DataView создание",-25} {swViewCreate.ElapsedMilliseconds,-15} {"1",-10}");
            Console.WriteLine($"{"DataView фильтрация",-25} {swViewFilter.ElapsedMilliseconds,-15} {iterations,-10}");
        }

        static void GenerateBankSearchReport(DataTable table)
        {
            Console.WriteLine("   Статистика по счетам:");

            // Активные счета
            DataView activeAccounts = new DataView(table);
            activeAccounts.RowFilter = "Status = 'Active'";
            decimal totalActiveBalance = 0;
            foreach (DataRowView row in activeAccounts)
            {
                totalActiveBalance += (decimal)row["Balance"];
            }

            // Сберегательные счета
            DataView savingsAccounts = new DataView(table);
            savingsAccounts.RowFilter = "AccountType = 'Savings'";
            decimal totalSavingsBalance = 0;
            foreach (DataRowView row in savingsAccounts)
            {
                totalSavingsBalance += (decimal)row["Balance"];
            }

            // Счета открытые в 2023-2024
            DataView recentAccounts = new DataView(table);
            recentAccounts.RowFilter = "OpenDate >= '2023-01-01'";

            Console.WriteLine($"   • Активных счетов: {activeAccounts.Count} (общий баланс: {totalActiveBalance:C})");
            Console.WriteLine($"   • Сберегательных счетов: {savingsAccounts.Count} (общий баланс: {totalSavingsBalance:C})");
            Console.WriteLine($"   • Счетов с 2023 года: {recentAccounts.Count}");
            Console.WriteLine($"   • Средний баланс: {table.AsEnumerable().Average(row => row.Field<decimal>("Balance")):C}");
        }
        #endregion

        #region Задание 13: DataView с несколькими уровнями фильтрации
        static void Task13_MultiLevelFiltering()
        {
            Console.WriteLine("ЗАДАНИЕ 13: DataView с несколькими уровнями фильтрации\n");
            Console.WriteLine("Цель: Демонстрация многоуровневой фильтрации\n");

            // 1. Создание таблицы событий
            DataTable events = CreateEventsTable();
            FillEventsData(events, 300);

            Console.WriteLine("1. Создана таблица событий (300 записей):");
            PrintEventsTable(events, 5);

            // 2. Создание DataView и многоуровневая фильтрация
            DataView eventsView = new DataView(events);

            Console.WriteLine("\n2. Многоуровневая фильтрация событий:");

            // Уровень 1: Фильтр по категории
            Console.WriteLine("\n   Уровень 1: Фильтр по категории 'Конференция'");
            eventsView.RowFilter = "Category = 'Конференция'";
            Console.WriteLine($"   Подходящих событий: {eventsView.Count}");
            PrintEventsSample(eventsView, 2);

            // Уровень 2: Добавление фильтра по приоритету
            Console.WriteLine("\n   Уровень 2: + Фильтр по приоритету 'High'");
            eventsView.RowFilter = "Category = 'Конференция' AND Priority = 'High'";
            Console.WriteLine($"   Подходящих событий: {eventsView.Count}");
            PrintEventsSample(eventsView, 2);

            // Уровень 3: Добавление фильтра по дате (только будущие события)
            Console.WriteLine("\n   Уровень 3: + Фильтр по дате (будущие события)");
            string futureDateFilter = $"Date >= '{DateTime.Now:yyyy-MM-dd}'";
            eventsView.RowFilter = $"Category = 'Конференция' AND Priority = 'High' AND {futureDateFilter}";
            Console.WriteLine($"   Подходящих событий: {eventsView.Count}");
            PrintEventsSample(eventsView, 2);

            // Уровень 4: Добавление фильтра по количеству участников
            Console.WriteLine("\n   Уровень 4: + Фильтр по участникам (> 50)");
            eventsView.RowFilter = $"Category = 'Конференция' AND Priority = 'High' AND {futureDateFilter} AND Attendees > 50";
            Console.WriteLine($"   Подходящих событий: {eventsView.Count}");

            if (eventsView.Count > 0)
            {
                PrintEventsSample(eventsView, eventsView.Count);
            }
            else
            {
                Console.WriteLine("   Событий не найдено");
            }

            // 3. Управление фильтрами
            Console.WriteLine("\n3. Управление фильтрами:");

            // Сохраняем текущий фильтр
            string currentFilter = eventsView.RowFilter;
            Console.WriteLine($"   Текущий фильтр: {currentFilter}");

            // Удаляем фильтр по дате
            Console.WriteLine("\n   Удаляем фильтр по дате:");
            eventsView.RowFilter = RemoveFilterPart(currentFilter, "Date");
            Console.WriteLine($"   Новый фильтр: {eventsView.RowFilter}");
            Console.WriteLine($"   Подходящих событий: {eventsView.Count}");

            // Удаляем фильтр по приоритету
            Console.WriteLine("\n   Удаляем фильтр по приоритету:");
            eventsView.RowFilter = RemoveFilterPart(eventsView.RowFilter, "Priority");
            Console.WriteLine($"   Новый фильтр: {eventsView.RowFilter}");
            Console.WriteLine($"   Подходящих событий: {eventsView.Count}");

            // 4. Сброс всех фильтров
            Console.WriteLine("\n4. Сброс всех фильтров:");
            eventsView.RowFilter = "";
            Console.WriteLine($"   Фильтр сброшен");
            Console.WriteLine($"   Всего событий: {eventsView.Count}");

            // 5. Другой пример многоуровневой фильтрации
            Console.WriteLine("\n5. Другой пример фильтрации (Workshop + Medium Priority + Past Events):");
            eventsView.RowFilter = "Category = 'Workshop' AND Priority = 'Medium' AND Date < '2024-01-01'";
            Console.WriteLine($"   Подходящих событий: {eventsView.Count}");
            PrintEventsSample(eventsView, 3);

            // 6. Сохранение и загрузка конфигурации фильтров
            Console.WriteLine("\n6. Сохранение и загрузка конфигурации фильтров:");

            // Сохраняем конфигурацию
            FilterConfiguration config = new FilterConfiguration
            {
                Category = "Конференция",
                Priority = "High",
                MinAttendees = 50,
                FutureEventsOnly = true
            };

            string savedFilter = BuildFilterFromConfiguration(config);
            Console.WriteLine($"   Сохранённый фильтр: {savedFilter}");

            // Загружаем конфигурацию
            Console.WriteLine("\n   Загружаем сохранённую конфигурацию:");
            eventsView.RowFilter = savedFilter;
            Console.WriteLine($"   Подходящих событий: {eventsView.Count}");

            // 7. Отчёт о фильтрации
            Console.WriteLine("\n7. Отчёт о фильтрации:");
            GenerateFilteringReport(events);
        }

        static DataTable CreateEventsTable()
        {
            DataTable table = new DataTable("События");

            table.Columns.Add("EventID", typeof(int));
            table.Columns.Add("EventName", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Location", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Priority", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Attendees", typeof(int));

            return table;
        }

        static void FillEventsData(DataTable table, int count)
        {
            string[] categories = { "Конференция", "Workshop", "Семинар", "Тренинг", "Встреча", "Презентация" };
            string[] priorities = { "Low", "Medium", "High", "Critical" };
            string[] statuses = { "Planned", "In Progress", "Completed", "Cancelled" };
            string[] locations = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань", "Онлайн" };
            string[] eventNames = { "IT Conference", "Dev Workshop", "Business Meeting", "Training Session",
                           "Annual Summit", "Tech Talk", "Product Launch", "Team Building" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2022, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                table.Rows.Add(
                    i,
                    $"{eventNames[rand.Next(eventNames.Length)]} {i}",
                    startDate.AddDays(rand.Next(0, 1095)), // 3 года
                    locations[rand.Next(locations.Length)],
                    categories[rand.Next(categories.Length)],
                    priorities[rand.Next(priorities.Length)],
                    statuses[rand.Next(statuses.Length)],
                    rand.Next(10, 500)
                );
            }
        }

        static void PrintEventsTable(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-4} {"Название",-25} {"Дата",-12} {"Категория",-12} {"Приоритет",-10}");
            Console.WriteLine(new string('-', 65));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["EventID"],-4} " +
                                $"{table.Rows[i]["EventName"],-25} " +
                                $"{((DateTime)table.Rows[i]["Date"]):dd.MM.yyyy} " +
                                $"{table.Rows[i]["Category"],-12} " +
                                $"{table.Rows[i]["Priority"],-10}");
            }
        }

        static void PrintEventsSample(DataView view, int maxRows)
        {
            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"   • {view[i]["EventName"]} ({((DateTime)view[i]["Date"]):dd.MM.yyyy}) - {view[i]["Location"]} - {view[i]["Attendees"]} чел.");
            }
            if (view.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {view.Count - maxRows} событий");
            }
        }

        static string RemoveFilterPart(string filter, string partToRemove)
        {
            // Упрощённое удаление части фильтра
            var parts = filter.Split(new[] { "AND" }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(p => p.Trim())
                             .Where(p => !p.Contains(partToRemove))
                             .ToList();

            return string.Join(" AND ", parts);
        }

        class FilterConfiguration
        {
            public string Category { get; set; }
            public string Priority { get; set; }
            public int? MinAttendees { get; set; }
            public bool FutureEventsOnly { get; set; }
            public string Status { get; set; }
            public string Location { get; set; }
        }

        static string BuildFilterFromConfiguration(FilterConfiguration config)
        {
            var conditions = new List<string>();

            if (!string.IsNullOrEmpty(config.Category))
                conditions.Add($"Category = '{config.Category}'");

            if (!string.IsNullOrEmpty(config.Priority))
                conditions.Add($"Priority = '{config.Priority}'");

            if (config.MinAttendees.HasValue)
                conditions.Add($"Attendees > {config.MinAttendees.Value}");

            if (config.FutureEventsOnly)
                conditions.Add($"Date >= '{DateTime.Now:yyyy-MM-dd}'");

            if (!string.IsNullOrEmpty(config.Status))
                conditions.Add($"Status = '{config.Status}'");

            if (!string.IsNullOrEmpty(config.Location))
                conditions.Add($"Location = '{config.Location}'");

            return string.Join(" AND ", conditions);
        }

        static void GenerateFilteringReport(DataTable table)
        {
            DataView view = new DataView(table);

            Console.WriteLine("   Статистика по категориям:");
            string[] categories = { "Конференция", "Workshop", "Семинар", "Тренинг", "Встреча", "Презентация" };

            foreach (string category in categories)
            {
                view.RowFilter = $"Category = '{category}'";
                int count = view.Count;
                int avgAttendees = count > 0 ? (int)view.Cast<DataRowView>().Average(r => (int)r["Attendees"]) : 0;

                Console.WriteLine($"   • {category}: {count} событий, в среднем {avgAttendees} участников");
            }

            Console.WriteLine("\n   Статистика по приоритетам:");
            string[] priorities = { "Low", "Medium", "High", "Critical" };

            foreach (string priority in priorities)
            {
                view.RowFilter = $"Priority = '{priority}'";
                int count = view.Count;

                Console.WriteLine($"   • {priority}: {count} событий");
            }
        }
        #endregion

        #region Задание 14: Сортировка по нескольким колонкам в DataView
        static void Task14_MultiColumnSorting()
        {
            Console.WriteLine("ЗАДАНИЕ 14: Сортировка по нескольким колонкам в DataView\n");
            Console.WriteLine("Цель: Демонстрация многоуровневой сортировки\n");

            // 1. Создание таблицы продаж сотрудников
            DataTable sales = CreateEmployeeSalesTable();
            FillEmployeeSalesData(sales, 500);

            Console.WriteLine("1. Создана таблица продаж сотрудников (500 записей):");
            PrintSalesTable(sales, 5);

            // 2. Создание DataView
            DataView salesView = new DataView(sales);

            // 3. Сортировка по одной колонке
            Console.WriteLine("\n2. Сортировка по одной колонке:");

            Console.WriteLine("   а) По сумме продаж (убывание):");
            salesView.Sort = "Amount DESC";
            Console.WriteLine("   Топ-5 сотрудников:");
            for (int i = 0; i < Math.Min(5, salesView.Count); i++)
            {
                Console.WriteLine($"   • {salesView[i]["EmployeeName"]}: {salesView[i]["Amount"]:C} - {salesView[i]["Region"]}");
            }

            Console.WriteLine("\n   б) По дате (возрастание):");
            salesView.Sort = "Date ASC";
            Console.WriteLine("   Первые 5 продаж:");
            for (int i = 0; i < Math.Min(5, salesView.Count); i++)
            {
                Console.WriteLine($"   • {((DateTime)salesView[i]["Date"]):dd.MM.yyyy}: {salesView[i]["EmployeeName"]} - {salesView[i]["Amount"]:C}");
            }

            // 4. Сортировка по двум колонкам
            Console.WriteLine("\n3. Сортировка по двум колонкам:");

            Console.WriteLine("   Отдел (возрастание), сумма продаж (убывание):");
            salesView.Sort = "Department ASC, Amount DESC";
            Console.WriteLine("   Топ-продавцы по отделам:");

            string currentDepartment = "";
            int displayed = 0;
            for (int i = 0; i < salesView.Count && displayed < 10; i++)
            {
                string department = salesView[i]["Department"].ToString();
                if (department != currentDepartment)
                {
                    Console.WriteLine($"\n   Отдел: {department}");
                    currentDepartment = department;
                }
                Console.WriteLine($"   • {salesView[i]["EmployeeName"]}: {salesView[i]["Amount"]:C}");
                displayed++;
            }

            // 5. Сортировка по трём колонкам
            Console.WriteLine("\n4. Сортировка по трём колонкам:");

            Console.WriteLine("   Регион → отдел → дата (убывание):");
            salesView.Sort = "Region ASC, Department ASC, Date DESC";

            Console.WriteLine("   Последние продажи по регионам и отделам:");
            string currentRegion = "";
            string currentDept = "";
            displayed = 0;

            for (int i = 0; i < salesView.Count && displayed < 8; i++)
            {
                string region = salesView[i]["Region"].ToString();
                string department = salesView[i]["Department"].ToString();

                if (region != currentRegion)
                {
                    Console.WriteLine($"\n   Регион: {region}");
                    currentRegion = region;
                    currentDept = "";
                }

                if (department != currentDept)
                {
                    Console.WriteLine($"   Отдел: {department}");
                    currentDept = department;
                }

                Console.WriteLine($"   • {((DateTime)salesView[i]["Date"]):dd.MM.yyyy}: {salesView[i]["EmployeeName"]} - {salesView[i]["Amount"]:C}");
                displayed++;
            }

            // 6. Динамическая сортировка
            Console.WriteLine("\n5. Динамическая сортировка:");

            // Пользователь выбирает сортировку
            Dictionary<int, (string Display, string Sort)> sortOptions = new Dictionary<int, (string, string)>
    {
        {1, ("По имени сотрудника (A-Z)", "EmployeeName ASC")},
        {2, ("По сумме продаж (убывание)", "Amount DESC")},
        {3, ("По отделу и дате", "Department ASC, Date DESC")},
        {4, ("По региону и сумме", "Region ASC, Amount DESC")},
        {5, ("По кварталу и сотруднику", "Quarter ASC, EmployeeName ASC")}
    };

            Console.WriteLine("   Выберите вариант сортировки:");
            foreach (var option in sortOptions)
            {
                Console.WriteLine($"   {option.Key}. {option.Value.Display}");
            }

            Console.Write("\n   Ваш выбор (1-5): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && sortOptions.ContainsKey(choice))
            {
                salesView.Sort = sortOptions[choice].Sort;
                Console.WriteLine($"\n   Применена сортировка: {sortOptions[choice].Display}");
                Console.WriteLine("   Первые 5 записей:");

                for (int i = 0; i < Math.Min(5, salesView.Count); i++)
                {
                    string info = $"{salesView[i]["EmployeeName"]} - {salesView[i]["Department"]}";
                    if (salesView.Sort.Contains("Amount"))
                        info += $" - {salesView[i]["Amount"]:C}";
                    if (salesView.Sort.Contains("Date"))
                        info += $" - {((DateTime)salesView[i]["Date"]):dd.MM.yyyy}";

                    Console.WriteLine($"   • {info}");
                }
            }

            // 7. Переключение направления сортировки
            Console.WriteLine("\n6. Переключение направления сортировки:");

            string columnToSort = "Amount";
            Console.WriteLine($"   Сортировка по {columnToSort}:");

            // Сначала по возрастанию
            salesView.Sort = $"{columnToSort} ASC";
            Console.WriteLine($"   Возрастание: от {salesView[0][columnToSort]:C} до {salesView[salesView.Count - 1][columnToSort]:C}");

            // Затем по убыванию
            salesView.Sort = $"{columnToSort} DESC";
            Console.WriteLine($"   Убывание: от {salesView[0][columnToSort]:C} до {salesView[salesView.Count - 1][columnToSort]:C}");

            // 8. Сравнение сортировки в DataView и Select()
            Console.WriteLine("\n7. Сравнение сортировки DataView vs Select():");
            CompareSortingMethods(sales);

            // 9. Отображение иерархии сортировки
            Console.WriteLine("\n8. Иерархия сортировки:");
            DisplaySortHierarchy(salesView.Sort);
        }

        static DataTable CreateEmployeeSalesTable()
        {
            DataTable table = new DataTable("Продажи сотрудников");

            table.Columns.Add("SalesID", typeof(int));
            table.Columns.Add("EmployeeName", typeof(string));
            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Quarter", typeof(string));
            table.Columns.Add("Region", typeof(string));

            return table;
        }

        static void FillEmployeeSalesData(DataTable table, int count)
        {
            string[] departments = { "Sales", "Marketing", "IT", "Finance", "HR", "Operations" };
            string[] regions = { "North", "South", "East", "West", "Central" };
            string[] firstNames = { "John", "Jane", "Bob", "Alice", "Mike", "Sarah", "Tom", "Emily" };
            string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Wilson" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2023, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];
                DateTime date = startDate.AddDays(rand.Next(0, 365));
                int quarter = (date.Month - 1) / 3 + 1;

                table.Rows.Add(
                    i,
                    $"{firstName} {lastName}",
                    departments[rand.Next(departments.Length)],
                    Math.Round((decimal)rand.NextDouble() * 10000, 2),
                    date,
                    $"Q{quarter} 2023",
                    regions[rand.Next(regions.Length)]
                );
            }
        }

        static void PrintSalesTable(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-4} {"Сотрудник",-20} {"Отдел",-12} {"Сумма",-10} {"Дата",-12}");
            Console.WriteLine(new string('-', 65));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["SalesID"],-4} " +
                                $"{table.Rows[i]["EmployeeName"],-20} " +
                                $"{table.Rows[i]["Department"],-12} " +
                                $"{table.Rows[i]["Amount"],10:C} " +
                                $"{((DateTime)table.Rows[i]["Date"]):dd.MM.yyyy}");
            }
        }

        static void CompareSortingMethods(DataTable table)
        {
            int iterations = 1000;

            Console.WriteLine($"{"Метод",-25} {"Время (мс)",-15} {"Операций",-10}");
            Console.WriteLine(new string('-', 50));

            // DataView сортировка
            Stopwatch swView = Stopwatch.StartNew();
            DataView view = new DataView(table);
            for (int i = 0; i < iterations; i++)
            {
                view.Sort = i % 2 == 0 ? "Amount DESC" : "Date ASC";
                var first = view[0];
            }
            swView.Stop();
            Console.WriteLine($"{"DataView Sort",-25} {swView.ElapsedMilliseconds,-15} {iterations,-10}");

            // Select() с сортировкой
            Stopwatch swSelect = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                string sort = i % 2 == 0 ? "Amount DESC" : "Date ASC";
                DataRow[] rows = table.Select("", sort);
                var first = rows.FirstOrDefault();
            }
            swSelect.Stop();
            Console.WriteLine($"{"Select() с сортировкой",-25} {swSelect.ElapsedMilliseconds,-15} {iterations,-10}");

            Console.WriteLine($"\n   DataView быстрее в {(double)swSelect.ElapsedMilliseconds / swView.ElapsedMilliseconds:F1} раза");
        }

        static void DisplaySortHierarchy(string sortExpression)
        {
            if (string.IsNullOrEmpty(sortExpression))
            {
                Console.WriteLine("   Сортировка не применена");
                return;
            }

            var sortColumns = sortExpression.Split(',')
                .Select(s => s.Trim())
                .ToList();

            Console.WriteLine("   Уровни сортировки:");
            for (int i = 0; i < sortColumns.Count; i++)
            {
                string direction = sortColumns[i].EndsWith("DESC", StringComparison.OrdinalIgnoreCase) ? "↓" : "↑";
                string column = sortColumns[i].Replace(" DESC", "").Replace(" ASC", "");

                Console.WriteLine($"   {i + 1}. {column} {direction}");
            }
        }
        #endregion

        #region Задание 15: Комбинирование нескольких DataView для анализа данных
        static void Task15_MultipleDataViews()
        {
            Console.WriteLine("ЗАДАНИЕ 15: Комбинирование нескольких DataView для анализа данных\n");
            Console.WriteLine("Цель: Демонстрация работы с несколькими представлениями\n");

            // 1. Создание таблицы студентов
            DataTable students = CreateStudentsTableForMultipleViews();
            FillStudentsDataForMultipleViews(students, 200);

            Console.WriteLine("1. Создана таблица студентов (200 записей):");
            PrintStudentsTableForMultipleViews(students, 5);

            // 2. Создание нескольких DataView
            Console.WriteLine("\n2. Создание нескольких DataView одной таблицы:");

            // DataView 1: Лучшие студенты
            DataView topStudentsView = new DataView(students);
            topStudentsView.RowFilter = "GPA > 4.0";
            topStudentsView.Sort = "GPA DESC";

            // DataView 2: Студенты первого курса
            DataView firstYearView = new DataView(students);
            firstYearView.RowFilter = "Year = 1";
            firstYearView.Sort = "GPA DESC";

            // DataView 3: Студенты в академическом отпуске
            DataView academicLeaveView = new DataView(students);
            academicLeaveView.RowFilter = "Status = 'Академический отпуск'";

            // DataView 4: Все студенты, отсортированные по факультету
            DataView facultyView = new DataView(students);
            facultyView.Sort = "Faculty ASC, GPA DESC";

            // 3. Статистика по каждому DataView
            Console.WriteLine("\n3. Статистика по каждому DataView:");

            Console.WriteLine("   DataView 1: Лучшие студенты (GPA > 4.0)");
            PrintViewStatistics(topStudentsView, "GPA");

            Console.WriteLine("\n   DataView 2: Студенты первого курса");
            PrintViewStatistics(firstYearView, "GPA");

            Console.WriteLine("\n   DataView 3: Студенты в академическом отпуске");
            Console.WriteLine($"   Количество: {academicLeaveView.Count}");

            Console.WriteLine("\n   DataView 4: Все студенты по факультетам");
            PrintFacultyStatistics(facultyView);

            // 4. Поиск студента во всех DataView
            Console.WriteLine("\n4. Поиск студента во всех DataView:");

            int studentIdToFind = 105;
            Console.WriteLine($"   Поиск студента с ID {studentIdToFind}:");

            DataRow student = students.Rows.Find(studentIdToFind);
            if (student != null)
            {
                Console.WriteLine($"   Найден: {student["Name"]} (GPA: {student["GPA"]}, Факультет: {student["Faculty"]})");

                // Проверяем в каждом представлении
                CheckStudentInView(topStudentsView, studentIdToFind, "Лучшие студенты");
                CheckStudentInView(firstYearView, studentIdToFind, "Первый курс");
                CheckStudentInView(academicLeaveView, studentIdToFind, "Академотпуск");
                CheckStudentInView(facultyView, studentIdToFind, "По факультетам");
            }

            // 5. Добавление нового студента
            Console.WriteLine("\n5. Добавление нового студента:");

            DataRow newStudent = students.NewRow();
            newStudent["StudentID"] = 999;
            newStudent["Name"] = "Новый Студент";
            newStudent["GPA"] = 4.5;
            newStudent["Faculty"] = "Информатика";
            newStudent["Year"] = 3;
            newStudent["Status"] = "Активный";
            students.Rows.Add(newStudent);

            Console.WriteLine("   Добавлен новый студент:");
            Console.WriteLine($"   • Имя: {newStudent["Name"]}");
            Console.WriteLine($"   • GPA: {newStudent["GPA"]}");
            Console.WriteLine($"   • Факультет: {newStudent["Faculty"]}");

            Console.WriteLine("\n   Влияние на DataView:");
            Console.WriteLine($"   • Лучшие студенты: было {topStudentsView.Table.Rows.Count - 1}, стало {topStudentsView.Count}");
            Console.WriteLine($"   • По факультетам: было {facultyView.Table.Rows.Count - 1}, стало {facultyView.Count}");

            // 6. Отображение всех представлений
            Console.WriteLine("\n6. Отображение всех представлений:");

            Console.WriteLine("\n   DataView 1: Лучшие студенты (первые 3):");
            PrintStudentsFromView(topStudentsView, 3);

            Console.WriteLine("\n   DataView 2: Студенты первого курса (первые 3):");
            PrintStudentsFromView(firstYearView, 3);

            Console.WriteLine("\n   DataView 3: Студенты в академическом отпуске:");
            PrintStudentsFromView(academicLeaveView, Math.Min(3, academicLeaveView.Count));

            Console.WriteLine("\n   DataView 4: Студенты по факультетам (первые 5):");
            PrintStudentsFromView(facultyView, 5);

            // 7. Анализ пересечений представлений
            Console.WriteLine("\n7. Анализ пересечений представлений:");
            AnalyzeViewIntersections(students, topStudentsView, firstYearView);
        }

        static DataTable CreateStudentsTableForMultipleViews()
        {
            DataTable table = new DataTable("Студенты");

            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("GPA", typeof(double));
            table.Columns.Add("Faculty", typeof(string));
            table.Columns.Add("Year", typeof(int));
            table.Columns.Add("Status", typeof(string));

            table.PrimaryKey = new DataColumn[] { table.Columns["StudentID"] };

            return table;
        }

        static void FillStudentsDataForMultipleViews(DataTable table, int count)
        {
            string[] faculties = { "Информатика", "Математика", "Физика", "Химия", "Биология",
                          "Экономика", "Юриспруденция", "Филология", "История" };
            string[] statuses = { "Активный", "Академический отпуск", "Выпускник", "Отчислен" };
            string[] firstNames = { "Алексей", "Мария", "Дмитрий", "Анна", "Сергей", "Екатерина",
                           "Андрей", "Ольга", "Михаил", "Наталья" };
            string[] lastNames = { "Иванов", "Петров", "Сидоров", "Смирнов", "Кузнецов",
                          "Попов", "Васильев", "Николаев", "Алексеев", "Морозов" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];
                double gpa = Math.Round(2.0 + rand.NextDouble() * 2.8, 2); // от 2.0 до 4.8

                table.Rows.Add(
                    100 + i,                    // StudentID
                    $"{lastName} {firstName}", // Name
                    gpa,                       // GPA
                    faculties[rand.Next(faculties.Length)], // Faculty
                    rand.Next(1, 6),           // Year (1-5)
                    statuses[rand.Next(statuses.Length)]    // Status
                );
            }
        }

        static void PrintStudentsTableForMultipleViews(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-5} {"Имя",-20} {"GPA",-6} {"Факультет",-15} {"Курс",-5} {"Статус",-20}");
            Console.WriteLine(new string('-', 75));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["StudentID"],-5} " +
                                $"{table.Rows[i]["Name"],-20} " +
                                $"{table.Rows[i]["GPA"],6:F2} " +
                                $"{table.Rows[i]["Faculty"],-15} " +
                                $"{table.Rows[i]["Year"],-5} " +
                                $"{table.Rows[i]["Status"],-20}");
            }
        }

        static void PrintViewStatistics(DataView view, string numericColumn)
        {
            Console.WriteLine($"   Количество: {view.Count}");

            if (view.Count > 0)
            {
                double min = double.MaxValue;
                double max = double.MinValue;
                double sum = 0;

                foreach (DataRowView row in view)
                {
                    double value = Convert.ToDouble(row[numericColumn]);
                    min = Math.Min(min, value);
                    max = Math.Max(max, value);
                    sum += value;
                }

                Console.WriteLine($"   Среднее {numericColumn}: {sum / view.Count:F2}");
                Console.WriteLine($"   Минимальное {numericColumn}: {min:F2}");
                Console.WriteLine($"   Максимальное {numericColumn}: {max:F2}");
            }
        }

        static void PrintFacultyStatistics(DataView view)
        {
            var facultyGroups = view.Cast<DataRowView>()
                .GroupBy(r => r["Faculty"].ToString())
                .Select(g => new
                {
                    Faculty = g.Key,
                    Count = g.Count(),
                    AvgGPA = g.Average(r => Convert.ToDouble(r["GPA"]))
                })
                .OrderByDescending(g => g.AvgGPA);

            foreach (var group in facultyGroups)
            {
                Console.WriteLine($"   • {group.Faculty}: {group.Count} студентов, средний GPA: {group.AvgGPA:F2}");
            }
        }

        static void CheckStudentInView(DataView view, int studentId, string viewName)
        {
            // Ищем студента в представлении
            bool found = false;
            for (int i = 0; i < view.Count; i++)
            {
                if ((int)view[i]["StudentID"] == studentId)
                {
                    found = true;
                    break;
                }
            }

            Console.WriteLine($"   • {viewName}: {(found ? "✓ присутствует" : "✗ отсутствует")}");
        }

        static void PrintStudentsFromView(DataView view, int maxRows)
        {
            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"   • {view[i]["Name"]} (ID: {view[i]["StudentID"]}) - GPA: {view[i]["GPA"]}, {view[i]["Faculty"]}");
            }
            if (view.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {view.Count - maxRows} студентов");
            }
        }

        static void MeasureMemoryUsage(DataTable table)
        {
            long memoryBefore = GC.GetTotalMemory(true);

            // Создаем несколько DataView
            List<DataView> views = new List<DataView>();
            for (int i = 0; i < 10; i++)
            {
                DataView view = new DataView(table);
                view.RowFilter = $"GPA > {3.0 + i * 0.2}";
                views.Add(view);
            }

            long memoryAfter = GC.GetTotalMemory(true);
            long memoryUsed = memoryAfter - memoryBefore;

            Console.WriteLine($"   Память до создания DataView: {memoryBefore / 1024} KB");
            Console.WriteLine($"   Память после создания 10 DataView: {memoryAfter / 1024} KB");
            Console.WriteLine($"   Использовано памяти: {memoryUsed / 1024} KB");
            Console.WriteLine($"   В среднем на DataView: {memoryUsed / 10 / 1024} KB");

            // Очищаем
            views.Clear();
            GC.Collect();
        }

        static void AnalyzeViewIntersections(DataTable table, DataView view1, DataView view2)
        {
            // Студенты, которые есть в обоих представлениях
            HashSet<int> view1Ids = new HashSet<int>();
            HashSet<int> view2Ids = new HashSet<int>();

            foreach (DataRowView row in view1)
                view1Ids.Add((int)row["StudentID"]);

            foreach (DataRowView row in view2)
                view2Ids.Add((int)row["StudentID"]);

            var intersection = view1Ids.Intersect(view2Ids);

            Console.WriteLine($"   Студентов в 1-м представлении: {view1Ids.Count}");
            Console.WriteLine($"   Студентов во 2-м представлении: {view2Ids.Count}");
            Console.WriteLine($"   Студентов в обоих представлениях: {intersection.Count()}");

            if (intersection.Any())
            {
                Console.WriteLine("   Примеры студентов в обоих представлениях:");
                foreach (int id in intersection.Take(3))
                {
                    DataRow student = table.Rows.Find(id);
                    if (student != null)
                    {
                        Console.WriteLine($"   • {student["Name"]} (GPA: {student["GPA"]}, Курс: {student["Year"]})");
                    }
                }
            }
        }
        #endregion

        #region Задание 16: Динамическое изменение фильтра и сортировки в реальном времени
        static void Task16_DynamicFiltering()
        {
            Console.WriteLine("ЗАДАНИЕ 16: Динамическое изменение фильтра и сортировки в реальном времени\n");
            Console.WriteLine("Цель: Демонстрация интерактивной фильтрации и сортировки\n");

            // 1. Создание таблицы фильмов
            DataTable movies = CreateMoviesTable();
            FillMoviesData(movies, 300);

            Console.WriteLine("1. Создана таблица фильмов (300 записей)");
            Console.WriteLine("   Нажмите любую клавишу для начала интерактивного режима...");
            Console.ReadKey();

            // 2. Создание DataView
            DataView moviesView = new DataView(movies);
            moviesView.Sort = "Title ASC";

            // 3. Начальные данные
            Console.Clear();
            Console.WriteLine("=== ИНТЕРАКТИВНАЯ ФИЛЬТРАЦИЯ ФИЛЬМОВ ===\n");
            PrintMovies(moviesView, 10);
            Console.WriteLine($"\nВсего фильмов: {moviesView.Count}");

            // 4. Интерактивный режим
            bool exit = false;
            string lastFilter = "";

            while (!exit)
            {
                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine("ВЫБЕРИТЕ ДЕЙСТВИЕ:");
                Console.WriteLine("1. Фильтр по жанру");
                Console.WriteLine("2. Поиск по названию");
                Console.WriteLine("3. Фильтр по рейтингу");
                Console.WriteLine("4. Фильтр по году");
                Console.WriteLine("5. Сортировка");
                Console.WriteLine("6. Сброс фильтров");
                Console.WriteLine("7. Показать текущие фильтры");
                Console.WriteLine("8. Выход");
                Console.Write("\nВаш выбор: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Фильтр по жанру
                        FilterByGenre(moviesView);
                        break;

                    case "2": // Поиск по названию
                        FilterByTitle(moviesView);
                        break;

                    case "3": // Фильтр по рейтингу
                        FilterByRating(moviesView);
                        break;

                    case "4": // Фильтр по году
                        FilterByYear(moviesView);
                        break;

                    case "5": // Сортировка
                        ChangeSorting(moviesView);
                        break;

                    case "6": // Сброс фильтров
                        moviesView.RowFilter = "";
                        moviesView.Sort = "Title ASC";
                        Console.WriteLine("\n✓ Фильтры сброшены");
                        break;

                    case "7": // Показать текущие фильтры
                        Console.WriteLine($"\nТекущий фильтр: {moviesView.RowFilter ?? "(нет)"}");
                        Console.WriteLine($"Текущая сортировка: {moviesView.Sort ?? "(нет)"}");
                        break;

                    case "8": // Выход
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("\n✗ Неверный выбор");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine($"\nНайдено фильмов: {moviesView.Count}");

                    if (moviesView.Count > 0)
                    {
                        Console.WriteLine("\nПервые 10 результатов:");
                        PrintMovies(moviesView, 10);

                        // Сохраняем последний фильтр
                        lastFilter = moviesView.RowFilter;
                    }
                    else
                    {
                        Console.WriteLine("\nФильмы не найдены. Попробуйте изменить критерии поиска.");
                    }
                }
            }

            // 5. Сохранение последнего фильтра
            Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ===");
            Console.WriteLine($"Последний использованный фильтр: {lastFilter ?? "(не использовался)"}");
            Console.WriteLine($"Всего фильмов в таблице: {movies.Rows.Count}");
        }

        static DataTable CreateMoviesTable()
        {
            DataTable table = new DataTable("Фильмы");

            table.Columns.Add("MovieID", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Director", typeof(string));
            table.Columns.Add("Year", typeof(int));
            table.Columns.Add("Genre", typeof(string));
            table.Columns.Add("Rating", typeof(double));
            table.Columns.Add("BoxOffice", typeof(decimal));

            return table;
        }

        static void FillMoviesData(DataTable table, int count)
        {
            string[] genres = { "Драма", "Комедия", "Боевик", "Фантастика", "Ужасы",
                       "Триллер", "Мелодрама", "Детектив", "Приключения", "Анимация" };
            string[] directors = { "Кристофер Нолан", "Стивен Спилберг", "Квентин Тарантино",
                          "Джеймс Кэмерон", "Питер Джексон", "Мартин Скорсезе",
                          "Ридли Скотт", "Тим Бёртон", "Гай Ричи", "Дэвид Финчер" };
            string[] titles = { "Начало", "Побег из Шоушенка", "Крёстный отец", "Тёмный рыцарь",
                       "Форрест Гамп", "Матрица", "Список Шиндлера", "Властелин колец",
                       "Звёздные войны", "Титаник", "Парк Юрского периода", "Гарри Поттер" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                table.Rows.Add(
                    i,
                    $"{titles[rand.Next(titles.Length)]} {i % 10}",
                    directors[rand.Next(directors.Length)],
                    1980 + rand.Next(45), // 1980-2025
                    genres[rand.Next(genres.Length)],
                    Math.Round(3 + rand.NextDouble() * 7, 1), // 3.0-10.0
                    Math.Round((decimal)rand.NextDouble() * 1000000000, 2) // до 1 млрд
                );
            }
        }

        static void PrintMovies(DataView view, int maxRows)
        {
            Console.WriteLine($"{"Название",-25} {"Год",-6} {"Жанр",-12} {"Рейтинг",-8} {"Касса",-15}");
            Console.WriteLine(new string('-', 70));

            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"{view[i]["Title"],-25} " +
                                $"{view[i]["Year"],-6} " +
                                $"{view[i]["Genre"],-12} " +
                                $"{view[i]["Rating"],8:F1} " +
                                $"{((decimal)view[i]["BoxOffice"]):C}");
            }
        }

        static void FilterByGenre(DataView view)
        {
            Console.WriteLine("\n=== ФИЛЬТР ПО ЖАНРУ ===");
            Console.WriteLine("Доступные жанры: Драма, Комедия, Боевик, Фантастика, Ужасы, Триллер, Мелодрама, Детектив, Приключения, Анимация");
            Console.Write("Введите жанр (или нажмите Enter для отмены): ");

            string genre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(genre))
            {
                string currentFilter = view.RowFilter ?? "";
                string newFilter = $"Genre = '{genre}'";

                if (!string.IsNullOrEmpty(currentFilter))
                {
                    newFilter = currentFilter + " AND " + newFilter;
                }

                view.RowFilter = newFilter;
                Console.WriteLine($"\n✓ Применён фильтр: {newFilter}");
            }
        }

        static void FilterByTitle(DataView view)
        {
            Console.WriteLine("\n=== ПОИСК ПО НАЗВАНИЮ ===");
            Console.Write("Введите часть названия (или нажмите Enter для отмены): ");

            string searchTerm = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string currentFilter = view.RowFilter ?? "";
                string newFilter = $"Title LIKE '%{searchTerm}%'";

                if (!string.IsNullOrEmpty(currentFilter))
                {
                    newFilter = currentFilter + " AND " + newFilter;
                }

                view.RowFilter = newFilter;
                Console.WriteLine($"\n✓ Применён фильтр: {newFilter}");
            }
        }

        static void FilterByRating(DataView view)
        {
            Console.WriteLine("\n=== ФИЛЬТР ПО РЕЙТИНГУ ===");
            Console.Write("Минимальный рейтинг (0-10): ");

            if (double.TryParse(Console.ReadLine(), out double minRating))
            {
                string currentFilter = view.RowFilter ?? "";
                string newFilter = $"Rating >= {minRating}";

                if (!string.IsNullOrEmpty(currentFilter))
                {
                    newFilter = currentFilter + " AND " + newFilter;
                }

                view.RowFilter = newFilter;
                Console.WriteLine($"\n✓ Применён фильтр: {newFilter}");
            }
        }

        static void FilterByYear(DataView view)
        {
            Console.WriteLine("\n=== ФИЛЬТР ПО ГОДУ ===");
            Console.Write("Минимальный год: ");
            string minYearInput = Console.ReadLine();

            Console.Write("Максимальный год: ");
            string maxYearInput = Console.ReadLine();

            if (int.TryParse(minYearInput, out int minYear) && int.TryParse(maxYearInput, out int maxYear))
            {
                string currentFilter = view.RowFilter ?? "";
                string newFilter = $"Year >= {minYear} AND Year <= {maxYear}";

                if (!string.IsNullOrEmpty(currentFilter))
                {
                    newFilter = currentFilter + " AND " + newFilter;
                }

                view.RowFilter = newFilter;
                Console.WriteLine($"\n✓ Применён фильтр: {newFilter}");
            }
        }

        static void ChangeSorting(DataView view)
        {
            Console.WriteLine("\n=== СОРТИРОВКА ===");
            Console.WriteLine("1. По названию (A-Z)");
            Console.WriteLine("2. По названию (Z-A)");
            Console.WriteLine("3. По году (новые первыми)");
            Console.WriteLine("4. По году (старые первыми)");
            Console.WriteLine("5. По рейтингу (высокий первым)");
            Console.WriteLine("6. По рейтингу (низкий первым)");
            Console.WriteLine("7. По кассе (большая первая)");
            Console.WriteLine("8. По кассе (маленькая первая)");
            Console.Write("\nВаш выбор: ");

            string choice = Console.ReadLine();
            string sortExpression = "";

            switch (choice)
            {
                case "1": sortExpression = "Title ASC"; break;
                case "2": sortExpression = "Title DESC"; break;
                case "3": sortExpression = "Year DESC"; break;
                case "4": sortExpression = "Year ASC"; break;
                case "5": sortExpression = "Rating DESC"; break;
                case "6": sortExpression = "Rating ASC"; break;
                case "7": sortExpression = "BoxOffice DESC"; break;
                case "8": sortExpression = "BoxOffice ASC"; break;
                default:
                    Console.WriteLine("\n✗ Неверный выбор");
                    return;
            }

            view.Sort = sortExpression;
            Console.WriteLine($"\n✓ Применена сортировка: {sortExpression}");
        }
        #endregion

        #region Задание 17: Использование DataView для создания графиков/диаграмм
        static void Task17_DataViewForCharts()
        {
            Console.WriteLine("ЗАДАНИЕ 17: Использование DataView для создания графиков/диаграмм\n");
            Console.WriteLine("Цель: Демонстрация визуализации данных из DataView\n");

            // 1. Создание таблицы продаж
            DataTable sales = CreateSalesForChartsTable();
            FillSalesForChartsData(sales, 12 * 5); // 5 лет по 12 месяцев

            Console.WriteLine("1. Создана таблица продаж по месяцам (60 записей)");

            // 2. Создание различных DataView
            Console.WriteLine("\n2. Создание DataView для различных представлений:");

            // DataView 1: По регионам
            DataView byRegionView = new DataView(sales);
            byRegionView.RowFilter = "Year = 2024";

            // DataView 2: По продуктам
            DataView byProductView = new DataView(sales);
            byProductView.RowFilter = "Region = 'Северный' AND Year = 2024";
            byProductView.Sort = "Product ASC";

            // DataView 3: По месяцам
            DataView byMonthView = new DataView(sales);
            byMonthView.RowFilter = "Region = 'Центральный' AND Product = 'Ноутбук'";
            byMonthView.Sort = "MonthNumber ASC";

            // 3. Создание графиков
            Console.WriteLine("\n3. Создание графиков и диаграмм:");

            // График 1: Продажи по регионам за 2024
            Console.WriteLine("\n   График 1: Продажи по регионам за 2024 год");
            CreateRegionChart(byRegionView);

            // График 2: Продажи по продуктам в Северном регионе
            Console.WriteLine("\n   График 2: Продажи по продуктам в Северном регионе (2024)");
            CreateProductChart(byProductView);

            // График 3: Продажи ноутбуков в Центральном регионе по месяцам
            Console.WriteLine("\n   График 3: Продажи ноутбуков в Центральном регионе по месяцам");
            CreateMonthlyChart(byMonthView);

            // 4. Интерактивное обновление диаграмм
            Console.WriteLine("\n4. Интерактивное обновление диаграмм:");

            Console.WriteLine("   Изменяем фильтр для графика по регионам:");
            byRegionView.RowFilter = "Year = 2023";
            Console.WriteLine("   Продажи по регионам за 2023 год:");
            CreateRegionChart(byRegionView);

            // 5. Фильтрация диаграмм
            Console.WriteLine("\n5. Фильтрация диаграмм:");
            Console.WriteLine("   Показываем только регионы с продажами > 5,000,000:");
            CreateFilteredRegionChart(byRegionView, 5000000);

            // 6. Вычисление итоговых значений
            Console.WriteLine("\n6. Итоговые значения:");
            CalculateTotals(sales);

            // 7. Создание комплексной визуализации
            Console.WriteLine("\n7. Комплексная визуализация данных:");
            CreateComprehensiveVisualization(sales);
        }

        static DataTable CreateSalesForChartsTable()
        {
            DataTable table = new DataTable("Продажи по месяцам");

            table.Columns.Add("SalesID", typeof(int));
            table.Columns.Add("Month", typeof(string));
            table.Columns.Add("MonthNumber", typeof(int));
            table.Columns.Add("Year", typeof(int));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Region", typeof(string));

            return table;
        }

        static void FillSalesForChartsData(DataTable table, int count)
        {
            string[] months = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                       "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            string[] products = { "Ноутбук", "Смартфон", "Планшет", "Монитор", "Принтер" };
            string[] regions = { "Центральный", "Северный", "Южный", "Западный", "Восточный" };

            Random rand = new Random();
            int year = 2020;
            int monthIndex = 0;

            for (int i = 1; i <= count; i++)
            {
                string month = months[monthIndex];
                int currentYear = year + (monthIndex / 12);

                table.Rows.Add(
                    i,
                    month,
                    monthIndex % 12 + 1,
                    currentYear,
                    products[rand.Next(products.Length)],
                    Math.Round((decimal)(rand.NextDouble() * 5000000 + 1000000), 2), // 1M-6M
                    regions[rand.Next(regions.Length)]
                );

                monthIndex++;
                if (monthIndex % 12 == 0)
                {
                    year++;
                }
            }
        }

        static void CreateRegionChart(DataView view)
        {
            // Группируем по регионам
            var regionGroups = view.Cast<DataRowView>()
                .GroupBy(r => r["Region"].ToString())
                .Select(g => new
                {
                    Region = g.Key,
                    Total = g.Sum(r => (decimal)r["Amount"])
                })
                .OrderByDescending(g => g.Total)
                .ToList();

            Console.WriteLine("   Регион          Сумма продаж     Диаграмма");
            Console.WriteLine(new string('-', 50));

            decimal maxTotal = regionGroups.Max(g => g.Total);

            foreach (var group in regionGroups)
            {
                int barLength = (int)((group.Total / maxTotal) * 30);
                string bar = new string('█', barLength);

                Console.WriteLine($"   {group.Region,-15} {group.Total,15:C}  {bar}");
            }
        }

        static void CreateProductChart(DataView view)
        {
            // Группируем по продуктам
            var productGroups = view.Cast<DataRowView>()
                .GroupBy(r => r["Product"].ToString())
                .Select(g => new
                {
                    Product = g.Key,
                    Total = g.Sum(r => (decimal)r["Amount"])
                })
                .OrderByDescending(g => g.Total)
                .ToList();

            Console.WriteLine("   Продукт         Сумма продаж     % от общего");
            Console.WriteLine(new string('-', 50));

            decimal grandTotal = productGroups.Sum(g => g.Total);

            foreach (var group in productGroups)
            {
                decimal percentage = (group.Total / grandTotal) * 100;

                Console.WriteLine($"   {group.Product,-15} {group.Total,15:C}  {percentage,6:F1}%");
            }

            // Простая круговая диаграмма в ASCII
            Console.WriteLine("\n   Круговая диаграмма:");
            string[] segments = { "███", "░░░", "▒▒▒", "▓▓▓", "●●●" };
            int segmentIndex = 0;

            foreach (var group in productGroups)
            {
                decimal percentage = (group.Total / grandTotal) * 100;
                int segmentsCount = (int)Math.Round(percentage / 5); // 1 сегмент = 5%

                if (segmentsCount > 0)
                {
                    string segment = segments[segmentIndex % segments.Length];
                    Console.WriteLine($"   {segment} {group.Product} ({percentage:F1}%)");
                    segmentIndex++;
                }
            }
        }

        static void CreateMonthlyChart(DataView view)
        {
            // Сортируем по месяцам
            view.Sort = "MonthNumber ASC";

            Console.WriteLine("   Месяц           Продажи          График");
            Console.WriteLine(new string('-', 50));

            decimal maxAmount = 0;
            var monthlyData = new List<(string Month, decimal Amount)>();

            for (int i = 0; i < view.Count; i++)
            {
                decimal amount = (decimal)view[i]["Amount"];
                monthlyData.Add((view[i]["Month"].ToString(), amount));
                maxAmount = Math.Max(maxAmount, amount);
            }

            // Находим масштаб для графика
            int maxBarHeight = 10;

            foreach (var data in monthlyData)
            {
                int barHeight = maxAmount > 0 ? (int)((data.Amount / maxAmount) * maxBarHeight) : 0;

                // Строим вертикальный график
                Console.Write($"   {data.Month,-12} {data.Amount,15:C}  ");

                for (int h = maxBarHeight; h > 0; h--)
                {
                    Console.Write(h <= barHeight ? "█" : " ");
                }

                Console.WriteLine($" {data.Amount:C}");
            }
        }

        static void CreateFilteredRegionChart(DataView view, decimal minAmount)
        {
            var regionGroups = view.Cast<DataRowView>()
                .GroupBy(r => r["Region"].ToString())
                .Select(g => new
                {
                    Region = g.Key,
                    Total = g.Sum(r => (decimal)r["Amount"])
                })
                .Where(g => g.Total > minAmount)
                .OrderByDescending(g => g.Total)
                .ToList();

            if (regionGroups.Any())
            {
                Console.WriteLine("   Регион          Сумма продаж");
                Console.WriteLine(new string('-', 40));

                foreach (var group in regionGroups)
                {
                    Console.WriteLine($"   {group.Region,-15} {group.Total,15:C}");
                }
            }
            else
            {
                Console.WriteLine("   Нет регионов с продажами выше указанного порога");
            }
        }

        static void CalculateTotals(DataTable table)
        {
            DataView allSales = new DataView(table);

            decimal totalSales = 0;
            foreach (DataRowView row in allSales)
            {
                totalSales += (decimal)row["Amount"];
            }

            // Продажи по годам
            var yearlySales = allSales.Cast<DataRowView>()
                .GroupBy(r => (int)r["Year"])
                .Select(g => new
                {
                    Year = g.Key,
                    Total = g.Sum(r => (decimal)r["Amount"])
                })
                .OrderBy(g => g.Year)
                .ToList();

            Console.WriteLine($"   Общий объём продаж: {totalSales:C}");
            Console.WriteLine($"   Среднемесячные продажи: {totalSales / table.Rows.Count:C}");

            Console.WriteLine("\n   Продажи по годам:");
            foreach (var yearData in yearlySales)
            {
                decimal percentage = (yearData.Total / totalSales) * 100;
                Console.WriteLine($"   • {yearData.Year}: {yearData.Total:C} ({percentage:F1}%)");
            }
        }

        static void CreateComprehensiveVisualization(DataTable table)
        {
            Console.WriteLine("\n   Комплексный отчёт:");
            Console.WriteLine(new string('=', 60));

            // Топ-3 продукта за все время
            DataView allProductsView = new DataView(table);
            var topProducts = allProductsView.Cast<DataRowView>()
                .GroupBy(r => r["Product"].ToString())
                .Select(g => new
                {
                    Product = g.Key,
                    Total = g.Sum(r => (decimal)r["Amount"]),
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Total)
                .Take(3)
                .ToList();

            Console.WriteLine("\n   Топ-3 продукта:");
            foreach (var product in topProducts)
            {
                Console.WriteLine($"   • {product.Product}: {product.Total:C} ({product.Count} продаж)");
            }

            // Самый прибыльный регион
            var topRegion = allProductsView.Cast<DataRowView>()
                .GroupBy(r => r["Region"].ToString())
                .Select(g => new
                {
                    Region = g.Key,
                    Total = g.Sum(r => (decimal)r["Amount"])
                })
                .OrderByDescending(g => g.Total)
                .FirstOrDefault();

            if (topRegion != null)
            {
                Console.WriteLine($"\n   Самый прибыльный регион: {topRegion.Region} ({topRegion.Total:C})");
            }

            // Сезонность продаж
            Console.WriteLine("\n   Сезонность продаж (по месяцам):");
            var monthlyTrend = allProductsView.Cast<DataRowView>()
                .GroupBy(r => (int)r["MonthNumber"])
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(r => (decimal)r["Amount"]),
                    Avg = g.Average(r => (decimal)r["Amount"])
                })
                .OrderBy(g => g.Month)
                .ToList();

            string[] monthNames = { "Янв", "Фев", "Мар", "Апр", "Май", "Июн",
                           "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" };

            foreach (var month in monthlyTrend)
            {
                int index = month.Month - 1;
                if (index >= 0 && index < monthNames.Length)
                {
                    Console.WriteLine($"   {monthNames[index]}: {month.Avg:C} среднее");
                }
            }
        }
        #endregion

        #region Задание 18: Поиск ближайших значений в DataView
        static void Task18_FindNearestValues()
        {
            Console.WriteLine("ЗАДАНИЕ 18: Поиск ближайших значений в DataView\n");
            Console.WriteLine("Цель: Демонстрация поиска ближайших значений и бинарного поиска\n");

            // 1. Создание таблицы цен на акции
            DataTable stocks = CreateStocksTable();
            FillStocksData(stocks, 1000);

            Console.WriteLine("1. Создана таблица цен на акции (1000 записей):");
            PrintStocksSample(stocks, 5);

            // 2. Создание отсортированного DataView
            DataView stocksByPrice = new DataView(stocks);
            stocksByPrice.Sort = "Price ASC";

            Console.WriteLine($"\n2. DataView отсортирован по цене");
            Console.WriteLine($"   Минимальная цена: {stocksByPrice[0]["Price"]:C}");
            Console.WriteLine($"   Максимальная цена: {stocksByPrice[stocksByPrice.Count - 1]["Price"]:C}");

            // 3. Поиск ближайшей цены
            Console.WriteLine("\n3. Поиск ближайшей цены:");
            decimal targetPrice = 150.50m;

            Console.WriteLine($"   Ищем акции ближайшие к {targetPrice:C}:");

            // Находим ближайшую снизу и сверху
            var nearest = FindNearestPrices(stocksByPrice, targetPrice);

            if (nearest.Below != null)
            {
                Console.WriteLine($"   • Ближайшая снизу: {nearest.Below["CompanyName"]} - {nearest.Below["Price"]:C} " +
                                 $"(разница: {targetPrice - (decimal)nearest.Below["Price"]:C})");
            }

            if (nearest.Above != null)
            {
                Console.WriteLine($"   • Ближайшая сверху: {nearest.Above["CompanyName"]} - {nearest.Above["Price"]:C} " +
                                 $"(разница: {(decimal)nearest.Above["Price"] - targetPrice:C})");
            }

            // 4. Поиск акций в диапазоне цен
            Console.WriteLine("\n4. Поиск акций в диапазоне цен:");
            decimal minPrice = 100.00m;
            decimal maxPrice = 200.00m;

            Console.WriteLine($"   Акции в диапазоне {minPrice:C} - {maxPrice:C}:");
            var inRange = FindStocksInRange(stocksByPrice, minPrice, maxPrice);
            Console.WriteLine($"   Найдено: {inRange.Count} акций");

            if (inRange.Count > 0)
            {
                Console.WriteLine("   Примеры:");
                foreach (var stock in inRange.Take(3))
                {
                    Console.WriteLine($"   • {stock["CompanyName"]}: {stock["Price"]:C} ({stock["Date"]:dd.MM.yyyy})");
                }
            }

            // 5. Поиск акции с максимальной ценой в диапазоне
            Console.WriteLine("\n5. Поиск акции с максимальной ценой в диапазоне:");
            var maxInRange = FindMaxPriceInRange(stocksByPrice, 50.00m, 150.00m);
            if (maxInRange != null)
            {
                Console.WriteLine($"   Самая дорогая акция в диапазоне 50-150: {maxInRange["CompanyName"]} - {maxInRange["Price"]:C}");
            }

            // 6. Поиск акции с минимальной ценой в диапазоне
            Console.WriteLine("\n6. Поиск акции с минимальной ценой в диапазоне:");
            var minInRange = FindMinPriceInRange(stocksByPrice, 200.00m, 300.00m);
            if (minInRange != null)
            {
                Console.WriteLine($"   Самая дешёвая акция в диапазоне 200-300: {minInRange["CompanyName"]} - {minInRange["Price"]:C}");
            }

            // 7. Поиск скачков цены
            Console.WriteLine("\n7. Поиск скачков цены (> 10% за день):");
            var priceJumps = FindPriceJumps(stocks);
            Console.WriteLine($"   Найдено скачков: {priceJumps.Count}");

            if (priceJumps.Count > 0)
            {
                Console.WriteLine("   Крупнейшие скачки:");
                foreach (var jump in priceJumps.Take(3))
                {
                    decimal changePercent = ((decimal)jump.Price2 - (decimal)jump.Price1) / (decimal)jump.Price1 * 100;
                    Console.WriteLine($"   • {jump.CompanyName}: {jump.Price1:C} → {jump.Price2:C} ({changePercent:+0.0;-0.0}%)");
                }
            }

            // 8. Использование BinarySearch для оптимального поиска
            Console.WriteLine("\n8. Использование BinarySearch для поиска:");
            decimal[] prices = new decimal[stocksByPrice.Count];
            for (int i = 0; i < stocksByPrice.Count; i++)
            {
                prices[i] = (decimal)stocksByPrice[i]["Price"];
            }

            decimal searchPrice = 175.25m;
            int binarySearchIndex = Array.BinarySearch(prices, searchPrice);

            if (binarySearchIndex >= 0)
            {
                Console.WriteLine($"   Точное совпадение найдено на позиции {binarySearchIndex}: {stocksByPrice[binarySearchIndex]["CompanyName"]}");
            }
            else
            {
                int insertionPoint = ~binarySearchIndex;
                Console.WriteLine($"   Точное совпадение не найдено.");
                Console.WriteLine($"   Цена должна быть вставлена на позицию {insertionPoint}");

                if (insertionPoint > 0 && insertionPoint < stocksByPrice.Count)
                {
                    Console.WriteLine($"   Ближайшая снизу: {stocksByPrice[insertionPoint - 1]["CompanyName"]} - {stocksByPrice[insertionPoint - 1]["Price"]:C}");
                    Console.WriteLine($"   Ближайшая сверху: {stocksByPrice[insertionPoint]["CompanyName"]} - {stocksByPrice[insertionPoint]["Price"]:C}");
                }
            }

            // 9. Производительность различных методов поиска
            Console.WriteLine("\n9. Сравнение производительности методов поиска:");
            CompareSearchPerformance(stocksByPrice, prices);
        }

        static DataTable CreateStocksTable()
        {
            DataTable table = new DataTable("Акции");

            table.Columns.Add("StockID", typeof(int));
            table.Columns.Add("CompanyName", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Volume", typeof(long));

            return table;
        }

        static void FillStocksData(DataTable table, int count)
        {
            string[] companies = { "Apple", "Microsoft", "Google", "Amazon", "Tesla",
                          "Facebook", "Netflix", "NVIDIA", "Intel", "AMD",
                          "IBM", "Oracle", "Adobe", "Salesforce", "Cisco" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2023, 1, 1);

            // Создаем исторические данные с трендами
            Dictionary<string, decimal> lastPrices = new Dictionary<string, decimal>();

            for (int i = 1; i <= count; i++)
            {
                string company = companies[rand.Next(companies.Length)];

                if (!lastPrices.ContainsKey(company))
                {
                    lastPrices[company] = 50 + (decimal)rand.NextDouble() * 450; // 50-500
                }

                // Изменяем цену с небольшим случайным шагом
                decimal change = (decimal)(rand.NextDouble() - 0.5) * 20; // -10..+10
                decimal price = Math.Max(10, lastPrices[company] + change); // не ниже 10
                lastPrices[company] = price;

                int daysOffset = rand.Next(0, 365);

                table.Rows.Add(
                    i,
                    company,
                    Math.Round(price, 2),
                    startDate.AddDays(daysOffset),
                    rand.Next(1000000, 10000000)
                );
            }
        }

        static void PrintStocksSample(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"Компания",-15} {"Цена",-10} {"Дата",-12} {"Объём",-15}");
            Console.WriteLine(new string('-', 55));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["CompanyName"],-15} " +
                                $"{table.Rows[i]["Price"],10:C} " +
                                $"{((DateTime)table.Rows[i]["Date"]):dd.MM.yyyy} " +
                                $"{((long)table.Rows[i]["Volume"]):N0}");
            }
        }

        static (DataRowView Below, DataRowView Above) FindNearestPrices(DataView sortedView, decimal targetPrice)
        {
            DataRowView below = null;
            DataRowView above = null;

            // Бинарный поиск ближайших значений
            int left = 0;
            int right = sortedView.Count - 1;

            while (left <= right)
            {
                int mid = (left + right) / 2;
                decimal midPrice = (decimal)sortedView[mid]["Price"];

                if (midPrice == targetPrice)
                {
                    // Точное совпадение
                    below = sortedView[mid];
                    above = sortedView[mid];
                    break;
                }
                else if (midPrice < targetPrice)
                {
                    below = sortedView[mid];
                    left = mid + 1;
                }
                else
                {
                    above = sortedView[mid];
                    right = mid - 1;
                }
            }

            return (below, above);
        }

        static List<DataRowView> FindStocksInRange(DataView sortedView, decimal minPrice, decimal maxPrice)
        {
            var result = new List<DataRowView>();

            // Находим первую позицию, где цена >= minPrice
            int startIndex = 0;
            for (int i = 0; i < sortedView.Count; i++)
            {
                if ((decimal)sortedView[i]["Price"] >= minPrice)
                {
                    startIndex = i;
                    break;
                }
            }

            // Добавляем все акции в диапазоне
            for (int i = startIndex; i < sortedView.Count; i++)
            {
                decimal price = (decimal)sortedView[i]["Price"];
                if (price > maxPrice)
                    break;

                result.Add(sortedView[i]);
            }

            return result;
        }

        static DataRowView FindMaxPriceInRange(DataView sortedView, decimal minPrice, decimal maxPrice)
        {
            DataRowView maxStock = null;
            decimal maxFoundPrice = decimal.MinValue;

            foreach (DataRowView row in sortedView)
            {
                decimal price = (decimal)row["Price"];

                if (price < minPrice)
                    continue;

                if (price > maxPrice)
                    break;

                if (price > maxFoundPrice)
                {
                    maxFoundPrice = price;
                    maxStock = row;
                }
            }

            return maxStock;
        }

        static DataRowView FindMinPriceInRange(DataView sortedView, decimal minPrice, decimal maxPrice)
        {
            // Поскольку DataView отсортирован по цене, первая подходящая акция будет минимальной
            foreach (DataRowView row in sortedView)
            {
                decimal price = (decimal)row["Price"];

                if (price >= minPrice && price <= maxPrice)
                {
                    return row;
                }
            }

            return null;
        }

        static List<StockPriceJump> FindPriceJumps(DataTable table)
        {
            var jumps = new List<StockPriceJump>();

            // Группируем по компании
            var companyGroups = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("CompanyName"))
                .ToList();

            foreach (var group in companyGroups)
            {
                // Сортируем по дате
                var sorted = group.OrderBy(row => row.Field<DateTime>("Date")).ToList();

                for (int i = 1; i < sorted.Count; i++)
                {
                    decimal prevPrice = sorted[i - 1].Field<decimal>("Price");
                    decimal currentPrice = sorted[i].Field<decimal>("Price");
                    decimal changePercent = Math.Abs((currentPrice - prevPrice) / prevPrice * 100);

                    if (changePercent > 10) // Скачок более 10%
                    {
                        jumps.Add(new StockPriceJump
                        {
                            CompanyName = group.Key,
                            Date1 = sorted[i - 1].Field<DateTime>("Date"),
                            Price1 = prevPrice,
                            Date2 = sorted[i].Field<DateTime>("Date"),
                            Price2 = currentPrice
                        });
                    }
                }
            }

            // Сортируем по величине скачка
            return jumps.OrderByDescending(j => Math.Abs((decimal)j.Price2 - (decimal)j.Price1)).ToList();
        }

        static void CompareSearchPerformance(DataView sortedView, decimal[] prices)
        {
            int iterations = 10000;
            Random rand = new Random();

            Console.WriteLine($"{"Метод",-30} {"Время (мс)",-15} {"Операций/сек",-15}");
            Console.WriteLine(new string('-', 60));

            // Линейный поиск
            Stopwatch swLinear = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                decimal target = 50 + (decimal)rand.NextDouble() * 450;

                for (int j = 0; j < sortedView.Count; j++)
                {
                    if ((decimal)sortedView[j]["Price"] >= target)
                    {
                        break;
                    }
                }
            }
            swLinear.Stop();

            // Бинарный поиск на массиве
            Stopwatch swBinary = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                decimal target = 50 + (decimal)rand.NextDouble() * 450;
                Array.BinarySearch(prices, target);
            }
            swBinary.Stop();

            // DataView.Find (только для отсортированных уникальных значений)
            Stopwatch swFind = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                decimal target = 50 + (decimal)rand.NextDouble() * 450;
                // Find работает только для точных совпадений
                int index = sortedView.Find(target);
            }
            swFind.Stop();

            Console.WriteLine($"{"Линейный поиск",-30} {swLinear.ElapsedMilliseconds,-15} {iterations * 1000 / swLinear.ElapsedMilliseconds,-15:F0}");
            Console.WriteLine($"{"BinarySearch на массиве",-30} {swBinary.ElapsedMilliseconds,-15} {iterations * 1000 / swBinary.ElapsedMilliseconds,-15:F0}");
            Console.WriteLine($"{"DataView.Find",-30} {swFind.ElapsedMilliseconds,-15} {iterations * 1000 / swFind.ElapsedMilliseconds,-15:F0}");

            Console.WriteLine($"\n   BinarySearch быстрее линейного в {(double)swLinear.ElapsedMilliseconds / swBinary.ElapsedMilliseconds:F1} раза");
        }

        class StockPriceJump
        {
            public string CompanyName { get; set; }
            public DateTime Date1 { get; set; }
            public object Price1 { get; set; }
            public DateTime Date2 { get; set; }
            public object Price2 { get; set; }
        }
        #endregion

        #region Задание 19: Валидация данных при редактировании через DataView
        static void Task19_ValidationInDataView()
        {
            Console.WriteLine("ЗАДАНИЕ 19: Валидация данных при редактировании через DataView\n");
            Console.WriteLine("Цель: Демонстрация валидации данных в DataView\n");

            // 1. Создание таблицы контактов с ограничениями
            DataTable contacts = CreateContactsTable();
            FillContactsData(contacts, 100);

            Console.WriteLine("1. Создана таблица контактов (100 записей):");
            PrintContactsTable(contacts, 5);

            // 2. Настройка валидации для таблицы
            Console.WriteLine("\n2. Настройка правил валидации:");
            SetupValidationRules(contacts);

            // 3. Создание DataView
            DataView contactsView = new DataView(contacts);
            contactsView.Sort = "Name ASC";

            Console.WriteLine("\n3. Редактирование контактов через DataView с валидацией:");

            // 4. Тестирование различных сценариев редактирования
            Console.WriteLine("\n4. Тестирование валидации:");

            // Сценарий 1: Корректное редактирование
            Console.WriteLine("\n   Сценарий 1: Корректное редактирование");
            if (contactsView.Count > 0)
            {
                try
                {
                    contactsView[0]["Email"] = "new.email@example.com";
                    contactsView[0]["Phone"] = "(123)456-7890";
                    contactsView[0].EndEdit();
                    Console.WriteLine("   ✓ Контакт успешно обновлён");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ✗ Ошибка: {ex.Message}");
                }
            }

            // Сценарий 2: Неверный email
            Console.WriteLine("\n   Сценарий 2: Неверный формат email");
            if (contactsView.Count > 1)
            {
                try
                {
                    contactsView[1]["Email"] = "invalid-email";
                    contactsView[1].EndEdit();
                    Console.WriteLine("   ✗ Ожидалась ошибка валидации");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ✓ Правильно: {ex.Message}");
                }
            }

            // Сценарий 3: Неверный телефон
            Console.WriteLine("\n   Сценарий 3: Неверный формат телефона");
            if (contactsView.Count > 2)
            {
                try
                {
                    contactsView[2]["Phone"] = "123";
                    contactsView[2].EndEdit();
                    Console.WriteLine("   ✗ Ожидалась ошибка валидации");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ✓ Правильно: {ex.Message}");
                }
            }

            // Сценарий 4: Пустое имя
            Console.WriteLine("\n   Сценарий 4: Пустое имя");
            if (contactsView.Count > 3)
            {
                try
                {
                    contactsView[3]["Name"] = "";
                    contactsView[3].EndEdit();
                    Console.WriteLine("   ✗ Ожидалась ошибка валидации");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ✓ Правильно: {ex.Message}");
                }
            }

            // Сценарий 5: Дата рождения в будущем
            Console.WriteLine("\n   Сценарий 5: Дата рождения в будущем");
            if (contactsView.Count > 4)
            {
                try
                {
                    contactsView[4]["BirthDate"] = DateTime.Now.AddDays(1);
                    contactsView[4].EndEdit();
                    Console.WriteLine("   ✗ Ожидалась ошибка валидации");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ✓ Правильно: {ex.Message}");
                }
            }

            // Сценарий 6: Пустой адрес
            Console.WriteLine("\n   Сценарий 6: Пустой адрес");
            if (contactsView.Count > 5)
            {
                try
                {
                    contactsView[5]["Address"] = "";
                    contactsView[5].EndEdit();
                    Console.WriteLine("   ✗ Ожидалась ошибка валидации");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ✓ Правильно: {ex.Message}");
                }
            }

            // 5. Автоисправление данных
            Console.WriteLine("\n5. Автоисправление данных:");
            TestAutoCorrection(contactsView);

            // 6. Логирование попыток редактирования
            Console.WriteLine("\n6. Логирование операций:");
            var editLog = LogEditOperations(contactsView);
            PrintEditLog(editLog);

            // 7. Статистика ошибок валидации
            Console.WriteLine("\n7. Статистика валидации:");
            PrintValidationStatistics(editLog);

            // 8. Пакетное редактирование с валидацией
            Console.WriteLine("\n8. Пакетное редактирование:");
            TestBatchEditing(contactsView);

            // 9. Откат невалидных изменений
            Console.WriteLine("\n9. Откат невалидных изменений:");
            TestRollbackInvalidChanges(contactsView);
        }

        static DataTable CreateContactsTable()
        {
            DataTable table = new DataTable("Контакты");

            table.Columns.Add("ContactID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Phone", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("BirthDate", typeof(DateTime));

            // Установка первичного ключа
            table.PrimaryKey = new DataColumn[] { table.Columns["ContactID"] };

            return table;
        }

        static void FillContactsData(DataTable table, int count)
        {
            string[] firstNames = { "Алексей", "Мария", "Дмитрий", "Елена", "Сергей",
                           "Ольга", "Андрей", "Наталья", "Михаил", "Светлана" };
            string[] lastNames = { "Иванов", "Петрова", "Сидоров", "Смирнова", "Кузнецов",
                          "Попова", "Васильев", "Николаева", "Алексеев", "Морозова" };
            string[] domains = { "gmail.com", "mail.ru", "yandex.ru", "outlook.com", "yahoo.com" };

            Random rand = new Random();
            DateTime startDate = new DateTime(1950, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];
                string domain = domains[rand.Next(domains.Length)];

                table.Rows.Add(
                    i,
                    $"{lastName} {firstName}",
                    $"{firstName.ToLower()}.{lastName.ToLower()}@{domain}",
                    $"({rand.Next(100, 1000)}){rand.Next(100, 1000)}-{rand.Next(1000, 10000)}",
                    $"ул. {lastName}а, д. {i}, кв. {rand.Next(1, 100)}",
                    startDate.AddDays(rand.Next(0, 25550)) // до 70 лет
                );
            }
        }

        static void PrintContactsTable(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-4} {"Имя",-20} {"Email",-25} {"Телефон",-15}");
            Console.WriteLine(new string('-', 70));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["ContactID"],-4} " +
                                $"{table.Rows[i]["Name"],-20} " +
                                $"{table.Rows[i]["Email"],-25} " +
                                $"{table.Rows[i]["Phone"],-15}");
            }
        }

        static void SetupValidationRules(DataTable table)
        {
            Console.WriteLine("   Правила валидации:");
            Console.WriteLine("   1. Email должен содержать '@' и '.'");
            Console.WriteLine("   2. Телефон в формате (XXX)XXX-XXXX");
            Console.WriteLine("   3. Имя не может быть пустым");
            Console.WriteLine("   4. Дата рождения не может быть в будущем");
            Console.WriteLine("   5. Адрес не может быть пустым");

            // Обработчик события ColumnChanged для валидации
            table.ColumnChanged += (sender, e) =>
            {
                if (e.Row.RowState == DataRowState.Detached)
                    return;

                switch (e.Column.ColumnName)
                {
                    case "Email":
                        string email = e.ProposedValue?.ToString() ?? "";
                        if (!email.Contains("@") || !email.Contains("."))
                        {
                            throw new ArgumentException($"Неверный формат email: {email}");
                        }
                        break;

                    case "Phone":
                        string phone = e.ProposedValue?.ToString() ?? "";
                        // Простая проверка формата (XXX)XXX-XXXX
                        if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\(\d{3}\)\d{3}-\d{4}$"))
                        {
                            throw new ArgumentException($"Неверный формат телефона. Используйте (XXX)XXX-XXXX: {phone}");
                        }
                        break;

                    case "Name":
                        string name = e.ProposedValue?.ToString() ?? "";
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            throw new ArgumentException("Имя не может быть пустым");
                        }
                        break;

                    case "BirthDate":
                        if (e.ProposedValue is DateTime birthDate)
                        {
                            if (birthDate > DateTime.Now)
                            {
                                throw new ArgumentException($"Дата рождения не может быть в будущем: {birthDate:dd.MM.yyyy}");
                            }
                        }
                        break;

                    case "Address":
                        string address = e.ProposedValue?.ToString() ?? "";
                        if (string.IsNullOrWhiteSpace(address))
                        {
                            throw new ArgumentException("Адрес не может быть пустым");
                        }
                        break;
                }
            };

            Console.WriteLine("   ✓ Правила валидации установлены");
        }

        static void TestAutoCorrection(DataView view)
        {
            Console.WriteLine("   Тест автоформатирования телефона:");

            if (view.Count > 6)
            {
                string originalPhone = view[6]["Phone"].ToString();
                string testPhone = "1234567890"; // Без форматирования

                try
                {
                    view[6]["Phone"] = testPhone;
                    view[6].EndEdit();
                    Console.WriteLine($"   ✗ Ожидалась ошибка: {testPhone}");
                }
                catch (Exception)
                {
                    // Автоисправление: форматируем телефон
                    string formattedPhone = $"({testPhone.Substring(0, 3)}){testPhone.Substring(3, 3)}-{testPhone.Substring(6)}";
                    view[6]["Phone"] = formattedPhone;
                    view[6].EndEdit();

                    Console.WriteLine($"   ✓ Автоисправление: {testPhone} → {formattedPhone}");
                }
            }
        }

        static List<EditLogEntry> LogEditOperations(DataView view)
        {
            var log = new List<EditLogEntry>();

            // Имитируем несколько операций редактирования
            Random rand = new Random();

            for (int i = 0; i < Math.Min(10, view.Count); i++)
            {
                var entry = new EditLogEntry
                {
                    Timestamp = DateTime.Now.AddMinutes(-rand.Next(0, 60)),
                    ContactID = (int)view[i]["ContactID"],
                    Field = "Email",
                    OldValue = view[i]["Email"].ToString(),
                    NewValue = $"updated{i}@example.com",
                    Success = rand.NextDouble() > 0.3 // 70% успешных операций
                };

                if (!entry.Success)
                {
                    entry.NewValue = "invalid-email";
                    entry.ErrorMessage = "Неверный формат email";
                }

                log.Add(entry);
            }

            return log;
        }

        static void PrintEditLog(List<EditLogEntry> log)
        {
            Console.WriteLine($"{"Время",-20} {"ID",-5} {"Поле",-10} {"Успешно",-10} {"Ошибка",-30}");
            Console.WriteLine(new string('-', 80));

            foreach (var entry in log)
            {
                string status = entry.Success ? "✓" : "✗";
                string error = entry.ErrorMessage ?? "-";

                Console.WriteLine($"{entry.Timestamp:HH:mm:ss} {entry.ContactID,-5} {entry.Field,-10} {status,-10} {error,-30}");
            }
        }

        static void PrintValidationStatistics(List<EditLogEntry> log)
        {
            int total = log.Count;
            int successful = log.Count(e => e.Success);
            int failed = total - successful;

            double successRate = (double)successful / total * 100;

            Console.WriteLine($"   Всего операций: {total}");
            Console.WriteLine($"   Успешных: {successful} ({successRate:F1}%)");
            Console.WriteLine($"   Неудачных: {failed} ({100 - successRate:F1}%)");

            if (failed > 0)
            {
                var errors = log.Where(e => !e.Success)
                               .GroupBy(e => e.ErrorMessage)
                               .Select(g => new { Error = g.Key, Count = g.Count() })
                               .OrderByDescending(g => g.Count);

                Console.WriteLine("\n   Частые ошибки:");
                foreach (var error in errors)
                {
                    Console.WriteLine($"   • {error.Error}: {error.Count} раз");
                }
            }
        }

        static void TestBatchEditing(DataView view)
        {
            Console.WriteLine("   Пакетное обновление email для 5 контактов:");

            int successful = 0;
            int failed = 0;

            for (int i = 0; i < Math.Min(5, view.Count); i++)
            {
                try
                {
                    string oldEmail = view[i]["Email"].ToString();
                    string newEmail = $"contact{i}@company.com";

                    view[i]["Email"] = newEmail;
                    view[i].EndEdit();

                    Console.WriteLine($"   ✓ {view[i]["Name"]}: {oldEmail} → {newEmail}");
                    successful++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ✗ {view[i]["Name"]}: {ex.Message}");
                    failed++;
                }
            }

            Console.WriteLine($"   Итог: {successful} успешно, {failed} с ошибками");
        }

        static void TestRollbackInvalidChanges(DataView view)
        {
            Console.WriteLine("   Тест отката невалидных изменений:");

            if (view.Count > 0)
            {
                string originalName = view[0]["Name"].ToString();
                string originalEmail = view[0]["Email"].ToString();

                try
                {
                    // Пытаемся установить невалидные значения
                    view[0]["Name"] = ""; // Пустое имя
                    view[0]["Email"] = "invalid-email";
                    view[0].EndEdit();
                }
                catch (Exception)
                {
                    // Откатываем изменения
                    view[0].CancelEdit();

                    // Проверяем, что значения вернулись к оригинальным
                    if (view[0]["Name"].ToString() == originalName &&
                        view[0]["Email"].ToString() == originalEmail)
                    {
                        Console.WriteLine($"   ✓ Изменения откачены: {originalName}, {originalEmail}");
                    }
                }
            }
        }

        class EditLogEntry
        {
            public DateTime Timestamp { get; set; }
            public int ContactID { get; set; }
            public string Field { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }
        #endregion

        #region Задание 20: Экспорт данных из DataView в различные форматы
        static void Task20_ExportFromDataView()
        {
            Console.WriteLine("ЗАДАНИЕ 20: Экспорт данных из DataView в различные форматы\n");
            Console.WriteLine("Цель: Демонстрация экспорта данных в различные форматы\n");

            // 1. Создание таблицы отчетов о продажах
            DataTable salesReports = CreateSalesReportsTable();
            FillSalesReportsData(salesReports, 500);

            Console.WriteLine("1. Создана таблица отчетов о продажах (500 записей):");
            PrintSalesReportsTable(salesReports, 5);

            // 2. Создание DataView с фильтром и сортировкой
            DataView filteredView = new DataView(salesReports);
            filteredView.RowFilter = "Date >= '2024-01-01' AND Region = 'Северный'";
            filteredView.Sort = "Date DESC, Amount DESC";

            Console.WriteLine($"\n2. DataView с фильтром (продажи с 2024 в Северном регионе):");
            Console.WriteLine($"   Записей в DataView: {filteredView.Count}");

            // 3. Экспорт в CSV
            Console.WriteLine("\n3. Экспорт в CSV:");
            string csvFile = "sales_export.csv";
            ExportToCSV(filteredView, csvFile);
            Console.WriteLine($"   ✓ Экспортировано {filteredView.Count} записей в {csvFile}");

            // 4. Экспорт в XML
            Console.WriteLine("\n4. Экспорт в XML:");
            string xmlFile = "sales_export.xml";
            ExportToXML(filteredView, xmlFile);
            Console.WriteLine($"   ✓ Экспортировано в {xmlFile}");

            // 5. Экспорт в JSON
            Console.WriteLine("\n5. Экспорт в JSON:");
            string jsonFile = "sales_export.json";
            ExportToJSON(filteredView, jsonFile);
            Console.WriteLine($"   ✓ Экспортировано в {jsonFile}");

            // 6. Экспорт в HTML
            Console.WriteLine("\n6. Экспорт в HTML таблицу:");
            string htmlFile = "sales_export.html";
            ExportToHTML(filteredView, htmlFile);
            Console.WriteLine($"   ✓ Экспортировано в {htmlFile}");

            // 7. Экспорт с заголовком и метаданными
            Console.WriteLine("\n7. Экспорт с метаданными:");
            ExportWithMetadata(filteredView, "sales_with_metadata.csv");

            // 8. Импорт из CSV обратно в DataTable
            Console.WriteLine("\n8. Импорт из CSV обратно в DataTable:");
            DataTable importedTable = ImportFromCSV("sales_export.csv");
            Console.WriteLine($"   ✓ Импортировано {importedTable.Rows.Count} записей");

            // 9. Сравнение экспортированных файлов
            Console.WriteLine("\n9. Сравнение экспортированных файлов:");
            CompareExportFiles();

            // 10. Экспорт с выбором колонок
            Console.WriteLine("\n10. Экспорт с выбором колонок:");
            ExportSelectedColumns(filteredView, new[] { "Date", "Product", "Amount", "Salesperson" }, "sales_selected.csv");

            // 11. Пакетный экспорт
            Console.WriteLine("\n11. Пакетный экспорт в несколько форматов:");
            BatchExport(filteredView, "batch_export");
        }

        static DataTable CreateSalesReportsTable()
        {
            DataTable table = new DataTable("Отчеты о продажах");

            table.Columns.Add("ReportID", typeof(int));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Amount", typeof(decimal)); // Calculated: Quantity * Price
            table.Columns.Add("Salesperson", typeof(string));
            table.Columns.Add("Region", typeof(string));

            return table;
        }

        static void FillSalesReportsData(DataTable table, int count)
        {
            string[] products = { "Ноутбук", "Смартфон", "Планшет", "Монитор", "Принтер", "Клавиатура", "Мышь" };
            string[] salespersons = { "Иванов", "Петров", "Сидоров", "Кузнецова", "Смирнова", "Попов", "Васильев" };
            string[] regions = { "Северный", "Южный", "Центральный", "Западный", "Восточный" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2023, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                int daysOffset = rand.Next(0, 730); // 2 года
                DateTime date = startDate.AddDays(daysOffset);
                int quantity = rand.Next(1, 100);
                decimal price = Math.Round((decimal)rand.NextDouble() * 50000, 2);
                decimal amount = quantity * price;

                table.Rows.Add(
                    i,
                    date,
                    products[rand.Next(products.Length)],
                    quantity,
                    price,
                    amount,
                    salespersons[rand.Next(salespersons.Length)],
                    regions[rand.Next(regions.Length)]
                );
            }
        }

        static void PrintSalesReportsTable(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"Дата",-12} {"Товар",-15} {"Кол-во",-8} {"Цена",-10} {"Сумма",-12} {"Продавец",-15} {"Регион",-12}");
            Console.WriteLine(new string('-', 85));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{((DateTime)table.Rows[i]["Date"]):dd.MM.yyyy} " +
                                $"{table.Rows[i]["Product"],-15} " +
                                $"{table.Rows[i]["Quantity"],8} " +
                                $"{table.Rows[i]["Price"],10:C} " +
                                $"{table.Rows[i]["Amount"],12:C} " +
                                $"{table.Rows[i]["Salesperson"],-15} " +
                                $"{table.Rows[i]["Region"],-12}");
            }
        }

        static void ExportToCSV(DataView view, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
                {
                    // Заголовок
                    for (int i = 0; i < view.Table.Columns.Count; i++)
                    {
                        writer.Write(view.Table.Columns[i].ColumnName);
                        if (i < view.Table.Columns.Count - 1)
                            writer.Write(",");
                    }
                    writer.WriteLine();

                    // Данные
                    foreach (DataRowView rowView in view)
                    {
                        for (int i = 0; i < view.Table.Columns.Count; i++)
                        {
                            object value = rowView[i];
                            string stringValue = "";

                            if (value != null && value != DBNull.Value)
                            {
                                stringValue = value.ToString();

                                // Экранирование специальных символов в CSV
                                if (stringValue.Contains(",") || stringValue.Contains("\"") || stringValue.Contains("\n"))
                                {
                                    stringValue = "\"" + stringValue.Replace("\"", "\"\"") + "\"";
                                }
                            }

                            writer.Write(stringValue);
                            if (i < view.Table.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                }

                Console.WriteLine($"   Размер файла: {new FileInfo(fileName).Length} байт");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Ошибка экспорта в CSV: {ex.Message}");
            }
        }

        static void ExportToXML(DataView view, string fileName)
        {
            try
            {
                DataTable tableForExport = view.ToTable();

                using (StreamWriter writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    writer.WriteLine("<SalesData>");

                    // Метаданные
                    writer.WriteLine("  <Metadata>");
                    writer.WriteLine($"    <ExportDate>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</ExportDate>");
                    writer.WriteLine($"    <RecordCount>{tableForExport.Rows.Count}</RecordCount>");
                    writer.WriteLine($"    <Filter>{view.RowFilter ?? "Нет"}</Filter>");
                    writer.WriteLine($"    <Sort>{view.Sort ?? "Нет"}</Sort>");
                    writer.WriteLine("  </Metadata>");

                    // Данные
                    writer.WriteLine("  <Records>");

                    foreach (DataRow row in tableForExport.Rows)
                    {
                        writer.WriteLine("    <Record>");

                        foreach (DataColumn column in tableForExport.Columns)
                        {
                            object value = row[column];
                            string stringValue = value != null && value != DBNull.Value ?
                                                System.Security.SecurityElement.Escape(value.ToString()) : "";

                            writer.WriteLine($"      <{column.ColumnName}>{stringValue}</{column.ColumnName}>");
                        }

                        writer.WriteLine("    </Record>");
                    }

                    writer.WriteLine("  </Records>");
                    writer.WriteLine("</SalesData>");
                }

                Console.WriteLine($"   Размер файла: {new FileInfo(fileName).Length} байт");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Ошибка экспорта в XML: {ex.Message}");
            }
        }

        static void ExportToJSON(DataView view, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("{");
                    writer.WriteLine("  \"metadata\": {");
                    writer.WriteLine($"    \"exportDate\": \"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\",");
                    writer.WriteLine($"    \"recordCount\": {view.Count},");
                    writer.WriteLine($"    \"filter\": \"{view.RowFilter ?? ""}\",");
                    writer.WriteLine($"    \"sort\": \"{view.Sort ?? ""}\"");
                    writer.WriteLine("  },");
                    writer.WriteLine("  \"data\": [");

                    for (int i = 0; i < view.Count; i++)
                    {
                        writer.WriteLine("    {");

                        for (int j = 0; j < view.Table.Columns.Count; j++)
                        {
                            object value = view[i][j];
                            string stringValue = "null";

                            if (value != null && value != DBNull.Value)
                            {
                                if (value is string)
                                {
                                    stringValue = "\"" + value.ToString().Replace("\"", "\\\"") + "\"";
                                }
                                else if (value is DateTime)
                                {
                                    stringValue = "\"" + ((DateTime)value).ToString("yyyy-MM-dd") + "\"";
                                }
                                else
                                {
                                    stringValue = value.ToString().ToLower(); // для bool
                                }
                            }

                            writer.Write($"      \"{view.Table.Columns[j].ColumnName}\": {stringValue}");

                            if (j < view.Table.Columns.Count - 1)
                                writer.WriteLine(",");
                            else
                                writer.WriteLine();
                        }

                        writer.Write(i < view.Count - 1 ? "    },\n" : "    }\n");
                    }

                    writer.WriteLine("  ]");
                    writer.WriteLine("}");
                }

                Console.WriteLine($"   Размер файла: {new FileInfo(fileName).Length} байт");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Ошибка экспорта в JSON: {ex.Message}");
            }
        }

        static void ExportToHTML(DataView view, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("<!DOCTYPE html>");
                    writer.WriteLine("<html lang='ru'>");
                    writer.WriteLine("<head>");
                    writer.WriteLine("  <meta charset='UTF-8'>");
                    writer.WriteLine($"  <title>Отчёт о продажах - {DateTime.Now:dd.MM.yyyy}</title>");
                    writer.WriteLine("  <style>");
                    writer.WriteLine("    body { font-family: Arial, sans-serif; margin: 20px; }");
                    writer.WriteLine("    h1 { color: #333; }");
                    writer.WriteLine("    table { border-collapse: collapse; width: 100%; margin-top: 20px; }");
                    writer.WriteLine("    th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
                    writer.WriteLine("    th { background-color: #4CAF50; color: white; }");
                    writer.WriteLine("    tr:nth-child(even) { background-color: #f2f2f2; }");
                    writer.WriteLine("    .metadata { background-color: #e8f4f8; padding: 10px; margin-bottom: 20px; }");
                    writer.WriteLine("  </style>");
                    writer.WriteLine("</head>");
                    writer.WriteLine("<body>");
                    writer.WriteLine($"  <h1>Отчёт о продажах</h1>");

                    // Метаданные
                    writer.WriteLine("  <div class='metadata'>");
                    writer.WriteLine($"    <strong>Дата экспорта:</strong> {DateTime.Now:dd.MM.yyyy HH:mm:ss}<br>");
                    writer.WriteLine($"    <strong>Количество записей:</strong> {view.Count}<br>");
                    writer.WriteLine($"    <strong>Фильтр:</strong> {view.RowFilter ?? "Нет"}<br>");
                    writer.WriteLine($"    <strong>Сортировка:</strong> {view.Sort ?? "Нет"}");
                    writer.WriteLine("  </div>");

                    // Таблица
                    writer.WriteLine("  <table>");
                    writer.WriteLine("    <thead>");
                    writer.WriteLine("      <tr>");

                    // Заголовки колонок
                    foreach (DataColumn column in view.Table.Columns)
                    {
                        writer.WriteLine($"        <th>{column.ColumnName}</th>");
                    }

                    writer.WriteLine("      </tr>");
                    writer.WriteLine("    </thead>");
                    writer.WriteLine("    <tbody>");

                    // Данные - ИСПРАВЛЕННАЯ ЧАСТЬ
                    foreach (DataRowView rowView in view)
                    {
                        writer.WriteLine("      <tr>");

                        foreach (DataColumn column in view.Table.Columns)
                        {
                            // Используем имя колонки вместо объекта DataColumn
                            object value = rowView[column.ColumnName];
                            string stringValue = value != null && value != DBNull.Value ?
                                                value.ToString() : "";

                            // Форматирование специальных типов
                            if (value is DateTime)
                            {
                                stringValue = ((DateTime)value).ToString("dd.MM.yyyy");
                            }
                            else if (value is decimal)
                            {
                                stringValue = ((decimal)value).ToString("C");
                            }
                            else if (value is int || value is long)
                            {
                                // Форматирование чисел с разделителями тысяч
                                if (long.TryParse(stringValue, out long longValue))
                                {
                                    stringValue = longValue.ToString("N0");
                                }
                            }

                            writer.WriteLine($"        <td>{System.Web.HttpUtility.HtmlEncode(stringValue)}</td>");
                        }

                        writer.WriteLine("      </tr>");
                    }

                    writer.WriteLine("    </tbody>");
                    writer.WriteLine("  </table>");
                    writer.WriteLine("</body>");
                    writer.WriteLine("</html>");
                }

                Console.WriteLine($"   Размер файла: {new FileInfo(fileName).Length} байт");
                Console.WriteLine("   Файл готов для открытия в браузере");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Ошибка экспорта в HTML: {ex.Message}");
            }
        }

        static void ExportWithMetadata(DataView view, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
                {
                    // Метаданные в комментариях
                    writer.WriteLine("# =========================================");
                    writer.WriteLine("# Экспорт данных о продажах");
                    writer.WriteLine("# =========================================");
                    writer.WriteLine($"# Дата экспорта: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine($"# Количество записей: {view.Count}");
                    writer.WriteLine($"# Применённый фильтр: {view.RowFilter ?? "Нет"}");
                    writer.WriteLine($"# Сортировка: {view.Sort ?? "Нет"}");
                    writer.WriteLine($"# Колонок: {view.Table.Columns.Count}");
                    writer.WriteLine("# =========================================");
                    writer.WriteLine();

                    // Заголовок
                    for (int i = 0; i < view.Table.Columns.Count; i++)
                    {
                        writer.Write(view.Table.Columns[i].ColumnName);
                        if (i < view.Table.Columns.Count - 1)
                            writer.Write(",");
                    }
                    writer.WriteLine();

                    // Данные
                    foreach (DataRowView rowView in view)
                    {
                        for (int i = 0; i < view.Table.Columns.Count; i++)
                        {
                            object value = rowView[i];
                            string stringValue = "";

                            if (value != null && value != DBNull.Value)
                            {
                                stringValue = value.ToString();

                                if (stringValue.Contains(",") || stringValue.Contains("\"") || stringValue.Contains("\n"))
                                {
                                    stringValue = "\"" + stringValue.Replace("\"", "\"\"") + "\"";
                                }
                            }

                            writer.Write(stringValue);
                            if (i < view.Table.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                }

                Console.WriteLine($"   ✓ Экспорт с метаданными выполнен в {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Ошибка: {ex.Message}");
            }
        }

        static DataTable ImportFromCSV(string fileName)
        {
            DataTable table = new DataTable();

            try
            {
                using (StreamReader reader = new StreamReader(fileName, System.Text.Encoding.UTF8))
                {
                    // Читаем первую строку - заголовки
                    string headerLine = reader.ReadLine();
                    if (headerLine == null)
                        return table;

                    string[] headers = headerLine.Split(',');

                    // Создаем колонки
                    foreach (string header in headers)
                    {
                        table.Columns.Add(header.Trim('\"'), typeof(string));
                    }

                    // Читаем данные
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                            continue;

                        string[] values = ParseCSVLine(line);
                        table.Rows.Add(values);
                    }
                }

                Console.WriteLine($"   ✓ Импорт завершён успешно");

                // Преобразуем типы данных
                ConvertColumnTypes(table);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Ошибка импорта: {ex.Message}");
            }

            return table;
        }

        static string[] ParseCSVLine(string line)
        {
            var values = new List<string>();
            bool inQuotes = false;
            string currentValue = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '\"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '\"')
                    {
                        currentValue += '\"';
                        i++; // Пропускаем следующий символ
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(currentValue);
                    currentValue = "";
                }
                else
                {
                    currentValue += c;
                }
            }

            values.Add(currentValue);
            return values.ToArray();
        }

        static void ConvertColumnTypes(DataTable table)
        {
            foreach (DataColumn column in table.Columns)
            {
                // Пробуем определить тип данных по содержимому
                bool allNull = true;
                bool allInt = true;
                bool allDecimal = true;
                bool allDateTime = true;

                foreach (DataRow row in table.Rows)
                {
                    string value = row[column].ToString();

                    if (!string.IsNullOrEmpty(value))
                    {
                        allNull = false;

                        if (!int.TryParse(value, out _))
                            allInt = false;

                        if (!decimal.TryParse(value, out _))
                            allDecimal = false;

                        if (!DateTime.TryParse(value, out _))
                            allDateTime = false;
                    }
                }

                // Меняем тип колонки
                if (!allNull)
                {
                    if (allDateTime)
                    {
                        column.DataType = typeof(DateTime);
                    }
                    else if (allInt)
                    {
                        column.DataType = typeof(int);
                    }
                    else if (allDecimal)
                    {
                        column.DataType = typeof(decimal);
                    }
                }
            }
        }

        static void CompareExportFiles()
        {
            string[] files = { "sales_export.csv", "sales_export.xml", "sales_export.json", "sales_export.html" };

            Console.WriteLine($"{"Формат",-10} {"Размер (байт)",-15} {"Строк",-10}");
            Console.WriteLine(new string('-', 40));

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    FileInfo info = new FileInfo(file);
                    int lineCount = 0;

                    try
                    {
                        lineCount = File.ReadAllLines(file).Length;
                    }
                    catch { }

                    Console.WriteLine($"{Path.GetExtension(file),-10} {info.Length,-15} {lineCount,-10}");
                }
            }
        }

        static void ExportSelectedColumns(DataView view, string[] columns, string fileName)
        {
            try
            {
                DataTable selectedTable = view.ToTable("SelectedData", false, columns);

                using (StreamWriter writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
                {
                    for (int i = 0; i < selectedTable.Columns.Count; i++)
                    {
                        writer.Write(selectedTable.Columns[i].ColumnName);
                        if (i < selectedTable.Columns.Count - 1)
                            writer.Write(",");
                    }
                    writer.WriteLine();

                    foreach (DataRow row in selectedTable.Rows)
                    {
                        for (int i = 0; i < selectedTable.Columns.Count; i++)
                        {
                            object value = row[i];
                            string stringValue = value?.ToString() ?? "";

                            if (stringValue.Contains(",") || stringValue.Contains("\"") || stringValue.Contains("\n"))
                            {
                                stringValue = "\"" + stringValue.Replace("\"", "\"\"") + "\"";
                            }

                            writer.Write(stringValue);
                            if (i < selectedTable.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                }

                Console.WriteLine($"   ✓ Экспортировано {selectedTable.Rows.Count} записей с {columns.Length} колонками");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Ошибка: {ex.Message}");
            }
        }

        static void BatchExport(DataView view, string baseFileName)
        {
            string[] formats = { "csv", "xml", "json" };

            foreach (string format in formats)
            {
                string fileName = $"{baseFileName}.{format}";

                switch (format)
                {
                    case "csv":
                        ExportToCSV(view, fileName);
                        break;
                    case "xml":
                        ExportToXML(view, fileName);
                        break;
                    case "json":
                        ExportToJSON(view, fileName);
                        break;
                }

                Console.WriteLine($"   • {format.ToUpper()}: {fileName}");
            }

            Console.WriteLine("   ✓ Пакетный экспорт завершён");
        }
        #endregion
        #region Задание 21: Использование DataView для создания иерархических представлений
        static void Task21_HierarchicalViews()
        {
            Console.WriteLine("ЗАДАНИЕ 21: Использование DataView для создания иерархических представлений\n");
            Console.WriteLine("Цель: Демонстрация иерархической структуры данных\n");

            // 1. Создание таблиц с иерархией
            DataTable categories = CreateCategoriesTable();
            DataTable products = CreateProductsTableForHierarchy();

            FillCategoriesData(categories);
            FillProductsDataForHierarchy(products, categories);

            Console.WriteLine("1. Созданы таблицы категорий и товаров с иерархией:");
            Console.WriteLine($"   Категорий: {categories.Rows.Count}");
            Console.WriteLine($"   Товаров: {products.Rows.Count}");

            // 2. Создание DataView для каждого уровня категорий
            Console.WriteLine("\n2. Создание DataView для каждого уровня категорий:");

            // DataView для корневых категорий (ParentCategoryID = 0 или NULL)
            DataView rootCategoriesView = new DataView(categories);
            rootCategoriesView.RowFilter = "ParentCategoryID IS NULL OR ParentCategoryID = 0";
            rootCategoriesView.Sort = "CategoryName ASC";

            Console.WriteLine($"   Корневых категорий: {rootCategoriesView.Count}");
            foreach (DataRowView category in rootCategoriesView)
            {
                Console.WriteLine($"   • {category["CategoryName"]}");
            }

            // 3. Создание DataView для товаров в конкретной категории
            Console.WriteLine("\n3. DataView для товаров в категории 'Электроника':");

            DataView electronicsProducts = new DataView(products);
            electronicsProducts.RowFilter = "CategoryID IN (SELECT CategoryID FROM Categories WHERE CategoryName = 'Электроника')";
            electronicsProducts.Sort = "ProductName ASC";

            Console.WriteLine($"   Товаров в электронике: {electronicsProducts.Count}");
            PrintProductsSample(electronicsProducts, 3);

            // 4. Навигация по иерархии
            Console.WriteLine("\n4. Навигация по иерархии:");

            // Получение родительской категории
            Console.WriteLine("   а) Получение родительской категории:");
            int childCategoryId = 5; // Смартфоны
            DataRow childCategory = categories.Rows.Find(childCategoryId);
            if (childCategory != null)
            {
                int? parentId = childCategory["ParentCategoryID"] as int?;
                if (parentId.HasValue)
                {
                    DataRow parentCategory = categories.Rows.Find(parentId.Value);
                    Console.WriteLine($"      Категория '{childCategory["CategoryName"]}' → родитель: '{parentCategory?["CategoryName"]}'");
                }
            }

            // Получение подкатегорий
            Console.WriteLine("\n   б) Получение подкатегорий:");
            int parentCategoryId = 1; // Электроника
            DataView subcategoriesView = new DataView(categories);
            subcategoriesView.RowFilter = $"ParentCategoryID = {parentCategoryId}";
            Console.WriteLine($"      Подкатегории электроники: {subcategoriesView.Count}");
            foreach (DataRowView subcategory in subcategoriesView)
            {
                Console.WriteLine($"      • {subcategory["CategoryName"]}");
            }

            // 5. Получение всех товаров категории и её подкатегорий
            Console.WriteLine("\n5. Получение всех товаров категории и её подкатегорий:");
            List<int> allCategoryIds = GetAllSubcategoryIds(categories, parentCategoryId);
            string categoryFilter = string.Join(",", allCategoryIds);

            DataView allElectronicsProducts = new DataView(products);
            allElectronicsProducts.RowFilter = $"CategoryID IN ({categoryFilter})";
            Console.WriteLine($"   Всего товаров в электронике и подкатегориях: {allElectronicsProducts.Count}");

            // 6. Вывод иерархии в виде дерева
            Console.WriteLine("\n6. Иерархия в виде дерева:");
            PrintCategoryTree(categories, null, 0);

            // 7. Поиск товара во всей иерархии
            Console.WriteLine("\n7. Поиск товара 'iPhone' во всей иерархии:");
            SearchProductInHierarchy(products, categories, "iPhone");

            // 8. Функция развёртывания/свёртывания категорий
            Console.WriteLine("\n8. Работа с развёрнутым/свёрнутым деревом:");
            var expandedCategories = new HashSet<int> { 1, 2 }; // Электроника и Одежда развёрнуты
            PrintCategoryTreeWithExpansion(categories, null, 0, expandedCategories);

            // 9. Создание сводного DataView
            Console.WriteLine("\n9. Сводный DataView с информацией о категориях:");
            CreatePivotDataView(products, categories);
        }

        static DataTable CreateCategoriesTable()
        {
            DataTable table = new DataTable("Категории");

            table.Columns.Add("CategoryID", typeof(int));
            table.Columns.Add("CategoryName", typeof(string));
            table.Columns.Add("ParentCategoryID", typeof(int));
            table.Columns.Add("Description", typeof(string));

            table.PrimaryKey = new DataColumn[] { table.Columns["CategoryID"] };

            return table;
        }

        static DataTable CreateProductsTableForHierarchy()
        {
            DataTable table = new DataTable("Товары");

            table.Columns.Add("ProductID", typeof(int));
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("CategoryID", typeof(int));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("InStock", typeof(bool));

            table.PrimaryKey = new DataColumn[] { table.Columns["ProductID"] };

            return table;
        }

        static void FillCategoriesData(DataTable table)
        {
            // Иерархия категорий:
            // - Электроника (ID: 1)
            //   - Ноутбуки (ID: 2, Parent: 1)
            //   - Смартфоны (ID: 5, Parent: 1)
            //   - Планшеты (ID: 6, Parent: 1)
            // - Одежда (ID: 3)
            //   - Мужская (ID: 7, Parent: 3)
            //   - Женская (ID: 8, Parent: 3)
            // - Книги (ID: 4)
            //   - Художественные (ID: 9, Parent: 4)
            //   - Технические (ID: 10, Parent: 4)

            table.Rows.Add(1, "Электроника", null, "Электронные устройства");
            table.Rows.Add(2, "Ноутбуки", 1, "Портативные компьютеры");
            table.Rows.Add(3, "Одежда", null, "Одежда и аксессуары");
            table.Rows.Add(4, "Книги", null, "Печатные издания");
            table.Rows.Add(5, "Смартфоны", 1, "Мобильные телефоны");
            table.Rows.Add(6, "Планшеты", 1, "Планшетные компьютеры");
            table.Rows.Add(7, "Мужская одежда", 3, "Одежда для мужчин");
            table.Rows.Add(8, "Женская одежда", 3, "Одежда для женщин");
            table.Rows.Add(9, "Художественная литература", 4, "Романы, рассказы");
            table.Rows.Add(10, "Техническая литература", 4, "Учебники, справочники");
        }

        static void FillProductsDataForHierarchy(DataTable products, DataTable categories)
        {
            Random rand = new Random();
            int productId = 1;

            // Товары для каждой категории
            foreach (DataRow category in categories.Rows)
            {
                int categoryId = (int)category["CategoryID"];
                string categoryName = category["CategoryName"].ToString();
                int productCount = rand.Next(3, 8);

                for (int i = 1; i <= productCount; i++)
                {
                    string productName = $"{categoryName} товар {i}";

                    // Специальные названия для некоторых категорий
                    if (categoryName == "Смартфоны")
                    {
                        string[] phoneNames = { "iPhone", "Samsung Galaxy", "Xiaomi", "Huawei", "Google Pixel" };
                        productName = $"{phoneNames[rand.Next(phoneNames.Length)]} {i}";
                    }
                    else if (categoryName == "Ноутбуки")
                    {
                        string[] laptopNames = { "MacBook", "Dell XPS", "Lenovo ThinkPad", "ASUS ROG", "HP Pavilion" };
                        productName = $"{laptopNames[rand.Next(laptopNames.Length)]} {i}";
                    }

                    products.Rows.Add(
                        productId++,
                        productName,
                        categoryId,
                        Math.Round((decimal)rand.NextDouble() * 100000, 2),
                        rand.NextDouble() > 0.2
                    );
                }
            }
        }

        static void PrintProductsSample(DataView view, int maxRows)
        {
            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"   • {view[i]["ProductName"]} - {view[i]["Price"]:C}");
            }
            if (view.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {view.Count - maxRows} товаров");
            }
        }

        static List<int> GetAllSubcategoryIds(DataTable categories, int parentId)
        {
            var result = new List<int> { parentId };

            // Рекурсивно находим все подкатегории
            var subcategories = categories.AsEnumerable()
                .Where(row => row.Field<int?>("ParentCategoryID") == parentId)
                .Select(row => row.Field<int>("CategoryID"))
                .ToList();

            foreach (int subId in subcategories)
            {
                result.AddRange(GetAllSubcategoryIds(categories, subId));
            }

            return result;
        }

        static void PrintCategoryTree(DataTable categories, int? parentId, int level)
        {
            var children = categories.AsEnumerable()
                .Where(row => row.Field<int?>("ParentCategoryID") == parentId)
                .OrderBy(row => row.Field<string>("CategoryName"));

            foreach (DataRow category in children)
            {
                string indent = new string(' ', level * 2);
                Console.WriteLine($"{indent}• {category["CategoryName"]} (ID: {category["CategoryID"]})");

                PrintCategoryTree(categories, category.Field<int>("CategoryID"), level + 1);
            }
        }

        static void SearchProductInHierarchy(DataTable products, DataTable categories, string searchTerm)
        {
            DataView searchView = new DataView(products);
            searchView.RowFilter = $"ProductName LIKE '%{searchTerm}%'";

            if (searchView.Count > 0)
            {
                Console.WriteLine($"   Найдено товаров: {searchView.Count}");

                foreach (DataRowView product in searchView)
                {
                    int categoryId = (int)product["CategoryID"];
                    DataRow category = categories.Rows.Find(categoryId);
                    string categoryName = category?["CategoryName"].ToString() ?? "Неизвестно";

                    // Находим полный путь категории
                    string categoryPath = GetCategoryPath(categories, categoryId);

                    Console.WriteLine($"   • {product["ProductName"]} - {categoryPath}");
                }
            }
            else
            {
                Console.WriteLine("   Товары не найдены");
            }
        }

        static string GetCategoryPath(DataTable categories, int categoryId)
        {
            var path = new List<string>();
            int? currentId = categoryId;

            while (currentId.HasValue)
            {
                DataRow category = categories.Rows.Find(currentId.Value);
                if (category == null) break;

                path.Insert(0, category["CategoryName"].ToString());
                currentId = category["ParentCategoryID"] as int?;
            }

            return string.Join(" → ", path);
        }

        static void PrintCategoryTreeWithExpansion(DataTable categories, int? parentId, int level, HashSet<int> expandedCategories)
        {
            var children = categories.AsEnumerable()
                .Where(row => row.Field<int?>("ParentCategoryID") == parentId)
                .OrderBy(row => row.Field<string>("CategoryName"));

            foreach (DataRow category in children)
            {
                int categoryId = category.Field<int>("CategoryID");
                string indent = new string(' ', level * 2);
                string marker = expandedCategories.Contains(categoryId) ? "[-]" : "[+]";

                Console.WriteLine($"{indent}{marker} {category["CategoryName"]} (ID: {categoryId})");

                if (expandedCategories.Contains(categoryId))
                {
                    PrintCategoryTreeWithExpansion(categories, categoryId, level + 1, expandedCategories);
                }
            }
        }

        static void CreatePivotDataView(DataTable products, DataTable categories)
        {
            // Создаем сводную таблицу: Категория -> Количество товаров -> Средняя цена
            var pivotData = products.AsEnumerable()
                .Join(categories.AsEnumerable(),
                      product => product.Field<int>("CategoryID"),
                      category => category.Field<int>("CategoryID"),
                      (product, category) => new
                      {
                          CategoryName = category.Field<string>("CategoryName"),
                          Price = product.Field<decimal>("Price"),
                          InStock = product.Field<bool>("InStock")
                      })
                .GroupBy(x => x.CategoryName)
                .Select(g => new
                {
                    Category = g.Key,
                    ProductCount = g.Count(),
                    AvgPrice = g.Average(x => x.Price),
                    InStockCount = g.Count(x => x.InStock)
                })
                .OrderBy(x => x.Category)
                .ToList();

            Console.WriteLine($"{"Категория",-25} {"Товаров",-10} {"В наличии",-10} {"Ср. цена",-12}");
            Console.WriteLine(new string('-', 60));

            foreach (var item in pivotData)
            {
                Console.WriteLine($"{item.Category,-25} {item.ProductCount,-10} {item.InStockCount,-10} {item.AvgPrice,12:C}");
            }
        }
        #endregion

        #region Задание 22: Комплексная фильтрация с использованием LIKE и других операторов
        static void Task22_AdvancedFiltering()
        {
            Console.WriteLine("ЗАДАНИЕ 22: Комплексная фильтрация с использованием LIKE и других операторов\n");
            Console.WriteLine("Цель: Демонстрация продвинутого поиска\n");

            // 1. Создание таблицы документов
            DataTable documents = CreateDocumentsTable();
            FillDocumentsData(documents, 1000);

            Console.WriteLine("1. Создана таблица документов (1000 записей):");
            PrintDocumentsSample(documents, 5);

            // 2. Создание DataView для поиска
            DataView searchView = new DataView(documents);

            // 3. Поиск с использованием различных операторов
            Console.WriteLine("\n2. Поиск с различными операторами:");

            // LIKE для частичного совпадения текста
            Console.WriteLine("\n   а) LIKE: Документы содержащие 'отчёт' в названии:");
            searchView.RowFilter = "Title LIKE '%отчёт%'";
            Console.WriteLine($"      Найдено: {searchView.Count}");
            PrintDocumentsFromView(searchView, 3);

            // BETWEEN для диапазонов дат
            Console.WriteLine("\n   б) BETWEEN: Документы за январь 2024:");
            searchView.RowFilter = "Date BETWEEN '2024-01-01' AND '2024-01-31'";
            Console.WriteLine($"      Найдено: {searchView.Count}");

            // IN для списка значений
            Console.WriteLine("\n   в) IN: Документы типов 'Заявление' или 'Договор':");
            searchView.RowFilter = "Type IN ('Заявление', 'Договор')";
            Console.WriteLine($"      Найдено: {searchView.Count}");

            // Комбинированные условия с AND
            Console.WriteLine("\n   г) AND: Черновики с 2024 года:");
            searchView.RowFilter = "Status = 'Черновик' AND Date >= '2024-01-01'";
            Console.WriteLine($"      Найдено: {searchView.Count}");

            // Комбинированные условия с OR
            Console.WriteLine("\n   д) OR: Завершённые или отклонённые документы:");
            searchView.RowFilter = "Status = 'Завершён' OR Status = 'Отклонён'";
            Console.WriteLine($"      Найдено: {searchView.Count}");

            // NOT для исключения
            Console.WriteLine("\n   е) NOT: Документы не в статусе 'Черновик':");
            searchView.RowFilter = "NOT (Status = 'Черновик')";
            Console.WriteLine($"      Найдено: {searchView.Count}");

            // Комплексное условие
            Console.WriteLine("\n   ж) Комплексное: Отчёты за 2024, не черновики, содержащие 'годовой':");
            searchView.RowFilter = "Type = 'Отчёт' AND Date >= '2024-01-01' AND NOT (Status = 'Черновик') AND Title LIKE '%годовой%'";
            Console.WriteLine($"      Найдено: {searchView.Count}");
            if (searchView.Count > 0)
            {
                PrintDocumentsFromView(searchView, searchView.Count);
            }

            // 4. Поиск с учётом регистра (эмуляция)
            Console.WriteLine("\n3. Поиск с учётом регистра (эмуляция):");
            SearchCaseSensitive(documents, "ОТЧЁТ");

            // 5. Динамическое построение фильтра
            Console.WriteLine("\n4. Динамическое построение фильтра:");

            var searchParams = new DocumentSearchParams
            {
                TitleContains = "проект",
                MinDate = new DateTime(2024, 1, 1),
                MaxDate = new DateTime(2024, 12, 31),
                Types = new[] { "Отчёт", "Документ" },
                Statuses = new[] { "Завершён" },
                AuthorContains = "иванов"
            };

            string dynamicFilter = BuildDocumentFilter(searchParams);
            Console.WriteLine($"   Сгенерированный фильтр: {dynamicFilter}");

            searchView.RowFilter = dynamicFilter;
            Console.WriteLine($"   Найдено документов: {searchView.Count}");

            // 6. Автодополнение при вводе
            Console.WriteLine("\n5. Автодополнение при вводе:");
            TestAutoComplete(documents, "отч");

            // 7. Сравнение производительности различных фильтров
            Console.WriteLine("\n6. Производительность различных фильтров:");
            CompareFilterPerformance(documents);

            // 8. Отчёт о сложных поисках
            Console.WriteLine("\n7. Отчёт о поисковых запросах:");
            GenerateSearchReport(documents);
        }

        static DataTable CreateDocumentsTable()
        {
            DataTable table = new DataTable("Документы");

            table.Columns.Add("DocumentID", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Content", typeof(string));
            table.Columns.Add("Author", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Status", typeof(string));

            return table;
        }

        static void FillDocumentsData(DataTable table, int count)
        {
            string[] types = { "Отчёт", "Заявление", "Договор", "Приказ", "Письмо", "Документ", "Проект" };
            string[] statuses = { "Черновик", "На проверке", "Завершён", "Отклонён", "В работе" };
            string[] authors = { "Иванов И.И.", "Петров П.П.", "Сидорова С.С.", "Кузнецов К.К.", "Смирнова М.М." };
            string[] titles = { "Годовой отчёт", "Отчёт о проделанной работе", "Заявление на отпуск",
                       "Договор оказания услуг", "Приказ о назначении", "Письмо-подтверждение",
                       "Техническое задание", "Проект документации", "Аналитический отчёт" };
            string[] contentWords = { "данный", "документ", "содержит", "информацию", "относительно",
                             "выполнения", "работ", "в рамках", "проекта", "необходимо",
                             "предоставить", "отчётность", "согласно", "требованиям" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2023, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                // Генерируем случайный контент
                string content = string.Join(" ", Enumerable.Range(0, rand.Next(10, 30))
                    .Select(_ => contentWords[rand.Next(contentWords.Length)]));

                table.Rows.Add(
                    i,
                    $"{titles[rand.Next(titles.Length)]} {i % 100}",
                    content,
                    authors[rand.Next(authors.Length)],
                    startDate.AddDays(rand.Next(0, 730)), // 2 года
                    types[rand.Next(types.Length)],
                    statuses[rand.Next(statuses.Length)]
                );
            }
        }

        static void PrintDocumentsSample(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-4} {"Название",-30} {"Автор",-15} {"Дата",-12} {"Тип",-10} {"Статус",-12}");
            Console.WriteLine(new string('-', 85));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["DocumentID"],-4} " +
                                $"{table.Rows[i]["Title"],-30} " +
                                $"{table.Rows[i]["Author"],-15} " +
                                $"{((DateTime)table.Rows[i]["Date"]):dd.MM.yyyy} " +
                                $"{table.Rows[i]["Type"],-10} " +
                                $"{table.Rows[i]["Status"],-12}");
            }
        }

        static void PrintDocumentsFromView(DataView view, int maxRows)
        {
            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"      • {view[i]["Title"]} ({view[i]["Date"]:dd.MM.yyyy}) - {view[i]["Author"]}");
            }
            if (view.Count > maxRows)
            {
                Console.WriteLine($"      ... и ещё {view.Count - maxRows} документов");
            }
        }

        static void SearchCaseSensitive(DataTable table, string searchTerm)
        {
            // Эмуляция поиска с учётом регистра через LINQ
            var results = table.AsEnumerable()
                .Where(row => row.Field<string>("Title").IndexOf(searchTerm, StringComparison.Ordinal) >= 0)
                .ToList();

            Console.WriteLine($"   Найдено с учётом регистра '{searchTerm}': {results.Count}");

            if (results.Count > 0)
            {
                Console.WriteLine("   Примеры:");
                foreach (var row in results.Take(3))
                {
                    Console.WriteLine($"   • {row["Title"]}");
                }
            }
        }

        class DocumentSearchParams
        {
            public string TitleContains { get; set; }
            public string ContentContains { get; set; }
            public string AuthorContains { get; set; }
            public DateTime? MinDate { get; set; }
            public DateTime? MaxDate { get; set; }
            public string[] Types { get; set; }
            public string[] Statuses { get; set; }
        }

        static string BuildDocumentFilter(DocumentSearchParams parameters)
        {
            var conditions = new List<string>();

            if (!string.IsNullOrEmpty(parameters.TitleContains))
                conditions.Add($"Title LIKE '%{parameters.TitleContains}%'");

            if (!string.IsNullOrEmpty(parameters.ContentContains))
                conditions.Add($"Content LIKE '%{parameters.ContentContains}%'");

            if (!string.IsNullOrEmpty(parameters.AuthorContains))
                conditions.Add($"Author LIKE '%{parameters.AuthorContains}%'");

            if (parameters.MinDate.HasValue)
                conditions.Add($"Date >= '{parameters.MinDate.Value:yyyy-MM-dd}'");

            if (parameters.MaxDate.HasValue)
                conditions.Add($"Date <= '{parameters.MaxDate.Value:yyyy-MM-dd}'");

            if (parameters.Types != null && parameters.Types.Length > 0)
            {
                string typesList = string.Join(", ", parameters.Types.Select(t => $"'{t}'"));
                conditions.Add($"Type IN ({typesList})");
            }

            if (parameters.Statuses != null && parameters.Statuses.Length > 0)
            {
                string statusesList = string.Join(", ", parameters.Statuses.Select(s => $"'{s}'"));
                conditions.Add($"Status IN ({statusesList})");
            }

            return string.Join(" AND ", conditions);
        }

        static void TestAutoComplete(DataTable table, string prefix)
        {
            var suggestions = table.AsEnumerable()
                .Select(row => row.Field<string>("Title"))
                .Where(title => title.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Distinct()
                .Take(5)
                .ToList();

            Console.WriteLine($"   Предложения для '{prefix}':");
            if (suggestions.Any())
            {
                foreach (string suggestion in suggestions)
                {
                    Console.WriteLine($"   • {suggestion}");
                }
            }
            else
            {
                Console.WriteLine("   Предложений не найдено");
            }
        }

        static void CompareFilterPerformance(DataTable table)
        {
            int iterations = 1000;
            Random rand = new Random();

            Console.WriteLine($"{"Фильтр",-40} {"Время (мс)",-15} {"Операций/сек",-15}");
            Console.WriteLine(new string('-', 70));

            // Простой фильтр
            Stopwatch swSimple = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                DataView view = new DataView(table);
                view.RowFilter = "Status = 'Завершён'";
                int count = view.Count;
            }
            swSimple.Stop();

            // Фильтр с LIKE
            Stopwatch swLike = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                DataView view = new DataView(table);
                view.RowFilter = "Title LIKE '%отчёт%'";
                int count = view.Count;
            }
            swLike.Stop();

            // Фильтр с AND
            Stopwatch swAnd = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                DataView view = new DataView(table);
                view.RowFilter = "Status = 'Завершён' AND Type = 'Отчёт'";
                int count = view.Count;
            }
            swAnd.Stop();

            // Комплексный фильтр
            Stopwatch swComplex = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                DataView view = new DataView(table);
                view.RowFilter = "Status = 'Завершён' AND Type = 'Отчёт' AND Date >= '2024-01-01' AND Title LIKE '%годовой%'";
                int count = view.Count;
            }
            swComplex.Stop();

            Console.WriteLine($"{"Status = 'Завершён'",-40} {swSimple.ElapsedMilliseconds,-15} {iterations * 1000 / swSimple.ElapsedMilliseconds,-15:F0}");
            Console.WriteLine($"{"Title LIKE '%отчёт%'",-40} {swLike.ElapsedMilliseconds,-15} {iterations * 1000 / swLike.ElapsedMilliseconds,-15:F0}");
            Console.WriteLine($"{"Status = 'Завершён' AND Type = 'Отчёт'",-40} {swAnd.ElapsedMilliseconds,-15} {iterations * 1000 / swAnd.ElapsedMilliseconds,-15:F0}");
            Console.WriteLine($"{"Комплексный фильтр",-40} {swComplex.ElapsedMilliseconds,-15} {iterations * 1000 / swComplex.ElapsedMilliseconds,-15:F0}");
        }

        static void GenerateSearchReport(DataTable table)
        {
            Console.WriteLine("   Статистика по типам документов:");

            var typeStats = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("Type"))
                .Select(g => new
                {
                    Type = g.Key,
                    Count = g.Count(),
                    AvgTitleLength = g.Average(row => row.Field<string>("Title").Length)
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            foreach (var stat in typeStats)
            {
                Console.WriteLine($"   • {stat.Type}: {stat.Count} документов (ср. длина названия: {stat.AvgTitleLength:F0} символов)");
            }

            Console.WriteLine("\n   Статистика по статусам:");

            var statusStats = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("Status"))
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Percentage = (double)g.Count() / table.Rows.Count * 100
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            foreach (var stat in statusStats)
            {
                Console.WriteLine($"   • {stat.Status}: {stat.Count} ({stat.Percentage:F1}%)");
            }

            // Самые частые слова в названиях
            Console.WriteLine("\n   Самые частые слова в названиях:");

            var allWords = table.AsEnumerable()
                .SelectMany(row => row.Field<string>("Title").Split(' ', '.', ',', '!', '?'))
                .Where(word => word.Length > 3)
                .GroupBy(word => word.ToLower())
                .Select(g => new { Word = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToList();

            foreach (var word in allWords)
            {
                Console.WriteLine($"   • '{word.Word}': {word.Count} раз");
            }
        }
        #endregion

        #region Задание 23: Использование DataView для создания отчётов с группировкой
        static void Task23_GroupingReports()
        {
            Console.WriteLine("ЗАДАНИЕ 23: Использование DataView для создания отчётов с группировкой\n");
            Console.WriteLine("Цель: Демонстрация создания отчётов с группировкой данных\n");

            // 1. Создание таблицы заказов
            DataTable orders = CreateOrdersTableForGrouping();
            FillOrdersDataForGrouping(orders, 1000);

            Console.WriteLine("1. Создана таблица заказов (1000 записей):");
            PrintOrdersSample(orders, 5);

            // 2. Группировка по регионам
            Console.WriteLine("\n2. Группировка по регионам:");
            DataView ordersView = new DataView(orders);
            var regionGroups = GroupByRegion(ordersView);
            PrintRegionReport(regionGroups);

            // 3. Группировка по статусу заказа
            Console.WriteLine("\n3. Группировка по статусу заказа:");
            var statusGroups = GroupByStatus(ordersView);
            PrintStatusReport(statusGroups);

            // 4. Группировка по месяцам
            Console.WriteLine("\n4. Группировка по месяцам:");
            var monthGroups = GroupByMonth(ordersView);
            PrintMonthlyReport(monthGroups);

            // 5. Многоуровневая группировка
            Console.WriteLine("\n5. Многоуровневая группировка (регион → месяц → статус):");
            var multiLevelGroups = GroupMultiLevel(ordersView);
            PrintMultiLevelReport(multiLevelGroups);

            // 6. Создание таблицы с результатами группировки
            Console.WriteLine("\n6. Таблица с результатами группировки:");
            DataTable groupedTable = CreateGroupedTable(regionGroups);
            PrintGroupedTable(groupedTable);

            // 7. Отчёт с индентацией
            Console.WriteLine("\n7. Отчёт с индентацией:");
            PrintIndentedReport(multiLevelGroups);

            // 8. Фильтрация отчёта
            Console.WriteLine("\n8. Фильтрованный отчёт (только успешные заказы):");
            DataView filteredView = new DataView(orders);
            filteredView.RowFilter = "Status = 'Delivered'";
            var filteredGroups = GroupByRegion(filteredView);
            PrintRegionReport(filteredGroups);

            // 9. Экспорт отчёта
            Console.WriteLine("\n9. Экспорт отчёта в файл:");
            ExportGroupedReport(regionGroups, "region_report.txt");

            // 10. Итоговые значения
            Console.WriteLine("\n10. Итоговые значения:");
            PrintSummaryStatistics(ordersView);
        }

        static DataTable CreateOrdersTableForGrouping()
        {
            DataTable table = new DataTable("Заказы");

            table.Columns.Add("OrderID", typeof(int));
            table.Columns.Add("CustomerName", typeof(string));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Region", typeof(string));
            table.Columns.Add("Status", typeof(string));

            return table;
        }

        static void FillOrdersDataForGrouping(DataTable table, int count)
        {
            string[] products = { "Ноутбук", "Смартфон", "Планшет", "Монитор", "Принтер", "Клавиатура", "Мышь" };
            string[] regions = { "Северный", "Южный", "Центральный", "Западный", "Восточный" };
            string[] statuses = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled", "Returned" };
            string[] firstNames = { "Иван", "Петр", "Сергей", "Андрей", "Дмитрий", "Алексей" };
            string[] lastNames = { "Иванов", "Петров", "Сидоров", "Смирнов", "Кузнецов", "Попов" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2023, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];

                table.Rows.Add(
                    i,
                    $"{lastName} {firstName}",
                    products[rand.Next(products.Length)],
                    Math.Round((decimal)rand.NextDouble() * 10000, 2),
                    startDate.AddDays(rand.Next(0, 730)),
                    regions[rand.Next(regions.Length)],
                    statuses[rand.Next(statuses.Length)]
                );
            }
        }

        static void PrintOrdersSample(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-5} {"Клиент",-20} {"Товар",-15} {"Сумма",-10} {"Регион",-12} {"Статус",-12}");
            Console.WriteLine(new string('-', 80));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["OrderID"],-5} " +
                                $"{table.Rows[i]["CustomerName"],-20} " +
                                $"{table.Rows[i]["Product"],-15} " +
                                $"{table.Rows[i]["Amount"],10:C} " +
                                $"{table.Rows[i]["Region"],-12} " +
                                $"{table.Rows[i]["Status"],-12}");
            }
        }

        static List<RegionGroup> GroupByRegion(DataView view)
        {
            return view.Cast<DataRowView>()
                .GroupBy(row => row["Region"].ToString())
                .Select(g => new RegionGroup
                {
                    Region = g.Key,
                    OrderCount = g.Count(),
                    TotalAmount = g.Sum(row => (decimal)row["Amount"]),
                    AvgAmount = g.Average(row => (decimal)row["Amount"])
                })
                .OrderByDescending(g => g.TotalAmount)
                .ToList();
        }

        static void PrintRegionReport(List<RegionGroup> groups)
        {
            decimal grandTotal = groups.Sum(g => g.TotalAmount);

            Console.WriteLine($"{"Регион",-15} {"Заказов",-10} {"Сумма",-15} {"Среднее",-15} {"% от общего",-12}");
            Console.WriteLine(new string('-', 70));

            foreach (var group in groups)
            {
                decimal percentage = (group.TotalAmount / grandTotal) * 100;

                Console.WriteLine($"{group.Region,-15} " +
                                $"{group.OrderCount,-10} " +
                                $"{group.TotalAmount,15:C} " +
                                $"{group.AvgAmount,15:C} " +
                                $"{percentage,12:F1}%");
            }

            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"{"Итого:",-15} {groups.Sum(g => g.OrderCount),-10} {grandTotal,15:C}");
        }

        static List<StatusGroup> GroupByStatus(DataView view)
        {
            return view.Cast<DataRowView>()
                .GroupBy(row => row["Status"].ToString())
                .Select(g => new StatusGroup
                {
                    Status = g.Key,
                    OrderCount = g.Count(),
                    TotalAmount = g.Sum(row => (decimal)row["Amount"])
                })
                .OrderByDescending(g => g.OrderCount)
                .ToList();
        }

        static void PrintStatusReport(List<StatusGroup> groups)
        {
            int totalOrders = groups.Sum(g => g.OrderCount);

            Console.WriteLine($"{"Статус",-15} {"Заказов",-10} {"% от общего",-12} {"Сумма",-15}");
            Console.WriteLine(new string('-', 55));

            foreach (var group in groups)
            {
                double percentage = (double)group.OrderCount / totalOrders * 100;

                Console.WriteLine($"{group.Status,-15} " +
                                $"{group.OrderCount,-10} " +
                                $"{percentage,12:F1}% " +
                                $"{group.TotalAmount,15:C}");
            }
        }

        static List<MonthGroup> GroupByMonth(DataView view)
        {
            return view.Cast<DataRowView>()
                .GroupBy(row => new
                {
                    Year = ((DateTime)row["Date"]).Year,
                    Month = ((DateTime)row["Date"]).Month
                })
                .Select(g => new MonthGroup
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    OrderCount = g.Count(),
                    TotalAmount = g.Sum(row => (decimal)row["Amount"]),
                    MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy")
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToList();
        }

        static void PrintMonthlyReport(List<MonthGroup> groups)
        {
            Console.WriteLine($"{"Месяц",-20} {"Заказов",-10} {"Сумма",-15} {"Тренд",-10}");
            Console.WriteLine(new string('-', 60));

            decimal? prevAmount = null;

            foreach (var group in groups)
            {
                string trend = "";
                if (prevAmount.HasValue)
                {
                    decimal change = ((group.TotalAmount - prevAmount.Value) / prevAmount.Value) * 100;
                    trend = change >= 0 ? $"↑ {change:F1}%" : $"↓ {Math.Abs(change):F1}%";
                }

                Console.WriteLine($"{group.MonthName,-20} " +
                                $"{group.OrderCount,-10} " +
                                $"{group.TotalAmount,15:C} " +
                                $"{trend,-10}");

                prevAmount = group.TotalAmount;
            }
        }

        static Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> GroupMultiLevel(DataView view)
        {
            var result = new Dictionary<string, Dictionary<string, Dictionary<string, decimal>>>();

            foreach (DataRowView row in view)
            {
                string region = row["Region"].ToString();
                string month = ((DateTime)row["Date"]).ToString("yyyy-MM");
                string status = row["Status"].ToString();
                decimal amount = (decimal)row["Amount"];

                if (!result.ContainsKey(region))
                    result[region] = new Dictionary<string, Dictionary<string, decimal>>();

                if (!result[region].ContainsKey(month))
                    result[region][month] = new Dictionary<string, decimal>();

                if (!result[region][month].ContainsKey(status))
                    result[region][month][status] = 0;

                result[region][month][status] += amount;
            }

            return result;
        }

        static void PrintMultiLevelReport(Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> data)
        {
            foreach (var region in data.OrderBy(r => r.Key))
            {
                Console.WriteLine($"\nРегион: {region.Key}");
                Console.WriteLine(new string('-', 40));

                decimal regionTotal = 0;

                foreach (var month in region.Value.OrderBy(m => m.Key))
                {
                    Console.WriteLine($"  {month.Key}:");

                    decimal monthTotal = 0;

                    foreach (var status in month.Value.OrderBy(s => s.Key))
                    {
                        Console.WriteLine($"    {status.Key}: {status.Value:C}");
                        monthTotal += status.Value;
                    }

                    Console.WriteLine($"    Итого за месяц: {monthTotal:C}");
                    regionTotal += monthTotal;
                }

                Console.WriteLine($"  Итого по региону: {regionTotal:C}");
            }
        }

        static DataTable CreateGroupedTable(List<RegionGroup> groups)
        {
            DataTable table = new DataTable("Группировка по регионам");

            table.Columns.Add("Region", typeof(string));
            table.Columns.Add("OrderCount", typeof(int));
            table.Columns.Add("TotalAmount", typeof(decimal));
            table.Columns.Add("AvgAmount", typeof(decimal));
            table.Columns.Add("Percentage", typeof(double));

            decimal grandTotal = groups.Sum(g => g.TotalAmount);

            foreach (var group in groups)
            {
                double percentage = (double)group.TotalAmount / (double)grandTotal * 100;

                table.Rows.Add(
                    group.Region,
                    group.OrderCount,
                    group.TotalAmount,
                    group.AvgAmount,
                    percentage
                );
            }

            // Итоговая строка
            table.Rows.Add(
                "Итого",
                groups.Sum(g => g.OrderCount),
                grandTotal,
                grandTotal / groups.Sum(g => g.OrderCount),
                100.0
            );

            return table;
        }

        static void PrintGroupedTable(DataTable table)
        {
            Console.WriteLine($"{"Регион",-15} {"Заказов",-10} {"Сумма",-15} {"Среднее",-15} {"%",-8}");
            Console.WriteLine(new string('-', 65));

            bool isTotalRow = false;

            foreach (DataRow row in table.Rows)
            {
                if (row["Region"].ToString() == "Итого")
                {
                    isTotalRow = true;
                    Console.WriteLine(new string('-', 65));
                }

                Console.WriteLine($"{row["Region"],-15} " +
                                $"{row["OrderCount"],-10} " +
                                $"{((decimal)row["TotalAmount"]),15:C} " +
                                $"{((decimal)row["AvgAmount"]),15:C} " +
                                $"{((double)row["Percentage"]),8:F1}%");

                if (isTotalRow)
                {
                    Console.WriteLine(new string('-', 65));
                }
            }
        }

        static void PrintIndentedReport(Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> data)
        {
            foreach (var region in data.OrderBy(r => r.Key))
            {
                Console.WriteLine($"{region.Key}");

                decimal regionTotal = 0;
                int regionCount = 0;

                foreach (var month in region.Value.OrderBy(m => m.Key))
                {
                    Console.WriteLine($"  ├─ {month.Key}");

                    decimal monthTotal = 0;

                    foreach (var status in month.Value.OrderBy(s => s.Key))
                    {
                        Console.WriteLine($"  │   ├─ {status.Key}: {status.Value:C}");
                        monthTotal += status.Value;
                    }

                    Console.WriteLine($"  │   └─ Итого: {monthTotal:C}");
                    regionTotal += monthTotal;
                    regionCount++;
                }

                Console.WriteLine($"  └─ Всего: {regionTotal:C} ({regionCount} месяцев)");
                Console.WriteLine();
            }
        }

        static void ExportGroupedReport(List<RegionGroup> groups, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("ОТЧЁТ ПО РЕГИОНАМ");
                    writer.WriteLine($"Дата генерации: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    writer.WriteLine(new string('=', 60));

                    decimal grandTotal = groups.Sum(g => g.TotalAmount);

                    foreach (var group in groups)
                    {
                        decimal percentage = (group.TotalAmount / grandTotal) * 100;

                        writer.WriteLine($"Регион: {group.Region}");
                        writer.WriteLine($"  Заказов: {group.OrderCount}");
                        writer.WriteLine($"  Общая сумма: {group.TotalAmount:C}");
                        writer.WriteLine($"  Средний заказ: {group.AvgAmount:C}");
                        writer.WriteLine($"  Доля от общего: {percentage:F1}%");
                        writer.WriteLine();
                    }

                    writer.WriteLine(new string('=', 60));
                    writer.WriteLine($"ИТОГО: {groups.Sum(g => g.OrderCount)} заказов на сумму {grandTotal:C}");
                }

                Console.WriteLine($"   ✓ Отчёт сохранён в {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Ошибка экспорта: {ex.Message}");
            }
        }

        static void PrintSummaryStatistics(DataView view)
        {
            int totalOrders = view.Count;
            decimal totalAmount = 0;
            decimal maxAmount = decimal.MinValue;
            decimal minAmount = decimal.MaxValue;

            foreach (DataRowView row in view)
            {
                decimal amount = (decimal)row["Amount"];
                totalAmount += amount;

                if (amount > maxAmount) maxAmount = amount;
                if (amount < minAmount) minAmount = amount;
            }

            Console.WriteLine($"   Всего заказов: {totalOrders}");
            Console.WriteLine($"   Общая сумма: {totalAmount:C}");
            Console.WriteLine($"   Средний заказ: {totalAmount / totalOrders:C}");
            Console.WriteLine($"   Максимальный заказ: {maxAmount:C}");
            Console.WriteLine($"   Минимальный заказ: {minAmount:C}");
            Console.WriteLine($"   Среднее количество заказов в день: {totalOrders / 730.0:F1}"); // 2 года
        }

        class RegionGroup
        {
            public string Region { get; set; }
            public int OrderCount { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal AvgAmount { get; set; }
        }

        class StatusGroup
        {
            public string Status { get; set; }
            public int OrderCount { get; set; }
            public decimal TotalAmount { get; set; }
        }

        class MonthGroup
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public string MonthName { get; set; }
            public int OrderCount { get; set; }
            public decimal TotalAmount { get; set; }
        }
        #endregion

        #region Задание 24: DataView с вычисляемыми столбцами
        static void Task24_CalculatedColumns()
        {
            Console.WriteLine("ЗАДАНИЕ 24: DataView с вычисляемыми столбцами\n");
            Console.WriteLine("Цель: Демонстрация работы с вычисляемыми данными\n");

            // 1. Создание таблицы деталей заказов
            DataTable orderDetails = CreateOrderDetailsTable();
            FillOrderDetailsData(orderDetails, 200);

            Console.WriteLine("1. Создана таблица деталей заказов (200 записей):");
            PrintOrderDetailsTable(orderDetails, 5);

            // 2. Добавление вычисляемых колонок
            Console.WriteLine("\n2. Добавление вычисляемых колонок:");

            // Колонка Total = Quantity * UnitPrice
            orderDetails.Columns.Add("Total", typeof(decimal), "Quantity * UnitPrice");

            // Колонка DiscountedTotal = Total * (1 - Discount)
            orderDetails.Columns.Add("DiscountedTotal", typeof(decimal), "Total * (1 - Discount)");

            // Колонка DiscountAmount = Total * Discount
            orderDetails.Columns.Add("DiscountAmount", typeof(decimal), "Total * Discount");

            Console.WriteLine("   Добавлены вычисляемые колонки:");
            Console.WriteLine("   • Total = Quantity * UnitPrice");
            Console.WriteLine("   • DiscountedTotal = Total * (1 - Discount)");
            Console.WriteLine("   • DiscountAmount = Total * Discount");

            // 3. Создание DataView
            DataView detailsView = new DataView(orderDetails);
            detailsView.Sort = "OrderID ASC, DiscountedTotal DESC";

            Console.WriteLine($"\n3. DataView создан ({detailsView.Count} записей)");

            // 4. Фильтрация по вычисляемому полю
            Console.WriteLine("\n4. Фильтрация по вычисляемому полю:");

            Console.WriteLine("   а) Записи с DiscountedTotal > 1000:");
            detailsView.RowFilter = "DiscountedTotal > 1000";
            Console.WriteLine($"      Найдено: {detailsView.Count}");
            PrintCalculatedSample(detailsView, 3);

            Console.WriteLine("\n   б) Записи с большой скидкой (Discount > 0.2):");
            detailsView.RowFilter = "Discount > 0.2";
            Console.WriteLine($"      Найдено: {detailsView.Count}");
            PrintCalculatedSample(detailsView, 3);

            // 5. Сортировка по вычисляемому полю
            Console.WriteLine("\n5. Сортировка по вычисляемому полю:");

            Console.WriteLine("   а) По DiscountedTotal (убывание):");
            detailsView.RowFilter = "";
            detailsView.Sort = "DiscountedTotal DESC";
            Console.WriteLine("   Топ-3 самых дорогих со скидкой:");
            for (int i = 0; i < Math.Min(3, detailsView.Count); i++)
            {
                Console.WriteLine($"   • {detailsView[i]["ProductName"]}: {detailsView[i]["DiscountedTotal"]:C} " +
                                 $"(скидка: {((decimal)detailsView[i]["DiscountAmount"]):C})");
            }

            Console.WriteLine("\n   б) По DiscountAmount (возрастание):");
            detailsView.Sort = "DiscountAmount ASC";
            Console.WriteLine("   Топ-3 самых маленьких скидок:");
            for (int i = 0; i < Math.Min(3, detailsView.Count); i++)
            {
                Console.WriteLine($"   • {detailsView[i]["ProductName"]}: скидка {detailsView[i]["DiscountAmount"]:C}");
            }

            // 6. Получение статистики по вычисляемым полям
            Console.WriteLine("\n6. Статистика по вычисляемым полям:");
            CalculateStatistics(detailsView);

            // 7. Сумма вычисляемых значений
            Console.WriteLine("\n7. Суммы вычисляемых значений:");
            CalculateSums(detailsView);

            // 8. Демонстрация обновления вычисляемых полей
            Console.WriteLine("\n8. Обновление вычисляемых полей при изменении данных:");
            DemonstrateCalculatedUpdate(orderDetails);

            // 9. Сравнение производительности
            Console.WriteLine("\n9. Сравнение производительности:");
            CompareCalculatedVsManual(orderDetails);

            // 10. Создание отчёта с вычисляемыми полями
            Console.WriteLine("\n10. Отчёт с вычисляемыми полями:");
            GenerateCalculatedReport(orderDetails);
        }

        static DataTable CreateOrderDetailsTable()
        {
            DataTable table = new DataTable("Детали заказов");

            table.Columns.Add("OrderDetailID", typeof(int));
            table.Columns.Add("OrderID", typeof(int));
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("UnitPrice", typeof(decimal));
            table.Columns.Add("Discount", typeof(decimal)); // от 0 до 1

            table.PrimaryKey = new DataColumn[] { table.Columns["OrderDetailID"] };

            return table;
        }

        static void FillOrderDetailsData(DataTable table, int count)
        {
            string[] products = { "Ноутбук", "Смартфон", "Планшет", "Монитор", "Принтер",
                         "Клавиатура", "Мышь", "Наушники", "Веб-камера", "Микрофон" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                table.Rows.Add(
                    i,
                    rand.Next(1, 51), // 50 заказов
                    products[rand.Next(products.Length)],
                    rand.Next(1, 11), // 1-10 штук
                    Math.Round((decimal)rand.NextDouble() * 50000, 2), // до 50,000
                    Math.Round(rand.NextDouble() * 0.5, 2) // скидка 0-50%
                );
            }
        }

        static void PrintOrderDetailsTable(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-4} {"Заказ",-6} {"Товар",-15} {"Кол-во",-8} {"Цена",-10} {"Скидка",-10}");
            Console.WriteLine(new string('-', 60));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["OrderDetailID"],-4} " +
                                $"{table.Rows[i]["OrderID"],-6} " +
                                $"{table.Rows[i]["ProductName"],-15} " +
                                $"{table.Rows[i]["Quantity"],8} " +
                                $"{table.Rows[i]["UnitPrice"],10:C} " +
                                $"{((decimal)table.Rows[i]["Discount"]),10:P0}");
            }
        }

        static void PrintCalculatedSample(DataView view, int maxRows)
        {
            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"   • {view[i]["ProductName"]}: {view[i]["Quantity"]} × {view[i]["UnitPrice"]:C} = " +
                                 $"{view[i]["Total"]:C} → {view[i]["DiscountedTotal"]:C} " +
                                 $"(скидка: {((decimal)view[i]["Discount"]):P0})");
            }
        }

        static void CalculateStatistics(DataView view)
        {
            view.RowFilter = "";

            decimal maxTotal = decimal.MinValue;
            decimal minTotal = decimal.MaxValue;
            decimal maxDiscounted = decimal.MinValue;
            decimal minDiscounted = decimal.MaxValue;
            decimal maxDiscountAmount = decimal.MinValue;

            foreach (DataRowView row in view)
            {
                decimal total = (decimal)row["Total"];
                decimal discounted = (decimal)row["DiscountedTotal"];
                decimal discountAmount = (decimal)row["DiscountAmount"];

                if (total > maxTotal) maxTotal = total;
                if (total < minTotal) minTotal = total;
                if (discounted > maxDiscounted) maxDiscounted = discounted;
                if (discounted < minDiscounted) minDiscounted = discounted;
                if (discountAmount > maxDiscountAmount) maxDiscountAmount = discountAmount;
            }

            Console.WriteLine($"   Максимальный Total: {maxTotal:C}");
            Console.WriteLine($"   Минимальный Total: {minTotal:C}");
            Console.WriteLine($"   Максимальный DiscountedTotal: {maxDiscounted:C}");
            Console.WriteLine($"   Минимальный DiscountedTotal: {minDiscounted:C}");
            Console.WriteLine($"   Максимальная сумма скидки: {maxDiscountAmount:C}");
        }

        static void CalculateSums(DataView view)
        {
            decimal sumTotal = 0;
            decimal sumDiscounted = 0;
            decimal sumDiscount = 0;
            int count = 0;

            foreach (DataRowView row in view)
            {
                sumTotal += (decimal)row["Total"];
                sumDiscounted += (decimal)row["DiscountedTotal"];
                sumDiscount += (decimal)row["DiscountAmount"];
                count++;
            }

            Console.WriteLine($"   Общая сумма (без скидки): {sumTotal:C}");
            Console.WriteLine($"   Общая сумма (со скидкой): {sumDiscounted:C}");
            Console.WriteLine($"   Общая сумма скидок: {sumDiscount:C}");
            Console.WriteLine($"   Средняя скидка на товар: {sumDiscount / count:C}");
            Console.WriteLine($"   Средний процент скидки: {(sumDiscount / sumTotal) * 100:F1}%");
        }

        static void DemonstrateCalculatedUpdate(DataTable table)
        {
            if (table.Rows.Count > 0)
            {
                DataRow firstRow = table.Rows[0];

                Console.WriteLine("   До изменения:");
                Console.WriteLine($"   • Quantity: {firstRow["Quantity"]}, UnitPrice: {firstRow["UnitPrice"]:C}");
                Console.WriteLine($"   • Total: {firstRow["Total"]:C}, DiscountedTotal: {firstRow["DiscountedTotal"]:C}");

                // Изменяем исходные данные
                firstRow["Quantity"] = (int)firstRow["Quantity"] * 2;
                firstRow["UnitPrice"] = (decimal)firstRow["UnitPrice"] * 1.1m;

                Console.WriteLine("\n   После изменения (×2 количество, +10% цена):");
                Console.WriteLine($"   • Quantity: {firstRow["Quantity"]}, UnitPrice: {firstRow["UnitPrice"]:C}");
                Console.WriteLine($"   • Total: {firstRow["Total"]:C}, DiscountedTotal: {firstRow["DiscountedTotal"]:C}");

                Console.WriteLine("\n   ✓ Вычисляемые поля автоматически обновились");
            }
        }

        static void CompareCalculatedVsManual(DataTable table)
        {
            int iterations = 10000;

            Console.WriteLine($"{"Метод",-30} {"Время (мс)",-15} {"Операций",-10}");
            Console.WriteLine(new string('-', 55));

            // Фильтрация по вычисляемому полю
            Stopwatch swCalculated = Stopwatch.StartNew();
            DataView calculatedView = new DataView(table);
            for (int i = 0; i < iterations; i++)
            {
                calculatedView.RowFilter = "DiscountedTotal > 1000";
                int count = calculatedView.Count;
            }
            swCalculated.Stop();

            // Фильтрация по обычному полю
            Stopwatch swRegular = Stopwatch.StartNew();
            DataView regularView = new DataView(table);
            for (int i = 0; i < iterations; i++)
            {
                regularView.RowFilter = "UnitPrice > 5000";
                int count = regularView.Count;
            }
            swRegular.Stop();

            // Ручной расчет (без вычисляемой колонки)
            Stopwatch swManual = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var results = table.AsEnumerable()
                    .Where(row => row.Field<int>("Quantity") * row.Field<decimal>("UnitPrice") *
                                 (1 - row.Field<decimal>("Discount")) > 1000)
                    .ToList();
                int count = results.Count;
            }
            swManual.Stop();

            Console.WriteLine($"{"Вычисляемое поле",-30} {swCalculated.ElapsedMilliseconds,-15} {iterations,-10}");
            Console.WriteLine($"{"Обычное поле",-30} {swRegular.ElapsedMilliseconds,-15} {iterations,-10}");
            Console.WriteLine($"{"Ручной расчет",-30} {swManual.ElapsedMilliseconds,-15} {iterations,-10}");

            Console.WriteLine($"\n   Вычисляемые поля быстрее ручного расчета в " +
                             $"{(double)swManual.ElapsedMilliseconds / swCalculated.ElapsedMilliseconds:F1} раза");
        }

        static void GenerateCalculatedReport(DataTable table)
        {
            // Группировка по продуктам
            var productStats = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("ProductName"))
                .Select(g => new
                {
                    Product = g.Key,
                    TotalQuantity = g.Sum(row => row.Field<int>("Quantity")),
                    AvgUnitPrice = g.Average(row => row.Field<decimal>("UnitPrice")),
                    TotalRevenue = g.Sum(row => row.Field<decimal>("Total")),
                    TotalDiscounted = g.Sum(row => row.Field<decimal>("DiscountedTotal")),
                    TotalDiscount = g.Sum(row => row.Field<decimal>("DiscountAmount")),
                    AvgDiscount = g.Average(row => row.Field<decimal>("Discount"))
                })
                .OrderByDescending(x => x.TotalDiscounted)
                .ToList();

            Console.WriteLine($"{"Товар",-15} {"Кол-во",-8} {"Ср.цена",-10} {"Выручка",-12} {"Со скидкой",-12} {"Скидка",-10} {"%",-8}");
            Console.WriteLine(new string('-', 80));

            decimal grandTotal = productStats.Sum(p => p.TotalDiscounted);

            foreach (var stat in productStats)
            {
                decimal discountPercentage = (stat.TotalDiscount / stat.TotalRevenue) * 100;
                decimal revenuePercentage = (stat.TotalDiscounted / grandTotal) * 100;

                Console.WriteLine($"{stat.Product,-15} " +
                                $"{stat.TotalQuantity,-8} " +
                                $"{stat.AvgUnitPrice,10:C} " +
                                $"{stat.TotalRevenue,12:C} " +
                                $"{stat.TotalDiscounted,12:C} " +
                                $"{stat.TotalDiscount,10:C} " +
                                $"{revenuePercentage,8:F1}%");
            }

            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"{"Итого:",-15} {productStats.Sum(p => p.TotalQuantity),-8} " +
                            $"{"",10} {productStats.Sum(p => p.TotalRevenue),12:C} " +
                            $"{grandTotal,12:C} {productStats.Sum(p => p.TotalDiscount),10:C}");
        }
        #endregion

        #region Задание 25: Поиск дубликатов с использованием DataView
        static void Task25_FindDuplicates()
        {
            Console.WriteLine("ЗАДАНИЕ 25: Поиск дубликатов с использованием DataView\n");
            Console.WriteLine("Цель: Демонстрация поиска и обработки дубликатов\n");

            // 1. Создание таблицы электронных писем с дубликатами
            DataTable emails = CreateEmailsTable();
            FillEmailsDataWithDuplicates(emails, 500);

            Console.WriteLine("1. Создана таблица электронных писем (500 записей, включая дубликаты):");
            PrintEmailsSample(emails, 8);

            // 2. Поиск точных дубликатов
            Console.WriteLine("\n2. Поиск точных дубликатов (одинаковые email):");
            DataView duplicateEmails = FindExactDuplicates(emails);
            Console.WriteLine($"   Найдено дубликатов: {duplicateEmails.Count}");
            PrintDuplicateGroups(duplicateEmails);

            // 3. Поиск похожих дубликатов
            Console.WriteLine("\n3. Поиск похожих дубликатов:");
            var similarDuplicates = FindSimilarDuplicates(emails);
            Console.WriteLine($"   Найдено групп похожих: {similarDuplicates.Count}");
            PrintSimilarDuplicates(similarDuplicates);

            // 4. Подсчёт количества дубликатов
            Console.WriteLine("\n4. Статистика дубликатов:");
            PrintDuplicateStatistics(emails);

            // 5. Создание DataView только дубликатов
            Console.WriteLine("\n5. DataView только дубликатов:");
            DataView duplicatesOnlyView = CreateDuplicatesOnlyView(emails);
            Console.WriteLine($"   Записей в DataView дубликатов: {duplicatesOnlyView.Count}");
            PrintEmailsFromView(duplicatesOnlyView, 5);

            // 6. Создание DataView только уникальных записей
            Console.WriteLine("\n6. DataView только уникальных записей:");
            DataView uniqueOnlyView = CreateUniqueOnlyView(emails);
            Console.WriteLine($"   Уникальных записей: {uniqueOnlyView.Count}");
            PrintEmailsFromView(uniqueOnlyView, 5);

            // 7. Выбор главной записи при дубликатах
            Console.WriteLine("\n7. Выбор главной записи при дубликатах:");
            var masterRecords = SelectMasterRecords(duplicateEmails);
            Console.WriteLine($"   Выбрано главных записей: {masterRecords.Count}");
            PrintMasterRecords(masterRecords);

            // 8. Объединение дубликатов
            Console.WriteLine("\n8. Объединение дубликатов:");
            DataTable mergedTable = MergeDuplicates(emails);
            Console.WriteLine($"   После объединения: {mergedTable.Rows.Count} записей (было: {emails.Rows.Count})");

            // 9. Отчёт о найденных дубликатах
            Console.WriteLine("\n9. Отчёт о дубликатах:");
            GenerateDuplicateReport(emails);

            // 10. Статистика
            Console.WriteLine("\n10. Итоговая статистика:");
            PrintFinalStatistics(emails, mergedTable);
        }

        static DataTable CreateEmailsTable()
        {
            DataTable table = new DataTable("Электронные письма");

            table.Columns.Add("EmailID", typeof(int));
            table.Columns.Add("EmailAddress", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Domain", typeof(string));
            table.Columns.Add("Duplicates", typeof(int)); // Количество дубликатов

            table.PrimaryKey = new DataColumn[] { table.Columns["EmailID"] };

            return table;
        }

        static void FillEmailsDataWithDuplicates(DataTable table, int count)
        {
            string[] domains = { "gmail.com", "mail.ru", "yandex.ru", "outlook.com", "yahoo.com" };
            string[] firstNames = { "ivan", "alex", "sergey", "dmitry", "andrey", "mikhail" };
            string[] lastNames = { "ivanov", "petrov", "sidorov", "smirnov", "kuznetsov", "popov" };

            Random rand = new Random();

            // Создаем уникальные email'ы
            int uniqueCount = count * 2 / 3; // 2/3 уникальных
            var uniqueEmails = new HashSet<string>();

            for (int i = 1; i <= uniqueCount; i++)
            {
                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];
                string domain = domains[rand.Next(domains.Length)];
                string email = $"{firstName}.{lastName}@{domain}";

                // Добавляем с вероятностью дублирования
                uniqueEmails.Add(email);

                table.Rows.Add(
                    i,
                    email,
                    $"{firstName} {lastName}",
                    domain,
                    0
                );
            }

            // Добавляем дубликаты (1/3 от общего количества)
            for (int i = uniqueCount + 1; i <= count; i++)
            {
                // Выбираем случайный существующий email
                int duplicateIndex = rand.Next(1, uniqueCount + 1);
                DataRow original = table.Rows[duplicateIndex - 1];

                // Создаем слегка изменённую версию (похожий дубликат) или точную копию
                bool isExactDuplicate = rand.NextDouble() > 0.5;

                if (isExactDuplicate)
                {
                    // Точный дубликат
                    table.Rows.Add(
                        i,
                        original["EmailAddress"],
                        original["Name"],
                        original["Domain"],
                        0
                    );
                }
                else
                {
                    // Похожий дубликат (разные домены или опечатки)
                    string email = original["EmailAddress"].ToString();
                    string name = original["Name"].ToString();

                    // Иногда меняем домен
                    if (rand.NextDouble() > 0.7)
                    {
                        string newDomain = domains[rand.Next(domains.Length)];
                        email = email.Substring(0, email.IndexOf('@') + 1) + newDomain;
                    }

                    // Иногда добавляем опечатки
                    if (rand.NextDouble() > 0.8)
                    {
                        name = name.Replace("a", "o").Replace("e", "i");
                    }

                    table.Rows.Add(
                        i,
                        email,
                        name,
                        email.Substring(email.IndexOf('@') + 1),
                        0
                    );
                }
            }
        }

        static void PrintEmailsSample(DataTable table, int maxRows)
        {
            Console.WriteLine($"{"ID",-4} {"Email",-30} {"Имя",-20} {"Домен",-15}");
            Console.WriteLine(new string('-', 75));

            for (int i = 0; i < Math.Min(maxRows, table.Rows.Count); i++)
            {
                Console.WriteLine($"{table.Rows[i]["EmailID"],-4} " +
                                $"{table.Rows[i]["EmailAddress"],-30} " +
                                $"{table.Rows[i]["Name"],-20} " +
                                $"{table.Rows[i]["Domain"],-15}");
            }
        }

        static DataView FindExactDuplicates(DataTable table)
        {
            // Находим email'ы, которые встречаются более одного раза
            var duplicateGroups = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("EmailAddress").ToLower())
                .Where(g => g.Count() > 1)
                .ToList();

            // Создаем DataView с дубликатами
            DataView duplicateView = new DataView(table);

            if (duplicateGroups.Any())
            {
                // Строим фильтр для всех дубликатов
                var duplicateEmails = duplicateGroups.Select(g => g.Key);
                string filter = string.Join(" OR ", duplicateEmails.Select(email => $"EmailAddress = '{email}'"));
                duplicateView.RowFilter = filter;
                duplicateView.Sort = "EmailAddress ASC, EmailID ASC";
            }
            else
            {
                duplicateView.RowFilter = "1 = 0"; // Пустой результат
            }

            return duplicateView;
        }

        static void PrintDuplicateGroups(DataView duplicateView)
        {
            if (duplicateView.Count == 0)
            {
                Console.WriteLine("   Дубликатов не найдено");
                return;
            }

            string currentEmail = "";
            int groupCount = 0;
            int duplicateCount = 0;

            for (int i = 0; i < duplicateView.Count; i++)
            {
                string email = duplicateView[i]["EmailAddress"].ToString();

                if (email != currentEmail)
                {
                    if (!string.IsNullOrEmpty(currentEmail))
                    {
                        Console.WriteLine($"      Всего: {duplicateCount} дубликатов");
                    }

                    currentEmail = email;
                    duplicateCount = 1;
                    groupCount++;

                    Console.WriteLine($"\n   Группа {groupCount}: {email}");
                    Console.WriteLine($"      • ID {duplicateView[i]["EmailID"]}: {duplicateView[i]["Name"]}");
                }
                else
                {
                    duplicateCount++;
                    Console.WriteLine($"      • ID {duplicateView[i]["EmailID"]}: {duplicateView[i]["Name"]}");
                }
            }

            if (duplicateCount > 0)
            {
                Console.WriteLine($"      Всего: {duplicateCount} дубликатов");
            }
        }

        static List<List<DataRow>> FindSimilarDuplicates(DataTable table)
        {
            var groups = new List<List<DataRow>>();
            var processed = new HashSet<int>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (processed.Contains(i)) continue;

                DataRow row1 = table.Rows[i];
                string email1 = row1["EmailAddress"].ToString().ToLower();
                string name1 = row1["Name"].ToString().ToLower();

                var similarGroup = new List<DataRow> { row1 };
                processed.Add(i);

                // Ищем похожие записи
                for (int j = i + 1; j < table.Rows.Count; j++)
                {
                    if (processed.Contains(j)) continue;

                    DataRow row2 = table.Rows[j];
                    string email2 = row2["EmailAddress"].ToString().ToLower();
                    string name2 = row2["Name"].ToString().ToLower();

                    // Проверяем сходство
                    bool isSimilar = false;

                    // 1. Похожие email (разные домены, но одинаковые имена)
                    if (GetEmailName(email1) == GetEmailName(email2) &&
                        GetDomain(email1) != GetDomain(email2))
                    {
                        isSimilar = true;
                    }
                    // 2. Похожие имена (разные email)
                    else if (AreNamesSimilar(name1, name2) && email1 != email2)
                    {
                        isSimilar = true;
                    }
                    // 3. Email отличаются на 1-2 символа (опечатки)
                    else if (CalculateLevenshteinDistance(email1, email2) <= 2)
                    {
                        isSimilar = true;
                    }

                    if (isSimilar)
                    {
                        similarGroup.Add(row2);
                        processed.Add(j);
                    }
                }

                if (similarGroup.Count > 1)
                {
                    groups.Add(similarGroup);
                }
            }

            return groups;
        }

        static string GetEmailName(string email)
        {
            int atIndex = email.IndexOf('@');
            return atIndex > 0 ? email.Substring(0, atIndex) : email;
        }

        static string GetDomain(string email)
        {
            int atIndex = email.IndexOf('@');
            return atIndex > 0 ? email.Substring(atIndex + 1) : "";
        }

        static bool AreNamesSimilar(string name1, string name2)
        {
            // Простая проверка сходства имён
            var words1 = name1.Split(' ');
            var words2 = name2.Split(' ');

            if (words1.Length != words2.Length) return false;

            for (int i = 0; i < words1.Length; i++)
            {
                if (words1[i].Length > 0 && words2[i].Length > 0)
                {
                    if (words1[i][0] != words2[i][0]) return false;
                }
            }

            return true;
        }

        static int CalculateLevenshteinDistance(string s, string t)
        {
            // Алгоритм Левенштейна для определения расстояния между строками
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }

        static void PrintSimilarDuplicates(List<List<DataRow>> groups)
        {
            if (groups.Count == 0)
            {
                Console.WriteLine("   Похожих дубликатов не найдено");
                return;
            }

            for (int g = 0; g < groups.Count; g++)
            {
                var group = groups[g];
                Console.WriteLine($"\n   Группа {g + 1} ({group.Count} похожих записей):");

                foreach (var row in group)
                {
                    Console.WriteLine($"      • ID {row["EmailID"]}: {row["EmailAddress"]} ({row["Name"]})");
                }
            }
        }

        static void PrintDuplicateStatistics(DataTable table)
        {
            // Точные дубликаты
            var exactDuplicates = FindExactDuplicates(table);
            int exactDuplicateCount = exactDuplicates.Count;

            // Похожие дубликаты
            var similarGroups = FindSimilarDuplicates(table);
            int similarDuplicateCount = similarGroups.Sum(g => g.Count);

            // Уникальные записи
            var uniqueEmails = new HashSet<string>(
                table.AsEnumerable().Select(row => row.Field<string>("EmailAddress").ToLower()));
            int uniqueCount = uniqueEmails.Count;

            Console.WriteLine($"   Всего записей: {table.Rows.Count}");
            Console.WriteLine($"   Уникальных email: {uniqueCount}");
            Console.WriteLine($"   Точных дубликатов: {exactDuplicateCount}");
            Console.WriteLine($"   Похожих дубликатов: {similarDuplicateCount}");
            Console.WriteLine($"   Процент дубликатов: {(double)(exactDuplicateCount + similarDuplicateCount) / table.Rows.Count * 100:F1}%");
        }

        static DataView CreateDuplicatesOnlyView(DataTable table)
        {
            var duplicateView = FindExactDuplicates(table);
            return duplicateView;
        }

        static DataView CreateUniqueOnlyView(DataTable table)
        {
            // Находим уникальные email'ы
            var uniqueEmails = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("EmailAddress").ToLower())
                .Where(g => g.Count() == 1)
                .SelectMany(g => g)
                .ToList();

            // Создаем новую таблицу с уникальными записями
            DataTable uniqueTable = table.Clone();
            foreach (var row in uniqueEmails)
            {
                uniqueTable.ImportRow(row);
            }

            return new DataView(uniqueTable);
        }

        static void PrintEmailsFromView(DataView view, int maxRows)
        {
            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"   • {view[i]["EmailAddress"]} ({view[i]["Name"]})");
            }
            if (view.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {view.Count - maxRows} записей");
            }
        }

        static List<DataRow> SelectMasterRecords(DataView duplicateView)
        {
            var masterRecords = new List<DataRow>();

            if (duplicateView.Count == 0) return masterRecords;

            string currentEmail = "";
            DataRow currentMaster = null;

            for (int i = 0; i < duplicateView.Count; i++)
            {
                string email = duplicateView[i]["EmailAddress"].ToString();

                if (email != currentEmail)
                {
                    // Начинается новая группа дубликатов
                    currentEmail = email;

                    // Выбираем главную запись (первую в группе)
                    currentMaster = duplicateView[i].Row;
                    masterRecords.Add(currentMaster);

                    // Обновляем поле Duplicates
                    int duplicateCount = CountDuplicatesInGroup(duplicateView, email);
                    currentMaster["Duplicates"] = duplicateCount - 1;
                }
            }

            return masterRecords;
        }

        static int CountDuplicatesInGroup(DataView view, string email)
        {
            int count = 0;
            for (int i = 0; i < view.Count; i++)
            {
                if (view[i]["EmailAddress"].ToString() == email)
                    count++;
            }
            return count;
        }

        static void PrintMasterRecords(List<DataRow> masterRecords)
        {
            foreach (var record in masterRecords)
            {
                Console.WriteLine($"   • {record["EmailAddress"]} (дубликатов: {record["Duplicates"]})");
            }
        }

        static DataTable MergeDuplicates(DataTable table)
        {
            // Создаем новую таблицу для объединённых данных
            DataTable mergedTable = table.Clone();

            // Группируем по email
            var groups = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("EmailAddress").ToLower())
                .ToList();

            foreach (var group in groups)
            {
                // Выбираем первую запись в качестве основной
                DataRow masterRow = group.First();

                // Создаем новую строку для объединённой таблицы
                DataRow newRow = mergedTable.NewRow();

                // Копируем данные из основной записи
                foreach (DataColumn column in mergedTable.Columns)
                {
                    newRow[column.ColumnName] = masterRow[column.ColumnName];
                }

                // Устанавливаем количество дубликатов
                newRow["Duplicates"] = group.Count() - 1;

                mergedTable.Rows.Add(newRow);
            }

            return mergedTable;
        }

        static void GenerateDuplicateReport(DataTable table)
        {
            Console.WriteLine("   Отчёт о дубликатах:");
            Console.WriteLine(new string('-', 60));

            // Самые частые дубликаты
            var topDuplicates = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("EmailAddress").ToLower())
                .Where(g => g.Count() > 1)
                .Select(g => new
                {
                    Email = g.Key,
                    Count = g.Count(),
                    Names = string.Join(", ", g.Select(row => row.Field<string>("Name")).Distinct())
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            if (topDuplicates.Any())
            {
                Console.WriteLine("\n   Топ-5 самых частых дубликатов:");
                foreach (var dup in topDuplicates)
                {
                    Console.WriteLine($"   • {dup.Email}: {dup.Count} раз (имена: {dup.Names})");
                }
            }

            // Дубликаты по доменам
            var duplicatesByDomain = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("Domain"))
                .Select(g => new
                {
                    Domain = g.Key,
                    Total = g.Count(),
                    Duplicates = g.GroupBy(row => row.Field<string>("EmailAddress").ToLower())
                                 .Count(inner => inner.Count() > 1)
                })
                .OrderByDescending(x => (double)x.Duplicates / x.Total)
                .ToList();

            Console.WriteLine("\n   Дубликаты по доменам:");
            foreach (var domain in duplicatesByDomain.Take(5))
            {
                double percentage = (double)domain.Duplicates / domain.Total * 100;
                Console.WriteLine($"   • {domain.Domain}: {domain.Duplicates} дубликатов ({percentage:F1}%)");
            }
        }

        static void PrintFinalStatistics(DataTable originalTable, DataTable mergedTable)
        {
            int originalCount = originalTable.Rows.Count;
            int mergedCount = mergedTable.Rows.Count;
            int removedCount = originalCount - mergedCount;

            double reductionPercentage = (double)removedCount / originalCount * 100;

            Console.WriteLine($"   Исходных записей: {originalCount}");
            Console.WriteLine($"   После объединения: {mergedCount}");
            Console.WriteLine($"   Удалено дубликатов: {removedCount}");
            Console.WriteLine($"   Сокращение: {reductionPercentage:F1}%");

            // Количество записей с дубликатами
            int recordsWithDuplicates = mergedTable.AsEnumerable()
                .Count(row => (int)row["Duplicates"] > 0);

            Console.WriteLine($"   Записей с дубликатами: {recordsWithDuplicates}");
            Console.WriteLine($"   Уникальных записей: {mergedCount - recordsWithDuplicates}");
        }
        #endregion
        #region Задание 26: Синхронизация нескольких DataView при изменении данных
        static void Task26_SyncDataViews()
        {
            Console.WriteLine("ЗАДАНИЕ 26: Синхронизация нескольких DataView при изменении данных\n");
            Console.WriteLine("Цель: Демонстрация синхронизации представлений при изменении данных\n");

            // 1. Создание основной таблицы товаров и связанных таблиц
            DataTable products = CreateProductsTableForSync();
            DataTable categories = CreateCategoriesTableForSync();
            DataTable discounts = CreateDiscountsTableForSync();

            // Заполнение данными
            FillCategoriesDataForSync(categories);
            FillDiscountsDataForSync(discounts);
            FillProductsDataForSync(products, categories, 50);

            Console.WriteLine("1. Созданы таблицы:");
            Console.WriteLine($"   • Товаров: {products.Rows.Count} записей");
            Console.WriteLine($"   • Категорий: {categories.Rows.Count} записей");
            Console.WriteLine($"   • Скидок: {discounts.Rows.Count} записей");

            // 2. Создание нескольких DataView
            Console.WriteLine("\n2. Создание нескольких DataView для таблицы товаров:");

            // DataView 1: По категориям
            DataView byCategoryView = new DataView(products);
            byCategoryView.RowFilter = "Category = 'Электроника'";
            byCategoryView.Sort = "Name ASC";
            Console.WriteLine($"   DataView 1 (Электроника): {byCategoryView.Count} товаров");

            // DataView 2: Дорогие товары
            DataView expensiveView = new DataView(products);
            expensiveView.RowFilter = "Price > 1000";
            expensiveView.Sort = "Price DESC";
            Console.WriteLine($"   DataView 2 (Цена > 1000): {expensiveView.Count} товаров");

            // DataView 3: Отсортированные по цене
            DataView sortedByPriceView = new DataView(products);
            sortedByPriceView.Sort = "Price ASC";
            Console.WriteLine($"   DataView 3 (Сортировка по цене): {sortedByPriceView.Count} товаров");

            // DataView 4: Мало на складе
            DataView lowStockView = new DataView(products);
            lowStockView.RowFilter = "Stock < 10";
            Console.WriteLine($"   DataView 4 (Остаток < 10): {lowStockView.Count} товаров");

            // 3. Создание журнала синхронизации
            List<SyncLogEntry> syncLog = new List<SyncLogEntry>();

            // 4. Подписка на события изменения данных
            products.RowChanged += (sender, e) =>
            {
                var log = new SyncLogEntry
                {
                    Timestamp = DateTime.Now,
                    Operation = e.Action.ToString(),
                    ProductID = e.Row["ProductID"].ToString(),
                    ProductName = e.Row["Name"].ToString(),
                    Details = $"Изменение: {e.Action}"
                };
                syncLog.Add(log);
                Console.WriteLine($"   [СИНХРОНИЗАЦИЯ] {log.Timestamp:HH:mm:ss}: {log.Operation} - {log.ProductName}");
            };

            products.TableNewRow += (sender, e) =>
            {
                var log = new SyncLogEntry
                {
                    Timestamp = DateTime.Now,
                    Operation = "Add",
                    ProductID = e.Row["ProductID"].ToString(),
                    ProductName = e.Row["Name"].ToString(),
                    Details = "Добавлен новый товар"
                };
                syncLog.Add(log);
            };

            products.RowDeleting += (sender, e) =>
            {
                var log = new SyncLogEntry
                {
                    Timestamp = DateTime.Now,
                    Operation = "Delete",
                    ProductID = e.Row["ProductID"].ToString(),
                    ProductName = e.Row["Name"].ToString(),
                    Details = "Удаление товара"
                };
                syncLog.Add(log);
            };

            // 5. Демонстрация синхронизации
            Console.WriteLine("\n3. Демонстрация синхронизации:");

            // Тест 1: Добавление товара
            Console.WriteLine("\n   Тест 1: Добавление нового товара (Ноутбук, 1500, Электроника):");
            DataRow newProduct = products.NewRow();
            newProduct["ProductID"] = 999;
            newProduct["Name"] = "Ноутбук Gaming Pro";
            newProduct["Category"] = "Электроника";
            newProduct["Price"] = 1500;
            newProduct["Stock"] = 5;
            products.Rows.Add(newProduct);

            DisplaySyncStatus("После добавления", byCategoryView, expensiveView, sortedByPriceView, lowStockView);

            // Тест 2: Изменение цены
            Console.WriteLine("\n   Тест 2: Изменение цены товара ID 5 (увеличение до 1200):");
            DataRow productToUpdate = products.Rows.Find(5);
            if (productToUpdate != null)
            {
                Console.WriteLine($"   До: {productToUpdate["Name"]} - {productToUpdate["Price"]:C}");
                productToUpdate["Price"] = 1200;
                Console.WriteLine($"   После: {productToUpdate["Name"]} - {productToUpdate["Price"]:C}");
            }

            DisplaySyncStatus("После изменения цены", byCategoryView, expensiveView, sortedByPriceView, lowStockView);

            // Тест 3: Уменьшение остатка
            Console.WriteLine("\n   Тест 3: Уменьшение остатка товара ID 3 до 3 единиц:");
            DataRow lowStockProduct = products.Rows.Find(3);
            if (lowStockProduct != null)
            {
                Console.WriteLine($"   До: {lowStockProduct["Name"]} - Остаток: {lowStockProduct["Stock"]}");
                lowStockProduct["Stock"] = 3;
                Console.WriteLine($"   После: {lowStockProduct["Name"]} - Остаток: {lowStockProduct["Stock"]}");
            }

            DisplaySyncStatus("После изменения остатка", byCategoryView, expensiveView, sortedByPriceView, lowStockView);

            // Тест 4: Удаление товара
            Console.WriteLine("\n   Тест 4: Удаление товара ID 10:");
            DataRow productToDelete = products.Rows.Find(10);
            if (productToDelete != null)
            {
                Console.WriteLine($"   Удаляем: {productToDelete["Name"]} - {productToDelete["Price"]:C}");
                productToDelete.Delete();
            }

            DisplaySyncStatus("После удаления", byCategoryView, expensiveView, sortedByPriceView, lowStockView);

            // Тест 5: Изменение категории
            Console.WriteLine("\n   Тест 5: Изменение категории товара ID 15 на 'Электроника':");
            DataRow productToRecategorize = products.Rows.Find(15);
            if (productToRecategorize != null && productToRecategorize["Category"].ToString() != "Электроника")
            {
                Console.WriteLine($"   До: {productToRecategorize["Name"]} - Категория: {productToRecategorize["Category"]}");
                productToRecategorize["Category"] = "Электроника";
                Console.WriteLine($"   После: {productToRecategorize["Name"]} - Категория: {productToRecategorize["Category"]}");
            }

            DisplaySyncStatus("После изменения категории", byCategoryView, expensiveView, sortedByPriceView, lowStockView);

            // 6. Отображение всех DataView
            Console.WriteLine("\n4. Отображение всех DataView:");

            Console.WriteLine("\n   DataView 1 - Электроника (первые 5):");
            DisplayDataView(byCategoryView, 5);

            Console.WriteLine("\n   DataView 2 - Дорогие товары (> 1000) (первые 5):");
            DisplayDataView(expensiveView, 5);

            Console.WriteLine("\n   DataView 3 - Сортировка по цене (первые 5):");
            DisplayDataView(sortedByPriceView, 5);

            Console.WriteLine("\n   DataView 4 - Мало на складе (< 10):");
            DisplayDataView(lowStockView, 5);

            // 7. Логирование синхронизации
            Console.WriteLine("\n5. Логи синхронизации:");
            DisplaySyncLog(syncLog);

            // 8. Статистика синхронизации
            Console.WriteLine("\n6. Статистика синхронизации:");
            DisplaySyncStatistics(syncLog, products, byCategoryView, expensiveView, sortedByPriceView, lowStockView);

            // 9. Проверка целостности данных
            Console.WriteLine("\n7. Проверка целостности данных:");
            VerifyDataConsistency(products, byCategoryView, expensiveView, sortedByPriceView, lowStockView);
        }

        static DataTable CreateProductsTableForSync()
        {
            DataTable table = new DataTable("Товары");

            table.Columns.Add("ProductID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Stock", typeof(int));

            table.PrimaryKey = new DataColumn[] { table.Columns["ProductID"] };

            return table;
        }

        static DataTable CreateCategoriesTableForSync()
        {
            DataTable table = new DataTable("Категории");

            table.Columns.Add("CategoryID", typeof(int));
            table.Columns.Add("CategoryName", typeof(string));

            table.PrimaryKey = new DataColumn[] { table.Columns["CategoryID"] };

            return table;
        }

        static DataTable CreateDiscountsTableForSync()
        {
            DataTable table = new DataTable("Скидки");

            table.Columns.Add("DiscountID", typeof(int));
            table.Columns.Add("ProductID", typeof(int));
            table.Columns.Add("DiscountPercent", typeof(decimal));
            table.Columns.Add("StartDate", typeof(DateTime));
            table.Columns.Add("EndDate", typeof(DateTime));

            return table;
        }

        static void FillCategoriesDataForSync(DataTable table)
        {
            string[] categories = { "Электроника", "Одежда", "Продукты", "Книги", "Спорт", "Мебель" };

            for (int i = 1; i <= categories.Length; i++)
            {
                table.Rows.Add(i, categories[i - 1]);
            }
        }

        static void FillDiscountsDataForSync(DataTable table)
        {
            Random rand = new Random();

            for (int i = 1; i <= 10; i++)
            {
                table.Rows.Add(
                    i,
                    rand.Next(1, 50),
                    Math.Round((decimal)rand.NextDouble() * 30, 2),
                    DateTime.Now.AddDays(-rand.Next(0, 30)),
                    DateTime.Now.AddDays(rand.Next(1, 60))
                );
            }
        }

        static void FillProductsDataForSync(DataTable products, DataTable categories, int count)
        {
            string[] productNames = { "Ноутбук", "Смартфон", "Планшет", "Монитор", "Телевизор",
                             "Футболка", "Джинсы", "Куртка", "Кроссовки", "Рубашка",
                             "Яблоки", "Банан", "Хлеб", "Молоко", "Сыр",
                             "Книга", "Журнал", "Учебник", "Энциклопедия", "Словарь",
                             "Мяч", "Велосипед", "Гантели", "Скакалка", "Ракетка",
                             "Стул", "Стол", "Шкаф", "Кровать", "Диван" };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                string category = "";
                decimal basePrice = 0;

                // Определяем категорию и базовую цену
                int categoryType = i % 6;
                switch (categoryType)
                {
                    case 0: // Электроника
                        category = "Электроника";
                        basePrice = 500 + (decimal)rand.NextDouble() * 2000;
                        break;
                    case 1: // Одежда
                        category = "Одежда";
                        basePrice = 50 + (decimal)rand.NextDouble() * 200;
                        break;
                    case 2: // Продукты
                        category = "Продукты";
                        basePrice = 10 + (decimal)rand.NextDouble() * 100;
                        break;
                    case 3: // Книги
                        category = "Книги";
                        basePrice = 20 + (decimal)rand.NextDouble() * 200;
                        break;
                    case 4: // Спорт
                        category = "Спорт";
                        basePrice = 100 + (decimal)rand.NextDouble() * 500;
                        break;
                    case 5: // Мебель
                        category = "Мебель";
                        basePrice = 1000 + (decimal)rand.NextDouble() * 3000;
                        break;
                }

                products.Rows.Add(
                    i,
                    $"{productNames[rand.Next(productNames.Length)]} {i}",
                    category,
                    Math.Round(basePrice, 2),
                    rand.Next(0, 50)
                );
            }
        }

        static void DisplaySyncStatus(string status, params DataView[] views)
        {
            Console.WriteLine($"\n   {status}:");
            for (int i = 0; i < views.Length; i++)
            {
                Console.WriteLine($"   • DataView {i + 1}: {views[i].Count} записей");
            }
        }

        static void DisplayDataView(DataView view, int maxRows)
        {
            if (view.Count == 0)
            {
                Console.WriteLine("   Нет данных");
                return;
            }

            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"   • {view[i]["ProductID"]}: {view[i]["Name"]} - " +
                                 $"{view[i]["Price"]:C} ({view[i]["Stock"]} шт.)");
            }
            if (view.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {view.Count - maxRows} записей");
            }
        }

        static void DisplaySyncLog(List<SyncLogEntry> log)
        {
            if (log.Count == 0)
            {
                Console.WriteLine("   Нет записей в логе");
                return;
            }

            Console.WriteLine($"{"Время",-12} {"Операция",-10} {"ID",-6} {"Товар",-25} {"Детали",-30}");
            Console.WriteLine(new string('-', 85));

            foreach (var entry in log.Take(10))
            {
                Console.WriteLine($"{entry.Timestamp:HH:mm:ss} {entry.Operation,-10} {entry.ProductID,-6} " +
                                 $"{entry.ProductName,-25} {entry.Details,-30}");
            }

            if (log.Count > 10)
            {
                Console.WriteLine($"   ... и ещё {log.Count - 10} записей");
            }
        }

        static void DisplaySyncStatistics(List<SyncLogEntry> log, DataTable table, params DataView[] views)
        {
            Console.WriteLine($"   Всего операций синхронизации: {log.Count}");
            Console.WriteLine($"   Товаров в основной таблице: {table.Rows.Count}");

            var operationsByType = log.GroupBy(l => l.Operation)
                                      .Select(g => new { Operation = g.Key, Count = g.Count() })
                                      .ToList();

            Console.WriteLine("\n   Операции по типам:");
            foreach (var op in operationsByType)
            {
                Console.WriteLine($"   • {op.Operation}: {op.Count} раз");
            }

            Console.WriteLine("\n   Текущие DataView:");
            for (int i = 0; i < views.Length; i++)
            {
                int visibleInOthers = 0;
                for (int j = 0; j < views.Length; j++)
                {
                    if (i != j)
                    {
                        // Проверяем пересечение представлений
                        foreach (DataRowView row in views[i])
                        {
                            int productId = (int)row["ProductID"];
                            bool found = false;
                            foreach (DataRowView otherRow in views[j])
                            {
                                if ((int)otherRow["ProductID"] == productId)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found) visibleInOthers++;
                        }
                    }
                }

                Console.WriteLine($"   • DataView {i + 1}: {views[i].Count} записей, " +
                                 $"пересекаются с другими: {visibleInOthers}");
            }
        }

        static void VerifyDataConsistency(DataTable table, params DataView[] views)
        {
            Console.WriteLine("   Проверка целостности данных:");

            // Проверка 1: Все товары в DataView существуют в основной таблице
            bool allViewsValid = true;
            for (int i = 0; i < views.Length; i++)
            {
                foreach (DataRowView rowView in views[i])
                {
                    int productId = (int)rowView["ProductID"];
                    DataRow originalRow = table.Rows.Find(productId);

                    if (originalRow == null || originalRow.RowState == DataRowState.Deleted)
                    {
                        Console.WriteLine($"   ✗ DataView {i + 1}: товар ID {productId} не найден в основной таблице");
                        allViewsValid = false;
                    }
                }
            }

            if (allViewsValid)
            {
                Console.WriteLine("   ✓ Все товары в DataView существуют в основной таблице");
            }

            // Проверка 2: DataView соответствуют своим фильтрам
            for (int i = 0; i < views.Length; i++)
            {
                bool filterValid = true;
                foreach (DataRowView rowView in views[i])
                {
                    // Проверяем, соответствует ли строка фильтру DataView
                    switch (i)
                    {
                        case 0: // byCategoryView - Электроника
                            if (rowView["Category"].ToString() != "Электроника")
                            {
                                filterValid = false;
                                Console.WriteLine($"   ✗ DataView 1: товар {rowView["Name"]} не электроника");
                            }
                            break;
                        case 1: // expensiveView - Price > 1000
                            if ((decimal)rowView["Price"] <= 1000)
                            {
                                filterValid = false;
                                Console.WriteLine($"   ✗ DataView 2: товар {rowView["Name"]} дешевле 1000");
                            }
                            break;
                        case 3: // lowStockView - Stock < 10
                            if ((int)rowView["Stock"] >= 10)
                            {
                                filterValid = false;
                                Console.WriteLine($"   ✗ DataView 4: товар {rowView["Name"]} с остатком >= 10");
                            }
                            break;
                    }
                }

                if (filterValid)
                {
                    Console.WriteLine($"   ✓ DataView {i + 1}: все записи соответствуют фильтру");
                }
            }

            // Проверка 3: Сортировка DataView 3
            bool sortValid = true;
            for (int i = 1; i < views[2].Count; i++)
            {
                decimal prevPrice = (decimal)views[2][i - 1]["Price"];
                decimal currentPrice = (decimal)views[2][i]["Price"];
                if (currentPrice < prevPrice)
                {
                    sortValid = false;
                    Console.WriteLine($"   ✗ DataView 3: нарушена сортировка на позиции {i}");
                    break;
                }
            }

            if (sortValid)
            {
                Console.WriteLine("   ✓ DataView 3: сортировка по цене корректна");
            }
        }

        class SyncLogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Operation { get; set; }
            public string ProductID { get; set; }
            public string ProductName { get; set; }
            public string Details { get; set; }
        }
        #endregion

        #region Задание 27: Использование DataView для оптимизации производительности больших таблиц
        static void Task27_PerformanceOptimization()
        {
            Console.WriteLine("ЗАДАНИЕ 27: Использование DataView для оптимизации производительности больших таблиц\n");
            Console.WriteLine("Цель: Демонстрация преимуществ DataView для производительности\n");

            // 1. Создание большой таблицы логов
            Console.WriteLine("1. Создание большой таблицы логов (1,000,000 записей)...");
            DataTable logs = CreateLogsTable();
            FillLogsData(logs, 1000000);

            Console.WriteLine($"   Создано: {logs.Rows.Count} записей");
            Console.WriteLine($"   Использовано памяти: {GC.GetTotalMemory(true) / 1024 / 1024} MB");

            // 2. Сценарии тестирования
            Console.WriteLine("\n2. Сценарии тестирования производительности:");

            // Сценарий 1: Поиск через Select()
            Console.WriteLine("\n   Сценарий 1: Поиск через DataTable.Select()");
            TestSelectScenario(logs);

            // Сценарий 2: DataView с фильтром
            Console.WriteLine("\n   Сценарий 2: DataView с фильтром");
            TestDataViewScenario(logs);

            // Сценарий 3: Повторный поиск в DataView
            Console.WriteLine("\n   Сценарий 3: Повторный поиск в DataView");
            TestRepeatedDataViewScenario(logs);

            // Сценарий 4: Индекс и поиск через Find()
            Console.WriteLine("\n   Сценарий 4: Индекс и поиск через Find()");
            TestFindScenario(logs);

            // 3. Сравнение производительности
            Console.WriteLine("\n3. Сравнение производительности:");

            // Подготовка данных для сравнения
            var comparisonResults = new List<PerformanceResult>();

            // Тест Select()
            Console.WriteLine("\n   Тест Select() - 100 итераций:");
            var selectResult = RunSelectPerformanceTest(logs, 100);
            comparisonResults.Add(selectResult);

            // Тест DataView
            Console.WriteLine("\n   Тест DataView - 100 итераций:");
            var dataViewResult = RunDataViewPerformanceTest(logs, 100);
            comparisonResults.Add(dataViewResult);

            // Тест Find()
            Console.WriteLine("\n   Тест Find() - 100 итераций:");
            var findResult = RunFindPerformanceTest(logs, 100);
            comparisonResults.Add(findResult);

            // 4. Отчёт о производительности
            Console.WriteLine("\n4. Отчёт о производительности:");
            DisplayPerformanceReport(comparisonResults);

            // 5. Анализ использования памяти
            Console.WriteLine("\n5. Анализ использования памяти:");
            AnalyzeMemoryUsage(logs);

            // 6. Рекомендации
            Console.WriteLine("\n6. Рекомендации по использованию DataView:");
            DisplayOptimizationRecommendations();

            // 7. Кэширование результатов
            Console.WriteLine("\n7. Демонстрация кэширования:");
            DemonstrateCaching(logs);
        }

        static DataTable CreateLogsTable()
        {
            DataTable table = new DataTable("Логи");

            table.Columns.Add("LogID", typeof(int));
            table.Columns.Add("Timestamp", typeof(DateTime));
            table.Columns.Add("Level", typeof(string));
            table.Columns.Add("Message", typeof(string));
            table.Columns.Add("Source", typeof(string));
            table.Columns.Add("EventID", typeof(int));

            table.PrimaryKey = new DataColumn[] { table.Columns["LogID"] };

            return table;
        }

        static void FillLogsData(DataTable table, int count)
        {
            string[] levels = { "INFO", "WARNING", "ERROR", "DEBUG", "FATAL" };
            string[] sources = { "Application", "Database", "Network", "Security", "System", "WebServer" };
            string[] messages = {
        "User login successful", "Database connection established", "File uploaded successfully",
        "Network timeout occurred", "Memory allocation failed", "Security audit completed",
        "Backup process started", "Cache cleared", "Configuration updated", "Session expired"
    };

            Random rand = new Random();
            DateTime startDate = new DateTime(2023, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                table.Rows.Add(
                    i,
                    startDate.AddSeconds(rand.Next(0, 31536000)), // В течение года
                    levels[rand.Next(levels.Length)],
                    $"{messages[rand.Next(messages.Length)]} {i}",
                    sources[rand.Next(sources.Length)],
                    rand.Next(1000, 9999)
                );

                // Показываем прогресс каждые 100000 записей
                if (i % 100000 == 0)
                {
                    Console.WriteLine($"   Создано {i} записей...");
                }
            }
        }

        static void TestSelectScenario(DataTable logs)
        {
            Console.WriteLine("   а) Первый поиск (ERROR логи):");
            Stopwatch sw = Stopwatch.StartNew();
            DataRow[] errorLogs = logs.Select("Level = 'ERROR'");
            sw.Stop();
            Console.WriteLine($"      Найдено: {errorLogs.Length} записей");
            Console.WriteLine($"      Время: {sw.ElapsedMilliseconds} мс");

            Console.WriteLine("\n   б) Второй поиск (INFO логи):");
            sw.Restart();
            DataRow[] infoLogs = logs.Select("Level = 'INFO'");
            sw.Stop();
            Console.WriteLine($"      Найдено: {infoLogs.Length} записей");
            Console.WriteLine($"      Время: {sw.ElapsedMilliseconds} мс");

            Console.WriteLine("\n   в) Сложный поиск (ERROR + Database):");
            sw.Restart();
            DataRow[] complexLogs = logs.Select("Level = 'ERROR' AND Source = 'Database'");
            sw.Stop();
            Console.WriteLine($"      Найдено: {complexLogs.Length} записей");
            Console.WriteLine($"      Время: {sw.ElapsedMilliseconds} мс");
        }

        static void TestDataViewScenario(DataTable logs)
        {
            Console.WriteLine("   а) Создание DataView с фильтром ERROR:");
            Stopwatch sw = Stopwatch.StartNew();
            DataView errorView = new DataView(logs);
            errorView.RowFilter = "Level = 'ERROR'";
            sw.Stop();
            Console.WriteLine($"      Время создания: {sw.ElapsedMilliseconds} мс");
            Console.WriteLine($"      Записей в DataView: {errorView.Count}");

            Console.WriteLine("\n   б) Изменение фильтра на INFO:");
            sw.Restart();
            errorView.RowFilter = "Level = 'INFO'";
            sw.Stop();
            Console.WriteLine($"      Время изменения фильтра: {sw.ElapsedMilliseconds} мс");
            Console.WriteLine($"      Записей в DataView: {errorView.Count}");

            Console.WriteLine("\n   в) Сложный фильтр (ERROR + Database):");
            sw.Restart();
            errorView.RowFilter = "Level = 'ERROR' AND Source = 'Database'";
            sw.Stop();
            Console.WriteLine($"      Время изменения фильтра: {sw.ElapsedMilliseconds} мс");
            Console.WriteLine($"      Записей в DataView: {errorView.Count}");
        }

        static void TestRepeatedDataViewScenario(DataTable logs)
        {
            Console.WriteLine("   а) Создание DataView один раз, много разное использование:");

            DataView logView = new DataView(logs);
            Stopwatch sw = Stopwatch.StartNew();

            string[] levels = { "ERROR", "INFO", "WARNING", "DEBUG" };
            foreach (string level in levels)
            {
                logView.RowFilter = $"Level = '{level}'";
                int count = logView.Count;
            }

            sw.Stop();
            Console.WriteLine($"      Время для 4 фильтраций: {sw.ElapsedMilliseconds} мс");
            Console.WriteLine($"      Среднее время на фильтр: {sw.ElapsedMilliseconds / 4.0:F2} мс");

            // Сравнение с Select()
            Console.WriteLine("\n   б) Сравнение с Select() для 4 фильтраций:");
            sw.Restart();

            foreach (string level in levels)
            {
                DataRow[] results = logs.Select($"Level = '{level}'");
                int count = results.Length;
            }

            sw.Stop();
            Console.WriteLine($"      Время для 4 Select(): {sw.ElapsedMilliseconds} мс");
            Console.WriteLine($"      Среднее время на Select(): {sw.ElapsedMilliseconds / 4.0:F2} мс");
        }

        static void TestFindScenario(DataTable logs)
        {
            Console.WriteLine("   а) Поиск по первичному ключу (LogID):");

            // Тест 1: Поиск существующего ID
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                int searchId = new Random().Next(1, logs.Rows.Count);
                DataRow log = logs.Rows.Find(searchId);
            }
            sw.Stop();
            Console.WriteLine($"      1000 поисков Find(): {sw.ElapsedMilliseconds} мс");
            Console.WriteLine($"      Среднее: {sw.ElapsedMilliseconds / 1000.0:F4} мс");

            // Тест 2: Select() для сравнения
            Console.WriteLine("\n   б) Сравнение с Select():");
            sw.Restart();
            for (int i = 0; i < 1000; i++)
            {
                int searchId = new Random().Next(1, logs.Rows.Count);
                DataRow[] results = logs.Select($"LogID = {searchId}");
            }
            sw.Stop();
            Console.WriteLine($"      1000 поисков Select(): {sw.ElapsedMilliseconds} мс");
            Console.WriteLine($"      Среднее: {sw.ElapsedMilliseconds / 1000.0:F4} мс");
        }

        static PerformanceResult RunSelectPerformanceTest(DataTable logs, int iterations)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Random rand = new Random();

            for (int i = 0; i < iterations; i++)
            {
                string[] levels = { "ERROR", "INFO", "WARNING" };
                string level = levels[rand.Next(levels.Length)];
                DataRow[] results = logs.Select($"Level = '{level}'");
            }

            sw.Stop();

            return new PerformanceResult
            {
                Method = "Select()",
                Iterations = iterations,
                TotalTime = sw.ElapsedMilliseconds,
                AvgTime = sw.ElapsedMilliseconds / (double)iterations,
                MemoryUsed = GC.GetTotalMemory(false) / 1024
            };
        }

        static PerformanceResult RunDataViewPerformanceTest(DataTable logs, int iterations)
        {
            // Создание DataView один раз
            DataView logView = new DataView(logs);
            Stopwatch sw = Stopwatch.StartNew();
            Random rand = new Random();

            for (int i = 0; i < iterations; i++)
            {
                string[] levels = { "ERROR", "INFO", "WARNING" };
                string level = levels[rand.Next(levels.Length)];
                logView.RowFilter = $"Level = '{level}'";
                int count = logView.Count;
            }

            sw.Stop();

            return new PerformanceResult
            {
                Method = "DataView",
                Iterations = iterations,
                TotalTime = sw.ElapsedMilliseconds,
                AvgTime = sw.ElapsedMilliseconds / (double)iterations,
                MemoryUsed = GC.GetTotalMemory(false) / 1024
            };
        }

        static PerformanceResult RunFindPerformanceTest(DataTable logs, int iterations)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Random rand = new Random();

            for (int i = 0; i < iterations; i++)
            {
                int searchId = rand.Next(1, logs.Rows.Count);
                DataRow result = logs.Rows.Find(searchId);
            }

            sw.Stop();

            return new PerformanceResult
            {
                Method = "Find()",
                Iterations = iterations,
                TotalTime = sw.ElapsedMilliseconds,
                AvgTime = sw.ElapsedMilliseconds / (double)iterations,
                MemoryUsed = GC.GetTotalMemory(false) / 1024
            };
        }

        static void DisplayPerformanceReport(List<PerformanceResult> results)
        {
            Console.WriteLine($"{"Метод",-15} {"Итераций",-10} {"Общее время",-15} {"Среднее время",-15} {"Память (KB)",-12}");
            Console.WriteLine(new string('-', 70));

            foreach (var result in results)
            {
                Console.WriteLine($"{result.Method,-15} {result.Iterations,-10} " +
                                 $"{result.TotalTime} мс{-15} {result.AvgTime:F4} мс{-15} " +
                                 $"{result.MemoryUsed,-12}");
            }

            // Находим лучший метод
            var bestMethod = results.OrderBy(r => r.AvgTime).First();
            Console.WriteLine($"\n   Лучший метод: {bestMethod.Method} ({bestMethod.AvgTime:F4} мс на операцию)");

            // Сравнение
            var selectResult = results.First(r => r.Method == "Select()");
            var dataViewResult = results.First(r => r.Method == "DataView");

            if (selectResult.AvgTime > 0 && dataViewResult.AvgTime > 0)
            {
                double speedup = selectResult.AvgTime / dataViewResult.AvgTime;
                Console.WriteLine($"   DataView быстрее Select() в {speedup:F1} раза");
            }
        }

        static void AnalyzeMemoryUsage(DataTable logs)
        {
            long memoryBefore = GC.GetTotalMemory(true);

            // Создание нескольких DataView
            List<DataView> views = new List<DataView>();
            string[] filters = { "Level = 'ERROR'", "Level = 'INFO'", "Source = 'Database'",
                        "EventID > 5000", "Timestamp >= '2023-06-01'" };

            foreach (string filter in filters)
            {
                DataView view = new DataView(logs);
                view.RowFilter = filter;
                views.Add(view);
            }

            long memoryAfter = GC.GetTotalMemory(true);
            long memoryUsed = memoryAfter - memoryBefore;

            Console.WriteLine($"   Память до создания DataView: {memoryBefore / 1024 / 1024} MB");
            Console.WriteLine($"   Память после {views.Count} DataView: {memoryAfter / 1024 / 1024} MB");
            Console.WriteLine($"   Использовано памяти: {memoryUsed / 1024} KB");
            Console.WriteLine($"   В среднем на DataView: {memoryUsed / views.Count / 1024} KB");

            // Освобождение памяти
            views.Clear();
            GC.Collect();
        }

        static void DisplayOptimizationRecommendations()
        {
            Console.WriteLine("   Рекомендации по использованию DataView:");
            Console.WriteLine("   1. Используйте DataView для многократных запросов к одним данным");
            Console.WriteLine("   2. Для поиска по первичному ключу всегда используйте Find()");
            Console.WriteLine("   3. DataView эффективен при частом изменении фильтров");
            Console.WriteLine("   4. Избегайте создания множества DataView для больших таблиц");
            Console.WriteLine("   5. Используйте DataView.ToTable() для создания независимых копий");
            Console.WriteLine("   6. Для сложных запросов комбинируйте DataView с LINQ");
            Console.WriteLine("   7. Очищайте ненужные DataView для освобождения памяти");
            Console.WriteLine("   8. Используйте кэширование результатов частых запросов");
        }

        static void DemonstrateCaching(DataTable logs)
        {
            Console.WriteLine("   Демонстрация кэширования с DataView:");

            // Без кэширования
            Console.WriteLine("\n   а) Без кэширования (5 одинаковых запросов):");
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < 5; i++)
            {
                DataView view = new DataView(logs);
                view.RowFilter = "Level = 'ERROR' AND Source = 'Database'";
                int count = view.Count;
            }

            sw.Stop();
            Console.WriteLine($"      Время: {sw.ElapsedMilliseconds} мс");

            // С кэшированием
            Console.WriteLine("\n   б) С кэшированием (1 DataView, 5 запросов):");
            DataView cachedView = new DataView(logs);

            sw.Restart();
            for (int i = 0; i < 5; i++)
            {
                cachedView.RowFilter = "Level = 'ERROR' AND Source = 'Database'";
                int count = cachedView.Count;
            }
            sw.Stop();
            Console.WriteLine($"      Время: {sw.ElapsedMilliseconds} мс");

            double improvement = (sw.ElapsedMilliseconds > 0) ?
                (sw.ElapsedMilliseconds * 1.0 / sw.ElapsedMilliseconds) : 1.0;
            Console.WriteLine($"      Улучшение производительности: в {improvement:F1} раза");
        }

        class PerformanceResult
        {
            public string Method { get; set; }
            public int Iterations { get; set; }
            public long TotalTime { get; set; }
            public double AvgTime { get; set; }
            public long MemoryUsed { get; set; }
        }
        #endregion

        #region Задание 28: Реализация кастомного поиска с использованием DataView
        static void Task28_CustomSearchEngine()
        {
            Console.WriteLine("ЗАДАНИЕ 28: Реализация кастомного поиска с использованием DataView\n");
            Console.WriteLine("Цель: Создание продвинутой системы поиска\n");

            // 1. Создание таблицы библиотеки
            Console.WriteLine("1. Создание таблицы библиотеки...");
            DataTable library = CreateLibraryTable();
            FillLibraryData(library, 500);

            Console.WriteLine($"   Создано книг: {library.Rows.Count}");
            Console.WriteLine("   Примеры книг:");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"   • {library.Rows[i]["Title"]} - {library.Rows[i]["Author"]}");
            }

            // 2. Создание поискового движка
            Console.WriteLine("\n2. Создание поискового движка...");
            SearchEngine searchEngine = new SearchEngine(library);

            // 3. Тестирование различных типов поиска
            Console.WriteLine("\n3. Тестирование поиска:");

            // Поиск по автору
            Console.WriteLine("\n   Поиск по автору (содержит 'Толстой'):");
            DataView authorResults = searchEngine.SearchByAuthor("Толстой");
            DisplaySearchResults(authorResults, 3);

            // Поиск по названию
            Console.WriteLine("\n   Поиск по названию (содержит 'война'):");
            DataView titleResults = searchEngine.SearchByTitle("война");
            DisplaySearchResults(titleResults, 3);

            // Поиск по жанру
            Console.WriteLine("\n   Поиск по жанру ('Фантастика'):");
            DataView genreResults = searchEngine.SearchByGenre("Фантастика");
            DisplaySearchResults(genreResults, 3);

            // Поиск по году
            Console.WriteLine("\n   Поиск по году (2000-2010):");
            DataView yearResults = searchEngine.SearchByYear(2000, 2010);
            DisplaySearchResults(yearResults, 3);

            // Комбинированный поиск
            Console.WriteLine("\n   Комбинированный поиск (Фантастика, рейтинг > 4, после 2000):");
            DataView combinedResults = searchEngine.CombinedSearch(
                genre: "Фантастика",
                minRating: 4.0,
                minYear: 2000
            );
            DisplaySearchResults(combinedResults, 3);

            // 4. Сохранение истории поисков
            Console.WriteLine("\n4. История поисков:");
            DisplaySearchHistory(searchEngine.SearchHistory);

            // 5. Рекомендации на основе истории
            Console.WriteLine("\n5. Рекомендации:");
            DisplayRecommendations(searchEngine);

            // 6. Сохранение результатов поиска
            Console.WriteLine("\n6. Сохранение результатов:");
            TestSaveResults(searchEngine);

            // 7. Интерфейс поиска
            Console.WriteLine("\n7. Интерактивный поиск:");
            InteractiveSearch(searchEngine);
        }

        static DataTable CreateLibraryTable()
        {
            DataTable table = new DataTable("Библиотека");

            table.Columns.Add("BookID", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Author", typeof(string));
            table.Columns.Add("Year", typeof(int));
            table.Columns.Add("ISBN", typeof(string));
            table.Columns.Add("Genre", typeof(string));
            table.Columns.Add("Rating", typeof(double));
            table.Columns.Add("Pages", typeof(int));

            table.PrimaryKey = new DataColumn[] { table.Columns["BookID"] };

            return table;
        }

        static void FillLibraryData(DataTable table, int count)
        {
            string[] authors = {
        "Лев Толстой", "Фёдор Достоевский", "Антон Чехов", "Александр Пушкин",
        "Михаил Булгаков", "Иван Тургенев", "Николай Гоголь", "Александр Дюма",
        "Жюль Верн", "Артур Конан Дойл", "Агата Кристи", "Джордж Оруэлл",
        "Рэй Брэдбери", "Айзек Азимов", "Фрэнк Герберт", "Дж. Р. Р. Толкин",
        "Джоан Роулинг", "Стивен Кинг", "Дэн Браун", "Пауло Коэльо"
    };

            string[] genres = {
        "Классика", "Фантастика", "Детектив", "Роман", "Фэнтези",
        "Научная литература", "Исторический роман", "Биография", "Поэзия", "Драма"
    };

            string[] titles = {
        "Война и мир", "Преступление и наказание", "Мастер и Маргарита",
        "Евгений Онегин", "Анна Каренина", "Братья Карамазовы", "Отцы и дети",
        "Мёртвые души", "Ревизор", "Герой нашего времени", "Обломов",
        "Идиот", "Бесы", "Подросток", "Капитанская дочка", "Пиковая дама",
        "Дубровский", "Руслан и Людмила", "Сказка о царе Салтане", "Мцыри"
    };

            Random rand = new Random();

            for (int i = 1; i <= count; i++)
            {
                string author = authors[rand.Next(authors.Length)];
                string genre = genres[rand.Next(genres.Length)];
                string title = $"{titles[rand.Next(titles.Length)]} {i % 20}";

                // Для некоторых авторов добавляем специальные книги
                if (author.Contains("Толстой") && i % 10 == 0)
                {
                    title = "Война и мир";
                }
                else if (author.Contains("Достоевский") && i % 10 == 1)
                {
                    title = "Преступление и наказание";
                }

                table.Rows.Add(
                    i,
                    title,
                    author,
                    1900 + rand.Next(124), // 1900-2024
                    $"978-{rand.Next(100, 1000)}-{rand.Next(10000, 100000)}-{rand.Next(0, 10)}",
                    genre,
                    Math.Round(3 + rand.NextDouble() * 2, 1), // 3.0-5.0
                    rand.Next(100, 1000)
                );
            }
        }

        static void DisplaySearchResults(DataView results, int maxRows)
        {
            if (results.Count == 0)
            {
                Console.WriteLine("   Не найдено");
                return;
            }

            Console.WriteLine($"   Найдено книг: {results.Count}");
            Console.WriteLine("   Примеры:");

            for (int i = 0; i < Math.Min(maxRows, results.Count); i++)
            {
                Console.WriteLine($"   • {results[i]["Title"]} - {results[i]["Author"]} " +
                                 $"({results[i]["Year"]}, {results[i]["Rating"]}★)");
            }

            if (results.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {results.Count - maxRows} книг");
            }
        }

        static void DisplaySearchHistory(List<SearchHistoryEntry> history)
        {
            if (history.Count == 0)
            {
                Console.WriteLine("   История поиска пуста");
                return;
            }

            Console.WriteLine($"   Всего поисков: {history.Count}");
            Console.WriteLine("\n   Последние 5 поисков:");

            foreach (var entry in history.TakeLast(5))
            {
                Console.WriteLine($"   • {entry.Timestamp:HH:mm:ss}: {entry.SearchType} - " +
                                 $"'{entry.SearchTerm}' ({entry.ResultsCount} результатов)");
            }

            // Статистика по типам поиска
            var stats = history.GroupBy(h => h.SearchType)
                               .Select(g => new { Type = g.Key, Count = g.Count() })
                               .OrderByDescending(g => g.Count)
                               .ToList();

            Console.WriteLine("\n   Статистика по типам:");
            foreach (var stat in stats)
            {
                Console.WriteLine($"   • {stat.Type}: {stat.Count} раз");
            }
        }

        static void DisplayRecommendations(SearchEngine searchEngine)
        {
            var recommendations = searchEngine.GetRecommendations();

            if (recommendations.Count == 0)
            {
                Console.WriteLine("   Недостаточно данных для рекомендаций");
                return;
            }

            Console.WriteLine($"   Рекомендации ({recommendations.Count} книг):");

            foreach (var book in recommendations.Take(5))
            {
                Console.WriteLine($"   • {book["Title"]} - {book["Author"]} " +
                                 $"({book["Genre"]}, {book["Rating"]}★)");
            }
        }

        static void TestSaveResults(SearchEngine searchEngine)
        {
            // Выполняем поиск
            DataView results = searchEngine.SearchByGenre("Фантастика");

            if (results.Count == 0)
            {
                Console.WriteLine("   Нет результатов для сохранения");
                return;
            }

            // Сохраняем в CSV
            string csvFile = "fantasy_books.csv";
            searchEngine.SaveResultsToCsv(results, csvFile);
            Console.WriteLine($"   Сохранено {results.Count} книг в {csvFile}");

            // Сохраняем в TXT
            string txtFile = "fantasy_books.txt";
            searchEngine.SaveResultsToTxt(results, txtFile);
            Console.WriteLine($"   Сохранено {results.Count} книг в {txtFile}");
        }

        static void InteractiveSearch(SearchEngine searchEngine)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n   === ИНТЕРАКТИВНЫЙ ПОИСК ===");
                Console.WriteLine("   1. Поиск по автору");
                Console.WriteLine("   2. Поиск по названию");
                Console.WriteLine("   3. Поиск по жанру");
                Console.WriteLine("   4. Поиск по году");
                Console.WriteLine("   5. Поиск по рейтингу");
                Console.WriteLine("   6. Комбинированный поиск");
                Console.WriteLine("   7. Показать историю");
                Console.WriteLine("   8. Показать рекомендации");
                Console.WriteLine("   9. Выход");
                Console.Write("\n   Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Поиск по автору
                        Console.Write("   Введите автора: ");
                        string author = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(author))
                        {
                            DataView results = searchEngine.SearchByAuthor(author);
                            Console.WriteLine($"\n   Найдено: {results.Count} книг");
                            DisplaySearchResults(results, 10);
                        }
                        break;

                    case "2": // Поиск по названию
                        Console.Write("   Введите название: ");
                        string title = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(title))
                        {
                            DataView results = searchEngine.SearchByTitle(title);
                            Console.WriteLine($"\n   Найдено: {results.Count} книг");
                            DisplaySearchResults(results, 10);
                        }
                        break;

                    case "3": // Поиск по жанру
                        Console.Write("   Введите жанр: ");
                        string genre = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(genre))
                        {
                            DataView results = searchEngine.SearchByGenre(genre);
                            Console.WriteLine($"\n   Найдено: {results.Count} книг");
                            DisplaySearchResults(results, 10);
                        }
                        break;

                    case "4": // Поиск по году
                        Console.Write("   Минимальный год: ");
                        if (int.TryParse(Console.ReadLine(), out int minYear))
                        {
                            Console.Write("   Максимальный год: ");
                            if (int.TryParse(Console.ReadLine(), out int maxYear))
                            {
                                DataView results = searchEngine.SearchByYear(minYear, maxYear);
                                Console.WriteLine($"\n   Найдено: {results.Count} книг");
                                DisplaySearchResults(results, 10);
                            }
                        }
                        break;

                    case "5": // Поиск по рейтингу
                        Console.Write("   Минимальный рейтинг (0-5): ");
                        if (double.TryParse(Console.ReadLine(), out double minRating))
                        {
                            DataView results = searchEngine.SearchByRating(minRating);
                            Console.WriteLine($"\n   Найдено: {results.Count} книг");
                            DisplaySearchResults(results, 10);
                        }
                        break;

                    case "6": // Комбинированный поиск
                        Console.WriteLine("   Комбинированный поиск (оставьте пустым, если не важно):");
                        Console.Write("   Автор: ");
                        string cAuthor = Console.ReadLine();
                        Console.Write("   Название: ");
                        string cTitle = Console.ReadLine();
                        Console.Write("   Жанр: ");
                        string cGenre = Console.ReadLine();
                        Console.Write("   Минимальный год: ");
                        string cMinYear = Console.ReadLine();
                        Console.Write("   Минимальный рейтинг: ");
                        string cMinRating = Console.ReadLine();

                        DataView cResults = searchEngine.CombinedSearch(
                            author: cAuthor,
                            title: cTitle,
                            genre: cGenre,
                            minYear: string.IsNullOrEmpty(cMinYear) ? (int?)null : int.Parse(cMinYear),
                            minRating: string.IsNullOrEmpty(cMinRating) ? (double?)null : double.Parse(cMinRating)
                        );

                        Console.WriteLine($"\n   Найдено: {cResults.Count} книг");
                        DisplaySearchResults(cResults, 10);
                        break;

                    case "7": // История
                        DisplaySearchHistory(searchEngine.SearchHistory);
                        break;

                    case "8": // Рекомендации
                        DisplayRecommendations(searchEngine);
                        break;

                    case "9": // Выход
                        exit = true;
                        Console.WriteLine("   Выход из интерактивного поиска");
                        break;

                    default:
                        Console.WriteLine("   Неверный выбор");
                        break;
                }
            }
        }

        class SearchEngine
        {
            private DataTable _library;
            public List<SearchHistoryEntry> SearchHistory { get; private set; }

            public SearchEngine(DataTable library)
            {
                _library = library;
                SearchHistory = new List<SearchHistoryEntry>();
            }

            public DataView SearchByAuthor(string author)
            {
                var view = new DataView(_library);
                if (!string.IsNullOrWhiteSpace(author))
                {
                    view.RowFilter = $"Author LIKE '%{author}%'";
                }

                AddToHistory("По автору", author, view.Count);
                return view;
            }

            public DataView SearchByTitle(string title)
            {
                var view = new DataView(_library);
                if (!string.IsNullOrWhiteSpace(title))
                {
                    view.RowFilter = $"Title LIKE '%{title}%'";
                }

                AddToHistory("По названию", title, view.Count);
                return view;
            }

            public DataView SearchByGenre(string genre)
            {
                var view = new DataView(_library);
                if (!string.IsNullOrWhiteSpace(genre))
                {
                    view.RowFilter = $"Genre = '{genre}'";
                }

                AddToHistory("По жанру", genre, view.Count);
                return view;
            }

            public DataView SearchByYear(int minYear, int maxYear)
            {
                var view = new DataView(_library);
                view.RowFilter = $"Year >= {minYear} AND Year <= {maxYear}";

                AddToHistory("По году", $"{minYear}-{maxYear}", view.Count);
                return view;
            }

            public DataView SearchByRating(double minRating)
            {
                var view = new DataView(_library);
                view.RowFilter = $"Rating >= {minRating}";

                AddToHistory("По рейтингу", $">= {minRating}", view.Count);
                return view;
            }

            public DataView CombinedSearch(string author = null, string title = null,
                                         string genre = null, int? minYear = null,
                                         double? minRating = null)
            {
                var conditions = new List<string>();

                if (!string.IsNullOrWhiteSpace(author))
                    conditions.Add($"Author LIKE '%{author}%'");

                if (!string.IsNullOrWhiteSpace(title))
                    conditions.Add($"Title LIKE '%{title}%'");

                if (!string.IsNullOrWhiteSpace(genre))
                    conditions.Add($"Genre = '{genre}'");

                if (minYear.HasValue)
                    conditions.Add($"Year >= {minYear.Value}");

                if (minRating.HasValue)
                    conditions.Add($"Rating >= {minRating.Value}");

                var view = new DataView(_library);
                if (conditions.Any())
                {
                    view.RowFilter = string.Join(" AND ", conditions);
                }

                string searchTerm = $"Автор: {author}, Название: {title}, Жанр: {genre}, " +
                                   $"Год от: {minYear}, Рейтинг от: {minRating}";

                AddToHistory("Комбинированный", searchTerm, view.Count);
                return view;
            }

            public List<DataRow> GetRecommendations()
            {
                var recommendations = new List<DataRow>();

                // Получаем последние поиски
                var recentSearches = SearchHistory.TakeLast(10).ToList();

                if (recentSearches.Count == 0)
                {
                    // Если нет истории, рекомендуем книги с высоким рейтингом
                    var highRatedBooks = new DataView(_library);
                    highRatedBooks.RowFilter = "Rating >= 4.5";
                    highRatedBooks.Sort = "Rating DESC";

                    foreach (DataRowView row in highRatedBooks)
                    {
                        recommendations.Add(row.Row);
                    }

                    return recommendations.Take(10).ToList();
                }

                // Анализируем историю поиска
                var popularGenres = recentSearches
                    .Where(h => !string.IsNullOrEmpty(h.SearchTerm))
                    .GroupBy(h => h.SearchTerm)
                    .OrderByDescending(g => g.Count())
                    .Take(3)
                    .Select(g => g.Key)
                    .ToList();

                // Рекомендуем книги по популярным жанрам
                foreach (string genre in popularGenres)
                {
                    var genreBooks = new DataView(_library);
                    genreBooks.RowFilter = $"Genre = '{genre}' AND Rating >= 4.0";
                    genreBooks.Sort = "Rating DESC";

                    foreach (DataRowView row in genreBooks)
                    {
                        if (!recommendations.Any(r => r["BookID"].Equals(row["BookID"])))
                        {
                            recommendations.Add(row.Row);
                        }
                    }
                }

                return recommendations.Take(10).ToList();
            }

            public void SaveResultsToCsv(DataView results, string fileName)
            {
                try
                {
                    using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
                    {
                        // Заголовок
                        writer.WriteLine("Title,Author,Year,Genre,Rating,Pages,ISBN");

                        // Данные
                        foreach (DataRowView row in results)
                        {
                            writer.WriteLine($"\"{row["Title"]}\",\"{row["Author"]}\"," +
                                            $"{row["Year"]},\"{row["Genre"]}\"," +
                                            $"{row["Rating"]},{row["Pages"]},\"{row["ISBN"]}\"");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   Ошибка сохранения: {ex.Message}");
                }
            }

            public void SaveResultsToTxt(DataView results, string fileName)
            {
                try
                {
                    using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
                    {
                        writer.WriteLine($"Результаты поиска ({results.Count} книг)");
                        writer.WriteLine(new string('=', 60));

                        foreach (DataRowView row in results)
                        {
                            writer.WriteLine($"Название: {row["Title"]}");
                            writer.WriteLine($"Автор: {row["Author"]}");
                            writer.WriteLine($"Год: {row["Year"]} | Жанр: {row["Genre"]} | Рейтинг: {row["Rating"]}★");
                            writer.WriteLine($"Страниц: {row["Pages"]} | ISBN: {row["ISBN"]}");
                            writer.WriteLine(new string('-', 60));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   Ошибка сохранения: {ex.Message}");
                }
            }

            private void AddToHistory(string searchType, string searchTerm, int resultsCount)
            {
                SearchHistory.Add(new SearchHistoryEntry
                {
                    Timestamp = DateTime.Now,
                    SearchType = searchType,
                    SearchTerm = searchTerm,
                    ResultsCount = resultsCount
                });

                // Ограничиваем историю 100 записями
                if (SearchHistory.Count > 100)
                {
                    SearchHistory.RemoveAt(0);
                }
            }
        }

        class SearchHistoryEntry
        {
            public DateTime Timestamp { get; set; }
            public string SearchType { get; set; }
            public string SearchTerm { get; set; }
            public int ResultsCount { get; set; }
        }
        #endregion
        #region Задание 29: Использование DataView для создания пользовательских представлений в приложении
        static void Task29_UserSpecificViews()
        {
            Console.WriteLine("ЗАДАНИЕ 29: Использование DataView для создания пользовательских представлений в приложении\n");
            Console.WriteLine("Цель: Создание системы представлений с ограничениями доступа\n");

            // 1. Создание таблицы сотрудников
            Console.WriteLine("1. Создание таблицы сотрудников...");
            DataTable employees = CreateEmployeesTableForUserViews();
            FillEmployeesDataForUserViews(employees, 100);

            Console.WriteLine($"   Создано сотрудников: {employees.Rows.Count}");
            Console.WriteLine("   Примеры сотрудников:");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"   • {employees.Rows[i]["Name"]} - {employees.Rows[i]["Department"]}");
            }

            // 2. Определение пользователей и их прав
            Console.WriteLine("\n2. Определение пользователей:");

            var users = new Dictionary<string, UserProfile>
            {
                ["HR Manager"] = new UserProfile
                {
                    Role = "HR Manager",
                    CanViewSalary = true,
                    CanEditData = true,
                    CanDeleteData = true,
                    AllowedDepartments = null, // Все отделы
                    MinSalary = 0,
                    MaxVisibleSalary = 1000000,
                    ViewColumns = new[] { "EmployeeID", "Name", "Department", "Position", "Salary", "HireDate", "Status", "Email", "Phone" }
                },
                ["IT Head"] = new UserProfile
                {
                    Role = "IT Head",
                    CanViewSalary = true,
                    CanEditData = true,
                    CanDeleteData = false,
                    AllowedDepartments = new[] { "IT" },
                    MinSalary = 0,
                    MaxVisibleSalary = 150000,
                    ViewColumns = new[] { "EmployeeID", "Name", "Department", "Position", "Salary", "HireDate", "Email" }
                },
                ["Finance"] = new UserProfile
                {
                    Role = "Finance",
                    CanViewSalary = true,
                    CanEditData = false,
                    CanDeleteData = false,
                    AllowedDepartments = null,
                    MinSalary = 50000,
                    MaxVisibleSalary = 200000,
                    ViewColumns = new[] { "EmployeeID", "Name", "Department", "Salary", "Status", "HireDate" }
                },
                ["Sales Manager"] = new UserProfile
                {
                    Role = "Sales Manager",
                    CanViewSalary = false,
                    CanEditData = false,
                    CanDeleteData = false,
                    AllowedDepartments = new[] { "Sales", "Marketing" },
                    MinSalary = 0,
                    MaxVisibleSalary = 0,
                    ViewColumns = new[] { "EmployeeID", "Name", "Department", "Position", "HireDate", "Email" }
                },
                ["General User"] = new UserProfile
                {
                    Role = "General User",
                    CanViewSalary = false,
                    CanEditData = false,
                    CanDeleteData = false,
                    AllowedDepartments = null,
                    MinSalary = 0,
                    MaxVisibleSalary = 0,
                    ViewColumns = new[] { "EmployeeID", "Name", "Department", "Position" }
                }
            };

            foreach (var user in users)
            {
                Console.WriteLine($"   • {user.Key}: {user.Value.Role}");
                Console.WriteLine($"     - Просмотр зарплаты: {user.Value.CanViewSalary}");
                Console.WriteLine($"     - Редактирование: {user.Value.CanEditData}");
                Console.WriteLine($"     - Удаление: {user.Value.CanDeleteData}");
                Console.WriteLine($"     - Колонки: {string.Join(", ", user.Value.ViewColumns)}");
            }

            // 3. Создание системы аудита
            var auditLog = new List<AuditLogEntry>();

            // 4. Тестирование представлений для каждого пользователя
            Console.WriteLine("\n3. Тестирование представлений:");

            foreach (var user in users)
            {
                Console.WriteLine($"\n   === Представление для: {user.Key} ===");

                // Создание DataView для пользователя
                DataView userView = CreateUserView(employees, user.Value);

                // Логирование доступа
                auditLog.Add(new AuditLogEntry
                {
                    Timestamp = DateTime.Now,
                    User = user.Key,
                    Role = user.Value.Role,
                    Action = "View",
                    Details = $"Просмотр данных сотрудников ({userView.Count} записей)",
                    IPAddress = "192.168.1." + new Random().Next(1, 255)
                });

                Console.WriteLine($"   Доступно записей: {userView.Count}");
                Console.WriteLine($"   Доступные колонки: {string.Join(", ", user.Value.ViewColumns)}");
                Console.WriteLine($"   Фильтр: {GetFilterDescription(user.Value)}");

                // Отображение первых 3 записей
                Console.WriteLine("\n   Первые 3 записи:");
                DisplayEmployeeView(userView, user.Value, 3);

                // Тест редактирования
                TestEditPermissions(user.Key, user.Value, employees, auditLog);

                // Тест удаления
                TestDeletePermissions(user.Key, user.Value, employees, auditLog);
            }

            // 5. Логи доступа
            Console.WriteLine("\n4. Логи доступа:");
            DisplayAuditLog(auditLog);

            // 6. Отчёт о доступе к данным
            Console.WriteLine("\n5. Отчёт о доступе к данным:");
            GenerateAccessReport(users, auditLog, employees);

            // 7. Интерфейс выбора представления
            Console.WriteLine("\n6. Интерактивный выбор представления:");
            InteractiveViewSelection(employees, users, auditLog);

            // 8. Экспорт данных
            Console.WriteLine("\n7. Экспорт данных:");
            TestDataExport(employees, users);

            // 9. Анализ безопасности
            Console.WriteLine("\n8. Анализ безопасности доступа:");
            AnalyzeSecurityMetrics(users, auditLog);
        }

        #region Методы для работы с данными
        static DataTable CreateEmployeesTableForUserViews()
        {
            DataTable table = new DataTable("Сотрудники");

            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("Position", typeof(string));
            table.Columns.Add("Salary", typeof(decimal));
            table.Columns.Add("HireDate", typeof(DateTime));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Phone", typeof(string));
            table.Columns.Add("ManagerID", typeof(int));
            table.Columns.Add("OfficeLocation", typeof(string));
            table.Columns.Add("LastPromotionDate", typeof(DateTime));
            table.Columns.Add("VacationDays", typeof(int));
            table.Columns.Add("PerformanceRating", typeof(double));

            table.PrimaryKey = new DataColumn[] { table.Columns["EmployeeID"] };

            // Добавляем ограничения
            table.Columns["EmployeeID"].AutoIncrement = true;
            table.Columns["EmployeeID"].AutoIncrementSeed = 1;
            table.Columns["EmployeeID"].AutoIncrementStep = 1;
            table.Columns["Name"].AllowDBNull = false;
            table.Columns["Salary"].DefaultValue = 0;
            table.Columns["HireDate"].DefaultValue = DateTime.Now;
            table.Columns["Status"].DefaultValue = "Active";
            table.Columns["PerformanceRating"].DefaultValue = 3.0;

            return table;
        }

        static void FillEmployeesDataForUserViews(DataTable table, int count)
        {
            string[] departments = { "IT", "HR", "Finance", "Sales", "Marketing", "Operations", "Support", "R&D", "Legal", "Admin" };
            string[] positions = { "Manager", "Senior Developer", "Developer", "Analyst", "Specialist",
                          "Director", "Coordinator", "Assistant", "Consultant", "Engineer" };
            string[] firstNames = { "Иван", "Мария", "Петр", "Анна", "Дмитрий", "Светлана", "Алексей", "Елена",
                           "Сергей", "Ольга", "Андрей", "Наталья", "Михаил", "Татьяна", "Владимир" };
            string[] lastNames = { "Иванов", "Петрова", "Сидоров", "Смирнова", "Кузнецов", "Попова",
                          "Васильев", "Морозова", "Новиков", "Федорова", "Козлов", "Николаева" };
            string[] statuses = { "Active", "Inactive", "On Leave", "On Probation", "Retired" };
            string[] offices = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2010, 1, 1);

            for (int i = 1; i <= count; i++)
            {
                string department = departments[rand.Next(departments.Length)];
                string position = positions[rand.Next(positions.Length)];

                // Базовая зарплата в зависимости от должности и отдела
                decimal baseSalary = position switch
                {
                    "Manager" or "Director" => 120000 + (decimal)rand.NextDouble() * 80000,
                    "Senior Developer" or "Senior Analyst" => 90000 + (decimal)rand.NextDouble() * 60000,
                    "Developer" or "Analyst" => 60000 + (decimal)rand.NextDouble() * 40000,
                    _ => 40000 + (decimal)rand.NextDouble() * 30000
                };

                // Корректировка по отделу
                baseSalary *= department switch
                {
                    "IT" or "Finance" => 1.2m,
                    "Sales" => 1.3m,
                    "R&D" => 1.15m,
                    _ => 1.0m
                };

                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];
                string email = $"{firstName.ToLower()[0]}.{lastName.ToLower()}@company.com";

                table.Rows.Add(
                    i,
                    $"{lastName} {firstName}",
                    department,
                    position,
                    Math.Round(baseSalary, 2),
                    startDate.AddDays(rand.Next(0, 4745)), // До 13 лет
                    statuses[rand.Next(statuses.Length)],
                    email.Replace(" ", ""),
                    $"+7-{900 + rand.Next(100)}-{rand.Next(100, 1000)}-{rand.Next(1000, 10000)}",
                    rand.Next(1, 10), // ManagerID
                    offices[rand.Next(offices.Length)],
                    startDate.AddDays(rand.Next(365, 365 * 5)), // Последнее повышение
                    rand.Next(10, 30), // Дни отпуска
                    Math.Round(2 + rand.NextDouble() * 3, 1) // Рейтинг 2.0-5.0
                );
            }
        }

        static DataView CreateUserView(DataTable employees, UserProfile profile)
        {
            DataView view = new DataView(employees);

            // Применяем фильтры
            var filters = new List<string>();

            // Фильтр по отделам
            if (profile.AllowedDepartments != null && profile.AllowedDepartments.Any())
            {
                string deptFilter = string.Join(" OR ", profile.AllowedDepartments.Select(d => $"Department = '{d}'"));
                filters.Add($"({deptFilter})");
            }

            // Фильтр по минимальной зарплате
            if (profile.MinSalary > 0)
            {
                filters.Add($"Salary >= {profile.MinSalary}");
            }

            // Фильтр по максимальной видимой зарплате
            if (profile.MaxVisibleSalary > 0)
            {
                filters.Add($"Salary <= {profile.MaxVisibleSalary}");
            }

            // Фильтр по статусу (обычно показываем только активных, если не HR)
            if (profile.Role != "HR Manager")
            {
                filters.Add($"Status = 'Active'");
            }

            // Применяем фильтры
            if (filters.Any())
            {
                view.RowFilter = string.Join(" AND ", filters);
            }

            // Скрываем колонки, которые пользователь не должен видеть
            if (!profile.CanViewSalary && profile.ViewColumns.Contains("Salary"))
            {
                profile.ViewColumns = profile.ViewColumns.Where(c => c != "Salary").ToArray();
            }

            // Настраиваем сортировку
            view.Sort = "Department ASC, Name ASC";

            return view;
        }

        static void DisplayEmployeeView(DataView view, UserProfile profile, int maxRows)
        {
            if (view.Count == 0)
            {
                Console.WriteLine("   Нет данных");
                return;
            }

            // Определяем ширину колонок
            var columnWidths = new Dictionary<string, int>
            {
                ["EmployeeID"] = 5,
                ["Name"] = 25,
                ["Department"] = 15,
                ["Position"] = 20,
                ["Salary"] = 12,
                ["HireDate"] = 12,
                ["Status"] = 12,
                ["Email"] = 25,
                ["Phone"] = 15,
                ["OfficeLocation"] = 15,
                ["PerformanceRating"] = 8
            };

            // Заголовок
            Console.WriteLine();
            string header = "";
            foreach (string column in profile.ViewColumns)
            {
                if (columnWidths.ContainsKey(column))
                {
                    header += column.PadRight(columnWidths[column]) + " ";
                }
            }
            Console.WriteLine($"   {header}");
            Console.WriteLine($"   {new string('-', header.Length)}");

            // Данные
            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                string row = "";
                foreach (string column in profile.ViewColumns)
                {
                    object value = view[i][column];
                    string formattedValue = FormatValue(value, column, profile);

                    if (columnWidths.ContainsKey(column))
                    {
                        row += formattedValue.PadRight(columnWidths[column]) + " ";
                    }
                }
                Console.WriteLine($"   {row}");
            }

            if (view.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {view.Count - maxRows} записей");
            }
        }

        static string FormatValue(object value, string column, UserProfile profile)
        {
            if (value == DBNull.Value || value == null)
                return "N/A";

            switch (column)
            {
                case "Salary":
                    if (profile.CanViewSalary)
                        return ((decimal)value).ToString("C0");
                    else
                        return "*****";

                case "HireDate":
                case "LastPromotionDate":
                    return ((DateTime)value).ToString("dd.MM.yyyy");

                case "PerformanceRating":
                    double rating = (double)value;
                    return $"{rating:F1}★";

                default:
                    return value.ToString();
            }
        }

        static string GetFilterDescription(UserProfile profile)
        {
            var filters = new List<string>();

            if (profile.AllowedDepartments != null && profile.AllowedDepartments.Any())
            {
                filters.Add($"Отделы: {string.Join(", ", profile.AllowedDepartments)}");
            }

            if (profile.MinSalary > 0)
            {
                filters.Add($"Зарплата ≥ {profile.MinSalary:C0}");
            }

            if (profile.MaxVisibleSalary > 0)
            {
                filters.Add($"Зарплата ≤ {profile.MaxVisibleSalary:C0}");
            }

            if (profile.Role != "HR Manager")
            {
                filters.Add("Только активные");
            }

            return filters.Any() ? string.Join("; ", filters) : "Без фильтров";
        }
        #endregion

        #region Тестирование прав доступа
        static void TestEditPermissions(string userName, UserProfile profile, DataTable employees, List<AuditLogEntry> auditLog)
        {
            Console.Write($"   Тест редактирования: ");

            if (profile.CanEditData)
            {
                try
                {
                    // Пробуем изменить зарплату первого доступного сотрудника
                    DataView userView = CreateUserView(employees, profile);
                    if (userView.Count > 0)
                    {
                        DataRow employee = userView[0].Row;
                        decimal oldSalary = (decimal)employee["Salary"];
                        decimal newSalary = oldSalary * 1.1m; // Увеличиваем на 10%

                        // Проверяем, может ли пользователь видеть зарплату для редактирования
                        if (profile.CanViewSalary)
                        {
                            employee["Salary"] = newSalary;

                            auditLog.Add(new AuditLogEntry
                            {
                                Timestamp = DateTime.Now,
                                User = userName,
                                Role = profile.Role,
                                Action = "Edit",
                                Details = $"Изменение зарплаты сотрудника {employee["Name"]}: {oldSalary:C0} → {newSalary:C0}",
                                IPAddress = "192.168.1." + new Random().Next(1, 255)
                            });

                            Console.WriteLine("✓ Разрешено (зарплата изменена)");
                        }
                        else
                        {
                            // Пробуем изменить другие поля
                            string oldOffice = employee["OfficeLocation"].ToString();
                            string newOffice = oldOffice == "Москва" ? "Санкт-Петербург" : "Москва";
                            employee["OfficeLocation"] = newOffice;

                            auditLog.Add(new AuditLogEntry
                            {
                                Timestamp = DateTime.Now,
                                User = userName,
                                Role = profile.Role,
                                Action = "Edit",
                                Details = $"Изменение офиса сотрудника {employee["Name"]}: {oldOffice} → {newOffice}",
                                IPAddress = "192.168.1." + new Random().Next(1, 255)
                            });

                            Console.WriteLine("✓ Разрешено (офис изменен)");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Ошибка: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("✗ Запрещено (как и должно быть)");
            }
        }

        static void TestDeletePermissions(string userName, UserProfile profile, DataTable employees, List<AuditLogEntry> auditLog)
        {
            Console.Write($"   Тест удаления: ");

            if (profile.CanDeleteData)
            {
                // Пробуем удалить тестовую запись (не реальную)
                try
                {
                    // Создаем временного сотрудника для теста удаления
                    DataRow testEmployee = employees.NewRow();
                    testEmployee["EmployeeID"] = 99999;
                    testEmployee["Name"] = "Тестовый Сотрудник";
                    testEmployee["Department"] = "Test";
                    testEmployee["Position"] = "Tester";
                    testEmployee["Salary"] = 1;
                    testEmployee["Status"] = "Active";
                    employees.Rows.Add(testEmployee);

                    // Удаляем его
                    testEmployee.Delete();

                    auditLog.Add(new AuditLogEntry
                    {
                        Timestamp = DateTime.Now,
                        User = userName,
                        Role = profile.Role,
                        Action = "Delete",
                        Details = "Удаление тестового сотрудника",
                        IPAddress = "192.168.1." + new Random().Next(1, 255)
                    });

                    Console.WriteLine("✓ Разрешено (тестовый сотрудник удален)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Ошибка: {ex.Message}");
                }
            }
            else
            {
                try
                {
                    // Пробуем удалить запись без прав
                    if (employees.Rows.Count > 0)
                    {
                        employees.Rows[0].Delete();
                        Console.WriteLine("✗ Не должно быть разрешено!");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("✓ Запрещено (как и должно быть)");
                }
            }
        }
        #endregion

        #region Логирование и отчеты
        static void DisplayAuditLog(List<AuditLogEntry> auditLog)
        {
            if (auditLog.Count == 0)
            {
                Console.WriteLine("   Лог пуст");
                return;
            }

            Console.WriteLine($"   Всего записей: {auditLog.Count}");
            Console.WriteLine("\n   Последние 15 записей:");

            Console.WriteLine($"   {"Время",-12} {"Пользователь",-15} {"Роль",-15} {"Действие",-10} {"Детали",-35} {"IP",-15}");
            Console.WriteLine($"   {new string('-', 108)}");

            foreach (var entry in auditLog.TakeLast(15))
            {
                Console.WriteLine($"   {entry.Timestamp:HH:mm:ss} {entry.User,-15} {entry.Role,-15} {entry.Action,-10} " +
                                 $"{TruncateString(entry.Details, 32),-35} {entry.IPAddress,-15}");
            }

            // Статистика по пользователям
            var userStats = auditLog.GroupBy(a => a.User)
                                   .Select(g => new { User = g.Key, Actions = g.Count() })
                                   .OrderByDescending(g => g.Actions)
                                   .ToList();

            Console.WriteLine("\n   Статистика по пользователям:");
            foreach (var stat in userStats)
            {
                Console.WriteLine($"   • {stat.User}: {stat.Actions} действий");
            }

            // Статистика по типам действий
            var actionStats = auditLog.GroupBy(a => a.Action)
                                     .Select(g => new { Action = g.Key, Count = g.Count() })
                                     .OrderByDescending(g => g.Count)
                                     .ToList();

            Console.WriteLine("\n   Статистика по действиям:");
            foreach (var stat in actionStats)
            {
                Console.WriteLine($"   • {stat.Action}: {stat.Count} раз");
            }
        }

        static void GenerateAccessReport(Dictionary<string, UserProfile> users, List<AuditLogEntry> auditLog, DataTable employees)
        {
            Console.WriteLine("   Отчёт о доступе к данным:");
            Console.WriteLine($"   Всего сотрудников: {employees.Rows.Count}");

            // Статистика по отделам
            var deptStats = employees.AsEnumerable()
                .Where(r => r.Field<string>("Status") == "Active")
                .GroupBy(r => r.Field<string>("Department"))
                .Select(g => new {
                    Department = g.Key,
                    Count = g.Count(),
                    AvgSalary = g.Average(r => r.Field<decimal>("Salary")),
                    MaxSalary = g.Max(r => r.Field<decimal>("Salary")),
                    MinSalary = g.Min(r => r.Field<decimal>("Salary"))
                })
                .OrderByDescending(g => g.AvgSalary)
                .ToList();

            Console.WriteLine("\n   Статистика по отделам (только активные сотрудники):");
            Console.WriteLine($"   {"Отдел",-15} {"Сотр.",-7} {"Ср.зарплата",-15} {"Мин",-12} {"Макс",-12}");
            Console.WriteLine($"   {new string('-', 65)}");

            foreach (var stat in deptStats)
            {
                Console.WriteLine($"   {stat.Department,-15} {stat.Count,-7} {stat.AvgSalary:C0,-15} {stat.MinSalary:C0,-12} {stat.MaxSalary:C0,-12}");
            }

            // Анализ доступа пользователей
            Console.WriteLine("\n   Анализ уровня доступа пользователей:");
            foreach (var user in users)
            {
                int accessibleCount = CreateUserView(employees, user.Value).Count;
                double accessibilityPercent = (double)accessibleCount / employees.Rows.Count * 100;

                Console.WriteLine($"   • {user.Key}:");
                Console.WriteLine($"     - Доступно записей: {accessibleCount} из {employees.Rows.Count} ({accessibilityPercent:F1}%)");
                Console.WriteLine($"     - Доступно колонок: {user.Value.ViewColumns.Length} из {employees.Columns.Count}");
                Console.WriteLine($"     - Права редактирования: {user.Value.CanEditData}");
                Console.WriteLine($"     - Права удаления: {user.Value.CanDeleteData}");
            }
        }

        static void AnalyzeSecurityMetrics(Dictionary<string, UserProfile> users, List<AuditLogEntry> auditLog)
        {
            Console.WriteLine("   Метрики безопасности:");

            // 1. Принцип минимальных привилегий
            Console.WriteLine("\n   1. Принцип минимальных привилегий:");
            foreach (var user in users)
            {
                bool followsPrinciple = !(user.Value.CanEditData && !user.Value.CanViewSalary) &&
                                       !(user.Value.CanDeleteData && !user.Value.CanEditData);

                Console.WriteLine($"   • {user.Key}: {(followsPrinciple ? "✓" : "✗")}");
            }

            // 2. Активность пользователей
            Console.WriteLine("\n   2. Активность пользователей за последний час:");
            DateTime lastHour = DateTime.Now.AddHours(-1);
            var recentActivity = auditLog.Where(a => a.Timestamp > lastHour)
                                        .GroupBy(a => a.User)
                                        .Select(g => new { User = g.Key, Activity = g.Count() })
                                        .OrderByDescending(g => g.Activity);

            foreach (var activity in recentActivity)
            {
                Console.WriteLine($"   • {activity.User}: {activity.Activity} действий");
            }

            // 3. Рискованные операции
            var riskyOperations = auditLog.Where(a => a.Action == "Delete" ||
                                                    (a.Action == "Edit" && a.Details.Contains("Salary")))
                                         .ToList();

            Console.WriteLine($"\n   3. Рискованные операции (всего {riskyOperations.Count}):");
            foreach (var op in riskyOperations.TakeLast(5))
            {
                Console.WriteLine($"   • {op.Timestamp:HH:mm:ss} {op.User}: {op.Details}");
            }
        }
        #endregion

        #region Экспорт данных
        static void TestDataExport(DataTable employees, Dictionary<string, UserProfile> users)
        {
            Console.WriteLine("   Тестирование экспорта данных:");

            // Для каждого типа пользователя создаем представление и экспортируем
            foreach (var user in users.Take(2)) // Берем только двух пользователей для демонстрации
            {
                Console.WriteLine($"\n   Экспорт для {user.Key}:");

                DataView userView = CreateUserView(employees, user.Value);

                if (userView.Count > 0)
                {
                    // Экспорт в CSV
                    string csvFile = $"export_{user.Key.Replace(" ", "_")}.csv";
                    ExportViewToCsv(userView, user.Value, csvFile);
                    Console.WriteLine($"   • CSV: {csvFile} ({userView.Count} записей)");

                    // Экспорт в TXT
                    string txtFile = $"export_{user.Key.Replace(" ", "_")}.txt";
                    ExportViewToTxt(userView, user.Value, txtFile);
                    Console.WriteLine($"   • TXT: {txtFile}");

                    // Экспорт в HTML
                    string htmlFile = $"export_{user.Key.Replace(" ", "_")}.html";
                    ExportViewToHtml(userView, user.Value, htmlFile);
                    Console.WriteLine($"   • HTML: {htmlFile}");
                }
            }
        }

        static void ExportViewToCsv(DataView view, UserProfile profile, string fileName)
        {
            try
            {
                using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    // Заголовок
                    writer.WriteLine(string.Join(",", profile.ViewColumns.Select(c => $"\"{c}\"")));

                    // Данные
                    foreach (DataRowView rowView in view)
                    {
                        var values = new List<string>();
                        foreach (string column in profile.ViewColumns)
                        {
                            object value = rowView[column];
                            string stringValue = FormatValueForExport(value, column, profile);

                            // Экранирование
                            if (stringValue.Contains("\"") || stringValue.Contains(","))
                            {
                                stringValue = $"\"{stringValue.Replace("\"", "\"\"")}\"";
                            }

                            values.Add(stringValue);
                        }
                        writer.WriteLine(string.Join(",", values));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Ошибка экспорта в CSV: {ex.Message}");
            }
        }

        static void ExportViewToTxt(DataView view, UserProfile profile, string fileName)
        {
            try
            {
                using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    writer.WriteLine($"Отчет для: {profile.Role}");
                    writer.WriteLine($"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm}");
                    writer.WriteLine($"Количество записей: {view.Count}");
                    writer.WriteLine(new string('=', 80));

                    foreach (DataRowView rowView in view)
                    {
                        foreach (string column in profile.ViewColumns)
                        {
                            object value = rowView[column];
                            string stringValue = FormatValueForExport(value, column, profile);
                            writer.WriteLine($"{column}: {stringValue}");
                        }
                        writer.WriteLine(new string('-', 80));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Ошибка экспорта в TXT: {ex.Message}");
            }
        }

        static void ExportViewToHtml(DataView view, UserProfile profile, string fileName)
        {
            try
            {
                using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    writer.WriteLine("<!DOCTYPE html>");
                    writer.WriteLine("<html>");
                    writer.WriteLine("<head>");
                    writer.WriteLine("<meta charset=\"UTF-8\">");
                    writer.WriteLine("<title>Отчет - " + profile.Role + "</title>");
                    writer.WriteLine("<style>");
                    writer.WriteLine("table { border-collapse: collapse; width: 100%; margin: 20px 0; }");
                    writer.WriteLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
                    writer.WriteLine("th { background-color: #4CAF50; color: white; }");
                    writer.WriteLine("tr:nth-child(even) { background-color: #f2f2f2; }");
                    writer.WriteLine(".header { background-color: #333; color: white; padding: 10px; }");
                    writer.WriteLine("</style>");
                    writer.WriteLine("</head>");
                    writer.WriteLine("<body>");
                    writer.WriteLine("<div class=\"header\">");
                    writer.WriteLine($"<h1>Отчет: {profile.Role}</h1>");
                    writer.WriteLine($"<p>Дата: {DateTime.Now:dd.MM.yyyy HH:mm}</p>");
                    writer.WriteLine($"<p>Записей: {view.Count}</p>");
                    writer.WriteLine("</div>");

                    if (view.Count > 0)
                    {
                        writer.WriteLine("<table>");
                        writer.WriteLine("<tr>");

                        // Заголовки
                        foreach (string column in profile.ViewColumns)
                        {
                            writer.WriteLine($"<th>{column}</th>");
                        }
                        writer.WriteLine("</tr>");

                        // Данные
                        foreach (DataRowView rowView in view)
                        {
                            writer.WriteLine("<tr>");
                            foreach (string column in profile.ViewColumns)
                            {
                                object value = rowView[column];
                                string stringValue = FormatValueForExport(value, column, profile);
                                writer.WriteLine($"<td>{stringValue}</td>");
                            }
                            writer.WriteLine("</tr>");
                        }

                        writer.WriteLine("</table>");
                    }

                    writer.WriteLine("</body>");
                    writer.WriteLine("</html>");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Ошибка экспорта в HTML: {ex.Message}");
            }
        }

        static string FormatValueForExport(object value, string column, UserProfile profile)
        {
            if (value == DBNull.Value || value == null)
                return "";

            switch (column)
            {
                case "Salary":
                    if (profile.CanViewSalary)
                        return ((decimal)value).ToString("F2");
                    else
                        return "HIDDEN";

                case "HireDate":
                case "LastPromotionDate":
                    return ((DateTime)value).ToString("yyyy-MM-dd");

                default:
                    return value.ToString();
            }
        }
        #endregion

        #region Интерактивный интерфейс
        static void InteractiveViewSelection(DataTable employees, Dictionary<string, UserProfile> users, List<AuditLogEntry> auditLog)
        {
            bool exit = false;

            Console.WriteLine("\n   === ИНТЕРАКТИВНЫЙ ВЫБОР ПРЕДСТАВЛЕНИЯ ===");

            while (!exit)
            {
                Console.WriteLine("\n   Доступные пользователи:");
                int index = 1;
                foreach (var user in users)
                {
                    Console.WriteLine($"   {index}. {user.Key} ({user.Value.Role})");
                    index++;
                }
                Console.WriteLine($"   {index}. Выход");
                Console.Write("\n   Выберите пользователя: ");

                string choice = Console.ReadLine();

                if (choice == index.ToString())
                {
                    exit = true;
                    Console.WriteLine("   Выход из интерактивного режима");
                    continue;
                }

                if (int.TryParse(choice, out int userIndex) && userIndex >= 1 && userIndex <= users.Count)
                {
                    var user = users.ElementAt(userIndex - 1);

                    Console.WriteLine($"\n   Вы выбрали: {user.Key}");
                    Console.WriteLine("   Доступные действия:");
                    Console.WriteLine("   1. Просмотреть данные");
                    Console.WriteLine("   2. Изменить фильтр");
                    Console.WriteLine("   3. Экспортировать данные");
                    Console.WriteLine("   4. Назад к выбору пользователя");
                    Console.Write("\n   Выберите действие: ");

                    string action = Console.ReadLine();

                    switch (action)
                    {
                        case "1": // Просмотр
                            DataView userView = CreateUserView(employees, user.Value);
                            Console.WriteLine($"\n   Данные для {user.Key}:");
                            DisplayEmployeeView(userView, user.Value, 10);

                            // Логируем просмотр
                            auditLog.Add(new AuditLogEntry
                            {
                                Timestamp = DateTime.Now,
                                User = $"Interactive_{user.Key}",
                                Role = user.Value.Role,
                                Action = "InteractiveView",
                                Details = $"Интерактивный просмотр {userView.Count} записей",
                                IPAddress = "127.0.0.1"
                            });
                            break;

                        case "2": // Изменить фильтр
                            Console.WriteLine("\n   Текущие ограничения доступа:");
                            Console.WriteLine($"   • Отделы: {(user.Value.AllowedDepartments == null ? "Все" : string.Join(", ", user.Value.AllowedDepartments))}");
                            Console.WriteLine($"   • Минимальная зарплата: {user.Value.MinSalary:C0}");
                            Console.WriteLine($"   • Максимальная видимая зарплата: {(user.Value.MaxVisibleSalary > 0 ? user.Value.MaxVisibleSalary.ToString("C0") : "Неограничено")}");
                            Console.WriteLine($"   • Может видеть зарплату: {user.Value.CanViewSalary}");
                            Console.WriteLine($"   • Может редактировать: {user.Value.CanEditData}");
                            Console.WriteLine($"   • Может удалять: {user.Value.CanDeleteData}");
                            break;

                        case "3": // Экспорт
                            userView = CreateUserView(employees, user.Value);
                            if (userView.Count > 0)
                            {
                                string exportFile = $"interactive_export_{user.Key.Replace(" ", "_")}.csv";
                                ExportViewToCsv(userView, user.Value, exportFile);
                                Console.WriteLine($"   Данные экспортированы в {exportFile}");
                            }
                            else
                            {
                                Console.WriteLine("   Нет данных для экспорта");
                            }
                            break;

                        case "4": // Назад
                            break;

                        default:
                            Console.WriteLine("   Неверный выбор");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("   Неверный выбор пользователя");
                }
            }
        }
        #endregion

        #region Вспомогательные методы для UserViews
        static string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
        }
        #endregion

        #region Классы моделей
        class UserProfile
        {
            public string Role { get; set; }
            public bool CanViewSalary { get; set; }
            public bool CanEditData { get; set; }
            public bool CanDeleteData { get; set; }
            public string[] AllowedDepartments { get; set; }
            public decimal MinSalary { get; set; }
            public decimal MaxVisibleSalary { get; set; }
            public string[] ViewColumns { get; set; }
        }

        class AuditLogEntry
        {
            public DateTime Timestamp { get; set; }
            public string User { get; set; }
            public string Role { get; set; }
            public string Action { get; set; }
            public string Details { get; set; }
            public string IPAddress { get; set; }
        }
        #endregion
        #endregion

        #region Задание 30: Комплексное приложение "Управление проектами" с использованием всех концепций DataView
        static void Task30_ProjectManagementSystem()
        {
            Console.WriteLine("ЗАДАНИЕ 30: Комплексное приложение 'Управление проектами'\n");
            Console.WriteLine("Цель: Демонстрация всех концепций DataView в реальном приложении\n");

            // 1. Создание и заполнение всех таблиц
            Console.WriteLine("1. Создание и заполнение базы данных проектов...");

            DataTable projects = CreateProjectsTable();
            DataTable tasks = CreateTasksTable();
            DataTable resources = CreateResourcesTable();
            DataTable workLogs = CreateWorkLogsTable();
            DataTable employees = CreateProjectEmployeesTable();

            // Установка связей между таблицами
            SetupRelationships(projects, tasks, resources, workLogs, employees);

            // Заполнение данными
            FillEmployeesData(employees);
            FillProjectsData(projects, employees);
            FillTasksData(tasks, projects, employees);
            FillResourcesData(resources, projects);
            FillWorkLogsData(workLogs, tasks, resources);

            Console.WriteLine("   Таблицы созданы:");
            Console.WriteLine($"   • Проекты: {projects.Rows.Count} записей");
            Console.WriteLine($"   • Задачи: {tasks.Rows.Count} записей");
            Console.WriteLine($"   • Ресурсы: {resources.Rows.Count} записей");
            Console.WriteLine($"   • Логи работы: {workLogs.Rows.Count} записей");
            Console.WriteLine($"   • Сотрудники: {employees.Rows.Count} записей");

            // 2. Создание системы управления проектами
            Console.WriteLine("\n2. Инициализация системы управления проектами...");
            ProjectManagementSystem pms = new ProjectManagementSystem(
                projects, tasks, resources, workLogs, employees
            );

            // 3. Тестирование различных представлений
            Console.WriteLine("\n3. Тестирование представлений данных:");

            // 3.1 Текущие проекты
            Console.WriteLine("\n   а) Текущие проекты (статус = 'In Progress'):");
            DataView currentProjects = pms.GetCurrentProjectsView();
            DisplayProjectsView(currentProjects, 3);

            // 3.2 Задачи по приоритету
            Console.WriteLine("\n   б) Задачи по приоритету (High):");
            DataView highPriorityTasks = pms.GetTasksByPriorityView("High");
            DisplayTasksView(highPriorityTasks, 3);

            // 3.3 Задачи по сотруднику
            Console.WriteLine("\n   в) Задачи сотрудника ID 1:");
            DataView employeeTasks = pms.GetEmployeeTasksView(1);
            DisplayTasksView(employeeTasks, 3);

            // 3.4 Задачи по статусу
            Console.WriteLine("\n   г) Задачи в статусе 'Pending':");
            DataView pendingTasks = pms.GetTasksByStatusView("Pending");
            DisplayTasksView(pendingTasks, 3);

            // 3.5 Перегруженные сотрудники
            Console.WriteLine("\n   д) Перегруженные сотрудники (> 40 часов/неделю):");
            DataView overloadedEmployees = pms.GetOverloadedEmployeesView();
            DisplayEmployeesView(overloadedEmployees, 3);

            // 4. CRUD операции
            Console.WriteLine("\n4. Тестирование CRUD операций:");

            // 4.1 Добавление проекта
            Console.WriteLine("\n   а) Добавление нового проекта:");
            DataRow newProject = pms.AddProject(
                "Mobile App Development",
                "In Progress",
                DateTime.Now,
                DateTime.Now.AddMonths(6),
                1,
                50000
            );
            Console.WriteLine($"   Добавлен проект: {newProject["ProjectName"]}");

            // 4.2 Добавление задачи
            Console.WriteLine("\n   б) Добавление новой задачи:");
            DataRow newTask = pms.AddTask(
                1,
                "Design UI Prototype",
                2,
                "Pending",
                "High",
                DateTime.Now.AddDays(14)
            );
            Console.WriteLine($"   Добавлена задача: {newTask["TaskName"]}");

            // 4.3 Изменение статуса задачи
            Console.WriteLine("\n   в) Изменение статуса задачи:");
            bool updated = pms.UpdateTaskStatus(1, "In Progress");
            Console.WriteLine($"   Статус задачи обновлен: {updated}");

            // 4.4 Удаление задачи
            Console.WriteLine("\n   г) Удаление задачи:");
            bool deleted = pms.DeleteTask(100); // Несуществующая задача для демонстрации
            Console.WriteLine($"   Задача удалена: {deleted}");

            // 5. Поиск и фильтрация
            Console.WriteLine("\n5. Тестирование поиска и фильтрации:");

            // 5.1 Поиск проектов по менеджеру
            Console.WriteLine("\n   а) Поиск проектов по менеджеру ID 1:");
            DataView managerProjects = pms.SearchProjectsByManager(1);
            DisplayProjectsView(managerProjects, 3);

            // 5.2 Поиск задач по диапазону дат
            Console.WriteLine("\n   б) Задачи с дедлайном в этом месяце:");
            DataView thisMonthTasks = pms.SearchTasksByDateRange(
                DateTime.Now,
                DateTime.Now.AddMonths(1)
            );
            DisplayTasksView(thisMonthTasks, 3);

            // 5.3 Комплексный поиск
            Console.WriteLine("\n   в) Комплексный поиск (High priority, In Progress):");
            DataView complexSearch = pms.ComplexTaskSearch(
                priority: "High",
                status: "In Progress",
                minDueDate: DateTime.Now,
                maxDueDate: DateTime.Now.AddMonths(1)
            );
            DisplayTasksView(complexSearch, 3);

            // 6. Расчеты и анализ
            Console.WriteLine("\n6. Расчеты и анализ:");

            // 6.1 Расчет затрат проекта
            Console.WriteLine("\n   а) Расчет затрат проекта ID 1:");
            decimal projectCost = pms.CalculateProjectCost(1);
            Console.WriteLine($"   Общие затраты проекта: {projectCost:C}");

            // 6.2 Прогноз завершения проекта
            Console.WriteLine("\n   б) Прогноз завершения проекта ID 1:");
            DateTime? completionDate = pms.ForecastProjectCompletion(1);
            Console.WriteLine($"   Прогноз завершения: {completionDate:dd.MM.yyyy}");

            // 6.3 Анализ использования ресурсов
            Console.WriteLine("\n   в) Анализ использования ресурсов:");
            var resourceUsage = pms.AnalyzeResourceUsage();
            DisplayResourceUsage(resourceUsage);

            // 7. Отчеты
            Console.WriteLine("\n7. Генерация отчетов:");

            // 7.1 Статус проектов
            Console.WriteLine("\n   а) Отчет о статусе проектов:");
            GenerateProjectStatusReport(pms);

            // 7.2 Использование ресурсов
            Console.WriteLine("\n   б) Отчет об использовании ресурсов:");
            GenerateResourceUsageReport(pms);

            // 7.3 Финансовый отчет
            Console.WriteLine("\n   в) Финансовый отчет:");
            GenerateFinancialReport(pms);

            // 8. Валидация и обработка ошибок
            Console.WriteLine("\n8. Тестирование валидации и обработки ошибок:");

            // 8.1 Попытка добавления проекта с невалидными данными
            Console.WriteLine("\n   а) Попытка добавления проекта без названия:");
            try
            {
                DataRow invalidProject = pms.AddProject(
                    "", // Пустое название
                    "In Progress",
                    DateTime.Now,
                    DateTime.Now.AddMonths(1),
                    1,
                    10000
                );
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"   Ошибка валидации: {ex.Message}");
            }

            // 8.2 Попытка добавления задачи в несуществующий проект
            Console.WriteLine("\n   б) Попытка добавления задачи в несуществующий проект:");
            try
            {
                DataRow invalidTask = pms.AddTask(
                    999, // Несуществующий проект
                    "Invalid Task",
                    1,
                    "Pending",
                    "High",
                    DateTime.Now.AddDays(7)
                );
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"   Ошибка валидации: {ex.Message}");
            }

            // 9. Логирование
            Console.WriteLine("\n9. Логирование действий:");
            DisplayActivityLog(pms.ActivityLog);

            // 10. Экспорт данных
            Console.WriteLine("\n10. Экспорт данных:");

            // 10.1 Экспорт текущих проектов в CSV
            Console.WriteLine("\n   а) Экспорт текущих проектов в CSV:");
            string csvFile = "current_projects.csv";
            pms.ExportToCsv(currentProjects, csvFile);
            Console.WriteLine($"   Экспортировано в: {csvFile}");

            // 10.2 Экспорт отчета в HTML
            Console.WriteLine("\n   б) Экспорт отчета в HTML:");
            string htmlFile = "project_report.html";
            pms.ExportToHtml(pms.GetCurrentProjectsView(), htmlFile);
            Console.WriteLine($"   Экспортировано в: {htmlFile}");

            // 11. Интерфейс системы
            Console.WriteLine("\n11. Интерактивный интерфейс системы:");
            InteractiveProjectManagement(pms);

            Console.WriteLine("\n=== Система управления проектами успешно протестирована ===");
        }

        #region Создание таблиц
        static DataTable CreateProjectsTable()
        {
            DataTable table = new DataTable("Проекты");

            table.Columns.Add("ProjectID", typeof(int));
            table.Columns.Add("ProjectName", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("StartDate", typeof(DateTime));
            table.Columns.Add("EndDate", typeof(DateTime));
            table.Columns.Add("ManagerID", typeof(int));
            table.Columns.Add("Budget", typeof(decimal));
            table.Columns.Add("ActualCost", typeof(decimal));
            table.Columns.Add("CompletionPercent", typeof(int));

            table.PrimaryKey = new DataColumn[] { table.Columns["ProjectID"] };

            // Добавление ограничений
            table.Columns["ProjectName"].AllowDBNull = false;
            table.Columns["Status"].DefaultValue = "Planned";
            table.Columns["CompletionPercent"].DefaultValue = 0;

            return table;
        }

        static DataTable CreateTasksTable()
        {
            DataTable table = new DataTable("Задачи");

            table.Columns.Add("TaskID", typeof(int));
            table.Columns.Add("ProjectID", typeof(int));
            table.Columns.Add("TaskName", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("AssignedTo", typeof(int));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Priority", typeof(string));
            table.Columns.Add("DueDate", typeof(DateTime));
            table.Columns.Add("EstimatedHours", typeof(decimal));
            table.Columns.Add("ActualHours", typeof(decimal));
            table.Columns.Add("CreatedDate", typeof(DateTime));
            table.Columns.Add("CompletedDate", typeof(DateTime));

            table.PrimaryKey = new DataColumn[] { table.Columns["TaskID"] };

            // Добавление ограничений
            table.Columns["TaskName"].AllowDBNull = false;
            table.Columns["Status"].DefaultValue = "Pending";
            table.Columns["Priority"].DefaultValue = "Medium";
            table.Columns["CreatedDate"].DefaultValue = DateTime.Now;

            return table;
        }

        static DataTable CreateResourcesTable()
        {
            DataTable table = new DataTable("Ресурсы");

            table.Columns.Add("ResourceID", typeof(int));
            table.Columns.Add("ResourceName", typeof(string));
            table.Columns.Add("Type", typeof(string)); // Employee, Equipment, Material
            table.Columns.Add("ProjectID", typeof(int));
            table.Columns.Add("HourlyRate", typeof(decimal));
            table.Columns.Add("Availability", typeof(string)); // Full-time, Part-time, As needed
            table.Columns.Add("Skills", typeof(string));

            table.PrimaryKey = new DataColumn[] { table.Columns["ResourceID"] };

            return table;
        }

        static DataTable CreateWorkLogsTable()
        {
            DataTable table = new DataTable("ЛогиРаботы");

            table.Columns.Add("LogID", typeof(int));
            table.Columns.Add("TaskID", typeof(int));
            table.Columns.Add("ResourceID", typeof(int));
            table.Columns.Add("Hours", typeof(decimal));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Notes", typeof(string));
            table.Columns.Add("Billable", typeof(bool));

            table.PrimaryKey = new DataColumn[] { table.Columns["LogID"] };

            // Добавление ограничений
            table.Columns["Hours"].DefaultValue = 0;
            table.Columns["Date"].DefaultValue = DateTime.Now;
            table.Columns["Billable"].DefaultValue = true;

            return table;
        }

        static DataTable CreateProjectEmployeesTable()
        {
            DataTable table = new DataTable("Сотрудники");

            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("FullName", typeof(string));
            table.Columns.Add("Position", typeof(string));
            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Phone", typeof(string));
            table.Columns.Add("HourlyRate", typeof(decimal));
            table.Columns.Add("WeeklyHours", typeof(decimal));

            table.PrimaryKey = new DataColumn[] { table.Columns["EmployeeID"] };

            return table;
        }

        static void SetupRelationships(DataTable projects, DataTable tasks, DataTable resources, DataTable workLogs, DataTable employees)
        {
            DataSet dataSet = new DataSet("ProjectManagement");

            // Добавляем таблицы в DataSet
            dataSet.Tables.AddRange(new[] { projects, tasks, resources, workLogs, employees });

            // Создаем связи
            // Проекты -> Задачи
            DataRelation projTasksRel = new DataRelation(
                "ProjectTasks",
                projects.Columns["ProjectID"],
                tasks.Columns["ProjectID"],
                true
            );
            dataSet.Relations.Add(projTasksRel);

            // Задачи -> Логи работы
            DataRelation taskLogsRel = new DataRelation(
                "TaskWorkLogs",
                tasks.Columns["TaskID"],
                workLogs.Columns["TaskID"],
                true
            );
            dataSet.Relations.Add(taskLogsRel);

            // Ресурсы -> Логи работы
            DataRelation resourceLogsRel = new DataRelation(
                "ResourceWorkLogs",
                resources.Columns["ResourceID"],
                workLogs.Columns["ResourceID"],
                true
            );
            dataSet.Relations.Add(resourceLogsRel);

            // Проекты -> Ресурсы
            DataRelation projResourcesRel = new DataRelation(
                "ProjectResources",
                projects.Columns["ProjectID"],
                resources.Columns["ProjectID"],
                true
            );
            dataSet.Relations.Add(projResourcesRel);

            // Сотрудники -> Задачи (назначенные)
            DataRelation empTasksRel = new DataRelation(
                "EmployeeTasks",
                employees.Columns["EmployeeID"],
                tasks.Columns["AssignedTo"],
                false
            );
            dataSet.Relations.Add(empTasksRel);
        }
        #endregion

        #region Заполнение данных
        static void FillEmployeesData(DataTable employees)
        {
            string[] positions = { "Project Manager", "Senior Developer", "Developer", "QA Engineer",
                          "UI/UX Designer", "Business Analyst", "System Architect" };

            string[] departments = { "IT", "Engineering", "Design", "Quality Assurance", "Management" };

            Random rand = new Random();

            for (int i = 1; i <= 20; i++)
            {
                string firstName = i % 2 == 0 ? "Мария" : "Иван";
                string lastName = i % 3 == 0 ? "Петрова" : i % 3 == 1 ? "Иванов" : "Сидоров";

                employees.Rows.Add(
                    i,
                    $"{lastName} {firstName}",
                    positions[rand.Next(positions.Length)],
                    departments[rand.Next(departments.Length)],
                    $"employee{i}@company.com",
                    $"+7-{900 + rand.Next(100)}-{rand.Next(100, 1000)}-{rand.Next(1000, 10000)}",
                    Math.Round(20 + rand.NextDouble() * 80, 2), // 20-100 в час
                    Math.Round(30 + rand.NextDouble() * 10, 1)  // 30-40 часов в неделю
                );
            }
        }

        static void FillProjectsData(DataTable projects, DataTable employees)
        {
            string[] projectNames = {
        "Разработка корпоративного портала",
        "Модернизация CRM системы",
        "Создание мобильного приложения",
        "Миграция на облачную инфраструктуру",
        "Внедрение системы BI",
        "Разработка API для партнеров",
        "Обновление веб-сайта компании",
        "Автоматизация бизнес-процессов",
        "Внедрение системы документооборота",
        "Разработка системы аналитики"
    };

            string[] statuses = { "Planned", "In Progress", "On Hold", "Completed", "Cancelled" };

            Random rand = new Random();
            DateTime startDate = new DateTime(2023, 1, 1);

            for (int i = 1; i <= 10; i++)
            {
                DateTime projectStart = startDate.AddDays(rand.Next(0, 365));

                projects.Rows.Add(
                    i,
                    $"{projectNames[rand.Next(projectNames.Length)]} v{i}",
                    $"Описание проекта {i}",
                    statuses[rand.Next(statuses.Length)],
                    projectStart,
                    projectStart.AddDays(rand.Next(60, 180)),
                    rand.Next(1, employees.Rows.Count + 1),
                    Math.Round(10000 + rand.NextDouble() * 90000, 2), // 10000-100000
                    Math.Round((decimal)(10000 + rand.NextDouble() * 90000) * 0.7m, 2), // 70% от бюджета
                    rand.Next(0, 101)
                );
            }
        }

        static void FillTasksData(DataTable tasks, DataTable projects, DataTable employees)
        {
            string[] taskNames = {
        "Анализ требований",
        "Проектирование архитектуры",
        "Разработка интерфейса",
        "Написание кода",
        "Тестирование модулей",
        "Интеграционное тестирование",
        "Документирование",
        "Развертывание",
        "Обучение пользователей",
        "Поддержка"
    };

            string[] priorities = { "Low", "Medium", "High", "Critical" };
            string[] statuses = { "Pending", "In Progress", "Completed", "On Hold", "Cancelled" };

            Random rand = new Random();
            int taskId = 1;

            foreach (DataRow project in projects.Rows)
            {
                int projectId = (int)project["ProjectID"];
                int taskCount = rand.Next(5, 15); // 5-14 задач на проект

                for (int i = 0; i < taskCount; i++)
                {
                    DateTime dueDate = ((DateTime)project["StartDate"]).AddDays(rand.Next(1, 180));

                    tasks.Rows.Add(
                        taskId++,
                        projectId,
                        $"{taskNames[rand.Next(taskNames.Length)]} для проекта {projectId}",
                        $"Описание задачи {taskId}",
                        rand.Next(1, employees.Rows.Count + 1),
                        statuses[rand.Next(statuses.Length)],
                        priorities[rand.Next(priorities.Length)],
                        dueDate,
                        Math.Round(8 + rand.NextDouble() * 40, 1), // 8-48 часов
                        Math.Round((8 + rand.NextDouble() * 40) * rand.NextDouble(), 1), // 0-фактические часы
                        ((DateTime)project["StartDate"]).AddDays(rand.Next(0, 30)),
                        dueDate < DateTime.Now && rand.NextDouble() > 0.7 ?
                            dueDate.AddDays(rand.Next(0, 14)) : (object)DBNull.Value
                    );
                }
            }
        }

        static void FillResourcesData(DataTable resources, DataTable projects)
        {
            Random rand = new Random();

            for (int i = 1; i <= 30; i++)
            {
                string type = rand.NextDouble() > 0.5 ? "Employee" : "Equipment";
                string resourceName = type == "Employee" ?
                    $"Сотрудник {i}" :
                    $"Оборудование {i}";

                resources.Rows.Add(
                    i,
                    resourceName,
                    type,
                    rand.Next(1, projects.Rows.Count + 1),
                    type == "Employee" ? Math.Round(20 + rand.NextDouble() * 80, 2) : 0,
                    rand.NextDouble() > 0.7 ? "Full-time" : "Part-time",
                    type == "Employee" ? $"Навыки {i}" : "Технические характеристики"
                );
            }
        }

        static void FillWorkLogsData(DataTable workLogs, DataTable tasks, DataTable resources)
        {
            Random rand = new Random();

            for (int i = 1; i <= 500; i++)
            {
                DataRow task = tasks.Rows[rand.Next(tasks.Rows.Count)];

                workLogs.Rows.Add(
                    i,
                    task["TaskID"],
                    rand.Next(1, resources.Rows.Count + 1),
                    Math.Round(1 + rand.NextDouble() * 7, 1), // 1-8 часов
                    ((DateTime)task["CreatedDate"]).AddDays(rand.Next(0, 60)),
                    $"Запись работы {i}",
                    rand.NextDouble() > 0.2 // 80% billable
                );
            }
        }
        #endregion

        #region Вспомогательные методы отображения
        static void DisplayProjectsView(DataView view, int maxRows)
        {
            if (view.Count == 0)
            {
                Console.WriteLine("   Нет данных");
                return;
            }

            Console.WriteLine($"   Найдено проектов: {view.Count}");
            Console.WriteLine($"   {"ID",-5} {"Название",-35} {"Статус",-15} {"Бюджет",-12} {"Завершение",-10}");
            Console.WriteLine($"   {new string('-', 82)}");

            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"   {view[i]["ProjectID"],-5} " +
                                $"{Truncate(view[i]["ProjectName"].ToString(), 32),-35} " +
                                $"{view[i]["Status"],-15} " +
                                $"{((decimal)view[i]["Budget"]):C0,-12} " +
                                $"{view[i]["CompletionPercent"]}%");
            }

            if (view.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {view.Count - maxRows} проектов");
            }
        }

        static void DisplayTasksView(DataView view, int maxRows)
        {
            if (view.Count == 0)
            {
                Console.WriteLine("   Нет данных");
                return;
            }

            Console.WriteLine($"   Найдено задач: {view.Count}");
            Console.WriteLine($"   {"ID",-5} {"Название",-30} {"Приоритет",-10} {"Статус",-12} {"Срок",-12}");
            Console.WriteLine($"   {new string('-', 74)}");

            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                DateTime dueDate = (DateTime)view[i]["DueDate"];
                string dueDateStr = dueDate.ToString("dd.MM.yyyy");

                Console.WriteLine($"   {view[i]["TaskID"],-5} " +
                                $"{Truncate(view[i]["TaskName"].ToString(), 27),-30} " +
                                $"{view[i]["Priority"],-10} " +
                                $"{view[i]["Status"],-12} " +
                                $"{dueDateStr,-12}");
            }

            if (view.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {view.Count - maxRows} задач");
            }
        }

        static void DisplayEmployeesView(DataView view, int maxRows)
        {
            if (view.Count == 0)
            {
                Console.WriteLine("   Нет данных");
                return;
            }

            Console.WriteLine($"   Найдено сотрудников: {view.Count}");
            Console.WriteLine($"   {"ID",-5} {"Имя",-25} {"Должность",-20} {"Часы/нед",-10}");
            Console.WriteLine($"   {new string('-', 65)}");

            for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
            {
                Console.WriteLine($"   {view[i]["EmployeeID"],-5} " +
                                $"{Truncate(view[i]["FullName"].ToString(), 22),-25} " +
                                $"{Truncate(view[i]["Position"].ToString(), 17),-20} " +
                                $"{view[i]["WeeklyHours"],-10:F1}");
            }

            if (view.Count > maxRows)
            {
                Console.WriteLine($"   ... и ещё {view.Count - maxRows} сотрудников");
            }
        }

        static void DisplayResourceUsage(Dictionary<string, decimal> usage)
        {
            if (usage.Count == 0)
            {
                Console.WriteLine("   Нет данных");
                return;
            }

            Console.WriteLine($"   {"Ресурс",-25} {"Использование (часы)",-20}");
            Console.WriteLine($"   {new string('-', 50)}");

            foreach (var kvp in usage.OrderByDescending(x => x.Value).Take(5))
            {
                Console.WriteLine($"   {Truncate(kvp.Key, 22),-25} {kvp.Value,-20:F1}");
            }
        }

        static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
        }
        #endregion

        #region Отчеты
        static void GenerateProjectStatusReport(ProjectManagementSystem pms)
        {
            var report = pms.GenerateProjectStatusReport();

            Console.WriteLine($"   {"Проект",-35} {"Статус",-15} {"Бюджет",-12} {"Затраты",-12} {"Остаток",-12} {"Завершение",-10}");
            Console.WriteLine($"   {new string('-', 101)}");

            foreach (var item in report)
            {
                decimal remaining = item.Budget - item.ActualCost;
                string statusColor = item.Status == "Completed" ? "✓" :
                                   item.Status == "In Progress" ? "→" :
                                   item.Status == "On Hold" ? "!" : " ";

                Console.WriteLine($"   {Truncate(item.ProjectName, 32),-35} " +
                                $"{statusColor} {item.Status,-13} " +
                                $"{item.Budget:C0,-12} " +
                                $"{item.ActualCost:C0,-12} " +
                                $"{remaining:C0,-12} " +
                                $"{item.CompletionPercent}%");
            }
        }

        static void GenerateResourceUsageReport(ProjectManagementSystem pms)
        {
            var report = pms.GenerateResourceUsageReport();

            Console.WriteLine($"   {"Ресурс",-25} {"Тип",-15} {"Проект",-20} {"Часы",-10} {"Стоимость",-12}");
            Console.WriteLine($"   {new string('-', 87)}");

            decimal totalCost = 0;

            foreach (var item in report.Take(10))
            {
                totalCost += item.Cost;

                Console.WriteLine($"   {Truncate(item.ResourceName, 22),-25} " +
                                $"{item.Type,-15} " +
                                $"{Truncate(item.ProjectName, 17),-20} " +
                                $"{item.TotalHours,-10:F1} " +
                                $"{item.Cost:C0,-12}");
            }

            Console.WriteLine($"   {new string('-', 87)}");
            Console.WriteLine($"   {"ВСЕГО:",-60} {totalCost:C0}");
        }

        static void GenerateFinancialReport(ProjectManagementSystem pms)
        {
            var report = pms.GenerateFinancialReport();

            Console.WriteLine($"   {"Проект",-35} {"Бюджет",-12} {"Затраты",-12} {"Прибыль",-12} {"Рентабельность",-15}");
            Console.WriteLine($"   {new string('-', 91)}");

            decimal totalBudget = 0;
            decimal totalCost = 0;

            foreach (var item in report)
            {
                totalBudget += item.Budget;
                totalCost += item.ActualCost;
                decimal profit = item.Budget - item.ActualCost;
                decimal margin = item.Budget > 0 ? (profit / item.Budget) * 100 : 0;

                Console.WriteLine($"   {Truncate(item.ProjectName, 32),-35} " +
                                $"{item.Budget:C0,-12} " +
                                $"{item.ActualCost:C0,-12} " +
                                $"{profit:C0,-12} " +
                                $"{margin:F1}%");
            }

            Console.WriteLine($"   {new string('-', 91)}");
            Console.WriteLine($"   {"ИТОГО:",-35} {totalBudget:C0,-12} {totalCost:C0,-12} " +
                            $"{(totalBudget - totalCost):C0,-12} " +
                            $"{(totalBudget > 0 ? ((totalBudget - totalCost) / totalBudget) * 100 : 0):F1}%");
        }

        static void GenerateOverdueTasksReport(ProjectManagementSystem pms)
        {
            var report = pms.GenerateOverdueTasksReport();

            Console.WriteLine($"   {"Задача",-30} {"Проект",-20} {"Назначена",-25} {"Просрочка",-10}");
            Console.WriteLine($"   {new string('-', 90)}");

            foreach (var item in report.Take(10))
            {
                int overdueDays = (DateTime.Now - item.DueDate).Days;

                Console.WriteLine($"   {Truncate(item.TaskName, 27),-30} " +
                                $"{Truncate(item.ProjectName, 17),-20} " +
                                $"{item.AssignedToName,-25} " +
                                $"{overdueDays} дн.");
            }

            if (report.Count > 10)
            {
                Console.WriteLine($"   ... и ещё {report.Count - 10} просроченных задач");
            }
        }
        #endregion

        #region Дополнительные методы
        static void DisplayActivityLog(List<ActivityLogEntry> log)
        {
            if (log.Count == 0)
            {
                Console.WriteLine("   Лог пуст");
                return;
            }

            Console.WriteLine($"   Всего записей: {log.Count}");
            Console.WriteLine("\n   Последние 10 записей:");

            Console.WriteLine($"   {"Время",-12} {"Действие",-20} {"Детали",-40}");
            Console.WriteLine($"   {new string('-', 77)}");

            foreach (var entry in log.TakeLast(10))
            {
                Console.WriteLine($"   {entry.Timestamp:HH:mm:ss} {entry.Action,-20} {Truncate(entry.Details, 36),-40}");
            }
        }

        static void InteractiveProjectManagement(ProjectManagementSystem pms)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n   === СИСТЕМА УПРАВЛЕНИЯ ПРОЕКТАМИ ===");
                Console.WriteLine("   1. Просмотреть проекты");
                Console.WriteLine("   2. Просмотреть задачи");
                Console.WriteLine("   3. Поиск проектов");
                Console.WriteLine("   4. Поиск задач");
                Console.WriteLine("   5. Добавить проект");
                Console.WriteLine("   6. Добавить задачу");
                Console.WriteLine("   7. Отчеты");
                Console.WriteLine("   8. Статистика");
                Console.WriteLine("   9. Логирование");
                Console.WriteLine("   0. Выход");
                Console.Write("\n   Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Просмотреть проекты
                        Console.WriteLine("\n   Типы представлений проектов:");
                        Console.WriteLine("   1. Все проекты");
                        Console.WriteLine("   2. Текущие проекты");
                        Console.WriteLine("   3. Проекты по статусу");
                        Console.Write("   Выберите тип: ");

                        string projectViewType = Console.ReadLine();

                        DataView projectsView = null;
                        switch (projectViewType)
                        {
                            case "1":
                                projectsView = pms.GetAllProjectsView();
                                break;
                            case "2":
                                projectsView = pms.GetCurrentProjectsView();
                                break;
                            case "3":
                                Console.Write("   Введите статус: ");
                                string status = Console.ReadLine();
                                projectsView = pms.GetProjectsByStatusView(status);
                                break;
                        }

                        if (projectsView != null)
                        {
                            Console.WriteLine("\n   Результаты:");
                            DisplayProjectsView(projectsView, 10);
                        }
                        break;

                    case "2": // Просмотреть задачи
                        Console.WriteLine("\n   Типы представлений задач:");
                        Console.WriteLine("   1. Все задачи");
                        Console.WriteLine("   2. Задачи по приоритету");
                        Console.WriteLine("   3. Задачи по статусу");
                        Console.WriteLine("   4. Задачи сотрудника");
                        Console.Write("   Выберите тип: ");

                        string taskViewType = Console.ReadLine();

                        DataView tasksView = null;
                        switch (taskViewType)
                        {
                            case "1":
                                tasksView = pms.GetAllTasksView();
                                break;
                            case "2":
                                Console.Write("   Введите приоритет: ");
                                string priority = Console.ReadLine();
                                tasksView = pms.GetTasksByPriorityView(priority);
                                break;
                            case "3":
                                Console.Write("   Введите статус: ");
                                string status = Console.ReadLine();
                                tasksView = pms.GetTasksByStatusView(status);
                                break;
                            case "4":
                                Console.Write("   Введите ID сотрудника: ");
                                if (int.TryParse(Console.ReadLine(), out int empId))
                                {
                                    tasksView = pms.GetEmployeeTasksView(empId);
                                }
                                break;
                        }

                        if (tasksView != null)
                        {
                            Console.WriteLine("\n   Результаты:");
                            DisplayTasksView(tasksView, 10);
                        }
                        break;

                    case "3": // Поиск проектов
                        Console.Write("\n   Введите название проекта: ");
                        string searchProjectName = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(searchProjectName))
                        {
                            var searchResults = pms.SearchProjectsByName(searchProjectName);
                            Console.WriteLine("\n   Результаты поиска:");
                            DisplayProjectsView(searchResults, 10);
                        }
                        break;

                    case "4": // Поиск задач
                        Console.WriteLine("\n   Критерии поиска задач:");
                        Console.Write("   Название задачи: ");
                        string searchTaskName = Console.ReadLine();
                        Console.Write("   Статус (оставьте пустым): ");
                        string searchStatus = Console.ReadLine();

                        var taskResults = pms.ComplexTaskSearch(
                            taskName: searchTaskName,
                            status: searchStatus
                        );

                        Console.WriteLine("\n   Результаты поиска:");
                        DisplayTasksView(taskResults, 10);
                        break;

                    case "5": // Добавить проект
                        try
                        {
                            Console.WriteLine("\n   Добавление нового проекта:");
                            Console.Write("   Название: ");
                            string newProjectName = Console.ReadLine();
                            Console.Write("   Описание: ");
                            string newProjectDesc = Console.ReadLine();
                            Console.Write("   Менеджер (ID): ");
                            int managerId = int.Parse(Console.ReadLine());
                            Console.Write("   Бюджет: ");
                            decimal budget = decimal.Parse(Console.ReadLine());

                            DataRow newProject = pms.AddProject(
                                newProjectName,
                                "Planned",
                                DateTime.Now,
                                DateTime.Now.AddMonths(6),
                                managerId,
                                budget
                            );

                            Console.WriteLine($"\n   Проект добавлен: ID {newProject["ProjectID"]}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"   Ошибка: {ex.Message}");
                        }
                        break;

                    case "6": // Добавить задачу
                        try
                        {
                            Console.WriteLine("\n   Добавление новой задачи:");
                            Console.Write("   ID проекта: ");
                            int projectId = int.Parse(Console.ReadLine());
                            Console.Write("   Название задачи: ");
                            string newTaskName = Console.ReadLine();
                            Console.Write("   Назначить на (ID сотрудника): ");
                            int assigneeId = int.Parse(Console.ReadLine());
                            Console.Write("   Приоритет: ");
                            string taskPriority = Console.ReadLine();
                            Console.Write("   Срок (в днях от сегодня): ");
                            int dueInDays = int.Parse(Console.ReadLine());

                            DataRow newTask = pms.AddTask(
                                projectId,
                                newTaskName,
                                assigneeId,
                                "Pending",
                                taskPriority,
                                DateTime.Now.AddDays(dueInDays)
                            );

                            Console.WriteLine($"\n   Задача добавлена: ID {newTask["TaskID"]}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"   Ошибка: {ex.Message}");
                        }
                        break;

                    case "7": // Отчеты
                        Console.WriteLine("\n   Доступные отчеты:");
                        Console.WriteLine("   1. Статус проектов");
                        Console.WriteLine("   2. Использование ресурсов");
                        Console.WriteLine("   3. Финансовый отчет");
                        Console.WriteLine("   4. Просроченные задачи");
                        Console.Write("   Выберите отчет: ");

                        string reportType = Console.ReadLine();

                        switch (reportType)
                        {
                            case "1":
                                Console.WriteLine("\n   Отчет о статусе проектов:");
                                GenerateProjectStatusReport(pms);
                                break;
                            case "2":
                                Console.WriteLine("\n   Отчет об использовании ресурсов:");
                                GenerateResourceUsageReport(pms);
                                break;
                            case "3":
                                Console.WriteLine("\n   Финансовый отчет:");
                                GenerateFinancialReport(pms);
                                break;
                            case "4":
                                Console.WriteLine("\n   Отчет о просроченных задачах:");
                                GenerateOverdueTasksReport(pms);
                                break;
                        }
                        break;

                    case "8": // Статистика
                        Console.WriteLine("\n   Статистика системы:");
                        Console.WriteLine($"   • Проектов: {pms.Projects.Rows.Count}");
                        Console.WriteLine($"   • Задач: {pms.Tasks.Rows.Count}");
                        Console.WriteLine($"   • Сотрудников: {pms.Employees.Rows.Count}");
                        Console.WriteLine($"   • Логов работы: {pms.WorkLogs.Rows.Count}");

                        decimal totalBudget = pms.Projects.AsEnumerable()
                            .Sum(r => r.Field<decimal>("Budget"));
                        decimal totalCost = pms.Projects.AsEnumerable()
                            .Sum(r => r.Field<decimal>("ActualCost"));

                        Console.WriteLine($"   • Общий бюджет: {totalBudget:C0}");
                        Console.WriteLine($"   • Общие затраты: {totalCost:C0}");
                        Console.WriteLine($"   • Общая прибыль: {(totalBudget - totalCost):C0}");
                        break;

                    case "9": // Логирование
                        Console.WriteLine("\n   Журнал активности системы:");
                        DisplayActivityLog(pms.ActivityLog);
                        break;

                    case "0": // Выход
                        exit = true;
                        Console.WriteLine("   Выход из системы управления проектами");
                        break;

                    default:
                        Console.WriteLine("   Неверный выбор");
                        break;
                }
            }
        }
        #endregion

        #region Классы системы
        class ProjectManagementSystem
        {
            public DataTable Projects { get; }
            public DataTable Tasks { get; }
            public DataTable Resources { get; }
            public DataTable WorkLogs { get; }
            public DataTable Employees { get; }
            public List<ActivityLogEntry> ActivityLog { get; }

            private Random _random;

            public ProjectManagementSystem(DataTable projects, DataTable tasks,
                                         DataTable resources, DataTable workLogs,
                                         DataTable employees)
            {
                Projects = projects;
                Tasks = tasks;
                Resources = resources;
                WorkLogs = workLogs;
                Employees = employees;
                ActivityLog = new List<ActivityLogEntry>();
                _random = new Random();

                // Подписка на события изменений
                SubscribeToEvents();

                LogActivity("System", "Система инициализирована");
            }

            #region CRUD операции
            public DataRow AddProject(string name, string status, DateTime startDate,
                                    DateTime endDate, int managerId, decimal budget)
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Название проекта не может быть пустым");

                if (budget <= 0)
                    throw new ArgumentException("Бюджет должен быть положительным числом");

                // Поиск менеджера
                DataRow manager = Employees.Rows.Find(managerId);
                if (manager == null)
                    throw new ArgumentException($"Сотрудник с ID {managerId} не найден");

                // Создание проекта
                DataRow newProject = Projects.NewRow();
                int newId = GetNextId(Projects, "ProjectID");

                newProject["ProjectID"] = newId;
                newProject["ProjectName"] = name;
                newProject["Description"] = $"Описание проекта '{name}'";
                newProject["Status"] = status;
                newProject["StartDate"] = startDate;
                newProject["EndDate"] = endDate;
                newProject["ManagerID"] = managerId;
                newProject["Budget"] = budget;
                newProject["ActualCost"] = 0;
                newProject["CompletionPercent"] = 0;

                Projects.Rows.Add(newProject);

                LogActivity("AddProject", $"Добавлен проект: {name} (ID: {newId})");

                return newProject;
            }

            public DataRow AddTask(int projectId, string taskName, int assignedTo,
                                  string status, string priority, DateTime dueDate)
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(taskName))
                    throw new ArgumentException("Название задачи не может быть пустым");

                DataRow project = Projects.Rows.Find(projectId);
                if (project == null)
                    throw new ArgumentException($"Проект с ID {projectId} не найден");

                DataRow employee = Employees.Rows.Find(assignedTo);
                if (employee == null)
                    throw new ArgumentException($"Сотрудник с ID {assignedTo} не найден");

                // Создание задачи
                DataRow newTask = Tasks.NewRow();
                int newId = GetNextId(Tasks, "TaskID");

                newTask["TaskID"] = newId;
                newTask["ProjectID"] = projectId;
                newTask["TaskName"] = taskName;
                newTask["Description"] = $"Описание задачи '{taskName}'";
                newTask["AssignedTo"] = assignedTo;
                newTask["Status"] = status;
                newTask["Priority"] = priority;
                newTask["DueDate"] = dueDate;
                newTask["EstimatedHours"] = Math.Round(8 + _random.NextDouble() * 24, 1); // 8-32 часов
                newTask["ActualHours"] = 0;
                newTask["CreatedDate"] = DateTime.Now;
                newTask["CompletedDate"] = DBNull.Value;

                Tasks.Rows.Add(newTask);

                LogActivity("AddTask", $"Добавлена задача: {taskName} (ID: {newId}) в проект {projectId}");

                return newTask;
            }

            public bool UpdateTaskStatus(int taskId, string newStatus)
            {
                DataRow task = Tasks.Rows.Find(taskId);
                if (task == null)
                    return false;

                string oldStatus = task["Status"].ToString();
                task["Status"] = newStatus;

                if (newStatus == "Completed")
                {
                    task["CompletedDate"] = DateTime.Now;
                }

                LogActivity("UpdateTask", $"Задача {taskId}: статус изменен с '{oldStatus}' на '{newStatus}'");

                return true;
            }

            public bool DeleteTask(int taskId)
            {
                DataRow task = Tasks.Rows.Find(taskId);
                if (task == null)
                    return false;

                // Проверяем, есть ли логи работы для этой задачи
                DataRow[] workLogs = WorkLogs.Select($"TaskID = {taskId}");
                if (workLogs.Length > 0)
                {
                    LogActivity("DeleteTask", $"Не удалось удалить задачу {taskId}: есть логи работы");
                    return false;
                }

                Tasks.Rows.Remove(task);

                LogActivity("DeleteTask", $"Задача удалена: {taskId}");

                return true;
            }
            #endregion

            #region Представления данных
            public DataView GetAllProjectsView()
            {
                return new DataView(Projects);
            }

            public DataView GetCurrentProjectsView()
            {
                DataView view = new DataView(Projects);
                view.RowFilter = "Status = 'In Progress'";
                view.Sort = "EndDate ASC";
                return view;
            }

            public DataView GetProjectsByStatusView(string status)
            {
                DataView view = new DataView(Projects);
                view.RowFilter = $"Status = '{status}'";
                view.Sort = "ProjectName ASC";
                return view;
            }

            public DataView GetAllTasksView()
            {
                return new DataView(Tasks);
            }

            public DataView GetTasksByPriorityView(string priority)
            {
                DataView view = new DataView(Tasks);
                view.RowFilter = $"Priority = '{priority}'";
                view.Sort = "DueDate ASC";
                return view;
            }

            public DataView GetTasksByStatusView(string status)
            {
                DataView view = new DataView(Tasks);
                view.RowFilter = $"Status = '{status}'";
                view.Sort = "Priority DESC, DueDate ASC";
                return view;
            }

            public DataView GetEmployeeTasksView(int employeeId)
            {
                DataView view = new DataView(Tasks);
                view.RowFilter = $"AssignedTo = {employeeId}";
                view.Sort = "DueDate ASC";
                return view;
            }

            public DataView GetOverloadedEmployeesView()
            {
                // Расчет часов работы каждого сотрудника за последнюю неделю
                DateTime lastWeek = DateTime.Now.AddDays(-7);

                var employeeHours = new Dictionary<int, decimal>();

                foreach (DataRow log in WorkLogs.Rows)
                {
                    DateTime logDate = (DateTime)log["Date"];
                    if (logDate >= lastWeek)
                    {
                        int resourceId = (int)log["ResourceID"];
                        decimal hours = (decimal)log["Hours"];

                        // Находим сотрудника по ресурсу
                        DataRow[] resources = Resources.Select($"ResourceID = {resourceId} AND Type = 'Employee'");
                        if (resources.Length > 0)
                        {
                            int employeeId = (int)resources[0]["ResourceID"];

                            if (!employeeHours.ContainsKey(employeeId))
                                employeeHours[employeeId] = 0;

                            employeeHours[employeeId] += hours;
                        }
                    }
                }

                // Фильтруем сотрудников с более чем 40 часами в неделю
                var overloadedEmployeeIds = employeeHours
                    .Where(kvp => kvp.Value > 40)
                    .Select(kvp => kvp.Key)
                    .ToList();

                DataView view = new DataView(Employees);
                if (overloadedEmployeeIds.Any())
                {
                    string filter = string.Join(" OR ", overloadedEmployeeIds.Select(id => $"EmployeeID = {id}"));
                    view.RowFilter = filter;
                }
                else
                {
                    view.RowFilter = "1 = 0"; // Нет результатов
                }

                view.Sort = "FullName ASC";
                return view;
            }
            #endregion

            #region Поиск и фильтрация
            public DataView SearchProjectsByName(string name)
            {
                DataView view = new DataView(Projects);
                view.RowFilter = $"ProjectName LIKE '%{name}%'";
                view.Sort = "ProjectName ASC";

                LogActivity("Search", $"Поиск проектов по названию: '{name}'");

                return view;
            }

            public DataView SearchProjectsByManager(int managerId)
            {
                DataView view = new DataView(Projects);
                view.RowFilter = $"ManagerID = {managerId}";
                view.Sort = "Status ASC, EndDate ASC";

                LogActivity("Search", $"Поиск проектов по менеджеру: {managerId}");

                return view;
            }

            public DataView SearchTasksByDateRange(DateTime startDate, DateTime endDate)
            {
                DataView view = new DataView(Tasks);
                view.RowFilter = $"DueDate >= #{startDate:yyyy-MM-dd}# AND DueDate <= #{endDate:yyyy-MM-dd}#";
                view.Sort = "DueDate ASC, Priority DESC";

                LogActivity("Search", $"Поиск задач по диапазону дат: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");

                return view;
            }

            public DataView ComplexTaskSearch(string taskName = null, string status = null,
                                            string priority = null, DateTime? minDueDate = null,
                                            DateTime? maxDueDate = null)
            {
                DataView view = new DataView(Tasks);
                var conditions = new List<string>();

                if (!string.IsNullOrWhiteSpace(taskName))
                    conditions.Add($"TaskName LIKE '%{taskName}%'");

                if (!string.IsNullOrWhiteSpace(status))
                    conditions.Add($"Status = '{status}'");

                if (!string.IsNullOrWhiteSpace(priority))
                    conditions.Add($"Priority = '{priority}'");

                if (minDueDate.HasValue)
                    conditions.Add($"DueDate >= #{minDueDate.Value:yyyy-MM-dd}#");

                if (maxDueDate.HasValue)
                    conditions.Add($"DueDate <= #{maxDueDate.Value:yyyy-MM-dd}#");

                if (conditions.Any())
                    view.RowFilter = string.Join(" AND ", conditions);

                view.Sort = "Priority DESC, DueDate ASC";

                LogActivity("Search", $"Комплексный поиск задач");

                return view;
            }
            #endregion

            #region Расчеты и анализ
            public decimal CalculateProjectCost(int projectId)
            {
                // Находим все задачи проекта
                DataRow[] projectTasks = Tasks.Select($"ProjectID = {projectId}");

                decimal totalCost = 0;

                foreach (DataRow task in projectTasks)
                {
                    int taskId = (int)task["TaskID"];

                    // Находим все логи работы для задачи
                    DataRow[] taskLogs = WorkLogs.Select($"TaskID = {taskId}");

                    foreach (DataRow log in taskLogs)
                    {
                        int resourceId = (int)log["ResourceID"];
                        decimal hours = (decimal)log["Hours"];
                        bool billable = (bool)log["Billable"];

                        if (billable)
                        {
                            // Находим ресурс
                            DataRow resource = Resources.Rows.Find(resourceId);
                            if (resource != null)
                            {
                                decimal hourlyRate = (decimal)resource["HourlyRate"];
                                totalCost += hours * hourlyRate;
                            }
                        }
                    }
                }

                LogActivity("Calculation", $"Расчет затрат проекта {projectId}: {totalCost:C}");

                return totalCost;
            }

            public DateTime? ForecastProjectCompletion(int projectId)
            {
                DataRow project = Projects.Rows.Find(projectId);
                if (project == null)
                    return null;

                DateTime startDate = (DateTime)project["StartDate"];
                DateTime plannedEndDate = (DateTime)project["EndDate"];
                int completionPercent = (int)project["CompletionPercent"];

                if (completionPercent <= 0)
                    return plannedEndDate;

                // Простая линейная экстраполяция
                TimeSpan elapsed = DateTime.Now - startDate;
                TimeSpan estimatedTotal = TimeSpan.FromTicks(elapsed.Ticks * 100 / completionPercent);

                DateTime forecast = startDate + estimatedTotal;

                LogActivity("Forecast", $"Прогноз завершения проекта {projectId}: {forecast:dd.MM.yyyy}");

                return forecast;
            }

            public Dictionary<string, decimal> AnalyzeResourceUsage()
            {
                var usage = new Dictionary<string, decimal>();

                foreach (DataRow log in WorkLogs.Rows)
                {
                    int resourceId = (int)log["ResourceID"];
                    decimal hours = (decimal)log["Hours"];

                    DataRow resource = Resources.Rows.Find(resourceId);
                    if (resource != null)
                    {
                        string resourceName = resource["ResourceName"].ToString();

                        if (!usage.ContainsKey(resourceName))
                            usage[resourceName] = 0;

                        usage[resourceName] += hours;
                    }
                }

                LogActivity("Analysis", "Анализ использования ресурсов");

                return usage;
            }
            #endregion

            #region Отчеты
            public List<ProjectStatusReportItem> GenerateProjectStatusReport()
            {
                var report = new List<ProjectStatusReportItem>();

                foreach (DataRow project in Projects.Rows)
                {
                    report.Add(new ProjectStatusReportItem
                    {
                        ProjectID = (int)project["ProjectID"],
                        ProjectName = project["ProjectName"].ToString(),
                        Status = project["Status"].ToString(),
                        Budget = (decimal)project["Budget"],
                        ActualCost = (decimal)project["ActualCost"],
                        CompletionPercent = (int)project["CompletionPercent"]
                    });
                }

                LogActivity("Report", "Сгенерирован отчет о статусе проектов");

                return report;
            }

            public List<ResourceUsageReportItem> GenerateResourceUsageReport()
            {
                var report = new List<ResourceUsageReportItem>();

                foreach (DataRow resource in Resources.Rows)
                {
                    int resourceId = (int)resource["ResourceID"];

                    // Суммируем часы работы ресурса
                    DataRow[] resourceLogs = WorkLogs.Select($"ResourceID = {resourceId}");
                    decimal totalHours = resourceLogs.Sum(log => (decimal)log["Hours"]);

                    if (totalHours > 0)
                    {
                        string projectName = "";
                        int projectId = (int)resource["ProjectID"];
                        DataRow project = Projects.Rows.Find(projectId);
                        if (project != null)
                        {
                            projectName = project["ProjectName"].ToString();
                        }

                        decimal hourlyRate = (decimal)resource["HourlyRate"];
                        decimal cost = totalHours * hourlyRate;

                        report.Add(new ResourceUsageReportItem
                        {
                            ResourceName = resource["ResourceName"].ToString(),
                            Type = resource["Type"].ToString(),
                            ProjectName = projectName,
                            TotalHours = totalHours,
                            Cost = cost
                        });
                    }
                }

                LogActivity("Report", "Сгенерирован отчет об использовании ресурсов");

                return report.OrderByDescending(r => r.Cost).ToList();
            }

            public List<FinancialReportItem> GenerateFinancialReport()
            {
                var report = new List<FinancialReportItem>();

                foreach (DataRow project in Projects.Rows)
                {
                    report.Add(new FinancialReportItem
                    {
                        ProjectID = (int)project["ProjectID"],
                        ProjectName = project["ProjectName"].ToString(),
                        Budget = (decimal)project["Budget"],
                        ActualCost = (decimal)project["ActualCost"],
                        Status = project["Status"].ToString()
                    });
                }

                LogActivity("Report", "Сгенерирован финансовый отчет");

                return report.OrderByDescending(r => r.Budget).ToList();
            }

            public List<OverdueTaskReportItem> GenerateOverdueTasksReport()
            {
                var report = new List<OverdueTaskReportItem>();

                DataView tasksView = new DataView(Tasks);
                tasksView.RowFilter = $"Status != 'Completed' AND DueDate < #{DateTime.Now:yyyy-MM-dd}#";

                foreach (DataRowView taskRow in tasksView)
                {
                    int taskId = (int)taskRow["TaskID"];
                    int projectId = (int)taskRow["ProjectID"];
                    int assignedTo = (int)taskRow["AssignedTo"];

                    string projectName = "";
                    DataRow project = Projects.Rows.Find(projectId);
                    if (project != null)
                    {
                        projectName = project["ProjectName"].ToString();
                    }

                    string assignedToName = "";
                    DataRow employee = Employees.Rows.Find(assignedTo);
                    if (employee != null)
                    {
                        assignedToName = employee["FullName"].ToString();
                    }

                    report.Add(new OverdueTaskReportItem
                    {
                        TaskID = taskId,
                        TaskName = taskRow["TaskName"].ToString(),
                        ProjectName = projectName,
                        AssignedToName = assignedToName,
                        DueDate = (DateTime)taskRow["DueDate"],
                        Priority = taskRow["Priority"].ToString()
                    });
                }

                LogActivity("Report", "Сгенерирован отчет о просроченных задачах");

                return report.OrderByDescending(r => (DateTime.Now - r.DueDate).Days).ToList();
            }
            #endregion

            #region Экспорт
            public void ExportToCsv(DataView dataView, string fileName)
            {
                try
                {
                    using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
                    {
                        // Заголовок
                        if (dataView.Count > 0)
                        {
                            var columns = dataView.Table.Columns;
                            writer.WriteLine(string.Join(",", columns.Cast<DataColumn>().Select(c => $"\"{c.ColumnName}\"")));

                            // Данные
                            foreach (DataRowView rowView in dataView)
                            {
                                var values = new List<string>();
                                foreach (DataColumn column in columns)
                                {
                                    object value = rowView[column.ColumnName];
                                    string stringValue = value?.ToString() ?? "";

                                    // Экранирование кавычек и запятых
                                    if (stringValue.Contains("\"") || stringValue.Contains(",") || stringValue.Contains("\n"))
                                    {
                                        stringValue = $"\"{stringValue.Replace("\"", "\"\"")}\"";
                                    }

                                    values.Add(stringValue);
                                }
                                writer.WriteLine(string.Join(",", values));
                            }
                        }
                    }

                    LogActivity("Export", $"Экспорт данных в CSV: {fileName}");
                }
                catch (Exception ex)
                {
                    LogActivity("ExportError", $"Ошибка экспорта в CSV: {ex.Message}");
                }
            }

            public void ExportToHtml(DataView dataView, string fileName)
            {
                try
                {
                    using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
                    {
                        writer.WriteLine("<html>");
                        writer.WriteLine("<head>");
                        writer.WriteLine("<style>");
                        writer.WriteLine("table { border-collapse: collapse; width: 100%; }");
                        writer.WriteLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
                        writer.WriteLine("th { background-color: #4CAF50; color: white; }");
                        writer.WriteLine("tr:nth-child(even) { background-color: #f2f2f2; }");
                        writer.WriteLine("</style>");
                        writer.WriteLine("</head>");
                        writer.WriteLine("<body>");
                        writer.WriteLine($"<h1>Отчет ({DateTime.Now:dd.MM.yyyy HH:mm})</h1>");
                        writer.WriteLine($"<p>Всего записей: {dataView.Count}</p>");

                        if (dataView.Count > 0)
                        {
                            writer.WriteLine("<table>");

                            // Заголовок
                            writer.WriteLine("<tr>");
                            foreach (DataColumn column in dataView.Table.Columns)
                            {
                                writer.WriteLine($"<th>{column.ColumnName}</th>");
                            }
                            writer.WriteLine("</tr>");

                            // Данные
                            foreach (DataRowView rowView in dataView)
                            {
                                writer.WriteLine("<tr>");
                                foreach (DataColumn column in dataView.Table.Columns)
                                {
                                    object value = rowView[column.ColumnName];
                                    writer.WriteLine($"<td>{value?.ToString() ?? ""}</td>");
                                }
                                writer.WriteLine("</tr>");
                            }

                            writer.WriteLine("</table>");
                        }

                        writer.WriteLine("</body>");
                        writer.WriteLine("</html>");
                    }

                    LogActivity("Export", $"Экспорт данных в HTML: {fileName}");
                }
                catch (Exception ex)
                {
                    LogActivity("ExportError", $"Ошибка экспорта в HTML: {ex.Message}");
                }
            }
            #endregion

            #region Вспомогательные методы
            private void SubscribeToEvents()
            {
                Projects.RowChanged += (sender, e) =>
                {
                    LogActivity("DataChange", $"Изменен проект {e.Row["ProjectID"]}: {e.Action}");
                };

                Tasks.RowChanged += (sender, e) =>
                {
                    LogActivity("DataChange", $"Изменена задача {e.Row["TaskID"]}: {e.Action}");
                };
            }

            private void LogActivity(string action, string details)
            {
                ActivityLog.Add(new ActivityLogEntry
                {
                    Timestamp = DateTime.Now,
                    Action = action,
                    Details = details
                });

                // Ограничиваем размер лога
                if (ActivityLog.Count > 1000)
                {
                    ActivityLog.RemoveAt(0);
                }
            }

            private int GetNextId(DataTable table, string idColumnName)
            {
                if (table.Rows.Count == 0)
                    return 1;

                int maxId = table.AsEnumerable()
                    .Select(r => (int)r[idColumnName])
                    .Max();

                return maxId + 1;
            }
            #endregion
        }

        class ActivityLogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Action { get; set; }
            public string Details { get; set; }
        }

        class ProjectStatusReportItem
        {
            public int ProjectID { get; set; }
            public string ProjectName { get; set; }
            public string Status { get; set; }
            public decimal Budget { get; set; }
            public decimal ActualCost { get; set; }
            public int CompletionPercent { get; set; }
        }

        class ResourceUsageReportItem
        {
            public string ResourceName { get; set; }
            public string Type { get; set; }
            public string ProjectName { get; set; }
            public decimal TotalHours { get; set; }
            public decimal Cost { get; set; }
        }

        class FinancialReportItem
        {
            public int ProjectID { get; set; }
            public string ProjectName { get; set; }
            public string Status { get; set; }
            public decimal Budget { get; set; }
            public decimal ActualCost { get; set; }
        }

        class OverdueTaskReportItem
        {
            public int TaskID { get; set; }
            public string TaskName { get; set; }
            public string ProjectName { get; set; }
            public string AssignedToName { get; set; }
            public DateTime DueDate { get; set; }
            public string Priority { get; set; }
        }
        #endregion
        #endregion

    }
}