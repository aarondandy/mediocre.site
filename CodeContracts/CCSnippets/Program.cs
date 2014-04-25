using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{

class Program {
    static void Main(string[] args) {
        var sounder = new Screech();
        Console.WriteLine(sounder.LoudNoise());
    }
}

[ContractClass(typeof(NoiseMakerContracts))]
public abstract class NoiseMaker {
    public virtual string LoudNoise() {
        Contract.Ensures(Contract.Result<string>() != null);
        return Noise().ToUpper();
    }
    public abstract string Noise();
}

[ContractClassFor(typeof(NoiseMaker))]
internal abstract class NoiseMakerContracts : NoiseMaker {
    public override string Noise() {
        Contract.Ensures(Contract.Result<string>() != null);
        throw new NotImplementedException();
    }
}

public class Screech : NoiseMaker{
    public override string Noise() {
        return "screeeeeeeech";
    }
}

public class Nope : NoiseMaker {
    public override string Noise() {
        return null; // ensures is false: Contract.Result<string>() != null
    }
}

[ContractClass(typeof(SomeBaseContracts))]
public abstract class SomeBase {
    public abstract string ImportantStuff(string input);
    public abstract string NobodyCares();
}

[ContractClassFor(typeof(SomeBase))]
internal abstract class SomeBaseContracts : SomeBase {
    public override string ImportantStuff(string input) {
        Contract.Requires(input != null);
        Contract.Ensures(Contract.Result<string>() != null);
        throw new NotImplementedException();
    }
    public abstract override string NobodyCares();
}

[ContractClass(typeof(SomeInterfaceContracts))]
public interface ISomeInterface {
    string TopValue(int n);
}

[ContractClassFor(typeof(ISomeInterface))]
internal abstract class SomeInterfaceContracts : ISomeInterface {
    public string TopValue(int n) {
        Contract.Requires(n >= 0);
        Contract.Ensures(Contract.Result<string>() != null);
        throw new NotImplementedException();
    }
}

[ContractClass(typeof(ExtraStuffContracts))]
public interface IExtraStuff : ISomeInterface {
    int GetNumber();
}

[ContractClassFor(typeof(IExtraStuff))]
internal abstract class ExtraStuffContracts : IExtraStuff {
    public int GetNumber() {
        Contract.Ensures(Contract.Result<int>() >= 0);
        return 0;
    }
    public abstract string TopValue(int n);
}



}
