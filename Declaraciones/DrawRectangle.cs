
using System.Drawing;

class DrawRectangle : AST
{
    List<(int x, int y)> Directions = [(1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, 1), (1, -1), (-1, -1)];
    Expresions dirX;
    Expresions dirY;
    Expresions height;
    Expresions width;
    Expresions distance;
    Canvas canvas;
    public DrawRectangle(Expresions dirX, Expresions dirY, Expresions distance, Expresions width, Expresions height, Canvas canvas)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.distance = distance;
        this.width = width;
        this.height = height;
        this.canvas = canvas;
    }
    public override void Execute()
    {
        dirX.Execute();
        dirY.Execute();
        distance.Execute();
        width.Execute();
        height.Execute();
        int x = Convert.ToInt32(dirX.value);
        int y = Convert.ToInt32(dirY.value);
        int d = Convert.ToInt32(distance.value);
        int w = Convert.ToInt32(width.value);
        int h = Convert.ToInt32(height.value);
        for (int i = 0; i < d; i++)
        {
            canvas.ActualX += x;
            canvas.ActualY += y;
        }
        Rectangle(w, h, canvas);
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        dirX.Execute();
        dirY.Execute();
        distance.Execute();
        int x = Convert.ToInt32(dirX.value);
        int y = Convert.ToInt32(dirY.value);
        int d = Convert.ToInt32(distance.value);
        bool X = dirX.SemanticCheck(errors, entorno);
        bool Y = dirY.SemanticCheck(errors, entorno);
        bool W = width.SemanticCheck(errors, entorno);
        bool H = height.SemanticCheck(errors, entorno);
        bool D = distance.SemanticCheck(errors, entorno);
        if (dirX.Type() != ExpresionsTypes.Numero || dirY.Type() != ExpresionsTypes.Numero || width.Type() != ExpresionsTypes.Numero || height.Type() != ExpresionsTypes.Numero || distance.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo int"));
            return false;
        }
        else if (x * d + canvas.ActualX < 0 || x * d + canvas.ActualX >= canvas.filas)
        {
            errors.Add(new Error(TypeOfError.Invalid, "El centro del rectangulo debe estar dentro de los limites del canvas"));
            return false;
        }
        else if (y * d + canvas.ActualY < 0 || y * d + canvas.ActualY >= canvas.columnas)
        {
            errors.Add(new Error(TypeOfError.Invalid, "El centro del rectangulo debe estar dentro de los limites del canvas"));
            return false;
        }
        return X && Y && W && H && D;
    }
    public void Rectangle(int w, int h, Canvas canvas)
    {
        List<(int x, int y)> RectangleDirections = [(1, 0), (0, -1), (-1, 0), (0, 1)];
        int x = canvas.ActualX;
        int y = canvas.ActualY;
        x -= h / 2 - 1;
        y += w / 2 + 1;
        for (int i = 0; i < RectangleDirections.Count; i++)
        {
            switch (RectangleDirections[i])
            {
                case (1, 0):
                    for (int j = 0; j <= h; j++)
                    {
                        if (x + 1 < 0 || x + 1 >= canvas.filas) break;
                        canvas.Board[x + 1, y] = canvas.BrushColor;
                        if(canvas.BrushSize > 1) FillDirection(x + 1, y, canvas, Directions);
                        x += 1;
                    }
                    break;
                case (0, -1):
                    for (int j = 0; j <= w; j++)
                    {
                        if (y - 1 < 0 || y - 1 >= canvas.columnas) break;
                        canvas.Board[x, y - 1] = canvas.BrushColor;
                        if(canvas.BrushSize > 1) FillDirection(x, y - 1, canvas, Directions);
                        y -= 1;
                    }
                    break;
                case (-1, 0):
                    for (int j = 0; j <= h; j++)
                    {
                        if (x - 1 < 0 || x - 1 >= canvas.filas) break;
                        canvas.Board[x - 1, y] = canvas.BrushColor;
                        if(canvas.BrushSize > 1) FillDirection(x - 1, y, canvas, Directions);
                        x -= 1;
                    }
                    break;
                case (0, 1):
                    for (int j = 0; j <= w; j++)
                    {
                        if (y + 1 < 0 || y + 1 >= canvas.columnas) break;
                        canvas.Board[x, y + 1] = canvas.BrushColor;
                        if(canvas.BrushSize > 1) FillDirection(x, y + 1, canvas, Directions);
                        y += 1;
                    }
                    break;
                default: break;
            }
        }
    }
    public void FillDirection(int x, int y, Canvas canvas, List<(int x, int y)> Directions)
    {
        for (int k = 0; k < Directions.Count; k++)
        {
            int tempX = x;
            int tempY = y;
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