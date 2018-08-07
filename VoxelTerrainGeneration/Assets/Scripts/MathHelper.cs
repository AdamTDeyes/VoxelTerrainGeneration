using UnityEngine;

public class Int3
{
    public int x, y, z;
    public Int3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Int3(Vector3 pos)
    {
        this.x = (int)pos.x;
        this.y = (int)pos.y;
        this.z = (int)pos.z;
    }

    public override string ToString()
    {
        return string.Format("X:{0},Y:{1},Z{2}", x, y, z);
    }

    internal void AddPos(Int3 int3)
    {
        this.x += int3.x;
        this.y += int3.y;
        this.z += int3.z;
    }

    internal void ToChunkCoordinates()
    {
        this.x = Mathf.FloorToInt(x / Chunk.ChunkWidth);
        this.z = Mathf.FloorToInt(z / Chunk.ChunkWidth);
    }
}

public class MathHelper
{
    public static MeshData DrawCube(Chunk chunk, Block[,,] _Blocks, Block block, int x, int y, int z, Vector2[] _uvmap)
    {

        MeshData d = new MeshData();
        if (block.Equals(Block.Air))
        {
            return new MeshData();
        }

        if (y - 1 <= 0 || _Blocks[x, y - 1, z].Istransparent())
        {
            d.Merge(new MeshData( // Bottom Face
        new System.Collections.Generic.List<Vector3>() {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)
        },
        new System.Collections.Generic.List<int>() {
                 0,2,1,3,1,2 //draw both triangles first 3 numbers are for the first, and the second 3 are for the second triangle.
        },
        _uvmap)); // Applies the texture
        }

        if (y + 1 >= Chunk.ChunkHeight || _Blocks[x, y + 1, z].Istransparent())
        {
            d.Merge(new MeshData( // Top Face
          new System.Collections.Generic.List<Vector3>() {
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,0),
            new Vector3(1,1,1)
           },
           new System.Collections.Generic.List<int>() {
                 0,1,2,3,2,1
           },
            _uvmap));
        }

        if (x - 1 <= 0 || _Blocks[x - 1, y, z].Istransparent())
        {
            d.Merge(new MeshData( // Front Face
         new System.Collections.Generic.List<Vector3>() {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(0,1,0),
            new Vector3(0,1,1)
          },
          new System.Collections.Generic.List<int>() {
                 0,1,2,3,2,1
          },
           _uvmap));
        }

        if (x + 1 >= Chunk.ChunkWidth || _Blocks[x + 1, y, z].Istransparent())
        {
            d.Merge(new MeshData( // Back Face
          new System.Collections.Generic.List<Vector3>() {
            new Vector3(1,0,0),
            new Vector3(1,0,1),
            new Vector3(1,1,0),
            new Vector3(1,1,1)
           },
           new System.Collections.Generic.List<int>() {
                 0,2,1,3,1,2
           },
            _uvmap));

        }

        if (z + 1 >= Chunk.ChunkWidth || _Blocks[x, y, z + 1].Istransparent())
        {
            d.Merge(new MeshData( // Right Face
          new System.Collections.Generic.List<Vector3>() {
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(0,1,1),
            new Vector3(1,1,1)
           },
           new System.Collections.Generic.List<int>() {
                 0,1,2,3,2,1
           },
            _uvmap));
        }

        if (z - 1 <= 0 || _Blocks[x, y, z - 1].Istransparent())
        {
            d.Merge(new MeshData( // Left Face
            new System.Collections.Generic.List<Vector3>() {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0)
         },
         new System.Collections.Generic.List<int>() {
                 0,2,1    ,3,1,2
         },
         _uvmap));

        }
        d.AddPos(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        return d;
    }

    internal static void AddBlock(Vector3 roundedposition, Block blocks)
    {
        if (roundedposition.y >= Chunk.ChunkHeight)
            return;
        int Chunkposx = Mathf.FloorToInt(roundedposition.x / Chunk.ChunkWidth);
        int Chunkposz = Mathf.FloorToInt(roundedposition.z / Chunk.ChunkWidth);
        Chunk currentchunk;
        try
        {
            currentchunk = World._Instance.GetChunk(Chunkposx, Chunkposz);
            if (currentchunk.GetType().Equals(typeof(ErroredChunk)))
            {
                Debug.Log("Current chunk is errored " + roundedposition.ToString());
            }
            int x = (int)(roundedposition.x - (Chunkposx * Chunk.ChunkWidth));
            int z = (int)(roundedposition.z - (Chunkposz * Chunk.ChunkWidth));
            int y = (int)roundedposition.y;

            currentchunk.SetBlock(x, y, z, blocks);
        }
        catch(System.Exception e)
        {
            Debug.Log("Current chunk is errored " + e.Message.ToString());
        }
    }
}