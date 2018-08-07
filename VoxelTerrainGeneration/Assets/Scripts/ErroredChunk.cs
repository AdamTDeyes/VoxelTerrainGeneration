using System;

public class ErroredChunk : Chunk
{
    public ErroredChunk(int px, int pz, World world) : base(px, pz, world)
    {

    }

    public override void OnUnityUpdate()
    {
        throw new Exception("Tried to use onunitupdate in erroredchunk class");
    }

    public override void Start()
    {
        throw new Exception("Tried to use start in erroredchunk class");
    }

    public override void Update()
    {
        throw new Exception("Tried to use update in erroredchunk class");
    }
}
