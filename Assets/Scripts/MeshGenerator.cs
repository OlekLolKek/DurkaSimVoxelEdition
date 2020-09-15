using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    #region Fields

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;

    #endregion


    #region UnityMethods

    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        CreateShape();
        UpdateMesh();
    }

    #endregion


    #region Methods

    private void CreateShape()
    {
        _vertices = new Vector3[]
        {
            new Vector3 (0,0,0),
            new Vector3 (0,0,1),
            new Vector3 (1,0,0)
        };

        _triangles = new int[]
        {
            0, 1, 2
        };
    }

    private void UpdateMesh()
    {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
    }

    #endregion
}
