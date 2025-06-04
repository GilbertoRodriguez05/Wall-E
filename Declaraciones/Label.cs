public class Label : AST
{
    Entorno entorno;
    public Block block;
    public string name;
    public Label(string name, Block block, Entorno entorno)
    {
        this.name = name;
        this.block = block;
        this.entorno = entorno;
        entorno.SetLabel(name);
    }
    public override void Execute()
    {
        block.Execute();
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        if (!IsIdentifier(name))
        {
            errors.Add(new Error(TypeOfError.Invalid, "Label no valido"));
            return false;
        }
        return true;
        
    }
    private bool IsIdentifier(string nombre)
    {
        if (string.IsNullOrEmpty(nombre)) return false;
        if (char.IsDigit(nombre[0])) return false;
        if (nombre[0] == '_') return false;
        foreach (char c in nombre)
        {
            if (!char.IsLetterOrDigit(c) && c != '_')
                return false;
        }
        return true;
    }
}