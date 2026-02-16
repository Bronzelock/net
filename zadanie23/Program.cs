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
            Console.WriteLine("Linq\n");
            
            var studenci = new List<Student>
            {
                new Student { NumerIndeksu = 101, Imie = "Jan", Nazwisko = "Kowalski", Wiek = 22, Plec = Plec.Mezczyzna, RokStudiow = 3, Semestr = 5 },
                new Student { NumerIndeksu = 102, Imie = "Anna", Nazwisko = "Nowak", Wiek = 21, Plec = Plec.Kobieta, RokStudiow = 3, Semestr = 5 },
                new Student { NumerIndeksu = 103, Imie = "Piotr", Nazwisko = "Wiśniewski", Wiek = 23, Plec = Plec.Mezczyzna, RokStudiow = 4, Semestr = 7 },
                new Student { NumerIndeksu = 104, Imie = "Maria", Nazwisko = "Lewandowska", Wiek = 20, Plec = Plec.Kobieta, RokStudiow = 2, Semestr = 3 },
                new Student { NumerIndeksu = 105, Imie = "Tomasz", Nazwisko = "Wójcik", Wiek = 24, Plec = Plec.Mezczyzna, RokStudiow = 4, Semestr = 7 },
                new Student { NumerIndeksu = 106, Imie = "Katarzyna", Nazwisko = "Kamińska", Wiek = 21, Plec = Plec.Kobieta, RokStudiow = 3, Semestr = 5 },
            };
            
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
            
            Console.WriteLine("Połaczone dane studentów i ocen:");
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
            
            Console.WriteLine("\nStudenci z wyższym wiekiem niż średnia:");
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
            
            Console.WriteLine("\nStudenci z wyższą średnią ocen niż na roku:");
            Console.WriteLine("--------------------------------------------------");
            
            var srednieStudentow = from o in oceny
                                  group o by o.NumerIndeksu into g
                                  select new
                                  {
                                      NumerIndeksu = g.Key,
                                      SredniaStudenta = g.Average(o => o.Ocena)
                                  };
            
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