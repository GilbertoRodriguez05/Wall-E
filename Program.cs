class Program
{
    static void Main(string[] args)
    {
        string codigoFuente = File.ReadAllText("test1.txt");
        Scanner escaner = new Scanner(codigoFuente);

        List<Token> tokens = escaner.ScanTokens();
        
        // Ejemplo: Imprime los tokens
        foreach (Token token in tokens)
        {
            Console.WriteLine(token.lexeme);
        }
    }
}