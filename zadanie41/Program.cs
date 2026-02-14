using System;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;

namespace Zadanie1
{
    // Model
    public class EventLog
    {
        public virtual int Id { get; protected set; }
        public virtual DateTime EventDate { get; set; }
        public virtual string Source { get; set; }
        public virtual string Type { get; set; }
        public virtual string Data { get; set; }
        public virtual string IP { get; set; }
    }

    // Mapowanie (uproszczone - bez enum)
    public class EventLogMap : ClassMap<EventLog>
    {
        public EventLogMap()
        {
            Table("Events");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.EventDate).Not.Nullable();
            Map(x => x.Source).Length(100).Not.Nullable();
            Map(x => x.Type).Length(20).Not.Nullable();
            Map(x => x.Data).Length(500);
            Map(x => x.IP).Length(45);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ZADANIE 1 - CRUD ZDARZEŃ ===\n");

            try
            {
                // Tworzenie fabryki sesji
                var factory = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.UsingFile("events.db"))
                    .Mappings(m => m.FluentMappings.Add<EventLogMap>())
                    .ExposeConfiguration(c => new SchemaExport(c).Create(true, true))
                    .BuildSessionFactory();

                // CREATE
                using (var session = factory.OpenSession())
                using (var tx = session.BeginTransaction())
                {
                    session.Save(new EventLog
                    {
                        EventDate = DateTime.Now,
                        Source = "App",
                        Type = "Info",
                        Data = "Aplikacja uruchomiona",
                        IP = "127.0.0.1"
                    });

                    session.Save(new EventLog
                    {
                        EventDate = DateTime.Now.AddMinutes(-5),
                        Source = "DB",
                        Type = "Warning",
                        Data = "Wolne zapytanie",
                        IP = "192.168.1.100"
                    });

                    session.Save(new EventLog
                    {
                        EventDate = DateTime.Now.AddMinutes(-10),
                        Source = "Network",
                        Type = "Error",
                        Data = "Timeout połączenia",
                        IP = "10.0.0.50"
                    });

                    tx.Commit();
                    Console.WriteLine("Dodano 3 zdarzenia\n");
                }

                // READ - wszystkie
                using (var session = factory.OpenSession())
                {
                    var events = session.Query<EventLog>().ToList();
                    Console.WriteLine("WSZYSTKIE ZDARZENIA:");
                    foreach (var e in events)
                        Console.WriteLine($"{e.Id} | {e.EventDate:HH:mm:ss} | {e.Source} | {e.Type} | {e.IP} | {e.Data}");
                }

                // UPDATE
                using (var session = factory.OpenSession())
                using (var tx = session.BeginTransaction())
                {
                    var ev = session.Query<EventLog>().FirstOrDefault();
                    if (ev != null)
                    {
                        ev.Data += " [EDYTOWANE]";
                        session.Update(ev);
                        tx.Commit();
                        Console.WriteLine($"\nZaktualizowano zdarzenie ID={ev.Id}");
                    }
                }

                // DELETE
                using (var session = factory.OpenSession())
                using (var tx = session.BeginTransaction())
                {
                    var ev = session.Query<EventLog>().FirstOrDefault();
                    if (ev != null)
                    {
                        session.Delete(ev);
                        tx.Commit();
                        Console.WriteLine($"Usunięto zdarzenie ID={ev.Id}");
                    }
                }

                // Podsumowanie
                using (var session = factory.OpenSession())
                {
                    Console.WriteLine($"\nPozostało zdarzeń: {session.Query<EventLog>().Count()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BŁĄD: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("\nKoniec. Naciśnij dowolny klawisz...");
            Console.ReadKey();
        }
    }
}