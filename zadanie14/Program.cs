using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Proste szyfrowanie Cezara
static class Cezar
{
    private static int przesuniecie = 3; // Możesz zmienić
    
    public static string Szyfruj(string tekst)
    {
        var wynik = new StringBuilder();
        foreach (char c in tekst)
        {
            if (char.IsLetter(c))
            {
                char offset = char.IsUpper(c) ? 'A' : 'a';
                wynik.Append((char)(((c - offset + przesuniecie) % 26) + offset));
            }
            else
                wynik.Append(c);
        }
        return wynik.ToString();
    }
    
    public static string Odszyfruj(string tekst)
    {
        var wynik = new StringBuilder();
        foreach (char c in tekst)
        {
            if (char.IsLetter(c))
            {
                char offset = char.IsUpper(c) ? 'A' : 'a';
                wynik.Append((char)(((c - offset - przesuniecie + 26) % 26) + offset));
            }
            else
                wynik.Append(c);
        }
        return wynik.ToString();
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== SZYFROWANIE CEZARA ===\n");
        
        string oryginal = "Ala ma kota";
        string zaszyfrowane = Cezar.Szyfruj(oryginal);
        string odszyfrowane = Cezar.Odszyfruj(zaszyfrowane);
        
        Console.WriteLine($"Oryginał:  {oryginal}");
        Console.WriteLine($"Zaszyfrowane (przesunięcie=3): {zaszyfrowane}");
        Console.WriteLine($"Odszyfrowane: {odszyfrowane}");
        
        // Przykład zapisu zaszyfrowanego pliku
        string tekst = "Jan Kowalski, Programista";
        File.WriteAllText("tajne.txt", Cezar.Szyfruj(tekst));
        Console.WriteLine($"\nZapisano zaszyfrowany tekst do tajne.txt");
        
        string odczyt = Cezar.Odszyfruj(File.ReadAllText("tajne.txt"));
        Console.WriteLine($"Odczytano i odszyfrowano: {odczyt}");
        
        Console.ReadKey();
    }
}