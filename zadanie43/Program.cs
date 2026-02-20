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
    public class LogEntry
    {
        public virtual Guid Id { get; protected set; } 
        public virtual DateTime Timestamp { get; set; }
        
        public virtual string Message { get; set; } = string.Empty; 
    }

    public class LogEntryMap : ClassMap<LogEntry>
    {
        public LogEntryMap()
        {
            Table("ErrorLogs");
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Timestamp).Not.Nullable();
            Map(x => x.Message).Not.Nullable();
        }
    }

    public class ErrorLogger
    {
        private readonly ISessionFactory _mainDbFactory;
        private readonly ISessionFactory _localDbFactory;
        public bool MainDbIsDown { get; set; } = false;

        public ErrorLogger(ISessionFactory mainDbFactory, ISessionFactory localDbFactory)
        {
            _mainDbFactory = mainDbFactory;
            _localDbFactory = localDbFactory;
        }

        public void LogError(LogEntry error)
        {
            if (MainDbIsDown)
            {
                Console.WriteLine("Główna baza leży, zapisuję log lokalnie");
                using var localSession = _localDbFactory.OpenSession();
                using var localTx = localSession.BeginTransaction();
                localSession.Save(error);
                localTx.Commit();
            }
            else
            {
                Console.WriteLine("Zapisuję do głównej bazy");
                using var session = _mainDbFactory.OpenSession();
                using var tx = session.BeginTransaction();
                session.Save(error);
                tx.Commit();
                
                FlushLocalDbToMain();
            }
        }

        private void FlushLocalDbToMain()
        {
            IList<LogEntry> pendingLogs;
            using (var localSession = _localDbFactory.OpenSession())
            {
                pendingLogs = localSession.Query<LogEntry>().ToList();
                if (!pendingLogs.Any()) return;
            }

            Console.WriteLine($"[BULK LOAD] Przenoszenie {pendingLogs.Count} logów z lokalnej do głównej bazy...");
            
            var originalIds = pendingLogs.Select(x => x.Id).ToList();

            using (var mainSession = _mainDbFactory.OpenStatelessSession()) 
            using (var mainTx = mainSession.BeginTransaction())
            {
                foreach (var log in pendingLogs) mainSession.Insert(log);
                mainTx.Commit();
            }

            using (var localSession = _localDbFactory.OpenSession())
            using (var localTx = localSession.BeginTransaction())
            {
                foreach (var id in originalIds)
                {
                    var entityToDelete = localSession.Load<LogEntry>(id);
                    localSession.Delete(entityToDelete);
                }
                localTx.Commit();
            }
        }
        
        public void PokazLogiGlowne()
        {
            using var session = _mainDbFactory.OpenSession();
            var logs = session.Query<LogEntry>().ToList();
            Console.WriteLine($"\nLogi w bazie głównej ({logs.Count}):");
            foreach(var l in logs) Console.WriteLine($"- [{l.Timestamp:HH:mm:ss}] {l.Message}");
        }
    }

    class Program
    {
        private static void BuildSchema(Configuration config) => new SchemaExport(config).Create(false, true);

        private static ISessionFactory CreateSessionFactory(string dbFileName)
        {
            try
            {
                return Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.UsingFile(dbFileName))
                    .Mappings(m => m.FluentMappings.Add<LogEntryMap>())
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
            Console.WriteLine("Tworzenie bazy głównej (main.sqlite) i lokalnej (local.sqlite)...");
            var mainFactory = CreateSessionFactory("main.sqlite");
            var localFactory = CreateSessionFactory("local.sqlite");

            var logger = new ErrorLogger(mainFactory, localFactory);

            logger.LogError(new LogEntry { Timestamp = DateTime.Now, Message = "Błąd #1 - Baza działa" });

            logger.MainDbIsDown = true; 
            logger.LogError(new LogEntry { Timestamp = DateTime.Now, Message = "Błąd #2 - Zapisany awaryjnie!" });
            logger.LogError(new LogEntry { Timestamp = DateTime.Now, Message = "Błąd #3 - Zapisany awaryjnie!" });

            logger.MainDbIsDown = false;
            logger.LogError(new LogEntry { Timestamp = DateTime.Now, Message = "Błąd #4 - Baza powróciła!" });

            logger.PokazLogiGlowne();
        }
    }
}