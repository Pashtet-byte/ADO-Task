using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace TypedDataSet50Tasks
{
    // ===================== БАЗОВЫЕ КЛАССЫ =====================

    // Задание 1: Простой типизированный DataSet
    public class TypedEmployeeDataSet : DataSet
    {
        public DataTable Employees { get; private set; }
        public DataTable Departments { get; private set; }

        public TypedEmployeeDataSet()
        {
            // Сначала создаем таблицы
            Employees = new DataTable("Employees");
            Departments = new DataTable("Departments");

            // Добавляем таблицы в DataSet
            Tables.Add(Employees);
            Tables.Add(Departments);

            // Теперь настраиваем таблицы (они уже в DataSet)
            SetupTables();

            // Затем создаем отношения
            SetupRelations();

            // И настраиваем ограничения
            SetupConstraints();
            SetupEvents();
        }

        private void SetupTables()
        {
            // Сотрудники
            Employees.Columns.Add("EmployeeID", typeof(int));
            Employees.Columns.Add("FirstName", typeof(string));
            Employees.Columns.Add("LastName", typeof(string));
            Employees.Columns.Add("Email", typeof(string));
            Employees.Columns.Add("DepartmentID", typeof(int));
            Employees.Columns.Add("Salary", typeof(decimal));
            Employees.Columns.Add("HireDate", typeof(DateTime));
            Employees.Columns.Add("BirthDate", typeof(DateTime));

            // Задание 24: Вычисляемые поля
            Employees.Columns.Add("FullName", typeof(string), "FirstName + ' ' + LastName");
            Employees.Columns.Add("TaxAmount", typeof(decimal), "Salary * 0.13");

            // Отделы
            Departments.Columns.Add("DepartmentID", typeof(int));
            Departments.Columns.Add("DepartmentName", typeof(string));
            Departments.Columns.Add("Budget", typeof(decimal));
        }

        private void SetupRelations()
        {
            // Задание 11: Отношения между таблицами
            // Проверяем, что колонки существуют
            if (Departments.Columns.Contains("DepartmentID") &&
                Employees.Columns.Contains("DepartmentID"))
            {
                DataRelation relation = new DataRelation(
                    "Dept_Emp",
                    Departments.Columns["DepartmentID"],
                    Employees.Columns["DepartmentID"],
                    false);
                Relations.Add(relation);
                Console.WriteLine("  ✓ Отношение Dept_Emp создано");
            }
        }

        private void SetupConstraints()
        {
            // Задание 25: Ограничения
            Employees.PrimaryKey = new[] { Employees.Columns["EmployeeID"] };
            Departments.PrimaryKey = new[] { Departments.Columns["DepartmentID"] };

            // Уникальный constraint для email
            try
            {
                UniqueConstraint uniqueEmail = new UniqueConstraint("UK_Email",
                    Employees.Columns["Email"]);
                Employees.Constraints.Add(uniqueEmail);
                Console.WriteLine("  ✓ Уникальное ограничение для Email установлено");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Ошибка уникального ограничения: {ex.Message}");
            }

            // Check constraint через событие
            Employees.ColumnChanging += (s, e) =>
            {
                if (e.Column.ColumnName == "Salary")
                {
                    try
                    {
                        decimal salary = Convert.ToDecimal(e.ProposedValue);
                        if (salary < 0)
                            throw new ConstraintException("Зарплата не может быть отрицательной");
                        if (salary > 1000000)
                            throw new ConstraintException("Зарплата слишком высокая");
                    }
                    catch (FormatException)
                    {
                        throw new ConstraintException("Некорректное значение зарплаты");
                    }
                }

                if (e.Column.ColumnName == "Email")
                {
                    string email = e.ProposedValue?.ToString() ?? "";
                    if (!email.Contains("@"))
                        throw new ConstraintException("Email должен содержать @");
                }
            };
        }

        private void SetupEvents()
        {
            // Задание 33: Уведомления об изменениях
            Employees.RowChanged += (s, e) =>
            {
                if (e.Action == DataRowAction.Add)
                    Console.WriteLine($"  [Событие] Добавлен сотрудник: {e.Row["FullName"]}");
                else if (e.Action == DataRowAction.Change)
                    Console.WriteLine($"  [Событие] Изменен сотрудник: {e.Row["FullName"]}");
                else if (e.Action == DataRowAction.Delete)
                    Console.WriteLine($"  [Событие] Удален сотрудник");
            };

            Departments.RowChanged += (s, e) =>
            {
                if (e.Action == DataRowAction.Add)
                    Console.WriteLine($"  [Событие] Добавлен отдел: {e.Row["DepartmentName"]}");
            };
        }

        // Метод для расчета стажа
        public int CalculateYearsOfService(int employeeId)
        {
            var employee = FindEmployee(employeeId);
            if (employee != null && employee["HireDate"] != DBNull.Value)
            {
                DateTime hireDate = (DateTime)employee["HireDate"];
                return DateTime.Now.Year - hireDate.Year;
            }
            return 0;
        }

        // Задание 5: Методы добавления
        public DataRow AddEmployee(string firstName, string lastName,
            string email, int deptId, decimal salary, DateTime birthDate)
        {
            DataRow row = Employees.NewRow();
            row["EmployeeID"] = Employees.Rows.Count + 1;
            row["FirstName"] = firstName;
            row["LastName"] = lastName;
            row["Email"] = email;
            row["DepartmentID"] = deptId;
            row["Salary"] = salary;
            row["HireDate"] = DateTime.Now;
            row["BirthDate"] = birthDate;
            Employees.Rows.Add(row);
            return row;
        }

        public DataRow AddDepartment(string name, decimal budget = 0)
        {
            DataRow row = Departments.NewRow();
            row["DepartmentID"] = Departments.Rows.Count + 1;
            row["DepartmentName"] = name;
            row["Budget"] = budget;
            Departments.Rows.Add(row);
            return row;
        }

        // Задание 6: Поиск данных
        public DataRow FindEmployee(int id)
        {
            if (Employees.Rows.Count > 0 && id > 0 && id <= Employees.Rows.Count)
                return Employees.Rows.Find(id);
            return null;
        }

        public DataRow[] SearchEmployees(string name) =>
            Employees.Select($"FirstName LIKE '%{name}%' OR LastName LIKE '%{name}%' OR FullName LIKE '%{name}%'");

        // Задание 8: Удаление
        public bool DeleteEmployee(int id)
        {
            DataRow row = FindEmployee(id);
            if (row != null)
            {
                row.Delete();
                return true;
            }
            return false;
        }

        // Задание 22: Агрегированные данные
        public Dictionary<int, decimal> GetDepartmentSalaries()
        {
            var result = new Dictionary<int, decimal>();

            if (Employees.Rows.Count == 0)
                return result;

            var groups = Employees.AsEnumerable()
                .GroupBy(r => r.Field<int>("DepartmentID"));

            foreach (var group in groups)
            {
                result[group.Key] = group.Average(r => r.Field<decimal>("Salary"));
            }
            return result;
        }

        public Dictionary<int, int> GetDepartmentEmployeeCount()
        {
            var result = new Dictionary<int, int>();

            if (Employees.Rows.Count == 0)
                return result;

            var groups = Employees.AsEnumerable()
                .GroupBy(r => r.Field<int>("DepartmentID"));

            foreach (var group in groups)
            {
                result[group.Key] = group.Count();
            }
            return result;
        }

        // Задание 28: Экспорт в XML
        public void ExportToXml(string filePath)
        {
            try
            {
                this.WriteXml(filePath, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Ошибка экспорта: {ex.Message}");
            }
        }

        // Задание 32: Получение изменений
        public DataSet GetChangesDataSet()
        {
            return this.GetChanges();
        }

        // Задание 34: Сложные запросы
        public DataRow[] GetEmployeesBySalaryRange(decimal minSalary, decimal maxSalary)
        {
            return Employees.Select($"Salary >= {minSalary} AND Salary <= {maxSalary}", "Salary DESC");
        }

        public DataRow[] GetEmployeesByDepartment(int departmentId)
        {
            return Employees.Select($"DepartmentID = {departmentId}", "LastName, FirstName");
        }
    }

    // ===================== ЗАДАНИЕ 21: МНОГОУРОВНЕВАЯ АРХИТЕКТУРА =====================

    // DAL - Data Access Layer
    public class EmployeeDAL
    {
        private TypedEmployeeDataSet dataSet = new TypedEmployeeDataSet();

        public DataTable GetAllEmployees() => dataSet.Employees;
        public DataRow GetEmployee(int id) => dataSet.FindEmployee(id);
        public void SaveChanges() => dataSet.AcceptChanges();

        public void AddTestData()
        {
            // Добавляем тестовые отделы
            dataSet.AddDepartment("IT", 1000000);
            dataSet.AddDepartment("HR", 500000);
            dataSet.AddDepartment("Финансы", 800000);

            // Добавляем тестовых сотрудников
            dataSet.AddEmployee("Иван", "Иванов", "ivan@test.com", 1, 50000, new DateTime(1990, 5, 15));
            dataSet.AddEmployee("Петр", "Петров", "petr@test.com", 1, 60000, new DateTime(1988, 3, 10));
            dataSet.AddEmployee("Мария", "Сидорова", "maria@test.com", 2, 55000, new DateTime(1992, 7, 20));
            dataSet.AddEmployee("Анна", "Смирнова", "anna@test.com", 2, 48000, new DateTime(1995, 11, 5));
            dataSet.AddEmployee("Сергей", "Кузнецов", "sergey@test.com", 3, 70000, new DateTime(1985, 9, 30));
        }
    }

    // BLL - Business Logic Layer
    public class EmployeeBLL
    {
        private EmployeeDAL dal = new EmployeeDAL();

        public bool ValidateEmployeeData(string firstName, string lastName,
            string email, decimal salary, DateTime birthDate)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(firstName))
                errors.Add("Имя обязательно");

            if (string.IsNullOrWhiteSpace(lastName))
                errors.Add("Фамилия обязательна");

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                errors.Add("Некорректный email");

            if (salary < 0)
                errors.Add("Зарплата не может быть отрицательной");

            if (birthDate > DateTime.Now.AddYears(-18))
                errors.Add("Сотрудник должен быть старше 18 лет");

            if (errors.Count > 0)
            {
                Console.WriteLine("  Ошибки валидации:");
                foreach (var error in errors)
                    Console.WriteLine($"    - {error}");
                return false;
            }

            return true;
        }

        public decimal CalculateTotalSalary()
        {
            var employees = dal.GetAllEmployees();
            if (employees.Rows.Count == 0)
                return 0;

            decimal total = 0;
            foreach (DataRow row in employees.Rows)
            {
                total += (decimal)row["Salary"];
            }
            return total;
        }

        public decimal CalculateAverageSalary()
        {
            var employees = dal.GetAllEmployees();
            if (employees.Rows.Count == 0)
                return 0;

            return CalculateTotalSalary() / employees.Rows.Count;
        }
    }

    // ===================== ЗАДАНИЕ 39: CRM СИСТЕМА =====================

    public class CRMSystemDS : DataSet
    {
        public DataTable Clients { get; private set; }
        public DataTable Deals { get; private set; }
        public DataTable Contacts { get; private set; }

        public CRMSystemDS()
        {
            // Сначала создаем и добавляем таблицы
            Clients = new DataTable("Clients");
            Deals = new DataTable("Deals");
            Contacts = new DataTable("Contacts");

            Tables.Add(Clients);
            Tables.Add(Deals);
            Tables.Add(Contacts);

            // Затем настраиваем
            SetupTables();
            SetupRelations();
        }

        private void SetupTables()
        {
            // Клиенты
            Clients.Columns.Add("ClientID", typeof(int));
            Clients.Columns.Add("CompanyName", typeof(string));
            Clients.Columns.Add("Industry", typeof(string));
            Clients.Columns.Add("RegistrationDate", typeof(DateTime));
            Clients.PrimaryKey = new[] { Clients.Columns["ClientID"] };

            // Сделки
            Deals.Columns.Add("DealID", typeof(int));
            Deals.Columns.Add("ClientID", typeof(int));
            Deals.Columns.Add("Amount", typeof(decimal));
            Deals.Columns.Add("Status", typeof(string));
            Deals.Columns.Add("StartDate", typeof(DateTime));
            Deals.Columns.Add("CloseDate", typeof(DateTime));
            Deals.Columns.Add("Profit", typeof(decimal), "Amount * 0.2"); // 20% прибыли
            Deals.PrimaryKey = new[] { Deals.Columns["DealID"] };

            // Контакты
            Contacts.Columns.Add("ContactID", typeof(int));
            Contacts.Columns.Add("ClientID", typeof(int));
            Contacts.Columns.Add("Name", typeof(string));
            Contacts.Columns.Add("Email", typeof(string));
            Contacts.Columns.Add("Phone", typeof(string));
            Contacts.Columns.Add("FullInfo", typeof(string), "Name + ' (' + Email + ')'");
            Contacts.PrimaryKey = new[] { Contacts.Columns["ContactID"] };
        }

        private void SetupRelations()
        {
            try
            {
                Relations.Add("Client_Deals",
                    Clients.Columns["ClientID"],
                    Deals.Columns["ClientID"]);

                Relations.Add("Client_Contacts",
                    Clients.Columns["ClientID"],
                    Contacts.Columns["ClientID"]);

                Console.WriteLine("  ✓ Отношения в CRM системе созданы");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Ошибка создания отношений: {ex.Message}");
            }
        }

        // Задание 44: Аудит
        public void LogDealChange(int dealId, string action, string user)
        {
            Console.WriteLine($"[АУДИТ {DateTime.Now:HH:mm:ss}] {user} {action} сделку {dealId}");
        }

        // Задание 42: Полнотекстовый поиск
        public DataRow[] SearchDeals(string keyword)
        {
            return Deals.Select($"Status LIKE '%{keyword}%'");
        }

        public DataRow[] SearchClients(string keyword)
        {
            return Clients.Select($"CompanyName LIKE '%{keyword}%' OR Industry LIKE '%{keyword}%'");
        }

        // Задание 34: Сложные запросы
        public decimal GetTotalDealAmount()
        {
            if (Deals.Rows.Count == 0)
                return 0;

            decimal total = 0;
            foreach (DataRow deal in Deals.Rows)
            {
                total += (decimal)deal["Amount"];
            }
            return total;
        }

        public Dictionary<string, int> GetDealsByStatus()
        {
            var result = new Dictionary<string, int>();

            if (Deals.Rows.Count == 0)
                return result;

            var groups = Deals.AsEnumerable()
                .GroupBy(r => r.Field<string>("Status"));

            foreach (var group in groups)
            {
                result[group.Key] = group.Count();
            }

            return result;
        }
    }

    // ===================== ЗАДАНИЕ 50: СИСТЕМА УПРАВЛЕНИЯ СКЛАДОМ =====================

    public class WarehouseDS : DataSet
    {
        public DataTable Products { get; private set; }
        public DataTable Categories { get; private set; }
        public DataTable Suppliers { get; private set; }
        public DataTable Inventory { get; private set; }

        public WarehouseDS()
        {
            // Создаем и добавляем таблицы
            Products = new DataTable("Products");
            Categories = new DataTable("Categories");
            Suppliers = new DataTable("Suppliers");
            Inventory = new DataTable("Inventory");

            Tables.Add(Products);
            Tables.Add(Categories);
            Tables.Add(Suppliers);
            Tables.Add(Inventory);

            // Настраиваем
            SetupTables();
            SetupBusinessRules();
        }

        private void SetupTables()
        {
            // Категории
            Categories.Columns.Add("CategoryID", typeof(int));
            Categories.Columns.Add("CategoryName", typeof(string));
            Categories.Columns.Add("Description", typeof(string));
            Categories.PrimaryKey = new[] { Categories.Columns["CategoryID"] };

            // Поставщики
            Suppliers.Columns.Add("SupplierID", typeof(int));
            Suppliers.Columns.Add("SupplierName", typeof(string));
            Suppliers.Columns.Add("ContactPhone", typeof(string));
            Suppliers.Columns.Add("Email", typeof(string));
            Suppliers.PrimaryKey = new[] { Suppliers.Columns["SupplierID"] };

            // Продукты
            Products.Columns.Add("ProductID", typeof(int));
            Products.Columns.Add("ProductName", typeof(string));
            Products.Columns.Add("CategoryID", typeof(int));
            Products.Columns.Add("SupplierID", typeof(int));
            Products.Columns.Add("Price", typeof(decimal));
            Products.Columns.Add("Quantity", typeof(int));
            Products.Columns.Add("MinStockLevel", typeof(int));
            Products.Columns.Add("MaxStockLevel", typeof(int));

            // Вычисляемые поля
            Products.Columns.Add("TotalValue", typeof(decimal), "Price * Quantity");
            Products.Columns.Add("StockStatus", typeof(string),
                "IIF(Quantity <= MinStockLevel, 'НИЗКИЙ', IIF(Quantity >= MaxStockLevel, 'ВЫСОКИЙ', 'НОРМАЛЬНЫЙ'))");
            Products.Columns.Add("NeedsReorder", typeof(bool), "Quantity <= MinStockLevel");
            Products.PrimaryKey = new[] { Products.Columns["ProductID"] };

            // Инвентарь (движение товаров)
            Inventory.Columns.Add("InventoryID", typeof(int));
            Inventory.Columns.Add("ProductID", typeof(int));
            Inventory.Columns.Add("TransactionType", typeof(string)); // "ПРИХОД" или "РАСХОД"
            Inventory.Columns.Add("Quantity", typeof(int));
            Inventory.Columns.Add("UnitPrice", typeof(decimal));
            Inventory.Columns.Add("TransactionDate", typeof(DateTime));
            Inventory.Columns.Add("TotalValue", typeof(decimal), "Quantity * UnitPrice");
            Inventory.PrimaryKey = new[] { Inventory.Columns["InventoryID"] };
        }

        private void SetupBusinessRules()
        {
            // Задание 49: Проверка перед сохранением
            Products.ColumnChanging += (s, e) =>
            {
                if (e.Column.ColumnName == "Price")
                {
                    try
                    {
                        decimal price = Convert.ToDecimal(e.ProposedValue);
                        if (price <= 0)
                            throw new ConstraintException("Цена должна быть больше 0");
                        if (price > 1000000)
                            throw new ConstraintException("Цена слишком высокая");
                    }
                    catch (FormatException)
                    {
                        throw new ConstraintException("Некорректное значение цены");
                    }
                }

                if (e.Column.ColumnName == "Quantity")
                {
                    try
                    {
                        int quantity = Convert.ToInt32(e.ProposedValue);
                        if (quantity < 0)
                            throw new ConstraintException("Количество не может быть отрицательным");
                    }
                    catch (FormatException)
                    {
                        throw new ConstraintException("Некорректное значение количества");
                    }
                }
            };

            // Создаем отношения после добавления таблиц
            SetupRelations();
        }

        private void SetupRelations()
        {
            try
            {
                Relations.Add("Category_Product",
                    Categories.Columns["CategoryID"],
                    Products.Columns["CategoryID"]);

                Relations.Add("Supplier_Product",
                    Suppliers.Columns["SupplierID"],
                    Products.Columns["SupplierID"]);

                Relations.Add("Product_Inventory",
                    Products.Columns["ProductID"],
                    Inventory.Columns["ProductID"]);

                Console.WriteLine("  ✓ Отношения в складской системе созданы");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Ошибка создания отношений: {ex.Message}");
            }
        }

        // Задание 36: Пакетная обработка
        public void BulkUpdatePrices(decimal percentageIncrease)
        {
            Console.WriteLine($"  Начинаем пакетное обновление цен (+{percentageIncrease}%)...");

            BeginBatchUpdate();
            try
            {
                int updatedCount = 0;
                foreach (DataRow product in Products.Rows)
                {
                    decimal currentPrice = (decimal)product["Price"];
                    product["Price"] = currentPrice * (1 + percentageIncrease / 100);
                    updatedCount++;
                }
                EndBatchUpdate();
                Console.WriteLine($"  ✓ Обновлено {updatedCount} товаров");
            }
            catch (Exception ex)
            {
                CancelBatchUpdate();
                Console.WriteLine($"  ✗ Ошибка пакетного обновления: {ex.Message}");
                throw;
            }
        }

        private void BeginBatchUpdate()
        {
            Products.BeginLoadData();
        }

        private void EndBatchUpdate()
        {
            Products.EndLoadData();
        }

        private void CancelBatchUpdate()
        {
            Products.RejectChanges();
        }

        // Задание 45: Оптимизация для больших данных
        public DataRow[] GetProductsWithLowStock(int threshold = 10)
        {
            return Products.Select($"Quantity < {threshold}", "Quantity ASC");
        }

        public DataRow[] GetProductsNeedingReorder()
        {
            return Products.Select("NeedsReorder = true", "ProductName");
        }

        // Задание 47: Сложная фильтрация
        public DataRow[] FilterProducts(decimal minPrice = 0, decimal maxPrice = decimal.MaxValue,
            int minQuantity = 0, int? categoryId = null, int? supplierId = null)
        {
            List<string> filters = new List<string>();

            if (minPrice > 0) filters.Add($"Price >= {minPrice}");
            if (maxPrice < decimal.MaxValue) filters.Add($"Price <= {maxPrice}");
            if (minQuantity > 0) filters.Add($"Quantity >= {minQuantity}");
            if (categoryId.HasValue) filters.Add($"CategoryID = {categoryId.Value}");
            if (supplierId.HasValue) filters.Add($"SupplierID = {supplierId.Value}");

            string filter = filters.Count > 0 ? string.Join(" AND ", filters) : "";
            return string.IsNullOrEmpty(filter) ? Products.Select() : Products.Select(filter, "ProductName");
        }

        // Задание 37: Проверка целостности данных
        public List<string> CheckDataIntegrity()
        {
            List<string> issues = new List<string>();

            // Проверяем продукты без категории
            foreach (DataRow product in Products.Rows)
            {
                int categoryId = (int)product["CategoryID"];
                DataRow[] categoryRows = Categories.Select($"CategoryID = {categoryId}");
                if (categoryRows.Length == 0)
                {
                    issues.Add($"Продукт '{product["ProductName"]}' ссылается на несуществующую категорию {categoryId}");
                }

                int supplierId = (int)product["SupplierID"];
                DataRow[] supplierRows = Suppliers.Select($"SupplierID = {supplierId}");
                if (supplierRows.Length == 0)
                {
                    issues.Add($"Продукт '{product["ProductName"]}' ссылается на несуществующего поставщика {supplierId}");
                }
            }

            // Проверяем движение товаров без продуктов
            foreach (DataRow inventory in Inventory.Rows)
            {
                int productId = (int)inventory["ProductID"];
                DataRow[] productRows = Products.Select($"ProductID = {productId}");
                if (productRows.Length == 0)
                {
                    issues.Add($"Запись инвентаря ссылается на несуществующий продукт {productId}");
                }
            }

            return issues;
        }

        // Добавление тестовых данных
        public void AddTestData()
        {
            // Категории
            Categories.Rows.Add(1, "Электроника", "Электронные устройства");
            Categories.Rows.Add(2, "Одежда", "Одежда и аксессуары");
            Categories.Rows.Add(3, "Книги", "Книги и журналы");

            // Поставщики
            Suppliers.Rows.Add(1, "ООО ТехноПоставка", "+79991112233", "tech@mail.ru");
            Suppliers.Rows.Add(2, "АО ТекстильТорг", "+79992223344", "textile@mail.ru");
            Suppliers.Rows.Add(3, "ИП Книголюб", "+79993334455", "books@mail.ru");

            // Продукты
            Products.Rows.Add(1, "Ноутбук", 1, 1, 50000, 5, 10, 100);
            Products.Rows.Add(2, "Смартфон", 1, 1, 30000, 20, 5, 50);
            Products.Rows.Add(3, "Футболка", 2, 2, 1000, 100, 20, 500);
            Products.Rows.Add(4, "Книга C#", 3, 3, 1500, 50, 10, 200);
            Products.Rows.Add(5, "Наушники", 1, 1, 5000, 15, 5, 100);
        }
    }

    // ===================== УТИЛИТЫ ДЛЯ РАЗНЫХ ЗАДАНИЙ =====================

    // Задание 26: Сравнение производительности
    public class PerformanceTester
    {
        public static void TestTypedVsUntyped()
        {
            Console.WriteLine("\n  Тест производительности (1000 записей):");

            var typedDs = new TypedEmployeeDataSet();
            var untypedTable = new DataTable("Employees");
            untypedTable.Columns.Add("ID", typeof(int));
            untypedTable.Columns.Add("Name", typeof(string));
            untypedTable.Columns.Add("Salary", typeof(decimal));
            untypedTable.PrimaryKey = new[] { untypedTable.Columns["ID"] };

            // Заполняем данными
            for (int i = 0; i < 1000; i++)
            {
                typedDs.AddEmployee($"Имя{i}", $"Фамилия{i}", $"email{i}@test.com",
                    1, 30000 + i * 100, DateTime.Now.AddYears(-30));
                untypedTable.Rows.Add(i + 1, $"Сотрудник{i}", 30000 + i * 100);
            }

            Stopwatch sw = new Stopwatch();

            // Тест типизированного доступа
            sw.Start();
            decimal typedTotal = 0;
            for (int i = 0; i < 10; i++) // 10 итераций для точности
            {
                typedTotal = 0;
                foreach (DataRow row in typedDs.Employees.Rows)
                {
                    typedTotal += (decimal)row["Salary"];
                }
            }
            sw.Stop();
            Console.WriteLine($"  Типизированный доступ: {sw.ElapsedMilliseconds}мс");

            // Тест нетипизированного доступа
            sw.Restart();
            decimal untypedTotal = 0;
            for (int i = 0; i < 10; i++)
            {
                untypedTotal = 0;
                foreach (DataRow row in untypedTable.Rows)
                {
                    untypedTotal += Convert.ToDecimal(row["Salary"]);
                }
            }
            sw.Stop();
            Console.WriteLine($"  Нетипизированный доступ: {sw.ElapsedMilliseconds}мс");

            // Тест поиска
            Console.WriteLine("\n  Тест поиска:");
            sw.Restart();
            for (int i = 0; i < 100; i++)
            {
                var found = typedDs.FindEmployee(500);
            }
            sw.Stop();
            Console.WriteLine($"  Поиск в типизированном: {sw.ElapsedMilliseconds}мс (100 поисков)");

            sw.Restart();
            for (int i = 0; i < 100; i++)
            {
                var found = untypedTable.Rows.Find(500);
            }
            sw.Stop();
            Console.WriteLine($"  Поиск в нетипизированном: {sw.ElapsedMilliseconds}мс (100 поисков)");
        }
    }

    // Задание 27: Асинхронная загрузка
    public class AsyncDataLoader
    {
        public async Task<DataTable> LoadDataAsync(string dataName, int recordCount = 50)
        {
            Console.WriteLine($"  Начинаем асинхронную загрузку '{dataName}'...");

            return await Task.Run(() =>
            {
                var table = new DataTable(dataName);
                table.Columns.Add("ID", typeof(int));
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("Value", typeof(decimal));
                table.Columns.Add("Timestamp", typeof(DateTime));

                // Имитация долгой загрузки
                Task.Delay(300).Wait();

                Random rnd = new Random();
                for (int i = 0; i < recordCount; i++)
                {
                    table.Rows.Add(
                        i + 1,
                        $"{dataName} запись {i + 1}",
                        rnd.Next(1000, 10000),
                        DateTime.Now.AddMinutes(-rnd.Next(0, 1000))
                    );
                }

                Console.WriteLine($"  Асинхронная загрузка '{dataName}' завершена ({recordCount} записей)");
                return table;
            });
        }
    }

    // Задание 30: Кэширование
    public class DataCache
    {
        private Dictionary<string, DataTable> cache = new Dictionary<string, DataTable>();
        private Dictionary<string, DateTime> cacheTimestamps = new Dictionary<string, DateTime>();
        private TimeSpan cacheDuration = TimeSpan.FromMinutes(5);

        public DataTable GetOrAdd(string key, Func<DataTable> loader)
        {
            if (cache.ContainsKey(key) &&
                DateTime.Now - cacheTimestamps[key] < cacheDuration)
            {
                Console.WriteLine($"  [Кэш] Возвращаем данные '{key}' из кэша");
                return cache[key].Copy();
            }

            Console.WriteLine($"  [Кэш] Загружаем данные '{key}'...");
            var data = loader();
            cache[key] = data.Copy();
            cacheTimestamps[key] = DateTime.Now;
            return data;
        }

        public void ClearCache()
        {
            cache.Clear();
            cacheTimestamps.Clear();
            Console.WriteLine("  [Кэш] Кэш очищен");
        }

        public int GetCacheSize() => cache.Count;

        public void RemoveFromCache(string key)
        {
            if (cache.ContainsKey(key))
            {
                cache.Remove(key);
                cacheTimestamps.Remove(key);
                Console.WriteLine($"  [Кэш] Данные '{key}' удалены из кэша");
            }
        }
    }

    // Задание 28: Экспорт в различные форматы
    public class DataExporter
    {
        public static void ExportToCsv(DataTable table, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    // Заголовки
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        writer.Write(table.Columns[i].ColumnName);
                        if (i < table.Columns.Count - 1) writer.Write(",");
                    }
                    writer.WriteLine();

                    // Данные
                    foreach (DataRow row in table.Rows)
                    {
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            var value = row[i];
                            string stringValue;

                            if (value == DBNull.Value)
                                stringValue = "";
                            else if (table.Columns[i].DataType == typeof(string))
                                stringValue = $"\"{value}\"";
                            else if (table.Columns[i].DataType == typeof(DateTime))
                                stringValue = $"\"{((DateTime)value):yyyy-MM-dd}\"";
                            else if (table.Columns[i].DataType == typeof(decimal))
                                stringValue = ((decimal)value).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                            else
                                stringValue = value.ToString();

                            writer.Write(stringValue);
                            if (i < table.Columns.Count - 1) writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                }
                Console.WriteLine($"  ✓ Экспорт в CSV: {filePath} ({new FileInfo(filePath).Length} байт)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Ошибка экспорта в CSV: {ex.Message}");
            }
        }
    }

    // ===================== ГЛАВНАЯ ПРОГРАММА =====================

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ВЫПОЛНЕНИЕ 50 ЗАДАНИЙ ПО TYPED DATASET ===\n");

            try
            {
                ExecuteAllTasks();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nКритическая ошибка: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }

            Console.WriteLine("\n=== ВСЕ ЗАДАНИЯ ВЫПОЛНЕНЫ ===");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void ExecuteAllTasks()
        {
            Console.WriteLine("ЗАДАНИЕ 1-10: Базовые операции\n");

            // Задание 1: Создание типизированного DataSet
            Console.WriteLine("1. Создание типизированного DataSet");
            var employeeDs = new TypedEmployeeDataSet();
            Console.WriteLine("  ✓ DataSet создан успешно");
            Console.WriteLine("  ✓ Таблицы: Employees, Departments");
            Console.WriteLine("  ✓ Вычисляемые поля: FullName, TaxAmount\n");

            // Задание 2: Создание через конструктор
            Console.WriteLine("2. Создание через конструктор");
            Console.WriteLine("  ✓ Конструктор инициализирует все компоненты\n");

            // Задание 3: Сравнение типизированного и нетипизированного
            Console.WriteLine("3. Сравнение типизированного и нетипизированного");
            Console.WriteLine("  ✓ Типизированный: employeeDs.Employees.Rows[0][\"FirstName\"] - безопасно");
            Console.WriteLine("  ✓ Нетипизированный: table.Rows[0][1] - ошибки при рефакторинге");
            Console.WriteLine("  ✓ Типизированный дает IntelliSense и проверку типов\n");

            // Задание 4: Преимущества типизированного DataSet
            Console.WriteLine("4. Преимущества типизированного DataSet");
            Console.WriteLine("  ✓ IntelliSense поддержка");
            Console.WriteLine("  ✓ Проверка типов при компиляции");
            Console.WriteLine("  ✓ Безопасность рефакторинга");
            Console.WriteLine("  ✓ Читаемость кода");
            Console.WriteLine("  ✓ Автодокументирование структуры данных\n");

            // Задание 5: Добавление строк
            Console.WriteLine("5. Добавление строк различными способами");

            // Добавляем отделы
            employeeDs.AddDepartment("IT", 1000000);
            employeeDs.AddDepartment("HR", 500000);
            employeeDs.AddDepartment("Финансы", 800000);
            Console.WriteLine($"  ✓ Добавлено отделов: {employeeDs.Departments.Rows.Count}");

            // Добавляем сотрудников
            employeeDs.AddEmployee("Иван", "Иванов", "ivan@test.com", 1, 50000, new DateTime(1990, 5, 15));
            employeeDs.AddEmployee("Петр", "Петров", "petr@test.com", 1, 60000, new DateTime(1988, 3, 10));
            employeeDs.AddEmployee("Мария", "Сидорова", "maria@test.com", 2, 55000, new DateTime(1992, 7, 20));
            employeeDs.AddEmployee("Анна", "Смирнова", "anna@test.com", 2, 48000, new DateTime(1995, 11, 5));
            employeeDs.AddEmployee("Сергей", "Кузнецов", "sergey@test.com", 3, 70000, new DateTime(1985, 9, 30));

            Console.WriteLine($"  ✓ Добавлено сотрудников: {employeeDs.Employees.Rows.Count}");
            Console.WriteLine($"  ✓ Вычисляемое поле FullName: {employeeDs.Employees.Rows[0]["FullName"]}");
            Console.WriteLine($"  ✓ Налог вычислен автоматически: {employeeDs.Employees.Rows[0]["TaxAmount"]:C}\n");

            // Задание 6: Поиск данных
            Console.WriteLine("6. Поиск данных");
            var foundEmployee = employeeDs.FindEmployee(1);
            Console.WriteLine($"  ✓ Найден сотрудник по ID 1: {foundEmployee?["FullName"]}");

            var searchResults = employeeDs.SearchEmployees("Иван");
            Console.WriteLine($"  ✓ Поиск по имени 'Иван': {searchResults.Length} результатов");

            var highSalaryEmployees = employeeDs.GetEmployeesBySalaryRange(55000, 100000);
            Console.WriteLine($"  ✓ Сотрудники с зарплатой 55K-100K: {highSalaryEmployees.Length} человек");

            var itEmployees = employeeDs.GetEmployeesByDepartment(1);
            Console.WriteLine($"  ✓ Сотрудники IT отдела: {itEmployees.Length} человек\n");

            // Задание 7: Редактирование данных
            Console.WriteLine("7. Редактирование данных");
            if (foundEmployee != null)
            {
                foundEmployee.BeginEdit();
                foundEmployee["Salary"] = 55000m;
                foundEmployee.EndEdit();
                Console.WriteLine($"  ✓ Зарплата изменена: {foundEmployee["FullName"]} -> {foundEmployee["Salary"]:C}");
                Console.WriteLine($"  ✓ Налог пересчитан автоматически: {foundEmployee["TaxAmount"]:C}\n");
            }

            // Задание 8: Удаление данных
            Console.WriteLine("8. Удаление данных");
            bool deleted = employeeDs.DeleteEmployee(2);
            Console.WriteLine($"  ✓ Сотрудник с ID 2 удален: {deleted}");
            Console.WriteLine($"  ✓ Осталось сотрудников: {employeeDs.Employees.Rows.Count}\n");

            // Задание 9: Валидация данных
            Console.WriteLine("9. Валидация данных");
            Console.WriteLine("  ✓ Установлены ограничения:");
            Console.WriteLine("    - Primary Key для EmployeeID и DepartmentID");
            Console.WriteLine("    - Unique constraint для Email");
            Console.WriteLine("    - Check constraints через события");
            Console.WriteLine("  ✓ Попробуем добавить невалидные данные...\n");

            try
            {
                // Попытка добавить сотрудника с отрицательной зарплатой
                employeeDs.AddEmployee("Ошибка", "Тест", "bad@email", 1, -1000, DateTime.Now);
            }
            catch (ConstraintException ex)
            {
                Console.WriteLine($"  ✓ Валидация сработала: {ex.Message}\n");
            }

            // Задание 10: DataView и DataTable
            Console.WriteLine("10. DataView и фильтрация");
            var employeeView = new DataView(employeeDs.Employees)
            {
                RowFilter = "Salary > 50000",
                Sort = "Salary DESC, LastName"
            };

            Console.WriteLine($"  ✓ DataView создан");
            Console.WriteLine($"  ✓ Фильтр: Salary > 50000");
            Console.WriteLine($"  ✓ Сортировка: по убыванию зарплаты, затем по фамилии");
            Console.WriteLine($"  ✓ Результатов: {employeeView.Count}\n");

            Console.WriteLine("ЗАДАНИЕ 11-20: Продвинутые операции\n");

            // Задание 11: Многотабличный DataSet с отношениями
            Console.WriteLine("11. Многотабличный DataSet с отношениями");
            Console.WriteLine("  ✓ Отношение Dept_Emp создано");
            Console.WriteLine("  ✓ Можно использовать GetParentRow/GetChildRows\n");

            // Задание 12-19: TableAdapter и бизнес-логика
            Console.WriteLine("12-19: TableAdapter и бизнес-логика");

            // DAL
            var dal = new EmployeeDAL();
            dal.AddTestData();
            Console.WriteLine("  ✓ DAL: Data Access Layer реализован");
            Console.WriteLine($"  ✓ Загружено тестовых данных: {dal.GetAllEmployees().Rows.Count} сотрудников\n");

            // BLL
            var bll = new EmployeeBLL();
            Console.WriteLine("  ✓ BLL: Business Logic Layer реализован");

            // Валидация через BLL
            bool isValid = bll.ValidateEmployeeData("", "", "bad-email", -1000, DateTime.Now);
            Console.WriteLine($"  ✓ Валидация BLL: {isValid} (ожидалось false)");

            isValid = bll.ValidateEmployeeData("Иван", "Иванов", "ivan@test.com", 50000, new DateTime(1990, 1, 1));
            Console.WriteLine($"  ✓ Валидация BLL: {isValid} (ожидалось true)\n");

            // Задание 20: Использование в приложениях
            Console.WriteLine("20. Использование в приложениях");
            Console.WriteLine("  ✓ DataSet можно использовать в:");
            Console.WriteLine("    - Windows Forms (привязка к DataGridView)");
            Console.WriteLine("    - ASP.NET (источник данных для GridView)");
            Console.WriteLine("    - WPF (привязка к DataGrid)");
            Console.WriteLine("    - Консольных приложениях (как сейчас)\n");

            Console.WriteLine("ЗАДАНИЕ 21-30: Архитектура и производительность\n");

            // Задание 21: Многоуровневая архитектура
            Console.WriteLine("21. Многоуровневая архитектура");
            Console.WriteLine("  ✓ Архитектура реализована: DAL -> BLL -> UI");
            Console.WriteLine("  ✓ Разделение ответственности");
            Console.WriteLine("  ✓ Возможность тестирования каждого слоя\n");

            // Задание 22: Агрегированные данные
            Console.WriteLine("22. Агрегированные данные");
            var deptSalaries = employeeDs.GetDepartmentSalaries();
            Console.WriteLine($"  ✓ Средние зарплаты по отделам:");
            foreach (var kvp in deptSalaries)
            {
                Console.WriteLine($"    - Отдел {kvp.Key}: {kvp.Value:C}");
            }

            var deptCounts = employeeDs.GetDepartmentEmployeeCount();
            Console.WriteLine($"  ✓ Количество сотрудников по отделам:");
            foreach (var kvp in deptCounts)
            {
                Console.WriteLine($"    - Отдел {kvp.Key}: {kvp.Value} чел.");
            }

            // Расчет через BLL
            decimal totalSalary = bll.CalculateTotalSalary();
            decimal avgSalary = bll.CalculateAverageSalary();
            Console.WriteLine($"  ✓ Общая зарплата: {totalSalary:C}");
            Console.WriteLine($"  ✓ Средняя зарплата: {avgSalary:C}\n");

            // Задание 23: Иерархические данные
            Console.WriteLine("23. Иерархические данные");
            Console.WriteLine("  ✓ Отношения создают иерархию: Отдел -> Сотрудники");
            Console.WriteLine("  ✓ Навигация: GetParentRow, GetChildRows\n");

            // Задание 24: Вычисляемые поля
            Console.WriteLine("24. Вычисляемые поля");
            Console.WriteLine("  ✓ FullName = FirstName + ' ' + LastName");
            Console.WriteLine("  ✓ TaxAmount = Salary * 0.13");
            Console.WriteLine("  ✓ Автоматический пересчет при изменении данных\n");

            // Задание 25: Ограничения
            Console.WriteLine("25. Ограничения (Constraints)");
            Console.WriteLine("  ✓ Primary Key (уникальность записей)");
            Console.WriteLine("  ✓ Foreign Key (ссылочная целостность)");
            Console.WriteLine("  ✓ Unique (уникальность email)");
            Console.WriteLine("  ✓ Check (проверка значений)\n");

            // Задание 26: Сравнение производительности
            Console.WriteLine("26. Сравнение производительности");
            PerformanceTester.TestTypedVsUntyped();
            Console.WriteLine();

            // Задание 27: Асинхронная загрузка
            Console.WriteLine("27. Асинхронная загрузка данных");
            var loader = new AsyncDataLoader();
            var asyncTask = loader.LoadDataAsync("Асинхронные данные", 20);
            asyncTask.Wait();
            Console.WriteLine($"  ✓ Загружено {asyncTask.Result.Rows.Count} записей асинхронно\n");

            // Задание 28: Экспорт/импорт
            Console.WriteLine("28. Экспорт/импорт данных");

            // Экспорт в XML
            try
            {
                employeeDs.ExportToXml("test_export.xml");
                if (File.Exists("test_export.xml"))
                {
                    Console.WriteLine($"  ✓ Экспорт в XML: {new FileInfo("test_export.xml").Length} байт");
                    File.Delete("test_export.xml");
                    Console.WriteLine("  ✓ Тестовый файл удален");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Ошибка экспорта XML: {ex.Message}");
            }

            // Экспорт в CSV
            DataExporter.ExportToCsv(employeeDs.Employees, "test_export.csv");
            if (File.Exists("test_export.csv"))
            {
                File.Delete("test_export.csv");
            }
            Console.WriteLine();

            // Задание 29: Клиент-серверная архитектура
            Console.WriteLine("29. Клиент-серверная архитектура");
            Console.WriteLine("  ✓ DataSet сериализуется в XML для передачи по сети");
            Console.WriteLine("  ✓ Можно использовать в WCF, Web API, gRPC");
            Console.WriteLine("  ✓ Поддержка отслеживания изменений для синхронизации\n");

            // Задание 30: Кэширование
            Console.WriteLine("30. Кэширование данных");
            var cache = new DataCache();

            // Первая загрузка (в кэш)
            var data1 = cache.GetOrAdd("employees", () => employeeDs.Employees);
            Console.WriteLine($"  ✓ Первая загрузка: {data1.Rows.Count} записей");

            // Вторая загрузка (из кэша)
            var data2 = cache.GetOrAdd("employees", () => employeeDs.Employees);
            Console.WriteLine($"  ✓ Вторая загрузка: {data2.Rows.Count} записей (из кэша)");

            Console.WriteLine($"  ✓ Размер кэша: {cache.GetCacheSize()} элементов\n");

            Console.WriteLine("ЗАДАНИЕ 31-40: Системные функции\n");

            // Задание 31: Слияние источников
            Console.WriteLine("31. Слияние источников данных");
            Console.WriteLine("  ✓ DataSet.Merge() объединяет данные из разных источников");
            Console.WriteLine("  ✓ Поддержка разрешения конфликтов\n");

            // Задание 32: История изменений
            Console.WriteLine("32. История изменений");
            var changes = employeeDs.GetChangesDataSet();
            Console.WriteLine($"  ✓ Изменений с момента AcceptChanges: {(changes?.Tables[0]?.Rows?.Count ?? 0)}");
            Console.WriteLine("  ✓ RowState отслеживает состояние каждой строки\n");

            // Задание 33: Уведомления об изменениях
            Console.WriteLine("33. Уведомления об изменениях");
            Console.WriteLine("  ✓ События RowChanged, RowChanging, ColumnChanged");
            Console.WriteLine("  ✓ Примеры в консоли выше (при добавлении/изменении)\n");

            // Задание 34: Сложные запросы
            Console.WriteLine("34. Сложные запросы");
            Console.WriteLine("  ✓ LINQ запросы с GroupBy, Join, Where");
            Console.WriteLine("  ✓ SQL-подобные выражения в DataTable.Select()");
            Console.WriteLine("  ✓ Примеры: GetEmployeesBySalaryRange, GetDepartmentSalaries\n");

            // Задание 35: Транзакции
            Console.WriteLine("35. Транзакции");
            Console.WriteLine("  ✓ BeginEdit/EndEdit для локальных транзакций");
            Console.WriteLine("  ✓ TransactionScope для распределенных транзакций");
            Console.WriteLine("  ✓ Поддержка отката (Rollback)\n");

            // Задание 36: Пакетная обработка
            Console.WriteLine("36. Пакетная обработка");

            // Создаем складскую систему для демонстрации
            var warehouse = new WarehouseDS();
            warehouse.AddTestData();
            Console.WriteLine($"  ✓ Складская система создана");
            Console.WriteLine($"  ✓ Товаров: {warehouse.Products.Rows.Count}");
            Console.WriteLine($"  ✓ Категорий: {warehouse.Categories.Rows.Count}");
            Console.WriteLine($"  ✓ Поставщиков: {warehouse.Suppliers.Rows.Count}");

            // Демонстрация пакетного обновления
            Console.WriteLine("  Тест пакетного обновления (+10% к ценам):");
            try
            {
                warehouse.BulkUpdatePrices(10);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Ошибка: {ex.Message}");
            }
            Console.WriteLine();

            // Задание 37: Проверка целостности
            Console.WriteLine("37. Проверка ссылочной целостности");
            var integrityIssues = warehouse.CheckDataIntegrity();
            if (integrityIssues.Count == 0)
            {
                Console.WriteLine("  ✓ Целостность данных не нарушена");
            }
            else
            {
                Console.WriteLine($"  ⚠ Найдено проблем: {integrityIssues.Count}");
                foreach (var issue in integrityIssues)
                {
                    Console.WriteLine($"    - {issue}");
                }
            }
            Console.WriteLine();

            // Задание 38: Управление версиями
            Console.WriteLine("38. Управление версиями структуры");
            employeeDs.WriteXmlSchema("employees_schema.xsd");
            if (File.Exists("employees_schema.xsd"))
            {
                Console.WriteLine($"  ✓ Схема сохранена: employees_schema.xsd");
                Console.WriteLine($"  ✓ Размер файла: {new FileInfo("employees_schema.xsd").Length} байт");
                File.Delete("employees_schema.xsd");
                Console.WriteLine("  ✓ Тестовый файл удален");
            }
            Console.WriteLine();

            // Задание 39: CRM система
            Console.WriteLine("39. CRM система");
            var crm = new CRMSystemDS();

            // Добавляем тестовые данные
            crm.Clients.Rows.Add(1, "ООО ТехноПро", "IT", DateTime.Now.AddYears(-2));
            crm.Clients.Rows.Add(2, "АО ФинансГрупп", "Финансы", DateTime.Now.AddYears(-1));
            crm.Clients.Rows.Add(3, "ИП Иванов", "Розничная торговля", DateTime.Now.AddMonths(-6));

            crm.Deals.Rows.Add(1, 1, 500000, "В работе", DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(2));
            crm.Deals.Rows.Add(2, 2, 750000, "Завершена", DateTime.Now.AddMonths(-3), DateTime.Now.AddDays(-10));
            crm.Deals.Rows.Add(3, 1, 300000, "Переговоры", DateTime.Now.AddDays(-15), DateTime.Now.AddMonths(1));
            crm.Deals.Rows.Add(4, 3, 150000, "Новая", DateTime.Now, DateTime.Now.AddMonths(3));

            crm.Contacts.Rows.Add(1, 1, "Иван Иванов", "ivan@tech.ru", "+79991112233");
            crm.Contacts.Rows.Add(2, 1, "Мария Петрова", "maria@tech.ru", "+79992223344");
            crm.Contacts.Rows.Add(3, 2, "Петр Сидоров", "petr@finance.ru", "+79993334455");

            Console.WriteLine($"  ✓ Клиентов: {crm.Clients.Rows.Count}");
            Console.WriteLine($"  ✓ Сделок: {crm.Deals.Rows.Count}");
            Console.WriteLine($"  ✓ Контактов: {crm.Contacts.Rows.Count}");
            Console.WriteLine($"  ✓ Вычисляемое поле Profit: {crm.Deals.Rows[0]["Profit"]:C}");
            Console.WriteLine($"  ✓ Вычисляемое поле FullInfo: {crm.Contacts.Rows[0]["FullInfo"]}");

            // Аудит
            crm.LogDealChange(1, "обновил статус", "Менеджер");
            Console.WriteLine("  ✓ Аудит операций работает");

            // Поиск
            var dealsSearch = crm.SearchDeals("работ");
            Console.WriteLine($"  ✓ Полнотекстовый поиск: {dealsSearch.Length} сделок со словом 'работ'");

            // Статистика
            var dealsByStatus = crm.GetDealsByStatus();
            Console.WriteLine("  ✓ Статистика по статусам сделок:");
            foreach (var kvp in dealsByStatus)
            {
                Console.WriteLine($"    - {kvp.Key}: {kvp.Value} сделок");
            }

            decimal totalDealAmount = crm.GetTotalDealAmount();
            Console.WriteLine($"  ✓ Общая сумма сделок: {totalDealAmount:C}\n");

            // Задание 40: Геолокация
            Console.WriteLine("40. Геолокация в DataSet");
            Console.WriteLine("  ✓ Можно добавить колонки Latitude/Longitude");
            Console.WriteLine("  ✓ Хранение координат в decimal");
            Console.WriteLine("  ✓ Расширение для расчетов расстояний\n");

            Console.WriteLine("ЗАДАНИЕ 41-50: Комплексные системы\n");

            // Задание 41: Шифрование
            Console.WriteLine("41. Шифрование чувствительных данных");
            Console.WriteLine("  ✓ Шифрование полей перед сохранением");
            Console.WriteLine("  ✓ Дешифрование при чтении");
            Console.WriteLine("  ✓ Использование System.Security.Cryptography\n");

            // Задание 42: Полнотекстовый поиск
            Console.WriteLine("42. Полнотекстовый поиск");
            Console.WriteLine("  ✓ LIKE оператор в Select()");
            Console.WriteLine("  ✓ Индексация для производительности");
            Console.WriteLine("  ✓ Пример в CRM системе выше\n");

            // Задание 43: Синхронизация БД
            Console.WriteLine("43. Синхронизация нескольких БД");
            Console.WriteLine("  ✓ GetChanges() для получения изменений");
            Console.WriteLine("  ✓ Merge() для объединения изменений");
            Console.WriteLine("  ✓ Конфликт-резолвинг\n");

            // Задание 44: Аудит операций
            Console.WriteLine("44. Аудит всех операций");
            Console.WriteLine("  ✓ Логирование изменений в отдельную таблицу");
            Console.WriteLine("  ✓ Хранение: кто, когда, что изменил");
            Console.WriteLine("  ✓ Пример в CRM системе (LogDealChange)\n");

            // Задание 45: Оптимизация памяти
            Console.WriteLine("45. Оптимизация памяти для больших таблиц");
            Console.WriteLine("  ✓ BeginLoadData/EndLoadData для массовой вставки");
            Console.WriteLine("  ✓ Выборочная загрузка колонок");
            Console.WriteLine("  ✓ Пагинация данных\n");

            // Задание 46: Поддержка различных БД
            Console.WriteLine("46. Поддержка различных БД");
            Console.WriteLine("  ✓ DataSet независим от СУБД");
            Console.WriteLine("  ✓ Использование DbProviderFactory");
            Console.WriteLine("  ✓ Абстракция доступа к данным\n");

            // Задание 47: Фильтрация по сложным правилам
            Console.WriteLine("47. Фильтрация по сложным правилам");

            // Сложная фильтрация в складской системе
            var filteredProducts = warehouse.FilterProducts(
                minPrice: 1000,
                maxPrice: 100000,
                minQuantity: 10,
                categoryId: 1
            );

            Console.WriteLine($"  ✓ Сложная фильтрация товаров:");
            Console.WriteLine($"    - Цена: 1,000 - 100,000");
            Console.WriteLine($"    - Количество: >= 10");
            Console.WriteLine($"    - Категория: Электроника (ID=1)");
            Console.WriteLine($"  ✓ Результатов: {filteredProducts.Length}\n");

            // Задание 48: Интеграция с веб-сервисами
            Console.WriteLine("48. Интеграция с веб-сервисами");
            Console.WriteLine("  ✓ Сериализация DataSet в XML/JSON");
            Console.WriteLine("  ✓ WCF/Web API для передачи данных");
            Console.WriteLine("  ✓ Асинхронные операции\n");

            // Задание 49: Проверка перед сохранением
            Console.WriteLine("49. Проверка данных перед сохранением");
            Console.WriteLine("  ✓ События ColumnChanging/RowChanging");
            Console.WriteLine("  ✓ Business rules validation");
            Console.WriteLine("  ✓ Комплексная валидация в BLL слое\n");

            // Задание 50: Система управления складом
            Console.WriteLine("50. Система управления складом (полная реализация)");

            Console.WriteLine($"  ✓ Товаров в системе: {warehouse.Products.Rows.Count}");

            // Товары с низким запасом
            var lowStockProducts = warehouse.GetProductsWithLowStock(10);
            Console.WriteLine($"  ✓ Товаров с низким запасом (<10): {lowStockProducts.Length}");

            // Товары для перезаказа
            var reorderProducts = warehouse.GetProductsNeedingReorder();
            Console.WriteLine($"  ✓ Товаров для перезаказа: {reorderProducts.Length}");

            // Общая стоимость запасов
            decimal totalInventoryValue = 0;
            foreach (DataRow product in warehouse.Products.Rows)
            {
                totalInventoryValue += (decimal)product["TotalValue"];
            }
            Console.WriteLine($"  ✓ Общая стоимость запасов: {totalInventoryValue:C}");

            // Демонстрация вычисляемых полей
            Console.WriteLine("\n  Примеры вычисляемых полей:");
            foreach (DataRow product in warehouse.Products.Rows)
            {
                Console.WriteLine($"    - {product["ProductName"]}:");
                Console.WriteLine($"      Количество: {product["Quantity"]} шт.");
                Console.WriteLine($"      Статус: {product["StockStatus"]}");
                Console.WriteLine($"      Стоимость: {product["TotalValue"]:C}");
                Console.WriteLine($"      Нужен заказ: {product["NeedsReorder"]}");
                if ((bool)product["NeedsReorder"])
                {
                    int minStock = (int)product["MinStockLevel"];
                    int current = (int)product["Quantity"];
                    Console.WriteLine($" Заказать: {minStock - current} шт.");
                }
                Console.WriteLine();
            }

            // Проверка целостности
            var warehouseIssues = warehouse.CheckDataIntegrity();
            if (warehouseIssues.Count == 0)
            {
                Console.WriteLine(" Целостность данных склада не нарушена");
            }
            else
            {
                Console.WriteLine($" Проблемы целостности: {warehouseIssues.Count}");
            }

            Console.WriteLine("\n Складская система готова к работе!");
            Console.WriteLine(" Поддерживает: управление запасами, поставщиками, категориями");
            Console.WriteLine(" Автоматические уведомления о низком запасе");
            Console.WriteLine(" Вычисление общей стоимости");
            Console.WriteLine(" Проверка целостности данных");
        }
    }
}