
using Microsoft.VisualBasic;

class VarDeclaration : AST
{
    public Expresions name { get; set; }
    public Expresions value { get; set; }
    public Token operador { get; set; }
    public Entorno Entorno { get; set; }
    public VarDeclaration(Expresions name, Expresions value, Token operador, Entorno Entorno)
    {
        this.name = name;
        this.value = value;
        this.operador = operador;
        this.Entorno = Entorno;
    }
    public override void Execute()
    {
        if (name is Variable)
        {
            if (operador.types == TokenTypes.Equal)
            {
                Entorno.SetValue(name.ToString(), value.value);
                return;
            }
        }
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        value.SemanticCheck(errors, entorno);
        if (name is Variable var)
        {
            if (operador.types == TokenTypes.Equal)
            {
                entorno.SetType(var.value.ToString(), value.Type());
                return true;
            }
        }
        return false;
    }
}