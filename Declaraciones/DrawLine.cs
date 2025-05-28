
class DrawLine : AST
{
    List<(int x, int y)> Directions = [(1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, 1), (1, -1), (-1, -1)];
    Expresions dirX;
    Expresions dirY;
    Expresions distance;
    Canvas canvas;
    public DrawLine(Expresions dirX, Expresions dirY, Expresions distance, Canvas canvas)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.distance = distance;
        this.canvas = canvas;
    }
    public override void Execute()
    {
        dirX.Execute();
        dirY.Execute();
        distance.Execute();
        int x = Convert.ToInt32(dirX.value);
        int y = Convert.ToInt32(dirY.value);
        int dist = Convert.ToInt32(distance.value);
        canvas.Board[canvas.ActualX, canvas.ActualY] = canvas.BrushColor;
        if (canvas.BrushSize > 1)
        {
            FillDirection(canvas, Directions);
        }
        for (int i = 1; i < dist; i++)
        {
            canvas.Board[canvas.ActualX + x, canvas.ActualY + y] = canvas.BrushColor;
            canvas.ActualX += x;
            canvas.ActualY += y;
            if (canvas.BrushSize > 1)
            {
                FillDirection(canvas, Directions);
            }
        }
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        dirX.Execute();
        dirY.Execute();
        distance.Execute();
        int x1 = Convert.ToInt32(dirX.value);
        int y1 = Convert.ToInt32(dirY.value);
        int dist1 = Convert.ToInt32(distance.value);
        bool x = dirX.SemanticCheck(errors, entorno);
        bool y = dirY.SemanticCheck(errors, entorno);
        bool dist = SemanticCheck(errors, entorno);
        if (dirX.Type() != ExpresionsTypes.Numero || dirY.Type() != ExpresionsTypes.Numero || distance.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo int"));
            return false;
        }
        if (canvas.ActualX + x1 * dist1 < 0 || canvas.ActualX + x1 * dist1 > canvas.filas)
        {
            errors.Add(new Error(TypeOfError.Invalid, "La distancia se sale de los limites del canvas"));
            return false;
        }
        if (canvas.ActualY + y1 * dist1 < 0 || canvas.ActualY + y1 * dist1 > canvas.columnas)
        {
            errors.Add(new Error(TypeOfError.Invalid, "La distancia se sale de los limites del canvas"));
            return false;
        }
        return x && y && dist;
    }
    public void FillDirection(Canvas canvas, List<(int x, int y)> Directions)
    {
        for (int k = 0; k < Directions.Count; k++)
        {
            int tempX = canvas.ActualX;
            int tempY = canvas.ActualY;
            for (int j = 0; j < canvas.BrushSize / 2; j++)
            {
                if (tempX + Directions[k].x < 0 || tempX + Directions[k].x >= canvas.filas) break;
                if (tempY + Directions[k].y < 0 || tempY + Directions[k].y >= canvas.columnas) break;
                canvas.Board[tempX + Directions[k].x, tempY + Directions[k].y] = canvas.BrushColor;
                tempX += Directions[k].x;
                tempY += Directions[k].y;
            }
        }
    } 
}