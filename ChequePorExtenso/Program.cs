using System;

namespace ChequePorExtenso
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Cheque cheque = new Cheque(0.01);
            Console.WriteLine(cheque.chequePorExtenso());
            Console.ReadLine();
        }
    }
}
