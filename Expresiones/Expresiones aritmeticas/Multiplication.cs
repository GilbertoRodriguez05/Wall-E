
class Multiplication : BinaryExpresions
{
    public override object value { get; set; }
    public Multiplication(Expresions Left, Expresions Right)
    {
        this.Left = Left;
        this.Right = Right;
    }
    public override void GetValue()
    {
        Right.GetValue();
        Left.GetValue();
        value = Convert.ToInt32(Right.value) * Convert.ToInt32(Left.value);
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        bool right = SemanticCheck(errors, entorno);
        bool left = SemanticCheck(errors, entorno);
        if (Right.Type() != ExpresionsTypes.Numero || Left.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "La multiplicacion debe ser entre dos numero"));
            return false;
        }
        return right && left;
    }
    public override ExpresionsTypes Type()
    {
        if (Right.Type() != ExpresionsTypes.Numero || Left.Type() != ExpresionsTypes.Numero) return ExpresionsTypes.Error;
        return ExpresionsTypes.Numero;
    }
}