using System;

namespace Lab03
{
    // Immutable result with PascalCase property names (encapsulation + C# conventions)
    public readonly struct MinResult
    {
        public double MinX { get; }
        public double MinY { get; }

        public MinResult(double minX, double minY)
        {
            MinX = minX;
            MinY = minY;
        }
    }

    public sealed class CubicPolynomial
    {
        // Public read-only properties — properly encapsulate internal state.
        public double A { get; }
        public double B { get; }
        public double C { get; }
        public double D { get; }

        public CubicPolynomial(double a, double b, double c, double d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        // Evaluate polynomial using Horner's method: A*x^3 + B*x^2 + C*x + D
        public double Evaluate(double x) => ((A * x + B) * x + C) * x + D;

        // Sampling-based minimization on [start, end] with step > 0.
        public MinResult FindMinimumOnInterval(double start, double end, double step)
        {
            if (step <= 0)
                throw new ArgumentOutOfRangeException(nameof(step), "Step must be > 0.");

            if (double.IsNaN(start) || double.IsNaN(end) || double.IsNaN(step))
                throw new ArgumentException("Arguments must be valid numbers.");

            // Normalize interval so sampling is increasing
            if (start > end)
            {
                var tmp = start;
                start = end;
                end = tmp;
            }

            double minX = start;
            double minY = Evaluate(start);

            // Use integer step count to avoid accumulated floating-point error
            int steps = (int)Math.Floor((end - start) / step);
            for (int i = 0; i <= steps; i++)
            {
                double x = start + i * step;
                double y = Evaluate(x);
                if (y < minY)
                {
                    minY = y;
                    minX = x;
                }
            }

            // Ensure endpoint is checked
            double yEnd = Evaluate(end);
            if (yEnd < minY)
            {
                minY = yEnd;
                minX = end;
            }

            return new MinResult(minX, minY);
        }
    }
}