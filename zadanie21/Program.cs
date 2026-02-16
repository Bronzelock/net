using System;
using System.Collections.Generic;

namespace Zadanie1_ONP
{
    abstract class OnpExpression
    {
        public abstract override string ToString();
    }

    class Value : OnpExpression
    {
        private string value;
        
        public Value(string val) { value = val; }
        public override string ToString() => value;
    }

    class Variable : OnpExpression
    {
        private string name;
        
        public Variable(string name) { this.name = name; }
        public override string ToString() => name;
    }

    class BinaryOperator : OnpExpression
    {
        private string op;
        private OnpExpression left;
        private OnpExpression right;
        
        public BinaryOperator(string op, OnpExpression left, OnpExpression right)
        {
            this.op = op;
            this.left = left;
            this.right = right;
        }
        
        public override string ToString()
        {
            return $"{left} {right} {op}";
        }
    }

    class Assignment : OnpExpression
    {
        private string variable;
        private OnpExpression expression;
        
        public Assignment(string variable, OnpExpression expr)
        {
            this.variable = variable;
            this.expression = expr;
        }
        
        public override string ToString()
        {
            return $"{variable} {expression} =";
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Konwersja na ONP\n");
            
            var a = new Variable("a");
            var b = new Variable("b");
            var c = new Variable("c");
            
            var mnozenie = new BinaryOperator("*", b, c);
            
            var odejmowanie = new BinaryOperator("-", a, mnozenie);
            
            var przypisanie = new Assignment("x", odejmowanie);
            
            Console.WriteLine($"Wyrażenie: x = a - b * c");
            Console.WriteLine($"ONP: {przypisanie}");
            
            Console.WriteLine("\nInne przykłady:");
            
            var dodawanie = new BinaryOperator("+", new Variable("a"), new Variable("b"));
            Console.WriteLine($"a + b -> {dodawanie}");
            
            var dod2 = new BinaryOperator("+", new Variable("a"), new Variable("b"));
            var mno2 = new BinaryOperator("*", dod2, new Variable("c"));
            Console.WriteLine($"(a + b) * c -> {mno2}");
            
            var piec = new Value("5");
            var trzy = new Value("3");
            var dwa = new Value("2");
            var mno3 = new BinaryOperator("*", trzy, dwa);
            var dod3 = new BinaryOperator("+", piec, mno3);
            Console.WriteLine($"5 + 3 * 2 -> {dod3}");
            
            Console.ReadKey();
        }
    }
}