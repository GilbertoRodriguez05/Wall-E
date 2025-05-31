public class Block : AST
{
    List<AST> declarations { get; }
    public Block(List<AST> declarations)
    {
        this.declarations = declarations;
    }
    public override void Execute()
    {
        foreach (AST item in declarations)
        {
            item.Execute();
        }
    }
    public override bool SemanticCheck(List<Error> errors, Entorno entorno)
    {
        foreach (AST item in declarations)
        {
            bool valid = item.SemanticCheck(errors, entorno);
            if (!valid) return false;
        }
        return true;
    }
}