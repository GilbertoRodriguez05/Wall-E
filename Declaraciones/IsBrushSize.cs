class IsBrushSize : AST
{
    Expresions size;
    int result;
    Canvas canvas;
    public IsBrushSize(Expresions size, Canvas canvas)
    {
        this.size = size;
        this.canvas = canvas;
    }
    public override void Execute()
    {
        size.Execute();
        if (Convert.ToInt32(size.value) == canvas.BrushSize) result = 1;
        else result = 0;
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        if (size.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo int"));
            return false;
        }
        return true;
    }
}