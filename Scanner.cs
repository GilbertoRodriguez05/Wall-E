using System.Security.Cryptography;

public class Scanner
{
    public string source;
    public List<Token> tokens = new List<Token> { };
    public int Start = 0;
    public int current = 0;
    public int line = 1;
    public Scanner(string source)
    {
        this.source = source;
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            Start = current;
            ScanToken();
        }
        tokens.Add(new Token(TokenTypes.EOF, "", null, line));
        return tokens;
    }
    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenTypes.AbreParentesis, null); break;
            case ')': AddToken(TokenTypes.CierraParentesis, null); break;
            case '[': AddToken(TokenTypes.AbreCorchete, null); break;
            case ']': AddToken(TokenTypes.CierraCorchete, null); break;
            case '+': AddToken(TokenTypes.Suma, null); break;
            case '/': AddToken(TokenTypes.Division, null); break;
            case '%': AddToken(TokenTypes.Modulo, null); break;
            case '-': AddToken(TokenTypes.Resta, null); break;
            case '*': AddToken(Match('*') ? TokenTypes.Potencia : TokenTypes.Multiplicacion, null); break;
            case '!': AddToken(Match('=') ? TokenTypes.Distinto : TokenTypes.Negacion, null); break;
            case '>': AddToken(Match('=') ? TokenTypes.MayorIgual : TokenTypes.Mayor, null); break;
            case '<': if (Match('=')) AddToken(TokenTypes.MenorIgual, null);
                      else if (Match('-')) AddToken(TokenTypes.Declaracion, null);
                      else AddToken(TokenTypes.Menor, null); break;
            case '=': if (Match('=')) AddToken(TokenTypes.Igual, null); break;
            case '&': if (Match('&')) AddToken(TokenTypes.And, null); break;
            case '|': if (Match('|')) AddToken(TokenTypes.Or, null); break;
            case '\t': break;
            case '\r': break;
            case ' ': break;
            case '\n': line++; break;
            case '"': String(); break;
            case ',': AddToken(TokenTypes.Coma, null); break;
            default:
                if (IsDigit(c)) Number();
                else if (IsSpanishLetter(c)) Identificador();
                else throw new ArgumentException("caracter inesperado en la linea " + line); break;
        }
    }
    private bool IsAtEnd()
    {
        return current >= source.Length;
    }
    private char Advance()
    {
        current++;
        return source[current - 1];
    }
    private bool Match(char Expected)
    {
        if (IsAtEnd()) return false;
        else if (source[current] != Expected)
        {
            return false;
        }
        current++;
        return true;
    }
    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return source[current];
    }
    public void AddToken(TokenTypes type, object literal)
    {
        string text = source.Substring(Start, current-Start);
        tokens.Add(new Token(type, text, literal, line));
    }
    private void String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') line++;
            Advance();
        }
        if (IsAtEnd()) throw new ArgumentException("Cadena sin terminar " + line);
        Advance();
        string value = source.Substring(Start + 1, current - Start - 1);
        AddToken(TokenTypes.Cadena, value);
    }
    public static bool IsSpanishLetter(char c)
    {
        string letrasValidas = "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
        return letrasValidas.Contains(c);
    }
    public static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }
    private void Number()
    {
        while (IsDigit(Peek())) Advance();
        AddToken(TokenTypes.Numero, int.Parse(source.Substring(Start, current-Start)));
    }
    private bool IsAlphaNumeric(char c)
    {
        return IsSpanishLetter(c) || IsDigit(c) || c == '_';
    }
    private void Identificador()
    {
        while (IsAlphaNumeric(Peek())) Advance();
        string text = source.Substring(Start, current-Start);
        if (!KeyWords.TryExecute(text, out TokenTypes type))
        {
            type = TokenTypes.Identificador;
        }
        AddToken(type, null);
    }
    public static Dictionary<string, TokenTypes> KeyWords = new Dictionary<string, TokenTypes>
    {
        {"Spawn", TokenTypes.Spawn},
        {"Color", TokenTypes.Color},
        {"Size", TokenTypes.Size},
        {"DrawLine", TokenTypes.DrawLine},
        {"DrawCircle", TokenTypes.DrawCircle},
        {"DrawRectangle", TokenTypes.DrawRectangle},
        {"Fill", TokenTypes.Fill},
        {"GetActualX", TokenTypes.GetActualX},
        {"GetActualY", TokenTypes.GetActualY},
        {"GetCanvasSize", TokenTypes.GetCanvasSize},
        {"GetColorCount", TokenTypes.GetColorCount},
        {"IsBrushColor", TokenTypes.IsBrushColor},
        {"IsBrushSize", TokenTypes.IsBrushSize},
        {"IsCanvasColor", TokenTypes.IsCanvasColor},
        {"IsColor", TokenTypes.IsColor}
    };
}