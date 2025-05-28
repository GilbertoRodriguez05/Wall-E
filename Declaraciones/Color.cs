
class Color : AST
{
    public List<string> DiferentsColor = new List<string> {"blue", "red", "green", "yellow", "black", "white", "orange", "purple", "transparent"};
    public Expresions color;
    public Canvas canvas;
    public Color(Expresions color, Canvas canvas)
    {
        this.color = color;
        this.canvas = canvas;
    }
    public override void Execute()
    {
        color.Execute();
        string colorValue = (string)color.value;
        switch (colorValue.ToLower())
        {
            case "red": canvas.BrushColor = Colors.Red; break;
            case "blue": canvas.BrushColor = Colors.Blue; break;
            case "green": canvas.BrushColor = Colors.Green; break;
            case "yellow": canvas.BrushColor = Colors.Yellow; break;
            case "black": canvas.BrushColor = Colors.Black; break;
            case "white": canvas.BrushColor = Colors.White; break;
            case "orange": canvas.BrushColor = Colors.Orange; break;
            case "purple": canvas.BrushColor = Colors.Purple; break;
            case "transparent": canvas.BrushColor = Colors.Transparent; break;
            default: break;
        }
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        color.Execute();
        string colorValue = (string)color.value;
        if (color.Type() != ExpresionsTypes.Cadena)
        {
            errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo string"));
            return false;
        }
        if (!DiferentsColor.Contains(colorValue))
        {
            errors.Add(new Error(TypeOfError.Invalid, "Color no definido"));
            return false;
        }
        return true;
    }
}