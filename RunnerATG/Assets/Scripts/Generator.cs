using UnityEngine;

public class Generator : MonoBehaviour
{
    // Adjustable variables for Unity Inspector
    int Width = 64;
    int Height = 64;
    [SerializeField]
    float Speed = 0.25f;
    [SerializeField]
    int TerrainOctaves = 6;
    [SerializeField]
    double TerrainFrequency = 1.25;

    // private variables
    FastNoiseLite HeightMap;
    MapData HeightData;
    Tile[,] Tiles;

    MeshFilter meshFilter;
    Mesh originalMesh;

    // Our texture output gameobject
    MeshRenderer HeightMapRenderer;
    Vector2 offset = Vector2.zero;

    void Start()
    {
        HeightMapRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        originalMesh = meshFilter.mesh;

        Initialize();
        GetData(HeightMap, ref HeightData);
        LoadTiles();

        UpdateNeighbors();

        HeightMapRenderer.materials[0].mainTexture = TextureGenerator.GetTexture(Width, Height, Tiles);
    }

    void Update()
    {
        // Обновите смещение каждый кадр, основываясь на скорости движения ландшафта
        offset -= new Vector2(0, Time.deltaTime * Speed);
        if (Input.GetKey(KeyCode.A))
            offset -= new Vector2(Time.deltaTime * Speed, 0);
        if (Input.GetKey(KeyCode.D))
            offset += new Vector2(Time.deltaTime * Speed, 0);

        // Обновите текстуру и высоты для каждого объекта Plane
        GetData(HeightMap, ref HeightData);
        LoadTiles();
        UpdateMesh();

        UpdateNeighbors();

        HeightMapRenderer.material.mainTexture = TextureGenerator.GetTexture(Width, Height, Tiles);
    }

    private void Initialize()
    {
        // Initialize the HeightMap Generator
        HeightMap = new FastNoiseLite(UnityEngine.Random.Range(0, int.MaxValue));
        HeightMap.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        HeightMap.SetFractalType(FastNoiseLite.FractalType.FBm);
        HeightMap.SetFractalOctaves(TerrainOctaves);
        HeightMap.SetFractalLacunarity((float)TerrainFrequency);
    }

    private void UpdateMesh()
    {
        Vector3[] vertices = originalMesh.vertices;

        int planeWidth = (int)Mathf.Sqrt(vertices.Length);
        int planeHeight = (int)Mathf.Sqrt(vertices.Length);

        for (int y = 0; y < planeHeight; y++)
        {
            for (int x = 0; x < planeWidth; x++)
            {
                int index = x + y * planeWidth;
                if (index < vertices.Length)
                {
                    int tileX = (int)(((float)x / (planeWidth - 1)) * (Width - 1));
                    int tileY = (int)(((float)y / (planeHeight - 1)) * (Height - 1));
                    vertices[index].y = Tiles[tileX, tileY].HeightValue * 10;
                }
            }
        }

        originalMesh.vertices = vertices;
        originalMesh.RecalculateNormals();
    }

    // Extract data from a noise generator
    private void GetData(FastNoiseLite generator, ref MapData mapData)
    {
        mapData = new MapData(Width, Height);

        // loop through each x, y point - get height value
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                float x1 = 0, x2 = 2;
                float y1 = 0, y2 = 2;
                float dx = x2 - x1;
                float dy = y2 - y1;

                float s = x / (float)Width;
                float t = y / (float)Height;

                float nx = x1 + Mathf.Cos(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                float ny = y1 + Mathf.Cos(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);
                float nz = x1 + Mathf.Sin(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);

                float heightValue = (float)HeightMap.GetNoise(nx, ny, nz);

                if (heightValue > mapData.Max) mapData.Max = heightValue;
                if (heightValue < mapData.Min) mapData.Min = heightValue;

                mapData.Data[x, y] = heightValue;
            }
        }
    }

    // Build a Tile array from our data
    private void LoadTiles()
    {
        Tiles = new Tile[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile t = new Tile();
                t.X = x;
                t.Y = y;

                float value = HeightData.Data[x, y];
                value = (value - HeightData.Min) / (HeightData.Max - HeightData.Min);

                t.HeightValue = value;

                Tiles[x, y] = t;
            }
        }
    }

    private void UpdateNeighbors()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile t = Tiles[x, y];

                t.Top = GetTop(t);
                t.Bottom = GetBottom(t);
                t.Left = GetLeft(t);
                t.Right = GetRight(t);
            }
        }
    }

    private Tile GetTop(Tile t)
    {
        return Tiles[t.X, MathHelper.Mod(t.Y - 1, Height)];
    }
    private Tile GetBottom(Tile t)
    {
        return Tiles[t.X, MathHelper.Mod(t.Y + 1, Height)];
    }
    private Tile GetLeft(Tile t)
    {
        return Tiles[MathHelper.Mod(t.X - 1, Width), t.Y];
    }
    private Tile GetRight(Tile t)
    {
        return Tiles[MathHelper.Mod(t.X + 1, Width), t.Y];
    }
}