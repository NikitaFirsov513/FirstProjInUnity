using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
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

                if (value == 1.45f)
                    value = 0;
                else
                    value = (((1.45f - value) * 1000) - 20f) / 0.003f / 10000;

                pixels[x + y * width] = new Color(value, value, value);
            }
        }
        FindEggCout(heightMap, pixels);

        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();


        //start fint egg count
        SaveToPng(texture);

        return texture;
    }
    public static void FindEggCout(List<List<float>> heightMap, Color[] pixels)
    {

        //перебор масива
        //если левый верхний, то идем по контуру
        //что обходим красим в красное
        //если дошли до начальной точки, то делаем заново
        int width = heightMap.Count - 1;
        int height = heightMap[0].Count - 1;
        int count = 0;
        string findPix = "";


        for (int y = 1; y < height; y++)
        {
            for (int x = 1; x < width; x++)
            {


                if (heightMap[x][y] == 1.45f)
                    continue;

                if (heightMap[x - 1][y] == 1.45f &&
                    heightMap[x - 1][y - 1] == 1.45f &&
                    heightMap[x][y - 1] == 1.45f &&
                    heightMap[x + 1][y - 1] == 1.45f &&
                    heightMap[x + 2][y - 1] == 1.45f)
                {

                    count++;
                    pixels[x + y * width] = new Color(1f, 1f, 0);

                    if (!findPix.Contains(x + ":" + y))
                        findPix = FindNextPix(heightMap, x, y, findPix, pixels);

                }
            }
        }
        Debug.Log("COUNT>" + count);


    }
    public static string FindNextPix(List<List<float>> heightMap, int startX, int startY, string findPix, Color[] pixels)
    {
        int nowX = startX, nowY = startY;
        int width = heightMap.Count - 1;
        bool isFind;
        bool isConvFind;
        int countLoop = 0;
        //var localDict = new Dictionary<string, bool>();
        string localCoord = "";
        int lastVector = 4;
        do
        {
            Debug.Log("********************************");
            Debug.Log("startX>" + startX);
            Debug.Log("startY>" + startY);
            countLoop++;
            if (localCoord == "")
                pixels[nowX + nowY * width] = new Color(0, 1f, 0);
            else
                pixels[nowX + nowY * width] = new Color(1f, 0, 0);
            isFind = false;
            isConvFind = false;
            

            for (int j = 1; j <= 9; j++)
            {

                int i = lastVector + 4 + j;


                if (i > 8)
                    i -= 8;
                if (i > 8)
                    i -= 8;
                //переделать колесо векторов движений
                if (isConvFind)
                {
                    Debug.Log("--------------------------------------------");
                    Debug.Log("isConvFind");
                    Debug.Log("CASE>\t" + i);
                    switch (i)
                    {
                        case 1:
                            if (heightMap[nowX - 1][nowY] != 1.45f)
                            {

                                //if (localCoord.Contains((nowX - 1) + ":" + (nowY)))
                                //{
                                //    isConvFind = false;
                                //    break;
                                //}
                                
                                isFind = true;
                                nowX -= 1;
                                nowY += 0;
                                
                            }
                            break;

                        case 2:
                            if (heightMap[nowX - 1][nowY + 1] != 1.45f)
                            {
                                //if (localCoord.Contains((nowX-1) + ":" + (nowY + 1))) {
                                //    isConvFind = false;
                                //    break;
                                //}
                                
                                isFind = true;
                                nowX -= 1;
                                nowY += 1;
                               


                            }
                            break;
                        case 3:
                            if (heightMap[nowX][nowY + 1] != 1.45f)
                            {
                                //if (localCoord.Contains((nowX ) + ":" + (nowY + 1)))
                                //{
                                //    isConvFind = false;
                                //    break;
                                //}
                               
                                isFind = true;
                                nowX += 0;
                                nowY += 1;
                               
                            }
                            break;
                        case 4:
                            if (heightMap[nowX + 1][nowY + 1] != 1.45f)
                            {
                                //if (localCoord.Contains((nowX + 1) + ":" + (nowY+1)))
                                //{
                                //    isConvFind = false;
                                //    break;
                                //}
                               
                                isFind = true;
                                nowX += 1;
                                nowY += 1;
                                
                            }
                            break;
                        case 5:
                            if (heightMap[nowX + 1][nowY] != 1.45f)
                            {
                                //if (localCoord.Contains((nowX + 1) + ":" + (nowY )))
                                //{
                                //    isConvFind = false;
                                //    break;
                                //}
                                
                                isFind = true;
                                nowX += 1;
                                nowY -= 0;
                                
                            }
                            break;
                        case 6:
                            if (heightMap[nowX + 1][nowY - 1] != 1.45f)
                            {
                                //    if (localCoord.Contains((nowX +1) + ":" + (nowY - 1)))
                                //    {
                                //        isConvFind = false;
                                //        break;
                                //    }
                               
                                isFind = true;
                                nowX += 1;
                                nowY -= 1;
                                
                            }
                            break;
                        case 7:
                            if (heightMap[nowX][nowY - 1] != 1.45f)
                            {
                                //if (localCoord.Contains((nowX) + ":" + (nowY - 1)))
                                //{
                                //    isConvFind = false;
                                //    break;
                                //}
                               
                                isFind = true;
                                nowX -= 0;
                                nowY -= 1;
                               
                            }
                            break;
                        case 8:
                            if (heightMap[nowX - 1][nowY - 1] != 1.45f)
                            {
                                //if (localCoord.Contains((nowX - 1) + ":" + (nowY -1)))
                                //{
                                //    isConvFind = false;
                                //    break;
                                //}
                                
                                isFind = true;
                                nowX -= 1;
                                nowY -= 1;
                                
                            }
                            break;
                        default:
                            break;
                    }
                    Debug.Log("nowX>" + nowX);
                    Debug.Log("nowY>" + nowY);
                }

                if (!isConvFind)
                {
                    Debug.Log("--------------------------------------------");
                    Debug.Log("!!!isConvFind");
                    Debug.Log("CASE>\t" + i);
                    switch (i)
                    {
                        case 1:
                            if (heightMap[nowX - 1][nowY] == 1.45f)
                                isConvFind = true;
                            break;
                        case 2:
                            if (heightMap[nowX - 1][nowY + 1] == 1.45f)
                                isConvFind = true;
                            break;
                        case 3:
                            if (heightMap[nowX][nowY + 1] == 1.45f)
                                isConvFind = true;
                            break;
                        case 4:
                            if (heightMap[nowX + 1][nowY + 1] == 1.45f)
                                isConvFind = true;
                            break;
                        case 5:
                            if (heightMap[nowX + 1][nowY] == 1.45f)
                                isConvFind = true;
                            break;
                        case 6:
                            if (heightMap[nowX + 1][nowY - 1] == 1.45f)
                                isConvFind = true;
                            break;
                        case 7:
                            if (heightMap[nowX][nowY - 1] == 1.45f)
                                isConvFind = true;
                            break;
                        case 8:
                            if (heightMap[nowX - 1][nowY - 1] == 1.45f)
                                isConvFind = true;
                            break;
                        default:
                            break;
                    }
                    Debug.Log("nowX>" + nowX);
                    Debug.Log("nowY>" + nowY);
                    Debug.Log("isConvFind>" + isConvFind);

                }


                //Debug.Log("-----------------");
                //Debug.Log("startX>" + startX + "\tstartY>" + startY);
                //Debug.Log("nowX>" + nowX + "\tnowY>" + nowY);
                //Debug.Log("i>" + i);
                //Debug.Log("isConvFind>" + isConvFind);
                //Debug.Log("isFind>" + isFind);
                //Debug.Log("-----------------");


                Debug.Log("countLoop>" + countLoop);


                if (isFind)
                {
                    lastVector = i;
                    localCoord += nowX + ":" + nowY + " | ";
                    //localDict.Add()
                    Debug.Log("%%%%%%%%%%%");
                    Debug.Log("FIIIIIIND");
                    Debug.Log("startX>" + startX + "\tstartY>" + startY);
                    Debug.Log("nowX>" + nowX + "\tnowY>" + nowY);
                    Debug.Log("%%%%%%%%%%%");

                    break;
                }
            }

            if (countLoop > 500)
            {
                pixels[nowX + nowY * width] = new Color(0, 0, 1f);

                Debug.Log("%%%%%%%%%%%");
                Debug.Log("TOOOOOMANYYYYY-------------------------------------------------------------");
                Debug.Log("startX>" + startX + "\tstartY>" + startY);
                Debug.Log("nowX>" + nowX + "\tnowY>" + nowY);
                Debug.Log("%%%%%%%%%%%");
                break;
            }


        } while (nowX != startX || nowY != startY);
        Debug.Log("ѕ–ќўј… я»„ ќ");
        return findPix + localCoord;




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

                float value = heightMap[width - x][height - y];

                if (value == 1.45f)
                    value = 0;
                else
                    value = (((1.45f - value) * 1000) - 21f) / 0.003f / 10000;


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
