using System.Security.Claims;

class IsCanvasColor : AST
{
    public List<string> DiferentsColor = new List<string> { "blue", "red", "green", "yellow", "black", "white", "orange", "purple", "transparent" };
    Expresions color;
    Expresions x;
    Expresions y;
    Canvas canvas;
    int result;
    public IsCanvasColor(Expresions color, Expresions x, Expresions y, Canvas canvas)
    {
        this.color = color;
        this.x = x;
        this.y = y;
        this.canvas = canvas;
    }
    public override void Execute()
    {
        color.Execute();
        x.Execute();
        y.Execute();
        int X = Convert.ToInt32(x.value);
        int Y = Convert.ToInt32(y.value);
        string Color = (string)color.value;
        if (canvas.Board[X + canvas.ActualX, Y + canvas.ActualY] == GetColor(Color)) result = 1;
        else result = 0;
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        color.Execute();
        x.Execute();
        y.Execute();
        int X = Convert.ToInt32(x.value);
        int Y = Convert.ToInt32(y.value);
        string Color = (string)color.value;
        bool x1 = x.SemanticCheck(errors, entorno);
        bool y1 = y.SemanticCheck(errors, entorno);
        bool c = color.SemanticCheck(errors, entorno);
        if (x.Type() != ExpresionsTypes.Numero || y.Type() != ExpresionsTypes.Numero || color.Type() != ExpresionsTypes.Cadena)
        {
            errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo string o un tipo int"));
            return false;
        }
        else if (X + canvas.ActualX < 0 || X + canvas.ActualX >= canvas.filas || Y + canvas.ActualY < 0 || Y + canvas.ActualY >= canvas.columnas)
        {
            errors.Add(new Error(TypeOfError.Invalid, "La casilla tiene que estar dentro de las dimensiones del canvas"));
            return false;
        }
        else if (!DiferentsColor.Contains(Color.ToLower()))
        {
            errors.Add(new Error(TypeOfError.Invalid, "El color no es válido"));
            return false;
        }
        return x1 && y1 && c;
    }
    public Colors GetColor(string colorValue)
    {
        switch (colorValue.ToLower())
        {
            case "red": return  Colors.Red;
            case "blue": return  Colors.Blue;
            case "green": return  Colors.Green;
            case "yellow": return  Colors.Yellow;
            case "black": return  Colors.Black;
            case "white": return  Colors.White;
            case "orange": return  Colors.Orange;
            case "purple": return  Colors.Purple;
            case "transparent": return  Colors.Transparent;
            default: return Colors.White;
        }
    }
}