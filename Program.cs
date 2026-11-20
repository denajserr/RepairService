using System;
using System.Globalization;
using System.Text;

namespace RepairService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentCulture = new CultureInfo("uk-UA");

            const int Capacity = 100;
            double[] values = new double[Capacity];
            int count = 0;
            bool running = true;

            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        if (count >= Capacity)
                        {
                            Console.WriteLine("Масив заповнений.");
                            break;
                        }

                        Console.Write("Введіть значення вартості ремонту: ");
                        string input = Console.ReadLine();

                        if (!double.TryParse(input, out double newValue)
                            || double.IsNaN(newValue)
                            || double.IsInfinity(newValue)
                            || newValue < 0)
                        {
                            Console.WriteLine("Помилка: введіть невід’ємне число.");
                            break;
                        }

                        if (AddValue(values, ref count, newValue, Capacity))
                            Console.WriteLine($"Значення додано. У масиві {count} записів.");
                        else
                            Console.WriteLine("Не вдалося додати значення.");

                        break;

                    case "2":
                        PrintValues(values, count);
                        break;

                    case "0":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nНатисніть будь-яку клавішу...");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== Сервіс ремонту техніки ===");
            Console.WriteLine("1 — додати значення вартості ремонту");
            Console.WriteLine("2 — показати значення");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        // Метод записує значення в масив і змінює лічильник через ref.
        static bool AddValue(double[] values, ref int count, double newValue, int capacity)
        {
            if (count >= capacity || count >= values.Length)
                return false;

            values[count] = newValue;
            count++;
            return true;
        }

        // Метод отримує масив і кількість заповнених елементів параметрами.
        static void PrintValues(double[] values, int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Масив порожній.");
                return;
            }

            Console.WriteLine("№\tВартість ремонту, грн");
            for (int i = 0; i < count; i++)
                Console.WriteLine($"{i + 1}\t{values[i]:F2}");
        }
    }
}