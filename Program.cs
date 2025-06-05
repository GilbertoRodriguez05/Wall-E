class Program
{
    static void Main(string[] args)
    {
        string codigoFuente = File.ReadAllText("test1.txt");
        Entorno entorno = new Entorno();
        Canvas canvas = new Canvas(0, 0);
        Scanner escaner = new Scanner(codigoFuente);
        List<Error> errors = new List<Error>();
        List<Token> tokens = escaner.ScanTokens();
        Parser parser = new Parser(tokens, errors, entorno, canvas);
        AST block = parser.Main();
        block.SemanticCheck(errors, entorno);
        foreach (Error item in errors)
        {
            System.Console.WriteLine(item.ToString());
        }
        // foreach (Token item in tokens)
        // {
        //     System.Console.WriteLine(Convert.ToString(item.types) + " " + Convert.ToString(item.lexeme));
        // }

    }
}