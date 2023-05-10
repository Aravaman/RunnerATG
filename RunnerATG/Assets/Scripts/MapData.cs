public class MapData
{
    public float[,] Data;
    public float Min { get; set; }
    public float Max { get; set; }

    //  онструктор класса MapData
    public MapData(int width, int height)
    {
        // »нициализаци€ массива данных карты
        Data = new float[width, height];
        // ”становка начальных значений дл€ минимальной и максимальной высоты
        Min = float.MaxValue;
        Max = float.MinValue;
    }
}
