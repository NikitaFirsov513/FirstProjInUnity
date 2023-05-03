using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
//using System.Diagnostics;
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

        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();




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

        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Debug.Log("RunTime>" + elapsedTime);

        Debug.Log("COUNT>" + count);


    }
    public static string FindNextPix(List<List<float>> heightMap, int startX, int startY, string findPix, Color[] pixels)
    {
        int nowX = startX, nowY = startY;
        int prevX = startX, prevY = startY;
        int minX = startX, maxX = startX;
        int minY = startY, maxY = startY;

        int width = heightMap.Count - 1;
        int countLoop = 0;
        int lastVector = 4;

        var mas = new List<List<int>>();
        mas.Add(new List<int>());
        mas[0].Add(1);

        bool isFind;
        bool isConvFind;

        string localCoord = "";
        Debug.Log("**************************************");


        do
        {
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

                if (isConvFind)
                {
                    switch (i)
                    {
                        case 1:
                            if (heightMap[nowX - 1][nowY] != 1.45f)
                            {
                                isFind = true;
                                nowX -= 1;
                                nowY += 0;
                            }
                            break;

                        case 2:
                            if (heightMap[nowX - 1][nowY + 1] != 1.45f)
                            {
                                isFind = true;
                                nowX -= 1;
                                nowY += 1;
                            }
                            break;
                        case 3:
                            if (heightMap[nowX][nowY + 1] != 1.45f)
                            {
                                isFind = true;
                                nowX += 0;
                                nowY += 1;
                            }
                            break;
                        case 4:
                            if (heightMap[nowX + 1][nowY + 1] != 1.45f)
                            {
                                isFind = true;
                                nowX += 1;
                                nowY += 1;
                            }
                            break;
                        case 5:
                            if (heightMap[nowX + 1][nowY] != 1.45f)
                            {
                                isFind = true;
                                nowX += 1;
                                nowY -= 0;
                            }
                            break;
                        case 6:
                            if (heightMap[nowX + 1][nowY - 1] != 1.45f)
                            {
                                isFind = true;
                                nowX += 1;
                                nowY -= 1;
                            }
                            break;
                        case 7:
                            if (heightMap[nowX][nowY - 1] != 1.45f)
                            {
                                isFind = true;
                                nowX -= 0;
                                nowY -= 1;
                            }
                            break;
                        case 8:
                            if (heightMap[nowX - 1][nowY - 1] != 1.45f)
                            {
                                isFind = true;
                                nowX -= 1;
                                nowY -= 1;
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (!isConvFind)
                {
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
                }







                if (isFind)
                {
                    lastVector = i;
                    localCoord += nowX + ":" + nowY + " | ";
                    //проверка мин/макс
                    


                    if(minX>nowX)
                        minX = nowX;
                    if (maxX < nowX)
                        maxX = nowX;

                    if (minY > nowY)
                        minY = nowY;
                    if (maxY < nowY)
                        maxY = nowY;


                    Debug.Log("||||||||||||||");
                    Debug.Log("prevX>" + prevX);
                    Debug.Log("nowX>" + nowX);
                    Debug.Log("prevY>" + prevY);
                    Debug.Log("minX>" + minX);
                    Debug.Log("maxX>" + maxX);


                    if (prevX - nowX > 0 && prevY - nowY < 0) {

                        //Ћево¬верх
                       


                        Debug.Log("Ћево¬верх");
                        //если за граници массива
                        //  -добавл€ем новый массив с 1цой в нужной точке
                        //  -добавл€ем к сущ-м массивам "0" слева

                        //если не выходит за граници массива
                        //  -добавл€ем новый массив с 1цой в нужной точке

                        if (mas[0].Count <= maxX - minX)
                        {
                            Debug.Log("Ћево¬верх¬ыход«а√раници ƒлинна>" + (maxX - minX+1));

                            
                            AddToMass(mas, -1);
                            mas.Insert(0, CreateMass(maxX - minX + 1, 0));
                        }
                        if (mas[0].Count > maxX - minX){

                            //не выходит за границы 

                        }

                    }
                    if (prevX - nowX > 0 && prevY - nowY > 0)
                    {

                        //Ћево¬низ

                        Debug.Log("Ћево¬низ");
                        int a = maxY - nowY;
                        int b = nowX - minX;

                        mas[a][b] = 1;

                        //добавить услови€ с/без переполнеи€ 
                        //пока только Ѕ≈« переполнени€
                    }
                    if (prevX - nowX < 0 && prevY - nowY < 0)
                    {

                        //ѕраво¬верх
                        Debug.Log("ѕраво¬верх");


                        if (mas[0].Count < maxX - minX)
                        {
                            //переполн€ет
                            AddToMass(mas, 1);

                            mas.Insert(0, CreateMass(maxX - minX + 1, nowX - minX));


                        }
                        if (mas[0].Count >= maxX - minX)
                        {
                            //не переполн€ет

                            mas.Insert(0, CreateMass(maxX - minX + 1, nowX - minX));

                        }
                      

                    }
                    if (prevX - nowX < 0 && prevY - nowY > 0)
                    {

                        //ѕраво¬низ
                        Debug.Log("ѕраво¬низ");
                        if (mas[0].Count <= maxX - minX)
                        {
                            //переполн€ет
                            AddToMass(mas, 1);

                            int a = maxY - nowY-1;
                            int b = nowX - minX - 1;

                            mas[a][b] = 1;


                        }
                        if (mas[0].Count > maxX - minX)
                        {
                            //не переполн€ет

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }

                    }
                    if (prevX - nowX < 0 && prevY - nowY == 0)
                    {

                        //ѕраво
                        Debug.Log("ѕраво");

                        if (mas[0].Count <= maxX - minX) {
                            //¬ыходит за границы
                            AddToMass(mas, 1);
                            
                            int a = maxY - nowY;
                            int b = nowX - minX - 1;

                            mas[a][b] = 1;
                        }
                        if (mas[0].Count > maxX - minX)
                        {

                            //не выходит за границы
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }

                    }
                    if (prevX - nowX > 0 && prevY - nowY == 0)
                    {

                        //Ћево
                        Debug.Log("Ћево");
                        int a = maxY - nowY;
                        int b = nowX - minX;

                        mas[a][b] = 1;

                        //добавить услови€ с/без переполнеи€ 
                        //пока только Ѕ≈« переполнени€
                    }
                    if (prevX - nowX == 0 && prevY - nowY < 0)
                    {

                        //¬верх
                        Debug.Log("¬верх");
                        mas.Insert(0, CreateMass(maxX - minX+1 , nowX - minX));
                        //добавить услови€ с/без переполнеи€ 
                        //пока только с переполнением

                    }
                    if (prevX - nowX == 0 && prevY - nowY > 0)
                    {

                        //¬низ
                        Debug.Log("¬низ");
                        int a = maxY - nowY;
                        int b = nowX - minX;

                        mas[a][b] = 1;
                        //добавить услови€ с/без переполнеи€ 
                        //пока только Ѕ≈« переполнени€

                    }

                    prevX = nowX; 
                    prevY=nowY;


                    //если prevX-nowX!=0, то было смещение по x
                    //если prevY-nowY!=0, то было смещение по y


                    //если было смещение по y:
                    //  -добавл€ем новый массив со значени€ми == 0, кроме того, куда переместились. 
                    //  -если prevX>nowX, то у других массивов вставл€ем в начало 0
                    //  -если prevX<nowX, то у других массивов вставл€ем в конец 
                    //  -если prevX<nowX, то у других массивов вставл€ем в конец 0
                    //иначе было смещение по x, то добавл€ем новые значени€ в массивы
                    //  -если prevX > nowX, то у других массивов вставл€ем в начало 0
                    //  -если prevXЅnowX, то у других массивов вставл€ем в конец 0

                    break;
                }
            }

            if (countLoop > 500)
            {
                pixels[nowX + nowY * width] = new Color(0, 0, 1f);
                break;
            }


        } while (nowX != startX || nowY != startY);

        Debug.Log("---------MAS---------");
        for (int i = 0; i < mas.Count; i++) {

            string str = "";
            for (int j = 0; j < mas[i].Count; j++)
            {
                str += mas[i][j].ToString() + " ";
                
            }
            Debug.Log(str);
            }

            return findPix + localCoord;




    }


    //метод "заполнить значени€ми"
    //вход: длинна массива, индекс 1ци
    public static List<int> CreateMass(int length, int index = -1)
    {


        var newList = new List<int>();


        for (int i = 0; i < length; i++)
        {

            if (i == index)
                newList.Add(1);
            else
                newList.Add(0);

        }

        return newList;

    }
    //метод "добавить значени€ в массив"
    //вход: массив, индекс куда вставл€ть "-1" влево, "1" вправо
    public static bool AddToMass(List<List<int>> list, int index = 0)
    {

        if (index == -1)
        {
            foreach (var mas in list)
            {
                mas.Insert(0, 0);
            }
        }

        if (index == 1)
        {
            foreach (var mas in list)
            {
                mas.Add(0);
            }
        }

        return true;
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
