using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    #region Fields

    [SerializeField] private Gradient _gradient;
    [SerializeField] private int _xSize = 20;
    [SerializeField] private int _zSize = 20;

    private MeshCollider _collider;
    private Vector3[] _vertices;
    private Color[] _colors;
    private Mesh _mesh;
    private int[] _triangles;
    private float _minTerrainHeight;
    private float _maxTerrainHeight;

    #endregion


    #region UnityMethods

    private void Start()
    {
        _mesh = new Mesh();
        _collider = GetComponent<MeshCollider>();
        GetComponent<MeshFilter>().mesh = _mesh;


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
                _vertices[i] = new Vector3(x, y, z);

                if (y > _maxTerrainHeight)
                {
                    _maxTerrainHeight = y;
                }
                if (y < _minTerrainHeight)
                {
                    _minTerrainHeight = y;
                }

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

        _colors = new Color[_vertices.Length];
        for (int i = 0, z = 0; z <= _zSize; z++)
        {
            for (int x = 0; x <= _xSize; x++)
            {
                float height = Mathf.InverseLerp(_minTerrainHeight, _maxTerrainHeight, _vertices[i].y);
                _colors[i] = _gradient.Evaluate(height);
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
        _mesh.colors = _colors;

        _mesh.RecalculateNormals();
    }

    #endregion
}
