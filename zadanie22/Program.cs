using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ZADANIE 2 - PORÓWNANIE LIST ===\n");
        
        var a = new List<int> { 1, 2, 3, 4 };
        var b = new List<int> { 20, 30 };
        var c = new List<int> { 5, 5, 5 };
        
        Console.WriteLine($"Lista a: [1,2,3,4] -> suma = {a.Sum()}");
        Console.WriteLine($"Lista b: [20,30] -> suma = {b.Sum()}");
        Console.WriteLine($"Lista c: [5,5,5] -> suma = {c.Sum()}");
        
        Console.WriteLine($"\na < b : {a.Sum()} < {b.Sum()} = {CzyMniejsza(a, b)}");
        Console.WriteLine($"a > b : {a.Sum()} > {b.Sum()} = {CzyWieksza(a, b)}");
        Console.WriteLine($"a < c : {a.Sum()} < {c.Sum()} = {CzyMniejsza(a, c)}");
        Console.WriteLine($"a > c : {a.Sum()} > {c.Sum()} = {CzyWieksza(a, c)}");
        Console.WriteLine($"b < c : {b.Sum()} < {c.Sum()} = {CzyMniejsza(b, c)}");
        Console.WriteLine($"b > c : {b.Sum()} > {c.Sum()} = {CzyWieksza(b, c)}");
    }
    
    static bool CzyMniejsza(List<int> l1, List<int> l2)
    {
        return l1.Sum() < l2.Sum();
    }
    
    static bool CzyWieksza(List<int> l1, List<int> l2)
    {
        return l1.Sum() > l2.Sum();
    }
}