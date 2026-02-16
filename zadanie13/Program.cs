using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

enum Stanowisko { Programista, Tester, Manager }

class Pracownik
{
    public int Id { get; set; }
    public string Imie { get; set; }
    public string Nazwisko { get; set; }
    public int Wiek { get; set; }
    public Stanowisko Stanowisko { get; set; }
    public decimal Placa { get; set; }

    public void Pokaz() => 
        Console.WriteLine($"{Id,2} | {Imie,-10} {Nazwisko,-12} | {Wiek,2} | {Stanowisko,-10} | {Placa,8:C}");
}

class Kartoteka
{
    public List<Pracownik> Lista { get; set; } = new List<Pracownik>();

    public void Dodaj(Pracownik p) => Lista.Add(p);
    public void PokazWszystkich()
    {
        foreach (var p in Lista) p.Pokaz();
        Console.WriteLine($"Razem: {Lista.Count}");
    }

    public void ZapiszTxt(string plik)
    {
        using var sw = new StreamWriter(plik);
        foreach (var p in Lista)
            sw.WriteLine($"{p.Id}|{p.Imie}|{p.Nazwisko}|{p.Wiek}|{p.Stanowisko}|{p.Placa}");
        Console.WriteLine($"Zapisano do {plik}");
    }

    public void WczytajTxt(string plik)
    {
        Lista.Clear();
        foreach (var line in File.ReadAllLines(plik))
        {
            var d = line.Split('|');
            Lista.Add(new Pracownik
            {
                Id = int.Parse(d[0]),
                Imie = d[1],
                Nazwisko = d[2],
                Wiek = int.Parse(d[3]),
                Stanowisko = Enum.Parse<Stanowisko>(d[4]),
                Placa = decimal.Parse(d[5])
            });
        }
        Console.WriteLine($"Wczytano z {plik}");
    }

    public void ZapiszJson(string plik)
    {
        var json = JsonSerializer.Serialize(Lista, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(plik, json);
        Console.WriteLine($"Zapisano do {plik}");
    }

    public void WczytajJson(string plik)
    {
        var json = File.ReadAllText(plik);
        Lista = JsonSerializer.Deserialize<List<Pracownik>>(json);
        Console.WriteLine($"Wczytano z {plik}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Zapis do pliku\n");
        
        var k = new Kartoteka();
        k.Dodaj(new Pracownik { Id = 1, Imie = "Jan", Nazwisko = "Kowalski", Wiek = 35, Stanowisko = Stanowisko.Programista, Placa = 8500 });
        k.Dodaj(new Pracownik { Id = 2, Imie = "Anna", Nazwisko = "Nowak", Wiek = 28, Stanowisko = Stanowisko.Tester, Placa = 6200 });
        
        Console.WriteLine("Przed zapisem:");
        k.PokazWszystkich();
        
        k.ZapiszTxt("pracownicy.txt");
        k.ZapiszJson("pracownicy.json");
        
        var k2 = new Kartoteka();
        k2.WczytajTxt("pracownicy.txt");
        Console.WriteLine("\nPo odczycie z TXT:");
        k2.PokazWszystkich();
        
        Console.ReadKey();
    }
}