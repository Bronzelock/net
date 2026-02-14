using System;
using System.Collections.Generic;

namespace Zadanie2_SortedList
{
    // ========== INTERFEJS STRATEGII ==========
    public interface ISortStrategy
    {
        void Sort(List<string> list);
    }

    // ========== KONKRETNE STRATEGIE ==========
    public class QuickSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            list.Sort();
            Console.WriteLine("Zastosowano strategię: QuickSort");
        }
    }

    public class BubbleSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (string.Compare(list[j], list[j + 1]) > 0)
                    {
                        string temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine("Zastosowano strategię: BubbleSort");
        }
    }

    public class MergeSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            list.Sort();
            Console.WriteLine("Zastosowano strategię: MergeSort");
        }
    }

    // ========== KLASA SORTEDLIST ==========
    public class SortedList
    {
        private List<string> _items = new List<string>();
        private ISortStrategy _sortStrategy;

        public void Add(string item)
        {
            _items.Add(item);
            Console.WriteLine($"Dodano: {item}");
        }

        public void Display()
        {
            Console.WriteLine("Zawartość listy:");
            foreach (var item in _items)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
        }

        public void SetSortStrategy(ISortStrategy strategy)
        {
            _sortStrategy = strategy;
        }

        public void Sort()
        {
            if (_sortStrategy == null)
            {
                Console.WriteLine("Nie ustawiono strategii sortowania!");
                return;
            }
            _sortStrategy.Sort(_items);
        }

        public List<string> GetItems() => _items;
    }

    // ========== PROGRAM GŁÓWNY ==========
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== ZADANIE 2 - SORTEDLIST (WZORZEC STRATEGY) ===\n");

            var sortedList = new SortedList();
            
            // Dodawanie elementów
            sortedList.Add("Piotr");
            sortedList.Add("Anna");
            sortedList.Add("Zofia");
            sortedList.Add("Tomasz");
            sortedList.Add("Katarzyna");

            Console.WriteLine("\nPrzed sortowaniem:");
            sortedList.Display();

            // QuickSort
            sortedList.SetSortStrategy(new QuickSort());
            sortedList.Sort();
            sortedList.Display();

            // Dodajemy nowe elementy
            sortedList.Add("Adam");
            sortedList.Add("Ewa");
            Console.WriteLine("\nPo dodaniu elementów:");
            sortedList.Display();

            // BubbleSort
            sortedList.SetSortStrategy(new BubbleSort());
            sortedList.Sort();
            sortedList.Display();

            // MergeSort
            sortedList.SetSortStrategy(new MergeSort());
            sortedList.Sort();
            sortedList.Display();

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }
    }
}