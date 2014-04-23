using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
class Program
{

    static void Main(string[] args) {
        var thing = new SmallThing();
        var index = thing.GetValue();
        var letter = GetLetter(index);
        Console.WriteLine(letter);
        Console.ReadKey();
    }

    static char GetLetter(int n) {
        Contract.Requires(n >= 0);
        Contract.Requires(n < 9);
        return "milksteak"[n];
    }

}

public class BaseThing
{
    public virtual int GetValue() {
        Contract.Ensures(Contract.Result<int>() >= 0);
        return 10;
    }
}

public class SmallThing : BaseThing
{
    public override int GetValue() {
        Contract.Ensures(Contract.Result<int>() < 4);
        return 3;
    }
}

}
