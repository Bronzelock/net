using System;
using System.Collections.Generic;
using System.Linq;

namespace Zadanie3_StudentLINQ
{
    enum Plec { Kobieta, Mezczyzna }
    
    class Student
    {
        public int NumerIndeksu { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public int Wiek { get; set; }
        public Plec Plec { get; set; }
        public int RokStudiow { get; set; }
        public int Semestr { get; set; }
        
        public override string ToString()
        {
            return $"{NumerIndeksu} | {Imie} {Nazwisko} | {Wiek} lat | {Plec} | Rok {RokStudiow} | Semestr {Semestr}";
        }
    }
    
    class Degree
    {
        public int NumerIndeksu { get; set; }
        public string Przedmiot { get; set; }
        public double Ocena { get; set; }
        public int RokZaliczenia { get; set; }
        public int Semestr { get; set; }
        
        public override string ToString()
        {
            return $"{NumerIndeksu} | {Przedmiot} | Ocena: {Ocena} | Rok: {RokZaliczenia} | Semestr: {Semestr}";
        }
    }
    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== ZADANIE 3 - STUDENT I DEGREE LINQ ===\n");
            
            // Przygotowanie danych studentów
            var studenci = new List<Student>
            {
                new Student { NumerIndeksu = 101, Imie = "Jan", Nazwisko = "Kowalski", Wiek = 22, Plec = Plec.Mezczyzna, RokStudiow = 3, Semestr = 5 },
                new Student { NumerIndeksu = 102, Imie = "Anna", Nazwisko = "Nowak", Wiek = 21, Plec = Plec.Kobieta, RokStudiow = 3, Semestr = 5 },
                new Student { NumerIndeksu = 103, Imie = "Piotr", Nazwisko = "Wiśniewski", Wiek = 23, Plec = Plec.Mezczyzna, RokStudiow = 4, Semestr = 7 },
                new Student { NumerIndeksu = 104, Imie = "Maria", Nazwisko = "Lewandowska", Wiek = 20, Plec = Plec.Kobieta, RokStudiow = 2, Semestr = 3 },
                new Student { NumerIndeksu = 105, Imie = "Tomasz", Nazwisko = "Wójcik", Wiek = 24, Plec = Plec.Mezczyzna, RokStudiow = 4, Semestr = 7 },
                new Student { NumerIndeksu = 106, Imie = "Katarzyna", Nazwisko = "Kamińska", Wiek = 21, Plec = Plec.Kobieta, RokStudiow = 3, Semestr = 5 },
            };
            
            // Przygotowanie danych ocen
            var oceny = new List<Degree>
            {
                new Degree { NumerIndeksu = 101, Przedmiot = "Matematyka", Ocena = 4.5, RokZaliczenia = 2023, Semestr = 5 },
                new Degree { NumerIndeksu = 101, Przedmiot = "Programowanie", Ocena = 5.0, RokZaliczenia = 2023, Semestr = 5 },
                new Degree { NumerIndeksu = 102, Przedmiot = "Matematyka", Ocena = 4.0, RokZaliczenia = 2023, Semestr = 5 },
                new Degree { NumerIndeksu = 102, Przedmiot = "Programowanie", Ocena = 4.5, RokZaliczenia = 2023, Semestr = 5 },
                new Degree { NumerIndeksu = 103, Przedmiot = "Matematyka", Ocena = 3.5, RokZaliczenia = 2023, Semestr = 7 },
                new Degree { NumerIndeksu = 103, Przedmiot = "Programowanie", Ocena = 4.0, RokZaliczenia = 2023, Semestr = 7 },
                new Degree { NumerIndeksu = 104, Przedmiot = "Matematyka", Ocena = 5.0, RokZaliczenia = 2023, Semestr = 3 },
                new Degree { NumerIndeksu = 104, Przedmiot = "Programowanie", Ocena = 4.5, RokZaliczenia = 2023, Semestr = 3 },
                new Degree { NumerIndeksu = 105, Przedmiot = "Matematyka", Ocena = 4.0, RokZaliczenia = 2023, Semestr = 7 },
                new Degree { NumerIndeksu = 105, Przedmiot = "Programowanie", Ocena = 3.5, RokZaliczenia = 2023, Semestr = 7 },
                new Degree { NumerIndeksu = 106, Przedmiot = "Matematyka", Ocena = 4.5, RokZaliczenia = 2023, Semestr = 5 },
                new Degree { NumerIndeksu = 106, Przedmiot = "Programowanie", Ocena = 5.0, RokZaliczenia = 2023, Semestr = 5 },
            };
            
