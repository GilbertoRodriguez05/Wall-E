
public class Label : AST
{
    public Block block;
    public string name;
    public Label(string name)
    {
        this.name = name;
    }
    public override void Execute()
    {
        block.Execute();
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        return true;
    }
}