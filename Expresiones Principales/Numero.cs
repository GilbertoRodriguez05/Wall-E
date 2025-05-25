
class Numero : Expresions
{
    public override object value { get; set; }
    public Numero(int value)
    {
        this.value = value;
    }
    public override void GetValue()
    {

    }
    public override bool SemanticCheck(List<Error> errors)
    {
        return true;
    }
    public override ExpresionsTypes Type()
    {
        return ExpresionsTypes.Numero;
    }
}