using System.Collections.Generic;

public class MainLoopable : ILoopable {

    private static MainLoopable _Instance;
    private List<ILoopable> _RegisteredLoopes = new List<ILoopable>();

    public static void Instantiate()
    {
        _Instance = new MainLoopable();
        //register
        Logger.Instantiate();
        World.Instantiate();
        //Debug.Log("MainLoopable");
        Block.Air.GetBlockName();
        BlockRegistry.RegisterBlocks();
    }

    public void OnApplicationQuit()
    {
        foreach (ILoopable L in _RegisteredLoopes)
        {
            L.OnApplicationQuit();
        }
    }

    public void RegisterLoopes(ILoopable L)
    {
        _RegisteredLoopes.Add(L);
    }

    public void DeRegisterLoopes(ILoopable i)
    {
        _RegisteredLoopes.Remove(i);
    }

    public static MainLoopable GetInstance()
    {
        return _Instance;
    }

    // Use this for initialization
    public void Start()
    {
        //Debug.Log("MainLoopable Start");
        foreach (ILoopable L in _RegisteredLoopes)
        {
            L.Start();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        //Debug.Log("MainLoopable Update");
        //Logger.Log("Updating");
        foreach (ILoopable L in _RegisteredLoopes)
        {
            L.Update();
        }
    }
}
