using UnityEngine;

public static class MyExtensions
{

    public static int[,,] ToIntArray(this Block[,,] _ChunkData)
    {
        int Lx = _ChunkData.GetLength(0);
        int Ly = _ChunkData.GetLength(1);
        int Lz = _ChunkData.GetLength(2);
        int[,,] data = new int[Lx, Ly, Lz];
        for (int x = 0; x < Lx; x++)
        {
            for (int y = 0; y < Ly; y++)
            {
                for (int z = 0; z < Lz; z++)
                {
                    data[x, y, z] = _ChunkData[x, y, z].GetID();
                }
            }
        }

        return data;
    }

    public static Block[,,] ToBlockArray(this int[,,] _data)
    {
        int Lx = _data.GetLength(0);
        int Ly = _data.GetLength(1);
        int Lz = _data.GetLength(2);
        Block[,,] ChunkData = new Block[Lx, Ly, Lz];
        for (int x = 0; x < Lx; x++)
        {
            for (int y = 0; y < Ly; y++)
            {
                for (int z = 0; z < Lz; z++)
                {
                    ChunkData[x, y, z] = BlockRegistry.GetBlockFromID(_data[x, y, z]);
                }
            }
        }

        return ChunkData;
    }

    public static Vector3 Cap(this Vector3 src)
    {
        return new Vector3(src.x < 0 ? 0 : src.x, src.y < 0 ? 0 : src.y, src.z < 0 ? 0 : src.z);
    }
}
