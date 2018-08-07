using System.Collections.Generic;
using UnityEngine;

public class MeshData {
    private List<Vector3> _Verts = new List<Vector3>();
    private List<int> _Tries = new List<int>();
    private List<Vector2> _UVS = new List<Vector2>();

    public MeshData(List<Vector3> V, List<int> T, Vector2[] UVS)
    {
        _Verts = V;
        _Tries = T;
        _UVS = new List<Vector2>(UVS);
    }

    public MeshData()
    {

    }

    public void AddPos(Vector3 loc)
    {
        for(int i = 0; i < _Verts.Count; i++)
        {
            _Verts[i] = _Verts[i] + loc;
        }
    }

    public void Merge(MeshData M)
    {
        if(M._Verts.Count <= 0)
        {
            return;
        }
        if (_Verts.Count <= 0)
        {
            _Verts = M._Verts;
            _Tries = M._Tries;
            _UVS = M._UVS;
            return;
        }
        int count = _Verts.Count;
        _Verts.AddRange(M._Verts);
        for (int i = 0; i < M._Tries.Count; i++)
        {
            _Tries.Add(M._Tries[i] + count);
        }
        _UVS.AddRange(M._UVS);
    }

    public Mesh ToMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = _Verts.ToArray();
        mesh.triangles = _Tries.ToArray();
        mesh.uv = _UVS.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        //mesh.Optimize();
        return mesh;
    }
}