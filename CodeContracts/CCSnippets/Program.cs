using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
class Program
{

    static void Main(string[] args) {
        var thing = new SmallThing();
        var letter = thing.GetLetter();
        Console.WriteLine(letter);
        Console.ReadKey();
    }

}

public interface IThing
{
    char GetLetter(int n);
}


public abstract class BaseThing
{

    public virtual int GetValue() {
        Contract.Ensures(Contract.Result<int>() >= 0);
        return 10;
    }

    public abstract char GetLetter(int n);
}

public class SmallThing : IThing
{
    public int GetValue() {
        Contract.Ensures(Contract.Result<int>() < 4);
        return 3;
    }

    public char GetLetter(int n) {
        Contract.Requires(n < 9);
        return "milksteak"[n];
    }

    public char GetLetter() {
        return GetLetter(-1);
    }

}

}
