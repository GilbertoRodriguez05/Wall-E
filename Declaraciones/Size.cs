
class Size : AST
{
    Expresions k;
    Canvas canvas;
    public Size(Expresions k, Canvas canvas)
    {
        this.k = k;
        this.canvas = canvas;
    }
    public override void Execute()
    {
        k.Execute();
        if (Convert.ToInt32(k.value) % 2 != 0)
        {
            canvas.BrushSize = Convert.ToInt32(k.value);
        }
        else
        {
            canvas.BrushSize = Convert.ToInt32(k.value) - 1;
        }
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        if (k.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "Se esparaba un tipo int"));
            return false;
        }
        return true;
    }
}