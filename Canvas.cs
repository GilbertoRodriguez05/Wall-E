using System.Drawing;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

public class Canvas
{
    public Colors[,] Board;
    public int ActualX { get; set; }
    public int ActualY { get; set; }
    public int BrushSize { get; set; }
    public Colors BrushColor { get; set; }
    public Canvas(int ActualX, int ActualY)
    {
        this.Board = new Colors[10, 10];
        this.ActualX = ActualX;
        this.ActualY = ActualY;
    }
    public int filas {get{return Board.GetLength(0);}}
    public int columnas {get{return Board.GetLength(1);}}
    public void InicializarCanvas(Colors colors = Colors.White)
    {
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                Board[i, j] = colors;
            }
        }
    }
}
public enum Colors
{
    Blue, Red, Green, Yellow, Black, White, Purple, Orange, Transparent
}