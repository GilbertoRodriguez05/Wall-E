public abstract class Expresions : AST
{
    public abstract object value { get; set; }
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