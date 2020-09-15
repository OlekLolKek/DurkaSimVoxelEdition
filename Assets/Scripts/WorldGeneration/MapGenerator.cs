using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    #region PrivateData

    private enum DrawMode
    {
        NoiseMap, ColorMap, Mesh
    }

    [System.Serializable]
    private struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }

    #endregion


    #region Fields

    public bool AutoUpdate;

    [SerializeField] private DrawMode _drawMode;

    [Range(0, 6)]
    [SerializeField] private int _levelOfDetail;
    [SerializeField] private float _noiseScale;

    [SerializeField] private int _octaves;
    [Range(0,1)]
    [SerializeField] private float _persistance;
    [SerializeField] private float _lacunarity;

    [SerializeField] private int _seed;
    [SerializeField] private Vector2 _offset;

    [SerializeField] private float _meshHeightMultiplier;
    [SerializeField] private AnimationCurve _meshHeightCurve;

    [SerializeField] private TerrainType[] regions;

    private const int MAP_CHUNK_SIZE = 241;

    #endregion


    #region UnityMethods

    private void OnValidate()
    {
        if (_lacunarity < 1)
        {
            _lacunarity = 1;
        }
        if (_octaves < 0)
        {
            _octaves = 0;
        }
    }


    #endregion


    #region Methods

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(MAP_CHUNK_SIZE, MAP_CHUNK_SIZE, _seed, _noiseScale, _octaves, _persistance, _lacunarity, _offset);

        Color[] colorMap = new Color[MAP_CHUNK_SIZE * MAP_CHUNK_SIZE];
        for (int y = 0; y < MAP_CHUNK_SIZE; y++)
        {
            for (int x = 0; x < MAP_CHUNK_SIZE; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * MAP_CHUNK_SIZE + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (_drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenetaror.TextureFromHeightMap(noiseMap));
        }
        else if (_drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenetaror.TextureFromColorMap(colorMap, MAP_CHUNK_SIZE, MAP_CHUNK_SIZE));
        }
        else if (_drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, _meshHeightMultiplier, _meshHeightCurve, _levelOfDetail), TextureGenetaror.TextureFromColorMap(colorMap, MAP_CHUNK_SIZE, MAP_CHUNK_SIZE));
        }

    }

    #endregion
}
