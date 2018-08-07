using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class World : ILoopable {

    public static World _Instance { get; private set; }
    private Thread WorldThread;
    private bool IsRunning;
    private static Int3 PlayerPos;
    private static readonly int RenderDistanceInChunks = 3;

    public static void Instantiate()
    {
        _Instance = new World();
        MainLoopable.GetInstance().RegisterLoopes(_Instance);
        System.Random r = new System.Random();
        PlayerPos = new Int3(r.Next(-1000, 1000),100, r.Next(-1000, 1000));
    }

    public void OnApplicationQuit()
    {
        foreach(Chunk c in _LoadedChunks)
        {
            try
            {
                Serializer.Serialize_ToFileFullPath<int[,,]>(FileManager.GetChunkString(c.PosX, c.PosZ), c.GetChunkSaveData());
            }
            catch(System.Exception E)
            {
                Debug.Log(E.ToString());
            }
            
            
        }
        IsRunning = false;
        Logger.Log("Stopping world thread");
    }

    private bool RanOnce = false;
    private List<Chunk> _LoadedChunks = new List<Chunk>();

    public void Start()
    {
        IsRunning = true;
        WorldThread = new Thread(() =>
        {
            Logger.Log("Initializing world thread");
            while (IsRunning)
            {
                //Logger.Log(new System.Exception("Unneeded exception"));
                try
                {
                    if (!RanOnce)
                    {
                        RanOnce = true;
                        for (int x = -RenderDistanceInChunks; x < RenderDistanceInChunks; x++)
                        {
                            for (int z = -RenderDistanceInChunks; z < RenderDistanceInChunks; z++)
                            {
                                Int3 newchunkpos = new Int3(PlayerPos.x, PlayerPos.y, PlayerPos.z);
                                newchunkpos.AddPos(new Int3(x * Chunk.ChunkWidth, 0, z * Chunk.ChunkWidth));
                                newchunkpos.ToChunkCoordinates();
                                if (System.IO.File.Exists(FileManager.GetChunkString(newchunkpos.x, newchunkpos.z)))
                                {
                                    try
                                    {
                                        _LoadedChunks.Add(new Chunk(newchunkpos.x, newchunkpos.z, Serializer.Deseralize_From_File<int[,,]>(FileManager.GetChunkString(newchunkpos.x, newchunkpos.z)),this));
                                    }
                                    catch (System.Exception E)
                                    {
                                        Debug.Log(E.ToString());
                                    }
                                }
                                else
                                {
                                    _LoadedChunks.Add(new Chunk(newchunkpos.x, newchunkpos.z, this));
                                    //Debug.Log("Cant find saves: " + "Data/C_" + x + "_" + z + ".chk");
                                }   
                            }
                        }
                        foreach (Chunk c in _LoadedChunks)
                        {
                            c.Start();
                        }
                    }
                    if (GameManager.PlayerLoaded())
                    {
                        PlayerPos = new Int3(GameManager.instance.playerpos);
                    }
                    foreach(Chunk c in _LoadedChunks)
                    {
                        if(Vector2.Distance(new Vector2(c.PosX*Chunk.ChunkWidth, c.PosZ * Chunk.ChunkWidth), new Vector2(PlayerPos.x,PlayerPos.z)) > ((RenderDistanceInChunks*2) * Chunk.ChunkWidth))
                        {
                            c.Degenerate();
                        }
                    }
                    for (int x = -RenderDistanceInChunks; x < RenderDistanceInChunks; x++)
                    {
                        for (int z = -RenderDistanceInChunks; z < RenderDistanceInChunks; z++)
                        {
                            Int3 newchunkpos = new Int3(PlayerPos.x, PlayerPos.y, PlayerPos.z);
                            newchunkpos.AddPos(new Int3(x * Chunk.ChunkWidth, 0, z * Chunk.ChunkWidth));
                            newchunkpos.ToChunkCoordinates();
                            if (!ChunkExists(newchunkpos.x, newchunkpos.z))
                            {
                                if (System.IO.File.Exists(FileManager.GetChunkString(newchunkpos.x, newchunkpos.z)))
                                {
                                    try
                                    {
                                        Chunk c = new Chunk(newchunkpos.x, newchunkpos.z, Serializer.Deseralize_From_File<int[,,]>(FileManager.GetChunkString(newchunkpos.x, newchunkpos.z)), this);
                                        c.Start();
                                        _LoadedChunks.Add(c);
                                    }
                                    catch (System.Exception E)
                                    {
                                        Debug.Log(E.ToString());
                                    }
                                }
                                else
                                {
                                    Chunk c = new Chunk(newchunkpos.x, newchunkpos.z, this);
                                    c.Start();
                                    _LoadedChunks.Add(c);
                                    //Debug.Log("Cant find saves: " + "Data/C_" + x + "_" + z + ".chk");
                                }
                            }
                        }
                    }
                            foreach (Chunk c in new List<Chunk>(_LoadedChunks))
                    {
                        c.Update();
                    }
                }
                catch(System.Exception E)
                {
                    UnityEngine.Debug.Log(E.StackTrace.ToString() + ":" + E.Data    .ToString());
                    Logger.Log(E);
                }
            }
            Logger.Log("World thread succesfully stopped");
            Logger.MainLog.Update(); //Rerun last log
        });
        WorldThread.Start();
    }

    internal void RemoveChunk(Chunk chunk)
    {
        _LoadedChunks.Remove(chunk);
    }

    public bool ChunkExists(int posx, int posz)
    {
        foreach (Chunk c in new List<Chunk>(_LoadedChunks))
        {
            if (c.PosX.Equals(posx) && c.PosZ.Equals(posz))
            {
                return true;
            }
        }
        return false;
    }

    public Chunk GetChunk(int posx, int posz)
    {
        foreach (Chunk c in new List<Chunk>(_LoadedChunks))
        {
            if(c.PosX.Equals(posx) && c.PosZ.Equals(posz))
            {
                return c;
            }
        }
        return new ErroredChunk(0,0, this);
    }

    public void Update()
    {
        List<Chunk> c = new List<Chunk>(_LoadedChunks);

        for (int i = c.Count-1;i>=0; i--)
        {
            c[i].OnUnityUpdate();
        }

        /*
        foreach (Chunk c in _LoadedChunks)
        {
            c.OnUnityUpdate();
        }*/
    }
}
