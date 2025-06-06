
class Substraction : BinaryExpresions
{
    public override object value { get; set; }
    public Substraction(Expresions left, Expresions right)
    {
        this.Right = Right;
        this.Left = Left;
    }
    public override void Execute()
    {
        Right.Execute();
        Left.Execute();
        value = Convert.ToInt32(Left) - Convert.ToInt32(Right);
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        bool right = Right.SemanticCheck(errors, entorno);
        bool left = Left.SemanticCheck(errors, entorno);
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