using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.IO;

namespace DataRelationsComplete
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ПОЛНЫЙ КУРС ПО DATARELATION (20 ЗАДАНИЙ) ===\n");

            while (true)
            {
                Console.WriteLine("\nВыберите задание для запуска (1-20):");
                for (int i = 1; i <= 20; i++)
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

                if (choice < 1 || choice > 20)
                {
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    continue;
                }

                Console.Clear();
                Console.WriteLine($"=== ЗАДАНИЕ {choice} ===\n");

                switch (choice)
                {
                    case 1: Task1_SimpleRelation(); break;
                    case 2: Task2_GetChildRows(); break;
                    case 3: Task3_GetParentRows(); break;
                    case 4: Task4_SelfRelation(); break;
                    case 5: Task5_SelfRelationFiltering(); break;
                    case 6: Task6_ManyToMany(); break;
                    case 7: Task7_ManyToManyNavigation(); break;
                    case 8: Task8_CalculatedFields(); break;
                    case 9: Task9_DeleteRule(); break;
                    case 10: Task10_UpdateRule(); break;
                    case 11: Task11_CombinedRules(); break;
                    case 12: Task12_RowStateDeleted(); break;
                    case 13: Task13_RowStateAdded(); break;
                    case 14: Task14_RowStateModified(); break;
                    case 15: Task15_CascadingDeletion(); break;
                    case 16: Task16_ReferentialIntegrity(); break;
                    case 17: Task17_DataViewFiltering(); break;
                    case 18: Task18_HierarchicalExport(); break;
                    case 19: Task19_ComplexEducationalSystem(); break;
                    case 20: Task20_PerformanceOptimization(); break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        #region Задание 1: Создание простого отношения DataRelation
        static void Task1_SimpleRelation()
        {
            Console.WriteLine("ЗАДАНИЕ 1: Создание простого отношения DataRelation\n");
            Console.WriteLine("Цель: Создать отношение 'один-ко-многим' между таблицами Категории и Товары\n");

            DataSet ds = new DataSet("StoreDB");

            // Создаем таблицу Категории
            DataTable categories = new DataTable("Категории");
            categories.Columns.Add("CategoryID", typeof(int));
            categories.Columns.Add("CategoryName", typeof(string));
            categories.Columns.Add("Description", typeof(string));
            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

            // Создаем таблицу Товары
            DataTable products = new DataTable("Товары");
            products.Columns.Add("ProductID", typeof(int));
            products.Columns.Add("ProductName", typeof(string));
            products.Columns.Add("Price", typeof(decimal));
            products.Columns.Add("Quantity", typeof(int));
            products.Columns.Add("CategoryID", typeof(int));
            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

            ds.Tables.Add(categories);
            ds.Tables.Add(products);

            // Заполняем данными
            FillCategoriesAndProducts(categories, products);

            // Создаем отношение
            Console.WriteLine("1. Создание отношения DataRelation:");
            try
            {
                DataRelation relation = new DataRelation(
                    "FK_Products_Categories",
                    categories.Columns["CategoryID"],
                    products.Columns["CategoryID"],
                    true);

                ds.Relations.Add(relation);
                Console.WriteLine("✓ Отношение успешно создано");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
                return;
            }

            // Информация об отношении
            Console.WriteLine("\n2. Информация об отношении:");
            DataRelation relationInfo = ds.Relations["FK_Products_Categories"];
            Console.WriteLine($"   Имя: {relationInfo.RelationName}");
            Console.WriteLine($"   Родительская таблица: {relationInfo.ParentTable.TableName}");
            Console.WriteLine($"   Дочерняя таблица: {relationInfo.ChildTable.TableName}");
            Console.WriteLine($"   Родительская колонка: {relationInfo.ParentColumns[0].ColumnName}");
            Console.WriteLine($"   Дочерняя колонка: {relationInfo.ChildColumns[0].ColumnName}");
            Console.WriteLine($"   CreateConstraints: {relationInfo.ChildKeyConstraint != null}");

            // Иерархическая структура
            Console.WriteLine("\n3. Иерархическая структура:");
            PrintCategoryProductHierarchy(ds);

            // Обработка исключений
            Console.WriteLine("\n4. Тест обработки исключений:");
            TestRelationExceptions();
        }

        static void FillCategoriesAndProducts(DataTable categories, DataTable products)
        {
            categories.Rows.Add(1, "Электроника", "Электронные устройства");
            categories.Rows.Add(2, "Книги", "Печатные издания");
            categories.Rows.Add(3, "Одежда", "Одежда и аксессуары");

            products.Rows.Add(101, "Ноутбук", 75000, 5, 1);
            products.Rows.Add(102, "Смартфон", 45000, 8, 1);
            products.Rows.Add(103, "Наушники", 12000, 15, 1);
            products.Rows.Add(104, "Программирование C#", 1500, 20, 2);
            products.Rows.Add(105, "Базы данных", 1800, 12, 2);
            products.Rows.Add(106, "Футболка", 1500, 30, 3);
            products.Rows.Add(107, "Джинсы", 3500, 18, 3);
            products.Rows.Add(108, "Куртка", 12000, 7, 3);
        }

        static void PrintCategoryProductHierarchy(DataSet ds)
        {
            DataTable categories = ds.Tables["Категории"];
            DataRelation relation = ds.Relations["FK_Products_Categories"];

            foreach (DataRow category in categories.Rows)
            {
                Console.WriteLine($"\n[{category["CategoryID"]}] {category["CategoryName"]}");
                Console.WriteLine($"   Описание: {category["Description"]}");

                DataRow[] products = category.GetChildRows(relation);
                if (products.Length == 0)
                {
                    Console.WriteLine("   Нет товаров");
                }
                else
                {
                    Console.WriteLine("   Товары:");
                    foreach (DataRow product in products)
                    {
                        Console.WriteLine($"   • {product["ProductName"]} - {product["Price"]:C} (Остаток: {product["Quantity"]})");
                    }
                }
            }
        }

        static void TestRelationExceptions()
        {
            try
            {
                DataSet testDs = new DataSet();
                DataTable t1 = new DataTable("T1");
                t1.Columns.Add("ID", typeof(int));

                DataTable t2 = new DataTable("T2");
                t2.Columns.Add("ID", typeof(string)); // Несовместимый тип

                testDs.Tables.Add(t1);
                testDs.Tables.Add(t2);

                try
                {
                    DataRelation badRel = new DataRelation(
                        "Bad",
                        t1.Columns["ID"],
                        t2.Columns["ID"],
                        true);

                    // Если дошли сюда, значит исключение не было брошено
                    testDs.Relations.Add(badRel);
                    Console.WriteLine("Ожидалось исключение, но его не было");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Правильно обработано несовпадение типов");
                }
                catch (InvalidConstraintException ex)
                {
                    Console.WriteLine("Правильно обработано несовпадение типов (InvalidConstraintException)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }
        }
        #endregion

        #region Задание 2: Получение дочерних строк
        static void Task2_GetChildRows()
        {
            Console.WriteLine("ЗАДАНИЕ 2: Получение дочерних строк с помощью GetChildRows()\n");
            Console.WriteLine("Цель: Научиться получать связанные данные через отношения\n");

            DataSet ds = CreateStoreDataSet();
            DataTable categories = ds.Tables["Категории"];
            DataRelation relation = ds.Relations["FK_Products_Categories"];

            // 1. Получение всех товаров по категориям
            Console.WriteLine("1. Все товары по категориям:");
            foreach (DataRow category in categories.Rows)
            {
                Console.WriteLine($"\nКатегория: {category["CategoryName"]}");
                DataRow[] products = category.GetChildRows(relation);

                if (products.Length == 0)
                    Console.WriteLine("   Нет товаров");
                else
                    foreach (DataRow product in products)
                        Console.WriteLine($"   • {product["ProductName"]}");
            }

            // 2. Поиск в конкретной категории
            Console.WriteLine("\n\n2. Поиск в категории 'Электроника':");
            DataRow[] electronics = categories.Select("CategoryName = 'Электроника'");
            if (electronics.Length > 0)
            {
                DataRow[] electronicProducts = electronics[0].GetChildRows(relation);
                Console.WriteLine($"Найдено товаров: {electronicProducts.Length}");
            }

            // 3. Подсчет количества товаров
            Console.WriteLine("\n\n3. Количество товаров по категориям:");
            Console.WriteLine($"{"Категория",-15} {"Кол-во товаров"}");
            Console.WriteLine(new string('-', 30));

            foreach (DataRow category in categories.Rows)
            {
                int count = category.GetChildRows(relation).Length;
                Console.WriteLine($"{category["CategoryName"],-15} {count}");
            }

            // 4. Суммарная стоимость
            Console.WriteLine("\n\n4. Суммарная стоимость товаров:");
            Console.WriteLine($"{"Категория",-15} {"Общая стоимость"}");
            Console.WriteLine(new string('-', 35));

            foreach (DataRow category in categories.Rows)
            {
                decimal total = 0;
                foreach (DataRow product in category.GetChildRows(relation))
                {
                    total += (decimal)product["Price"] * (int)product["Quantity"];
                }
                Console.WriteLine($"{category["CategoryName"],-15} {total:C}");
            }

            // 5. Фильтрация по цене
            Console.WriteLine("\n\n5. Категории с товарами дороже 10000:");
            decimal minPrice = 10000;
            foreach (DataRow category in categories.Rows)
            {
                bool hasExpensive = false;
                foreach (DataRow product in category.GetChildRows(relation))
                {
                    if ((decimal)product["Price"] > minPrice)
                    {
                        hasExpensive = true;
                        break;
                    }
                }

                if (hasExpensive)
                    Console.WriteLine($"• {category["CategoryName"]}");
            }
        }

        static DataSet CreateStoreDataSet()
        {
            DataSet ds = new DataSet("Store");

            DataTable categories = new DataTable("Категории");
            categories.Columns.Add("CategoryID", typeof(int));
            categories.Columns.Add("CategoryName", typeof(string));
            categories.Columns.Add("Description", typeof(string));
            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

            DataTable products = new DataTable("Товары");
            products.Columns.Add("ProductID", typeof(int));
            products.Columns.Add("ProductName", typeof(string));
            products.Columns.Add("Price", typeof(decimal));
            products.Columns.Add("Quantity", typeof(int));
            products.Columns.Add("CategoryID", typeof(int));
            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

            ds.Tables.Add(categories);
            ds.Tables.Add(products);

            FillCategoriesAndProducts(categories, products);

            DataRelation relation = new DataRelation(
                "FK_Products_Categories",
                categories.Columns["CategoryID"],
                products.Columns["CategoryID"],
                true);

            ds.Relations.Add(relation);
            return ds;
        }
        #endregion

        #region Задание 3: Получение родительских строк
        static void Task3_GetParentRows()
        {
            Console.WriteLine("ЗАДАНИЕ 3: Получение родительских строк с помощью GetParentRows()\n");
            Console.WriteLine("Цель: Научиться получать информацию о родительских записях\n");

            // Создаем DataSet БЕЗ ограничений целостности для теста
            DataSet ds = new DataSet("Store");

            DataTable categories = new DataTable("Категории");
            categories.Columns.Add("CategoryID", typeof(int));
            categories.Columns.Add("CategoryName", typeof(string));
            categories.Columns.Add("Description", typeof(string));
            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

            DataTable products = new DataTable("Товары");
            products.Columns.Add("ProductID", typeof(int));
            products.Columns.Add("ProductName", typeof(string));
            products.Columns.Add("Price", typeof(decimal));
            products.Columns.Add("Quantity", typeof(int));
            products.Columns.Add("CategoryID", typeof(int));
            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

            ds.Tables.Add(categories);
            ds.Tables.Add(products);

            // Заполняем данными
            categories.Rows.Add(1, "Электроника", "Электронные устройства");
            categories.Rows.Add(2, "Книги", "Печатные издания");
            categories.Rows.Add(3, "Одежда", "Одежда и аксессуары");

            products.Rows.Add(101, "Ноутбук", 75000, 5, 1);
            products.Rows.Add(102, "Смартфон", 45000, 8, 1);
            products.Rows.Add(103, "Наушники", 12000, 15, 1);
            products.Rows.Add(104, "Программирование C#", 1500, 20, 2);
            products.Rows.Add(105, "Базы данных", 1800, 12, 2);
            products.Rows.Add(106, "Футболка", 1500, 30, 3);
            products.Rows.Add(107, "Джинсы", 3500, 18, 3);
            products.Rows.Add(108, "Куртка", 12000, 7, 3);

            // Создаем отношение БЕЗ ограничений целостности
            DataRelation relation = new DataRelation(
                "FK_Products_Categories",
                categories.Columns["CategoryID"],
                products.Columns["CategoryID"],
                false); // false = не создавать ограничения целостности

            ds.Relations.Add(relation);

            // 1. Информация о товарах с категориями
            Console.WriteLine("1. Товары с категориями:");
            Console.WriteLine($"{"Товар",-20} {"Категория",-15} {"Описание категории"}");
            Console.WriteLine(new string('-', 70));

            for (int i = 0; i < Math.Min(5, products.Rows.Count); i++)
            {
                DataRow product = products.Rows[i];
                DataRow[] parents = product.GetParentRows(relation);

                string category = parents.Length > 0 ?
                    parents[0]["CategoryName"].ToString() : "Без категории";
                string desc = parents.Length > 0 ?
                    parents[0]["Description"].ToString() : "";

                Console.WriteLine($"{product["ProductName"],-20} {category,-15} {desc}");
            }

            // 2. Поиск по ID
            Console.WriteLine("\n\n2. Поиск товара по ID:");
            int searchId = 102;
            DataRow found = products.Rows.Find(searchId);

            if (found != null)
            {
                Console.WriteLine($"Товар найден: {found["ProductName"]}");
                DataRow[] parents = found.GetParentRows(relation);
                if (parents.Length > 0)
                    Console.WriteLine($"Категория: {parents[0]["CategoryName"]}");
            }

            // 3. Товары без категорий
            Console.WriteLine("\n\n3. Товары без категорий (orphaned records):");

            // Метод 1: Добавляем товар с несуществующей категорией ДО создания отношения
            Console.WriteLine("Метод 1: Добавление orphaned записи:");
            DataRow orphan = products.NewRow();
            orphan["ProductID"] = 999;
            orphan["ProductName"] = "Сиротский товар";
            orphan["Price"] = 1000;
            orphan["Quantity"] = 1;
            orphan["CategoryID"] = 999; // Несуществующая категория
            products.Rows.Add(orphan);

            Console.WriteLine("Добавлен товар с CategoryID = 999 (несуществующая категория)");

            // Метод 2: Изменяем существующий товар на несуществующую категорию
            Console.WriteLine("\nМетод 2: Изменение категории существующего товара:");
            DataRow existingProduct = products.Rows.Find(103); // Наушники
            int originalCategory = (int)existingProduct["CategoryID"];
            existingProduct["CategoryID"] = 888; // Несуществующая категория

            // Проверяем orphaned records
            int orphanCount = 0;
            Console.WriteLine("\nПоиск orphaned records:");
            foreach (DataRow product in products.Rows)
            {
                DataRow[] parents = product.GetParentRows(relation);
                if (parents.Length == 0)
                {
                    Console.WriteLine($"  • {product["ProductName"]} (ID: {product["ProductID"]}, CategoryID: {product["CategoryID"]})");
                    orphanCount++;
                }
            }

            Console.WriteLine($"Всего orphaned records: {orphanCount}");

            // Восстанавливаем оригинальное значение
            existingProduct["CategoryID"] = originalCategory;

            // 4. Проверка целостности (ручная)
            Console.WriteLine("\n\n4. Ручная проверка ссылочной целостности:");
            int issues = 0;

            // Создаем HashSet существующих категорий для быстрой проверки
            HashSet<int> existingCategories = new HashSet<int>(
                categories.Rows.Cast<DataRow>().Select(r => (int)r["CategoryID"]));

            foreach (DataRow product in products.Rows)
            {
                int categoryId = (int)product["CategoryID"];
                if (!existingCategories.Contains(categoryId))
                {
                    Console.WriteLine($"✗ Товар {product["ProductID"]} '{product["ProductName"]}' " +
                        $"ссылается на несуществующую категорию {categoryId}");
                    issues++;
                }
            }

            if (issues == 0)
                Console.WriteLine("✓ Все ссылки целостны");
            else
                Console.WriteLine($"\nВсего нарушений целостности: {issues}");

            // 5. Демонстрация работы с ограничениями
            Console.WriteLine("\n\n5. Демонстрация ограничений целостности:");

            // Создаем новый DataSet С ограничениями
            DataSet dsWithConstraints = new DataSet("StoreWithConstraints");

            DataTable categories2 = categories.Clone();
            DataTable products2 = products.Clone();

            dsWithConstraints.Tables.Add(categories2);
            dsWithConstraints.Tables.Add(products2);

            // Копируем данные (кроме orphaned записи)
            foreach (DataRow row in categories.Rows)
                categories2.ImportRow(row);

            foreach (DataRow row in products.Rows)
            {
                if ((int)row["ProductID"] != 999) // Не копируем orphaned запись
                    products2.ImportRow(row);
            }

            // Создаем отношение С ограничениями
            DataRelation relationWithConstraints = new DataRelation(
                "FK_Products_Categories",
                categories2.Columns["CategoryID"],
                products2.Columns["CategoryID"],
                true); // true = создавать ограничения целостности

            dsWithConstraints.Relations.Add(relationWithConstraints);

            Console.WriteLine("Создано отношение с ограничениями целостности");
            Console.WriteLine("Попытка добавить товар с несуществующей категорией...");

            try
            {
                DataRow badProduct = products2.NewRow();
                badProduct["ProductID"] = 1000;
                badProduct["ProductName"] = "Некорректный товар";
                badProduct["Price"] = 1000;
                badProduct["Quantity"] = 1;
                badProduct["CategoryID"] = 777; // Несуществующая категория

                products2.Rows.Add(badProduct);
                Console.WriteLine("✗ Ожидалось исключение, но его не было");
            }
            catch (InvalidConstraintException ex)
            {
                Console.WriteLine($"✓ Исключение перехвачено: {ex.Message}");
            }

            // 6. Удаление orphaned записи из оригинального DataSet
            Console.WriteLine("\n\n6. Очистка данных:");
            orphan.Delete();
            products.AcceptChanges();
            Console.WriteLine("Orphaned запись удалена");
        }
        #endregion

        #region Задание 4: Отношение "сам к себе"
        static void Task4_SelfRelation()
        {
            Console.WriteLine("ЗАДАНИЕ 4: Создание отношения 'сам к себе' для иерархических данных\n");
            Console.WriteLine("Цель: Работа с иерархическими данными сотрудников\n");

            DataSet ds = new DataSet("Company");
            DataTable employees = CreateEmployeesTable();
            ds.Tables.Add(employees);

            // Создаем отношение "сам к себе"
            try
            {
                DataRelation selfRelation = new DataRelation(
                    "FK_Employees_Managers",
                    employees.Columns["EmployeeID"],
                    employees.Columns["ManagerID"],
                    false);

                ds.Relations.Add(selfRelation);
                Console.WriteLine("✓ Отношение 'сам к себе' создано");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
                return;
            }

            DataRelation relation = ds.Relations["FK_Employees_Managers"];

            // 1. Иерархия сотрудников
            Console.WriteLine("\n1. Иерархия компании:");
            DataRow[] topManagers = employees.Select("ManagerID IS NULL");
            foreach (DataRow manager in topManagers)
                PrintEmployeeHierarchy(manager, relation, 0);

            // 2. Сотрудники и руководители
            Console.WriteLine("\n\n2. Сотрудники и их руководители:");
            Console.WriteLine($"{"Сотрудник",-25} {"Отдел",-15} {"Руководитель",-25}");
            Console.WriteLine(new string('-', 70));

            foreach (DataRow emp in employees.Rows)
            {
                string managerName = "Нет (глава)";
                DataRow[] managers = emp.GetParentRows(relation);
                if (managers.Length > 0)
                    managerName = managers[0]["EmployeeName"].ToString();

                Console.WriteLine($"{emp["EmployeeName"],-25} {emp["Department"],-15} {managerName,-25}");
            }

            // 3. Менеджеры и подчиненные
            Console.WriteLine("\n\n3. Менеджеры и их подчиненные:");
            foreach (DataRow emp in employees.Rows)
            {
                DataRow[] subordinates = emp.GetChildRows(relation);
                if (subordinates.Length > 0)
                {
                    Console.WriteLine($"\n{emp["EmployeeName"]} ({emp["Department"]}):");
                    foreach (DataRow sub in subordinates)
                        Console.WriteLine($"  • {sub["EmployeeName"]}");
                }
            }

            // 4. Глубина иерархии
            Console.WriteLine("\n\n4. Глубина иерархии:");
            Console.WriteLine($"{"Сотрудник",-25} {"Уровень"}");
            Console.WriteLine(new string('-', 35));

            foreach (DataRow emp in employees.Rows)
            {
                int level = CalculateHierarchyLevel(emp, relation);
                Console.WriteLine($"{emp["EmployeeName"],-25} {level}");
            }
        }

        static DataTable CreateEmployeesTable()
        {
            DataTable employees = new DataTable("Сотрудники");
            employees.Columns.Add("EmployeeID", typeof(int));
            employees.Columns.Add("EmployeeName", typeof(string));
            employees.Columns.Add("Department", typeof(string));
            employees.Columns.Add("Salary", typeof(decimal));
            employees.Columns.Add("ManagerID", typeof(int));
            employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

            // Заполняем иерархию
            employees.Rows.Add(1, "Иванов И.И.", "Дирекция", 250000, DBNull.Value);
            employees.Rows.Add(2, "Петров П.П.", "IT", 180000, 1);
            employees.Rows.Add(3, "Сидорова А.В.", "Финансы", 170000, 1);
            employees.Rows.Add(4, "Васильева М.Д.", "IT", 120000, 2);
            employees.Rows.Add(5, "Николаев Д.А.", "Финансы", 110000, 3);
            employees.Rows.Add(6, "Смирнов С.В.", "IT", 90000, 4);
            employees.Rows.Add(7, "Федорова О.П.", "IT", 85000, 4);

            return employees;
        }

        static void PrintEmployeeHierarchy(DataRow employee, DataRelation relation, int level)
        {
            string indent = new string(' ', level * 2);
            Console.WriteLine($"{indent}• {employee["EmployeeName"]} ({employee["Department"]}) - {employee["Salary"]:C}");

            foreach (DataRow subordinate in employee.GetChildRows(relation))
                PrintEmployeeHierarchy(subordinate, relation, level + 1);
        }

        static int CalculateHierarchyLevel(DataRow employee, DataRelation relation)
        {
            int level = 0;
            DataRow current = employee;

            while (true)
            {
                DataRow[] managers = current.GetParentRows(relation);
                if (managers.Length == 0)
                    break;

                level++;
                current = managers[0];
            }

            return level;
        }
        #endregion

        #region Задание 5: Фильтрация в отношении "сам к себе"
        static void Task5_SelfRelationFiltering()
        {
            Console.WriteLine("ЗАДАНИЕ 5: Получение данных из отношения 'сам к себе' с фильтрацией\n");
            Console.WriteLine("Цель: Научиться фильтровать иерархические данные\n");

            DataSet ds = new DataSet("Company");
            DataTable employees = CreateEmployeesTable();
            ds.Tables.Add(employees);

            DataRelation relation = new DataRelation(
                "FK_Employees_Managers",
                employees.Columns["EmployeeID"],
                employees.Columns["ManagerID"],
                false);
            ds.Relations.Add(relation);

            // 1. Прямые подчиненные
            Console.WriteLine("1. Прямые подчиненные менеджера:");
            int managerId = 2; // Петров П.П.
            DataRow manager = employees.Rows.Find(managerId);

            if (manager != null)
            {
                Console.WriteLine($"Менеджер: {manager["EmployeeName"]}");
                DataRow[] subordinates = manager.GetChildRows(relation);

                if (subordinates.Length > 0)
                    foreach (DataRow sub in subordinates)
                        Console.WriteLine($"  • {sub["EmployeeName"]}");
                else
                    Console.WriteLine("  Нет подчиненных");
            }

            // 2. Цепочка руководства
            Console.WriteLine("\n\n2. Цепочка руководства сотрудника:");
            int empId = 6; // Смирнов С.В.
            DataRow employee = employees.Rows.Find(empId);

            if (employee != null)
            {
                Console.WriteLine($"Сотрудник: {employee["EmployeeName"]}");
                Console.WriteLine("Цепочка руководства:");
                PrintManagementChain(employee, relation);
            }

            // 3. Сотрудники по уровням
            Console.WriteLine("\n\n3. Сотрудники по уровням иерархии:");
            Dictionary<int, List<DataRow>> byLevel = new Dictionary<int, List<DataRow>>();

            foreach (DataRow emp in employees.Rows)
            {
                int level = CalculateHierarchyLevel(emp, relation);
                if (!byLevel.ContainsKey(level))
                    byLevel[level] = new List<DataRow>();

                byLevel[level].Add(emp);
            }

            foreach (var level in byLevel.Keys.OrderBy(k => k))
            {
                Console.WriteLine($"\nУровень {level}:");
                foreach (DataRow emp in byLevel[level])
                    Console.WriteLine($"  • {emp["EmployeeName"]}");
            }

            // 4. Статистика иерархии
            Console.WriteLine("\n\n4. Статистика иерархии:");
            Console.WriteLine($"Всего сотрудников: {employees.Rows.Count}");
            Console.WriteLine($"Уровней иерархии: {byLevel.Keys.Max() + 1}");
            Console.WriteLine($"Среднее число подчиненных: {employees.Rows.Count / (double)byLevel.Keys.Count:F1}");

            // 5. Коллеги (общий руководитель)
            Console.WriteLine("\n\n5. Коллеги (общий руководитель):");
            int colleagueId = 6; // Смирнов С.В.
            DataRow colleague = employees.Rows.Find(colleagueId);

            if (colleague != null && colleague["ManagerID"] != DBNull.Value)
            {
                int managerID = (int)colleague["ManagerID"];
                DataRow commonManager = employees.Rows.Find(managerID);

                Console.WriteLine($"Сотрудник: {colleague["EmployeeName"]}");
                Console.WriteLine($"Общий руководитель: {commonManager["EmployeeName"]}");

                DataRow[] allColleagues = commonManager.GetChildRows(relation);
                Console.WriteLine("Коллеги:");
                foreach (DataRow col in allColleagues)
                    if ((int)col["EmployeeID"] != colleagueId)
                        Console.WriteLine($"  • {col["EmployeeName"]}");
            }
        }

        static void PrintManagementChain(DataRow employee, DataRelation relation)
        {
            int level = 0;
            DataRow current = employee;

            Console.WriteLine($"{new string(' ', level * 2)}• {current["EmployeeName"]}");

            while (true)
            {
                DataRow[] managers = current.GetParentRows(relation);
                if (managers.Length == 0)
                    break;

                current = managers[0];
                level++;
                Console.WriteLine($"{new string(' ', level * 2)}↑ {current["EmployeeName"]}");
            }
        }
        #endregion

        #region Задание 6: Отношение многие-ко-многим
        static void Task6_ManyToMany()
        {
            Console.WriteLine("ЗАДАНИЕ 6: Реализация отношения многие-ко-многим через промежуточную таблицу\n");
            Console.WriteLine("Цель: Создать систему студентов и курсов\n");

            DataSet ds = CreateUniversityDataSet();
            DataTable students = ds.Tables["Студенты"];
            DataTable courses = ds.Tables["Курсы"];
            DataTable registration = ds.Tables["Регистрация"];

            DataRelation studentRel = ds.Relations["FK_Registration_Students"];
            DataRelation courseRel = ds.Relations["FK_Registration_Courses"];

            // 1. Курсы студента
            Console.WriteLine("1. Курсы студента:");
            int studentId = 101;
            DataRow student = students.Rows.Find(studentId);

            if (student != null)
            {
                Console.WriteLine($"Студент: {student["StudentName"]}");
                DataRow[] registrations = student.GetChildRows(studentRel);

                if (registrations.Length > 0)
                {
                    Console.WriteLine("Зарегистрирован на:");
                    foreach (DataRow reg in registrations)
                    {
                        DataRow[] courseRows = reg.GetParentRows(courseRel);
                        if (courseRows.Length > 0)
                            Console.WriteLine($"  • {courseRows[0]["CourseName"]}");
                    }
                }
                else
                    Console.WriteLine("  Не зарегистрирован на курсы");
            }

            // 2. Студенты курса
            Console.WriteLine("\n\n2. Студенты курса:");
            string courseId = "C001";
            DataRow course = courses.Rows.Find(courseId);

            if (course != null)
            {
                Console.WriteLine($"Курс: {course["CourseName"]}");
                DataRow[] registrations = course.GetChildRows(courseRel);

                if (registrations.Length > 0)
                {
                    Console.WriteLine("Студенты:");
                    foreach (DataRow reg in registrations)
                    {
                        DataRow[] studentRows = reg.GetParentRows(studentRel);
                        if (studentRows.Length > 0)
                            Console.WriteLine($"  • {studentRows[0]["StudentName"]}");
                    }
                }
                else
                    Console.WriteLine("  Нет студентов");
            }

            // 3. Статистика
            Console.WriteLine("\n\n3. Статистика:");
            Console.WriteLine($"{"Курс",-20} {"Студентов",-10}");
            Console.WriteLine(new string('-', 30));

            foreach (DataRow crs in courses.Rows)
            {
                int count = crs.GetChildRows(courseRel).Length;
                Console.WriteLine($"{crs["CourseName"],-20} {count,-10}");
            }

            Console.WriteLine($"\n{"Студент",-20} {"Курсов",-10}");
            Console.WriteLine(new string('-', 30));

            foreach (DataRow std in students.Rows)
            {
                int count = std.GetChildRows(studentRel).Length;
                Console.WriteLine($"{std["StudentName"],-20} {count,-10}");
            }

            // 4. Матрица регистраций
            Console.WriteLine("\n\n4. Матрица регистраций:");
            Console.Write("Студент/Курс".PadRight(20));
            foreach (DataRow crs in courses.Rows)
                Console.Write(crs["CourseName"].ToString().Substring(0, Math.Min(10, crs["CourseName"].ToString().Length)).PadRight(12));
            Console.WriteLine();
            Console.WriteLine(new string('-', 20 + courses.Rows.Count * 12));

            foreach (DataRow std in students.Rows)
            {
                Console.Write(std["StudentName"].ToString().PadRight(20));
                DataRow[] stdRegs = std.GetChildRows(studentRel);

                foreach (DataRow crs in courses.Rows)
                {
                    bool isRegistered = false;
                    foreach (DataRow reg in stdRegs)
                    {
                        DataRow[] regCourses = reg.GetParentRows(courseRel);
                        if (regCourses.Length > 0 && (string)regCourses[0]["CourseID"] == (string)crs["CourseID"])
                        {
                            isRegistered = true;
                            break;
                        }
                    }

                    Console.Write($"{(isRegistered ? "[X]" : "[ ]").PadRight(12)}");
                }
                Console.WriteLine();
            }
        }

        static DataSet CreateUniversityDataSet()
        {
            DataSet ds = new DataSet("University");

            // Студенты
            DataTable students = new DataTable("Студенты");
            students.Columns.Add("StudentID", typeof(int));
            students.Columns.Add("StudentName", typeof(string));
            students.Columns.Add("Email", typeof(string));
            students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

            // Курсы
            DataTable courses = new DataTable("Курсы");
            courses.Columns.Add("CourseID", typeof(string));
            courses.Columns.Add("CourseName", typeof(string));
            courses.Columns.Add("Instructor", typeof(string));
            courses.PrimaryKey = new DataColumn[] { courses.Columns["CourseID"] };

            // Регистрация
            DataTable registration = new DataTable("Регистрация");
            registration.Columns.Add("RegistrationID", typeof(int));
            registration.Columns.Add("StudentID", typeof(int));
            registration.Columns.Add("CourseID", typeof(string));
            registration.Columns.Add("EnrollmentDate", typeof(DateTime));
            registration.Columns.Add("Grade", typeof(double));
            registration.PrimaryKey = new DataColumn[] { registration.Columns["RegistrationID"] };

            ds.Tables.Add(students);
            ds.Tables.Add(courses);
            ds.Tables.Add(registration);

            // Данные
            students.Rows.Add(101, "Иван Петров", "ivan@example.com");
            students.Rows.Add(102, "Мария Сидорова", "maria@example.com");
            students.Rows.Add(103, "Петр Иванов", "petr@example.com");

            courses.Rows.Add("C001", "C# Programming", "Дмитрий Волков");
            courses.Rows.Add("C002", "Database Design", "Светлана Морозова");
            courses.Rows.Add("C003", "Web Development", "Алексей Новиков");

            registration.Rows.Add(1, 101, "C001", new DateTime(2024, 1, 15), 4.5);
            registration.Rows.Add(2, 101, "C002", new DateTime(2024, 1, 20), 3.8);
            registration.Rows.Add(3, 102, "C001", new DateTime(2024, 1, 15), 4.8);
            registration.Rows.Add(4, 102, "C003", new DateTime(2024, 2, 5), 4.2);
            registration.Rows.Add(5, 103, "C002", new DateTime(2024, 1, 20), 3.5);
            registration.Rows.Add(6, 103, "C003", new DateTime(2024, 2, 5), 4.0);

            // Отношения
            ds.Relations.Add(new DataRelation(
                "FK_Registration_Students",
                students.Columns["StudentID"],
                registration.Columns["StudentID"],
                true));

            ds.Relations.Add(new DataRelation(
                "FK_Registration_Courses",
                courses.Columns["CourseID"],
                registration.Columns["CourseID"],
                true));

            return ds;
        }
        #endregion

        #region Задание 7: Навигация по отношению многие-ко-многим
        static void Task7_ManyToManyNavigation()
        {
            Console.WriteLine("ЗАДАНИЕ 7: Навигация по отношению многие-ко-многим в обе стороны\n");
            Console.WriteLine("Цель: Полная навигация по системе студенты-курсы-оценки\n");

            DataSet ds = CreateUniversityDataSet();
            DataTable students = ds.Tables["Студенты"];
            DataTable courses = ds.Tables["Курсы"];
            DataTable registration = ds.Tables["Регистрация"];

            DataRelation studentRel = ds.Relations["FK_Registration_Students"];
            DataRelation courseRel = ds.Relations["FK_Registration_Courses"];

            // 1. Студент → Курсы → Оценки
            Console.WriteLine("1. Студент с курсами и оценками:");
            int studentId = 101;
            DataRow mainStudent = students.Rows.Find(studentId); // Изменено имя

            if (mainStudent != null)
            {
                Console.WriteLine($"Студент: {mainStudent["StudentName"]}");
                DataRow[] registrations = mainStudent.GetChildRows(studentRel);

                foreach (DataRow reg in registrations)
                {
                    DataRow[] courseRows = reg.GetParentRows(courseRel);
                    if (courseRows.Length > 0)
                    {
                        DataRow courseData = courseRows[0]; // Изменено имя
                        double grade = (double)reg["Grade"];
                        Console.WriteLine($"  • {courseData["CourseName"]}: {grade:F1}");
                    }
                }
            }

            // 2. Курс → Студенты → Оценки
            Console.WriteLine("\n\n2. Курс со студентами и оценками:");
            string courseId = "C001";
            DataRow mainCourse = courses.Rows.Find(courseId); // Изменено имя

            if (mainCourse != null)
            {
                Console.WriteLine($"Курс: {mainCourse["CourseName"]}");
                DataRow[] registrations = mainCourse.GetChildRows(courseRel);

                foreach (DataRow reg in registrations)
                {
                    DataRow[] studentRows = reg.GetParentRows(studentRel);
                    if (studentRows.Length > 0)
                    {
                        DataRow studentData = studentRows[0]; // Изменено имя
                        double grade = (double)reg["Grade"];
                        Console.WriteLine($"  • {studentData["StudentName"]}: {grade:F1}");
                    }
                }
            }

            // 3. Студенты на одних курсах
            Console.WriteLine("\n\n3. Студенты, учащиеся на одних курсах:");
            int compareStudentId = 101;
            DataRow baseStudent = students.Rows.Find(compareStudentId); // Изменено имя

            if (baseStudent != null)
            {
                Console.WriteLine($"Базовый студент: {baseStudent["StudentName"]}");
                DataRow[] compareRegs = baseStudent.GetChildRows(studentRel);
                var compareCourses = new HashSet<string>();

                foreach (DataRow reg in compareRegs)
                {
                    DataRow[] courseRows = reg.GetParentRows(courseRel);
                    if (courseRows.Length > 0)
                        compareCourses.Add(courseRows[0]["CourseID"].ToString());
                }

                foreach (DataRow otherStudent in students.Rows)
                {
                    if ((int)otherStudent["StudentID"] != compareStudentId)
                    {
                        DataRow[] otherRegs = otherStudent.GetChildRows(studentRel);
                        var otherCourses = new HashSet<string>();

                        foreach (DataRow reg in otherRegs)
                        {
                            DataRow[] courseRows = reg.GetParentRows(courseRel);
                            if (courseRows.Length > 0)
                                otherCourses.Add(courseRows[0]["CourseID"].ToString());
                        }

                        var commonCourses = compareCourses.Intersect(otherCourses).ToList();
                        if (commonCourses.Count > 0)
                        {
                            Console.WriteLine($"  • {otherStudent["StudentName"]}: общие курсы: {commonCourses.Count}");
                        }
                    }
                }
            }

            // 4. Полная информация о регистрации
            Console.WriteLine("\n\n4. Полная информация о всех регистрациях:");
            Console.WriteLine($"{"ID",-5} {"Студент",-15} {"Курс",-20} {"Оценка",-8} {"Дата",-12}");
            Console.WriteLine(new string('-', 65));

            foreach (DataRow reg in registration.Rows)
            {
                DataRow[] studentRows = reg.GetParentRows(studentRel);
                DataRow[] courseRows = reg.GetParentRows(courseRel);

                string studentName = studentRows.Length > 0 ? studentRows[0]["StudentName"].ToString() : "?";
                string courseName = courseRows.Length > 0 ? courseRows[0]["CourseName"].ToString() : "?";

                Console.WriteLine($"{reg["RegistrationID"],-5} {studentName,-15} {courseName,-20} {reg["Grade"],-8:F1} {((DateTime)reg["EnrollmentDate"]):dd.MM.yyyy}");
            }

            // 5. Средняя оценка студента
            Console.WriteLine("\n\n5. Средняя оценка студентов:");
            Console.WriteLine($"{"Студент",-15} {"Средняя оценка"}");
            Console.WriteLine(new string('-', 30));

            foreach (DataRow stud in students.Rows) // Изменено имя
            {
                DataRow[] regs = stud.GetChildRows(studentRel);
                if (regs.Length > 0)
                {
                    double sum = 0;
                    foreach (DataRow reg in regs)
                        sum += (double)reg["Grade"];

                    double average = sum / regs.Length;
                    Console.WriteLine($"{stud["StudentName"],-15} {average:F2}");
                }
            }

            // 6. Средняя оценка по курсам
            Console.WriteLine("\n\n6. Средняя оценка по курсам:");
            Console.WriteLine($"{"Курс",-20} {"Средняя оценка"}");
            Console.WriteLine(new string('-', 35));

            foreach (DataRow crs in courses.Rows) // Изменено имя
            {
                DataRow[] regs = crs.GetChildRows(courseRel);
                if (regs.Length > 0)
                {
                    double sum = 0;
                    foreach (DataRow reg in regs)
                        sum += (double)reg["Grade"];

                    double average = sum / regs.Length;
                    Console.WriteLine($"{crs["CourseName"],-20} {average:F2}");
                }
            }

            // 7. Лучшие студенты
            Console.WriteLine("\n\n7. Лучшие студенты (оценка > 4.5):");
            Console.WriteLine($"{"Студент",-15} {"Средняя оценка"} {"Курсы"}");
            Console.WriteLine(new string('-', 50));

            foreach (DataRow stud in students.Rows) // Изменено имя
            {
                DataRow[] regs = stud.GetChildRows(studentRel);
                if (regs.Length > 0)
                {
                    double sum = 0;
                    foreach (DataRow reg in regs)
                        sum += (double)reg["Grade"];

                    double average = sum / regs.Length;

                    if (average > 4.5)
                    {
                        var courseNames = new List<string>();
                        foreach (DataRow reg in regs)
                        {
                            DataRow[] courseRows = reg.GetParentRows(courseRel);
                            if (courseRows.Length > 0)
                                courseNames.Add(courseRows[0]["CourseName"].ToString());
                        }

                        Console.WriteLine($"{stud["StudentName"],-15} {average,15:F2} {string.Join(", ", courseNames)}");
                    }
                }
            }
        }
        #endregion

        #region Задание 8: Расчетные поля через DataRelation
        static void Task8_CalculatedFields()
        {
            Console.WriteLine("ЗАДАНИЕ 8: Использование DataRelation для создания рассчитываемых полей\n");
            Console.WriteLine("Цель: Создать автоматически рассчитываемые поля через выражения\n");

            DataSet ds = new DataSet("StoreCalculated");

            // Создаем таблицы
            DataTable categories = new DataTable("Категории");
            categories.Columns.Add("CategoryID", typeof(int));
            categories.Columns.Add("CategoryName", typeof(string));
            categories.Columns.Add("Description", typeof(string));
            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

            DataTable products = new DataTable("Товары");
            products.Columns.Add("ProductID", typeof(int));
            products.Columns.Add("ProductName", typeof(string));
            products.Columns.Add("Price", typeof(decimal));
            products.Columns.Add("Quantity", typeof(int));
            products.Columns.Add("CategoryID", typeof(int));
            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

            ds.Tables.Add(categories);
            ds.Tables.Add(products);

            // Заполняем данными
            FillCategoriesAndProducts(categories, products);

            // Создаем отношение
            DataRelation relation = new DataRelation(
                "CatProdRelation",
                categories.Columns["CategoryID"],
                products.Columns["CategoryID"],
                false);

            ds.Relations.Add(relation);

            Console.WriteLine("1. Добавление расчетных полей:");

            // ВАЖНО: В .NET Framework синтаксис для агрегатных функций с Child отличается
            // Правильный синтаксис: Count(Child.ColumnName)

            // Способ 1: Количество товаров в категории
            try
            {
                DataColumn productCount = new DataColumn("ProductCount", typeof(int));
                productCount.Expression = "Count(Child.ProductID)";
                categories.Columns.Add(productCount);
                Console.WriteLine("✓ Добавлено поле: ProductCount = Count(Child.ProductID)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            // Способ 2: Суммарная стоимость товаров в категории
            try
            {
                DataColumn totalValue = new DataColumn
                {
                    ColumnName = "TotalValue",
                    DataType = typeof(decimal),
                    Expression = "Sum(Child(Price * Quantity))"  // ПРАВИЛЬНЫЙ СИНТАКСИС
                };
                categories.Columns.Add(totalValue);
                Console.WriteLine("✓ Добавлено поле: TotalValue = Sum(Child(Price * Quantity))");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
                Console.WriteLine("Пробуем альтернативный синтаксис...");

                try
                {
                    // Альтернативный синтаксис
                    DataColumn totalValueAlt = new DataColumn
                    {
                        ColumnName = "TotalValue",
                        DataType = typeof(decimal),
                        Expression = "Sum(Child, Price * Quantity)"  // Другой вариант синтаксиса
                    };
                    categories.Columns.Add(totalValueAlt);
                    Console.WriteLine("✓ Добавлено поле: TotalValue = Sum(Child, Price * Quantity)");
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"✗ Альтернативный синтаксис тоже не работает: {ex2.Message}");
                }
            }

            // Способ 3: Средняя цена товаров в категории
            try
            {
                DataColumn avgPrice = new DataColumn
                {
                    ColumnName = "AvgPrice",
                    DataType = typeof(decimal),
                    Expression = "Avg(Child(Price))"  // ПРАВИЛЬНЫЙ СИНТАКСИС
                };
                categories.Columns.Add(avgPrice);
                Console.WriteLine("✓ Добавлено поле: AvgPrice = Avg(Child(Price))");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            // Способ 4: Минимальная цена в категории
            try
            {
                DataColumn minPrice = new DataColumn
                {
                    ColumnName = "MinPrice",
                    DataType = typeof(decimal),
                    Expression = "Min(Child(Price))"  // ПРАВИЛЬНЫЙ СИНТАКСИС
                };
                categories.Columns.Add(minPrice);
                Console.WriteLine("✓ Добавлено поле: MinPrice = Min(Child(Price))");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            // Способ 5: Максимальная цена в категории
            try
            {
                DataColumn maxPrice = new DataColumn
                {
                    ColumnName = "MaxPrice",
                    DataType = typeof(decimal),
                    Expression = "Max(Child(Price))"  // ПРАВИЛЬНЫЙ СИНТАКСИС
                };
                categories.Columns.Add(maxPrice);
                Console.WriteLine("✓ Добавлено поле: MaxPrice = Max(Child(Price))");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            // 2. Проверяем расчеты
            Console.WriteLine("\n2. Расчетные значения:");
            Console.WriteLine($"{"Категория",-15} {"Товаров",-8} {"Суммарная",-12} {"Средняя",-10} {"Мин",-8} {"Макс",-8}");
            Console.WriteLine($"{"",-15} {"",-8} {"стоимость",-12} {"цена",-10} {"цена",-8} {"цена",-8}");
            Console.WriteLine(new string('-', 70));

            foreach (DataRow category in categories.Rows)
            {
                string categoryName = category["CategoryName"].ToString();
                string productCount = "N/A";
                string totalValue = "N/A";
                string avgPrice = "N/A";
                string minPrice = "N/A";
                string maxPrice = "N/A";

                // Безопасный доступ к значениям
                if (categories.Columns.Contains("ProductCount") && !category.IsNull("ProductCount"))
                    productCount = category["ProductCount"].ToString();

                if (categories.Columns.Contains("TotalValue") && !category.IsNull("TotalValue"))
                    totalValue = ((decimal)category["TotalValue"]).ToString("C");

                if (categories.Columns.Contains("AvgPrice") && !category.IsNull("AvgPrice"))
                    avgPrice = ((decimal)category["AvgPrice"]).ToString("C");

                if (categories.Columns.Contains("MinPrice") && !category.IsNull("MinPrice"))
                    minPrice = ((decimal)category["MinPrice"]).ToString("C");

                if (categories.Columns.Contains("MaxPrice") && !category.IsNull("MaxPrice"))
                    maxPrice = ((decimal)category["MaxPrice"]).ToString("C");

                Console.WriteLine($"{categoryName,-15} {productCount,-8} {totalValue,-12} {avgPrice,-10} {minPrice,-8} {maxPrice,-8}");
            }

            // 3. Расчетные поля в самой таблице товаров (БЕЗ использования Child)
            Console.WriteLine("\n3. Расчетные поля в таблице товаров:");

            // Стоимость на складе (цена * количество)
            try
            {
                DataColumn stockValue = new DataColumn
                {
                    ColumnName = "StockValue",
                    DataType = typeof(decimal),
                    Expression = "Price * Quantity"
                };
                products.Columns.Add(stockValue);
                Console.WriteLine("✓ Добавлено поле в товары: StockValue = Price * Quantity");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            // Цена с НДС
            try
            {
                DataColumn priceWithTax = new DataColumn
                {
                    ColumnName = "PriceWithTax",
                    DataType = typeof(decimal),
                    Expression = "Price * 1.2"
                };
                products.Columns.Add(priceWithTax);
                Console.WriteLine("✓ Добавлено поле в товары: PriceWithTax = Price * 1.2");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            // 4. Демонстрация работы
            Console.WriteLine("\n4. Демонстрация работы расчетных полей:");

            Console.WriteLine("\nа) Исходные данные категории 'Электроника':");
            DataRow electronics = categories.Rows.Find(1);

            if (electronics != null)
            {
                Console.WriteLine($"  Категория: {electronics["CategoryName"]}");

                if (categories.Columns.Contains("ProductCount"))
                    Console.WriteLine($"  Товаров: {electronics["ProductCount"]}");

                if (categories.Columns.Contains("TotalValue"))
                    Console.WriteLine($"  Суммарная стоимость: {((decimal)electronics["TotalValue"]):C}");

                if (categories.Columns.Contains("AvgPrice"))
                    Console.WriteLine($"  Средняя цена: {((decimal)electronics["AvgPrice"]):C}");
            }

            Console.WriteLine("\nб) Ручной расчет для проверки:");
            DataRow[] electronicProducts = electronics.GetChildRows(relation);
            int manualCount = electronicProducts.Length;
            decimal manualTotal = 0;
            List<decimal> prices = new List<decimal>();

            foreach (DataRow product in electronicProducts)
            {
                decimal price = (decimal)product["Price"];
                int quantity = (int)product["Quantity"];
                manualTotal += price * quantity;
                prices.Add(price);
            }

            decimal manualAvg = prices.Count > 0 ? prices.Average() : 0;
            decimal manualMin = prices.Count > 0 ? prices.Min() : 0;
            decimal manualMax = prices.Count > 0 ? prices.Max() : 0;

            Console.WriteLine($"  Ручной расчет:");
            Console.WriteLine($"    Товаров: {manualCount}");
            Console.WriteLine($"    Суммарная стоимость: {manualTotal:C}");
            Console.WriteLine($"    Средняя цена: {manualAvg:C}");
            Console.WriteLine($"    Мин. цена: {manualMin:C}");
            Console.WriteLine($"    Макс. цена: {manualMax:C}");

            // 5. Показываем товары с расчетными полями
            Console.WriteLine("\n5. Товары с расчетными полями:");

            if (products.Columns.Contains("StockValue") && products.Columns.Contains("PriceWithTax"))
            {
                Console.WriteLine($"{"Товар",-25} {"Цена",-10} {"Кол-во",-8} {"На складе",-12} {"Цена с НДС",-12}");
                Console.WriteLine(new string('-', 70));

                foreach (DataRow product in electronicProducts)
                {
                    Console.WriteLine($"{product["ProductName"],-25} " +
                        $"{((decimal)product["Price"]):C,-10} " +
                        $"{product["Quantity"],-8} " +
                        $"{((decimal)product["StockValue"]):C,-12} " +
                        $"{((decimal)product["PriceWithTax"]):C,-12}");
                }
            }

            // 6. Сложные выражения (продвинутые возможности)
            Console.WriteLine("\n6. Сложные выражения:");

            // Добавляем поле скидки
            try
            {
                DataColumn discountPrice = new DataColumn
                {
                    ColumnName = "DiscountPrice",
                    DataType = typeof(decimal),
                    Expression = "IIF(Price > 10000, Price * 0.9, Price)"
                };
                products.Columns.Add(discountPrice);
                Console.WriteLine("✓ Добавлено поле: DiscountPrice = IIF(Price > 10000, Price * 0.9, Price)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            // Добавляем категорию цены
            try
            {
                DataColumn priceCategory = new DataColumn
                {
                    ColumnName = "PriceCategory",
                    DataType = typeof(string),
                    Expression = "IIF(Price < 5000, 'Бюджетный', IIF(Price < 20000, 'Средний', 'Премиум'))"
                };
                products.Columns.Add(priceCategory);
                Console.WriteLine("✓ Добавлено поле: PriceCategory = IIF(Price < 5000, 'Бюджетный', IIF(Price < 20000, 'Средний', 'Премиум'))");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            // Показываем товары с новыми полями
            Console.WriteLine("\nТовары с новыми полями:");

            if (products.Columns.Contains("DiscountPrice") && products.Columns.Contains("PriceCategory"))
            {
                Console.WriteLine($"{"Товар",-25} {"Цена",-10} {"Со скидкой",-12} {"Категория",-10}");
                Console.WriteLine(new string('-', 60));

                foreach (DataRow product in electronicProducts)
                {
                    Console.WriteLine($"{product["ProductName"],-25} " +
                        $"{((decimal)product["Price"]):C,-10} " +
                        $"{((decimal)product["DiscountPrice"]):C,-12} " +
                        $"{product["PriceCategory"],-10}");
                }
            }

            // 7. Демонстрация изменения данных
            Console.WriteLine("\n7. Демонстрация изменения данных:");

            // Изменяем цену товара
            DataRow productToUpdate = products.Rows.Find(101); // Ноутбук
            if (productToUpdate != null)
            {
                decimal oldPrice = (decimal)productToUpdate["Price"];
                productToUpdate["Price"] = 80000;

                Console.WriteLine($"Изменена цена ноутбука с {oldPrice:C} на {((decimal)productToUpdate["Price"]):C}");

                // Обновляем данные
                products.AcceptChanges();
                categories.AcceptChanges();

                if (electronics != null && categories.Columns.Contains("AvgPrice"))
                {
                    Console.WriteLine($"Новая средняя цена в категории: {((decimal)electronics["AvgPrice"]):C}");
                }
            }

            // 8. Очистка
            Console.WriteLine("\n8. Очистка данных:");

            // Удаляем расчетные поля
            string[] columnsToRemove = {
        "ProductCount", "TotalValue", "AvgPrice", "MinPrice", "MaxPrice",
        "StockValue", "PriceWithTax", "DiscountPrice", "PriceCategory"
    };

            foreach (string colName in columnsToRemove)
            {
                if (categories.Columns.Contains(colName))
                    categories.Columns.Remove(colName);

                if (products.Columns.Contains(colName))
                    products.Columns.Remove(colName);
            }

            // Восстанавливаем исходную цену
            if (productToUpdate != null)
            {
                productToUpdate["Price"] = 75000;
                products.AcceptChanges();
                categories.AcceptChanges();
            }

            Console.WriteLine("✓ Все расчетные поля удалены");
            Console.WriteLine("✓ Исходные данные восстановлены");
        }
        #endregion

        #region Задание 9: DeleteRule
        static void Task9_DeleteRule()
        {
            Console.WriteLine("ЗАДАНИЕ 9: Использование DeleteRule для определения поведения при удалении родительской записи\n");
            Console.WriteLine("Цель: Демонстрация различных DeleteRule\n");

            Console.WriteLine("1. DeleteRule.Cascade:");
            TestDeleteRuleCascade();

            Console.WriteLine("\n2. DeleteRule.SetNull:");
            TestDeleteRuleSetNull();

            Console.WriteLine("\n3. DeleteRule.None:");
            TestDeleteRuleNone();
        }

        static void TestDeleteRuleCascade()
        {
            try
            {
                DataSet ds = new DataSet("DepartmentCascade");

                // Таблица Отделы
                DataTable departments = new DataTable("Отделы");
                departments.Columns.Add("DepartmentID", typeof(int));
                departments.Columns.Add("DepartmentName", typeof(string));
                departments.PrimaryKey = new DataColumn[] { departments.Columns["DepartmentID"] };

                // Таблица Сотрудники
                DataTable employees = new DataTable("Сотрудники");
                employees.Columns.Add("EmployeeID", typeof(int));
                employees.Columns.Add("EmployeeName", typeof(string));
                employees.Columns.Add("DepartmentID", typeof(int));
                employees.Columns.Add("Salary", typeof(decimal));
                employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

                ds.Tables.Add(departments);
                ds.Tables.Add(employees);

                // Заполняем данными
                departments.Rows.Add(1, "IT");
                departments.Rows.Add(2, "HR");
                departments.Rows.Add(3, "Finance");

                employees.Rows.Add(101, "Иван Иванов", 1, 50000);
                employees.Rows.Add(102, "Петр Петров", 1, 55000);
                employees.Rows.Add(103, "Мария Сидорова", 2, 45000);
                employees.Rows.Add(104, "Анна Смирнова", 3, 60000);

                Console.WriteLine("До удаления:");
                PrintDepartmentsAndEmployees(ds);

                // Создаем ForeignKeyConstraint с правилом Cascade
                ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
                    "FK_Employees_Departments",
                    departments.Columns["DepartmentID"],
                    employees.Columns["DepartmentID"])
                {
                    DeleteRule = Rule.Cascade,
                    UpdateRule = Rule.Cascade
                };

                // Добавляем constraint в таблицу employees
                employees.Constraints.Add(fkConstraint);

                // Создаем DataRelation для навигации
                DataRelation relation = new DataRelation(
                    "FK_Employees_Departments_Rel",
                    departments.Columns["DepartmentID"],
                    employees.Columns["DepartmentID"],
                    false); // false - не создавать constraint (мы уже создали его выше)

                ds.Relations.Add(relation);

                // Удаляем отдел
                DataRow deptToDelete = departments.Rows.Find(1);
                Console.WriteLine($"\nУдаляем отдел: {deptToDelete["DepartmentName"]}");
                deptToDelete.Delete();

                Console.WriteLine("\nПосле удаления отдела IT:");
                PrintDepartmentsAndEmployees(ds);

                Console.WriteLine($"\nОжидание: все сотрудники отдела IT (ID 101 и 102) должны быть удалены каскадно.");
                Console.WriteLine($"Результат: сотрудники с ID 101 и 102 {((employees.Rows.Find(101)?.RowState == DataRowState.Deleted || employees.Rows.Find(101) == null) && (employees.Rows.Find(102)?.RowState == DataRowState.Deleted || employees.Rows.Find(102) == null) ? "удалены" : "не удалены")}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void TestDeleteRuleSetNull()
        {
            try
            {
                DataSet ds = new DataSet("DepartmentSetNull");

                DataTable departments = new DataTable("Отделы");
                departments.Columns.Add("DepartmentID", typeof(int));
                departments.Columns.Add("DepartmentName", typeof(string));
                departments.PrimaryKey = new DataColumn[] { departments.Columns["DepartmentID"] };

                DataTable employees = new DataTable("Сотрудники");
                employees.Columns.Add("EmployeeID", typeof(int));
                employees.Columns.Add("EmployeeName", typeof(string));
                employees.Columns.Add("DepartmentID", typeof(int));
                employees.Columns.Add("Salary", typeof(decimal));
                employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

                ds.Tables.Add(departments);
                ds.Tables.Add(employees);

                departments.Rows.Add(1, "IT");
                departments.Rows.Add(2, "HR");

                employees.Rows.Add(101, "Иван Иванов", 1, 50000);
                employees.Rows.Add(102, "Петр Петров", 1, 55000);

                Console.WriteLine("До удаления:");
                PrintDepartmentsAndEmployees(ds);

                // Создаем ForeignKeyConstraint с правилом SetNull
                ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
                    "FK_Employees_Departments",
                    departments.Columns["DepartmentID"],
                    employees.Columns["DepartmentID"])
                {
                    DeleteRule = Rule.SetNull,
                    UpdateRule = Rule.Cascade
                };

                // Добавляем constraint в таблицу employees
                employees.Constraints.Add(fkConstraint);

                // Создаем DataRelation для навигации
                DataRelation relation = new DataRelation(
                    "FK_Employees_Departments_Rel",
                    departments.Columns["DepartmentID"],
                    employees.Columns["DepartmentID"],
                    false);

                ds.Relations.Add(relation);

                DataRow deptToDelete = departments.Rows.Find(1);
                Console.WriteLine($"\nУдаляем отдел: {deptToDelete["DepartmentName"]}");
                deptToDelete.Delete();

                Console.WriteLine("\nПосле удаления отдела IT:");
                PrintDepartmentsAndEmployees(ds);

                Console.WriteLine($"\nОжидание: у сотрудников отдела IT поле DepartmentID должно быть установлено в NULL.");

                // Проверяем результат
                DataRow emp101 = employees.Rows.Find(101);
                DataRow emp102 = employees.Rows.Find(102);

                bool isNull101 = emp101 != null && emp101["DepartmentID"] == DBNull.Value;
                bool isNull102 = emp102 != null && emp102["DepartmentID"] == DBNull.Value;

                Console.WriteLine($"Результат: сотрудник 101 - DepartmentID {(isNull101 ? "NULL" : "не NULL")}, сотрудник 102 - DepartmentID {(isNull102 ? "NULL" : "не NULL")}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void TestDeleteRuleNone()
        {
            try
            {
                DataSet ds = new DataSet("DepartmentNone");

                DataTable departments = new DataTable("Отделы");
                departments.Columns.Add("DepartmentID", typeof(int));
                departments.Columns.Add("DepartmentName", typeof(string));
                departments.PrimaryKey = new DataColumn[] { departments.Columns["DepartmentID"] };

                DataTable employees = new DataTable("Сотрудники");
                employees.Columns.Add("EmployeeID", typeof(int));
                employees.Columns.Add("EmployeeName", typeof(string));
                employees.Columns.Add("DepartmentID", typeof(int));
                employees.Columns.Add("Salary", typeof(decimal));
                employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

                ds.Tables.Add(departments);
                ds.Tables.Add(employees);

                departments.Rows.Add(1, "IT");
                departments.Rows.Add(2, "HR");

                employees.Rows.Add(101, "Иван Иванов", 1, 50000);
                employees.Rows.Add(102, "Петр Петров", 1, 55000);

                Console.WriteLine("До удаления:");
                PrintDepartmentsAndEmployees(ds);

                // Создаем ForeignKeyConstraint с правилом None
                ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
                    "FK_Employees_Departments",
                    departments.Columns["DepartmentID"],
                    employees.Columns["DepartmentID"])
                {
                    DeleteRule = Rule.None,
                    UpdateRule = Rule.None
                };

                // Добавляем constraint в таблицу employees
                employees.Constraints.Add(fkConstraint);

                // Создаем DataRelation для навигации
                DataRelation relation = new DataRelation(
                    "FK_Employees_Departments_Rel",
                    departments.Columns["DepartmentID"],
                    employees.Columns["DepartmentID"],
                    false);

                ds.Relations.Add(relation);

                try
                {
                    DataRow deptToDelete = departments.Rows.Find(1);
                    Console.WriteLine($"\nПытаемся удалить отдел: {deptToDelete["DepartmentName"]}");
                    deptToDelete.Delete();

                    // Если дошли сюда, значит исключение не было брошено
                    Console.WriteLine("\nПосле удаления отдела IT:");
                    PrintDepartmentsAndEmployees(ds);
                    Console.WriteLine("✗ Ожидалось исключение, но его не было");
                }
                catch (InvalidConstraintException ex)
                {
                    Console.WriteLine($"✓ Правильно обработана ошибка при попытке удалить родительскую запись с зависимыми данными.");
                    Console.WriteLine($"  Сообщение ошибки: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Неожиданная ошибка: {ex.GetType().Name}: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при настройке: {ex.Message}");
            }
        }

        static void PrintDepartmentsAndEmployees(DataSet ds)
        {
            DataTable departments = ds.Tables["Отделы"];
            DataTable employees = ds.Tables["Сотрудники"];

            Console.WriteLine("\nОтделы:");
            foreach (DataRow dept in departments.Rows)
            {
                if (dept.RowState != DataRowState.Deleted)
                    Console.WriteLine($"  [{dept["DepartmentID"]}] {dept["DepartmentName"]}");
                else if (dept.HasVersion(DataRowVersion.Original))
                    Console.WriteLine($"  [{dept["DepartmentID", DataRowVersion.Original]}] {dept["DepartmentName", DataRowVersion.Original]} [УДАЛЕН]");
            }

            Console.WriteLine("\nСотрудники:");
            foreach (DataRow emp in employees.Rows)
            {
                if (emp.RowState != DataRowState.Deleted)
                {
                    string deptId = emp["DepartmentID"] == DBNull.Value ? "NULL" : emp["DepartmentID"].ToString();
                    Console.WriteLine($"  [{emp["EmployeeID"]}] {emp["EmployeeName"]} (Отдел: {deptId}, Зарплата: {emp["Salary"]:C})");
                }
                else if (emp.HasVersion(DataRowVersion.Original))
                {
                    string deptId = emp["DepartmentID", DataRowVersion.Original] == DBNull.Value ? "NULL" : emp["DepartmentID", DataRowVersion.Original].ToString();
                    Console.WriteLine($"  [{emp["EmployeeID", DataRowVersion.Original]}] {emp["EmployeeName", DataRowVersion.Original]} (Отдел был: {deptId}) [УДАЛЕН]");
                }
            }
        }
        #endregion

        #region Задание 10: UpdateRule
        static void Task10_UpdateRule()
        {
            Console.WriteLine("ЗАДАНИЕ 10: Использование UpdateRule для определения поведения при изменении первичного ключа\n");
            Console.WriteLine("Цель: Демонстрация различных UpdateRule\n");

            Console.WriteLine("1. UpdateRule.Cascade:");
            TestUpdateRule(Rule.Cascade, "Cascade");

            Console.WriteLine("\n2. UpdateRule.SetNull:");
            TestUpdateRule(Rule.SetNull, "SetNull");

            Console.WriteLine("\n3. UpdateRule.None:");
            TestUpdateRule(Rule.None, "None");
        }

        static void TestUpdateRule(Rule rule, string ruleName)
        {
            Console.WriteLine($"\n=== Тестирование UpdateRule.{ruleName} ===");

            try
            {
                DataSet ds = new DataSet($"Update{ruleName}");

                DataTable departments = new DataTable("Отделы");
                departments.Columns.Add("DepartmentID", typeof(int));
                departments.Columns.Add("DepartmentName", typeof(string));
                departments.PrimaryKey = new DataColumn[] { departments.Columns["DepartmentID"] };

                DataTable employees = new DataTable("Сотрудники");
                employees.Columns.Add("EmployeeID", typeof(int));
                employees.Columns.Add("EmployeeName", typeof(string));
                employees.Columns.Add("DepartmentID", typeof(int));
                employees.Columns.Add("Salary", typeof(decimal));
                employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

                ds.Tables.Add(departments);
                ds.Tables.Add(employees);

                departments.Rows.Add(1, "IT");
                departments.Rows.Add(2, "HR");

                employees.Rows.Add(101, "Иван Иванов", 1, 50000);
                employees.Rows.Add(102, "Петр Петров", 1, 55000);

                Console.WriteLine("\nИсходные данные:");
                Console.WriteLine("Отделы:");
                foreach (DataRow dept in departments.Rows)
                    Console.WriteLine($"  [{dept["DepartmentID"]}] {dept["DepartmentName"]}");

                Console.WriteLine("\nСотрудники:");
                foreach (DataRow emp in employees.Rows)
                    Console.WriteLine($"  [{emp["EmployeeID"]}] {emp["EmployeeName"]} (Отдел: {emp["DepartmentID"]})");

                // Создаем ForeignKeyConstraint с указанным правилом
                ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
                    "FK_Employees_Departments_Update",
                    departments.Columns["DepartmentID"],
                    employees.Columns["DepartmentID"])
                {
                    DeleteRule = Rule.Cascade,  // Для Delete используем Cascade по умолчанию
                    UpdateRule = rule
                };

                // Добавляем constraint в таблицу employees
                employees.Constraints.Add(fkConstraint);

                Console.WriteLine($"\nУстанавливаем UpdateRule = {ruleName}");

                // Изменяем ID отдела
                DataRow deptToUpdate = departments.Rows.Find(1);
                Console.WriteLine($"\nИзменяем ID отдела с 1 на 100: {deptToUpdate["DepartmentName"]}");

                try
                {
                    deptToUpdate["DepartmentID"] = 100;

                    Console.WriteLine("\nРезультат после изменения:");
                    Console.WriteLine("Отделы:");
                    foreach (DataRow dept in departments.Rows)
                        Console.WriteLine($"  [{dept["DepartmentID"]}] {dept["DepartmentName"]}");

                    Console.WriteLine("\nСотрудники:");
                    foreach (DataRow emp in employees.Rows)
                    {
                        string deptInfo = emp["DepartmentID"] == DBNull.Value ? "NULL" : emp["DepartmentID"].ToString();
                        Console.WriteLine($"  [{emp["EmployeeID"]}] {emp["EmployeeName"]} (Отдел: {deptInfo})");
                    }

                    // Анализируем результат
                    Console.WriteLine($"\nАнализ результата для UpdateRule.{ruleName}:");

                    switch (rule)
                    {
                        case Rule.Cascade:
                            // Проверяем, что DepartmentID у сотрудников изменился
                            DataRow emp101 = employees.Rows.Find(101);
                            DataRow emp102 = employees.Rows.Find(102);
                            bool updated101 = emp101 != null && (int)emp101["DepartmentID"] == 100;
                            bool updated102 = emp102 != null && (int)emp102["DepartmentID"] == 100;
                            Console.WriteLine($"  Ожидание: у сотрудников отдела IT DepartmentID должен измениться с 1 на 100");
                            Console.WriteLine($"  Результат: сотрудник 101 - DepartmentID = {emp101?["DepartmentID"]} {(updated101 ? "✓" : "✗")}, сотрудник 102 - DepartmentID = {emp102?["DepartmentID"]} {(updated102 ? "✓" : "✗")}");
                            break;

                        case Rule.SetNull:
                            // Проверяем, что DepartmentID установлен в NULL
                            emp101 = employees.Rows.Find(101);
                            emp102 = employees.Rows.Find(102);
                            bool isNull101 = emp101 != null && emp101["DepartmentID"] == DBNull.Value;
                            bool isNull102 = emp102 != null && emp102["DepartmentID"] == DBNull.Value;
                            Console.WriteLine($"  Ожидание: у сотрудников отдела IT DepartmentID должен быть NULL");
                            Console.WriteLine($"  Результат: сотрудник 101 - DepartmentID {(isNull101 ? "NULL ✓" : "не NULL ✗")}, сотрудник 102 - DepartmentID {(isNull102 ? "NULL ✓" : "не NULL ✗")}");
                            break;

                        case Rule.None:
                            Console.WriteLine($"  Ожидание: должно было возникнуть исключение при изменении ключа отдела с сотрудниками");
                            Console.WriteLine($"  Результат: изменение прошло без ошибок (это не должно было произойти)");
                            break;
                    }
                }
                catch (InvalidConstraintException ex)
                {
                    Console.WriteLine($"\n✓ Правильно обработана ошибка при попытке изменить родительский ключ с зависимыми данными.");
                    Console.WriteLine($"  Сообщение ошибки: {ex.Message}");

                    if (rule == Rule.None)
                        Console.WriteLine($"  Ожидание для Rule.None: ✓ ошибка обработана правильно");
                    else
                        Console.WriteLine($"  Неожиданная ошибка для Rule.{ruleName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n✗ Неожиданная ошибка: {ex.GetType().Name}: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при настройке: {ex.Message}");
            }
        }
        #endregion

        #region Задание 11: Комбинированные правила
        static void Task11_CombinedRules()
        {
            Console.WriteLine("ЗАДАНИЕ 11: Комбинирование DeleteRule и UpdateRule в одном приложении\n");
            Console.WriteLine("Цель: Создание системы управления заказами с каскадными операциями\n");

            DataSet ds = CreateOrderManagementSystem();

            if (ds == null)
            {
                Console.WriteLine("Ошибка при создании системы управления заказами");
                return;
            }

            DataTable customers = ds.Tables["Заказчики"];
            DataTable orders = ds.Tables["Заказы"];
            DataTable orderDetails = ds.Tables["ОрдеротовыеКетали"];

            Console.WriteLine("1. Исходные данные:");
            PrintOrderHierarchy(ds);

            // 2. Добавление нового заказчика
            Console.WriteLine("\n\n2. Добавление нового заказчика:");
            DataRow newCustomer = customers.NewRow();
            newCustomer["CustomerID"] = 103;
            newCustomer["CustomerName"] = "Новая компания";
            newCustomer["Email"] = "new@example.com";
            customers.Rows.Add(newCustomer);

            DataRow newOrder = orders.NewRow();
            newOrder["OrderID"] = 1004;
            newOrder["OrderDate"] = DateTime.Now;
            newOrder["CustomerID"] = 103;
            newOrder["Total"] = 15000;
            orders.Rows.Add(newOrder);

            DataRow newDetail = orderDetails.NewRow();
            newDetail["DetailID"] = 7;
            newDetail["OrderID"] = 1004;
            newDetail["ProductID"] = 101;
            newDetail["Quantity"] = 2;
            newDetail["Price"] = 7500;
            orderDetails.Rows.Add(newDetail);

            Console.WriteLine($"Добавлен заказчик: {newCustomer["CustomerName"]}");
            Console.WriteLine($"Добавлен заказ: #{newOrder["OrderID"]} на сумму {newOrder["Total"]:C}");
            Console.WriteLine($"Добавлена деталь заказа: товар {newDetail["ProductID"]}, количество {newDetail["Quantity"]}");

            // 3. Удаление заказчика
            Console.WriteLine("\n\n3. Удаление заказчика (каскадное удаление):");
            DataRow customerToDelete = customers.Rows.Find(101);
            if (customerToDelete != null)
            {
                Console.WriteLine($"Удаляем заказчика: {customerToDelete["CustomerName"]}");

                int ordersBefore = orders.Rows.Count;
                int detailsBefore = orderDetails.Rows.Count;

                customerToDelete.Delete();

                Console.WriteLine($"Заказов до удаления: {ordersBefore}, после: {orders.Rows.Count}");
                Console.WriteLine($"Деталей заказа до удаления: {detailsBefore}, после: {orderDetails.Rows.Count}");
                Console.WriteLine($"Удалено заказов: {ordersBefore - orders.Rows.Count}");
                Console.WriteLine($"Удалено деталей заказа: {detailsBefore - orderDetails.Rows.Count}");
            }
            else
            {
                Console.WriteLine("Заказчик с ID 101 не найден");
            }

            // 4. Изменение ID заказа
            Console.WriteLine("\n\n4. Изменение ID заказа (каскадное обновление):");
            DataRow orderToUpdate = orders.Rows.Find(1002);
            if (orderToUpdate != null)
            {
                Console.WriteLine($"Меняем ID заказа с 1002 на 2002");

                orderToUpdate["OrderID"] = 2002;

                // Проверяем обновление в деталях заказа
                DataRow[] details = orderToUpdate.GetChildRows("FK_OrderDetails_Orders");
                Console.WriteLine($"Обновлено деталей заказа: {details.Length}");

                foreach (DataRow detail in details)
                {
                    Console.WriteLine($"  Деталь #{detail["DetailID"]} теперь ссылается на заказ #{detail["OrderID"]}");
                }
            }
            else
            {
                Console.WriteLine("Заказ с ID 1002 не найден");
            }

            // 5. Отчет об изменениях
            Console.WriteLine("\n\n5. Отчет об изменениях:");
            Console.WriteLine($"Всего заказчиков: {customers.Rows.Count}");
            Console.WriteLine($"Всего заказов: {orders.Rows.Count}");
            Console.WriteLine($"Всего деталей заказа: {orderDetails.Rows.Count}");

            int deletedRows = 0;
            foreach (DataRow row in customers.Rows)
                if (row.RowState == DataRowState.Deleted) deletedRows++;
            foreach (DataRow row in orders.Rows)
                if (row.RowState == DataRowState.Deleted) deletedRows++;
            foreach (DataRow row in orderDetails.Rows)
                if (row.RowState == DataRowState.Deleted) deletedRows++;

            Console.WriteLine($"Всего удалено записей: {deletedRows}");

            // Показываем текущее состояние
            Console.WriteLine("\n\nТекущее состояние системы:");
            PrintOrderHierarchy(ds);
        }

        static DataSet CreateOrderManagementSystem()
        {
            try
            {
                DataSet ds = new DataSet("OrderSystem");

                // Заказчики
                DataTable customers = new DataTable("Заказчики");
                customers.Columns.Add("CustomerID", typeof(int));
                customers.Columns.Add("CustomerName", typeof(string));
                customers.Columns.Add("Email", typeof(string));
                customers.PrimaryKey = new DataColumn[] { customers.Columns["CustomerID"] };

                // Заказы
                DataTable orders = new DataTable("Заказы");
                orders.Columns.Add("OrderID", typeof(int));
                orders.Columns.Add("OrderDate", typeof(DateTime));
                orders.Columns.Add("CustomerID", typeof(int));
                orders.Columns.Add("Total", typeof(decimal));
                orders.PrimaryKey = new DataColumn[] { orders.Columns["OrderID"] };

                // Детали заказов
                DataTable orderDetails = new DataTable("ОрдеротовыеКетали");
                orderDetails.Columns.Add("DetailID", typeof(int));
                orderDetails.Columns.Add("OrderID", typeof(int));
                orderDetails.Columns.Add("ProductID", typeof(int));
                orderDetails.Columns.Add("Quantity", typeof(int));
                orderDetails.Columns.Add("Price", typeof(decimal));
                orderDetails.PrimaryKey = new DataColumn[] { orderDetails.Columns["DetailID"] };

                ds.Tables.Add(customers);
                ds.Tables.Add(orders);
                ds.Tables.Add(orderDetails);

                // Данные
                customers.Rows.Add(101, "ООО Рога и копыта", "info@roga.ru");
                customers.Rows.Add(102, "ИП Сидоров", "sidorov@mail.ru");

                orders.Rows.Add(1001, new DateTime(2024, 1, 15), 101, 120000);
                orders.Rows.Add(1002, new DateTime(2024, 1, 20), 101, 85000);
                orders.Rows.Add(1003, new DateTime(2024, 2, 5), 102, 45000);

                orderDetails.Rows.Add(1, 1001, 101, 2, 75000);
                orderDetails.Rows.Add(2, 1001, 102, 3, 15000);
                orderDetails.Rows.Add(3, 1002, 103, 5, 17000);
                orderDetails.Rows.Add(4, 1003, 101, 1, 75000);
                orderDetails.Rows.Add(5, 1003, 104, 10, 1500);
                orderDetails.Rows.Add(6, 1003, 105, 2, 1800);

                // Создаем ForeignKeyConstraints для каскадных операций

                // Отношение 1: Заказчики → Заказы
                ForeignKeyConstraint fkCustomersOrders = new ForeignKeyConstraint(
                    "FK_Orders_Customers",
                    customers.Columns["CustomerID"],
                    orders.Columns["CustomerID"])
                {
                    DeleteRule = Rule.Cascade,    // При удалении заказчика удаляются его заказы
                    UpdateRule = Rule.Cascade     // При изменении ID заказчика обновляются заказы
                };
                orders.Constraints.Add(fkCustomersOrders);

                // Отношение 2: Заказы → Детали заказов
                ForeignKeyConstraint fkOrdersDetails = new ForeignKeyConstraint(
                    "FK_OrderDetails_Orders",
                    orders.Columns["OrderID"],
                    orderDetails.Columns["OrderID"])
                {
                    DeleteRule = Rule.Cascade,    // При удалении заказа удаляются его детали
                    UpdateRule = Rule.Cascade     // При изменении ID заказа обновляются детали
                };
                orderDetails.Constraints.Add(fkOrdersDetails);

                // Создаем DataRelations для навигации (без создания constraints, так как мы их уже создали)
                DataRelation custOrderRel = new DataRelation(
                    "FK_Orders_Customers_Rel",
                    customers.Columns["CustomerID"],
                    orders.Columns["CustomerID"],
                    false); // false - не создавать constraints

                DataRelation orderDetailsRel = new DataRelation(
                    "FK_OrderDetails_Orders_Rel",
                    orders.Columns["OrderID"],
                    orderDetails.Columns["OrderID"],
                    false); // false - не создавать constraints

                ds.Relations.Add(custOrderRel);
                ds.Relations.Add(orderDetailsRel);

                Console.WriteLine("✓ Система управления заказами создана");
                Console.WriteLine($"  Заказчиков: {customers.Rows.Count}");
                Console.WriteLine($"  Заказов: {orders.Rows.Count}");
                Console.WriteLine($"  Деталей заказа: {orderDetails.Rows.Count}");
                Console.WriteLine($"  Отношений: {ds.Relations.Count}");
                Console.WriteLine($"  Constraints: {orders.Constraints.Count + orderDetails.Constraints.Count}");

                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка при создании системы: {ex.Message}");
                return null;
            }
        }

        static void PrintOrderHierarchy(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                Console.WriteLine("Нет данных для отображения");
                return;
            }

            DataTable customers = ds.Tables["Заказчики"];
            DataTable orders = ds.Tables["Заказы"];
            DataTable orderDetails = ds.Tables["ОрдеротовыеКетали"];

            // Используем отношения для навигации
            DataRelation custOrderRel = ds.Relations["FK_Orders_Customers_Rel"];
            DataRelation orderDetailsRel = ds.Relations["FK_OrderDetails_Orders_Rel"];

            Console.WriteLine("\nИерархия заказов:");
            Console.WriteLine(new string('=', 60));

            foreach (DataRow customer in customers.Rows)
            {
                if (customer.RowState != DataRowState.Deleted)
                {
                    Console.WriteLine($"\nЗаказчик: {customer["CustomerName"]} ({customer["Email"]})");

                    DataRow[] customerOrders = customer.GetChildRows(custOrderRel);
                    if (customerOrders.Length == 0)
                    {
                        Console.WriteLine("  Нет заказов");
                    }
                    else
                    {
                        foreach (DataRow order in customerOrders)
                        {
                            if (order.RowState != DataRowState.Deleted)
                            {
                                Console.WriteLine($"  Заказ #{order["OrderID"]} от {((DateTime)order["OrderDate"]):dd.MM.yyyy} " +
                                    $"на сумму {order["Total"]:C}");

                                DataRow[] details = order.GetChildRows(orderDetailsRel);
                                if (details.Length == 0)
                                {
                                    Console.WriteLine("    Нет деталей заказа");
                                }
                                else
                                {
                                    Console.WriteLine("    Детали заказа:");
                                    foreach (DataRow detail in details)
                                    {
                                        if (detail.RowState != DataRowState.Deleted)
                                        {
                                            Console.WriteLine($"      • Товар {detail["ProductID"]}: " +
                                                $"{detail["Quantity"]} × {detail["Price"]:C} = " +
                                                $"{(int)detail["Quantity"] * (decimal)detail["Price"]:C}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static void PrintCustomerOrders(DataRow customer, DataSet ds)
        {
            if (customer == null || customer.RowState == DataRowState.Deleted || ds == null)
                return;

            DataRelation custOrderRel = ds.Relations["FK_Orders_Customers_Rel"];
            DataRelation orderDetailsRel = ds.Relations["FK_OrderDetails_Orders_Rel"];

            Console.WriteLine($"\nЗаказчик: {customer["CustomerName"]}");
            DataRow[] orders = customer.GetChildRows(custOrderRel);

            if (orders.Length == 0)
            {
                Console.WriteLine("  Нет заказов");
            }
            else
            {
                foreach (DataRow order in orders)
                {
                    Console.WriteLine($"  Заказ #{order["OrderID"]} на сумму {order["Total"]:C}");
                }
            }
        }
        #endregion

        #region Задание 12: RowState Deleted
        static void Task12_RowStateDeleted()
        {
            Console.WriteLine("ЗАДАНИЕ 12: Использование RowState для получения информации о удаляемых строках\n");
            Console.WriteLine("Цель: Отслеживание удаляемых записей перед сохранением\n");

            DataSet ds = CreateStoreDataSet();
            DataTable products = ds.Tables["Товары"];
            DataRelation relation = ds.Relations["FK_Products_Categories"];

            Console.WriteLine("1. Исходное состояние:");
            PrintAllProducts(ds);

            // 2. Выполняем операции
            Console.WriteLine("\n\n2. Выполнение операций:");

            // Добавляем 2 новых товара
            Console.WriteLine("  Добавляем 2 новых товара:");
            DataRow newProduct1 = products.NewRow();
            newProduct1["ProductID"] = 201;
            newProduct1["ProductName"] = "Новый товар 1";
            newProduct1["Price"] = 5000;
            newProduct1["Quantity"] = 10;
            newProduct1["CategoryID"] = 1;
            products.Rows.Add(newProduct1);

            DataRow newProduct2 = products.NewRow();
            newProduct2["ProductID"] = 202;
            newProduct2["ProductName"] = "Новый товар 2";
            newProduct2["Price"] = 7000;
            newProduct2["Quantity"] = 8;
            newProduct2["CategoryID"] = 2;
            products.Rows.Add(newProduct2);

            // Модифицируем 2 товара
            Console.WriteLine("  Модифицируем 2 товара:");
            DataRow productToModify1 = products.Rows.Find(101);
            productToModify1["Price"] = 80000;

            DataRow productToModify2 = products.Rows.Find(102);
            productToModify2["Price"] = 50000;

            // Помечаем 3 товара на удаление
            Console.WriteLine("  Помечаем 3 товара на удаление:");
            DataRow productToDelete1 = products.Rows.Find(103);
            productToDelete1.Delete();

            DataRow productToDelete2 = products.Rows.Find(104);
            productToDelete2.Delete();

            DataRow productToDelete3 = products.Rows.Find(105);
            productToDelete3.Delete();

            // 3. Анализ RowState
            Console.WriteLine("\n\n3. Анализ RowState:");
            Console.WriteLine($"Всего строк: {products.Rows.Count}");

            var byState = products.Rows.Cast<DataRow>()
                .GroupBy(r => r.RowState)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var state in byState)
            {
                Console.WriteLine($"  {state.Key}: {state.Value.Count} строк");
            }

            // 4. Получение удаляемых строк
            Console.WriteLine("\n\n4. Получение удаляемых строк:");
            DataRow[] deletedRows = products.Select(null, null, DataViewRowState.Deleted);
            Console.WriteLine($"Найдено удаляемых строк: {deletedRows.Length}");

            // 5. Информация об удаляемых товарах
            Console.WriteLine("\n\n5. Информация об удаляемых товарах:");
            Console.WriteLine($"{"Товар",-25} {"Категория",-15} {"Причина"}");
            Console.WriteLine(new string('-', 60));

            foreach (DataRow deletedRow in deletedRows)
            {
                string productName = deletedRow.HasVersion(DataRowVersion.Original) ?
                    deletedRow["ProductName", DataRowVersion.Original].ToString() : "?";

                string categoryName = "Неизвестно";
                if (deletedRow.HasVersion(DataRowVersion.Original))
                {
                    DataRow[] parents = deletedRow.GetParentRows(relation);
                    if (parents.Length > 0)
                        categoryName = parents[0]["CategoryName"].ToString();
                }

                Console.WriteLine($"{productName,-25} {categoryName,-15} Удаление");
            }

            // 6. Отчет перед удалением
            Console.WriteLine("\n\n6. Отчет перед удалением:");
            Console.WriteLine("Что будет удалено:");

            foreach (DataRow deletedRow in deletedRows)
            {
                if (deletedRow.HasVersion(DataRowVersion.Original))
                {
                    int categoryID = (int)deletedRow["CategoryID", DataRowVersion.Original];
                    DataTable categories = ds.Tables["Категории"];
                    DataRow category = categories.Rows.Find(categoryID);

                    if (category != null)
                    {
                        // Считаем сколько товаров останется в категории
                        DataRow[] remainingProducts = category.GetChildRows(relation);
                        int remainingCount = remainingProducts.Count(r => r.RowState != DataRowState.Deleted);

                        Console.WriteLine($"  • Товар: {deletedRow["ProductName", DataRowVersion.Original]}");
                        Console.WriteLine($"    Категория: {category["CategoryName"]}");
                        Console.WriteLine($"    Останется товаров в категории: {remainingCount}");
                    }
                }
            }

            // 7. Отмена удаления для конкретных строк
            Console.WriteLine("\n\n7. Отмена удаления для товара 'Наушники':");
            foreach (DataRow row in products.Rows)
            {
                if (row.RowState == DataRowState.Deleted &&
                    row.HasVersion(DataRowVersion.Original) &&
                    row["ProductName", DataRowVersion.Original].ToString() == "Наушники")
                {
                    row.RejectChanges();
                    Console.WriteLine("  ✓ Удаление отменено");
                    break;
                }
            }

            // 8. Финальное состояние
            Console.WriteLine("\n\n8. Финальное состояние:");
            PrintAllProducts(ds);
        }

        static void PrintAllProducts(DataSet ds)
        {
            DataTable products = ds.Tables["Товары"];
            DataRelation relation = ds.Relations["FK_Products_Categories"];

            Console.WriteLine($"{"ID",-5} {"Товар",-25} {"Цена",-10} {"Категория",-15} {"Статус"}");
            Console.WriteLine(new string('-', 70));

            foreach (DataRow product in products.Rows)
            {
                if (product.RowState != DataRowState.Deleted)
                {
                    string productName = product["ProductName"].ToString();
                    decimal price = (decimal)product["Price"];
                    string status = product.RowState.ToString();

                    string categoryName = "?";
                    DataRow[] parents = product.GetParentRows(relation);
                    if (parents.Length > 0)
                        categoryName = parents[0]["CategoryName"].ToString();

                    Console.WriteLine($"{product["ProductID"],-5} {productName,-25} {price,-10:C} {categoryName,-15} {status}");
                }
            }
        }
        #endregion

        #region Задание 13: RowState Added
        static void Task13_RowStateAdded()
        {
            Console.WriteLine("ЗАДАНИЕ 13: Получение связанной информации для строк со статусом Added\n");
            Console.WriteLine("Цель: Анализ новых добавленных записей перед сохранением\n");

            DataSet ds = CreateUniversityDataSet();
            DataTable students = ds.Tables["Студенты"];
            DataTable courses = ds.Tables["Курсы"];
            DataTable registration = ds.Tables["Регистрация"];

            DataRelation studentRel = ds.Relations["FK_Registration_Students"];
            DataRelation courseRel = ds.Relations["FK_Registration_Courses"];

            Console.WriteLine("1. Исходные данные:");
            Console.WriteLine($"Студентов: {students.Rows.Count}");
            Console.WriteLine($"Курсов: {courses.Rows.Count}");
            Console.WriteLine($"Регистраций: {registration.Rows.Count}");

            // Сохраняем исходное состояние проверки ограничений
            bool originalEnforceConstraints = ds.EnforceConstraints;
            Console.WriteLine($"Проверка ограничений включена: {originalEnforceConstraints}");

            List<DataRow> addedRowsList = new List<DataRow>();

            try
            {
                // Временно отключаем проверку ограничений для добавления тестовых данных
                ds.EnforceConstraints = false;
                Console.WriteLine("\nВременно отключаем проверку ограничений...");

                // 2. Добавляем новые регистрации
                Console.WriteLine("\n\n2. Добавляем новые регистрации (включая некорректные для теста):");

                // Новая регистрация 1 - корректная
                DataRow newReg1 = registration.NewRow();
                newReg1["RegistrationID"] = 100;
                newReg1["StudentID"] = 101;
                newReg1["CourseID"] = "C001";
                newReg1["EnrollmentDate"] = DateTime.Now;
                newReg1["Grade"] = 4.7;
                registration.Rows.Add(newReg1);
                addedRowsList.Add(newReg1);
                Console.WriteLine($"  Добавлена регистрация {newReg1["RegistrationID"]} для студента 101, курс C001");

                // Новая регистрация 2 - корректная
                DataRow newReg2 = registration.NewRow();
                newReg2["RegistrationID"] = 101;
                newReg2["StudentID"] = 102;
                newReg2["CourseID"] = "C002";
                newReg2["EnrollmentDate"] = DateTime.Now;
                newReg2["Grade"] = 4.3;
                registration.Rows.Add(newReg2);
                addedRowsList.Add(newReg2);
                Console.WriteLine($"  Добавлена регистрация {newReg2["RegistrationID"]} для студента 102, курс C002");

                // Новая регистрация 3 - с несуществующим студентом (для теста ошибок)
                DataRow newReg3 = registration.NewRow();
                newReg3["RegistrationID"] = 102;
                newReg3["StudentID"] = 999; // Несуществующий ID
                newReg3["CourseID"] = "C001";
                newReg3["EnrollmentDate"] = DateTime.Now;
                newReg3["Grade"] = 4.0;
                registration.Rows.Add(newReg3);
                addedRowsList.Add(newReg3);
                Console.WriteLine($"  Добавлена регистрация {newReg3["RegistrationID"]} для несуществующего студента 999 (для теста)");

                // Новая регистрация 4 - с несуществующим курсом
                DataRow newReg4 = registration.NewRow();
                newReg4["RegistrationID"] = 103;
                newReg4["StudentID"] = 101;
                newReg4["CourseID"] = "C999"; // Несуществующий курс
                newReg4["EnrollmentDate"] = DateTime.Now;
                newReg4["Grade"] = 4.5;
                registration.Rows.Add(newReg4);
                addedRowsList.Add(newReg4);
                Console.WriteLine($"  Добавлена регистрация {newReg4["RegistrationID"]} для несуществующего курса C999 (для теста)");

                Console.WriteLine("Итого: добавлено 4 новые регистрации (2 корректные, 2 с ошибками)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении данных: {ex.Message}");
                return;
            }

            // 3. Получение новых регистраций (используем наш список, так как Select может не работать с выключенными constraints)
            Console.WriteLine("\n\n3. Анализ добавленных регистраций:");
            Console.WriteLine($"Всего добавлено регистраций: {addedRowsList.Count}");

            // 4. Отчет о новых регистрациях
            Console.WriteLine("\n\n4. Отчет о новых регистрациях:");
            Console.WriteLine($"{"ID",-5} {"Студент",-15} {"Курс",-20} {"Оценка",-8} {"Дата",-12} {"Статус валидации"}");
            Console.WriteLine(new string('-', 80));

            List<DataRow> invalidRows = new List<DataRow>();
            List<DataRow> validRows = new List<DataRow>();

            foreach (DataRow reg in addedRowsList)
            {
                // Получаем информацию о студенте
                int studentID = (int)reg["StudentID"];
                DataRow student = students.Rows.Find(studentID);
                string studentName = student != null ? student["StudentName"].ToString() : "Не найден";
                bool studentExists = student != null && student.RowState != DataRowState.Deleted;

                // Получаем информацию о курсе
                string courseID = (string)reg["CourseID"];
                DataRow course = courses.Rows.Find(courseID);
                string courseName = course != null ? course["CourseName"].ToString() : "Не найден";
                bool courseExists = course != null && course.RowState != DataRowState.Deleted;

                // Валидация
                bool isValid = studentExists && courseExists;
                string validationStatus = isValid ? "✓ OK" : "✗ Ошибка";

                if (!isValid)
                    invalidRows.Add(reg);
                else
                    validRows.Add(reg);

                Console.WriteLine($"{reg["RegistrationID"],-5} {studentName,-15} {courseName,-20} " +
                    $"{reg["Grade"],-8:F1} {((DateTime)reg["EnrollmentDate"]):dd.MM.yyyy} {validationStatus}");
            }

            // 5. Валидация перед сохранением
            Console.WriteLine("\n\n5. Валидация перед сохранением:");

            if (invalidRows.Count > 0)
            {
                Console.WriteLine($"Найдено невалидных регистраций: {invalidRows.Count}");
                foreach (DataRow invalidReg in invalidRows)
                {
                    int studentID = (int)invalidReg["StudentID"];
                    string courseID = (string)invalidReg["CourseID"];
                    Console.WriteLine($"  ✗ Регистрация {invalidReg["RegistrationID"]}: " +
                        $"Студент ID={studentID} {(students.Rows.Find(studentID) == null ? "не найден" : "удален")}, " +
                        $"Курс ID={courseID} {(courses.Rows.Find(courseID) == null ? "не найден" : "удален")}");
                }
            }
            else
            {
                Console.WriteLine("✓ Все новые регистрации валидны");
            }

            // 6. Статистика по новым регистрациям
            Console.WriteLine("\n\n6. Статистика по новым регистрациям:");

            // По студентам (только валидные)
            var studentStats = validRows
                .GroupBy(r => (int)r["StudentID"])
                .Select(g => new
                {
                    StudentID = g.Key,
                    Count = g.Count(),
                    StudentName = GetStudentName(g.Key, students)
                });

            if (studentStats.Any())
            {
                Console.WriteLine("\nПо студентам (только валидные):");
                foreach (var stat in studentStats)
                {
                    Console.WriteLine($"  • {stat.StudentName}: {stat.Count} новых регистраций");
                }
            }

            // По курсам (только валидные)
            var courseStats = validRows
                .GroupBy(r => (string)r["CourseID"])
                .Select(g => new
                {
                    CourseID = g.Key,
                    Count = g.Count(),
                    CourseName = GetCourseName(g.Key, courses)
                });

            if (courseStats.Any())
            {
                Console.WriteLine("\nПо курсам (только валидные):");
                foreach (var stat in courseStats)
                {
                    Console.WriteLine($"  • {stat.CourseName}: {stat.Count} новых регистраций");
                }
            }

            // 7. Очистка невалидных записей перед включением проверки ограничений
            Console.WriteLine("\n\n7. Очистка невалидных записей:");

            if (invalidRows.Count > 0)
            {
                Console.WriteLine($"Удаляем {invalidRows.Count} невалидных регистраций...");

                foreach (DataRow invalidRow in invalidRows)
                {
                    Console.WriteLine($"  Удаление регистрации {invalidRow["RegistrationID"]}");
                    invalidRow.Delete();
                }

                // Применяем изменения
                registration.AcceptChanges();
                Console.WriteLine($"Удалено регистраций: {invalidRows.Count}");
            }
            else
            {
                Console.WriteLine("Нет невалидных записей для удаления");
            }

            // 8. Восстанавливаем проверку ограничений
            Console.WriteLine("\n\n8. Восстановление проверки ограничений:");
            try
            {
                ds.EnforceConstraints = true;
                Console.WriteLine("✓ Проверка ограничений успешно включена");
            }
            catch (ConstraintException ex)
            {
                Console.WriteLine($"✗ Не удалось включить проверку ограничений: {ex.Message}");
                Console.WriteLine("  В данных всё ещё есть нарушения ограничений");

                // Пытаемся найти оставшиеся проблемы
                Console.WriteLine("\nПоиск оставшихся проблем:");
                foreach (DataRow reg in registration.Rows)
                {
                    if (reg.RowState != DataRowState.Deleted)
                    {
                        int studentID = (int)reg["StudentID"];
                        string courseID = (string)reg["CourseID"];

                        DataRow student = students.Rows.Find(studentID);
                        DataRow course = courses.Rows.Find(courseID);

                        if (student == null || course == null)
                        {
                            Console.WriteLine($"  ✗ Регистрация {reg["RegistrationID"]}: " +
                                $"Студент {studentID} {(student == null ? "не найден" : "OK")}, " +
                                $"Курс {courseID} {(course == null ? "не найден" : "OK")}");
                        }
                    }
                }

                // Оставляем проверку отключенной
                ds.EnforceConstraints = false;
                Console.WriteLine("Проверка ограничений остаётся отключенной");
            }

            // 9. Финальная статистика
            Console.WriteLine("\n\n9. Финальная статистика:");
            Console.WriteLine($"Всего студентов в системе: {students.Rows.Count}");
            Console.WriteLine($"Всего курсов в системе: {courses.Rows.Count}");
            Console.WriteLine($"Всего регистраций в системе: {registration.Rows.Count}");

            // Подсчитываем добавленные строки (те, что не были удалены)
            int remainingAdded = registration.Rows.Cast<DataRow>()
                .Count(r => r.RowState == DataRowState.Added ||
                           (r.RowState == DataRowState.Unchanged && addedRowsList.Contains(r)));

            Console.WriteLine($"Добавленных регистраций (сохранённых): {remainingAdded}");
            Console.WriteLine($"Валидных добавленных регистраций: {validRows.Count}");
            Console.WriteLine($"Удалённых невалидных регистраций: {invalidRows.Count}");

            // 10. Демонстрация GetChanges
            Console.WriteLine("\n\n10. Демонстрация GetChanges():");

            DataSet changes = ds.GetChanges(DataRowState.Added);
            if (changes != null && changes.Tables.Contains("Регистрация"))
            {
                int addedInChanges = changes.Tables["Регистрация"].Rows.Count;
                Console.WriteLine($"GetChanges(DataRowState.Added) вернул {addedInChanges} добавленных регистраций");

                if (addedInChanges > 0)
                {
                    Console.WriteLine("Добавленные регистрации:");
                    foreach (DataRow changeRow in changes.Tables["Регистрация"].Rows)
                    {
                        Console.WriteLine($"  • Регистрация {changeRow["RegistrationID"]} для студента {changeRow["StudentID"]}, курс {changeRow["CourseID"]}");
                    }
                }
            }
            else
            {
                Console.WriteLine("GetChanges() не вернул добавленных регистраций");
            }
        }

        static string GetStudentName(int studentID, DataTable students)
        {
            DataRow student = students.Rows.Find(studentID);
            return student != null ? student["StudentName"].ToString() : "Неизвестно";
        }

        static string GetCourseName(string courseID, DataTable courses)
        {
            DataRow course = courses.Rows.Find(courseID);
            return course != null ? course["CourseName"].ToString() : "Неизвестно";
        }
        #endregion

        #region Задание 14: RowState Modified
        static void Task14_RowStateModified()
        {
            Console.WriteLine("ЗАДАНИЕ 14: Получение связанной информации для строк со статусом Modified\n");
            Console.WriteLine("Цель: Анализ изменённых записей перед сохранением\n");

            DataSet ds = CreateUniversityDataSet();
            DataTable students = ds.Tables["Студенты"];
            DataTable courses = ds.Tables["Курсы"];
            DataTable registration = ds.Tables["Регистрация"];

            DataRelation studentRel = ds.Relations["FK_Registration_Students"];
            DataRelation courseRel = ds.Relations["FK_Registration_Courses"];

            Console.WriteLine("1. Исходные оценки:");
            PrintAllGrades(registration, studentRel, courseRel);

            // 2. Модифицируем оценки
            Console.WriteLine("\n\n2. Модификация оценок:");

            // Меняем оценку 1
            DataRow grade1 = registration.Rows.Find(1);
            Console.WriteLine($"Меняем оценку {grade1["Grade"]} → 4.8");
            grade1["Grade"] = 4.8;

            // Меняем оценку 2
            DataRow grade2 = registration.Rows.Find(2);
            Console.WriteLine($"Меняем оценку {grade2["Grade"]} → 4.0");
            grade2["Grade"] = 4.0;

            // Меняем оценку 3
            DataRow grade3 = registration.Rows.Find(3);
            Console.WriteLine($"Меняем оценку {grade3["Grade"]} → 5.0");
            grade3["Grade"] = 5.0;

            // Меняем на невалидное значение
            DataRow grade4 = registration.Rows.Find(4);
            Console.WriteLine($"Меняем оценку {grade4["Grade"]} → 1.5 (невалидно)");
            grade4["Grade"] = 1.5;

            // 3. Получение измененных строк
            Console.WriteLine("\n\n3. Получение измененных строк:");
            DataRow[] modifiedRows = registration.Select(null, null, DataViewRowState.ModifiedCurrent);
            Console.WriteLine($"Найдено измененных записей: {modifiedRows.Length}");

            // 4. Отчет об изменениях
            Console.WriteLine("\n\n4. Отчет об изменениях:");
            Console.WriteLine($"{"ID",-5} {"Студент",-15} {"Курс",-20} {"Старая оценка",-15} {"Новая оценка",-15} {"Изменение",-10}");
            Console.WriteLine(new string('-', 85));

            int improved = 0, worsened = 0, same = 0;

            foreach (DataRow reg in modifiedRows)
            {
                if (reg.HasVersion(DataRowVersion.Original) && reg.HasVersion(DataRowVersion.Current))
                {
                    double oldGrade = (double)reg["Grade", DataRowVersion.Original];
                    double newGrade = (double)reg["Grade", DataRowVersion.Current];

                    string studentName = "?";
                    DataRow[] studentRows = reg.GetParentRows(studentRel);
                    if (studentRows.Length > 0)
                        studentName = studentRows[0]["StudentName"].ToString();

                    string courseName = "?";
                    DataRow[] courseRows = reg.GetParentRows(courseRel);
                    if (courseRows.Length > 0)
                        courseName = courseRows[0]["CourseName"].ToString();

                    double change = newGrade - oldGrade;
                    string changeSymbol = change > 0 ? "↑" : change < 0 ? "↓" : "→";

                    Console.WriteLine($"{reg["RegistrationID"],-5} {studentName,-15} {courseName,-20} " +
                        $"{oldGrade,15:F1} {newGrade,15:F1} {change,10:F1} {changeSymbol}");

                    if (newGrade > oldGrade) improved++;
                    else if (newGrade < oldGrade) worsened++;
                    else same++;
                }
            }

            // 5. Статистика изменений
            Console.WriteLine("\n\n5. Статистика изменений:");
            Console.WriteLine($"Улучшилось оценок: {improved}");
            Console.WriteLine($"Ухудшилось оценок: {worsened}");
            Console.WriteLine($"Осталось без изменений: {same}");
            Console.WriteLine($"Всего изменений: {improved + worsened + same}");

            // 6. Валидация перед сохранением
            Console.WriteLine("\n\n6. Валидация измененных оценок:");
            bool hasErrors = false;

            foreach (DataRow reg in modifiedRows)
            {
                if (reg.HasVersion(DataRowVersion.Current))
                {
                    double grade = (double)reg["Grade", DataRowVersion.Current];

                    if (grade < 2.0 || grade > 5.0)
                    {
                        hasErrors = true;
                        Console.WriteLine($"✗ Оценка {grade:F1} вне допустимого диапазона (2.0-5.0)");

                        // Автоматическое исправление
                        if (grade < 2.0)
                        {
                            reg["Grade"] = 2.0;
                            Console.WriteLine($"  Исправлено на 2.0");
                        }
                        else if (grade > 5.0)
                        {
                            reg["Grade"] = 5.0;
                            Console.WriteLine($"  Исправлено на 5.0");
                        }
                    }
                }
            }

            if (!hasErrors)
                Console.WriteLine("✓ Все оценки в допустимом диапазоне");

            // 7. Детальный анализ дельты
            Console.WriteLine("\n\n7. Детальный анализ изменений:");

            var studentChanges = modifiedRows
                .Where(r => r.HasVersion(DataRowVersion.Original) && r.HasVersion(DataRowVersion.Current))
                .GroupBy(r =>
                {
                    DataRow[] studentRows = r.GetParentRows(studentRel);
                    return studentRows.Length > 0 ? studentRows[0]["StudentName"].ToString() : "Неизвестно";
                })
                .Select(g => new
                {
                    Student = g.Key,
                    Changes = g.Count(),
                    AvgChange = g.Average(r =>
                        (double)r["Grade", DataRowVersion.Current] -
                        (double)r["Grade", DataRowVersion.Original])
                });

            Console.WriteLine($"{"Студент",-15} {"Изменений",-10} {"Среднее изменение"}");
            Console.WriteLine(new string('-', 45));

            foreach (var change in studentChanges)
            {
                Console.WriteLine($"{change.Student,-15} {change.Changes,-10} {change.AvgChange,15:F2}");
            }

            // 8. Финальные оценки
            Console.WriteLine("\n\n8. Финальные оценки:");
            PrintAllGrades(registration, studentRel, courseRel);
        }

        static void PrintAllGrades(DataTable registration, DataRelation studentRel, DataRelation courseRel)
        {
            Console.WriteLine($"{"ID",-5} {"Студент",-15} {"Курс",-20} {"Оценка",-8} {"Статус"}");
            Console.WriteLine(new string('-', 60));

            foreach (DataRow reg in registration.Rows)
            {
                if (reg.RowState != DataRowState.Deleted)
                {
                    string studentName = "?";
                    DataRow[] studentRows = reg.GetParentRows(studentRel);
                    if (studentRows.Length > 0)
                        studentName = studentRows[0]["StudentName"].ToString();

                    string courseName = "?";
                    DataRow[] courseRows = reg.GetParentRows(courseRel);
                    if (courseRows.Length > 0)
                        courseName = courseRows[0]["CourseName"].ToString();

                    double grade = (double)reg["Grade"];
                    string status = reg.RowState.ToString();

                    Console.WriteLine($"{reg["RegistrationID"],-5} {studentName,-15} {courseName,-20} {grade,-8:F1} {status}");
                }
            }
        }
        #endregion

        #region Задание 15: Каскадное удаление
        static void Task15_CascadingDeletion()
        {
            Console.WriteLine("ЗАДАНИЕ 15: Каскадное удаление с отслеживанием изменений во всех таблицах\n");
            Console.WriteLine("Цель: Отслеживание каскадного удаления через все уровни иерархии\n");

            DataSet ds = CreateOrderManagementSystem();
            DataTable customers = ds.Tables["Заказчики"];
            DataTable orders = ds.Tables["Заказы"];
            DataTable orderDetails = ds.Tables["ОрдеротовыеКетали"];

            DataRelation custOrderRel = ds.Relations["FK_Orders_Customers"];
            DataRelation orderDetailsRel = ds.Relations["FK_OrderDetails_Orders"];

            Console.WriteLine("1. Исходная иерархия:");
            PrintOrderHierarchy(ds);

            // 2. Удаляем заказчика
            Console.WriteLine("\n\n2. Удаление заказчика (каскадное):");
            DataRow customerToDelete = customers.Rows.Find(101);
            Console.WriteLine($"Удаляем заказчика: {customerToDelete["CustomerName"]}");

            // Сохраняем информацию перед удалением
            List<DeletedRecordInfo> allDeletedRecords = new List<DeletedRecordInfo>();

            // Собираем информацию о заказчике
            allDeletedRecords.Add(new DeletedRecordInfo
            {
                TableName = "Заказчики",
                RecordID = customerToDelete["CustomerID"].ToString(),
                RecordName = customerToDelete["CustomerName"].ToString()
            });

            // Собираем информацию о заказах заказчика
            DataRow[] customerOrders = customerToDelete.GetChildRows(custOrderRel);
            foreach (DataRow order in customerOrders)
            {
                allDeletedRecords.Add(new DeletedRecordInfo
                {
                    TableName = "Заказы",
                    RecordID = order["OrderID"].ToString(),
                    RecordName = $"Заказ #{order["OrderID"]}",
                    ParentID = customerToDelete["CustomerID"].ToString()
                });

                // Собираем информацию о деталях заказа
                DataRow[] details = order.GetChildRows(orderDetailsRel);
                foreach (DataRow detail in details)
                {
                    allDeletedRecords.Add(new DeletedRecordInfo
                    {
                        TableName = "Детали заказов",
                        RecordID = detail["DetailID"].ToString(),
                        RecordName = $"Деталь #{detail["DetailID"]}",
                        ParentID = order["OrderID"].ToString()
                    });
                }
            }

            // Выполняем удаление
            customerToDelete.Delete();

            // 3. Анализ удаленных строк
            Console.WriteLine("\n\n3. Анализ удаленных строк:");
            Console.WriteLine("Удаленные строки по таблицам:");

            var deletedByTable = allDeletedRecords
                .GroupBy(r => r.TableName)
                .Select(g => new { Table = g.Key, Count = g.Count() });

            foreach (var table in deletedByTable)
            {
                Console.WriteLine($"  {table.Table}: {table.Count} записей");
            }

            // 4. Полный отчет об удалении
            Console.WriteLine("\n\n4. Полный отчет об удалении:");
            Console.WriteLine("Цепочка удаления:");

            foreach (var record in allDeletedRecords)
            {
                string indent = "";
                if (record.TableName == "Заказы")
                    indent = "  ";
                else if (record.TableName == "Детали заказов")
                    indent = "    ";

                Console.WriteLine($"{indent}{record.TableName}: {record.RecordName}");
            }

            // 5. Информация о RowState
            Console.WriteLine("\n\n5. RowState после удаления:");

            int totalDeletedRows = 0;
            foreach (DataRow row in customers.Rows)
                if (row.RowState == DataRowState.Deleted) totalDeletedRows++;
            foreach (DataRow row in orders.Rows)
                if (row.RowState == DataRowState.Deleted) totalDeletedRows++;
            foreach (DataRow row in orderDetails.Rows)
                if (row.RowState == DataRowState.Deleted) totalDeletedRows++;

            Console.WriteLine($"Всего помечено на удаление: {totalDeletedRows} строк");
            Console.WriteLine($"Ожидалось удалить: {allDeletedRecords.Count} строк");

            // 6. Получение удаленных строк через GetChanges
            Console.WriteLine("\n\n6. Получение удаленных строк через GetChanges():");

            DataSet changes = ds.GetChanges(DataRowState.Deleted);
            if (changes != null)
            {
                Console.WriteLine("Удаленные строки в GetChanges():");

                foreach (DataTable table in changes.Tables)
                {
                    Console.WriteLine($"  Таблица {table.TableName}: {table.Rows.Count} строк");

                    foreach (DataRow row in table.Rows)
                    {
                        string originalInfo = "";
                        if (row.HasVersion(DataRowVersion.Original))
                        {
                            // Пытаемся получить осмысленное представление строки
                            if (table.TableName == "Заказчики")
                                originalInfo = $"'{row["CustomerName", DataRowVersion.Original]}'";
                            else if (table.TableName == "Заказы")
                                originalInfo = $"Заказ #{row["OrderID", DataRowVersion.Original]}";
                            else if (table.TableName == "ОрдеротовыеКетали")
                                originalInfo = $"Деталь #{row["DetailID", DataRowVersion.Original]}";
                        }

                        Console.WriteLine($"    • {originalInfo}");
                    }
                }
            }

            // 7. Откат изменений
            Console.WriteLine("\n\n7. Откат изменений (RejectChanges):");
            Console.WriteLine("Выполняем откат всех изменений...");

            ds.RejectChanges();

            Console.WriteLine("Состояние после отката:");
            Console.WriteLine($"Заказчиков: {customers.Rows.Count}");
            Console.WriteLine($"Заказов: {orders.Rows.Count}");
            Console.WriteLine($"Деталей заказов: {orderDetails.Rows.Count}");

            // 8. Проверка восстановления
            Console.WriteLine("\n\n8. Проверка восстановления данных:");
            bool allRestored = true;

            foreach (var record in allDeletedRecords)
            {
                bool exists = false;

                if (record.TableName == "Заказчики")
                {
                    DataRow row = customers.Rows.Find(int.Parse(record.RecordID));
                    exists = row != null && row.RowState != DataRowState.Deleted;
                }
                else if (record.TableName == "Заказы")
                {
                    DataRow row = orders.Rows.Find(int.Parse(record.RecordID));
                    exists = row != null && row.RowState != DataRowState.Deleted;
                }
                else if (record.TableName == "Детали заказов")
                {
                    DataRow row = orderDetails.Rows.Find(int.Parse(record.RecordID));
                    exists = row != null && row.RowState != DataRowState.Deleted;
                }

                if (!exists)
                {
                    allRestored = false;
                    Console.WriteLine($"✗ Не восстановлено: {record.TableName} - {record.RecordName}");
                }
            }

            if (allRestored)
                Console.WriteLine("✓ Все данные успешно восстановлены");
        }

        class DeletedRecordInfo
        {
            public string TableName { get; set; }
            public string RecordID { get; set; }
            public string RecordName { get; set; }
            public string ParentID { get; set; }
        }
        #endregion

        #region Задание 16: Проверка ссылочной целостности
        static void Task16_ReferentialIntegrity()
        {
            Console.WriteLine("ЗАДАНИЕ 16: Проверка ссылочной целостности перед сохранением\n");
            Console.WriteLine("Цель: Валидация всех отношений перед сохранением данных\n");

            DataSet ds = CreateStoreDataSet();
            DataTable categories = ds.Tables["Категории"];
            DataTable products = ds.Tables["Товары"];
            DataRelation relation = ds.Relations["FK_Products_Categories"];

            Console.WriteLine("1. Исходное состояние:");
            Console.WriteLine($"Категорий: {categories.Rows.Count}");
            Console.WriteLine($"Товаров: {products.Rows.Count}");

            // Сохраняем исходное состояние проверки ограничений
            bool originalEnforceConstraints = ds.EnforceConstraints;

            try
            {
                // Временно отключаем проверку ограничений для создания тестовых нарушений
                ds.EnforceConstraints = false;
                Console.WriteLine("\nВременно отключаем проверку ограничений для создания тестовых данных...");

                // 2. Создаем нарушения целостности
                Console.WriteLine("\n\n2. Создаем нарушения целостности:");

                // Добавляем товар с несуществующей категорией
                DataRow orphanProduct = products.NewRow();
                orphanProduct["ProductID"] = 999;
                orphanProduct["ProductName"] = "Товар-сирота";
                orphanProduct["Price"] = 1000;
                orphanProduct["Quantity"] = 1;
                orphanProduct["CategoryID"] = 9999; // Несуществующая категория
                products.Rows.Add(orphanProduct);
                Console.WriteLine("Добавлен товар с несуществующей категорией (ID: 9999)");

                // Изменяем ID категории у существующей категории
                DataRow categoryToModify = categories.Rows.Find(2);
                if (categoryToModify != null)
                {
                    Console.WriteLine($"Изменен ID категории '{categoryToModify["CategoryName"]}' с 2 на 200");
                    categoryToModify["CategoryID"] = 200;
                }

                // Удаляем категорию
                DataRow categoryToDelete = categories.Rows.Find(3);
                if (categoryToDelete != null)
                {
                    Console.WriteLine($"Удалена категория '{categoryToDelete["CategoryName"]}' с ID: 3");
                    categoryToDelete.Delete();
                }

                // 3. Проверка целостности
                Console.WriteLine("\n\n3. Проверка целостности (CheckReferentialIntegrity):");

                var integrityReport = CheckReferentialIntegrity(ds);

                Console.WriteLine($"Нарушений целостности: {integrityReport.Violations.Count}");

                if (integrityReport.Violations.Count > 0)
                {
                    Console.WriteLine("\nНарушения целостности:");
                    foreach (var violation in integrityReport.Violations)
                    {
                        Console.WriteLine($"  • {violation}");
                    }
                }

                // 4. Автоматическое исправление
                Console.WriteLine("\n\n4. Автоматическое исправление:");

                int fixedOrphans = 0;
                List<DataRow> productsToFix = new List<DataRow>();

                // Собираем товары, которые нужно исправить
                foreach (DataRow product in products.Rows)
                {
                    if (product.RowState != DataRowState.Deleted)
                    {
                        DataRow[] parents = product.GetParentRows(relation);
                        if (parents.Length == 0)
                        {
                            productsToFix.Add(product);
                        }
                    }
                }

                // Исправляем товары
                foreach (DataRow product in productsToFix)
                {
                    Console.WriteLine($"Товар {product["ProductID"]} '{product["ProductName"]}' без категории");

                    // Варианты исправления:
                    // 1. Удалить товар
                    // 2. Установить NULL
                    // 3. Привязать к существующей категории

                    // Используем вариант 3: привязать к первой существующей категории
                    if (categories.Rows.Count > 0)
                    {
                        DataRow firstCategory = categories.Rows[0];
                        product["CategoryID"] = firstCategory["CategoryID"];
                        fixedOrphans++;
                        Console.WriteLine($"  Исправлено: привязан к категории '{firstCategory["CategoryName"]}' (ID: {firstCategory["CategoryID"]})");
                    }
                    else
                    {
                        // Если нет категорий, устанавливаем NULL
                        product["CategoryID"] = DBNull.Value;
                        fixedOrphans++;
                        Console.WriteLine($"  Исправлено: установлен NULL (нет доступных категорий)");
                    }
                }

                Console.WriteLine($"Исправлено orphaned records: {fixedOrphans}");

                // 5. Восстанавливаем проверку ограничений и проверяем целостность
                Console.WriteLine("\n\n5. Восстановление проверки ограничений:");

                try
                {
                    ds.EnforceConstraints = true;
                    Console.WriteLine("✓ Проверка ограничений успешно включена");

                    // Повторная проверка целостности
                    integrityReport = CheckReferentialIntegrity(ds);
                    Console.WriteLine($"Оставшихся нарушений: {integrityReport.Violations.Count}");

                    if (integrityReport.Violations.Count == 0)
                    {
                        Console.WriteLine("✓ Целостность восстановлена");
                    }
                    else
                    {
                        Console.WriteLine("\nОставшиеся нарушения:");
                        foreach (var violation in integrityReport.Violations)
                        {
                            Console.WriteLine($"  • {violation}");
                        }
                    }
                }
                catch (ConstraintException ex)
                {
                    Console.WriteLine($"✗ Не удалось включить проверку ограничений: {ex.Message}");
                    Console.WriteLine("  В данных всё ещё есть нарушения ограничений");
                    ds.EnforceConstraints = false;
                }

                // 6. Отчет перед сохранением
                Console.WriteLine("\n\n6. Отчет перед сохранением:");
                Console.WriteLine($"Категорий: {categories.Rows.Count}");
                Console.WriteLine($"Товаров: {products.Rows.Count}");

                int pendingChanges = 0;
                foreach (DataRow row in categories.Rows)
                    if (row.RowState != DataRowState.Unchanged) pendingChanges++;
                foreach (DataRow row in products.Rows)
                    if (row.RowState != DataRowState.Unchanged) pendingChanges++;

                Console.WriteLine($"Ожидающих изменений: {pendingChanges}");

                // 7. Подробный отчет о нарушениях
                Console.WriteLine("\n\n7. Подробный отчет о нарушениях:");

                // Находим товары с NULL категориями
                var nullCategoryProducts = products.Rows
                    .Cast<DataRow>()
                    .Where(r => r.RowState != DataRowState.Deleted &&
                               (r["CategoryID"] == DBNull.Value || r["CategoryID"] == null))
                    .ToList();

                if (nullCategoryProducts.Count > 0)
                {
                    Console.WriteLine("Товары без категорий:");
                    foreach (DataRow product in nullCategoryProducts)
                    {
                        Console.WriteLine($"  • {product["ProductName"]} (ID: {product["ProductID"]})");
                    }
                }
                else
                {
                    Console.WriteLine("Товары без категорий: нет");
                }

                // Находим категории без товаров
                var emptyCategories = categories.Rows
                    .Cast<DataRow>()
                    .Where(r => r.RowState != DataRowState.Deleted)
                    .Where(r => r.GetChildRows(relation).Length == 0)
                    .ToList();

                if (emptyCategories.Count > 0)
                {
                    Console.WriteLine("\nКатегории без товаров:");
                    foreach (DataRow category in emptyCategories)
                    {
                        Console.WriteLine($"  • {category["CategoryName"]} (ID: {category["CategoryID"]})");
                    }
                }
                else
                {
                    Console.WriteLine("\nКатегории без товаров: нет");
                }

                // 8. Рекомендации по исправлению
                Console.WriteLine("\n\n8. Рекомендации по исправлению:");

                if (nullCategoryProducts.Count > 0)
                {
                    Console.WriteLine("Для товаров без категорий:");
                    Console.WriteLine("  1. Удалить товары");
                    Console.WriteLine("  2. Привязать к существующей категории");
                    Console.WriteLine("  3. Создать новую категорию");
                }

                if (emptyCategories.Count > 0)
                {
                    Console.WriteLine("\nДля пустых категорий:");
                    Console.WriteLine("  1. Удалить категории");
                    Console.WriteLine("  2. Добавить товары в категории");
                    Console.WriteLine("  3. Переименовать и использовать для новых товаров");
                }

                // 9. Восстановление исходных данных (для чистоты теста)
                Console.WriteLine("\n\n9. Восстановление исходных данных:");

                // Откатываем изменения
                ds.RejectChanges();

                // Восстанавливаем проверку ограничений
                ds.EnforceConstraints = originalEnforceConstraints;

                Console.WriteLine("✓ Исходные данные восстановлены");
                Console.WriteLine($"Категорий: {categories.Rows.Count}");
                Console.WriteLine($"Товаров: {products.Rows.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");

                // Восстанавливаем исходное состояние в случае ошибки
                try
                {
                    ds.RejectChanges();
                    ds.EnforceConstraints = originalEnforceConstraints;
                }
                catch { }
            }
        }

        class IntegrityReport
        {
            public List<string> Violations { get; set; } = new List<string>();
            public bool IsValid => Violations.Count == 0;
        }

        static IntegrityReport CheckReferentialIntegrity(DataSet ds)
        {
            var report = new IntegrityReport();

            foreach (DataRelation relation in ds.Relations)
            {
                DataTable parentTable = relation.ParentTable;
                DataTable childTable = relation.ChildTable;

                // Получаем все существующие ID родительской таблицы
                HashSet<object> parentIds = new HashSet<object>();
                foreach (DataRow parentRow in parentTable.Rows)
                {
                    if (parentRow.RowState != DataRowState.Deleted)
                    {
                        parentIds.Add(parentRow[relation.ParentColumns[0]]);
                    }
                }

                // Проверяем все строки в дочерней таблице
                foreach (DataRow childRow in childTable.Rows)
                {
                    if (childRow.RowState != DataRowState.Deleted)
                    {
                        object childKeyValue = childRow[relation.ChildColumns[0]];

                        // Проверяем NULL значения
                        if (childKeyValue == DBNull.Value || childKeyValue == null)
                        {
                            // NULL разрешен, если колонка допускает NULL
                            if (!relation.ChildColumns[0].AllowDBNull)
                            {
                                string violation = $"{childTable.TableName}.{childRow[relation.ChildColumns[0]]} " +
                                    $"имеет NULL значение в колонке {relation.ChildColumns[0].ColumnName}, которая не допускает NULL";
                                report.Violations.Add(violation);
                            }
                        }
                        else if (!parentIds.Contains(childKeyValue))
                        {
                            // Не найден родитель
                            string violation = $"{childTable.TableName}.ID={childRow[relation.ChildColumns[0]]} " +
                                $"ссылается на несуществующий {parentTable.TableName}.{relation.ParentColumns[0].ColumnName}={childKeyValue}";
                            report.Violations.Add(violation);
                        }
                    }
                }
            }

            return report;
        }
        #endregion

        #region Задание 17: DataView фильтрация
        static void Task17_DataViewFiltering()
        {
            Console.WriteLine("ЗАДАНИЕ 17: Использование DataRelation для фильтрации данных в DataView\n");
            Console.WriteLine("Цель: Фильтрация и сортировка данных через DataView с использованием отношений\n");

            DataSet ds = CreateOrderManagementSystem();
            DataTable customers = ds.Tables["Заказчики"];
            DataTable orders = ds.Tables["Заказы"];
            DataRelation relation = ds.Relations["FK_Orders_Customers"];

            Console.WriteLine("1. Создание DataView для заказов:");
            DataView ordersView = new DataView(orders);

            // 2. Фильтрация: заказы конкретного заказчика
            Console.WriteLine("\n\n2. Фильтрация: заказы заказчика с ID 101");
            int customerId = 101;

            DataRow customer = customers.Rows.Find(customerId);
            if (customer != null)
            {
                // Получаем заказы через отношение
                DataRow[] customerOrders = customer.GetChildRows(relation);

                // Создаем DataView с фильтром
                DataView customerOrdersView = new DataView(orders);
                customerOrdersView.RowFilter = $"CustomerID = {customerId}";

                Console.WriteLine($"Заказы заказчика {customer["CustomerName"]}:");
                Console.WriteLine($"{"ID заказа",-10} {"Дата",-12} {"Сумма",-10}");
                Console.WriteLine(new string('-', 35));

                foreach (DataRowView rowView in customerOrdersView)
                {
                    Console.WriteLine($"{rowView["OrderID"],-10} {((DateTime)rowView["OrderDate"]):dd.MM.yyyy} {rowView["Total"],-10:C}");
                }
            }

            // 3. Фильтрация: заказы после определенной даты
            Console.WriteLine("\n\n3. Фильтрация: заказы после 2024-01-01");
            DateTime filterDate = new DateTime(2024, 1, 1);

            DataView dateFilteredView = new DataView(orders);
            dateFilteredView.RowFilter = $"OrderDate > #{filterDate:MM/dd/yyyy}#";
            dateFilteredView.Sort = "OrderDate DESC";

            Console.WriteLine($"Заказы после {filterDate:dd.MM.yyyy}:");
            foreach (DataRowView rowView in dateFilteredView)
            {
                string customerName = "Неизвестно";
                DataRow[] parents = rowView.Row.GetParentRows(relation);
                if (parents.Length > 0)
                    customerName = parents[0]["CustomerName"].ToString();

                Console.WriteLine($"  • Заказ #{rowView["OrderID"]} от {((DateTime)rowView["OrderDate"]):dd.MM.yyyy} " +
                    $"(Клиент: {customerName}, Сумма: {rowView["Total"]:C})");
            }

            // 4. Фильтрация: заказы на сумму больше указанной
            Console.WriteLine("\n\n4. Фильтрация: заказы на сумму > 50000");
            decimal minAmount = 50000;

            DataView amountFilteredView = new DataView(orders);
            amountFilteredView.RowFilter = $"Total > {minAmount}";
            amountFilteredView.Sort = "Total DESC";

            Console.WriteLine($"Заказы на сумму больше {minAmount:C}:");
            foreach (DataRowView rowView in amountFilteredView)
            {
                Console.WriteLine($"  • Заказ #{rowView["OrderID"]}: {rowView["Total"]:C}");
            }

            // 5. Комбинированная фильтрация
            Console.WriteLine("\n\n5. Комбинированная фильтрация: заказы клиента 101 после 2024-01-01 на сумму > 50000");

            DataView combinedView = new DataView(orders);
            combinedView.RowFilter = $"CustomerID = 101 AND OrderDate > #{filterDate:MM/dd/yyyy}# AND Total > {minAmount}";
            combinedView.Sort = "Total DESC, OrderDate DESC";

            Console.WriteLine("Результаты:");
            if (combinedView.Count == 0)
            {
                Console.WriteLine("  Нет заказов, удовлетворяющих всем критериям");
            }
            else
            {
                foreach (DataRowView rowView in combinedView)
                {
                    Console.WriteLine($"  • Заказ #{rowView["OrderID"]} от {((DateTime)rowView["OrderDate"]):dd.MM.yyyy} " +
                        $"на сумму {rowView["Total"]:C}");
                }
            }

            // 6. Использование GetChildRows с DataView
            Console.WriteLine("\n\n6. Использование GetChildRows() с DataView:");

            Console.WriteLine("Заказчики и их заказы через DataView:");
            DataView customersView = new DataView(customers);
            customersView.Sort = "CustomerName";

            foreach (DataRowView customerView in customersView)
            {
                Console.WriteLine($"\n{customerView["CustomerName"]}:");

                DataRow[] customerOrders = customerView.Row.GetChildRows(relation);
                if (customerOrders.Length == 0)
                {
                    Console.WriteLine("  Нет заказов");
                }
                else
                {
                    // Создаем DataView для заказов этого клиента
                    DataView customerOrdersView = new DataView(orders);
                    customerOrdersView.RowFilter = $"CustomerID = {customerView["CustomerID"]}";
                    customerOrdersView.Sort = "OrderDate DESC";

                    foreach (DataRowView orderView in customerOrdersView)
                    {
                        Console.WriteLine($"  • Заказ #{orderView["OrderID"]} от " +
                            $"{((DateTime)orderView["OrderDate"]):dd.MM.yyyy}: {orderView["Total"]:C}");
                    }
                }
            }

            // 7. Сложная фильтрация с подзапросами
            Console.WriteLine("\n\n7. Сложная фильтрация: заказчики с заказами > 50000");

            // Получаем ID заказчиков с крупными заказами
            var bigOrderCustomerIds = orders.Rows
                .Cast<DataRow>()
                .Where(r => r.RowState != DataRowState.Deleted && (decimal)r["Total"] > minAmount)
                .Select(r => (int)r["CustomerID"])
                .Distinct()
                .ToList();

            if (bigOrderCustomerIds.Count > 0)
            {
                string customerFilter = string.Join(" OR ", bigOrderCustomerIds.Select(id => $"CustomerID = {id}"));
                DataView bigOrderCustomersView = new DataView(customers);
                bigOrderCustomersView.RowFilter = customerFilter;

                Console.WriteLine("Заказчики с крупными заказами:");
                foreach (DataRowView customerView in bigOrderCustomersView)
                {
                    Console.WriteLine($"  • {customerView["CustomerName"]}");
                }
            }

            // 8. Сортировка с учетом связанных данных
            Console.WriteLine("\n\n8. Сортировка заказов с информацией о заказчиках:");

            DataView sortedOrdersView = new DataView(orders);
            sortedOrdersView.Sort = "CustomerID, Total DESC";

            Console.WriteLine($"{"Клиент",-20} {"Заказ",-10} {"Дата",-12} {"Сумма",-10}");
            Console.WriteLine(new string('-', 55));

            foreach (DataRowView orderView in sortedOrdersView)
            {
                string customerName = "Неизвестно";
                DataRow[] parents = orderView.Row.GetParentRows(relation);
                if (parents.Length > 0)
                    customerName = parents[0]["CustomerName"].ToString();

                Console.WriteLine($"{customerName,-20} #{orderView["OrderID"],-8} " +
                    $"{((DateTime)orderView["OrderDate"]):dd.MM.yyyy} {orderView["Total"],-10:C}");
            }
        }
        #endregion

        #region Задание 18: Экспорт иерархических данных
        static void Task18_HierarchicalExport()
        {
            Console.WriteLine("ЗАДАНИЕ 18: Экспорт иерархических данных с использованием DataRelation\n");
            Console.WriteLine("Цель: Экспорт данных в XML и другие форматы с сохранением иерархии\n");

            DataSet ds = CreateOrderManagementSystem();

            Console.WriteLine("1. Экспорт в XML (WriteXml):");

            string xmlFilePath = "orders_hierarchy.xml";
            ds.WriteXml(xmlFilePath, XmlWriteMode.WriteSchema);

            Console.WriteLine($"✓ Данные экспортированы в {xmlFilePath}");
            Console.WriteLine($"Размер файла: {new FileInfo(xmlFilePath).Length} байт");

            // 2. Экспорт без схемы
            Console.WriteLine("\n\n2. Экспорт в XML без схемы:");

            string xmlNoSchemaPath = "orders_data.xml";
            ds.WriteXml(xmlNoSchemaPath, XmlWriteMode.IgnoreSchema);

            Console.WriteLine($"✓ Данные экспортированы в {xmlNoSchemaPath}");

            // 3. Ручной обход иерархии
            Console.WriteLine("\n\n3. Ручной обход иерархии (рекурсивный метод):");

            string manualExportPath = "orders_manual.txt";
            using (StreamWriter writer = new StreamWriter(manualExportPath))
            {
                ExportHierarchyToText(ds, writer);
            }

            Console.WriteLine($"✓ Данные экспортированы в {manualExportPath}");

            // 4. Импорт из XML
            Console.WriteLine("\n\n4. Импорт данных из XML:");

            DataSet importedDs = new DataSet();
            importedDs.ReadXml(xmlFilePath);

            Console.WriteLine($"Импортировано таблиц: {importedDs.Tables.Count}");
            Console.WriteLine($"Импортировано отношений: {importedDs.Relations.Count}");

            // 5. Проверка восстановления отношений
            Console.WriteLine("\n\n5. Проверка восстановления отношений:");

            if (importedDs.Relations.Count > 0)
            {
                Console.WriteLine("Отношения восстановлены:");
                foreach (DataRelation relation in importedDs.Relations)
                {
                    Console.WriteLine($"  • {relation.RelationName}: " +
                        $"{relation.ParentTable.TableName}.{relation.ParentColumns[0].ColumnName} → " +
                        $"{relation.ChildTable.TableName}.{relation.ChildColumns[0].ColumnName}");
                }
            }

            // 6. Экспорт в CSV с иерархией
            Console.WriteLine("\n\n6. Экспорт в CSV с иерархией:");

            string csvPath = "orders_hierarchy.csv";
            ExportToCSV(ds, csvPath);

            Console.WriteLine($"✓ Данные экспортированы в {csvPath}");

            // 7. Экспорт в JSON-подобном формате
            Console.WriteLine("\n\n7. Экспорт в JSON-подобном формате:");

            string jsonPath = "orders_hierarchy.json";
            ExportToJSON(ds, jsonPath);

            Console.WriteLine($"✓ Данные экспортированы в {jsonPath}");

            // 8. Сравнение форматов
            Console.WriteLine("\n\n8. Сравнение форматов экспорта:");

            Console.WriteLine($"{"Формат",-15} {"Размер",-10} {"Сохраняет схему",-15} {"Читаемость"}");
            Console.WriteLine(new string('-', 60));

            var files = new[]
            {
                new { Format = "XML со схемой", Path = xmlFilePath },
                new { Format = "XML без схемы", Path = xmlNoSchemaPath },
                new { Format = "Текст", Path = manualExportPath },
                new { Format = "CSV", Path = csvPath },
                new { Format = "JSON", Path = jsonPath }
            };

            foreach (var file in files)
            {
                if (File.Exists(file.Path))
                {
                    long size = new FileInfo(file.Path).Length;
                    bool hasSchema = file.Format.Contains("со схемой");
                    string readability = file.Format.Contains("XML") ? "Средняя" :
                                       file.Format.Contains("JSON") ? "Высокая" : "Высокая";

                    Console.WriteLine($"{file.Format,-15} {size,-10} {hasSchema,-15} {readability}");
                }
            }
        }

        static void ExportHierarchyToText(DataSet ds, StreamWriter writer)
        {
            DataTable customers = ds.Tables["Заказчики"];
            DataRelation custOrderRel = ds.Relations["FK_Orders_Customers"];
            DataRelation orderDetailsRel = ds.Relations["FK_OrderDetails_Orders"];

            writer.WriteLine("=== ЭКСПОРТ ИЕРАРХИЧЕСКИХ ДАННЫХ ===");
            writer.WriteLine($"Время экспорта: {DateTime.Now}");
            writer.WriteLine();

            foreach (DataRow customer in customers.Rows)
            {
                writer.WriteLine($"ЗАКАЗЧИК: {customer["CustomerName"]}");
                writer.WriteLine($"Email: {customer["Email"]}");
                writer.WriteLine();

                DataRow[] orders = customer.GetChildRows(custOrderRel);
                if (orders.Length == 0)
                {
                    writer.WriteLine("  Нет заказов");
                }
                else
                {
                    writer.WriteLine("  ЗАКАЗЫ:");
                    foreach (DataRow order in orders)
                    {
                        writer.WriteLine($"  • Заказ #{order["OrderID"]}");
                        writer.WriteLine($"    Дата: {((DateTime)order["OrderDate"]):dd.MM.yyyy}");
                        writer.WriteLine($"    Сумма: {order["Total"]:C}");

                        DataRow[] details = order.GetChildRows(orderDetailsRel);
                        if (details.Length > 0)
                        {
                            writer.WriteLine("    Детали заказа:");
                            foreach (DataRow detail in details)
                            {
                                decimal total = (int)detail["Quantity"] * (decimal)detail["Price"];
                                writer.WriteLine($"      - Товар {detail["ProductID"]}: " +
                                    $"{detail["Quantity"]} × {detail["Price"]:C} = {total:C}");
                            }
                        }
                        writer.WriteLine();
                    }
                }
                writer.WriteLine(new string('=', 60));
                writer.WriteLine();
            }
        }

        static void ExportToCSV(DataSet ds, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                DataTable customers = ds.Tables["Заказчики"];
                DataRelation custOrderRel = ds.Relations["FK_Orders_Customers"];
                DataRelation orderDetailsRel = ds.Relations["FK_OrderDetails_Orders"];

                writer.WriteLine("CustomerID,CustomerName,Email,OrderID,OrderDate,Total,DetailID,ProductID,Quantity,Price,LineTotal");

                foreach (DataRow customer in customers.Rows)
                {
                    DataRow[] orders = customer.GetChildRows(custOrderRel);

                    if (orders.Length == 0)
                    {
                        // Заказчик без заказов
                        writer.WriteLine($"{customer["CustomerID"]},\"{customer["CustomerName"]}\",{customer["Email"]},,,,,,,");
                    }
                    else
                    {
                        foreach (DataRow order in orders)
                        {
                            DataRow[] details = order.GetChildRows(orderDetailsRel);

                            if (details.Length == 0)
                            {
                                // Заказ без деталей
                                writer.WriteLine($"{customer["CustomerID"]},\"{customer["CustomerName"]}\",{customer["Email"]}," +
                                    $"{order["OrderID"]},{((DateTime)order["OrderDate"]):yyyy-MM-dd},{order["Total"]},,,,,,");
                            }
                            else
                            {
                                foreach (DataRow detail in details)
                                {
                                    decimal lineTotal = (int)detail["Quantity"] * (decimal)detail["Price"];
                                    writer.WriteLine($"{customer["CustomerID"]},\"{customer["CustomerName"]}\",{customer["Email"]}," +
                                        $"{order["OrderID"]},{((DateTime)order["OrderDate"]):yyyy-MM-dd},{order["Total"]}," +
                                        $"{detail["DetailID"]},{detail["ProductID"]},{detail["Quantity"]},{detail["Price"]},{lineTotal}");
                                }
                            }
                        }
                    }
                }
            }
        }

        static void ExportToJSON(DataSet ds, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                DataTable customers = ds.Tables["Заказчики"];
                DataRelation custOrderRel = ds.Relations["FK_Orders_Customers"];
                DataRelation orderDetailsRel = ds.Relations["FK_OrderDetails_Orders"];

                writer.WriteLine("[");

                bool firstCustomer = true;
                foreach (DataRow customer in customers.Rows)
                {
                    if (!firstCustomer) writer.WriteLine(",");
                    firstCustomer = false;

                    writer.WriteLine("  {");
                    writer.WriteLine($"    \"customerId\": {customer["CustomerID"]},");
                    writer.WriteLine($"    \"customerName\": \"{customer["CustomerName"]}\",");
                    writer.WriteLine($"    \"email\": \"{customer["Email"]}\",");

                    DataRow[] orders = customer.GetChildRows(custOrderRel);
                    writer.WriteLine("    \"orders\": [");

                    bool firstOrder = true;
                    foreach (DataRow order in orders)
                    {
                        if (!firstOrder) writer.WriteLine(",");
                        firstOrder = false;

                        writer.WriteLine("      {");
                        writer.WriteLine($"        \"orderId\": {order["OrderID"]},");
                        writer.WriteLine($"        \"orderDate\": \"{((DateTime)order["OrderDate"]):yyyy-MM-dd}\",");
                        writer.WriteLine($"        \"total\": {order["Total"]},");

                        DataRow[] details = order.GetChildRows(orderDetailsRel);
                        writer.WriteLine("        \"orderDetails\": [");

                        bool firstDetail = true;
                        foreach (DataRow detail in details)
                        {
                            if (!firstDetail) writer.WriteLine(",");
                            firstDetail = false;

                            decimal lineTotal = (int)detail["Quantity"] * (decimal)detail["Price"];
                            writer.WriteLine("          {");
                            writer.WriteLine($"            \"detailId\": {detail["DetailID"]},");
                            writer.WriteLine($"            \"productId\": {detail["ProductID"]},");
                            writer.WriteLine($"            \"quantity\": {detail["Quantity"]},");
                            writer.WriteLine($"            \"price\": {detail["Price"]},");
                            writer.WriteLine($"            \"lineTotal\": {lineTotal}");
                            writer.Write("          }");
                        }

                        writer.WriteLine("\n        ]");
                        writer.Write("      }");
                    }

                    writer.WriteLine("\n    ]");
                    writer.Write("  }");
                }

                writer.WriteLine("\n]");
            }
        }
        #endregion

        #region Задание 19: Комплексное образовательное учреждение
        static void Task19_ComplexEducationalSystem()
        {
            Console.WriteLine("ЗАДАНИЕ 19: Комплексное приложение: управление образовательным учреждением\n");
            Console.WriteLine("Цель: Реализация полной системы с использованием всех изученных концепций\n");

            DataSet ds = CreateEducationalSystem();

            if (ds == null || ds.Tables.Count == 0)
            {
                Console.WriteLine("Ошибка при создании образовательной системы");
                return;
            }

            DataTable faculties = ds.Tables["Факультеты"];
            DataTable specialties = ds.Tables["Специальности"];
            DataTable students = ds.Tables["Студенты"];
            DataTable subjects = ds.Tables["Предметы"];
            DataTable grades = ds.Tables["Оценки"];

            Console.WriteLine("1. Структура образовательной системы:");
            Console.WriteLine($"   Факультетов: {faculties?.Rows.Count ?? 0}");
            Console.WriteLine($"   Специальностей: {specialties?.Rows.Count ?? 0}");
            Console.WriteLine($"   Студентов: {students?.Rows.Count ?? 0}");
            Console.WriteLine($"   Предметов: {subjects?.Rows.Count ?? 0}");
            Console.WriteLine($"   Оценок: {grades?.Rows.Count ?? 0}");

            // Проверяем наличие необходимых отношений
            if (!ds.Relations.Contains("FK_Specialties_Faculties") ||
                !ds.Relations.Contains("FK_Students_Specialties") ||
                !ds.Relations.Contains("FK_Grades_Students") ||
                !ds.Relations.Contains("FK_Grades_Subjects"))
            {
                Console.WriteLine("Внимание: не все отношения созданы!");
                return;
            }

            // 2. Добавление нового факультета с каскадным удалением
            Console.WriteLine("\n\n2. Добавление и удаление факультета:");

            // Ищем следующий свободный ID для факультета и специальности
            int nextFacultyId = GetNextAvailableId(faculties, "FacultyID");
            int nextSpecialtyId = GetNextAvailableId(specialties, "SpecialtyID");

            // Добавляем новый факультет
            DataRow newFaculty = faculties.NewRow();
            newFaculty["FacultyID"] = nextFacultyId;
            newFaculty["FacultyName"] = "Юридический";
            faculties.Rows.Add(newFaculty);

            // Добавляем специальность
            DataRow newSpecialty = specialties.NewRow();
            newSpecialty["SpecialtyID"] = nextSpecialtyId;
            newSpecialty["SpecialtyName"] = "Юриспруденция";
            newSpecialty["FacultyID"] = nextFacultyId;
            specialties.Rows.Add(newSpecialty);

            Console.WriteLine($"Добавлен факультет: {newFaculty["FacultyName"]} (ID: {newFaculty["FacultyID"]})");
            Console.WriteLine($"Добавлена специальность: {newSpecialty["SpecialtyName"]} (ID: {newSpecialty["SpecialtyID"]})");

            // Показываем текущее состояние
            Console.WriteLine("\nТекущее состояние перед удалением:");
            Console.WriteLine("Факультеты:");
            foreach (DataRow faculty in faculties.Rows)
            {
                if (faculty.RowState != DataRowState.Deleted)
                    Console.WriteLine($"  [{faculty["FacultyID"]}] {faculty["FacultyName"]}");
            }

            Console.WriteLine("\nСпециальности:");
            foreach (DataRow specialty in specialties.Rows)
            {
                if (specialty.RowState != DataRowState.Deleted)
                {
                    DataRow[] facultyParents = specialty.GetParentRows("FK_Specialties_Faculties");
                    string facultyName = facultyParents.Length > 0 ? facultyParents[0]["FacultyName"].ToString() : "Нет факультета";
                    Console.WriteLine($"  [{specialty["SpecialtyID"]}] {specialty["SpecialtyName"]} (Факультет: {facultyName})");
                }
            }

            // Удаляем факультет (каскадное удаление специальностей)
            newFaculty.Delete();
            Console.WriteLine($"\nУдаляем факультет 'Юридический' (ID: {nextFacultyId}) - ожидается каскадное удаление специальностей...");

            // Подсчитываем оставшиеся специальности
            int remainingSpecialties = 0;
            int deletedSpecialties = 0;

            foreach (DataRow row in specialties.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                    remainingSpecialties++;
                else if (row.HasVersion(DataRowVersion.Original))
                    deletedSpecialties++;
            }

            Console.WriteLine($"\nРезультат после удаления:");
            Console.WriteLine($"  Удалено специальностей: {deletedSpecialties}");
            Console.WriteLine($"  Осталось специальностей: {remainingSpecialties}");

            // Проверяем, удалилась ли добавленная специальность
            DataRow[] deletedSpecialtyRows = specialties.Select(null, null, DataViewRowState.Deleted);
            bool ourSpecialtyDeleted = false;

            foreach (DataRow deletedRow in deletedSpecialtyRows)
            {
                if (deletedRow.HasVersion(DataRowVersion.Original) &&
                    (int)deletedRow["SpecialtyID", DataRowVersion.Original] == nextSpecialtyId)
                {
                    ourSpecialtyDeleted = true;
                    Console.WriteLine($"  ✓ Наша специальность 'Юриспруденция' (ID: {nextSpecialtyId}) удалена каскадно");
                    break;
                }
            }

            if (!ourSpecialtyDeleted)
            {
                Console.WriteLine($"  ✗ Наша специальность 'Юриспруденция' (ID: {nextSpecialtyId}) не удалена");
            }

            // Откатываем изменения
            Console.WriteLine("\nОткатываем изменения для продолжения тестов...");
            ds.RejectChanges();

            // Показываем восстановленное состояние
            Console.WriteLine("\nВосстановленное состояние после отката:");
            Console.WriteLine($"Факультетов: {faculties.Rows.Count}");
            Console.WriteLine($"Специальностей: {specialties.Rows.Count}");

            // Вспомогательный метод для поиска следующего свободного ID
            static int GetNextAvailableId(DataTable table, string idColumnName)
            {
                if (!table.Columns.Contains(idColumnName))
                    return 1;

                // Получаем все существующие ID (исключая удаленные)
                var existingIds = new HashSet<int>();
                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState != DataRowState.Deleted)
                    {
                        existingIds.Add((int)row[idColumnName]);
                    }
                }

                // Находим следующий свободный ID
                int nextId = 1;
                while (existingIds.Contains(nextId))
                {
                    nextId++;
                }

                return nextId;
            }

            // 3. Получение всех студентов факультета
            Console.WriteLine("\n\n3. Студенты факультета 'Информационные технологии':");
            DataRow[] itFacultyRows = faculties.Select("FacultyName = 'Информационные технологии'");

            if (itFacultyRows.Length > 0)
            {
                DataRow itFaculty = itFacultyRows[0];
                DataRow[] facultySpecialties = itFaculty.GetChildRows("FK_Specialties_Faculties");

                Console.WriteLine($"Специальности факультета '{itFaculty["FacultyName"]}':");
                int totalStudents = 0;

                foreach (DataRow specialty in facultySpecialties)
                {
                    DataRow[] specialtyStudents = specialty.GetChildRows("FK_Students_Specialties");
                    totalStudents += specialtyStudents.Length;

                    Console.WriteLine($"\n  Специальность: {specialty["SpecialtyName"]}");
                    Console.WriteLine($"  Студентов: {specialtyStudents.Length}");

                    if (specialtyStudents.Length > 0)
                    {
                        foreach (DataRow student in specialtyStudents)
                        {
                            Console.WriteLine($"    • {student["StudentName"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("    Нет студентов");
                    }
                }

                Console.WriteLine($"\nВсего студентов на факультете: {totalStudents}");
            }

            // 4. Получение всех оценок студента
            Console.WriteLine("\n\n4. Оценки студента 'Иван Петров':");
            DataRow[] ivanRows = students.Select("StudentName = 'Иван Петров'");

            if (ivanRows.Length > 0)
            {
                DataRow student = ivanRows[0];
                DataRow[] studentGrades = student.GetChildRows("FK_Grades_Students");

                Console.WriteLine($"Оценки студента {student["StudentName"]}:");

                if (studentGrades.Length > 0)
                {
                    foreach (DataRow grade in studentGrades)
                    {
                        DataRow[] subjectRows = grade.GetParentRows("FK_Grades_Subjects");
                        if (subjectRows.Length > 0)
                        {
                            Console.WriteLine($"  • {subjectRows[0]["SubjectName"]}: {grade["Grade"]} " +
                                $"(Дата: {((DateTime)grade["Date"]):dd.MM.yyyy})");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("  Нет оценок");
                }
            }

            // 5. Расчет среднего балла
            Console.WriteLine("\n\n5. Расчет средних баллов:");

            // Средний балл студентов
            Console.WriteLine("\nСредний балл студентов:");
            foreach (DataRow stud in students.Rows)
            {
                DataRow[] studentGrades = stud.GetChildRows("FK_Grades_Students");
                if (studentGrades.Length > 0)
                {
                    double avg = studentGrades.Average(g => (double)g["Grade"]);
                    Console.WriteLine($"  • {stud["StudentName"]}: {avg:F2} ({studentGrades.Length} оценок)");
                }
                else
                {
                    Console.WriteLine($"  • {stud["StudentName"]}: нет оценок");
                }
            }

            // Средний балл по специальностям
            Console.WriteLine("\nСредний балл по специальностям:");
            foreach (DataRow specialty in specialties.Rows)
            {
                DataRow[] specialtyStudents = specialty.GetChildRows("FK_Students_Specialties");
                List<double> allGrades = new List<double>();

                foreach (DataRow stud in specialtyStudents)
                {
                    DataRow[] studentGrades = stud.GetChildRows("FK_Grades_Students");
                    allGrades.AddRange(studentGrades.Select(g => (double)g["Grade"]));
                }

                if (allGrades.Count > 0)
                {
                    double avg = allGrades.Average();
                    Console.WriteLine($"  • {specialty["SpecialtyName"]}: {avg:F2} " +
                        $"(оценок: {allGrades.Count}, студентов: {specialtyStudents.Length})");
                }
                else
                {
                    Console.WriteLine($"  • {specialty["SpecialtyName"]}: нет оценок");
                }
            }

            // Средний балл по факультетам
            Console.WriteLine("\nСредний балл по факультетам:");
            foreach (DataRow faculty in faculties.Rows)
            {
                DataRow[] facultySpecialties = faculty.GetChildRows("FK_Specialties_Faculties");
                List<double> allGrades = new List<double>();
                int totalStudents = 0;

                foreach (DataRow specialty in facultySpecialties)
                {
                    DataRow[] specialtyStudents = specialty.GetChildRows("FK_Students_Specialties");
                    totalStudents += specialtyStudents.Length;

                    foreach (DataRow stud in specialtyStudents)
                    {
                        DataRow[] studentGrades = stud.GetChildRows("FK_Grades_Students");
                        allGrades.AddRange(studentGrades.Select(g => (double)g["Grade"]));
                    }
                }

                if (allGrades.Count > 0)
                {
                    double avg = allGrades.Average();
                    Console.WriteLine($"  • {faculty["FacultyName"]}: {avg:F2} " +
                        $"(оценок: {allGrades.Count}, студентов: {totalStudents})");
                }
                else
                {
                    Console.WriteLine($"  • {faculty["FacultyName"]}: нет оценок");
                }
            }

            // 6. Отслеживание изменений
            Console.WriteLine("\n\n6. Отслеживание всех изменений:");

            // Вносим изменения
            DataRow gradeToModify = grades.Rows.Find(1);
            if (gradeToModify != null)
            {
                double oldGrade = (double)gradeToModify["Grade"];
                gradeToModify["Grade"] = 5.0;
                Console.WriteLine($"  Изменена оценка с {oldGrade} на 5.0");
            }
            else
            {
                Console.WriteLine("  Оценка с ID 1 не найдена");
            }

            DataRow studentToModify = students.Rows.Find(101);
            if (studentToModify != null)
            {
                string oldName = studentToModify["StudentName"].ToString();
                studentToModify["StudentName"] = "Иван Петров (модифицирован)";
                Console.WriteLine($"  Изменено имя студента с '{oldName}' на '{studentToModify["StudentName"]}'");
            }
            else
            {
                Console.WriteLine("  Студент с ID 101 не найден");
            }

            // Добавляем нового студента - проверяем существование специальности
            Console.WriteLine("\n  Попытка добавить нового студента...");
            int nextStudentId = GetNextAvailableId(students, "StudentID");

            // Проверяем существование специальности 101
            DataRow specialty101 = specialties.Rows.Find(101);
            if (specialty101 != null && specialty101.RowState != DataRowState.Deleted)
            {
                DataRow newStudent = students.NewRow();
                newStudent["StudentID"] = nextStudentId;
                newStudent["StudentName"] = "Новый студент";
                newStudent["SpecialtyID"] = 101;
                students.Rows.Add(newStudent);
                Console.WriteLine($"    ✓ Добавлен новый студент: {newStudent["StudentName"]} (ID: {newStudent["StudentID"]}, Специальность: 101)");
            }
            else
            {
                // Если специальность 101 не существует, используем первую доступную специальность
                DataRow firstAvailableSpecialty = null;
                foreach (DataRow specialty in specialties.Rows)
                {
                    if (specialty.RowState != DataRowState.Deleted)
                    {
                        firstAvailableSpecialty = specialty;
                        break;
                    }
                }

                if (firstAvailableSpecialty != null)
                {
                    DataRow newStudent = students.NewRow();
                    newStudent["StudentID"] = nextStudentId;
                    newStudent["StudentName"] = "Новый студент";
                    newStudent["SpecialtyID"] = firstAvailableSpecialty["SpecialtyID"];
                    students.Rows.Add(newStudent);
                    Console.WriteLine($"    ✓ Добавлен новый студент: {newStudent["StudentName"]} (ID: {newStudent["StudentID"]}, Специальность: {firstAvailableSpecialty["SpecialtyID"]})");
                }
                else
                {
                    Console.WriteLine("    ✗ Не удалось добавить студента: нет доступных специальностей");
                }
            }

            // Пытаемся удалить студента
            DataRow studentToDelete = students.Rows.Find(104);
            if (studentToDelete != null)
            {
                Console.WriteLine($"  Попытка удалить студента с ID 104...");

                // Проверяем, есть ли у студента оценки
                DataRow[] studentGrades = studentToDelete.GetChildRows("FK_Grades_Students");
                if (studentGrades.Length > 0)
                {
                    Console.WriteLine($"    ✗ Нельзя удалить студента: у него {studentGrades.Length} оценок (ожидается каскадное удаление)");

                    // Показываем оценки студента
                    Console.WriteLine("    Оценки студента:");
                    foreach (DataRow grade in studentGrades)
                    {
                        DataRow[] subjectParents = grade.GetParentRows("FK_Grades_Subjects");
                        string subjectName = subjectParents.Length > 0 ? subjectParents[0]["SubjectName"].ToString() : "Неизвестно";
                        Console.WriteLine($"      • {subjectName}: {grade["Grade"]}");
                    }

                    // Временно отключаем проверку ограничений для демонстрации каскадного удаления
                    bool originalEnforce = ds.EnforceConstraints;
                    ds.EnforceConstraints = false;

                    try
                    {
                        studentToDelete.Delete();
                        Console.WriteLine($"    ✓ Студент удален (оценки будут удалены каскадно при сохранении)");

                        // Проверяем, что оценки помечены на удаление
                        int deletedGradesCount = 0;
                        foreach (DataRow grade in studentGrades)
                        {
                            if (grade.RowState == DataRowState.Deleted)
                                deletedGradesCount++;
                        }
                        Console.WriteLine($"    Помечено на удаление оценок: {deletedGradesCount}");
                    }
                    finally
                    {
                        ds.EnforceConstraints = originalEnforce;
                    }
                }
                else
                {
                    studentToDelete.Delete();
                    Console.WriteLine($"    ✓ Студент удален (у него не было оценок)");
                }
            }
            else
            {
                Console.WriteLine("  Студент с ID 104 не найден");
            }

            // Анализируем изменения
            Console.WriteLine("\nИзмененные строки по таблицам:");

            string[] tableNames = { "Факультеты", "Специальности", "Студенты", "Предметы", "Оценки" };
            int totalChanges = 0;

            foreach (string tableName in tableNames)
            {
                if (ds.Tables.Contains(tableName))
                {
                    DataTable table = ds.Tables[tableName];

                    // Используем безопасный способ подсчета изменений
                    int added = 0, modified = 0, deleted = 0;

                    foreach (DataRow row in table.Rows)
                    {
                        switch (row.RowState)
                        {
                            case DataRowState.Added:
                                added++;
                                break;
                            case DataRowState.Modified:
                                modified++;
                                break;
                            case DataRowState.Deleted:
                                deleted++;
                                break;
                        }
                    }

                    totalChanges += added + modified + deleted;

                    if (added > 0 || modified > 0 || deleted > 0)
                    {
                        Console.WriteLine($"  {tableName}: Добавлено={added}, Изменено={modified}, Удалено={deleted}");

                        // Детализация для отладки
                        if (tableName == "Оценки" && deleted > 0)
                        {
                            Console.WriteLine("    Удаленные оценки:");
                            foreach (DataRow row in table.Rows)
                            {
                                if (row.RowState == DataRowState.Deleted && row.HasVersion(DataRowVersion.Original))
                                {
                                    Console.WriteLine($"      • ID {row["GradeID", DataRowVersion.Original]}, " +
                                        $"Студент {row["StudentID", DataRowVersion.Original]}, " +
                                        $"Оценка {row["Grade", DataRowVersion.Original]}");
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Всего изменений: {totalChanges}");

            // Проверяем состояние constraints
            Console.WriteLine($"\nПроверка ограничений включена: {ds.EnforceConstraints}");

            // 7. Валидация ссылочной целостности
            Console.WriteLine("\n\n7. Валидация ссылочной целостности:");

            bool integrityValid = true;
            List<string> integrityIssues = new List<string>();

            // Проверяем студентов без специальностей
            foreach (DataRow stud in students.Rows)
            {
                if (stud.RowState != DataRowState.Deleted)
                {
                    DataRow[] parents = stud.GetParentRows("FK_Students_Specialties");
                    if (parents.Length == 0)
                    {
                        integrityIssues.Add($"Студент {stud["StudentName"]} без специальности");
                        integrityValid = false;
                    }
                }
            }

            // Проверяем оценки без студентов или предметов
            foreach (DataRow grade in grades.Rows)
            {
                if (grade.RowState != DataRowState.Deleted)
                {
                    DataRow[] studentParents = grade.GetParentRows("FK_Grades_Students");
                    DataRow[] subjectParents = grade.GetParentRows("FK_Grades_Subjects");

                    if (studentParents.Length == 0)
                    {
                        integrityIssues.Add($"Оценка {grade["GradeID"]} без студента");
                        integrityValid = false;
                    }

                    if (subjectParents.Length == 0)
                    {
                        integrityIssues.Add($"Оценка {grade["GradeID"]} без предмета");
                        integrityValid = false;
                    }
                }
            }

            if (integrityValid)
            {
                Console.WriteLine("✓ Целостность данных сохранена");
            }
            else
            {
                Console.WriteLine("✗ Найдены проблемы целостности:");
                foreach (var issue in integrityIssues)
                {
                    Console.WriteLine($"  • {issue}");
                }
            }

            // 8. Финальные отчеты
            Console.WriteLine("\n\n8. Финальные отчеты:");

            Console.WriteLine("\nОтчет по факультетам:");
            Console.WriteLine($"{"Факультет",-30} {"Специальностей",-15} {"Студентов",-10} {"Средний балл",-12}");
            Console.WriteLine(new string('-', 70));

            foreach (DataRow faculty in faculties.Rows)
            {
                DataRow[] facultySpecialties = faculty.GetChildRows("FK_Specialties_Faculties");
                int studentCount = 0;
                List<double> allGrades = new List<double>();

                foreach (DataRow specialty in facultySpecialties)
                {
                    DataRow[] specialtyStudents = specialty.GetChildRows("FK_Students_Specialties");
                    studentCount += specialtyStudents.Length;

                    foreach (DataRow stud in specialtyStudents)
                    {
                        DataRow[] studentGrades = stud.GetChildRows("FK_Grades_Students");
                        allGrades.AddRange(studentGrades.Select(g => (double)g["Grade"]));
                    }
                }

                double avgGrade = allGrades.Count > 0 ? allGrades.Average() : 0;
                Console.WriteLine($"{faculty["FacultyName"],-30} {facultySpecialties.Length,-15} " +
                    $"{studentCount,-10} {avgGrade,12:F2}");
            }

            Console.WriteLine("\nОтчет по студентам:");
            Console.WriteLine($"{"Студент",-25} {"Специальность",-20} {"Оценок",-8} {"Средний балл",-12}");
            Console.WriteLine(new string('-', 70));

            foreach (DataRow stud in students.Rows)
            {
                if (stud.RowState != DataRowState.Deleted)
                {
                    DataRow[] studentGrades = stud.GetChildRows("FK_Grades_Students");
                    double avgGrade = studentGrades.Length > 0 ?
                        studentGrades.Average(g => (double)g["Grade"]) : 0;

                    string specialtyName = "Неизвестно";
                    DataRow[] specialtyParents = stud.GetParentRows("FK_Students_Specialties");
                    if (specialtyParents.Length > 0)
                        specialtyName = specialtyParents[0]["SpecialtyName"].ToString();

                    Console.WriteLine($"{stud["StudentName"],-25} {specialtyName,-20} " +
                        $"{studentGrades.Length,-8} {avgGrade,12:F2}");
                }
            }

            Console.WriteLine("\nОтчет об изменениях перед сохранением:");
            int pendingChanges = 0;
            foreach (DataTable table in ds.Tables)
            {
                int tableChanges = table.Rows.Cast<DataRow>()
                    .Count(r => r.RowState != DataRowState.Unchanged);

                if (tableChanges > 0)
                {
                    Console.WriteLine($"  {table.TableName}: {tableChanges} изменений");
                    pendingChanges += tableChanges;
                }
            }

            if (pendingChanges == 0)
                Console.WriteLine("  Нет ожидающих изменений");
            else
                Console.WriteLine($"Всего ожидающих изменений: {pendingChanges}");

            // 9. Сохранение изменений
            Console.WriteLine("\n\n9. Сохранение изменений:");

            if (pendingChanges > 0)
            {
                Console.WriteLine("Применяем изменения...");
                ds.AcceptChanges();
                Console.WriteLine("✓ Изменения сохранены");
            }

            // 10. Финальная статистика
            Console.WriteLine("\n\n10. Финальная статистика системы:");
            Console.WriteLine($"Факультетов: {faculties.Rows.Count}");
            Console.WriteLine($"Специальностей: {specialties.Rows.Count}");
            Console.WriteLine($"Студентов: {students.Rows.Count}");
            Console.WriteLine($"Предметов: {subjects.Rows.Count}");
            Console.WriteLine($"Оценок: {grades.Rows.Count}");
            Console.WriteLine($"Отношений в DataSet: {ds.Relations.Count}");
        }

        static DataSet CreateEducationalSystem()
        {
            try
            {
                Console.WriteLine("\nСоздание образовательной системы...");

                DataSet ds = new DataSet("EducationalSystem");

                // 1. Факультеты
                DataTable faculties = new DataTable("Факультеты");
                faculties.Columns.Add("FacultyID", typeof(int));
                faculties.Columns.Add("FacultyName", typeof(string));
                faculties.PrimaryKey = new DataColumn[] { faculties.Columns["FacultyID"] };

                // 2. Специальности
                DataTable specialties = new DataTable("Специальности");
                specialties.Columns.Add("SpecialtyID", typeof(int));
                specialties.Columns.Add("SpecialtyName", typeof(string));
                specialties.Columns.Add("FacultyID", typeof(int));
                specialties.PrimaryKey = new DataColumn[] { specialties.Columns["SpecialtyID"] };

                // 3. Студенты
                DataTable students = new DataTable("Студенты");
                students.Columns.Add("StudentID", typeof(int));
                students.Columns.Add("StudentName", typeof(string));
                students.Columns.Add("SpecialtyID", typeof(int));
                students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

                // 4. Предметы
                DataTable subjects = new DataTable("Предметы");
                subjects.Columns.Add("SubjectID", typeof(int));
                subjects.Columns.Add("SubjectName", typeof(string));
                subjects.PrimaryKey = new DataColumn[] { subjects.Columns["SubjectID"] };

                // 5. Оценки
                DataTable grades = new DataTable("Оценки");
                grades.Columns.Add("GradeID", typeof(int));
                grades.Columns.Add("StudentID", typeof(int));
                grades.Columns.Add("SubjectID", typeof(int));
                grades.Columns.Add("Grade", typeof(double));
                grades.Columns.Add("Date", typeof(DateTime));
                grades.PrimaryKey = new DataColumn[] { grades.Columns["GradeID"] };

                // Добавляем таблицы в DataSet
                ds.Tables.Add(faculties);
                ds.Tables.Add(specialties);
                ds.Tables.Add(students);
                ds.Tables.Add(subjects);
                ds.Tables.Add(grades);

                Console.WriteLine("✓ Таблицы созданы");

                // Заполняем данными
                // Факультеты
                faculties.Rows.Add(1, "Информационные технологии");
                faculties.Rows.Add(2, "Экономический");
                faculties.Rows.Add(3, "Гуманитарный");

                // Специальности
                specialties.Rows.Add(101, "Программирование", 1);
                specialties.Rows.Add(102, "Сетевые технологии", 1);
                specialties.Rows.Add(103, "Финансы", 2);
                specialties.Rows.Add(104, "Менеджмент", 2);

                // Студенты
                students.Rows.Add(101, "Иван Петров", 101);
                students.Rows.Add(102, "Мария Сидорова", 101);
                students.Rows.Add(103, "Петр Иванов", 102);
                students.Rows.Add(104, "Анна Смирнова", 103);

                // Предметы
                subjects.Rows.Add(1, "C# Programming");
                subjects.Rows.Add(2, "Database Design");
                subjects.Rows.Add(3, "Web Development");
                subjects.Rows.Add(4, "Economics");

                // Оценки
                grades.Rows.Add(1, 101, 1, 4.5, new DateTime(2024, 1, 15));
                grades.Rows.Add(2, 101, 2, 4.0, new DateTime(2024, 1, 20));
                grades.Rows.Add(3, 101, 3, 4.8, new DateTime(2024, 2, 5));
                grades.Rows.Add(4, 102, 1, 4.2, new DateTime(2024, 1, 15));
                grades.Rows.Add(5, 102, 2, 4.5, new DateTime(2024, 1, 20));
                grades.Rows.Add(6, 103, 3, 3.8, new DateTime(2024, 2, 5));
                grades.Rows.Add(7, 104, 4, 4.9, new DateTime(2024, 1, 10));

                Console.WriteLine("✓ Данные заполнены");

                // Создаем ForeignKeyConstraints для управления целостностью

                // Отношение 1: Факультеты → Специальности
                ForeignKeyConstraint fkFacSpec = new ForeignKeyConstraint(
                    "FK_Specialties_Faculties_Constraint",
                    faculties.Columns["FacultyID"],
                    specialties.Columns["FacultyID"])
                {
                    DeleteRule = Rule.Cascade,    // Каскадное удаление
                    UpdateRule = Rule.Cascade     // Каскадное обновление
                };
                specialties.Constraints.Add(fkFacSpec);
                Console.WriteLine("✓ Создан constraint: Факультеты → Специальности (Cascade)");

                // Отношение 2: Специальности → Студенты
                ForeignKeyConstraint fkSpecStud = new ForeignKeyConstraint(
                    "FK_Students_Specialties_Constraint",
                    specialties.Columns["SpecialtyID"],
                    students.Columns["SpecialtyID"])
                {
                    DeleteRule = Rule.SetNull,    // При удалении специальности устанавливаем NULL у студентов
                    UpdateRule = Rule.Cascade     // Каскадное обновление
                };
                students.Constraints.Add(fkSpecStud);
                Console.WriteLine("✓ Создан constraint: Специальности → Студенты (SetNull/Cascade)");

                // Отношение 3: Студенты → Оценки
                ForeignKeyConstraint fkStudGrade = new ForeignKeyConstraint(
                    "FK_Grades_Students_Constraint",
                    students.Columns["StudentID"],
                    grades.Columns["StudentID"])
                {
                    DeleteRule = Rule.Cascade,    // Каскадное удаление
                    UpdateRule = Rule.Cascade     // Каскадное обновление
                };
                grades.Constraints.Add(fkStudGrade);
                Console.WriteLine("✓ Создан constraint: Студенты → Оценки (Cascade)");

                // Отношение 4: Предметы → Оценки
                ForeignKeyConstraint fkSubjGrade = new ForeignKeyConstraint(
                    "FK_Grades_Subjects_Constraint",
                    subjects.Columns["SubjectID"],
                    grades.Columns["SubjectID"])
                {
                    DeleteRule = Rule.Cascade,    // Каскадное удаление
                    UpdateRule = Rule.Cascade     // Каскадное обновление
                };
                grades.Constraints.Add(fkSubjGrade);
                Console.WriteLine("✓ Создан constraint: Предметы → Оценки (Cascade)");

                // Создаем DataRelations для навигации (без создания constraints)
                DataRelation facSpecRel = new DataRelation(
                    "FK_Specialties_Faculties",
                    faculties.Columns["FacultyID"],
                    specialties.Columns["FacultyID"],
                    false); // false - не создавать constraints

                DataRelation specStudRel = new DataRelation(
                    "FK_Students_Specialties",
                    specialties.Columns["SpecialtyID"],
                    students.Columns["SpecialtyID"],
                    false);

                DataRelation studGradeRel = new DataRelation(
                    "FK_Grades_Students",
                    students.Columns["StudentID"],
                    grades.Columns["StudentID"],
                    false);

                DataRelation subjGradeRel = new DataRelation(
                    "FK_Grades_Subjects",
                    subjects.Columns["SubjectID"],
                    grades.Columns["SubjectID"],
                    false);

                // Добавляем отношения в DataSet
                ds.Relations.Add(facSpecRel);
                ds.Relations.Add(specStudRel);
                ds.Relations.Add(studGradeRel);
                ds.Relations.Add(subjGradeRel);

                Console.WriteLine("✓ Созданы 4 отношения для навигации");
                Console.WriteLine("✓ Образовательная система успешно создана!\n");

                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка при создании образовательной системы: {ex.Message}");
                Console.WriteLine($"Детали: {ex.StackTrace}");
                return null;
            }
        }
        #endregion

        #region Задание 20: Оптимизация производительности
        static void Task20_PerformanceOptimization()
        {
            Console.WriteLine("ЗАДАНИЕ 20: Оптимизация и производительность при работе с DataRelation на больших объёмах данных\n");
            Console.WriteLine("Цель: Сравнение различных подходов к работе с отношениями\n");

            Console.WriteLine("Создание тестового набора данных...");
            DataSet largeDs = CreateLargeDataSet();

            DataTable companies = largeDs.Tables["Компании"];
            DataTable departments = largeDs.Tables["Отделы"];
            DataTable employees = largeDs.Tables["Сотрудники"];

            DataRelation compDeptRel = largeDs.Relations["FK_Departments_Companies"];
            DataRelation deptEmpRel = largeDs.Relations["FK_Employees_Departments"];

            Console.WriteLine($"Тестовые данные созданы:");
            Console.WriteLine($"  Компаний: {companies.Rows.Count}");
            Console.WriteLine($"  Отделов: {departments.Rows.Count}");
            Console.WriteLine($"  Сотрудников: {employees.Rows.Count}");

            // 1. Подход 1: GetChildRows в цикле
            Console.WriteLine("\n\n1. Подход 1: GetChildRows() в цикле");
            var stopwatch1 = Stopwatch.StartNew();

            int totalEmployees1 = 0;
            foreach (DataRow company in companies.Rows)
            {
                DataRow[] companyDepartments = company.GetChildRows(compDeptRel);
                foreach (DataRow department in companyDepartments)
                {
                    DataRow[] departmentEmployees = department.GetChildRows(deptEmpRel);
                    totalEmployees1 += departmentEmployees.Length;
                }
            }

            stopwatch1.Stop();
            Console.WriteLine($"  Результат: {totalEmployees1} сотрудников");
            Console.WriteLine($"  Время: {stopwatch1.ElapsedMilliseconds} мс");

            // 2. Подход 2: DataView с фильтром
            Console.WriteLine("\n2. Подход 2: DataView с фильтром");
            var stopwatch2 = Stopwatch.StartNew();

            int totalEmployees2 = 0;
            DataView employeesView = new DataView(employees);

            foreach (DataRow company in companies.Rows)
            {
                DataRow[] companyDepartments = company.GetChildRows(compDeptRel);

                foreach (DataRow department in companyDepartments)
                {
                    employeesView.RowFilter = $"DepartmentID = {department["DepartmentID"]}";
                    totalEmployees2 += employeesView.Count;
                }
            }

            stopwatch2.Stop();
            Console.WriteLine($"  Результат: {totalEmployees2} сотрудников");
            Console.WriteLine($"  Время: {stopwatch2.ElapsedMilliseconds} мс");

            // 3. Подход 3: LINQ to DataSet
            Console.WriteLine("\n3. Подход 3: LINQ to DataSet");
            var stopwatch3 = Stopwatch.StartNew();

            var query = from company in companies.AsEnumerable()
                        join department in departments.AsEnumerable()
                            on company["CompanyID"] equals department["CompanyID"]
                        join employee in employees.AsEnumerable()
                            on department["DepartmentID"] equals employee["DepartmentID"]
                        group employee by company["CompanyName"] into g
                        select new { Company = g.Key, EmployeeCount = g.Count() };

            int totalEmployees3 = query.Sum(x => x.EmployeeCount);
            stopwatch3.Stop();

            Console.WriteLine($"  Результат: {totalEmployees3} сотрудников");
            Console.WriteLine($"  Время: {stopwatch3.ElapsedMilliseconds} мс");

            // 4. Подход 4: Прямой доступ через отношения
            Console.WriteLine("\n4. Подход 4: Прямой доступ через Select");
            var stopwatch4 = Stopwatch.StartNew();

            int totalEmployees4 = 0;
            foreach (DataRow company in companies.Rows)
            {
                string filter = $"CompanyID = {company["CompanyID"]}";
                DataRow[] companyDepartments = departments.Select(filter);

                foreach (DataRow department in companyDepartments)
                {
                    string empFilter = $"DepartmentID = {department["DepartmentID"]}";
                    DataRow[] departmentEmployees = employees.Select(empFilter);
                    totalEmployees4 += departmentEmployees.Length;
                }
            }

            stopwatch4.Stop();
            Console.WriteLine($"  Результат: {totalEmployees4} сотрудников");
            Console.WriteLine($"  Время: {stopwatch4.ElapsedMilliseconds} мс");

            // 5. Подход 5: Кэширование результатов
            Console.WriteLine("\n5. Подход 5: С кэшированием результатов");
            var stopwatch5 = Stopwatch.StartNew();

            // Кэшируем отделы по компаниям
            var departmentsByCompany = departments.AsEnumerable()
                .GroupBy(d => d["CompanyID"])
                .ToDictionary(g => g.Key, g => g.ToList());

            // Кэшируем сотрудников по отделам
            var employeesByDepartment = employees.AsEnumerable()
                .GroupBy(e => e["DepartmentID"])
                .ToDictionary(g => g.Key, g => g.ToList());

            int totalEmployees5 = 0;
            foreach (DataRow company in companies.Rows)
            {
                if (departmentsByCompany.TryGetValue(company["CompanyID"], out var compDepartments))
                {
                    foreach (DataRow department in compDepartments)
                    {
                        if (employeesByDepartment.TryGetValue(department["DepartmentID"], out var deptEmployees))
                        {
                            totalEmployees5 += deptEmployees.Count;
                        }
                    }
                }
            }

            stopwatch5.Stop();
            Console.WriteLine($"  Результат: {totalEmployees5} сотрудников");
            Console.WriteLine($"  Время: {stopwatch5.ElapsedMilliseconds} мс");

            // 6. Сравнение производительности
            Console.WriteLine("\n\n6. Сравнение производительности:");
            Console.WriteLine($"{"Подход",-25} {"Время (мс)",-15} {"Относительная скорость"}");
            Console.WriteLine(new string('-', 60));

            var results = new[]
            {
                new { Name = "GetChildRows() в цикле", Time = stopwatch1.ElapsedMilliseconds },
                new { Name = "DataView с фильтром", Time = stopwatch2.ElapsedMilliseconds },
                new { Name = "LINQ to DataSet", Time = stopwatch3.ElapsedMilliseconds },
                new { Name = "Прямой доступ через Select", Time = stopwatch4.ElapsedMilliseconds },
                new { Name = "С кэшированием", Time = stopwatch5.ElapsedMilliseconds }
            };

            long minTime = results.Min(r => r.Time);

            foreach (var result in results.OrderBy(r => r.Time))
            {
                double relativeSpeed = (double)result.Time / minTime;
                Console.WriteLine($"{result.Name,-25} {result.Time,-15} {relativeSpeed,20:F1}x");
            }

            // 7. Измерение использования памяти
            Console.WriteLine("\n\n7. Измерение использования памяти:");

            GC.Collect();
            GC.WaitForPendingFinalizers();

            long initialMemory = GC.GetTotalMemory(true);

            // Тестируем самый медленный подход
            var stopwatchMem = Stopwatch.StartNew();
            int testResult = 0;

            for (int i = 0; i < 10; i++)
            {
                foreach (DataRow company in companies.Rows)
                {
                    DataRow[] companyDepartments = company.GetChildRows(compDeptRel);
                    foreach (DataRow department in companyDepartments)
                    {
                        DataRow[] departmentEmployees = department.GetChildRows(deptEmpRel);
                        testResult += departmentEmployees.Length;
                    }
                }
            }

            stopwatchMem.Stop();
            long finalMemory = GC.GetTotalMemory(true);
            long memoryUsed = finalMemory - initialMemory;

            Console.WriteLine($"  Память использовано: {memoryUsed / 1024} KB");
            Console.WriteLine($"  Время выполнения 10 итераций: {stopwatchMem.ElapsedMilliseconds} мс");

            // 8. Оптимизация самого медленного подхода
            Console.WriteLine("\n\n8. Оптимизация самого медленного подхода:");

            // Анализируем, какой подход был самым медленным
            var slowestApproach = results.OrderByDescending(r => r.Time).First();
            Console.WriteLine($"Самый медленный подход: {slowestApproach.Name}");

            Console.WriteLine("\nРекомендации по оптимизации:");
            Console.WriteLine("1. Используйте кэширование при частых запросах");
            Console.WriteLine("2. Избегайте создания DataView в циклах");
            Console.WriteLine("3. Используйте LINQ для сложных запросов");
            Console.WriteLine("4. Используйте GetChanges() вместо перебора всех строк");
            Console.WriteLine("5. Отключайте отслеживание изменений при массовых операциях");
            Console.WriteLine("6. Используйте первичные ключи для быстрого поиска");
            Console.WriteLine("7. Ограничивайте объем выбираемых данных");

            // 9. Практические примеры оптимизации
            Console.WriteLine("\n\n9. Практические примеры оптимизации:");

            Console.WriteLine("Пример 1: Массовое обновление");
            var swUpdate = Stopwatch.StartNew();

            // Неоптимизированный вариант
            foreach (DataRow employee in employees.Rows)
            {
                employee["Salary"] = (decimal)employee["Salary"] * 1.1m;
            }
            swUpdate.Stop();
            Console.WriteLine($"  Неоптимизированное обновление: {swUpdate.ElapsedMilliseconds} мс");

            // Откатываем изменения
            employees.RejectChanges();

            // Оптимизированный вариант
            swUpdate.Restart();
            employees.Columns["Salary"].ReadOnly = false;
            foreach (DataRow employee in employees.Rows)
            {
                employee.BeginEdit();
                employee["Salary"] = (decimal)employee["Salary"] * 1.1m;
                employee.EndEdit();
            }
            swUpdate.Stop();
            Console.WriteLine($"  Оптимизированное обновление: {swUpdate.ElapsedMilliseconds} мс");

            // 10. Выводы и рекомендации
            Console.WriteLine("\n\n10. Выводы и рекомендации:");
            Console.WriteLine("✓ Для небольших наборов данных используйте GetChildRows()");
            Console.WriteLine("✓ Для средних наборов данных используйте LINQ to DataSet");
            Console.WriteLine("✓ Для больших наборов данных используйте кэширование");
            Console.WriteLine("✓ Избегайте создания объектов в циклах");
            Console.WriteLine("✓ Используйте BeginEdit/EndEdit для массовых обновлений");
            Console.WriteLine("✓ Отключайте constraints при массовых операциях");
            Console.WriteLine("✓ Используйте DataView для фильтрации и сортировки");
            Console.WriteLine("✓ Используйте GetChanges() для работы только с измененными данными");
        }

        static DataSet CreateLargeDataSet()
        {
            DataSet ds = new DataSet("LargeDataSet");

            // Компании
            DataTable companies = new DataTable("Компании");
            companies.Columns.Add("CompanyID", typeof(int));
            companies.Columns.Add("CompanyName", typeof(string));
            companies.PrimaryKey = new DataColumn[] { companies.Columns["CompanyID"] };

            // Отделы
            DataTable departments = new DataTable("Отделы");
            departments.Columns.Add("DepartmentID", typeof(int));
            departments.Columns.Add("DepartmentName", typeof(string));
            departments.Columns.Add("CompanyID", typeof(int));
            departments.PrimaryKey = new DataColumn[] { departments.Columns["DepartmentID"] };

            // Сотрудники
            DataTable employees = new DataTable("Сотрудники");
            employees.Columns.Add("EmployeeID", typeof(int));
            employees.Columns.Add("EmployeeName", typeof(string));
            employees.Columns.Add("DepartmentID", typeof(int));
            employees.Columns.Add("Salary", typeof(decimal));
            employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

            ds.Tables.Add(companies);
            ds.Tables.Add(departments);
            ds.Tables.Add(employees);

            Random random = new Random();

            // Заполняем компании (10 компаний)
            for (int i = 1; i <= 10; i++)
            {
                companies.Rows.Add(i, $"Компания {i}");
            }

            int departmentId = 1;
            int employeeId = 1;

            // Для каждой компании создаем отделы (10 отделов на компанию)
            for (int companyId = 1; companyId <= 10; companyId++)
            {
                for (int deptNum = 1; deptNum <= 10; deptNum++)
                {
                    departments.Rows.Add(departmentId, $"Отдел {deptNum}", companyId);

                    // Для каждого отдела создаем сотрудников (100 сотрудников на отдел)
                    for (int empNum = 1; empNum <= 100; empNum++)
                    {
                        employees.Rows.Add(
                            employeeId,
                            $"Сотрудник {employeeId}",
                            departmentId,
                            random.Next(30000, 150000)
                        );
                        employeeId++;
                    }

                    departmentId++;
                }
            }

            // Создаем отношения
            ds.Relations.Add(new DataRelation(
                "FK_Departments_Companies",
                companies.Columns["CompanyID"],
                departments.Columns["CompanyID"],
                true));

            ds.Relations.Add(new DataRelation(
                "FK_Employees_Departments",
                departments.Columns["DepartmentID"],
                employees.Columns["DepartmentID"],
                true));

            return ds;
        }
        #endregion
    }
}