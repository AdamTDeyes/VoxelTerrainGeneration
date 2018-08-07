using System.Collections.Generic;
using UnityEngine;

public class UVMap {

    private static List<UVMap> _Maps = new List<UVMap>();
    public string name;
    public Vector2[] _UVMap;

    public UVMap(string name, Vector2[] _UVMap)
    {
        this.name = name;
        this._UVMap = _UVMap;
    }

    public void Register()
    {
        _Maps.Add(this);
    }

    public static UVMap GetUvMap(string name)
    {
        foreach (UVMap m in _Maps)
        {
            if (m.name.Equals(name))
            {
                return m;
            }
        }
        Debug.Log("Can't find assoicated image called " + name);
        foreach (UVMap m in _Maps)
        {
            if (m.name.Equals(name))
            {
                return m;
            }
        }
        return _Maps[0];
    }
}
