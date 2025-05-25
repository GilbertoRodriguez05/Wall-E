public class Addition : BinaryExpresions
{
    public override object value { get; set;}
    public Addition(Expresions Left, Expresions Right)
    {
        this.Left = Left;
        this.Right = Right;
    }
    public override void GetValue()
    {
        Left.GetValue();
        Right.GetValue();
        value = Convert.ToInt32(Left.value) + Convert.ToInt32(Right.value);
    }
    public override ExpresionsTypes Type()
    {
        if (Left.Type() != ExpresionsTypes.Numero || Right.Type() != ExpresionsTypes.Numero) return ExpresionsTypes.Error;
        return ExpresionsTypes.Numero;
    }
    public override bool SemanticCheck(List<Error> errors)
    {
        bool right = Right.SemanticCheck(errors);
        bool left = Left.SemanticCheck(errors);
        if (Right.Type() != ExpresionsTypes.Numero || Left.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "La adicion debe ser entre dos numeros"));
            return false;
        }
        return left && right;
    }
}