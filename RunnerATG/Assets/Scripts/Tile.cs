public class Tile
{
	// �������� ������ ������
	public float HeightValue { get; set; }
	// ���������� ������ �� ���� X � Y
	public int X, Y;

	// �������� ������: �����, ������, ������ � �����
	public Tile Left;
	public Tile Right;
	public Tile Top;
	public Tile Bottom;
}
