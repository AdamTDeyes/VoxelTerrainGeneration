using UnityEngine;
using System.IO;

public class TextureAtlas {

    public static readonly TextureAtlas _Instance = new TextureAtlas();
    public static Texture2D _ATLAS { get; private set; }

    public void CreateAtlas()
    {
        string[] _Images = Directory.GetFiles("Assets/Textures/blocks/");
        foreach(string s in _Images)
        {
            Debug.Log("Image found for atlas" + s);

        }
        int PixelWidth = 16;
        int PixelHeight = 16;
        int AtlasWidth = Mathf.CeilToInt((Mathf.Sqrt(_Images.Length)+1) * PixelWidth);
        int AtlasHeight = Mathf.CeilToInt((Mathf.Sqrt(_Images.Length)+1) * PixelHeight);
        Texture2D Atlas = new Texture2D(AtlasWidth, AtlasHeight);
        int count = 0;
        for(int x = 0; x < AtlasWidth / PixelWidth; x++)
        {
            for (int y = 0; y < AtlasWidth / PixelWidth; y++)
            {
                if (count >= _Images.Length) goto end; 
                Texture2D temp = new Texture2D(0, 0);
                temp.LoadImage(File.ReadAllBytes(_Images[count]));
                Atlas.SetPixels(x * PixelWidth, y * PixelHeight, PixelWidth, PixelHeight, temp.GetPixels());

                float startx = x * PixelWidth;
                float starty = y * PixelHeight;
                float perpixelratiox = 1.0f / (float)Atlas.width;
                float perpixelratioy = 1.0f / (float)Atlas.height;
                startx *= perpixelratiox;
                starty *= perpixelratioy;
                float endx = startx + (perpixelratiox * PixelWidth);
                float endy = starty + (perpixelratioy * PixelHeight);

                UVMap m = new UVMap(_Images[count], new Vector2[] 
                {
                    new Vector2(startx,starty),
                    new Vector2(startx,endy),
                    new Vector2(endx,starty),
                    new Vector2(endx,endy)
                });
                m.Register();

                count++;
            }
        }
        end:;
        _ATLAS = Atlas;
        File.WriteAllBytes("Atlas.png",Atlas.EncodeToPNG());
    }
}
