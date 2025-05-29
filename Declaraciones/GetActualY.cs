class GetActualY : AST
{
    Canvas canvas;
    int result;
    public GetActualY(Canvas canvas)
    {
        this.canvas = canvas;
    }
    public override void Execute()
    {
        result = canvas.ActualY;
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        return true;
    }
}