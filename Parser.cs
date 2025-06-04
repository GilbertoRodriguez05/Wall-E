using System.Drawing;

class Parser
{
    Entorno entorno;
    Canvas canvas;
    List<Token> tokens = new List<Token>();
    List<Error> errors = new List<Error>();
    public int Current = 0;
    public string code;

    Spawn spawn;
    public Parser(List<Token> tokens, List<Error> errors, Entorno entorno, Canvas canvas)
    {
        this.tokens = tokens;
        this.errors = errors;
        this.entorno = entorno;
        this.canvas = canvas;
    }

    public AST Main()
    {
        if (!Match(TokenTypes.Spawn) && !Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba spawn", Previous().line));

        Numero x = null;
        Numero y = null;
        if (Match(TokenTypes.Numero)) x = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma"));

        if (Match(TokenTypes.Numero)) y = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un parentesis cerrado", Previous().line));


        spawn = new Spawn(x, y, canvas);
        return Block();


    }
    public Expresions Expresions()
    {
        return Or();
    }
    public Expresions Or()
    {
        Expresions expresion = And();
        while (Match(TokenTypes.Or))
        {
            Expresions right = And();
            expresion = new Or(expresion, right);
        }
        return expresion;
    }
    public Expresions And()
    {
        Expresions expresion = Igualdad();
        while (Match(TokenTypes.And))
        {
            Expresions right = Igualdad();
            expresion = new And(expresion, right);
        }
        return expresion;
    }
    public Expresions Igualdad()
    {
        Expresions expresion = Comparation();
        while (Match(TokenTypes.Igual, TokenTypes.Distinto))
        {
            Expresions right = Comparation();
            Token Operator = Previous();
            if (Operator.types == TokenTypes.Distinto) expresion = new Diferent(expresion, right);
            else if (Operator.types == TokenTypes.Igual) expresion = new Equal(expresion, right);
        }
        return expresion;
    }
    public Expresions Comparation()
    {
        Expresions expresion = Term();
        while (Match(TokenTypes.Mayor, TokenTypes.Menor, TokenTypes.MayorIgual, TokenTypes.MenorIgual))
        {
            Expresions right = Term();
            Token Operator = Previous();
            if (Operator.types == TokenTypes.Mayor) expresion = new GreaterThan(expresion, right);
            else if (Operator.types == TokenTypes.Menor) expresion = new LessThan(expresion, right);
            else if (Operator.types == TokenTypes.MayorIgual) expresion = new GreaterOrEqual(expresion, right);
            else if (Operator.types == TokenTypes.MenorIgual) expresion = new LessOrEqual(expresion, right);
        }
        return expresion;
    }
    public Expresions Term()
    {
        Expresions expresion = Factor();
        while (Match(TokenTypes.Suma, TokenTypes.Resta))
        {
            Expresions right = Factor();
            Token Operator = Previous();
            if (Operator.types == TokenTypes.Suma) expresion = new Addition(expresion, right);
            else if (Operator.types == TokenTypes.Resta) expresion = new Substraction(expresion, right);
        }
        return expresion;
    }
    public Expresions Factor()
    {
        Expresions expresion = Exponent();
        while (Match(TokenTypes.Multiplicacion, TokenTypes.Division))
        {
            Expresions right = Exponent();
            Token Operator = Previous();
            if (Operator.types == TokenTypes.Multiplicacion) expresion = new Multiplication(expresion, right);
            else if (Operator.types == TokenTypes.Division) expresion = new Divition(expresion, right);
        }
        return expresion;
    }
    public Expresions Exponent()
    {
        Expresions expresions = Negation();
        while (Match(TokenTypes.Potencia, TokenTypes.Modulo))
        {
            Expresions right = Negation();
            Token Operator = Previous();

            if (Operator.types == TokenTypes.Potencia) expresions = new Pow(expresions, right);
            else if (Operator.types == TokenTypes.Modulo) expresions = new Module(expresions, right);
        }
        return expresions;
    }
    public Expresions Negation()
    {
        if (Match(TokenTypes.Negacion))
        {
            Expresions right = Negation();
            return new Not((bool)right.value);
        }
        return Primary();
    }
    public Expresions Primary()
    {
        if (Match(TokenTypes.Numero)) return new Numero(Convert.ToInt32(Previous().literal));
        else if (Match(TokenTypes.Cadena)) return new Cadena(Previous().lexeme);
        else if (Match(TokenTypes.True)) return new Booleano(true);
        else if (Match(TokenTypes.False)) return new Booleano(false);
        else if (Match(TokenTypes.AbreParentesis))
        {
            Expresions expresion = Expresions();
            if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba )", Previous().line));
            return new Grouping(expresion);
        }
        else if (Match(TokenTypes.Identificador))
        {
            if (Match(TokenTypes.Declaracion))
            {
                Token variable = Previous();
                Expresions expresion = new Variable(variable.lexeme);

                return expresion;
            }
            else
            {
                errors.Add(new Error(TypeOfError.Invalid, "Expresion invalida", Previous().line));
                throw new Exception("Expresion invalida " +  Previous().line);
            }
        }
        else
        {
            errors.Add(new Error(TypeOfError.Invalid, "Expresion invalida", Previous().line));
            throw new Exception("Expresion invalida " +  Previous().line);
        }
    }
    public bool Match(params TokenTypes[] types)
    {
        foreach (TokenTypes item in types)
        {
            if (Check(item))
            {
                code += tokens[Current].lexeme + " ";
                Move();
                return true;
            }
        }
        return false;
    }
    public bool Check(TokenTypes types)
    {
        if (IsAtEnd()) return false;
        return Actual().types == types;
    }
    public bool Next(params TokenTypes[] types)
    {
        Token token = tokens[Current + 1];
        return token != null && types.Contains(token.types);
    }
    public Token Move()
    {
        if (!IsAtEnd()) Current++;
        return Previous();
    }
    public bool IsAtEnd()
    {
        if (Current == tokens.Count - 1) return true;
        else return false;
    }
    public Token Actual()
    {
        return tokens[Current];
    }
    public Token Previous()
    {
        return tokens[Current - 1];
    }
    public bool IsKeyword(Token token)
    {
        if (Scanner.KeyWords.ContainsValue(token.types))
        {
            Move();
            return true;
        }
        return false;
    }
    public AST VarDeclaration(Expresions expresions)
    {
        Token operador = Previous();
        Expresions initializer = Expresions();
        return new VarDeclaration(expresions, initializer, operador, entorno);
    }
    public AST Declaration()
    {
        AST ast = null;
        if (Check(TokenTypes.Identificador))
        {
            Expresions expresions = Expresions();
            ast = new ExpresionEvaluator(expresions);
            if (Match(TokenTypes.Declaracion))
            {
                ast = VarDeclaration(expresions);
            }
        }
        else throw new Error(TypeOfError.Invalid, "Expresion invalida" , Previous().line);
        return ast;
    }

