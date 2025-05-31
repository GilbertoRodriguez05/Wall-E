class Goto : AST
{
    Label Label;
    Expresions Condition;
    public Goto(Label Label, Expresions Condition)
    {
        this.Label = Label;
        this.Condition = Condition;
    }
    public override void Execute()
    {
        Condition.Execute();
        if (Condition is Booleano condition && (bool)condition.value)
        {
            Label.Execute();
        }
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        if (Condition.Type() != ExpresionsTypes.Bool)
        {
            errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo bool"));
            return false;
        }
        else if (entorno.SetLabel(Label.name))
        {
            errors.Add(new Error(TypeOfError.VariableUndefined, "Label no definida"));
            return false;
        }
        return Condition.SemanticCheck(errors, entorno);
    }
}