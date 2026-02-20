using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NHibernateTest
{
    public enum Stanowisko { Programista, Tester, Manager }

    public class Pracownik
    {
        public virtual int Id { get; protected set; }
        public virtual string Imie { get; set; }
        public virtual string Nazwisko { get; set; }
        public virtual int Wiek { get; set; }
        public virtual Stanowisko Stanowisko { get; set; }
        public virtual decimal Placa { get; set; }

        public virtual bool Poprawny() => 
            !string.IsNullOrWhiteSpace(Imie) && 
            !string.IsNullOrWhiteSpace(Nazwisko) && 
            Wiek >= 18 && Wiek <= 100 && 
            Placa > 0;

        public virtual void Pokaz() => 
            Console.WriteLine($"{Id,2} | {Imie,-10} {Nazwisko,-12} | {Wiek,2} | {Stanowisko,-10} | {Placa,8:C}");
    }

    public class PracownikMap : ClassMap<Pracownik>
    {
        public PracownikMap()
        {
            Table("Pracownicy");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Imie).Not.Nullable();
            Map(x => x.Nazwisko).Not.Nullable();
            Map(x => x.Wiek);
            Map(x => x.Stanowisko).CustomType<Stanowisko>();
            Map(x => x.Placa);
        }
    }

    public class Kartoteka
    {
        private readonly ISessionFactory _sessionFactory;

        public Kartoteka(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool Dodaj(Pracownik p)
        {
            if (!p.Poprawny()) return false;
            
            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();
            session.Save(p);
            transaction.Commit();
            return true;
        }

        public void Wszyscy()
        {
            using var session = _sessionFactory.OpenSession();
            var lista = session.Query<Pracownik>().ToList();
            
            Console.WriteLine("\nLista pracowników w bazie danych:");
            foreach (var p in lista) p.Pokaz();
            Console.WriteLine($"Razem: {lista.Count}\n");
        }
    }

    class Program
    {
        private static void BuildSchema(Configuration config)
        {
            new SchemaExport(config).Create(false, true);
        }

        private static ISessionFactory CreateSessionFactory()
        {
            try
            {
                return Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.UsingFile("zadanie2.sqlite"))
                    .Mappings(m => m.FluentMappings.Add<PracownikMap>())
                    .ExposeConfiguration(cfg => 
                    {
                        cfg.SetProperty(NHibernate.Cfg.Environment.UseProxyValidator, "false");
                        BuildSchema(cfg);
                    })
                    .BuildSessionFactory();
            }
            catch (FluentConfigurationException ex)
            {
                Console.WriteLine("\nBłąd konfiguracji nhibernate");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Szczegóły (InnerException): " + ex.InnerException.Message);
                }
                throw;
            }
        }

        static void Main()
        {
            Console.WriteLine("Łączenie z bazą i generowanie schematu");
            
            var sessionFactory = CreateSessionFactory();
            var k = new Kartoteka(sessionFactory);
            
            Console.WriteLine("Dodawanie pracowników");
            k.Dodaj(new Pracownik { Imie = "Jan", Nazwisko = "Kowalski", Wiek = 35, Stanowisko = Stanowisko.Programista, Placa = 8500 });
            k.Dodaj(new Pracownik { Imie = "Anna", Nazwisko = "Nowak", Wiek = 28, Stanowisko = Stanowisko.Tester, Placa = 6200 });
            k.Dodaj(new Pracownik { Imie = "Piotr", Nazwisko = "Wiśniewski", Wiek = 42, Stanowisko = Stanowisko.Manager, Placa = 12000 });
            
            k.Wszyscy();
        }
    }
}