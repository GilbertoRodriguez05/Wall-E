
using Microsoft.VisualBasic;

class And : BinaryExpresions
{
    public override object value { get; set; }
    public And(Expresions Right, Expresions Left)
    {
        this.Right = Right;
        this.Left = Left;
    }
    public override void GetValue()
    {
        Right.GetValue();
        Left.GetValue();
        value = (bool)Right.value && (bool)Left.value;
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        bool right = Right.SemanticCheck(errors, entorno);
        bool left = Left.SemanticCheck(errors, entorno);
        if (Right.Type() != ExpresionsTypes.Bool || Left.Type() != ExpresionsTypes.Bool)
        {
            errors.Add(new Error(TypeOfError.Expected, "Solo se puede aplicar a expresiones booleanas"));
            return false;
        }
        return right && left;
    }
    public override ExpresionsTypes Type()
    {
        if (Right.Type() != ExpresionsTypes.Bool || Left.Type() != ExpresionsTypes.Bool)
        {
            return ExpresionsTypes.Error;
        }
        return ExpresionsTypes.Bool;
    }
}