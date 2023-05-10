public class Tile
{
	// Значение высоты плитки
	public float HeightValue { get; set; }
	// Координаты плитки по осям X и Y
	public int X, Y;

	// Соседние плитки: слева, справа, сверху и снизу
	public Tile Left;
	public Tile Right;
	public Tile Top;
	public Tile Bottom;
}
