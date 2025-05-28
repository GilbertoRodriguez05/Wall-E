class DrawCircle : AST
{
    Expresions dirX;
    Expresions dirY;
    Expresions radius;
    Canvas canvas;
    public DrawCircle(Expresions dirX, Expresions dirY, Expresions radius, Canvas canvas)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.radius = radius;
        this.canvas = canvas;
    }
    public override void Execute()
    {
        throw new NotImplementedException();
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        throw new NotImplementedException();
    }
}