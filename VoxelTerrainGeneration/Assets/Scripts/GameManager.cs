using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    private List<Delegate> _Delegates = new List<Delegate>();

    public Canvas canvas;
    //public GameObject Player;
    private Transform Player;
    public Vector3 playerpos;

    public float dx = 1;
    public float dz = 1;
    public float my = 1;
    public float cutoff = 1;
    public float mul = 1;

    public static float sdx = 50;
    public static float sdz = 50;
    public static float smy = 0.25f;
    public static float scutoff = 1.8f;
    public static float smul = 1;

    public static GameManager instance;
    private bool IsPlayerLoaded = false;

    private MainLoopable main;

    public void RegisterDelegate(Delegate d)
    {
        _Delegates.Add(d);
    }
    public void StartPlayer(Vector3 Pos)
    {
        canvas.gameObject.SetActive(false);

        GameObject t = Transform.Instantiate(Resources.Load<GameObject>("Player"), Pos, Quaternion.identity) as GameObject;
        t.transform.position = Pos;
        Player = t.transform;
        //Player = GameObject.FindWithTag("Player");
        //Player.gameObject.transform = t.transform;
    }
	// Use this for initialization
	void Start ()
    {
        /* test stuff
        object[] o = Serializer.Deseralize_From_File<object[]>("Data/Saves/World/Chunk/Chunk1.chk");
        Serializer.Serialize_Tofile<object[]>("Data/Saves/World/Chunk/", "Chunk1", ".chk", new object[] {"12","13","14"});
        */
        FileManager.RegisterFile();
        instance = this;
        TextureAtlas._Instance.CreateAtlas();
        MainLoopable.Instantiate();
        main = MainLoopable.GetInstance();
        main.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Player != null)
        {
            playerpos = Player.transform.position;
            IsPlayerLoaded = true;
        }
        /*
        sdx = dx;
        sdz = dz;
        smy = my;
        scutoff = cutoff;
        smul = mul;*/

        main.Update();
        foreach(Delegate d in new List<Delegate>(_Delegates))
        {
            d.DynamicInvoke();
            _Delegates.Remove(d);
        }
	}

    void OnApplicationQuit()
    {
        main.OnApplicationQuit();
    }

    internal static bool PlayerLoaded()
    {
        return instance.IsPlayerLoaded;
    }
}
