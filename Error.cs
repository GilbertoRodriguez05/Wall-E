using System.Net.NetworkInformation;

public class Error : Exception
{
    public TypeOfError typeOfError { get; private set; }
    public string argument { get; private set; }
    public int line { get; private set; }
    public Error(TypeOfError typeOfError, string argument, int line = 0)
    {
        this.typeOfError = typeOfError;
        this.argument = argument;
        this.line = line;
    }
    public override string ToString()
    {
        return argument + " Error de tipo " + typeOfError + " en la linea " + line;
    }

}
public enum TypeOfError
{
    Expected,
    VariableUndefined,
    Invalid
}