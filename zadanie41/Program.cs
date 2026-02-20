using System;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NHibernateZadania
{
    public class SignalEvent
    {
        public virtual int Id { get; protected set; }
        public virtual DateTime EventDate { get; set; }
        public virtual string Source { get; set; }
        public virtual string EventType { get; set; }
        public virtual string AdditionalData { get; set; }
        public virtual string Identifier { get; set; }
    }

    public class SignalEventMap : ClassMap<SignalEvent>
    {
        public SignalEventMap()
        {
            Table("SignalEvents");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.EventDate).Not.Nullable();
            Map(x => x.Source).Not.Nullable();
            Map(x => x.EventType)
                .Not.Nullable()
                .Check("EventType IN ('informacja', 'ostrzeżenie', 'błąd', 'błąd krytyczny')")
                .Index("idx_event_type");
            Map(x => x.AdditionalData);
            Map(x => x.Identifier).Length(45);
        }
    }

    public class SignalEventRepository
    {
        private readonly ISessionFactory _sessionFactory;
        public SignalEventRepository(ISessionFactory sessionFactory) => _sessionFactory = sessionFactory;

        public void Create(SignalEvent ev)
        {
            using var session = _sessionFactory.OpenSession();
            using var tx = session.BeginTransaction();
            session.Save(ev);
            tx.Commit();
        }

        public void showAll()
        {
            using var session = _sessionFactory.OpenSession();
            var events = session.Query<SignalEvent>().ToList();
            Console.WriteLine("\nZapisane zdarzenia");
            foreach (var e in events)
                Console.WriteLine($"[{e.EventDate:yyyy-MM-dd HH:mm}] {e.EventType.ToUpper()} ze źródła {e.Source}: {e.AdditionalData} (ID: {e.Id})");
        }
    }

    class Program
    {
        private static void BuildSchema(Configuration config) => new SchemaExport(config).Create(false, true);

        private static ISessionFactory CreateSessionFactory()
        {
            try
            {
                return Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.UsingFile("zadanie1.sqlite"))
                    .Mappings(m => m.FluentMappings.Add<SignalEventMap>())
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
            Console.WriteLine("Inicjalizacja bazy danych Zadania 1");
            var factory = CreateSessionFactory();
            var repo = new SignalEventRepository(factory);

            Console.WriteLine("Dodawanie zdarzeń...");
            repo.Create(new SignalEvent { EventDate = DateTime.Now, Source = "Serwer_A", EventType = "informacja", Identifier = "192.168.1.10", AdditionalData = "Uruchomiono usługę" });
            repo.Create(new SignalEvent { EventDate = DateTime.Now, Source = "Aplikacja_Web", EventType = "błąd", Identifier = "10.0.0.5", AdditionalData = "Brak połączenia z API" });

            repo.showAll();
        }
    }
}