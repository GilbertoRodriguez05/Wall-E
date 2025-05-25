class Equal: BinaryExpresions
{
    public override object value { get; set; }
    public Equal(Expresions Right, Expresions Left)
    {
        this.Right = Right;
        this.Left = Left;
    }
    public override void GetValue()
    {
        Right.GetValue();
        Left.GetValue();
        if (Right.Type() == ExpresionsTypes.Numero && Left.Type() == ExpresionsTypes.Numero)
        {
            value = Convert.ToInt32(Left.value) == Convert.ToInt32(Right.value);
        }
        else if (Right.Type() == ExpresionsTypes.Bool && Left.Type() == ExpresionsTypes.Bool)
        {
            value = (bool)Right.value == (bool)Left.value;
        }
        else
        {
            value = (string)Right.value == (string)Left.value;
        }
    }
    public override bool SemanticCheck(List<Error> errors)
    {
        bool right = Right.SemanticCheck(errors);
        bool left = Left.SemanticCheck(errors);
        if (Right.Type() != ExpresionsTypes.Numero || Left.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "Solo se puede comparar entre dos expresiones del mismo tipo"));
            return false;
        }
        else if (Right.Type() != ExpresionsTypes.Bool || Left.Type() != ExpresionsTypes.Bool)
        {
            errors.Add(new Error(TypeOfError.Expected, "Solo se puede comparar entre dos expresiones del mismo tipo"));
            return false;
        }
        else if (Right.Type() != ExpresionsTypes.Cadena || Left.Type() != ExpresionsTypes.Cadena)
        {
            errors.Add(new Error(TypeOfError.Expected, "Solo se puede comparar entre dos expresiones del mismo tipo"));
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
        else if (Right.Type() != ExpresionsTypes.Bool || Left.Type() != ExpresionsTypes.Bool)
        {
            return ExpresionsTypes.Error;
        }
        else if (Right.Type() != ExpresionsTypes.Cadena || Left.Type() != ExpresionsTypes.Cadena)
        {
            return ExpresionsTypes.Error;
        }
        return ExpresionsTypes.Bool;
    }
}