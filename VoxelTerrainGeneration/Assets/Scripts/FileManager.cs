
public class FileManager {

    public static readonly string ChunkSaveDirectory = "Data/World/DevWorld/Chunk/";
    public static void RegisterFile()
    {
        Serializer.Check_Gen_Folder(ChunkSaveDirectory);
    }

    public static string GetChunkString(int x, int z)
    {
        return string.Format("{0}C{1}_{2}.CHK", ChunkSaveDirectory, x, z);
    }
}
