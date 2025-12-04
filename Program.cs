using System;
using System.Collections.Generic;
using System.Globalization;
using Lab03;

namespace LabWork
{
    class Program
    {
        // Читає double у циклі; приймає '.' або ','; 'q' — відмінити.
        static bool ReadDouble(string prompt, out double value)
        {
            value = 0;
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (s == null)
                {
                    Console.WriteLine("No input. Aborting.");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    return false;
                }

                s = s.Trim();
                if (string.Equals(s, "q", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Aborted by user.");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    return false;
                }

                // Спробуємо кілька варіантів парсингу (поточна культура, invariant з заміною ','->'.', та явно uk-UA)
                if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out value)
                    || double.TryParse(s.Replace(',', '.'), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value)
                    || double.TryParse(s.Replace('.', ','), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.GetCultureInfo("uk-UA"), out value))
                {
                    return true;
                }

                Console.WriteLine("Invalid numeric input. Use digits and optional decimal separator (',' or '.'). Enter 'q' to abort.");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Cubic polynomials minimization (sampling method)\n");

            var polynomials = new List<CubicPolynomial>
            {
                new CubicPolynomial(-1, 6, 5, -54),
                new CubicPolynomial(1, -3, 2, 1),
                new CubicPolynomial(4, 1, -4, 80),
                new CubicPolynomial(1, -5, 9, 1),
                new CubicPolynomial(-2, -2, -1, 15)
            };

            Console.WriteLine("Default polynomials:");
            for (int i = 0; i < polynomials.Count; i++)
            {
                var p = polynomials[i];
                Console.WriteLine($"{i + 1}: y = {p.A}x^3 + {p.B}x^2 + {p.C}x + {p.D}");
            }

            if (!ReadDouble("Enter interval start (f): ", out double f)) return;
            if (!ReadDouble("Enter interval end (g): ", out double g)) return;
            if (!ReadDouble("Enter precision / step E (E > 0): ", out double E)) return;

            if (f > g || E <= 0)
            {
                Console.WriteLine("Invalid interval [f, g] or non-positive precision E.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            double overallMinY = double.MaxValue;
            double overallMinX = f;
            CubicPolynomial best = null;

            // Вивести мінімум для кожного полінома та знайти кращий
            for (int i = 0; i < polynomials.Count; i++)
            {
                var poly = polynomials[i];
                var local = poly.FindMinimumOnInterval(f, g, E); // MinResult

                Console.WriteLine($"\nPolynomial {i + 1} minimum:");
                Console.WriteLine($"y = {poly.A}x^3 + {poly.B}x^2 + {poly.C}x + {poly.D}");
                Console.WriteLine($"minY = {local.MinY:F4} at x = {local.MinX:F4}");

                if (local.MinY < overallMinY)
                {
                    overallMinY = local.MinY;
                    overallMinX = local.MinX;
                    best = poly;
                }
            }

            if (best != null)
            {
                Console.WriteLine("\nBest polynomial:");
                Console.WriteLine($"y = {best.A}x^3 + {best.B}x^2 + {best.C}x + {best.D}");
                Console.WriteLine($"Minimum on [{f}; {g}]: minY = {overallMinY:F4} at x = {overallMinX:F4}");
            }
            else
            {
                Console.WriteLine("No valid polynomial found.");
            }

            // --- пауза перед закриттям консолі ---
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}