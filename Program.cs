using System;
using System.Text;
using System.Globalization;

namespace RepairService
{
    internal class Program
    {
        const double HourRate = 350.0;
        const double PartsCost = 200.0;
        const double MaxSurcharge = 0.30;
        
        const int MaxTableSize = 20;

        static readonly string[] DeviceTypes = { "Смартфон", "Ноутбук", "Планшет", "Телевізор", "Принтер" };
        static readonly string[] Months = { "Січ", "Лют", "Бер", "Кві", "Тра", "Чер",
            "Лип", "Сер", "Вер", "Жов", "Лис", "Гру" };
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentCulture = new CultureInfo("uk-UA");
            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();
                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": ShowCostTable(); Pause(); break;
                    case "3": ShowRequestsTable(); Pause(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Немає такого пункту. Спробуйте ще раз."); Pause(); break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== Сервіс ремонту техніки ===");
            Console.WriteLine("1 — довідка про програму");
            Console.WriteLine("2 — вартість ремонту залежно від часу");
            Console.WriteLine("3 — заявки за типами пристроїв і місяцями");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }
        static void ShowAbout()
        {
            string[] names = { "Номер заявки", "Клієнт", "Пристрій", "Несправність",
                "Дата прийому", "Дата видачі", "Вартість",
                "Код статусу (довідник)" };
            string[] types = { "int", "string", "string", "string",
                "DateTime", "DateTime", "double", "int" };

            Console.WriteLine("Тема: Сервіс ремонту техніки.");
            Console.WriteLine("Сутності: Користувач, Заявка, Статус (довідник).");
            Console.WriteLine($"Поля сутності «Заявка» ({names.Length}):");
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine("{0,2}. {1,-24}{2}", i + 1, names[i], types[i]);
            }
            Console.WriteLine("Підсумок: середній термін ремонту");
            Console.WriteLine("за типами пристроїв.");
            Console.WriteLine("Місткість сховища: 100 записів.");
        }

        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();
                if (int.TryParse(text, out int value) && value >= min && value <= max) return value;
                Console.WriteLine($"Помилка: введіть ціле число від {min} до {max}.");
            }
        }

        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();
                if (double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out double value)
                    && !double.IsNaN(value) && !double.IsInfinity(value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"Помилка: введіть число від {min} до {max}.");
            }
        }

        static double CalculateCost(double hours)
        {
            double work = HourRate * hours;
            double surcharge = Math.Min(MaxSurcharge, 0.05 * hours);
            return work * (1 + surcharge) + PartsCost;
        }

        static void PrintCostTable(double start, double end, double step)
        {
            Console.WriteLine("\nВартість ремонту залежно від часу роботи (грн)");
            Console.WriteLine("{0,8}{1,14}", "Годин", "З деталями");
            for (double hours = start; hours <= end + 1e-9; hours += step)
                Console.WriteLine("{0,8:F2}{1,14:F2}", hours, CalculateCost(hours));
        }

        static void ShowCostTable()
        {
            double start = ReadDouble("Початок (години): ", 0, 1000);
            double end = ReadDouble("Кінець (години): ", start, 1000);
            double step = ReadDouble("Крок (години): ", 0.01, 1000);
            if ((end - start) / step > 200)
            {
                Console.WriteLine("Занадто багато рядків. Збільште крок або зменште межі.");
                return;
            }
            PrintCostTable(start, end, step);
        }


        static string RowName(int index)
        {
            return index < DeviceTypes.Length ? DeviceTypes[index] : "Тип " + (index + 1);
        }

        static string ColumnName(int index)
        {
            return index < Months.Length ? Months[index] : "М" + (index + 1);
        }

// умовні дані: справжні заявки з'являться разом зі сховищем
        static int RequestCount(int row, int col)
        {
            return (row * 5 + col * 3 + row * col) % 9 + 1;
        }

        static void ShowRequestsTable()
        {
            int rows = ReadInt("Кількість рядків (типів): ", 1, MaxTableSize);
            int cols = ReadInt("Кількість стовпців (місяців): ", 1, MaxTableSize);
            int[] colTotals = new int[cols];
            int grandTotal = 0;
            string line = new string('-', 11 + cols * 5 + 7);
            Console.WriteLine("\nПрийнято заявок за типами пристроїв і місяцями");
            Console.Write("{0,-11}", "Тип");
            for (int c = 0; c < cols; c++) Console.Write("{0,5}", ColumnName(c));
            Console.WriteLine("{0,7}", "Разом");
            Console.WriteLine(line);
            for (int r = 0; r < rows; r++)
            {
                int rowTotal = 0;
                Console.Write("{0,-11}", RowName(r));
                for (int c = 0; c < cols; c++)
                {
                    int value = RequestCount(r, c);
                    rowTotal += value;
                    colTotals[c] += value;
                    Console.Write("{0,5}", value);
                }
                grandTotal += rowTotal;
                Console.WriteLine("{0,7}", rowTotal);
            }
            Console.WriteLine(line);
            Console.Write("{0,-11}", "Разом");
            for (int c = 0; c < cols; c++) Console.Write("{0,5}", colTotals[c]);
            Console.WriteLine("{0,7}", grandTotal);
        }
    }
}