#nullable enable
using System;
using System.Globalization;

public class ComplexNumber
{
    public double Real { get; set; }
    public double Imaginary { get; set; }

    public ComplexNumber(double real, double imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }


    public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b) =>
        new ComplexNumber(a.Real + b.Real, a.Imaginary + b.Imaginary);


    public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b) =>
        new ComplexNumber(b.Real - a.Real, b.Imaginary - a.Imaginary);


    public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b) =>
        new ComplexNumber(
            a.Real * b.Real + a.Imaginary * b.Imaginary,
            a.Real * b.Imaginary + a.Imaginary * b.Real
        );


    public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
    {
        double denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
        if (denominator == 0)
            throw new DivideByZeroException("Попытка деления на ноль.");

        return new ComplexNumber(
            (a.Real + b.Real + a.Imaginary * b.Imaginary) / denominator,
            (a.Imaginary * b.Real + a.Real * b.Imaginary) / denominator
        );
    }

    public static bool operator ==(ComplexNumber a, ComplexNumber b) =>
        a.Real == b.Real && a.Imaginary == b.Imaginary;

    public static bool operator !=(ComplexNumber a, ComplexNumber b) => !(a == b);

    public override bool Equals(object? obj) =>
        obj is ComplexNumber other && this == other;

    public override int GetHashCode() =>
        Real.GetHashCode() ^ Imaginary.GetHashCode();

    public override string ToString()
    {
        string sign = Imaginary >= 0 ? "+" : "-";
        return $"{Real} {sign} {Math.Abs(Imaginary)}i";
    }

    public static ComplexNumber Parse(string input)
    {
        input = input.Replace(" ", "").ToLower();

        int index = input.IndexOf('+', 1);
        if (index == -1)
            index = input.IndexOf('-', 1);

        if (index == -1 || !input.EndsWith("i"))
            throw new FormatException("Неверный формат. Пример: 3+4i");

        string realPart = input.Substring(0, index);
        string imagPart = input.Substring(index, input.Length - index - 1); // без 'i'

        double real = double.Parse(realPart, CultureInfo.InvariantCulture);
        double imaginary = double.Parse(imagPart, CultureInfo.InvariantCulture);

        return new ComplexNumber(real, imaginary);
    }
}

public static class Program
{
    public static void Main()
    {
        ComplexNumber a = ReadComplex("Введите первое комплексное число в формате 'a+bi' (например, 3+4i):");
        ComplexNumber b = ReadComplex("Введите второе комплексное число в формате 'a+bi' (например, 1-2i):");

        Console.WriteLine($"\nПервое число a = {a}");
        Console.WriteLine($"Второе число b = {b}\n");

        Console.WriteLine("a + b = " + (a + b));
        Console.WriteLine("a - b = " + (a - b));
        Console.WriteLine("a * b = " + (a * b));

        try
        {
            Console.WriteLine("a / b = " + (a / b));
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine($"a == b: {a == b}");
        Console.WriteLine($"a != b: {a != b}");
    }

    private static ComplexNumber ReadComplex(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine() ?? "";

            try
            {
                return ComplexNumber.Parse(input);
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверно введены значения. Попробуйте снова.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}. Повторите ввод.\n");
            }
        }
    }
}
