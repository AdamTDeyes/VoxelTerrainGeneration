using UnityEngine;

public class Block : ITickable
{
    private bool IsTransparent;
    public static Block Dirt = new Block("Dirt", false, "Assets/Textures/blocks/dirt.png");
    public static Block Stone = new Block("Stone", false, "Assets/Textures/blocks/stone.png");
    public static Block Grass = new Block("Grass", false, "Assets/Textures/blocks/grass_path_top.png");
    public static Block Bedrock = new Block("Bedrock", false, "Assets/Textures/blocks/bedrock.png");
    public static Block Air = new Block("Air", true);
    private string name;
    private Vector2[] _UVMap;
    private static int CurrentID = 0;
    private int ID;
    private string BlockName;

    public Block(string Name, bool IsTransparent)
    {
        this.BlockName = name;
        this.IsTransparent = IsTransparent;

        REGISTER();
    }

    private void REGISTER()
    {
        ID = CurrentID;
        CurrentID++;
        BlockRegistry.RegisterBlock(this);
    }
    
    public string GetBlockName()
    {
        return BlockName;
    }

    public int GetID()
    {
        return ID;
    }

    public Block(string BlockName, bool IsTransparent, string name)
    {
        this.BlockName = BlockName;
        this.IsTransparent = IsTransparent;
        this.name = name;

        _UVMap = UVMap.GetUvMap(name)._UVMap;
        REGISTER();
    }

    public bool Istransparent()
    {
        return IsTransparent;
    }

    public void Start()
    {
        
    }

    public void Tick()
    {
        
    }

    public void Update()
    {
        
    }

    public void OnUnityUpdate()
    {

    }

    public virtual MeshData Draw(Chunk chunk, Block[,,] _blocks, int x, int y, int z)
    {
        if (this.Equals(Air))
            return new MeshData();
        try
        {
            return MathHelper.DrawCube(chunk, _blocks, this, x, y, z, this._UVMap);
        }catch(System.Exception e)
        {
            UnityEngine.Debug.Log("In draw cube: " + e.StackTrace.ToString());
        }
        return new MeshData();
    }
}
