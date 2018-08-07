using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer {

    public static bool Check_Gen_Folder(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }
        else
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (System.Exception E)
            {
                Logger.MainLog.log(E.StackTrace.ToString());
            }
        }
        return false;
    }
    public static void Serialize_ToFile<T>(string path, string Filename, string extension, T _Data) where T : class
    {
        if (Check_Gen_Folder(path))
        {
            try
            {
                using (Stream s = File.OpenWrite(string.Format("{0}{1}.{2}", path, Filename, extension)))
                {
                    BinaryFormatter f = new BinaryFormatter();
                    f.Serialize(s, _Data);
                }
            }
            catch (System.Exception E)
            {
                Logger.MainLog.log(E.StackTrace.ToString());
            }
        }
        else
        {
            throw new System.Exception("Cant get correct director");
        }
    }

    public static void Serialize_ToFileFullPath<T>(string path, T _Data) where T : class
    {
        try
        {
            using (Stream s = File.OpenWrite(path))
            {
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(s, _Data);
            }
        }
        catch (System.Exception E)
        {
            Logger.MainLog.log(E.StackTrace.ToString());
        }
    }

    public static T Deseralize_From_File<T>(string path) where T: class
    {
        if(File.Exists(path))
        {
            try
            {
                using (Stream s = File.OpenRead(path))
                {
                    BinaryFormatter f = new BinaryFormatter();
                    return f.Deserialize(s) as T;
                }
            }
            catch(System.Exception E)
            {
                Logger.MainLog.log(E.StackTrace.ToString());
            }
        }
        else
        {
            throw new System.Exception("File cannot be found");
        }
        return null;
    }
}
