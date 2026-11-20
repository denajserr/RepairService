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

                    case "3":
                        ShowStatistics(values, count);
                        break;

                    case "4":
                        int originalNumber = 50;
                        Console.WriteLine($"Ціле число до виклику методу: {originalNumber}");
                        IncreaseInteger(originalNumber);
                        Console.WriteLine($"Ціле число після виклику методу: {originalNumber}");

                        double[] demoValues = { 50.0 };
                        int demoCount = 1;
                        Console.WriteLine($"Елемент масиву до виклику методу: {demoValues[0]:F2}");
                        IncreaseFirstValue(demoValues, demoCount);
                        Console.WriteLine($"Елемент масиву після виклику методу: {demoValues[0]:F2}");
                        break;

                    case "5":
                        Console.Write($"Введіть номер елемента для видалення (1–{count}): ");
                        string indexInput = Console.ReadLine();

                        if (!int.TryParse(indexInput, out int number))
                        {
                            Console.WriteLine("Помилка: введіть цілий номер елемента.");
                            break;
                        }

                        if (RemoveAt(values, ref count, number - 1))
                            Console.WriteLine($"Елемент №{number} видалено. Залишилося записів: {count}.");
                        else
                            Console.WriteLine($"Неправильний номер. Введіть число від 1 до {count}.");

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
            Console.WriteLine("3 — показати статистику");
            Console.WriteLine("4 — дослід передавання параметрів");
            Console.WriteLine("5 — видалити значення за номером");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        static bool AddValue(double[] values, ref int count, double newValue, int capacity)
        {
            if (count >= capacity || count >= values.Length)
                return false;

            values[count] = newValue;
            count++;
            return true;
        }

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

        static bool FindMaximum(
            double[] values,
            int count,
            out double maximum,
            out int number)
        {
            if (count <= 0)
            {
                maximum = 0;
                number = -1;
                return false;
            }

            maximum = values[0];
            number = 1;

            for (int i = 1; i < count; i++)
            {
                if (values[i] > maximum)
                {
                    maximum = values[i];
                    number = i + 1;
                }
            }

            return true;
        }

        static void ShowStatistics(double[] values, int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Немає значень для обчислення статистики.");
                return;
            }

            double sum = 0;
            for (int i = 0; i < count; i++)
                sum += values[i];

            double average = sum / count;
            FindMaximum(values, count, out double maximum, out int number);

            Console.WriteLine($"Кількість значень: {count}");
            Console.WriteLine($"Сума: {sum:F2} грн");
            Console.WriteLine($"Середнє значення: {average:F2} грн");
            Console.WriteLine($"Найбільше значення: {maximum:F2} грн");
            Console.WriteLine($"Його номер у списку: {number}");
        }

        static void IncreaseInteger(int value)
        {
            value += 100;
        }

        static void IncreaseFirstValue(double[] values, int count)
        {
            if (count > 0)
                values[0] += 100;
        }

        // Видаляє елемент за індексом і зсуває наступні значення вліво.
        static bool RemoveAt(double[] values, ref int count, int index)
        {
            if (index < 0 || index >= count)
                return false;

            for (int i = index; i < count - 1; i++)
                values[i] = values[i + 1];

            count--;
            values[count] = 0;
            return true;
        }
    }
}