
class Booleano : Expresions
{
    public override object value { get; set; }
    public Booleano(bool value)
    {
        this.value = value;
    }
    public override void GetValue()
    {

    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        return true;
    }
    public override ExpresionsTypes Type()
    {
        return ExpresionsTypes.Bool;
    }
}