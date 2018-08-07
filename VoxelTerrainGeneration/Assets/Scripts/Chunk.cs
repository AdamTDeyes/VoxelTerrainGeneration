using System;
using UnityEngine;

public class Chunk : ITickable
{
    private static bool FirstChunk = false;
    private bool IsFirstChunk = false;
    private World world;
    public static readonly int ChunkWidth = 20;
    public static readonly int ChunkHeight = 20;
    private Block[,,] _Blocks;
    public int PosX { private set; get; }
    public int PosZ { private set; get; }

    public Chunk(int px, int pz, World world)
    {
        PosX = px;
        PosZ = pz;
        this.world = world;
    }

    public Chunk(int px,int pz, int[,,] _data, World world)
    {
        HasGenerated = true;
        PosX = px;
        PosZ = pz;
        LoadChunkFromData(_data);
        this.world = world;
    }

    protected bool HasGenerated = false;

    public float GetHeight(float px, float pz, float py)
    {
        px += (PosX * ChunkWidth);
        pz += (PosZ * ChunkWidth);

        float p1 = Mathf.PerlinNoise(px / GameManager.sdx, pz / GameManager.sdz) * GameManager.smul;
        p1 *= (GameManager.smy * py);
        return p1;
    }

    public virtual void Start()
    {
        if (!FirstChunk)
        {
            FirstChunk = true;
            IsFirstChunk = true;
        }
        if (HasGenerated)
            return;
        _Blocks = new Block[ChunkWidth, ChunkHeight, ChunkWidth];
        for(int x = 0; x < ChunkWidth; x++)
        {
            for (int y = 0; y < ChunkHeight; y++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    float perlin = GetHeight(x, z, y);
                    if(perlin > GameManager.scutoff)
                    {
                        _Blocks[x, y, z] = Block.Air;                        
                    }
                    else
                    {
                        if (perlin > GameManager.scutoff / 1.1)
                        {
                            _Blocks[x, y, z] = Block.Grass;
                        }
                        else if (perlin > GameManager.scutoff / 2)
                        {
                            _Blocks[x, y, z] = Block.Dirt;
                        }
                        else
                        {
                            _Blocks[x, y, z] = Block.Stone;
                        }
                    }
                    if (y < 1)
                    {
                        _Blocks[x, y, z] = Block.Bedrock;
                    }
                }
            }
        }
        HasGenerated = true;
    }

    public void Tick()
    {
        
    }

    public void Degenerate()
    {
        try
        {
            Serializer.Serialize_ToFileFullPath<int[,,]>(FileManager.GetChunkString(PosX, PosZ), GetChunkSaveData());
        }
        catch (System.Exception E)
        {
            Debug.Log(E.ToString());
        }
        GameManager.instance.RegisterDelegate(new Action(() => {
            GameObject.Destroy(go);
        }));
        world.RemoveChunk(this);
    }

    public int[,,] GetChunkSaveData()
    {
        return _Blocks.ToIntArray();
    }

    public void LoadChunkFromData(int[,,] _data)
    {
        _Blocks = _data.ToBlockArray();
    }

    protected bool HasDrawn = false;
    private MeshData data;
    protected bool Drawnlock = false;
    bool NeedToUpdate = false;

    public virtual void Update()
    {
        if (NeedToUpdate)
        {
            if (!Drawnlock && !RenderingLock)
            {
                HasDrawn = false;
                HasRendered = false;
                NeedToUpdate = false;
            }
        }
        if (!HasDrawn && HasGenerated && !Drawnlock)
        {
            Drawnlock = true;
            data = new MeshData();
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int y = 0; y < ChunkHeight; y++)
                {
                    for (int z = 0; z < ChunkWidth; z++)
                    {
                        data.Merge(_Blocks[x, y, z].Draw(this, _Blocks, x, y, z));
                    }
                }
            }
            Drawnlock = false;
            HasDrawn = true;
        }
    }

    protected bool HasRendered = false;
    private GameObject go;
    private bool RenderingLock = false;

    public virtual void OnUnityUpdate()
    {
        if(HasGenerated && !HasRendered && HasDrawn && !RenderingLock)
        {
            RenderingLock = true;
            HasRendered = true;
            Mesh mesh = data.ToMesh();
            if(go == null)
            {
                go = new GameObject();
            }
            Transform t = go.transform;
            if (t.gameObject.GetComponent<MeshFilter>() == null)
            {
                t.gameObject.AddComponent<MeshFilter>();
                //t.gameObject.AddComponent<MeshRenderer>();
                t.gameObject.AddComponent<MeshCollider>();
                //t.gameObject.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Chunkmat");
                t.transform.position = new Vector3(PosX * ChunkWidth, 0, PosZ * ChunkWidth);
                Texture2D tmp = new Texture2D(0, 0);
                tmp.LoadImage(System.IO.File.ReadAllBytes("Atlas.png"));
                tmp.filterMode = FilterMode.Point;
                t.gameObject.AddComponent<MeshRenderer>().material.mainTexture = tmp;
            }
            t.transform.GetComponent<MeshFilter>().sharedMesh = mesh;
            t.transform.GetComponent<MeshCollider>().sharedMesh = mesh;

            RenderingLock = false;

            if (IsFirstChunk)
            {
                GameManager.instance.StartPlayer(new Vector3(PosX * ChunkWidth, 100,PosZ * ChunkWidth));
                IsFirstChunk = false;
            }
        }
    }

    internal void SetBlock(int x, int y, int z, Block blocks)
    {
        _Blocks[x, y, z] = blocks;
        NeedToUpdate = true;
    }
}