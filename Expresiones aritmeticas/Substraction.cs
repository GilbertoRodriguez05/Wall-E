
class Substraction : BinaryExpresions
{
    public override object value { get; set; }
    public Substraction(Expresions left, Expresions right)
    {
        this.Right = Right;
        this.Left = Left;
    }
    public override void GetValue()
    {
        Right.GetValue();
        Left.GetValue();
        value = Convert.ToInt32(Left) - Convert.ToInt32(Right);
    }
    public override bool SemanticCheck(List<Error> errors)
    {
        bool right = Right.SemanticCheck(errors);
        bool left = Left.SemanticCheck(errors);
        if (Right.Type() != ExpresionsTypes.Numero || Left.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "La resta tiene que ser entre dos numeros"));
            return false;
        }
        return left && right;
    }
    public override ExpresionsTypes Type()
    {
        if (Right.Type() != ExpresionsTypes.Numero || Left.Type() != ExpresionsTypes.Numero) return ExpresionsTypes.Error;
        return ExpresionsTypes.Numero;
    }
}