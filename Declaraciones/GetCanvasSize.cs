using System.Security.Cryptography;

class GetCanvasSize : AST
{
    int result;
    (int, int) tuple;
    Canvas canvas;
    public GetCanvasSize(Canvas canvas)
    {
        this.canvas = canvas;
    }
    public override void Execute()
    {
        if (canvas.filas == canvas.columnas) result = canvas.filas;
        else
        {
            tuple.Item1 = canvas.filas;
            tuple.Item2 = canvas.columnas;
        }
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        return true;
    }
}