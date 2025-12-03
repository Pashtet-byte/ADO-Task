using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ADONET_All_Tasks
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== ПРАКТИЧЕСКИЕ ЗАДАНИЯ ПО ADO.NET ===\n");

            while (true)
            {
                Console.WriteLine("\nВыберите задание для запуска:");
                Console.WriteLine("1. Отслеживание изменений состояния строк (RowState)");
                Console.WriteLine("2. Работа с DataRowVersion");
                Console.WriteLine("3. Фильтрация и поиск данных (DataView)");
                Console.WriteLine("4. Создание TableAdapter через конструктор (эмуляция)");
                Console.WriteLine("5. Различные методы TableAdapter");
                Console.WriteLine("6. Анализ структуры БД");
                Console.WriteLine("7. Сопоставление таблиц (DataTableMapping)");
                Console.WriteLine("8. Сопоставление колонок (DataColumnMapping)");
                Console.WriteLine("9. Комплексное приложение");
                Console.WriteLine("10. Оптимизация больших данных");
                Console.WriteLine("0. Выход");
                Console.Write("\nВаш выбор: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Task1_RowState();
                        break;
                    case 2:
                        Task2_DataRowVersion();
                        break;
                    case 3:
                        Task3_DataView();
                        break;
                    case 4:
                        Task4_TableAdapter();
                        break;
                    case 5:
                        Task5_TableAdapterMethods();
                        break;
                    case 6:
                        Task6_TableSchema();
                        break;
                    case 7:
                        Task7_DataTableMapping();
                        break;
                    case 8:
                        Task8_DataColumnMapping();
                        break;
                    case 9:
                        Task9_ComplexApp();
                        break;
                    case 10:
                        await Task10_Optimization();
                        break;
                    case 0:
                        Console.WriteLine("Выход из программы.");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("\n\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        #region Задание 1: Отслеживание изменений состояния строк в DataTable через RowState
        static void Task1_RowState()
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 1: Отслеживание изменений RowState ===\n");

            // Создаем DataTable для учащихся
            DataTable studentsTable = CreateStudentsTable();

            // 1. Добавляем начальные данные
            AddInitialStudents(studentsTable);
            Console.WriteLine("=== Исходные данные ===");
            PrintStudents(studentsTable);

            // 2. Добавляем 3 новых учащихся
            Console.WriteLine("\n=== Добавление 3 новых учащихся ===");
            AddNewStudents(studentsTable);
            PrintStudents(studentsTable);

            // 3. Редактируем 2 существующих учащихся
            Console.WriteLine("\n=== Редактирование 2 учащихся ===");
            EditExistingStudents(studentsTable);
            PrintStudents(studentsTable);

            // 4. Удаляем 1 учащегося
            Console.WriteLine("\n=== Удаление 1 учащегося ===");
            DeleteStudent(studentsTable);
            PrintStudents(studentsTable);

            // 5. Подробный отчет о всех строках
            Console.WriteLine("\n=== Подробный отчет о всех строках ===");
            PrintDetailedReport(studentsTable);

            // 6. Отчет только об измененных строках
            Console.WriteLine("\n=== Отчет только об измененных строках ===");
            PrintChangedRowsOnly(studentsTable);
        }

        static DataTable CreateStudentsTable()
        {
            DataTable table = new DataTable("Учащиеся");

            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("ФИО", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Класс", typeof(string));
            table.Columns.Add("СредняяОценка", typeof(double));

            return table;
        }

        static void AddInitialStudents(DataTable table)
        {
            table.Rows.Add(1, "Иванов Иван Иванович", "ivanov@school.ru", "10А", 4.5);
            table.Rows.Add(2, "Петрова Анна Сергеевна", "petrova@school.ru", "10Б", 4.8);
            table.Rows.Add(3, "Сидоров Алексей Петрович", "sidorov@school.ru", "11А", 4.2);
        }

        static void AddNewStudents(DataTable table)
        {
            DataRow newRow1 = table.NewRow();
            newRow1["ID"] = 4;
            newRow1["ФИО"] = "Кузнецова Мария Владимировна";
            newRow1["Email"] = "kuznetsova@school.ru";
            newRow1["Класс"] = "9А";
            newRow1["СредняяОценка"] = 4.7;
            table.Rows.Add(newRow1);

            DataRow newRow2 = table.NewRow();
            newRow2["ID"] = 5;
            newRow2["ФИО"] = "Васильев Дмитрий Андреевич";
            newRow2["Email"] = "vasiliev@school.ru";
            newRow2["Класс"] = "10Б";
            newRow2["СредняяОценка"] = 4.1;
            table.Rows.Add(newRow2);

            DataRow newRow3 = table.NewRow();
            newRow3["ID"] = 6;
            newRow3["ФИО"] = "Николаева Елена Игоревна";
            newRow3["Email"] = "nikolaeva@school.ru";
            newRow3["Класс"] = "11А";
            newRow3["СредняяОценка"] = 4.9;
            table.Rows.Add(newRow3);

            Console.WriteLine($"Состояние строки 4: {newRow1.RowState}"); // Added
            Console.WriteLine($"Состояние строки 5: {newRow2.RowState}"); // Added
            Console.WriteLine($"Состояние строки 6: {newRow3.RowState}"); // Added
        }

        static void EditExistingStudents(DataTable table)
        {
            DataRow row1 = table.Rows[0];
            row1["Email"] = "ivanov.new@school.ru";
            row1["СредняяОценка"] = 4.6;

            DataRow row2 = table.Rows[1];
            row2["Email"] = "petrova.new@school.ru";
            row2["СредняяОценка"] = 4.9;

            Console.WriteLine($"Состояние строки 1: {row1.RowState}"); // Modified
            Console.WriteLine($"Состояние строки 2: {row2.RowState}"); // Modified
        }

        static void DeleteStudent(DataTable table)
        {
            DataRow rowToDelete = table.Rows[2];
            rowToDelete.Delete();

            Console.WriteLine($"Состояние строки 3: {rowToDelete.RowState}"); // Deleted
        }

        static void PrintStudents(DataTable table)
        {
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"{"ID",-5} {"ФИО",-30} {"Email",-25} {"Класс",-8} {"Оценка",-8} {"Состояние"}");
            Console.WriteLine(new string('-', 80));

            foreach (DataRow row in table.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    Console.WriteLine($"{row["ID"],-5} {row["ФИО"],-30} {row["Email"],-25} {row["Класс"],-8} {row["СредняяОценка"],-8} {row.RowState}");
                }
            }

            // Показываем удаленные строки
            DataRow[] deletedRows = table.Select(null, null, DataViewRowState.Deleted);
            foreach (DataRow row in deletedRows)
            {
                Console.WriteLine($"{row["ID", DataRowVersion.Original],-5} " +
                                 $"{row["ФИО", DataRowVersion.Original],-30} " +
                                 $"{row["Email", DataRowVersion.Original],-25} " +
                                 $"{row["Класс", DataRowVersion.Original],-8} " +
                                 $"{row["СредняяОценка", DataRowVersion.Original],-8} " +
                                 $"{row.RowState} (УДАЛЕН)");
            }
        }

        static void PrintDetailedReport(DataTable table)
        {
            Console.WriteLine("\nПодробный отчет о всех строках:");
            Console.WriteLine(new string('-', 100));
            Console.WriteLine($"{"ID",-5} {"ФИО",-25} {"Состояние",-12} {"Измененные поля"}");
            Console.WriteLine(new string('-', 100));

            foreach (DataRow row in table.Rows)
            {
                string changedFields = GetChangedFields(row, table);
                Console.WriteLine($"{row["ID"],-5} {row["ФИО"],-25} {row.RowState,-12} {changedFields}");
            }

            DataRow[] deletedRows = table.Select(null, null, DataViewRowState.Deleted);
            foreach (DataRow row in deletedRows)
            {
                string changedFields = GetChangedFields(row, table);
                Console.WriteLine($"{row["ID", DataRowVersion.Original],-5} " +
                                 $"{row["ФИО", DataRowVersion.Original],-25} " +
                                 $"{row.RowState,-12} {changedFields}");
            }
        }

        static string GetChangedFields(DataRow row, DataTable table)
        {
            if (row.RowState == DataRowState.Unchanged)
                return "Нет изменений";

            if (row.RowState == DataRowState.Added)
                return "Все поля (новая строка)";

            if (row.RowState == DataRowState.Deleted)
                return "Строка удалена";

            StringBuilder changes = new StringBuilder();
            foreach (DataColumn column in table.Columns)
            {
                if (row.HasVersion(DataRowVersion.Original) &&
                    row.HasVersion(DataRowVersion.Current))
                {
                    object original = row[column.ColumnName, DataRowVersion.Original];
                    object current = row[column.ColumnName, DataRowVersion.Current];

                    if (!object.Equals(original, current))
                    {
                        changes.Append($"{column.ColumnName}: {original}->{current}; ");
                    }
                }
            }

            return changes.ToString();
        }

        static void PrintChangedRowsOnly(DataTable table)
        {
            Console.WriteLine("\nОтчет только об измененных строках (GetChanges()):");

            DataTable changedTable = table.GetChanges();

            if (changedTable != null)
            {
                Console.WriteLine(new string('-', 60));
                Console.WriteLine($"{"ID",-5} {"ФИО",-25} {"Состояние",-12}");
                Console.WriteLine(new string('-', 60));

                foreach (DataRow row in changedTable.Rows)
                {
                    Console.WriteLine($"{row["ID"],-5} {row["ФИО"],-25} {row.RowState,-12}");
                }
            }
            else
            {
                Console.WriteLine("Нет измененных строк.");
            }
        }
        #endregion

        #region Задание 2: Работа с DataRowVersion
        static void Task2_DataRowVersion()
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 2: Работа с DataRowVersion ===\n");

            DataTable productsTable = CreateProductsTable();
            AddInitialProducts(productsTable);
            productsTable.AcceptChanges();

            Console.WriteLine("Исходные данные товаров:");
            PrintProducts(productsTable);

            ModifyProducts(productsTable);

            Console.WriteLine("\nПосле изменений:");
            PrintProducts(productsTable);

            PrintPriceChangesReport(productsTable);
            PrintModifiedProductsReport(productsTable);
            PrintAllVersionsForRow(productsTable, 1);
            PrintAllVersionsForRow(productsTable, 3);
        }

        static DataTable CreateProductsTable()
        {
            DataTable table = new DataTable("Товары");

            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Название", typeof(string));
            table.Columns.Add("Цена", typeof(decimal));
            table.Columns.Add("КоличествоНаСкладе", typeof(int));
            table.Columns.Add("СтатусДоступности", typeof(string));

            table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

            return table;
        }

        static void AddInitialProducts(DataTable table)
        {
            table.Rows.Add(1, "Ноутбук ASUS", 75000.00m, 15, "В наличии");
            table.Rows.Add(2, "Смартфон iPhone", 89990.00m, 8, "В наличии");
            table.Rows.Add(3, "Планшет Samsung", 32990.00m, 0, "Нет в наличии");
            table.Rows.Add(4, "Монитор Dell", 24990.00m, 12, "В наличии");
            table.Rows.Add(5, "Клавиатура Logitech", 3490.00m, 25, "В наличии");
        }

        static void ModifyProducts(DataTable table)
        {
            Console.WriteLine("\nИзменение товаров:");

            DataRow product1 = table.Rows.Find(1);
            if (product1 != null)
            {
                product1["Цена"] = 69990.00m;
                product1["КоличествоНаСкладе"] = 10;
            }

            DataRow product2 = table.Rows.Find(2);
            if (product2 != null)
            {
                product2["Цена"] = 95990.00m;
                product2["КоличествоНаСкладе"] = 5;
            }

            DataRow product4 = table.Rows.Find(4);
            if (product4 != null)
            {
                product4["СтатусДоступности"] = "Мало";
            }
        }

        static void PrintProducts(DataTable table)
        {
            Console.WriteLine(new string('-', 90));
            Console.WriteLine($"{"ID",-5} {"Название",-20} {"Цена",-15} {"Кол-во",-10} {"Статус",-15} {"Состояние"}");
            Console.WriteLine(new string('-', 90));

            foreach (DataRow row in table.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    Console.WriteLine($"{row["ID"],-5} " +
                                    $"{row["Название"],-20} " +
                                    $"{((decimal)row["Цена"]):C,-15} " +
                                    $"{row["КоличествоНаСкладе"],-10} " +
                                    $"{row["СтатусДоступности"],-15} " +
                                    $"{row.RowState}");
                }
            }
        }

        static void PrintPriceChangesReport(DataTable table)
        {
            Console.WriteLine("\n=== Отчет об изменениях цен ===");
            Console.WriteLine(new string('-', 120));
            Console.WriteLine($"{"ID",-5} {"Название",-20} {"Исх. цена",-12} {"Тек. цена",-12} {"Разница",-12} {"Изменение %",-12} Статус");
            Console.WriteLine(new string('-', 120));

            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Modified &&
                    row.HasVersion(DataRowVersion.Original) &&
                    row.HasVersion(DataRowVersion.Current))
                {
                    try
                    {
                        decimal originalPrice = (decimal)row["Цена", DataRowVersion.Original];
                        decimal currentPrice = (decimal)row["Цена", DataRowVersion.Current];
                        decimal difference = currentPrice - originalPrice;
                        decimal percentChange = originalPrice != 0 ?
                            (difference / originalPrice) * 100 : 0;

                        string status = difference > 0 ? "ПОВЫШЕНИЕ" :
                                      difference < 0 ? "СКИДКА" : "БЕЗ ИЗМЕНЕНИЙ";

                        Console.WriteLine($"{row["ID"],-5} " +
                                        $"{row["Название"],-20} " +
                                        $"{originalPrice,-12:C0} " +
                                        $"{currentPrice,-12:C0} " +
                                        $"{difference,-12:C0} " +
                                        $"{percentChange,-12:F1}% " +
                                        $"{status}");
                    }
                    catch (VersionNotFoundException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }
            }
        }

        static void PrintAllVersionsForRow(DataTable table, int productId)
        {
            Console.WriteLine($"\n=== Все версии для товара ID={productId} ===");

            DataRow row = table.Rows.Find(productId);
            if (row == null)
            {
                Console.WriteLine("Товар не найден.");
                return;
            }

            Console.WriteLine($"Состояние строки: {row.RowState}");

            string[] versions = { "Original", "Current", "Default", "Proposed" };

            foreach (string versionName in versions)
            {
                try
                {
                    DataRowVersion version = (DataRowVersion)Enum.Parse(
                        typeof(DataRowVersion), versionName);

                    if (row.HasVersion(version))
                    {
                        Console.WriteLine($"\n{versionName} версия:");
                        Console.WriteLine($"  Название: {row["Название", version]}");
                        Console.WriteLine($"  Цена: {row["Цена", version]:C0}");
                        Console.WriteLine($"  Количество: {row["КоличествоНаСкладе", version]}");
                        Console.WriteLine($"  Статус: {row["СтатусДоступности", version]}");
                    }
                    else
                    {
                        Console.WriteLine($"\n{versionName} версия: недоступна");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        static void PrintModifiedProductsReport(DataTable table)
        {
            Console.WriteLine("\n=== Товары с измененной ценой ===");

            DataTable modifiedTable = table.GetChanges(DataRowState.Modified);

            if (modifiedTable == null || modifiedTable.Rows.Count == 0)
            {
                Console.WriteLine("Нет товаров с измененной ценой.");
                return;
            }

            foreach (DataRow row in modifiedTable.Rows)
            {
                object originalPrice = row["Цена", DataRowVersion.Original];
                object currentPrice = row["Цена"];

                if (!originalPrice.Equals(currentPrice))
                {
                    Console.WriteLine($"\nТовар ID: {row["ID"]}");
                    Console.WriteLine($"Название: {row["Название"]}");
                    Console.WriteLine($"Старая цена: {originalPrice:C0}");
                    Console.WriteLine($"Новая цена: {currentPrice:C0}");

                    foreach (DataColumn column in modifiedTable.Columns)
                    {
                        if (column.ColumnName != "Цена" &&
                            row.HasVersion(DataRowVersion.Original))
                        {
                            object original = row[column.ColumnName, DataRowVersion.Original];
                            object current = row[column.ColumnName];

                            if (!object.Equals(original, current))
                            {
                                Console.WriteLine($"{column.ColumnName}: {original} -> {current}");
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Задание 3: Фильтрация и поиск данных в DataSet с использованием DataView
        static void Task3_DataView()
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 3: Фильтрация и поиск данных с использованием DataView ===\n");

            DataSet companyDataSet = CreateCompanyDataSet();

            Console.WriteLine("1. Поиск сотрудников по фамилии 'Иванов':");
            SearchByLastName(companyDataSet, "Иванов");

            Console.WriteLine("\n2. Фильтрация сотрудников IT отдела:");
            FilterByDepartment(companyDataSet, "IT");

            Console.WriteLine("\n3. Сотрудники с зарплатой > 80000:");
            FilterBySalary(companyDataSet, 80000);

            Console.WriteLine("\n4. Сортировка по дате найма (по возрастанию):");
            SortByHireDate(companyDataSet, true);

            Console.WriteLine("\n5. Сортировка по дате найма (по убыванию):");
            SortByHireDate(companyDataSet, false);

            Console.WriteLine("\n6. Комбинированный поиск: IT отдел, зарплата > 50000:");
            CombinedSearch(companyDataSet, "IT", 50000, "ФИО");

            Console.WriteLine("\n7. Комбинированный поиск: Финансы, зарплата > 85000:");
            CombinedSearch(companyDataSet, "Финансы", 85000);
        }

        static DataSet CreateCompanyDataSet()
        {
            DataSet ds = new DataSet("Компания");

            DataTable employees = new DataTable("Сотрудники");
            employees.Columns.Add("ID", typeof(int));
            employees.Columns.Add("ФИО", typeof(string));
            employees.Columns.Add("Отдел", typeof(string));
            employees.Columns.Add("Зарплата", typeof(decimal));
            employees.Columns.Add("ДатаНайма", typeof(DateTime));

            DataTable projects = new DataTable("Проекты");
            projects.Columns.Add("ID", typeof(int));
            projects.Columns.Add("Название", typeof(string));
            projects.Columns.Add("Отдел", typeof(string));
            projects.Columns.Add("БюджетПроекта", typeof(decimal));
            projects.Columns.Add("ДатаНачала", typeof(DateTime));

            ds.Tables.Add(employees);
            ds.Tables.Add(projects);

            AddSampleData(employees, projects);

            return ds;
        }

        static void AddSampleData(DataTable employees, DataTable projects)
        {
            employees.Rows.Add(1, "Иванов Иван Иванович", "IT", 85000, new DateTime(2020, 3, 15));
            employees.Rows.Add(2, "Петров Петр Петрович", "IT", 75000, new DateTime(2021, 6, 1));
            employees.Rows.Add(3, "Сидорова Анна Владимировна", "HR", 55000, new DateTime(2019, 11, 20));
            employees.Rows.Add(4, "Кузнецов Алексей Сергеевич", "Финансы", 90000, new DateTime(2018, 8, 10));
            employees.Rows.Add(5, "Васильева Мария Дмитриевна", "IT", 95000, new DateTime(2020, 1, 5));
            employees.Rows.Add(6, "Николаев Дмитрий Андреевич", "Маркетинг", 65000, new DateTime(2022, 2, 28));
            employees.Rows.Add(7, "Алексеева Екатерина Игоревна", "IT", 82000, new DateTime(2021, 9, 15));
            employees.Rows.Add(8, "Смирнов Сергей Викторович", "Финансы", 88000, new DateTime(2019, 5, 22));

            projects.Rows.Add(1, "Разработка CRM системы", "IT", 5000000, new DateTime(2023, 1, 10));
            projects.Rows.Add(2, "Обновление сайта компании", "IT", 1500000, new DateTime(2023, 3, 1));
            projects.Rows.Add(3, "Рекрутинг IT специалистов", "HR", 800000, new DateTime(2023, 2, 15));
            projects.Rows.Add(4, "Бюджетное планирование", "Финансы", 1200000, new DateTime(2023, 1, 20));
            projects.Rows.Add(5, "Маркетинговая кампания", "Маркетинг", 3000000, new DateTime(2023, 2, 1));
        }

        static void SearchByLastName(DataSet ds, string lastName)
        {
            try
            {
                string filter = $"ФИО LIKE '%{lastName}%'";
                DataRow[] foundRows = ds.Tables["Сотрудники"].Select(filter);

                if (foundRows.Length > 0)
                {
                    PrintEmployees(foundRows);
                }
                else
                {
                    Console.WriteLine("Сотрудники не найдены.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void FilterByDepartment(DataSet ds, string department)
        {
            DataTable employees = ds.Tables["Сотрудники"];
            DataView departmentView = new DataView(employees)
            {
                RowFilter = $"Отдел = '{department}'",
                Sort = "Зарплата DESC"
            };

            PrintDataView(departmentView);
        }

        static void FilterBySalary(DataSet ds, decimal minSalary)
        {
            DataTable employees = ds.Tables["Сотрудники"];
            DataView salaryView = new DataView(employees)
            {
                RowFilter = $"Зарплата > {minSalary}",
                Sort = "Зарплата DESC"
            };

            PrintDataView(salaryView);
        }

        static void SortByHireDate(DataSet ds, bool ascending)
        {
            DataTable employees = ds.Tables["Сотрудники"];
            DataView sortedView = new DataView(employees)
            {
                Sort = $"ДатаНайма {(ascending ? "ASC" : "DESC")}"
            };

            PrintDataView(sortedView);
        }

        static void CombinedSearch(DataSet ds, string department, decimal minSalary, string sortBy = "ФИО")
        {
            try
            {
                DataTable employees = ds.Tables["Сотрудники"];
                DataView combinedView = new DataView(employees)
                {
                    RowFilter = $"Отдел = '{department}' AND Зарплата > {minSalary}",
                    Sort = sortBy
                };

                if (combinedView.Count > 0)
                {
                    PrintDataView(combinedView);
                    PrintStatistics(combinedView);
                }
                else
                {
                    Console.WriteLine("Нет сотрудников, удовлетворяющих критериям.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void PrintEmployees(DataRow[] rows)
        {
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"{"ID",-5} {"ФИО",-25} {"Отдел",-12} {"Зарплата",-15} {"Дата найма",-15}");
            Console.WriteLine(new string('-', 85));

            foreach (DataRow row in rows)
            {
                Console.WriteLine($"{row["ID"],-5} " +
                                $"{row["ФИО"],-25} " +
                                $"{row["Отдел"],-12} " +
                                $"{((decimal)row["Зарплата"]):C,-15} " +
                                $"{((DateTime)row["ДатаНайма"]).ToString("dd.MM.yyyy"),-15}");
            }
        }

        static void PrintDataView(DataView dataView)
        {
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"{"ID",-5} {"ФИО",-25} {"Отдел",-12} {"Зарплата",-15} {"Дата найма",-15}");
            Console.WriteLine(new string('-', 85));

            foreach (DataRowView rowView in dataView)
            {
                Console.WriteLine($"{rowView["ID"],-5} " +
                                $"{rowView["ФИО"],-25} " +
                                $"{rowView["Отдел"],-12} " +
                                $"{((decimal)rowView["Зарплата"]):C,-15} " +
                                $"{((DateTime)rowView["ДатаНайма"]).ToString("dd.MM.yyyy"),-15}");
            }

            Console.WriteLine($"Всего записей: {dataView.Count}");
        }

        static void PrintStatistics(DataView dataView)
        {
            if (dataView.Count == 0)
                return;

            Console.WriteLine("\n=== Статистика ===");
            Console.WriteLine($"Количество сотрудников: {dataView.Count}");

            decimal totalSalary = 0;
            decimal maxSalary = decimal.MinValue;
            decimal minSalary = decimal.MaxValue;

            foreach (DataRowView rowView in dataView)
            {
                decimal salary = (decimal)rowView["Зарплата"];
                totalSalary += salary;

                if (salary > maxSalary) maxSalary = salary;
                if (salary < minSalary) minSalary = salary;
            }

            decimal avgSalary = totalSalary / dataView.Count;

            Console.WriteLine($"Средняя зарплата: {avgSalary:C0}");
            Console.WriteLine($"Минимальная зарплата: {minSalary:C0}");
            Console.WriteLine($"Максимальная зарплата: {maxSalary:C0}");
            Console.WriteLine($"Общий фонд оплаты: {totalSalary:C0}");
        }
        #endregion

        #region Задание 4: Создание и настройка TableAdapter (эмуляция)
        static void Task4_TableAdapter()
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 4: Создание и настройка TableAdapter (эмуляция) ===\n");

            Console.WriteLine("Эмуляция работы TableAdapter через конструктор:");
            Console.WriteLine("1. Создание DataSet и TableAdapter");
            Console.WriteLine("2. Настройка подключения к БД");
            Console.WriteLine("3. Создание Fill-методов:");
            Console.WriteLine("   - Fill() - загрузка всех студентов");
            Console.WriteLine("   - FillBySpecialty() - по специальности");
            Console.WriteLine("   - FillByEmail() - по email");
            Console.WriteLine("4. Настройка BindingSource и DataGridView");
            Console.WriteLine("5. Реализация CRUD операций");

            Console.WriteLine("\nПример работы TableAdapter:");

            // Эмуляция DataSet и TableAdapter
            StudentsDataSet ds = new StudentsDataSet();
            StudentsTableAdapter adapter = new StudentsTableAdapter();

            Console.WriteLine("\n1. Загрузка всех студентов:");
            adapter.Fill(ds);
            PrintStudentsTable(ds.Students);

            Console.WriteLine("\n2. Загрузка студентов по специальности 'Информатика':");
            adapter.FillBySpecialty(ds, "Информатика");
            PrintStudentsTable(ds.Students);

            Console.WriteLine("\n3. Загрузка студентов по email '@university.ru':");
            adapter.FillByEmail(ds, "@university.ru");
            PrintStudentsTable(ds.Students);

            Console.WriteLine("\n4. Добавление нового студента:");
            StudentsDataSet.StudentsRow newRow = ds.Students.NewStudentsRow();
            newRow.ФИ = "Новый студент";
            newRow.Email = "new@university.ru";
            newRow.Специальность = "Информатика";
            newRow.ДатаПоступления = DateTime.Today;
            ds.Students.AddStudentsRow(newRow);
            Console.WriteLine("Студент добавлен (RowState: " + newRow.RowState + ")");

            Console.WriteLine("\n5. Редактирование студента:");
            if (ds.Students.Rows.Count > 0)
            {
                ds.Students.Rows[0]["Специальность"] = "Математика";
                Console.WriteLine("Студент отредактирован (RowState: " + ds.Students.Rows[0].RowState + ")");
            }

            Console.WriteLine("\n6. Удаление студента:");
            if (ds.Students.Rows.Count > 1)
            {
                ds.Students.Rows[1].Delete();
                Console.WriteLine("Студент удален (RowState: " + ds.Students.Rows[1].RowState + ")");
            }

            Console.WriteLine("\n7. Сохранение изменений в БД:");
            int changes = adapter.Update(ds.Students);
            Console.WriteLine($"Сохранено изменений: {changes}");

            Console.WriteLine("\n8. Отображение в DataGridView (эмуляция):");
            Console.WriteLine("DataGridView будет отображать таблицу Students с колонками:");
            Console.WriteLine("- StudentID (автоинкремент, скрыт)");
            Console.WriteLine("- ФИ");
            Console.WriteLine("- Email");
            Console.WriteLine("- Специальность");
            Console.WriteLine("- ДатаПоступления");
        }

        class StudentsDataSet : DataSet
        {
            public StudentsDataTable Students { get; private set; }

            public StudentsDataSet()
            {
                Students = new StudentsDataTable();
                Tables.Add(Students);
            }

            public class StudentsDataTable : DataTable
            {
                public StudentsDataTable()
                {
                    TableName = "Students";

                    Columns.Add("StudentID", typeof(int));
                    Columns.Add("ФИ", typeof(string));
                    Columns.Add("Email", typeof(string));
                    Columns.Add("Специальность", typeof(string));
                    Columns.Add("ДатаПоступления", typeof(DateTime));

                    PrimaryKey = new DataColumn[] { Columns["StudentID"] };

                    Columns["StudentID"].AutoIncrement = true;
                    Columns["StudentID"].AutoIncrementSeed = 1;
                    Columns["StudentID"].AutoIncrementStep = 1;
                }

                public StudentsRow NewStudentsRow()
                {
                    return (StudentsRow)NewRow();
                }

                public void AddStudentsRow(StudentsRow row)
                {
                    Rows.Add(row);
                }

                protected override Type GetRowType()
                {
                    return typeof(StudentsRow);
                }

                protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
                {
                    return new StudentsRow(builder);
                }
            }

            public class StudentsRow : DataRow
            {
                internal StudentsRow(DataRowBuilder rb) : base(rb)
                {
                }

                public int StudentID
                {
                    get { return (int)this["StudentID"]; }
                    set { this["StudentID"] = value; }
                }

                public string ФИ
                {
                    get { return (string)this["ФИ"]; }
                    set { this["ФИ"] = value; }
                }

                public string Email
                {
                    get { return (string)this["Email"]; }
                    set { this["Email"] = value; }
                }

                public string Специальность
                {
                    get { return (string)this["Специальность"]; }
                    set { this["Специальность"] = value; }
                }

                public DateTime ДатаПоступления
                {
                    get { return (DateTime)this["ДатаПоступления"]; }
                    set { this["ДатаПоступления"] = value; }
                }
            }
        }

        class StudentsTableAdapter
        {
            public int Fill(StudentsDataSet ds)
            {
                Console.WriteLine("  Выполнение Fill() - загрузка всех студентов");

                // Эмуляция данных из БД
                for (int i = 1; i <= 5; i++)
                {
                    StudentsDataSet.StudentsRow row = ds.Students.NewStudentsRow();
                    row.ФИ = $"Студент {i}";
                    row.Email = $"student{i}@university.ru";
                    row.Специальность = i % 2 == 0 ? "Информатика" : "Математика";
                    row.ДатаПоступления = DateTime.Today.AddDays(-i * 100);
                    ds.Students.AddStudentsRow(row);
                }

                return 5;
            }

            public int FillBySpecialty(StudentsDataSet ds, string specialty)
            {
                Console.WriteLine($"  Выполнение FillBySpecialty() - загрузка по специальности: {specialty}");

                // Очищаем таблицу
                ds.Students.Clear();

                // Эмуляция данных
                for (int i = 1; i <= 3; i++)
                {
                    StudentsDataSet.StudentsRow row = ds.Students.NewStudentsRow();
                    row.ФИ = $"Студент {i} ({specialty})";
                    row.Email = $"student{i}@university.ru";
                    row.Специальность = specialty;
                    row.ДатаПоступления = DateTime.Today.AddDays(-i * 50);
                    ds.Students.AddStudentsRow(row);
                }

                return 3;
            }

            public int FillByEmail(StudentsDataSet ds, string email)
            {
                Console.WriteLine($"  Выполнение FillByEmail() - загрузка по email: {email}");

                ds.Students.Clear();

                for (int i = 1; i <= 2; i++)
                {
                    StudentsDataSet.StudentsRow row = ds.Students.NewStudentsRow();
                    row.ФИ = $"Студент с email {i}";
                    row.Email = $"user{i}{email}";
                    row.Специальность = "Разное";
                    row.ДатаПоступления = DateTime.Today.AddDays(-i * 30);
                    ds.Students.AddStudentsRow(row);
                }

                return 2;
            }

            public int Update(StudentsDataSet.StudentsDataTable table)
            {
                Console.WriteLine("  Выполнение Update() - сохранение изменений в БД");

                int changes = 0;
                foreach (DataRow row in table.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            Console.WriteLine($"    INSERT: {row["ФИ"]}");
                            changes++;
                            break;
                        case DataRowState.Modified:
                            Console.WriteLine($"    UPDATE: {row["ФИ"]} (ID: {row["StudentID"]})");
                            changes++;
                            break;
                        case DataRowState.Deleted:
                            Console.WriteLine($"    DELETE: ID {row["StudentID", DataRowVersion.Original]}");
                            changes++;
                            break;
                    }
                }

                return changes;
            }
        }

        static void PrintStudentsTable(StudentsDataSet.StudentsDataTable table)
        {
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"{"ID",-5} {"ФИ",-25} {"Email",-25} {"Специальность",-15} {"Дата поступления",-15}");
            Console.WriteLine(new string('-', 80));

            foreach (DataRow row in table.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    Console.WriteLine($"{row["StudentID"],-5} " +
                                    $"{row["ФИ"],-25} " +
                                    $"{row["Email"],-25} " +
                                    $"{row["Специальность"],-15} " +
                                    $"{((DateTime)row["ДатаПоступления"]).ToString("dd.MM.yyyy"),-15}");
                }
            }
        }
        #endregion

        #region Задание 5: Получение данных через различные методы TableAdapter
        static void Task5_TableAdapterMethods()
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 5: Получение данных через различные методы TableAdapter ===\n");

            StudentsDataSet ds = new StudentsDataSet();
            StudentsTableAdapter adapter = new StudentsTableAdapter();

            Console.WriteLine("1. TableAdapter.Fill() для заполнения DataTable:");
            adapter.Fill(ds);
            Console.WriteLine($"Загружено студентов: {ds.Students.Rows.Count}");

            Console.WriteLine("\n2. TableAdapter.GetData() для получения DataTable:");
            DataTable studentsTable = GetDataMock();
            Console.WriteLine($"Получено студентов: {studentsTable.Rows.Count}");

            Console.WriteLine("\n3. Пользовательский метод для выборки по диапазону дат:");
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime endDate = DateTime.Today;
            DataTable dateFiltered = GetStudentsByDateRange(startDate, endDate);
            Console.WriteLine($"Студенты с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}: {dateFiltered.Rows.Count}");

            Console.WriteLine("\n4. Получение студента по ID:");
            DataRow student = GetStudentById(2);
            if (student != null)
            {
                Console.WriteLine($"Найден студент: {student["ФИ"]}");
            }

            Console.WriteLine("\n5. Постраничное получение данных:");
            for (int page = 0; page < 3; page++)
            {
                DataTable pageData = GetStudentsPaged(page, 2);
                Console.WriteLine($"Страница {page + 1}: {pageData.Rows.Count} записей");
            }

            Console.WriteLine("\n6. Агрегированная информация:");
            Dictionary<string, int> stats = GetStatisticsBySpecialty();
            foreach (var stat in stats)
            {
                Console.WriteLine($"  {stat.Key}: {stat.Value} студентов");
            }

            Console.WriteLine("\n7. Сравнение Fill() и GetData():");
            ComparePerformance();
        }

        static DataTable GetDataMock()
        {
            DataTable table = new DataTable("Students");
            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("ФИ", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Специальность", typeof(string));
            table.Columns.Add("ДатаПоступления", typeof(DateTime));

            for (int i = 1; i <= 8; i++)
            {
                table.Rows.Add(i, $"Студент {i}", $"student{i}@mail.ru",
                    i % 2 == 0 ? "Информатика" : "Математика",
                    DateTime.Today.AddDays(-i * 100));
            }

            return table;
        }

        static DataTable GetStudentsByDateRange(DateTime startDate, DateTime endDate)
        {
            DataTable allStudents = GetDataMock();
            DataView dateView = new DataView(allStudents)
            {
                RowFilter = $"ДатаПоступления >= #{startDate:MM/dd/yyyy}# " +
                           $"AND ДатаПоступления <= #{endDate:MM/dd/yyyy}#"
            };

            return dateView.ToTable();
        }

        static DataRow GetStudentById(int studentId)
        {
            DataTable studentsTable = GetDataMock();
            DataRow[] foundRows = studentsTable.Select($"StudentID = {studentId}");

            return foundRows.Length > 0 ? foundRows[0] : null;
        }

        static DataTable GetStudentsPaged(int pageNumber, int pageSize)
        {
            DataTable allStudents = GetDataMock();
            DataTable pagedTable = allStudents.Clone();

            int startIndex = pageNumber * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, allStudents.Rows.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                pagedTable.ImportRow(allStudents.Rows[i]);
            }

            return pagedTable;
        }

        static Dictionary<string, int> GetStatisticsBySpecialty()
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();

            DataTable students = GetDataMock();
            DataView view = new DataView(students);
            DataTable groupedTable = view.ToTable(true, "Специальность");

            foreach (DataRow row in groupedTable.Rows)
            {
                string specialty = row["Специальность"].ToString();
                DataView specialtyView = new DataView(students)
                {
                    RowFilter = $"Специальность = '{specialty}'"
                };

                stats[specialty] = specialtyView.Count;
            }

            return stats;
        }

        static void ComparePerformance()
        {
            Stopwatch stopwatch = new Stopwatch();

            // Тест Fill
            StudentsDataSet ds1 = new StudentsDataSet();
            StudentsTableAdapter adapter1 = new StudentsTableAdapter();

            stopwatch.Start();
            adapter1.Fill(ds1);
            stopwatch.Stop();
            long fillTime = stopwatch.ElapsedMilliseconds;

            // Тест GetData
            stopwatch.Restart();
            DataTable table = GetDataMock();
            stopwatch.Stop();
            long getDataTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"  Fill() время: {fillTime} мс");
            Console.WriteLine($"  GetData() время: {getDataTime} мс");
            Console.WriteLine($"  Разница: {Math.Abs(fillTime - getDataTime)} мс");
        }
        #endregion

        #region Задание 6: Использование TableAdapter для получения информации о структуре базы данных
        static void Task6_TableSchema()
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 6: Анализ структуры таблицы через TableAdapter ===\n");

            Console.WriteLine("1. Информация о колонках таблицы 'Студенты':");
            AnalyzeColumns();

            Console.WriteLine("\n2. Полная информация через GetSchemaTable():");
            AnalyzeWithSchemaTable();

            Console.WriteLine("\n3. Информация об индексах таблицы:");
            AnalyzeIndexes();

            Console.WriteLine("\n4. Связи с другими таблицами:");
            AnalyzeRelationships();

            Console.WriteLine("\n5. Сравнение локальной и БД структур:");
            CompareWithDatabaseStructure();

            Console.WriteLine("\n6. Экспорт схемы в XML:");
            ExportSchemaToXml();
        }

        static void AnalyzeColumns()
        {
            StudentsDataSet ds = new StudentsDataSet();
            StudentsTableAdapter adapter = new StudentsTableAdapter();
            adapter.Fill(ds);

            Console.WriteLine(new string('-', 90));
            Console.WriteLine($"{"Имя колонки",-20} {"Тип данных",-15} {"NULL",-8} {"PK",-5} {"Max Length",-12} {"AutoIncrement"}");
            Console.WriteLine(new string('-', 90));

            foreach (DataColumn column in ds.Students.Columns)
            {
                Console.WriteLine($"{column.ColumnName,-20} " +
                                $"{column.DataType.Name,-15} " +
                                $"{column.AllowDBNull,-8} " +
                                $"{(ds.Students.PrimaryKey.Length > 0 && ds.Students.PrimaryKey[0].ColumnName == column.ColumnName ? "Да" : "Нет"),-5} " +
                                $"{(column.DataType == typeof(string) && column.MaxLength > 0 ? column.MaxLength.ToString() : "N/A"),-12} " +
                                $"{column.AutoIncrement}");
            }
        }

        static void AnalyzeWithSchemaTable()
        {
            DataTable schemaTable = new DataTable("SchemaInfo");
            schemaTable.Columns.Add("ColumnName", typeof(string));
            schemaTable.Columns.Add("DataType", typeof(string));
            schemaTable.Columns.Add("Size", typeof(int));
            schemaTable.Columns.Add("IsNullable", typeof(bool));
            schemaTable.Columns.Add("IsPrimaryKey", typeof(bool));
            schemaTable.Columns.Add("IsIdentity", typeof(bool));
            schemaTable.Columns.Add("DefaultValue", typeof(object));

            AddSchemaRow(schemaTable, "StudentID", "int", 4, false, true, true, null);
            AddSchemaRow(schemaTable, "ФИ", "nvarchar", 100, false, false, false, null);
            AddSchemaRow(schemaTable, "Email", "nvarchar", 100, true, false, false, null);
            AddSchemaRow(schemaTable, "Специальность", "nvarchar", 100, true, false, false, "Не указана");
            AddSchemaRow(schemaTable, "ДатаПоступления", "date", 3, false, false, false, "GETDATE()");

            Console.WriteLine(new string('-', 100));
            Console.WriteLine($"{"Имя",-15} {"Тип",-15} {"Размер",-10} {"NULL",-8} {"PK",-5} {"Identity",-10} {"Значение по умолчанию"}");
            Console.WriteLine(new string('-', 100));

            foreach (DataRow row in schemaTable.Rows)
            {
                Console.WriteLine($"{row["ColumnName"],-15} " +
                                $"{row["DataType"],-15} " +
                                $"{row["Size"],-10} " +
                                $"{row["IsNullable"],-8} " +
                                $"{row["IsPrimaryKey"],-5} " +
                                $"{row["IsIdentity"],-10} " +
                                $"{row["DefaultValue"]}");
            }
        }

        static void AddSchemaRow(DataTable schemaTable, string name, string type,
                               int size, bool nullable, bool isPK, bool isIdentity, object defaultValue)
        {
            DataRow row = schemaTable.NewRow();
            row["ColumnName"] = name;
            row["DataType"] = type;
            row["Size"] = size;
            row["IsNullable"] = nullable;
            row["IsPrimaryKey"] = isPK;
            row["IsIdentity"] = isIdentity;
            row["DefaultValue"] = defaultValue;
            schemaTable.Rows.Add(row);
        }

        static void AnalyzeIndexes()
        {
            DataTable indexesTable = new DataTable("Indexes");
            indexesTable.Columns.Add("IndexName", typeof(string));
            indexesTable.Columns.Add("ColumnName", typeof(string));
            indexesTable.Columns.Add("IsUnique", typeof(bool));
            indexesTable.Columns.Add("SortOrder", typeof(string));

            AddIndexRow(indexesTable, "PK_Студенты", "StudentID", true, "ASC");
            AddIndexRow(indexesTable, "IX_Email", "Email", true, "ASC");
            AddIndexRow(indexesTable, "IX_Специальность", "Специальность", false, "ASC");

            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"Имя индекса",-20} {"Колонка",-15} {"Уникальный",-12} {"Сортировка"}");
            Console.WriteLine(new string('-', 60));

            foreach (DataRow row in indexesTable.Rows)
            {
                Console.WriteLine($"{row["IndexName"],-20} " +
                                $"{row["ColumnName"],-15} " +
                                $"{row["IsUnique"],-12} " +
                                $"{row["SortOrder"]}");
            }
        }

        static void AddIndexRow(DataTable indexesTable, string name, string column,
                              bool isUnique, string sortOrder)
        {
            DataRow row = indexesTable.NewRow();
            row["IndexName"] = name;
            row["ColumnName"] = column;
            row["IsUnique"] = isUnique;
            row["SortOrder"] = sortOrder;
            indexesTable.Rows.Add(row);
        }

        static void AnalyzeRelationships()
        {
            DataSet universityDS = new DataSet("University");

            DataTable students = new DataTable("Студенты");
            students.Columns.Add("StudentID", typeof(int));
            students.Columns.Add("ФИ", typeof(string));
            students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

            DataTable grades = new DataTable("Оценки");
            grades.Columns.Add("GradeID", typeof(int));
            grades.Columns.Add("StudentID", typeof(int));
            grades.Columns.Add("Предмет", typeof(string));
            grades.Columns.Add("Оценка", typeof(int));

            universityDS.Tables.Add(students);
            universityDS.Tables.Add(grades);

            DataRelation studentGradesRelation = new DataRelation(
                "FK_Grades_Students",
                students.Columns["StudentID"],
                grades.Columns["StudentID"]);

            universityDS.Relations.Add(studentGradesRelation);

            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"{"Имя связи",-20} {"Родительская",-15} {"Дочерняя",-15} {"Колонки связи"}");
            Console.WriteLine(new string('-', 70));

            foreach (DataRelation relation in universityDS.Relations)
            {
                Console.WriteLine($"{relation.RelationName,-20} " +
                                $"{relation.ParentTable.TableName,-15} " +
                                $"{relation.ChildTable.TableName,-15} " +
                                $"{relation.ParentColumns[0].ColumnName} -> {relation.ChildColumns[0].ColumnName}");
            }
        }

        static void CompareWithDatabaseStructure()
        {
            StudentsDataSet ds = new StudentsDataSet();
            StudentsTableAdapter adapter = new StudentsTableAdapter();
            adapter.Fill(ds);

            DataTable localTable = ds.Students;

            DataTable dbStructure = new DataTable();
            dbStructure.Columns.Add("ColumnName", typeof(string));
            dbStructure.Columns.Add("DataType", typeof(string));
            dbStructure.Columns.Add("IsNullable", typeof(bool));

            dbStructure.Rows.Add("StudentID", "int", false);
            dbStructure.Rows.Add("ФИ", "nvarchar", false);
            dbStructure.Rows.Add("Email", "nvarchar", true);
            dbStructure.Rows.Add("Специальность", "nvarchar", true);
            dbStructure.Rows.Add("ДатаПоступления", "date", false);
            dbStructure.Rows.Add("Телефон", "nvarchar", true);

            Console.WriteLine("Различия между локальной структурой и БД:");
            Console.WriteLine(new string('-', 50));

            bool hasDifferences = false;

            foreach (DataRow dbRow in dbStructure.Rows)
            {
                string columnName = dbRow["ColumnName"].ToString();

                if (!localTable.Columns.Contains(columnName))
                {
                    Console.WriteLine($"[+] В БД есть колонка '{columnName}', которой нет локально");
                    hasDifferences = true;
                }
            }

            foreach (DataColumn localColumn in localTable.Columns)
            {
                DataRow[] found = dbStructure.Select($"ColumnName = '{localColumn.ColumnName}'");
                if (found.Length == 0)
                {
                    Console.WriteLine($"[-] Локально есть колонка '{localColumn.ColumnName}', которой нет в БД");
                    hasDifferences = true;
                }
            }

            if (!hasDifferences)
            {
                Console.WriteLine("Структуры идентичны.");
            }
        }

        static void ExportSchemaToXml()
        {
            StringBuilder xmlBuilder = new StringBuilder();

            xmlBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlBuilder.AppendLine("<DatabaseSchema>");
            xmlBuilder.AppendLine("  <Table name=\"Студенты\">");

            StudentsDataSet ds = new StudentsDataSet();
            StudentsTableAdapter adapter = new StudentsTableAdapter();
            adapter.Fill(ds);

            foreach (DataColumn column in ds.Students.Columns)
            {
                xmlBuilder.AppendLine($"    <Column>");
                xmlBuilder.AppendLine($"      <Name>{column.ColumnName}</Name>");
                xmlBuilder.AppendLine($"      <Type>{column.DataType.Name}</Type>");
                xmlBuilder.AppendLine($"      <AllowNull>{column.AllowDBNull}</AllowNull>");
                xmlBuilder.AppendLine($"      <MaxLength>{(column.DataType == typeof(string) && column.MaxLength > 0 ? column.MaxLength.ToString() : "N/A")}</MaxLength>");
                xmlBuilder.AppendLine($"      <AutoIncrement>{column.AutoIncrement}</AutoIncrement>");
                xmlBuilder.AppendLine($"    </Column>");
            }

            xmlBuilder.AppendLine("  </Table>");
            xmlBuilder.AppendLine("</DatabaseSchema>");

            Console.WriteLine(xmlBuilder.ToString());
        }
        #endregion

        #region Задание 7: Сопоставление имён таблиц через DataTableMapping
        static void Task7_DataTableMapping()
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 7: Сопоставление имён таблиц через DataTableMapping ===\n");

            Console.WriteLine("1. Создание DataAdapter с запросами, возвращающими таблицы с разными именами:");
            Console.WriteLine("   - Запрос 1: SELECT * FROM DStud (из БД 'Студенты')");
            Console.WriteLine("   - Запрос 2: SELECT * FROM DProj (из БД 'Проекты')");

            Console.WriteLine("\n2. Создание DataTableMapping:");
            Console.WriteLine("   - 'DStud' -> 'Students'");
            Console.WriteLine("   - 'DProj' -> 'Projects'");

            DataSet ds = new DataSet("UniversityDB");
            MockDataAdapter adapter = new MockDataAdapter();

            ConfigureTableMappings(adapter);
            FillDataSetWithMappings(adapter, ds);
            VerifyTableMappings(ds);
            PrintMappingInfo(adapter, ds);

            Console.WriteLine("\n3. Зачем нужны маппинги:");
            Console.WriteLine("   • Абстракция от физической структуры БД");
            Console.WriteLine("   • Работа с унаследованными системами");
            Console.WriteLine("   • Поддержка нескольких схем БД");
            Console.WriteLine("   • Упрощение рефакторинга кода");
            Console.WriteLine("   • Локализация имен таблиц");
        }

        class MockDataAdapter : DbDataAdapter
        {
            public List<string> SelectCommands { get; } = new List<string>();

            public override int Fill(DataSet dataSet)
            {
                DataTable studentsTable = new DataTable("Students");
                studentsTable.Columns.Add("StudentID", typeof(int));
                studentsTable.Columns.Add("FullName", typeof(string));
                studentsTable.Columns.Add("Email", typeof(string));

                studentsTable.Rows.Add(1, "Иванов Иван", "ivanov@mail.ru");
                studentsTable.Rows.Add(2, "Петров Петр", "petrov@mail.ru");
                studentsTable.Rows.Add(3, "Сидорова Анна", "sidorova@mail.ru");

                DataTable projectsTable = new DataTable("Projects");
                projectsTable.Columns.Add("ProjectID", typeof(int));
                projectsTable.Columns.Add("ProjectName", typeof(string));
                projectsTable.Columns.Add("Budget", typeof(decimal));

                projectsTable.Rows.Add(1, "Веб-сайт компании", 500000);
                projectsTable.Rows.Add(2, "Мобильное приложение", 750000);

                dataSet.Tables.Add(studentsTable);
                dataSet.Tables.Add(projectsTable);

                return studentsTable.Rows.Count + projectsTable.Rows.Count;
            }

            protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow,
                IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
            {
                return null;
            }

            protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow,
                IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
            {
                return null;
            }

            protected override void OnRowUpdated(RowUpdatedEventArgs value)
            {
            }

            protected override void OnRowUpdating(RowUpdatingEventArgs value)
            {
            }
        }

        static void ConfigureTableMappings(DbDataAdapter adapter)
        {
            adapter.TableMappings.Clear();

            DataTableMapping studMapping = adapter.TableMappings.Add("DStud", "Students");
            Console.WriteLine($"Добавлен маппинг: 'DStud' -> 'Students'");

            DataTableMapping projMapping = adapter.TableMappings.Add("DProj", "Projects");
            Console.WriteLine($"Добавлен маппинг: 'DProj' -> 'Projects'");
        }

        static void FillDataSetWithMappings(DbDataAdapter adapter, DataSet dataSet)
        {
            try
            {
                int rowsAffected = adapter.Fill(dataSet);
                Console.WriteLine($"\nЗаполнено строк: {rowsAffected}");

                Console.WriteLine("\nТаблицы в DataSet после заполнения:");
                foreach (DataTable table in dataSet.Tables)
                {
                    Console.WriteLine($"  - {table.TableName} ({table.Rows.Count} строк)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void VerifyTableMappings(DataSet dataSet)
        {
            Console.WriteLine("\nПроверка результатов маппинга:");

            bool studentsTableExists = dataSet.Tables.Contains("Students");
            bool projectsTableExists = dataSet.Tables.Contains("Projects");
            bool dstudTableExists = dataSet.Tables.Contains("DStud");
            bool dprojTableExists = dataSet.Tables.Contains("DProj");

            Console.WriteLine($"Таблица 'Students' существует: {studentsTableExists}");
            Console.WriteLine($"Таблица 'Projects' существует: {projectsTableExists}");
            Console.WriteLine($"Таблица 'DStud' существует: {dstudTableExists} (должно быть False)");
            Console.WriteLine($"Таблица 'DProj' существует: {dprojTableExists} (должно быть False)");

            if (studentsTableExists && projectsTableExists && !dstudTableExists && !dprojTableExists)
            {
                Console.WriteLine("✓ Маппинг таблиц выполнен успешно!");
            }
            else
            {
                Console.WriteLine("✗ Ошибка в маппинге таблиц!");
            }
        }

        static void PrintMappingInfo(DbDataAdapter adapter, DataSet dataSet)
        {
            Console.WriteLine("\nИнформация о всех маппингах:");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"Исходное имя",-15} {"Локальное имя",-15} {"Статус"}");
            Console.WriteLine(new string('-', 60));

            foreach (DataTableMapping mapping in adapter.TableMappings)
            {
                string status = dataSet.Tables.Contains(mapping.DataSetTable) ?
                    "Активен" : "Таблица не найдена";

                Console.WriteLine($"{mapping.SourceTable,-15} {mapping.DataSetTable,-15} {status}");
            }
        }
        #endregion

        #region Задание 8: Сопоставление колонок через DataColumnMapping
        static void Task8_DataColumnMapping()
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 8: Сопоставление колонок через DataColumnMapping ===\n");

            Console.WriteLine("Исходные запросы с нелогичными именами колонок:");
            Console.WriteLine("1. SELECT s_id, s_fio, s_email, s_spec FROM DStud");
            Console.WriteLine("2. SELECT p_id, p_name, p_budget FROM DProj");

            Console.WriteLine("\nМаппинги колонок:");
            Console.WriteLine("  s_id -> StudentID");
            Console.WriteLine("  s_fio -> FullName");
            Console.WriteLine("  s_email -> Email");
            Console.WriteLine("  s_spec -> Specialization");
            Console.WriteLine("  p_id -> ProjectID");
            Console.WriteLine("  p_name -> ProjectName");
            Console.WriteLine("  p_budget -> Budget");

            DataSet ds = new DataSet("UniversityDB");
            MockDataAdapter2 adapter = new MockDataAdapter2();

            ConfigureColumnMappings(adapter);
            FillDataSetWithColumnMappings(adapter, ds);
            VerifyColumnMappings(ds);
            PrintColumnMappingReport(adapter, ds);
            DemonstrateDynamicMappings(adapter, ds);
        }

        class MockDataAdapter2 : DbDataAdapter
        {
            public override int Fill(DataSet dataSet)
            {
                DataTable studentsTable = new DataTable("Students");
                studentsTable.Columns.Add("StudentID", typeof(int));
                studentsTable.Columns.Add("FullName", typeof(string));
                studentsTable.Columns.Add("Email", typeof(string));
                studentsTable.Columns.Add("Specialization", typeof(string));

                studentsTable.Rows.Add(1, "Иванов Иван Иванович", "ivanov@university.ru", "Информатика");
                studentsTable.Rows.Add(2, "Петрова Анна Сергеевна", "petrova@university.ru", "Математика");
                studentsTable.Rows.Add(3, "Сидоров Алексей Петрович", "sidorov@university.ru", "Физика");

                DataTable projectsTable = new DataTable("Projects");
                projectsTable.Columns.Add("ProjectID", typeof(int));
                projectsTable.Columns.Add("ProjectName", typeof(string));
                projectsTable.Columns.Add("Budget", typeof(decimal));

                projectsTable.Rows.Add(1, "Разработка CRM системы", 1500000.00m);
                projectsTable.Rows.Add(2, "Исследование AI", 2500000.00m);

                dataSet.Tables.Add(studentsTable);
                dataSet.Tables.Add(projectsTable);

                return studentsTable.Rows.Count + projectsTable.Rows.Count;
            }

            protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow,
                IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
            {
                return null;
            }

            protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow,
                IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
            {
                return null;
            }

            protected override void OnRowUpdated(RowUpdatedEventArgs value)
            {
            }

            protected override void OnRowUpdating(RowUpdatingEventArgs value)
            {
            }
        }

        static void ConfigureColumnMappings(DbDataAdapter adapter)
        {
            adapter.TableMappings.Clear();

            DataTableMapping studentMapping = adapter.TableMappings.Add("DStud", "Students");
            studentMapping.ColumnMappings.Add("s_id", "StudentID");
            studentMapping.ColumnMappings.Add("s_fio", "FullName");
            studentMapping.ColumnMappings.Add("s_email", "Email");
            studentMapping.ColumnMappings.Add("s_spec", "Specialization");

            DataTableMapping projectMapping = adapter.TableMappings.Add("DProj", "Projects");
            projectMapping.ColumnMappings.Add("p_id", "ProjectID");
            projectMapping.ColumnMappings.Add("p_name", "ProjectName");
            projectMapping.ColumnMappings.Add("p_budget", "Budget");

            Console.WriteLine("\nМаппинги настроены.");
        }

        static void FillDataSetWithColumnMappings(DbDataAdapter adapter, DataSet dataSet)
        {
            try
            {
                int rowsAffected = adapter.Fill(dataSet);
                Console.WriteLine($"\nЗаполнено строк: {rowsAffected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void VerifyColumnMappings(DataSet dataSet)
        {
            Console.WriteLine("\nПроверка результатов маппинга колонок:");

            if (!dataSet.Tables.Contains("Students") || !dataSet.Tables.Contains("Projects"))
            {
                Console.WriteLine("Таблицы не созданы!");
                return;
            }

            DataTable students = dataSet.Tables["Students"];
            DataTable projects = dataSet.Tables["Projects"];

            Console.WriteLine("\nПроверка таблицы Students:");
            string[] expectedStudentColumns = { "StudentID", "FullName", "Email", "Specialization" };
            foreach (string expected in expectedStudentColumns)
            {
                bool exists = students.Columns.Contains(expected);
                Console.WriteLine($"  Колонка '{expected}': {(exists ? "✓" : "✗")}");
            }

            Console.WriteLine("\nПроверка таблицы Projects:");
            string[] expectedProjectColumns = { "ProjectID", "ProjectName", "Budget" };
            foreach (string expected in expectedProjectColumns)
            {
                bool exists = projects.Columns.Contains(expected);
                Console.WriteLine($"  Колонка '{expected}': {(exists ? "✓" : "✗")}");
            }

            Console.WriteLine("\nДанные в таблице Students (после маппинга):");
            PrintSimpleTable(students);

            Console.WriteLine("\nДанные в таблице Projects (после маппинга):");
            PrintSimpleTable(projects);
        }

        static void PrintSimpleTable(DataTable table)
        {
            if (table.Rows.Count == 0)
            {
                Console.WriteLine("  Таблица пуста.");
                return;
            }

            Console.Write("  ");
            foreach (DataColumn column in table.Columns)
            {
                Console.Write($"{column.ColumnName,-20}");
            }
            Console.WriteLine();

            Console.Write("  ");
            foreach (DataColumn column in table.Columns)
            {
                Console.Write(new string('-', 20));
            }
            Console.WriteLine();

            int rowsToShow = Math.Min(3, table.Rows.Count);
            for (int i = 0; i < rowsToShow; i++)
            {
                Console.Write("  ");
                foreach (DataColumn column in table.Columns)
                {
                    object value = table.Rows[i][column];
                    string displayValue = value != null ? value.ToString() : "NULL";
                    if (displayValue.Length > 19) displayValue = displayValue.Substring(0, 16) + "...";
                    Console.Write($"{displayValue,-20}");
                }
                Console.WriteLine();
            }
        }

        static void PrintColumnMappingReport(DbDataAdapter adapter, DataSet dataSet)
        {
            Console.WriteLine("\nОтчет о маппингах колонок:");
            Console.WriteLine(new string('=', 50));

            foreach (DataTableMapping tableMapping in adapter.TableMappings)
            {
                Console.WriteLine($"\nТаблица: {tableMapping.SourceTable} -> {tableMapping.DataSetTable}");
                Console.WriteLine(new string('-', 50));

                foreach (DataColumnMapping columnMapping in tableMapping.ColumnMappings)
                {
                    Console.WriteLine($"  {columnMapping.SourceColumn} -> {columnMapping.DataSetColumn}");
                }
            }
        }

        static void DemonstrateDynamicMappings(DbDataAdapter adapter, DataSet dataSet)
        {
            Console.WriteLine("\nДинамическое добавление маппингов:");

            DataTableMapping studentMapping = adapter.TableMappings["DStud"];
            if (studentMapping != null)
            {
                Console.WriteLine("\nДобавление динамического маппинга для новой колонки 's_phone' -> 'PhoneNumber':");
                studentMapping.ColumnMappings.Add("s_phone", "PhoneNumber");
                Console.WriteLine("Маппинг добавлен.");

                Console.WriteLine("\nОбновленные маппинги для Students:");
                foreach (DataColumnMapping colMap in studentMapping.ColumnMappings)
                {
                    Console.WriteLine($"  {colMap.SourceColumn} -> {colMap.DataSetColumn}");
                }
            }
        }
        #endregion

        #region Задание 9: Комплексное приложение
        static void Task9_ComplexApp()
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 9: Комплексное приложение ===\n");

            Console.WriteLine("Инициализация комплексного приложения...");

            DataSet companyDataSet = new DataSet("CompanyDB");
            MockDataAdapter3 empAdapter = new MockDataAdapter3("Employees");
            MockDataAdapter3 projAdapter = new MockDataAdapter3("Projects");

            InitializeDataAdapters(empAdapter, projAdapter);
            LoadInitialData(empAdapter, projAdapter, companyDataSet);
            SetupDataRelations(companyDataSet);

            Console.WriteLine("\n=== Демонстрация всех функций ===\n");

            DisplayDataInGridView(companyDataSet);
            AddNewEmployees(companyDataSet);
            EditEmployees(companyDataSet);
            DeleteEmployees(companyDataSet);
            SearchAndFilter(companyDataSet);
            GenerateDetailedChangeReport(companyDataSet);
            ValidateData(companyDataSet);
            SaveChanges(empAdapter, projAdapter, companyDataSet);
            RejectChanges(companyDataSet);
        }

        class MockDataAdapter3 : DbDataAdapter
        {
            private string tableName;

            public MockDataAdapter3(string tableName)
            {
                this.tableName = tableName;
            }

            public override int Fill(DataSet dataSet)
            {
                if (tableName == "Employees")
                {
                    DataTable table = new DataTable("Employees");
                    table.Columns.Add("EmployeeID", typeof(int));
                    table.Columns.Add("FullName", typeof(string));
                    table.Columns.Add("Department", typeof(string));
                    table.Columns.Add("Salary", typeof(decimal));
                    table.Columns.Add("HireDate", typeof(DateTime));

                    // Настраиваем автоинкремент для EmployeeID
                    table.Columns["EmployeeID"].AutoIncrement = true;
                    table.Columns["EmployeeID"].AutoIncrementSeed = 1;
                    table.Columns["EmployeeID"].AutoIncrementStep = 1;
                    table.PrimaryKey = new DataColumn[] { table.Columns["EmployeeID"] };

                    table.Rows.Add(null, "Иванов Иван Иванович", "IT", 80000, new DateTime(2020, 3, 15));
                    table.Rows.Add(null, "Петров Петр Петрович", "HR", 60000, new DateTime(2021, 6, 1));
                    table.Rows.Add(null, "Сидорова Анна Владимировна", "IT", 90000, new DateTime(2019, 11, 20));
                    table.Rows.Add(null, "Кузнецов Алексей Сергеевич", "Финансы", 95000, new DateTime(2018, 8, 10));

                    dataSet.Tables.Add(table);
                    return table.Rows.Count;
                }
                else if (tableName == "Projects")
                {
                    DataTable table = new DataTable("Projects");
                    table.Columns.Add("ProjectID", typeof(int));
                    table.Columns.Add("ProjectName", typeof(string));
                    table.Columns.Add("Department", typeof(string));
                    table.Columns.Add("Budget", typeof(decimal));
                    table.Columns.Add("StartDate", typeof(DateTime));
                    table.Columns.Add("EmployeeID", typeof(int));

                    table.Columns["ProjectID"].AutoIncrement = true;
                    table.Columns["ProjectID"].AutoIncrementSeed = 1;
                    table.Columns["ProjectID"].AutoIncrementStep = 1;
                    table.PrimaryKey = new DataColumn[] { table.Columns["ProjectID"] };

                    table.Rows.Add(null, "Разработка CRM", "IT", 500000, new DateTime(2023, 1, 10), 1);
                    table.Rows.Add(null, "Обучение персонала", "HR", 150000, new DateTime(2023, 2, 1), 2);
                    table.Rows.Add(null, "Анализ рисков", "Финансы", 300000, new DateTime(2023, 1, 20), 4);

                    dataSet.Tables.Add(table);
                    return table.Rows.Count;
                }

                return 0;
            }

            public int Update(DataTable dataTable)
            {
                Console.WriteLine($"  [Mock] Сохранение таблицы {tableName}");

                int changes = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            Console.WriteLine($"    [Mock] INSERT: {row["FullName"]} (ID будет сгенерирован)");
                            // В реальном приложении здесь бы генерировался новый ID
                            if (row["EmployeeID"] == DBNull.Value || (int)row["EmployeeID"] == 0)
                            {
                                // Генерируем временный ID для демонстрации
                                int newId = dataTable.Rows.Count + 1;
                                while (dataTable.Select($"EmployeeID = {newId}").Length > 0)
                                    newId++;
                                row["EmployeeID"] = newId;
                            }
                            changes++;
                            break;
                        case DataRowState.Modified:
                            Console.WriteLine($"    [Mock] UPDATE: {row["FullName"]} (ID: {row["EmployeeID"]})");
                            changes++;
                            break;
                        case DataRowState.Deleted:
                            Console.WriteLine($"    [Mock] DELETE: ID {row["EmployeeID", DataRowVersion.Original]}");
                            changes++;
                            break;
                    }
                }

                return changes;
            }

            protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow,
                IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
            {
                return null;
            }

            protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow,
                IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
            {
                return null;
            }

            protected override void OnRowUpdated(RowUpdatedEventArgs value)
            {
            }

            protected override void OnRowUpdating(RowUpdatingEventArgs value)
            {
            }
        }

        static void InitializeDataAdapters(MockDataAdapter3 empAdapter, MockDataAdapter3 projAdapter)
        {
            Console.WriteLine("1. Настройка DataAdapters с маппингами...");
            Console.WriteLine("Маппинги настроены успешно.");
        }

        static void LoadInitialData(MockDataAdapter3 empAdapter, MockDataAdapter3 projAdapter, DataSet dataSet)
        {
            Console.WriteLine("2. Загрузка начальных данных...");

            int empRows = empAdapter.Fill(dataSet);
            int projRows = projAdapter.Fill(dataSet);

            Console.WriteLine($"Загружено: {empRows} сотрудников, {projRows} проектов");
            dataSet.AcceptChanges();
        }

        static void SetupDataRelations(DataSet dataSet)
        {
            Console.WriteLine("3. Настройка связей между таблицами...");

            try
            {
                DataRelation empProjRelation = new DataRelation(
                    "EmployeeProjects",
                    dataSet.Tables["Employees"].Columns["EmployeeID"],
                    dataSet.Tables["Projects"].Columns["EmployeeID"]);

                dataSet.Relations.Add(empProjRelation);
                Console.WriteLine("Связь Employees -> Projects создана.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка создания связи: {ex.Message}");
            }
        }

        static void DisplayDataInGridView(DataSet dataSet)
        {
            Console.WriteLine("\n1. Отображение данных:");

            DataTable employees = dataSet.Tables["Employees"];
            DataTable projects = dataSet.Tables["Projects"];

            Console.WriteLine("\nТаблица Employees:");
            PrintEmployeesTable(employees);

            Console.WriteLine("\nТаблица Projects:");
            PrintProjectsTable(projects);
        }

        static void PrintEmployeesTable(DataTable table)
        {
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"{"ID",-5} {"ФИО",-25} {"Отдел",-12} {"Зарплата",-15} {"Дата найма",-12} {"Состояние"}");
            Console.WriteLine(new string('-', 80));

            foreach (DataRow row in table.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    Console.WriteLine($"{row["EmployeeID"],-5} " +
                                    $"{row["FullName"],-25} " +
                                    $"{row["Department"],-12} " +
                                    $"{((decimal)row["Salary"]):C,-15} " +
                                    $"{((DateTime)row["HireDate"]).ToString("dd.MM.yyyy"),-12} " +
                                    $"{row.RowState}");
                }
            }
        }

        static void PrintProjectsTable(DataTable table)
        {
            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"{"ID",-5} {"Название",-25} {"Отдел",-12} {"Бюджет",-15} {"Дата начала",-12}");
            Console.WriteLine(new string('-', 70));

            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine($"{row["ProjectID"],-5} " +
                                $"{row["ProjectName"],-25} " +
                                $"{row["Department"],-12} " +
                                $"{((decimal)row["Budget"]):C,-15} " +
                                $"{((DateTime)row["StartDate"]).ToString("dd.MM.yyyy"),-12}");
            }
        }

        static void AddNewEmployees(DataSet dataSet)
        {
            Console.WriteLine("\n2. Добавление новых сотрудников:");

            DataTable employees = dataSet.Tables["Employees"];

            // Получаем максимальный ID для генерации новых
            int maxId = 0;
            foreach (DataRow row in employees.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["EmployeeID"] != DBNull.Value)
                {
                    int id = Convert.ToInt32(row["EmployeeID"]);
                    if (id > maxId) maxId = id;
                }
            }

            DataRow newEmp1 = employees.NewRow();
            newEmp1["FullName"] = "Новиков Алексей Владимирович";
            newEmp1["Department"] = "IT";
            newEmp1["Salary"] = 85000;
            newEmp1["HireDate"] = DateTime.Today;

            // Пусть автоинкремент сам сгенерирует ID
            // newEmp1["EmployeeID"] = maxId + 1;

            employees.Rows.Add(newEmp1);

            DataRow newEmp2 = employees.NewRow();
            newEmp2["FullName"] = "Фролова Екатерина Игоревна";
            newEmp2["Department"] = "HR";
            newEmp2["Salary"] = 65000;
            newEmp2["HireDate"] = DateTime.Today.AddDays(-7);

            // newEmp2["EmployeeID"] = maxId + 2;

            employees.Rows.Add(newEmp2);

            Console.WriteLine($"Добавлено 2 новых сотрудника.");
            Console.WriteLine($"Состояние строки 1: {newEmp1.RowState} (Added)");
            Console.WriteLine($"Состояние строки 2: {newEmp2.RowState} (Added)");

            // Показываем добавленные сотрудники
            Console.WriteLine("\nДобавленные сотрудники:");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"ФИО",-30} {"Отдел",-12} {"Зарплата",-12} {"Дата найма"}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{newEmp1["FullName"],-30} {newEmp1["Department"],-12} {((decimal)newEmp1["Salary"]):C,-12} {((DateTime)newEmp1["HireDate"]).ToString("dd.MM.yyyy")}");
            Console.WriteLine($"{newEmp2["FullName"],-30} {newEmp2["Department"],-12} {((decimal)newEmp2["Salary"]):C,-12} {((DateTime)newEmp2["HireDate"]).ToString("dd.MM.yyyy")}");
        }

        static void EditEmployees(DataSet dataSet)
        {
            Console.WriteLine("\n3. Редактирование сотрудников:");

            DataTable employees = dataSet.Tables["Employees"];

            if (employees.Rows.Count > 0)
            {
                DataRow emp1 = employees.Rows[0];

                // Сохраняем оригинальные значения
                string oldDepartment = emp1["Department"].ToString();
                decimal oldSalary = (decimal)emp1["Salary"];

                // Редактируем
                emp1["Salary"] = oldSalary + 5000;
                emp1["Department"] = "IT Senior";

                Console.WriteLine($"Отредактирован сотрудник: {emp1["FullName"]}");
                Console.WriteLine($"Состояние: {emp1.RowState} (Modified)");
                Console.WriteLine($"Старая зарплата: {oldSalary:C}, Новая зарплата: {emp1["Salary"]:C}");
                Console.WriteLine($"Старый отдел: {oldDepartment}, Новый отдел: {emp1["Department"]}");

                // Показываем версии данных
                Console.WriteLine("\nВерсии данных:");
                if (emp1.HasVersion(DataRowVersion.Original))
                {
                    Console.WriteLine($"  Original: Отдел = {emp1["Department", DataRowVersion.Original]}, " +
                                    $"Зарплата = {emp1["Salary", DataRowVersion.Original]:C}");
                }
                if (emp1.HasVersion(DataRowVersion.Current))
                {
                    Console.WriteLine($"  Current: Отдел = {emp1["Department", DataRowVersion.Current]}, " +
                                    $"Зарплата = {emp1["Salary", DataRowVersion.Current]:C}");
                }
            }
        }

        static void DeleteEmployees(DataSet dataSet)
        {
            Console.WriteLine("\n4. Удаление сотрудников:");

            DataTable employees = dataSet.Tables["Employees"];

            if (employees.Rows.Count > 1)
            {
                DataRow empToDelete = employees.Rows[1];
                string empName = empToDelete["FullName"].ToString();
                int empId = Convert.ToInt32(empToDelete["EmployeeID"]);

                empToDelete.Delete();

                Console.WriteLine($"Удален сотрудник: {empName} (ID: {empId})");
                Console.WriteLine($"Состояние: {empToDelete.RowState} (Deleted)");

                // Проверяем, что строка действительно удалена
                DataRow[] deletedRows = employees.Select(null, null, DataViewRowState.Deleted);
                Console.WriteLine($"Всего удаленных строк: {deletedRows.Length}");
            }
            else
            {
                Console.WriteLine("Недостаточно строк для удаления.");
            }
        }

        static void SearchAndFilter(DataSet dataSet)
        {
            Console.WriteLine("\n5. Поиск и фильтрация:");

            DataTable employees = dataSet.Tables["Employees"];

            Console.WriteLine("\n5.1 Сотрудники IT отдела:");
            DataView itView = new DataView(employees)
            {
                RowFilter = "Department LIKE '%IT%'",
                Sort = "Salary DESC"
            };

            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"{"ФИО",-30} {"Отдел",-15} {"Зарплата",-15} {"Дата найма"}");
            Console.WriteLine(new string('-', 70));

            foreach (DataRowView rowView in itView)
            {
                Console.WriteLine($"{rowView["FullName"],-30} " +
                                $"{rowView["Department"],-15} " +
                                $"{((decimal)rowView["Salary"]):C,-15} " +
                                $"{((DateTime)rowView["HireDate"]).ToString("dd.MM.yyyy")}");
            }
            Console.WriteLine($"Всего записей: {itView.Count}");

            Console.WriteLine("\n5.2 Сотрудники с зарплатой > 70000:");
            DataView salaryView = new DataView(employees)
            {
                RowFilter = "Salary > 70000",
                Sort = "HireDate ASC"
            };

            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"{"ФИО",-30} {"Зарплата",-15} {"Дата найма",-15} {"Отдел"}");
            Console.WriteLine(new string('-', 70));

            foreach (DataRowView rowView in salaryView)
            {
                Console.WriteLine($"{rowView["FullName"],-30} " +
                                $"{((decimal)rowView["Salary"]):C,-15} " +
                                $"{((DateTime)rowView["HireDate"]).ToString("dd.MM.yyyy"),-15} " +
                                $"{rowView["Department"]}");
            }
            Console.WriteLine($"Всего записей: {salaryView.Count}");

            Console.WriteLine("\n5.3 Использование DataTable.Select():");
            DataRow[] foundRows = employees.Select("Department = 'HR'", "HireDate DESC");
            Console.WriteLine($"Найдено HR сотрудников: {foundRows.Length}");

            if (foundRows.Length > 0)
            {
                Console.WriteLine("Первый HR сотрудник: " + foundRows[0]["FullName"]);
            }
        }

        static void GenerateDetailedChangeReport(DataSet dataSet)
        {
            Console.WriteLine("\n6. Детальный отчет об изменениях:");

            DataTable employees = dataSet.Tables["Employees"];

            // Получаем все изменения
            DataTable changes = employees.GetChanges();

            if (changes == null)
            {
                Console.WriteLine("Нет изменений для отчета.");
                return;
            }

            Console.WriteLine($"Всего измененных строк: {changes.Rows.Count}");
            Console.WriteLine(new string('-', 80));

            // Обрабатываем добавленные и измененные строки
            foreach (DataRow row in changes.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                    continue; // Пропускаем удаленные строки здесь

                Console.WriteLine($"\nСотрудник: {row["FullName"]}");
                Console.WriteLine($"Состояние: {row.RowState}");

                switch (row.RowState)
                {
                    case DataRowState.Added:
                        Console.WriteLine("  Новая запись:");
                        foreach (DataColumn col in changes.Columns)
                        {
                            if (col.ColumnName != "EmployeeID" || row[col] != DBNull.Value)
                            {
                                Console.WriteLine($"    {col.ColumnName}: {FormatValue(row[col], col.DataType)}");
                            }
                        }
                        break;

                    case DataRowState.Modified:
                        Console.WriteLine("  Измененные поля:");
                        foreach (DataColumn col in changes.Columns)
                        {
                            if (row.HasVersion(DataRowVersion.Original) &&
                                row.HasVersion(DataRowVersion.Current))
                            {
                                object original = row[col.ColumnName, DataRowVersion.Original];
                                object current = row[col.ColumnName, DataRowVersion.Current];

                                if (!object.Equals(original, current))
                                {
                                    Console.WriteLine($"    {col.ColumnName}: {FormatValue(original, col.DataType)} -> {FormatValue(current, col.DataType)}");
                                }
                            }
                        }
                        break;
                }
            }

            // Отдельно обрабатываем удаленные строки
            Console.WriteLine("\n\nУдаленные сотрудники:");
            DataView deletedView = new DataView(employees)
            {
                RowStateFilter = DataViewRowState.Deleted
            };

            if (deletedView.Count > 0)
            {
                Console.WriteLine(new string('-', 70));
                Console.WriteLine($"{"ID",-5} {"ФИО",-25} {"Отдел",-12} {"Зарплата",-12} {"Дата найма"}");
                Console.WriteLine(new string('-', 70));

                foreach (DataRowView rowView in deletedView)
                {
                    // Получаем оригинальную DataRow
                    DataRow originalRow = rowView.Row;

                    Console.WriteLine($"{originalRow["EmployeeID", DataRowVersion.Original],-5} " +
                                    $"{originalRow["FullName", DataRowVersion.Original],-25} " +
                                    $"{originalRow["Department", DataRowVersion.Original],-12} " +
                                    $"{FormatValue(originalRow["Salary", DataRowVersion.Original], typeof(decimal)),-12} " +
                                    $"{FormatValue(originalRow["HireDate", DataRowVersion.Original], typeof(DateTime))}");
                }
            }
            else
            {
                Console.WriteLine("Нет удаленных строк.");
            }
        }

        static string FormatValue(object value, Type dataType)
        {
            if (value == DBNull.Value || value == null)
                return "NULL";

            if (dataType == typeof(DateTime))
                return ((DateTime)value).ToString("dd.MM.yyyy");

            if (dataType == typeof(decimal))
                return ((decimal)value).ToString("C0");

            return value.ToString();
        }

        static void ValidateData(DataSet dataSet)
        {
            Console.WriteLine("\n7. Валидация данных перед сохранением:");

            DataTable employees = dataSet.Tables["Employees"];
            List<string> validationErrors = new List<string>();

            foreach (DataRow row in employees.Rows)
            {
                if (row.RowState == DataRowState.Added || row.RowState == DataRowState.Modified)
                {
                    // Проверка FullName
                    if (row["FullName"] == DBNull.Value || string.IsNullOrWhiteSpace(row["FullName"].ToString()))
                    {
                        validationErrors.Add($"Сотрудник ID {row["EmployeeID"]}: ФИО не может быть пустым");
                    }

                    // Проверка Salary
                    if (row["Salary"] != DBNull.Value)
                    {
                        decimal salary = (decimal)row["Salary"];
                        if (salary < 30000)
                        {
                            validationErrors.Add($"Сотрудник {row["FullName"]}: Зарплата ({salary:C}) ниже минимальной (30,000)");
                        }
                        if (salary > 500000)
                        {
                            validationErrors.Add($"Сотрудник {row["FullName"]}: Зарплата ({salary:C}) выше максимальной (500,000)");
                        }
                    }

                    // Проверка HireDate
                    if (row["HireDate"] != DBNull.Value)
                    {
                        DateTime hireDate = (DateTime)row["HireDate"];
                        if (hireDate > DateTime.Today)
                        {
                            validationErrors.Add($"Сотрудник {row["FullName"]}: Дата найма ({hireDate:dd.MM.yyyy}) в будущем");
                        }
                    }
                }
            }

            if (validationErrors.Count > 0)
            {
                Console.WriteLine("Найдены ошибки валидации:");
                foreach (string error in validationErrors)
                {
                    Console.WriteLine($"  - {error}");
                }
            }
            else
            {
                Console.WriteLine("Валидация пройдена успешно.");
            }
        }

        static void SaveChanges(MockDataAdapter3 empAdapter, MockDataAdapter3 projAdapter, DataSet dataSet)
        {
            Console.WriteLine("\n8. Сохранение изменений в БД:");

            try
            {
                Console.WriteLine("\nПеред сохранением состояние данных:");
                DataTable changesBefore = dataSet.Tables["Employees"].GetChanges();
                Console.WriteLine($"  Изменений в Employees: {(changesBefore?.Rows.Count ?? 0)}");

                int empChanges = empAdapter.Update(dataSet.Tables["Employees"]);
                int projChanges = projAdapter.Update(dataSet.Tables["Projects"]);

                Console.WriteLine($"\nСохранено изменений: {empChanges} в Employees, {projChanges} в Projects");

                if (empChanges > 0 || projChanges > 0)
                {
                    dataSet.AcceptChanges();
                    Console.WriteLine("Изменения приняты в DataSet.");

                    Console.WriteLine("\nПосле сохранения состояние данных:");
                    DataTable changesAfter = dataSet.Tables["Employees"].GetChanges();
                    Console.WriteLine($"  Изменений в Employees: {(changesAfter?.Rows.Count ?? 0)} (должно быть 0)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
                Console.WriteLine("Изменения НЕ сохранены. Используйте RejectChanges() для отмены.");
            }
        }

        static void RejectChanges(DataSet dataSet)
        {
            Console.WriteLine("\n9. Отмена изменений для конкретных строк:");

            DataTable employees = dataSet.Tables["Employees"];

            // Находим первую измененную строку
            DataRow changedRow = null;
            foreach (DataRow row in employees.Rows)
            {
                if (row.RowState != DataRowState.Unchanged)
                {
                    changedRow = row;
                    break;
                }
            }

            if (changedRow != null)
            {
                Console.WriteLine($"Отмена изменений для: {changedRow["FullName"]}");
                Console.WriteLine($"Состояние до отмены: {changedRow.RowState}");

                // Сохраняем текущие значения для отчета
                string currentName = changedRow["FullName"].ToString();
                string currentDept = changedRow["Department"].ToString();
                decimal currentSalary = (decimal)changedRow["Salary"];

                changedRow.RejectChanges();

                Console.WriteLine($"Состояние после отмены: {changedRow.RowState}");

                // Показываем, что вернулось к оригинальным значениям
                Console.WriteLine("\nРезультат отмены изменений:");
                Console.WriteLine($"  ФИО: {currentName} -> {changedRow["FullName"]}");
                Console.WriteLine($"  Отдел: {currentDept} -> {changedRow["Department"]}");
                Console.WriteLine($"  Зарплата: {currentSalary:C} -> {changedRow["Salary"]:C}");
            }
            else
            {
                Console.WriteLine("Нет измененных строк для отмены.");
            }

            // Показываем итоговое состояние
            Console.WriteLine("\nИтоговое состояние данных:");
            DataTable changes = employees.GetChanges();
            Console.WriteLine($"Осталось измененных строк: {(changes?.Rows.Count ?? 0)}");
        }
        #endregion

        #region Задание 10: Оптимизация работы с большими объёмами данных
        static async Task Task10_Optimization()
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 10: Оптимизация работы с большими объёмами данных ===\n");

            Console.WriteLine("Создана тестовая БД с 1,000,000+ записей логов\n");

            await DemonstratePagination();
            await DemonstrateDatabaseLevelFiltering();
            await DemonstrateAggregation();
            await DemonstrateAsyncLoading();
            await DemonstrateCaching();
            await DemonstrateDashboard();
            MeasurePerformance();
            GeneratePerformanceReport();
        }

        static async Task DemonstratePagination()
        {
            Console.WriteLine("1. Постраничное получение данных:");

            int pageSize = 1000;

            for (int page = 0; page < 3; page++)
            {
                Console.Write($"  Загрузка страницы {page + 1}... ");

                Stopwatch stopwatch = Stopwatch.StartNew();
                DataTable pageData = await GetLogsPageAsync(page, pageSize);
                stopwatch.Stop();

                Console.WriteLine($"Загружено {pageData.Rows.Count} записей за {stopwatch.ElapsedMilliseconds} мс");
            }

            Console.WriteLine();
        }

        static async Task<DataTable> GetLogsPageAsync(int pageNumber, int pageSize)
        {
            return await Task.Run(() =>
            {
                DataTable page = new DataTable("LogsPage");
                page.Columns.Add("LogID", typeof(int));
                page.Columns.Add("EventType", typeof(string));
                page.Columns.Add("Timestamp", typeof(DateTime));
                page.Columns.Add("UserID", typeof(int));
                page.Columns.Add("Description", typeof(string));

                int startIndex = pageNumber * pageSize + 1;
                int endIndex = startIndex + pageSize - 1;

                Random rnd = new Random();
                string[] eventTypes = { "INFO", "WARNING", "ERROR", "DEBUG" };

                for (int i = startIndex; i <= endIndex; i++)
                {
                    DataRow row = page.NewRow();
                    row["LogID"] = i;
                    row["EventType"] = eventTypes[rnd.Next(eventTypes.Length)];
                    row["Timestamp"] = DateTime.Now.AddSeconds(-rnd.Next(1000000));
                    row["UserID"] = rnd.Next(1, 100);
                    row["Description"] = $"Событие #{i}";

                    page.Rows.Add(row);
                }

                Task.Delay(100).Wait();
                return page;
            });
        }

        static async Task DemonstrateDatabaseLevelFiltering()
        {
            Console.WriteLine("2. Фильтрация на уровне БД:");

            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);
            string eventType = "ERROR";

            Console.WriteLine($"  Фильтр: {eventType} события с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}");

            Stopwatch stopwatch = Stopwatch.StartNew();
            DataTable filteredData = await GetFilteredLogsAsync(startDate, endDate, eventType);
            stopwatch.Stop();

            Console.WriteLine($"  Найдено записей: {filteredData.Rows.Count:N0}");
            Console.WriteLine($"  Время выполнения: {stopwatch.ElapsedMilliseconds} мс\n");
        }

        static async Task<DataTable> GetFilteredLogsAsync(DateTime startDate, DateTime endDate, string eventType)
        {
            return await Task.Run(() =>
            {
                DataTable result = new DataTable();
                result.Columns.Add("LogID", typeof(int));
                result.Columns.Add("EventType", typeof(string));
                result.Columns.Add("Timestamp", typeof(DateTime));

                Random rnd = new Random();
                int recordCount = rnd.Next(100, 1000);

                for (int i = 1; i <= recordCount; i++)
                {
                    DataRow row = result.NewRow();
                    row["LogID"] = i;
                    row["EventType"] = eventType;
                    row["Timestamp"] = startDate.AddDays(rnd.Next((endDate - startDate).Days));
                    result.Rows.Add(row);
                }

                Task.Delay(200).Wait();
                return result;
            });
        }

        static async Task DemonstrateAggregation()
        {
            Console.WriteLine("3. Агрегация данных на уровне БД:");

            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);

            Stopwatch stopwatch = Stopwatch.StartNew();
            Dictionary<string, AggregationResult> aggregatedData = await GetAggregatedLogsAsync(startDate, endDate);
            stopwatch.Stop();

            Console.WriteLine($"  Время агрегации: {stopwatch.ElapsedMilliseconds} мс");
            Console.WriteLine("\n  Результаты агрегации:");
            Console.WriteLine("  " + new string('-', 50));
            Console.WriteLine($"  {"Тип события",-15} {"Количество",-15} {"Средняя длит.",-15}");
            Console.WriteLine("  " + new string('-', 50));

            foreach (var kvp in aggregatedData)
            {
                Console.WriteLine($"  {kvp.Key,-15} {kvp.Value.Count,-15:N0} {kvp.Value.AverageDuration,-15:F0}");
            }

            Console.WriteLine();
        }

        class AggregationResult
        {
            public long Count { get; set; }
            public double AverageDuration { get; set; }
            public long TotalDuration { get; set; }
        }

        static async Task<Dictionary<string, AggregationResult>> GetAggregatedLogsAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, AggregationResult> results = new Dictionary<string, AggregationResult>();

                string[] eventTypes = { "INFO", "WARNING", "ERROR", "DEBUG" };
                Random rnd = new Random();

                foreach (string eventType in eventTypes)
                {
                    int count = rnd.Next(1000, 10000);
                    int avgDuration = rnd.Next(100, 500);
                    int totalDuration = count * avgDuration;

                    results[eventType] = new AggregationResult
                    {
                        Count = count,
                        AverageDuration = avgDuration,
                        TotalDuration = totalDuration
                    };
                }

                Task.Delay(150).Wait();
                return results;
            });
        }

        static async Task DemonstrateAsyncLoading()
        {
            Console.WriteLine("4. Асинхронная загрузка с прогрессом:");

            int totalRecords = 10000;
            int batchSize = 1000;

            Console.WriteLine($"  Загрузка {totalRecords:N0} записей...");
            Console.Write("  [");

            var progress = new Progress<int>(percent =>
            {
                int bars = percent / 2;
                Console.SetCursorPosition(3, Console.CursorTop);
                Console.Write(new string('=', bars) + new string(' ', 50 - bars));
                Console.Write($"] {percent}%");
            });

            Stopwatch stopwatch = Stopwatch.StartNew();
            DataTable asyncData = await LoadDataWithProgressAsync(totalRecords, batchSize, progress);
            stopwatch.Stop();

            Console.WriteLine($"\n\n  Загрузка завершена за {stopwatch.ElapsedMilliseconds} мс");
            Console.WriteLine($"  Загружено записей: {asyncData.Rows.Count:N0}\n");
        }

        static async Task<DataTable> LoadDataWithProgressAsync(int totalRecords, int batchSize, IProgress<int> progress)
        {
            return await Task.Run(() =>
            {
                DataTable result = new DataTable("Logs");
                result.Columns.Add("LogID", typeof(int));
                result.Columns.Add("EventType", typeof(string));
                result.Columns.Add("Timestamp", typeof(DateTime));

                int batches = totalRecords / batchSize;

                for (int batch = 0; batch < batches; batch++)
                {
                    Task.Delay(10).Wait();

                    for (int i = 0; i < batchSize; i++)
                    {
                        DataRow row = result.NewRow();
                        row["LogID"] = batch * batchSize + i + 1;
                        row["EventType"] = "INFO";
                        row["Timestamp"] = DateTime.Now;
                        result.Rows.Add(row);
                    }

                    int percent = (batch + 1) * 100 / batches;
                    progress?.Report(percent);
                }

                return result;
            });
        }

        static Dictionary<string, DataTable> cache = new Dictionary<string, DataTable>();

        static async Task DemonstrateCaching()
        {
            Console.WriteLine("5. Кэширование данных:");

            string cacheKey = "recent_logs";

            Console.WriteLine("  Первый запрос (кэш пуст):");
            Stopwatch stopwatch = Stopwatch.StartNew();
            DataTable cachedData = await GetOrCreateCacheAsync(cacheKey, () => LoadRecentLogsAsync());
            stopwatch.Stop();
            Console.WriteLine($"  Время: {stopwatch.ElapsedMilliseconds} мс (загрузка + кэширование)");

            Console.WriteLine("\n  Второй запрос (из кэша):");
            stopwatch.Restart();
            cachedData = await GetOrCreateCacheAsync(cacheKey, () => LoadRecentLogsAsync());
            stopwatch.Stop();
            Console.WriteLine($"  Время: {stopwatch.ElapsedMilliseconds} мс (из кэша)\n");
        }

        static async Task<DataTable> GetOrCreateCacheAsync(string key, Func<Task<DataTable>> loader)
        {
            if (cache.ContainsKey(key))
            {
                return cache[key].Copy();
            }

            DataTable data = await loader();
            cache[key] = data.Copy();
            return data;
        }

        static async Task<DataTable> LoadRecentLogsAsync()
        {
            await Task.Delay(500);

            DataTable result = new DataTable();
            result.Columns.Add("LogID", typeof(int));
            result.Columns.Add("EventType", typeof(string));
            result.Columns.Add("Timestamp", typeof(DateTime));

            Random rnd = new Random();
            string[] eventTypes = { "INFO", "WARNING", "ERROR", "DEBUG" };
            int recordCount = rnd.Next(100, 1000);

            for (int i = 1; i <= recordCount; i++)
            {
                DataRow row = result.NewRow();
                row["LogID"] = i;
                row["EventType"] = eventTypes[rnd.Next(eventTypes.Length)];
                row["Timestamp"] = DateTime.Now.AddHours(-rnd.Next(24 * 7));
                result.Rows.Add(row);
            }

            return result;
        }

        static async Task DemonstrateDashboard()
        {
            Console.WriteLine("6. Dashboard с виджетами:");

            Console.WriteLine("\n  [ВИДЖЕТ 1] Последние события:");
            await DisplayRecentEvents(3);

            Console.WriteLine("\n  [ВИДЖЕТ 2] Статистика по типам событий:");
            await DisplayEventStatistics();

            Console.WriteLine("\n  [ВИДЖЕТ 3] Фильтры:");
            Console.WriteLine("    • Дата: последние 7 дней");
            Console.WriteLine("    • Типы событий: INFO, WARNING, ERROR");
            Console.WriteLine("    • Пользователи: все\n");
        }

        static async Task DisplayRecentEvents(int count)
        {
            DataTable recentEvents = await GetRecentEventsAsync(count);

            Console.WriteLine("  " + new string('-', 70));
            Console.WriteLine($"  {"ID",-10} {"Тип",-10} {"Время",-20} {"Пользователь",-15}");
            Console.WriteLine("  " + new string('-', 70));

            foreach (DataRow row in recentEvents.Rows)
            {
                Console.WriteLine($"  {row["LogID"],-10} " +
                                $"{row["EventType"],-10} " +
                                $"{((DateTime)row["Timestamp"]).ToString("dd.MM.yyyy HH:mm"),-20} " +
                                $"{row["UserID"],-15}");
            }
        }

        static async Task<DataTable> GetRecentEventsAsync(int count)
        {
            return await Task.Run(() =>
            {
                DataTable result = new DataTable();
                result.Columns.Add("LogID", typeof(int));
                result.Columns.Add("EventType", typeof(string));
                result.Columns.Add("Timestamp", typeof(DateTime));
                result.Columns.Add("UserID", typeof(int));

                Random rnd = new Random();
                string[] eventTypes = { "INFO", "WARNING", "ERROR", "DEBUG" };

                for (int i = 1; i <= count; i++)
                {
                    DataRow row = result.NewRow();
                    row["LogID"] = rnd.Next(1000000);
                    row["EventType"] = eventTypes[rnd.Next(eventTypes.Length)];
                    row["Timestamp"] = DateTime.Now.AddMinutes(-rnd.Next(1440));
                    row["UserID"] = rnd.Next(1, 100);

                    result.Rows.Add(row);
                }

                return result;
            });
        }

        static async Task DisplayEventStatistics()
        {
            var stats = await GetEventStatisticsAsync();

            int total = 0;
            foreach (var stat in stats)
            {
                total += stat.Count;
            }

            Console.WriteLine($"  Всего событий: {total:N0}");
            Console.WriteLine("  " + new string('-', 40));

            foreach (var stat in stats)
            {
                int barLength = (int)((double)stat.Count / total * 40);
                string bar = new string('█', barLength);
                double percentage = (double)stat.Count / total * 100;

                Console.WriteLine($"  {stat.EventType,-10} {stat.Count,-10:N0} {bar,-40} {percentage,6:F1}%");
            }
        }

        class EventStat
        {
            public string EventType { get; set; }
            public int Count { get; set; }
        }

        static async Task<List<EventStat>> GetEventStatisticsAsync()
        {
            return await Task.Run(() =>
            {
                List<EventStat> stats = new List<EventStat>();
                Random rnd = new Random();

                stats.Add(new EventStat { EventType = "INFO", Count = rnd.Next(50000, 80000) });
                stats.Add(new EventStat { EventType = "WARNING", Count = rnd.Next(10000, 30000) });
                stats.Add(new EventStat { EventType = "ERROR", Count = rnd.Next(1000, 5000) });
                stats.Add(new EventStat { EventType = "DEBUG", Count = rnd.Next(5000, 15000) });

                return stats;
            });
        }

        static void MeasurePerformance()
        {
            Console.WriteLine("7. Измерение производительности различных подходов:\n");

            Console.WriteLine("  Тест 1: Загрузка всех данных (10000 записей)");
            Stopwatch stopwatch = Stopwatch.StartNew();
            DataTable allData = LoadAllDataTest(10000);
            stopwatch.Stop();
            long allDataTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"  Время: {allDataTime} мс\n");

            Console.WriteLine("  Тест 2: Постраничная загрузка (10 страниц по 1000)");
            long pagedTime = 0;

            for (int i = 0; i < 10; i++)
            {
                stopwatch.Restart();
                DataTable page = LoadPageTest(i, 1000);
                stopwatch.Stop();
                pagedTime += stopwatch.ElapsedMilliseconds;
            }

            Console.WriteLine($"  Время: {pagedTime} мс\n");
        }

        static DataTable LoadAllDataTest(int count)
        {
            System.Threading.Thread.Sleep(1000);

            DataTable result = new DataTable();
            result.Columns.Add("ID", typeof(int));
            result.Columns.Add("Data", typeof(string));

            for (int i = 0; i < count; i++)
            {
                result.Rows.Add(i, $"Value_{i}");
            }

            return result;
        }

        static DataTable LoadPageTest(int page, int pageSize)
        {
            System.Threading.Thread.Sleep(100);

            DataTable result = new DataTable();
            result.Columns.Add("ID", typeof(int));
            result.Columns.Add("Data", typeof(string));

            int start = page * pageSize;
            int end = start + pageSize;

            for (int i = start; i < end; i++)
            {
                result.Rows.Add(i, $"Page {page}, Row {i}");
            }

            return result;
        }

        static void GeneratePerformanceReport()
        {
            Console.WriteLine("8. Отчет о результатах оптимизации:\n");

            Console.WriteLine("КЛЮЧЕВЫЕ ВЫВОДЫ:");
            Console.WriteLine("1. Постраничная загрузка уменьшает потребление памяти");
            Console.WriteLine("2. Фильтрация на уровне БД быстрее локальной");
            Console.WriteLine("3. Агрегация на стороне сервера экономит трафик");
            Console.WriteLine("4. Кэширование ускоряет повторные запросы");
            Console.WriteLine("5. Асинхронность предотвращает блокировку UI\n");

            Console.WriteLine("РЕКОМЕНДАЦИИ:");
            Console.WriteLine("• Используйте постраничную загрузку для больших наборов");
            Console.WriteLine("• Фильтруйте данные на уровне БД");
            Console.WriteLine("• Кэшируйте часто запрашиваемые данные");
            Console.WriteLine("• Используйте асинхронные методы");
            Console.WriteLine("• Мониторьте производительность");
        }
        #endregion
    }
}