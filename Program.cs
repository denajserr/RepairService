using System;
using System.Text;
using System.Globalization;

namespace RepairService
{
    internal class Program
    {
        const int Capacity = 100;
        const double HourRate = 350.0;
        const double PartsCost = 200.0;
        const double MaxSurcharge = 0.30;

        static readonly string[] StatusNames = { "Прийнято", "У ремонті", "Видано" };

        // Паралельні масиви зберігають записи заявок без власних класів і колекцій.
        static readonly int[] RequestNumbers = new int[Capacity];
        static readonly string[] Clients = new string[Capacity];
        static readonly string[] Devices = new string[Capacity];
        static readonly string[] Faults = new string[Capacity];
        static readonly DateTime[] ReceivedDates = new DateTime[Capacity];
        static readonly DateTime[] IssuedDates = new DateTime[Capacity];
        static readonly double[] Costs = new double[Capacity];
        static readonly int[] StatusCodes = new int[Capacity];
        static int requestCount;

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
                    case "2": AddRequest(); Pause(); break;
                    case "3": ShowRequests(); Pause(); break;
                    case "4": ShowAverageRepairTime(); Pause(); break;
                    case "5": ShowCostTable(); Pause(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Немає такого пункту. Спробуйте ще раз."); Pause(); break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== Сервіс ремонту техніки ===");
            Console.WriteLine("1 — про програму");
            Console.WriteLine("2 — додати заявку");
            Console.WriteLine("3 — переглянути заявки");
            Console.WriteLine("4 — середній термін ремонту за пристроями");
            Console.WriteLine("5 — таблиця вартості ремонту");
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
            string[] fields = { "Номер заявки — int", "Клієнт — string", "Пристрій — string",
                "Несправність — string", "Дата прийому — DateTime", "Дата видачі — DateTime",
                "Вартість — double", "Код статусу — int" };

            Console.WriteLine("Тема: Сервіс ремонту техніки.");
            Console.WriteLine("Сутності: Користувач, Заявка, Статус (довідник).");
            Console.WriteLine("Поля сутності «Заявка»:");
            for (int i = 0; i < fields.Length; i++) Console.WriteLine($"{i + 1}. {fields[i]}");
            Console.WriteLine("Підсумкова величина: середній термін ремонту за типами пристроїв.");
            Console.WriteLine($"Місткість сховища: {Capacity}; зайнято: {requestCount}.");
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

        static string ReadText(string prompt, int maxLength)
        {
            while (true)
            {
                Console.Write(prompt);
                string value = Console.ReadLine().Trim();
                if (value.Length > 0 && value.Length <= maxLength) return value;
                Console.WriteLine($"Помилка: текст має містити від 1 до {maxLength} символів.");
            }
        }

        static DateTime ReadDate(string prompt, DateTime min, DateTime max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();
                if (DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime date)
                    && date.Date >= min.Date && date.Date <= max.Date) return date.Date;
                Console.WriteLine($"Помилка: введіть дату від {min:dd.MM.yyyy} до {max:dd.MM.yyyy}.");
            }
        }

        static void AddRequest()
        {
            if (requestCount >= Capacity)
            {
                Console.WriteLine("Сховище заявок заповнене.");
                return;
            }

            int i = requestCount;
            RequestNumbers[i] = i + 1;
            Clients[i] = ReadText("Клієнт: ", 40);
            Devices[i] = ReadText("Тип пристрою: ", 30);
            Faults[i] = ReadText("Несправність: ", 100);
            ReceivedDates[i] = ReadDate("Дата прийому (дд.мм.рррр): ", DateTime.MinValue, DateTime.Today);
            IssuedDates[i] = ReadDate("Дата видачі (дд.мм.рррр): ", ReceivedDates[i], DateTime.MaxValue);
            Costs[i] = ReadDouble("Вартість ремонту (грн): ", 0, 10000000);
            Console.WriteLine("Статуси: 1 — Прийнято, 2 — У ремонті, 3 — Видано.");
            StatusCodes[i] = ReadInt("Код статусу: ", 1, StatusNames.Length);
            requestCount++;
            Console.WriteLine($"Заявку №{RequestNumbers[i]} збережено. Зайнято {requestCount} із {Capacity} місць.");
        }

        static void ShowRequests()
        {
            if (requestCount == 0) { Console.WriteLine("Заявок поки немає."); return; }

            Console.WriteLine("№  Клієнт              Пристрій        Прийом      Видача      Вартість   Статус");
            for (int i = 0; i < requestCount; i++)
                Console.WriteLine($"{RequestNumbers[i],-3}{Clients[i],-20}{Devices[i],-16}" +
                    $"{ReceivedDates[i]:dd.MM.yyyy}  {IssuedDates[i]:dd.MM.yyyy}  " +
                    $"{Costs[i],9:F2}  {StatusNames[StatusCodes[i] - 1]}");
        }

        // Повертає середню кількість днів ремонту для заданого типу пристрою.
        static double CalculateAverageDuration(string device)
        {
            double totalDays = 0;
            int count = 0;
            for (int i = 0; i < requestCount; i++)
            {
                if (string.Equals(Devices[i], device, StringComparison.CurrentCultureIgnoreCase))
                {
                    totalDays += (IssuedDates[i] - ReceivedDates[i]).TotalDays;
                    count++;
                }
            }
            return count == 0 ? -1 : totalDays / count;
        }

        static bool IsFirstDevice(int index)
        {
            for (int i = 0; i < index; i++)
                if (string.Equals(Devices[i], Devices[index], StringComparison.CurrentCultureIgnoreCase)) return false;
            return true;
        }

        static void ShowAverageRepairTime()
        {
            if (requestCount == 0) { Console.WriteLine("Додайте заявки, щоб обчислити середній термін."); return; }
            Console.WriteLine("Середній термін ремонту за типами пристроїв:");
            for (int i = 0; i < requestCount; i++)
                if (IsFirstDevice(i))
                    Console.WriteLine($"{Devices[i]}: {CalculateAverageDuration(Devices[i]):F1} дн.");
        }

        static double CalculateCost(double hours)
        {
            double work = HourRate * hours;
            double surcharge = Math.Min(MaxSurcharge, 0.05 * hours);
            return work * (1 + surcharge) + PartsCost;
        }

        // Табулювання отримує межі та крок як параметри, а обчислення вартості делегує окремому методу.
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
    }
}
