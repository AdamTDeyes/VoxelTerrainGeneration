using System.Collections.Generic;
using UnityEngine;

public class BlockRegistry {

    private static readonly bool DebugMode = false;
    private static List<Block> _REGISTERBLOCKS = new List<Block>();

    public static void RegisterBlock(Block b)
    {
        _REGISTERBLOCKS.Add(b);
    }

    public static void RegisterBlocks()
    {
        if (DebugMode)
        {
            int i = 0;
            List<string> _names = new List<string>();
            foreach (Block b in _REGISTERBLOCKS)
            {
                _names.Add(string.Format("CurrentID: {0}, BlockName: {1}, BlockID: {2}", i, b.GetBlockName(), b.GetID()));
                i++;
            }
            System.IO.File.WriteAllLines("BlockRegistry.txt", _names.ToArray());
        }
    }

    internal static Block GetBlockFromID(int v)
    {
        try
        {
            return _REGISTERBLOCKS[v];
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
        return null;
    }
}
