public abstract class Expresions
{
    public abstract object value { get; set; }

    public abstract void GetValue();
    public abstract bool SemanticCheck(List<Error> errors, Entorno entorno);
    public abstract ExpresionsTypes Type();
}

public abstract class BinaryExpresions : Expresions
{
    public Expresions Right { get; set; }
    public Expresions Left { get; set; }
    public BinaryExpresions()
    {

    }
}