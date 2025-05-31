class Fill : AST
{

    Canvas canvas;
    List<(int x, int y)> Directions = [(1, 0), (0, -1), (-1, 0), (0, 1)];
    public Fill(Canvas canvas)
    {
        this.canvas = canvas;
    }
    public override void Execute()
    {
        bool[,] Mask = new bool[canvas.filas, canvas.columnas];
        Colors ActualColor = canvas.Board[canvas.ActualX, canvas.ActualY];
        Filling(canvas.ActualX, canvas.ActualY, canvas, ActualColor, Directions, Mask);
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        return true;
    }
    public void Filling(int x, int y, Canvas canvas, Colors ActualColor, List<(int x, int y)> Directions, bool[,] Mask)
    {
        if (canvas.Board[x, y] != ActualColor) return;
        else
        {
            for (int i = 0; i < Directions.Count; i++)
            {
                if (x + Directions[i].x < 0 || x + Directions[i].x >= canvas.filas) continue;
                else if (y + Directions[i].y < 0 || y + Directions[i].y >= canvas.columnas) continue;
                else if (Mask[x + Directions[i].x, y + Directions[i].y]) continue;
                else
                {
                    canvas.Board[x + Directions[i].x, y + Directions[i].y] = canvas.BrushColor;
                    Mask[x + Directions[i].x, y + Directions[i].y] = true;
                    Filling(x + Directions[i].x, y + Directions[i].y, canvas, ActualColor, Directions, Mask);
                }
            }
        }
    }
}