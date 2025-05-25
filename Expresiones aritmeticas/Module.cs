
class Module : BinaryExpresions
{
    public override object value { get; set; }
    public Module(Expresions Right, Expresions Left)
    {
        this.Right = Right;
        this.Left = Left;
    }
    public override void GetValue()
    {
        Right.GetValue();
        Left.GetValue();
        value = Convert.ToInt32(Left.value) % Convert.ToInt32(Right.value);
    }
    public override bool SemanticCheck(List<Error> errors)
    {
        bool right = Right.SemanticCheck(errors);
        bool left = Left.SemanticCheck(errors);
        if (Right.Type() != ExpresionsTypes.Numero || Left.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "La operacion modulo solo se pude hacer entre dos numeros"));
            return false;
        }
        return right && left;
    }
    public override ExpresionsTypes Type()
    {
        if (Right.Type() != ExpresionsTypes.Numero || Left.Type() != ExpresionsTypes.Numero)
        {
            return ExpresionsTypes.Error;
        }
        return ExpresionsTypes.Numero;
    }
}