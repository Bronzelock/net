using System;
using System.Collections.Generic;

namespace Zadanie1_Wzorce
{
    public sealed class ConfigurationManager
    {
        private static readonly Lazy<ConfigurationManager> _instance = 
            new Lazy<ConfigurationManager>(() => new ConfigurationManager());

        private ConfigurationManager()
        {
            Console.WriteLine("ConfigurationManager został utworzony");
        }

        public static ConfigurationManager Instance => _instance.Value;

        public string AppName { get; set; } = "MojaAplikacja";
        public int MaxUsers { get; set; } = 100;

        public void ShowSettings()
        {
            Console.WriteLine($"Aplikacja: {AppName}, Maksymalna liczba użytkowników: {MaxUsers}");
        }
    }

    public interface IObserver
    {
        void Update(string message);
    }

    public class User : IObserver
    {
        public string Name { get; set; }

        public User(string name)
        {
            Name = name;
        }

        public void Update(string message)
        {
            Console.WriteLine($"Użytkownik {Name} otrzymał wiadomość: {message}");
        }
    }

    public class NewsAgency
    {
        private List<IObserver> _observers = new List<IObserver>();
        private string _latestNews;

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
            Console.WriteLine($"Dodano obserwatora: {((User)observer).Name}");
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
            Console.WriteLine($"Usunięto obserwatora: {((User)observer).Name}");
        }

        private void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_latestNews);
            }
        }

        public string LatestNews
        {
            get { return _latestNews; }
            set
            {
                _latestNews = value;
                Console.WriteLine($"\n[Aktualność] Nowa wiadomość: {_latestNews}");
                NotifyObservers();
            }
        }
    }

    public interface ICompressionStrategy
    {
        void Compress(string filePath);
    }

    public class ZipCompression : ICompressionStrategy
    {
        public void Compress(string filePath)
        {
            Console.WriteLine($"Kompresuję plik {filePath} używając zip");
        }
    }

    public class RarCompression : ICompressionStrategy
    {
        public void Compress(string filePath)
        {
            Console.WriteLine($"Kompresuję plik {filePath} używając rar");
        }
    }

    public class SevenZipCompression : ICompressionStrategy
    {
        public void Compress(string filePath)
        {
            Console.WriteLine($"Kompresuję plik {filePath} używając 7-zip");
        }
    }

    public class Compressor
    {
        private ICompressionStrategy _strategy;

        public void SetStrategy(ICompressionStrategy strategy)
        {
            _strategy = strategy;
            Console.WriteLine($"Zmieniono strategię na: {strategy.GetType().Name}");
        }

        public void CompressFile(string filePath)
        {
            if (_strategy == null)
            {
                Console.WriteLine("Najpierw ustaw strategię kompresji");
                return;
            }
            _strategy.Compress(filePath);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Wzorce projektowe\n");

            Console.WriteLine("singleton");
            var config1 = ConfigurationManager.Instance;
            config1.ShowSettings();

            var config2 = ConfigurationManager.Instance;
            config2.AppName = "ZmienionaNazwa";
            config1.ShowSettings();
            Console.WriteLine($"Czy to ta sama instancja? {ReferenceEquals(config1, config2)}\n");

            Console.WriteLine("observer");
            var agency = new NewsAgency();
            var user1 = new User("Alice");
            var user2 = new User("Bob");
            var user3 = new User("Charlie");

            agency.Attach(user1);
            agency.Attach(user2);
            agency.Attach(user3);

            agency.LatestNews = "Nowy breaking news";
            agency.LatestNews = "Dziś piątek";

            agency.Detach(user2);
            agency.LatestNews = "Bob już nie subskrybuje";
            Console.WriteLine();

            Console.WriteLine("strategy");
            var compressor = new Compressor();
            compressor.CompressFile("dokument.txt");

            compressor.SetStrategy(new ZipCompression());
            compressor.CompressFile("dokument.txt");

            compressor.SetStrategy(new RarCompression());
            compressor.CompressFile("zdjecie.jpg");

            compressor.SetStrategy(new SevenZipCompression());
            compressor.CompressFile("film.mp4");

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }
    }
}