            // 1. Połączenie danych z klasy Student i Degree
            Console.WriteLine("1. POŁĄCZONE DANE STUDENTÓW I ICH OCEN:");
            Console.WriteLine("----------------------------------------");
            
            var polaczone = from s in studenci
                           join o in oceny on s.NumerIndeksu equals o.NumerIndeksu
                           select new
                           {
                               s.NumerIndeksu,
                               s.Imie,
                               s.Nazwisko,
                               s.Wiek,
                               s.RokStudiow,
                               o.Przedmiot,
                               o.Ocena
                           };
            
            foreach (var item in polaczone)
            {
                Console.WriteLine($"{item.NumerIndeksu} | {item.Imie} {item.Nazwisko} | {item.Wiek} lat | Rok {item.RokStudiow} | {item.Przedmiot} | Ocena: {item.Ocena}");
            }
            
            // 2. Wyświetl studentów, których wiek jest większy niż średnia dla studentów na roku
            Console.WriteLine("\n2. STUDENCI Z WIEKIEM WYŻSZYM OD ŚREDNIEJ NA SWOIM ROKU:");
            Console.WriteLine("-----------------------------------------------------");
            
            var sredniWiekNaRoku = studenci
                .GroupBy(s => s.RokStudiow)
                .Select(g => new { Rok = g.Key, SredniWiek = g.Average(s => s.Wiek) });
            
            var starsiStudenci = from s in studenci
                                 join sw in sredniWiekNaRoku on s.RokStudiow equals sw.Rok
                                 where s.Wiek > sw.SredniWiek
                                 select s;
            
            foreach (var s in starsiStudenci)
            {
                var sredniaRoku = sredniWiekNaRoku.First(x => x.Rok == s.RokStudiow).SredniWiek;
                Console.WriteLine($"{s} | Średnia na roku: {sredniaRoku:F2}");
            }
            
            // 3. Wyświetl studentów, których średnia ocen jest większa niż pozostałych na roku
            Console.WriteLine("\n3. STUDENCI ZE ŚREDNIĄ WYŻSZĄ OD ŚREDNIEJ ROCZNIKA:");
            Console.WriteLine("--------------------------------------------------");
            
            // Obliczamy średnią dla każdego studenta
            var srednieStudentow = from o in oceny
                                  group o by o.NumerIndeksu into g
                                  select new
                                  {
                                      NumerIndeksu = g.Key,
                                      SredniaStudenta = g.Average(o => o.Ocena)
                                  };
            
            // Łączymy ze studentami żeby dostać rok studiów
            var studenciZeSrednia = from s in studenci
                                    join ss in srednieStudentow on s.NumerIndeksu equals ss.NumerIndeksu
                                    select new
                                    {
                                        s.NumerIndeksu,
                                        s.Imie,
                                        s.Nazwisko,
                                        s.RokStudiow,
                                        ss.SredniaStudenta
                                    };
            
            // Obliczamy średnią na roku (bez studenta)
            var wynik = from s in studenciZeSrednia
                       group s by s.RokStudiow into rocznik
                       from student in rocznik
                       let sredniaReszty = rocznik
                           .Where(x => x.NumerIndeksu != student.NumerIndeksu)
                           .Average(x => (double?)x.SredniaStudenta) ?? 0
                       where student.SredniaStudenta > sredniaReszty
                       select new
                       {
                           student.NumerIndeksu,
                           student.Imie,
                           student.Nazwisko,
                           student.RokStudiow,
                           student.SredniaStudenta,
                           SredniaReszty = sredniaReszty
                       };
            
            foreach (var item in wynik)
            {
                Console.WriteLine($"{item.NumerIndeksu} | {item.Imie} {item.Nazwisko} | Rok {item.RokStudiow} | Średnia: {item.SredniaStudenta:F2} | Średnia reszty: {item.SredniaReszty:F2}");
            }
            
            Console.ReadKey();
        }
    }
}