using System;

// Prosta hierarchia trójkątów
abstract class Trojkat
{
    protected double a, b, c;
    
    public Trojkat(double a, double b, double c) 
    { 
        this.a = a; this.b = b; this.c = c; 
    }
    
    public abstract double Pole();
    public abstract double Obwod();
    
    public override string ToString()
    {
        return $"Boki: {a:F2}, {b:F2}, {c:F2}, Obwód: {Obwod():F2}, Pole: {Pole():F2}";
    }
}

class Rownoboczny : Trojkat
{
    public Rownoboczny(double bok) : base(bok, bok, bok) { }
    public override double Pole() => (Math.Sqrt(3) / 4) * a * a;
    public override double Obwod() => 3 * a;
}

class Rownoramienny : Trojkat
{
    public Rownoramienny(double podstawa, double ramie) : base(podstawa, ramie, ramie) { }
    public override double Pole()
    {
        double h = Math.Sqrt(b * b - (a / 2) * (a / 2));
        return (a * h) / 2;
    }
    public override double Obwod() => a + 2 * b;
}

class Prostokatny : Trojkat
{
    public Prostokatny(double a, double b) : base(a, b, Math.Sqrt(a * a + b * b)) { }
    public override double Pole() => (a * b) / 2;
    public override double Obwod() => a + b + c;
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== TRÓJKĄTY ===\n");
        
        Console.WriteLine(new Rownoboczny(5));
        Console.WriteLine(new Rownoramienny(6, 5));
        Console.WriteLine(new Prostokatny(3, 4));
        
        Console.ReadKey();
    }
}