
class Grouping : Expresions
{
    public override object value { get; set; }
    Expresions expresions;
    public Grouping(Expresions expresions)
    {
        this.expresions = expresions;
    }
    public override void Execute()
    {
        expresions.Execute();
        value = expresions.value;
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        bool group = expresions.SemanticCheck(errors, entorno);
        if (expresions.Type() == ExpresionsTypes.Numero) return group;
        else if (expresions.Type() == ExpresionsTypes.Cadena) return group;
        else if (expresions.Type() == ExpresionsTypes.Bool) return group;
        else
        {
            errors.Add(new Error(TypeOfError.Invalid, "Expresion invalida"));
            return false;
        }
    }
    public override ExpresionsTypes Type()
    {
        if (expresions.Type() == ExpresionsTypes.Numero) return ExpresionsTypes.Numero;
        else if (expresions.Type() == ExpresionsTypes.Cadena) return ExpresionsTypes.Cadena;
        else if (expresions.Type() == ExpresionsTypes.Bool) return ExpresionsTypes.Bool;
        else return ExpresionsTypes.Error;
    }
}