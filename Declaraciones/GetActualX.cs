class GetActualX : AST
{
    Canvas canvas;
    int result;
    public GetActualX(Canvas canvas)
    {
        this.canvas = canvas;
    }
    public override void Execute()
    {
        result = canvas.ActualX;
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        return true;
    }
}