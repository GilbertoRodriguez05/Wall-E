
using System.Security.Cryptography.X509Certificates;

class Spawn : Declarations
{
    public Expresions initialX;
    public Expresions initialY;
    public Canvas canvas;
    public Spawn(Expresions x, Expresions y, Canvas canvas)
    {
        initialX = x;
        initialY = y;
        this.canvas = canvas;
    }
    public override void Execute()
    {
        canvas.InicializarCanvas();
        initialX.GetValue();
        initialY.GetValue();
        canvas.ActualX = Convert.ToInt32(initialX.value);
        canvas.ActualY = Convert.ToInt32(initialY.value);
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        bool x = initialX.SemanticCheck(errors, entorno);
        bool y = initialY.SemanticCheck(errors, entorno);
        if (initialX.Type() != ExpresionsTypes.Numero || initialY.Type() != ExpresionsTypes.Numero)
        {
            errors.Add(new Error(TypeOfError.Expected, "Se esperaban parametros de tipo entero"));
            return false;
        }
        if (Convert.ToInt32(initialX.value) < 0 || Convert.ToInt32(initialX.value) >= canvas.filas)
        {
            errors.Add(new Error(TypeOfError.Invalid, "La Posicion inicial debe estar dentro del rango del canvas"));
            return false;
        }
        if (Convert.ToInt32(initialY.value) < 0 || Convert.ToInt32(initialY.value) >= canvas.filas)
        {
            errors.Add(new Error(TypeOfError.Invalid, "La Posicion inicial debe estar dentro del rango del canvas"));
            return false;
        }
        return x && y;
    }
}