using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class TextureGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public Texture2D tex;

    public static Texture2D GetTexture(List<List<float>> heightMap)
    {
        int width = heightMap.Count - 1;
        int height = heightMap[0].Count - 1;
        var texture = new Texture2D(width, height);
        var pixels = new Color[width * height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                float value = heightMap[x][y];

                if (value != 1.45f)
                    pixels[x + y * width] = Color.black;
                else
                    pixels[x + y * width] = Color.white;



                //pixels[x + y * width] = Color.Lerp(Color.black, Color.white, value);
            }
        }

        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();


        SaveToPng(texture);

        return texture;
    }
    public static void SaveToPng(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();


        var path = EditorUtility.SaveFilePanel(
                    "Save texture as PNG",
                    "",
                    "Image.png",
                    "png");

        File.WriteAllBytes(path, bytes);

    }


    public static void UpdateImage(List<List<float>> heightMap)
    {

        RawImage image = GameObject.Find("RawImage").GetComponent<RawImage>();

        int width = heightMap.Count - 1;

        int height = heightMap[0].Count - 1;
        var texture = new Texture2D(width, 200);
        var pixels = new Color[width * 200];


        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < 200; y++)
            {

                if (y > 200 || y > heightMap[0].Count - 1) break;

                //float value = heightMap[width - x][height - y];
                float value = heightMap[width - x][height - y];
                float nowWidth = width - x;
                float nowHeight = height - y;

                if (value == 1.45f)
                    value = 0;
                else
                {
                    //value = ((1.45f - value) * 1000) - 41.979551154f;
                    value = (((1.45f - value) * 1000) - 21f) / 0.003f / 10000;
                    //Debug.Log("-----------------------------------------------");

                    //Debug.Log("������>  "+ nowWidth + " ��������>  "+ nowHeight);
                    ////Debug.Log("��������>  " + value);
                    ////Debug.Log("������>  " + (1.45f - value));
                    //Debug.Log("����>  " + value);

                    //Debug.Log("-----------------------------------------------");

                }

                //value = (((1.45f - value) * 1000000) - 41960.36f+34.09461f)/100;
                //value = ((1.45f - value) * 1000);
                //else
                //    pixels[x + y * width] = Color.white;
                //if (value != 0)
                //Debug.Log(value);
                pixels[x + y * width] = new Color(value, value, value);
            }
        }


       

        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        image.rectTransform.sizeDelta = new Vector2(width, 200);
        image.texture = texture;

    }
}