    public AST Color()
    {
        Cadena color = null;
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

        if (Match(TokenTypes.Cadena)) color = new Cadena(Previous().lexeme);
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba una cadena", Previous().line));

        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));

        return new Color(color, canvas);
    }
    public AST Size()
    {
        Numero size = null;
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

        if (Match(TokenTypes.Numero)) size = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new Size(size, canvas);
    }
    public AST DrawLine()
    {
        Numero intX = null;
        Numero intY = null;
        Numero Distance = null;
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

        if (Match(TokenTypes.Numero)) intX = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if(!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) intY = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if(!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) Distance = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new DrawLine(intX, intY, Distance, canvas);
    }
    public AST DrawCircle()
    {
        Numero intX = null;
        Numero intY = null;
        Numero radius = null;
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

        if (Match(TokenTypes.Numero)) intX = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) intY = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) radius = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new DrawCircle(intX, intY, radius, canvas);
    }
    public AST DrawRectangle()
    {
        Numero intX = null;
        Numero intY = null;
        Numero width = null;
        Numero height = null;
        Numero distance = null;
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

        if (Match(TokenTypes.Numero)) intX = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) intY = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) distance = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) width = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) height = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new DrawRectangle(intX, intY, distance, width, height, canvas);
    }
    public AST Fill()
    {
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));
        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new Fill(canvas);
    }
    public AST GetActualX()
    {
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));
        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new GetActualX(canvas);
    }
    public AST GetActualY()
    {
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));
        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new GetActualY(canvas);
    }
    public AST GetCanvasSize()
    {
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));
        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new GetCanvasSize(canvas);
    }
    public AST GetColorCount()
    {
        Cadena color = null;
        Numero x1 = null;
        Numero y1 = null;
        Numero x2 = null;
        Numero y2 = null;
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

        if (Match(TokenTypes.Numero)) color = new Cadena(Previous().lexeme);
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba una cadena", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) x1 = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) y1 = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) x2 = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) y2 = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new GetColorCount(color, x1, y1, x2, y2, canvas);
    }
    public AST IsBrushColor()
    {
        Cadena color = null;
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

        if (Match(TokenTypes.Cadena)) color = new Cadena(Previous().lexeme);
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba una cadena", Previous().line));

        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new IsBrushColor(color, canvas);
    }
    public AST IsBrushSize()
    {
        Numero k = null;
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

        if (Match(TokenTypes.Numero)) k = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new IsBrushSize(k, canvas);
    }
    public AST IsCanvasColor()
    {
        Cadena color = null;
        Numero v = null;
        Numero h = null;
        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un abre parentesis", Previous().line));

        if (Match(TokenTypes.Cadena)) color = new Cadena(Previous().lexeme);
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba una cadena", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) v = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.Coma)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba una coma", Previous().line));

        if (Match(TokenTypes.Numero)) h = new Numero(Convert.ToInt32(Previous().literal));
        else errors.Add(new Error(TypeOfError.Expected, "Se esperaba un numero", Previous().line));

        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba un cierra parentesis", Previous().line));
        return new IsCanvasColor(color, v, h, canvas);
    }
    public AST Label()
    {
       return new Label(Previous().lexeme, Block(), entorno);
    }
    public AST GoTo()
    {
        Label label = null;
        if (!Match(TokenTypes.AbreCorchete)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba [", Previous().line));

        if (Match(TokenTypes.Identificador) && entorno.labels.Contains(Previous().lexeme))
        label = new Label(Previous().lexeme, Block(), entorno);
        else errors.Add(new Error(TypeOfError.Expected, "Se esparaba un label", Previous().line));

        if (!Match(TokenTypes.CierraCorchete)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba ]", Previous().line));

        if (!Match(TokenTypes.AbreParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba (", Previous().line));
        Expresions Condition = Expresions();
        if (!Match(TokenTypes.CierraParentesis)) errors.Add(new Error(TypeOfError.Expected, "Se esperaba )", Previous().line));

        return new GoTo(label, Condition);
    }
    public Block Block()
    {
        List<AST> Declarations = [spawn];
        do
        {
            try
            {
                if (Match(TokenTypes.GetActualX)) Declarations.Add(new GetActualX(canvas));
                else if (Match(TokenTypes.GetActualY)) Declarations.Add(new GetActualY(canvas));
                else if (Match(TokenTypes.Color)) Declarations.Add(Color());
                else if (Match(TokenTypes.Size)) Declarations.Add(Size());
                else if (Match(TokenTypes.DrawLine)) Declarations.Add(DrawLine());
                else if (Match(TokenTypes.DrawCircle)) Declarations.Add(DrawCircle());
                else if (Match(TokenTypes.DrawRectangle)) Declarations.Add(DrawRectangle());
                else if (Match(TokenTypes.Fill)) Declarations.Add(Fill());
                else if (Match(TokenTypes.GetActualX)) Declarations.Add(GetActualX());
                else if (Match(TokenTypes.GetActualY)) Declarations.Add(GetActualY());
                else if (Match(TokenTypes.GetCanvasSize)) Declarations.Add(GetCanvasSize());
                else if (Match(TokenTypes.GetColorCount)) Declarations.Add(GetColorCount());
                else if (Match(TokenTypes.IsBrushColor)) Declarations.Add(IsBrushColor());
                else if (Match(TokenTypes.IsBrushSize)) Declarations.Add(IsBrushSize());
                else if (Match(TokenTypes.IsCanvasColor)) Declarations.Add(IsCanvasColor());
                else if (Match(TokenTypes.GoTo)) Declarations.Add(GoTo());
                else if (Match(TokenTypes.Identificador))
                {
                    if (Check(TokenTypes.Declaracion))
                    {
                        Declarations.Add(Declaration());
                    }
                    else
                    {
                        Declarations.Add(Label());
                    }
                }
                else Declarations.Add(Declaration());
            }
            catch (Error)
            {
                throw new Error (TypeOfError.Invalid, "Error en la declaracion", Previous().line);
            }
        } while (!Match(TokenTypes.EOF));

        return new Block(Declarations);
    }
    
    
}