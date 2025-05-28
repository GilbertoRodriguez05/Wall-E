class Variable : Expresions
{
    public string var;
    public Entorno entorno { get; set; }
    public override object value { get; set; }
    public Variable(string var)
    {
        this.var = var;
    }
    public override void Execute()
    {
        value = entorno.Execute(var);
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        this.entorno = entorno;
        if (entorno.GetType(var) == ExpresionsTypes.Error)
        {
            errors.Add(new Error(TypeOfError.VariableUndefined, "La variable no esta definida"));
            return false;
        }
        else return true;
    }
    public override ExpresionsTypes Type()
    {
        if (entorno.GetType(var) == ExpresionsTypes.Error) return ExpresionsTypes.Error;
        else return entorno.GetType(var);
    }
}