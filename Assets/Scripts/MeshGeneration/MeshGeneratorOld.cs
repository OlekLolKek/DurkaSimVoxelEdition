using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorOld : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _offsetX = 100.0f;
    [SerializeField] private float _offsetY = 100.0f;
    [SerializeField] private float _scale = 20.0f;
    [SerializeField] private int _xSize = 20;
    [SerializeField] private int _zSize = 20;

    private MeshCollider _collider;
    private Vector3[] _vertices;
    private Vector2[] _uvs;
    private Renderer _renderer;
    private Mesh _mesh;
    private int[] _triangles;
    private int _height = 256;
    private int _width = 256;


    #endregion


    #region UnityMethods

    private void Start()
    {
        _offsetX = Random.Range(0.0f, 99999);
        _offsetY = Random.Range(0.0f, 99999);


        _mesh = new Mesh();
        _collider = GetComponent<MeshCollider>();
        GetComponent<MeshFilter>().mesh = _mesh;

        _renderer = GetComponent<Renderer>();
        //_renderer.material.mainTexture = GenerateTexture();


        CreateShape();
        _collider.sharedMesh = _mesh;
    }

    private void Update()
    {
        UpdateMesh();
    }

    #endregion


    #region Methods

    private void CreateShape()
    {
        _vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];


        for (int i = 0, z = 0; z <= _zSize; z++)
        {
            for (int x = 0; x <= _xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2.0f;
                //float y = GetNoiseSample(x, z);
                _vertices[i] = new Vector3(x, y, z);

                i++;
            }
        }

        _triangles = new int[_xSize * _zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < _zSize; z++)
        {
            for (int x = 0; x < _xSize; x++)
            {
                _triangles[tris + 0] = vert + 0;
                _triangles[tris + 1] = vert + _xSize + 1;
                _triangles[tris + 2] = vert + 1;
                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + _xSize + 1;
                _triangles[tris + 5] = vert + _xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }


        _uvs = new Vector2[_vertices.Length];

        for (int i = 0, z = 0; z <= _zSize; z++)
        {
            for (int x = 0; x <= _xSize; x++)
            {
                _uvs[i] = new Vector2((float)x / _xSize, (float)z / _zSize);
                i++;
            }
        }

        UpdateMesh();
    }

    private void UpdateMesh()
    {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs;

        _mesh.RecalculateNormals();
    }

    private Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(_width, _height);

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    private Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / _width * _scale + _offsetX;
        float yCoord = (float)y / _height * _scale + _offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }

    #endregion
}
