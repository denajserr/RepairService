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
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            CultureInfo.CurrentCulture = new CultureInfo("uk-UA");

            bool running = true;
            while (running)
            {
                Console.WriteLine("=== Сервіс ремонту техніки ===");
                Console.WriteLine("1 — довідка про програму");
                Console.WriteLine("2 — вартість ремонту залежно від часу");
                Console.WriteLine("3 — заявки за типами пристроїв і місяцями");
                Console.WriteLine("0 — вихід");
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowAbout();
                        Pause();
                        break;
                    case "2":
                        ShowCostTable();
                        Pause();
                        break;
                    case "3":
                        Console.WriteLine("Поки не зроблено.");
                        Pause();
                        break;
                }
            }
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

        static void ShowCostTable()
        {
            double start = 0.5;
            double end = 6.0;
            double step = 0.5;

            Console.WriteLine("Вартість ремонту залежно від часу роботи (грн)");
            Console.WriteLine("{0,8}{1,12}{2,14}", "Годин", "Робота", "З деталями");

            // запас 1e-9 захищає останній рядок від похибки дробових чисел
            for (double hours = start; hours <= end + 1e-9; hours += step)
            {
                double work = HourRate * hours;
                double surcharge = Math.Min(MaxSurcharge, 0.05 * hours);
                double total = work * (1 + surcharge) + PartsCost;
                Console.WriteLine("{0,8:F2}{1,12:F2}{2,14:F2}", hours, work, total);
            }
        }
    }
}