using System;
using System.Collections;
using System.Collections.Generic;

namespace Zadanie3_ObservableCollection
{
    public interface ICollectionObserver
    {
        void OnItemAdded(object item);
        void OnItemRemoved(object item);
        void OnListCleared();
    }

    public class ConsoleLogger : ICollectionObserver
    {
        private string _collectionName;

        public ConsoleLogger(string collectionName)
        {
            _collectionName = collectionName;
        }

        public void OnItemAdded(object item)
        {
            Console.WriteLine($"[{_collectionName}] Dodano element: {item}");
        }

        public void OnItemRemoved(object item)
        {
            Console.WriteLine($"[{_collectionName}] Usunięto element: {item}");
        }

        public void OnListCleared()
        {
            Console.WriteLine($"[{_collectionName}] Wyczyszczono całą kolekcję");
        }
    }

    public class ObservableCollection<T> : IEnumerable<T>
    {
        private List<T> _list = new List<T>();
        private List<ICollectionObserver> _observers = new List<ICollectionObserver>();

        public void Attach(ICollectionObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(ICollectionObserver observer)
        {
            _observers.Remove(observer);
        }

        private void NotifyAdd(T item)
        {
            foreach (var observer in _observers)
            {
                observer.OnItemAdded(item);
            }
        }

        private void NotifyRemove(T item)
        {
            foreach (var observer in _observers)
            {
                observer.OnItemRemoved(item);
            }
        }

        private void NotifyClear()
        {
            foreach (var observer in _observers)
            {
                observer.OnListCleared();
            }
        }

        public void Add(T item)
        {
            _list.Add(item);
            NotifyAdd(item);
        }

        public bool Remove(T item)
        {
            bool removed = _list.Remove(item);
            if (removed)
            {
                NotifyRemove(item);
            }
            return removed;
        }

        public void Clear()
        {
            _list.Clear();
            NotifyClear();
        }

        // Indekser
        public T this[int index]
        {
            get { return _list[index]; }
            set 
            { 
                _list[index] = value;
            }
        }

        public int Count => _list.Count;

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Obserwacja kolekcji\n");

            var observableList = new ObservableCollection<string>();
            var logger = new ConsoleLogger("MojaLista");

            observableList.Attach(logger);

            Console.WriteLine("Dodawanie elementów");
            observableList.Add("Apple");
            observableList.Add("Banana");
            observableList.Add("Cherry");
            observableList.Add("Date");

            Console.WriteLine("\nUsuwanie elementu");
            observableList.Remove("Banana");

            Console.WriteLine("\nCzyszczenie kolekcji");
            observableList.Clear();

            Console.WriteLine("\nDodawanie po czyszczeniu");
            observableList.Add("Xylophone");
            observableList.Add("Zebra");

            Console.WriteLine("\nIteracja po elementach kolekcji");
            foreach (var item in observableList)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();

            Console.WriteLine($"\nLiczba elementów: {observableList.Count}");

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }
    }
}