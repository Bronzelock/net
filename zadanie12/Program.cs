using System;
using System.Collections.Generic;
using System.Linq;

enum Stanowisko { Programista, Tester, Manager }

class Pracownik
{
    public int Id { get; set; }
    public string Imie { get; set; }
    public string Nazwisko { get; set; }
    public int Wiek { get; set; }
    public Stanowisko Stanowisko { get; set; }
    public decimal Placa { get; set; }

    public bool Poprawny() => 
        !string.IsNullOrWhiteSpace(Imie) && 
        !string.IsNullOrWhiteSpace(Nazwisko) && 
        Wiek >= 18 && Wiek <= 100 && 
        Placa > 0;

    public bool Pasuje(string tekst) => 
        Imie.ToLower().Contains(tekst.ToLower()) || 
        Nazwisko.ToLower().Contains(tekst.ToLower());

    public void Pokaz() => 
        Console.WriteLine($"{Id,2} | {Imie,-10} {Nazwisko,-12} | {Wiek,2} | {Stanowisko,-10} | {Placa,8:C}");
}

class Kartoteka
{
    private List<Pracownik> lista = new List<Pracownik>();

    public bool Dodaj(Pracownik p)
    {
        if (!p.Poprawny()) return false;
        lista.Add(p);
        return true;
    }

    public bool Usun(int id)
    {
        var p = lista.FirstOrDefault(x => x.Id == id);
        return p != null && lista.Remove(p);
    }

    public void Wszyscy()
    {
        Console.WriteLine("\nLista pracowników");
        foreach (var p in lista) p.Pokaz();
        Console.WriteLine($"Razem: {lista.Count}\n");
    }

    public List<Pracownik> Szukaj(string tekst) => 
        lista.Where(p => p.Pasuje(tekst)).ToList();
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Kartoteka\n");
        
        var k = new Kartoteka();
        
        k.Dodaj(new Pracownik { Id = 1, Imie = "Jan", Nazwisko = "Kowalski", Wiek = 35, Stanowisko = Stanowisko.Programista, Placa = 8500 });
        k.Dodaj(new Pracownik { Id = 2, Imie = "Anna", Nazwisko = "Nowak", Wiek = 28, Stanowisko = Stanowisko.Tester, Placa = 6200 });
        k.Dodaj(new Pracownik { Id = 3, Imie = "Piotr", Nazwisko = "Wiśniewski", Wiek = 42, Stanowisko = Stanowisko.Manager, Placa = 12000 });
        
        k.Wszyscy();
        
        Console.WriteLine("Szukanie 'anna':");
        foreach (var p in k.Szukaj("anna")) p.Pokaz();
        
        Console.WriteLine("\nUsuwanie ID=2");
        k.Usun(2);
        k.Wszyscy();
        
        Console.ReadKey();
    }
}