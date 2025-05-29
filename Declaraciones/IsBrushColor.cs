class IsBrushColor : AST
{
    public List<string> DiferentsColor = new List<string> {"blue", "red", "green", "yellow", "black", "white", "orange", "purple", "transparent"};
    Expresions color;
    Canvas canvas;
    int result;
    public IsBrushColor(Expresions color, Canvas canvas)
    {
        this.color = color;
        this.canvas = canvas;
    }
    public override void Execute()
    {
        color.Execute();
        string colorValue = (string)color.value;

        if (GetColor(colorValue) == canvas.BrushColor) result = 1;
        else result = 0;
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
        else if (!DiferentsColor.Contains(colorValue.ToLower()))
        {
            errors.Add(new Error(TypeOfError.Invalid, "Color no definido"));
            return false;
        }
        return true;
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