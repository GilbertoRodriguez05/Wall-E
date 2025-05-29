class DrawCircle : AST
{
    List<(int x, int y)> Directions = [(1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, 1), (1, -1), (-1, -1)];
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
        dirX.Execute();
        dirY.Execute();
        radius.Execute();
        int x = Convert.ToInt32(dirX.value);
        int y = Convert.ToInt32(dirY.value);
        int r = Convert.ToInt32(radius.value);
        SetCenter(x, y, r, canvas);
        Circle(Directions, r, canvas);
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        dirX.Execute();
        dirY.Execute();
        radius.Execute();
        int x = Convert.ToInt32(dirX.value);
        int y = Convert.ToInt32(dirY.value);
        int r = Convert.ToInt32(radius.value);
        bool X = dirX.SemanticCheck(errors, entorno);
        bool Y = dirY.SemanticCheck(errors, entorno);
        bool R = radius.SemanticCheck(errors, entorno);
        if (dirX.Type() != ExpresionsTypes.Numero || dirY.Type() != ExpresionsTypes.Numero || radius.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo int"));
            return false;
        }
        else if (x * r + canvas.ActualX < 0 || x * r + canvas.ActualX >= canvas.filas)
        {
            errors.Add(new Error(TypeOfError.Invalid, "El centro del circulo tiene que estar dentro de los limites del canvas"));
            return false;
        }
        else if (y * r + canvas.ActualY < 0 || y * r + canvas.ActualY >= canvas.columnas)
        {
            errors.Add(new Error(TypeOfError.Invalid, "El centro del circulo tiene que estar dentro de los limites del canvas"));
            return false;
        }
        else if (!Directions.Contains((x, y)))
        {
            errors.Add(new Error(TypeOfError.Invalid, "Direccion no valida"));
        }
        return X && Y && R;
    }
    public void SetCenter(int x, int y, int r, Canvas canvas)
    {
        if (x != 0 && y != 0)
        {
            for (int i = 0; i < r; i++)
            {
                canvas.ActualX += x;
                canvas.ActualY += y;
            }
        }
        else
        {
            for (int i = 0; i <= r; i++)
            {
                canvas.ActualX += x;
                canvas.ActualY += y;
            }
        }
    }
    public void Circle(List<(int x, int y)> directions, int r, Canvas canvas)
    {
        int x;
        int y;
        for (int i = 0; i < directions.Count; i++)
        {
            x = canvas.ActualX;
            y = canvas.ActualY;
            if (directions[i].x != 0 && directions[i].y != 0)
            {
                for (int j = 0; j < r; j++)
                {
                    x += directions[i].x;
                    y += directions[i].y;
                }
                if (x < 0 || x >= canvas.filas || y < 0 || y >= canvas.columnas)
                {
                    break;
                }
                else
                {
                    canvas.Board[x, y] = canvas.BrushColor;
                }
            }
            else
            {
                for (int j = 0; j <= r; j++)
                {
                    x += directions[i].x;
                    y += directions[i].y;
                }
                if (x < 0 || x >= canvas.filas || y < 0 || y >= canvas.columnas)
                {
                    break;
                }
                else
                {
                    canvas.Board[x, y] = canvas.BrushColor;
                }
                DrawSide(x, y, r, i, directions, canvas);
            }
        }
    }
    public void DrawSide(int x, int y, int r, int ActualDirection, List<(int x, int y)> directions, Canvas canvas)
    {
        int x1 = x;
        int y1 = y;
        if (directions[ActualDirection].x == 0)
        {
            for (int i = 0; i < (2 * r - 1) / 2; i++)
            {
                if (x1 + 1 < 0 || x1 + 1 >= canvas.filas) break;
                canvas.Board[x1 + 1, y1] = canvas.BrushColor;
                if (canvas.BrushSize > 1) FillDirection(x1 + 1, y1, canvas, directions);
                x1 += 1;
            }
            x1 = x;
            for (int j = 0; j < (2 * r - 1) / 2; j++)
            {
                if (x1 - 1 < 0 || x1 - 1 >= canvas.filas) break;
                canvas.Board[x1 - 1, y1] = canvas.BrushColor;
                if (canvas.BrushSize > 1) FillDirection(x1 - 1, y1, canvas, directions);
            }
        }
        else
        {
            for (int i = 0; i < (2 * r - 1) / 2; i++)
            {
                if (y1 + 1 < 0 || y1 + 1 >= canvas.columnas) break;
                canvas.Board[x1, y1 + 1] = canvas.BrushColor;
                if (canvas.BrushSize > 1) FillDirection(x1, y1 + 1, canvas, directions);
                y1 += 1;
            }
            y1 = y;
            for (int j = 0; j < (2 * r - 1) / 2; j++)
            {
                if (y1 - 1 < 0 || y1 - 1 >= canvas.columnas) break;
                canvas.Board[x1, y1 - 1] = canvas.BrushColor;
                if (canvas.BrushSize > 1) FillDirection(x, y1 - 1, canvas, directions);
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