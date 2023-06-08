using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ImageProcessing : MonoBehaviour
{
    // Start is called before the first frame update
    public static void Start(List<List<float>> heightMap)
    {

        int width = heightMap.Count - 1;
        int height = heightMap[0].Count - 1;

        Color[] initPixels = GenerateDefaultImage(heightMap, width, height);

        //копирование для обработки
        Color[] onePixels = new Color[width * height];
        initPixels.CopyTo(onePixels, 0);
        float maxValue = GlobalVar.getMinDistance();

        onePixels = FindAllContour(heightMap, onePixels, maxValue, 1);

        SaveToPng(onePixels, "onePixels", width, height);



        Color[] twoPixels = new Color[width * height];
        initPixels.CopyTo(twoPixels, 0);

        twoPixels = FindAllContour(heightMap, twoPixels, maxValue, 2);

        SaveToPng(twoPixels, "twoPixels", width, height);


    }


    public static Color[] FindAllContour(List<List<float>> heightMap, Color[] pixels, float maxValue, int colOtsu = 0)
    {

        int width = heightMap.Count - 1;
        int height = heightMap[0].Count - 1;
        string findPix = "";

        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();



        for (int y = 1; y < height; y++)
        {
            for (int x = 1; x < width; x++)
            {



                if (heightMap[x][y] > maxValue)
                    continue;

                if (heightMap[x - 1][y] > maxValue &&
                    heightMap[x - 1][y - 1] > maxValue &&
                    heightMap[x][y - 1] > maxValue &&
                    heightMap[x + 1][y - 1] > maxValue &&
                    heightMap[x + 2][y - 1] > maxValue)
                {

                    //pixels[x + y * width] = new Color(1f, 1f, 0);

                    if (!findPix.Contains(x + ":" + y))
                    {
                        //доделать
                        findPix = FindNextPix(heightMap, pixels, x, y, findPix,colOtsu);

                    }

                }
            }
        }











        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Debug.Log("RunTime>" + elapsedTime);





        return pixels;
    }
    //Генерация изначального изображения

    public static string FindNextPix(List<List<float>> heightMap, Color[] pixels, int startX, int startY, string findPix, int colOtsu = 0)
    {


        int nowX = startX, nowY = startY;
        int prevX = startX, prevY = startY;
        int minX = startX, maxX = startX;
        int minY = startY, maxY = startY;

        int width = heightMap.Count - 1;
        int countLoop = 0;
        int lastVector = 4;

        float minDistance = GlobalVar.getMinDistance();

        var mas = new List<List<int>>();
        mas.Add(new List<int>());
        mas[0].Add(1);

        bool isFind;
        bool isConvFind;

        string localCoord = "";
        Debug.Log("**************************************");
        Debug.Log("startX>" + startX);
        Debug.Log("startY>" + startY);



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
                            if (heightMap[nowX - 1][nowY] < minDistance)
                            {
                                isFind = true;
                                nowX -= 1;
                                nowY += 0;
                            }
                            break;

                        case 2:
                            if (heightMap[nowX - 1][nowY + 1] < minDistance)
                            {
                                isFind = true;
                                nowX -= 1;
                                nowY += 1;
                            }
                            break;
                        case 3:
                            if (heightMap[nowX][nowY + 1] < minDistance)
                            {
                                isFind = true;
                                nowX += 0;
                                nowY += 1;
                            }
                            break;
                        case 4:
                            if (heightMap[nowX + 1][nowY + 1] < minDistance)
                            {
                                isFind = true;
                                nowX += 1;
                                nowY += 1;
                            }
                            break;
                        case 5:
                            if (heightMap[nowX + 1][nowY] < minDistance)
                            {
                                isFind = true;
                                nowX += 1;
                                nowY -= 0;
                            }
                            break;
                        case 6:
                            if (heightMap[nowX + 1][nowY - 1] < minDistance)
                            {
                                isFind = true;
                                nowX += 1;
                                nowY -= 1;
                            }
                            break;
                        case 7:
                            if (heightMap[nowX][nowY - 1] < minDistance)
                            {
                                isFind = true;
                                nowX -= 0;
                                nowY -= 1;
                            }
                            break;
                        case 8:
                            if (heightMap[nowX - 1][nowY - 1] < minDistance)
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
                            if (heightMap[nowX - 1][nowY] > minDistance)
                                isConvFind = true;
                            break;
                        case 2:
                            if (heightMap[nowX - 1][nowY + 1] > minDistance)
                                isConvFind = true;
                            break;
                        case 3:
                            if (heightMap[nowX][nowY + 1] > minDistance)
                                isConvFind = true;
                            break;
                        case 4:
                            if (heightMap[nowX + 1][nowY + 1] > minDistance)
                                isConvFind = true;
                            break;
                        case 5:
                            if (heightMap[nowX + 1][nowY] > minDistance)
                                isConvFind = true;
                            break;
                        case 6:
                            if (heightMap[nowX + 1][nowY - 1] > minDistance)
                                isConvFind = true;
                            break;
                        case 7:
                            if (heightMap[nowX][nowY - 1] > minDistance)
                                isConvFind = true;
                            break;
                        case 8:
                            if (heightMap[nowX - 1][nowY - 1] > minDistance)
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

                    if (minX > nowX)
                        minX = nowX;
                    if (maxX < nowX)
                        maxX = nowX;

                    if (minY > nowY)
                        minY = nowY;
                    if (maxY < nowY)
                        maxY = nowY;





                    if (prevX - nowX > 0 && prevY - nowY < 0)
                    {

                        //ЛевоВверх



                        //Debug.Log("ЛевоВверх");
                        //если за граници массива
                        //  -добавляем новый массив с 1цой в нужной точке
                        //  -добавляем к сущ-м массивам "0" слева

                        //если не выходит за граници массива
                        //  -добавляем новый массив с 1цой в нужной точке

                        if (mas[0].Count < maxX - minX + 1 && mas.Count < maxY - minY + 1)
                        {
                            //Debug.Log("ЛевоВверхВыходЗаГраници Длинна>" + (maxX - minX + 1));


                            AddToMass(mas, -1);
                            mas.Insert(0, CreateMass(maxX - minX + 1, 0));
                        }
                        else if (mas.Count < maxY - minY + 1)
                        {
                            mas.Insert(0, CreateMass(maxX - minX + 1, nowX - minX));
                        }
                        if (mas[0].Count < maxX - minX + 1)
                        {
                            AddToMass(mas, -1);

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        else
                        {
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }


                    }
                    if (prevX - nowX > 0 && prevY - nowY > 0)
                    {

                        //ЛевоВниз

                        //Debug.Log("ЛевоВниз");



                        if (mas[0].Count < maxX - minX + 1)
                        {
                            AddToMass(mas, -1);
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        else
                        {
                            //не переполняет
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;

                        }

                        //добавить условия с/без переполнеия 
                        //пока только БЕЗ переполнения
                    }
                    if (prevX - nowX < 0 && prevY - nowY < 0)
                    {

                        //ПравоВверх
                        //Debug.Log("ПравоВверх");


                        if (mas[0].Count < maxX - minX + 1 && mas.Count < maxY - minY + 1)
                        {
                            //Debug.Log("ЛевоВверхВыходЗаГраници Длинна>" + (maxX - minX + 1));


                            AddToMass(mas, 1);
                            mas.Insert(0, CreateMass(maxX - minX + 1, nowX - minX));
                        }
                        else if (mas.Count < maxY - minY + 1)
                        {
                            mas.Insert(0, CreateMass(maxX - minX + 1, nowX - minX));
                        }
                        if (mas[0].Count < maxX - minX + 1)
                        {
                            AddToMass(mas, 1);

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        else
                        {
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }


                    }
                    if (prevX - nowX < 0 && prevY - nowY > 0)
                    {

                        //ПравоВниз
                        //Debug.Log("ПравоВниз");
                        if (mas[0].Count < maxX - minX + 1)
                        {
                            //переполняет
                            AddToMass(mas, 1);

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;

                        }
                        else
                        {
                            //не переполняет

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }

                    }
                    if (prevX - nowX < 0 && prevY - nowY == 0)
                    {

                        //Право
                        //Debug.Log("Право");

                        if (mas[0].Count < maxX - minX + 1)
                        {
                            //Выходит за границы
                            AddToMass(mas, 1);

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        else
                        {

                            //не выходит за границы
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }

                    }
                    if (prevX - nowX > 0 && prevY - nowY == 0)
                    {

                        //Лево
                        //Debug.Log("Лево");



                        if (mas[0].Count < maxX - minX + 1)
                        {
                            //Debug.Log("Лево С переполнением");

                            AddToMass(mas, -1);

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        else
                        {
                            //Debug.Log("Лево БЕЗ переполнения");

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }


                        //добавить условия с/без переполнеия 
                        //пока только БЕЗ переполнения
                    }
                    if (prevX - nowX == 0 && prevY - nowY < 0)
                    {

                        //Вверх

                        if (mas.Count < maxY - minY + 1)
                        {
                            //Debug.Log("Вверх С переполнением");
                            mas.Insert(0, CreateMass(maxX - minX + 1, nowX - minX));
                        }
                        else
                        {
                            //Debug.Log("Вверх БЕЗ переполнением");
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        //добавить условия с/без переполнеия 
                        //пока только с переполнением

                    }
                    if (prevX - nowX == 0 && prevY - nowY > 0)
                    {


                        if (mas.Count < maxY - minY + 1)
                        {
                            //Debug.Log("Вверх С переполнением");
                            //mas.add(CreateMass(maxX - minX, nowX - minX));
                            mas.Add(CreateMass(maxX - minX, nowX - minX));
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        else
                        {
                            //Debug.Log("Вверх БЕЗ переполнением");
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }

                        //Вниз
                        //Debug.Log("Вниз");
                        //int a = maxY - nowY;
                        ////int b = mas[0].Count - (maxX - nowX) - 1;
                        //int b = nowX - minX;

                        //mas[a][b] = 1;
                        //добавить условия с/без переполнеия 
                        //пока только БЕЗ переполнения

                    }


                    prevX = nowX;
                    prevY = nowY;


                    //если prevX-nowX!=0, то было смещение по x
                    //если prevY-nowY!=0, то было смещение по y


                    //если было смещение по y:
                    //  -добавляем новый массив со значениями == 0, кроме того, куда переместились. 
                    //  -если prevX>nowX, то у других массивов вставляем в начало 0
                    //  -если prevX<nowX, то у других массивов вставляем в конец 
                    //  -если prevX<nowX, то у других массивов вставляем в конец 0
                    //иначе было смещение по x, то добавляем новые значения в массивы
                    //  -если prevX > nowX, то у других массивов вставляем в начало 0
                    //  -если prevXБnowX, то у других массивов вставляем в конец 0

                    break;
                }
            }

            if (countLoop > 500)
            {
                pixels[nowX + nowY * width] = new Color(0, 0, 1f);
                break;
            }


        } while (nowX != startX || nowY != startY);

        Debug.Log(@"/--------MAS---------\");
        for (int i = 0; i <= mas.Count - 1; i++)
        {

            string str = "";
            for (int j = 0; j <= mas[0].Count - 1; j++)
            {
                str += mas[i][j].ToString() + " ";

            }
            Debug.Log(str);
        }


        Debug.Log(@"\--------MAS---------/");


        for (int i = 0; i < colOtsu; i++)
        {




            //Вызов метода Оцу
            MethodOtsu(minX, minY, mas, heightMap, pixels);
        }



        //int S = CalcSquare(mas);
        //Debug.Log("SQUARE>" + S);
        //Debug.Log("EGG>" + S / 18);
        //доделать скрипт распознования количества яиц
        //CalcEgg.addSum(S / 15);

        return findPix + localCoord;




    }
    struct Bright
    {

        public float val;
        public float col;
        public float Pk;


        public Bright(float val, float col, float Pk)
        {
            this.val = val;
            this.col = col;
            this.Pk = Pk;
        }
        public void setCol(float col)
        {
            this.col = col;

        }
        public float getCol()
        {
            return this.col;

        }
        public void setPk(float Pk)
        {
            this.Pk = Pk;

        }

        public string toString()
        {
            return "val>" + val + "\tcol>" + col + "\tPk>" + Pk;

        }
    }


    //изменить метод он не должен менять пиксели
    public static int MethodOtsu(int minX, int minY, List<List<int>> mas, List<List<float>> heightMap, Color[] testCenterPix)
    {
        List<List<float>> localMas = new List<List<float>>();

        int width = heightMap.Count - 1;
        int height = heightMap[0].Count - 1;

        var listBright = new List<Bright>();
        float colPix = (mas.Count * mas[0].Count);
        //string str = "";


        for (int i = mas.Count - 1; i >= 0; i--)
        {
            localMas.Add(new List<float>());

            for (int j = 0; j <= mas[i].Count - 1; j++)
            {
                float power = heightMap[minX + j][minY + i];

                power = (1.45f + GlobalVar.getNoise() - power) * 20;

                localMas[mas.Count - 1 - i].Add(power);

                listBright.Exists(x => x.val == power);
                if (listBright.Exists(x => x.val == power))
                {
                    var elem = listBright.Find(x => x.val == power);
                    elem.setCol(elem.getCol() + 1f);
                    //elem.setPk(elem.getCol() / colPix);

                }
                else
                {
                    listBright.Add(new Bright(power, 1f, 0f));
                    var elem = listBright.Find(x => x.val == power);
                    //elem.setPk(elem.getCol() / colPix);

                }



            }
        }


        listBright = listBright.OrderBy(o => o.val).ToList();
        float O2B = float.MinValue;
        int maxK = 0;
        float ipiG = 0;
        for (int i = 0; i <= listBright.Count - 1; i++)
        {
            float P1k = 0;
            float P2k = 0;

            float ip1i = 0;
            float ip2i = 0;

            ipiG += i * listBright[i].getCol() / colPix;

            for (int j = 0; j <= i; j++)
            {
                P1k += listBright[j].getCol() / colPix;
                ip1i += j * listBright[j].getCol() / colPix;
            }

            for (int k = listBright.Count - i; k <= listBright.Count - 1; k++)
            {
                P2k += listBright[k].getCol() / colPix;
                ip2i += k * listBright[k].getCol() / colPix;
            }




            float m1k = ip1i / P1k;
            float m2k = ip2i / P2k;

            float o2b = P1k * P2k * (m1k - m2k) * (m1k - m2k);

            Debug.Log("P1k>" + P1k);
            Debug.Log("P2k>" + P2k);

            Debug.Log("m1k>" + m1k);
            Debug.Log("m2k>" + m2k);

            Debug.Log("o2b>" + o2b);

            if (O2B < o2b)
            {
                O2B = o2b;
                maxK = i;
            }

        }

        Debug.Log("MAX O2B>" + O2B);
        Debug.Log("MAX K>" + maxK);
        Debug.Log("VAL>" + listBright[maxK].val);


        Debug.Log("ipiG>" + ipiG);


        for (int i = 0; i <= localMas.Count - 1; i++)
        {
            for (int j = 0; j <= localMas[i].Count - 1; j++)
            {
                float val = localMas[i][j];

                //if (val >= 0.75f)
                //    testCenterPix[(minX + j) + (minY + localMas.Count - 1 - i) * width] = new Color(1f, 1f, 1f);
                //else
                //    testCenterPix[(minX + j) + (minY + localMas.Count - 1 - i) * width] = new Color(0f, 0f, 0f);


                if (val >= listBright[maxK].val)
                    testCenterPix[(minX + j) + (minY + localMas.Count - 1 - i) * width] = new Color(val, val, val);
                else
                    testCenterPix[(minX + j) + (minY + localMas.Count - 1 - i) * width] = new Color(0f, 0f, 0f);
            }
        }

        return 0;
    }


    //метод "заполнить значениями"
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
    //метод "добавить значения в массив"
    //вход: массив, индекс куда вставлять "-1" влево, "1" вправо
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


    public static Color[] GenerateDefaultImage(List<List<float>> heightMap, int width, int height)
    {

        string name = "DefaultImage";

        //power = (1.45f + GlobalVar.getNoise() - power) * 20;


        float borderVal = GlobalVar.getBorderVal();

        Color[] pixels = new Color[width * height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                float value = heightMap[x][y];

                if (value >= borderVal)
                    value = 0;
                else
                    value = (1.45f + GlobalVar.getNoise() - value) * 20;
                //value = (((1.45f - value) * 1000) - 20f) / 0.003f / 10000;

                pixels[x + y * width] = new Color(value, value, value);
            }
        }

        SaveToPng(pixels, name, width, height);
        return pixels;
    }




    public static void SaveToPng(Color[] pixels, string name, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();


        var path = EditorUtility.SaveFilePanel(
                    "Save texture as PNG",
                    "",
                    name,
                    "png");

        File.WriteAllBytes(path, bytes);

    }
}
