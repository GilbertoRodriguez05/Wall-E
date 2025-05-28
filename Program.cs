class Program
{
    static void Main(string[] args)
    {
        string codigoFuente = File.ReadAllText("test1.txt");
        Scanner escaner = new Scanner(codigoFuente);

        List<Token> tokens = escaner.ScanTokens();
        foreach (Token token in tokens)
        {
            System.Console.WriteLine($"Token : {token.lexeme}  literal {token.literal} linea:  {token.line}");
        }

        // Canvas canvas = new Canvas(0, 0);
        // Numero numero = new Numero(1);
        // Numero numero1 = new Numero(2);

        // Spawn spawn = new Spawn(numero, numero1, canvas);
        // spawn.Execute();
        // System.Console.WriteLine(canvas.ActualX);
        // System.Console.WriteLine(canvas.ActualY);
    }
}