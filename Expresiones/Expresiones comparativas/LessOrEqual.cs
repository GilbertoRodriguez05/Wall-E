class LessOrEqual : BinaryExpresions
{
    public override object value { get; set; }
    public LessOrEqual(Expresions Right, Expresions Left)
    {
        this.Right = Right;
        this.Left = Left;
    }
    public override void Execute()
    {
        Right.Execute();
        Left.Execute();
        value = Convert.ToInt32(Left.value) <= Convert.ToInt32(Right.value);
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        bool right = Right.SemanticCheck(errors, entorno);
        bool left = Left.SemanticCheck(errors, entorno);
        if (Right.Type() != ExpresionsTypes.Numero || Left.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "Solo se puede comparar entre dos numeros"));
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
        return ExpresionsTypes.Bool;
    }
}