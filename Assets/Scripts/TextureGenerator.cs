using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEditor.ShaderData;
//using System.Diagnostics;
using static UnityEngine.GraphicsBuffer;

public class TextureGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public Texture2D tex;

    public static Texture2D GetTexture(List<List<float>> heightMap)
    {
        float borderVal = GlobalVar.getBorderVal();

        int width = heightMap.Count - 1;
        int height = heightMap[0].Count - 1;
        var texture = new Texture2D(width, height);
        var testCenterTexture = new Texture2D(width, height);
        var defaultTexture = new Texture2D(width, height);

        var pixels = new Color[width * height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                float value = heightMap[x][y];

                if (value >= borderVal)
                    value = 0;
                else
                    value = (((1.45f - value) * 1000) - 20f) / 0.003f / 10000;

                pixels[x + y * width] = new Color(value, value, value);
            }
        }

        defaultTexture.SetPixels(pixels);
        defaultTexture.wrapMode = TextureWrapMode.Clamp;
        defaultTexture.Apply();
        SaveToPng(defaultTexture);

        Color[] testCenterPix = new Color[pixels.Length];
        Array.Copy(pixels, testCenterPix, pixels.Length);

        FindEggCout(heightMap, pixels, testCenterPix);
        //FindEggCoutV2(heightMap, pixels);

        testCenterTexture.SetPixels(testCenterPix);
        testCenterTexture.wrapMode = TextureWrapMode.Clamp;
        testCenterTexture.Apply();
        SaveToPng(testCenterTexture);


        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        SaveToPng(texture);

        return texture;
    }
    public static void FindEggCoutV2(List<List<float>> heightMap, Color[] pixels)
    {


        int width = heightMap.Count - 1;
        int height = heightMap[0].Count - 1;
        int count = 0;
        float borderVal = GlobalVar.getBorderVal();
        string findPix = "";

        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();




        for (int y = 1; y < height; y++)
        {
            for (int x = 1; x < width; x++)
            {


                //float value = heightMap[y][x];

                if (heightMap[x][y] > borderVal)
                    continue;

                float left = heightMap[x - 1][y];
                float left_bot = heightMap[x - 1][y - 1];
                float bot = heightMap[x - 1][y - 1];
                float right_bot = heightMap[x - 1][y - 1];

                if (heightMap[x - 1][y] > borderVal &&
                    heightMap[x - 1][y - 1] > borderVal &&
                    heightMap[x][y - 1] > borderVal &&
                    heightMap[x + 1][y - 1] > borderVal &&
                    heightMap[x + 2][y - 1] > borderVal)
                {

                    pixels[x + y * width] = new Color(1f, 1f, 0);
                    count++;

                }
            }
        }



        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

        Debug.Log("COUNT>" + count);
        Debug.Log("RunTime>" + elapsedTime);




    }
    public static void FindEggCout(List<List<float>> heightMap, Color[] pixels, Color[] testCenterPix)
    {

        //������� ������
        //���� ����� ������, �� ���� �� �������
        //��� ������� ������ � �������
        //���� ����� �� ��������� �����, �� ������ ������
        int width = heightMap.Count - 1;
        int height = heightMap[0].Count - 1;
        string findPix = "";

        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();


        float minDistance = GlobalVar.getMinDistance();

        for (int y = 1; y < height; y++)
        {
            for (int x = 1; x < width; x++)
            {



                if (heightMap[x][y] > minDistance)
                    continue;

                if (heightMap[x - 1][y] > minDistance &&
                    heightMap[x - 1][y - 1] > minDistance &&
                    heightMap[x][y - 1] > minDistance &&
                    heightMap[x + 1][y - 1] > minDistance &&
                    heightMap[x + 2][y - 1] > minDistance)
                {

                    pixels[x + y * width] = new Color(1f, 1f, 0);

                    if (!findPix.Contains(x + ":" + y))
                        findPix = FindNextPix(heightMap, x, y, findPix, pixels, testCenterPix);

                }
            }
        }



        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Debug.Log("RunTime>" + elapsedTime);



    }
    public static string FindNextPix(List<List<float>> heightMap, int startX, int startY, string findPix, Color[] pixels, Color[] testCenterPix)
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


                    //�������� ���/����

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

                        //���������



                        //Debug.Log("���������");
                        //���� �� ������� �������
                        //  -��������� ����� ������ � 1��� � ������ �����
                        //  -��������� � ���-� �������� "0" �����

                        //���� �� ������� �� ������� �������
                        //  -��������� ����� ������ � 1��� � ������ �����

                        if (mas[0].Count < maxX - minX + 1 && mas.Count < maxY - minY + 1)
                        {
                            //Debug.Log("����������������������� ������>" + (maxX - minX + 1));


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

                        //��������

                        //Debug.Log("��������");



                        if (mas[0].Count < maxX - minX + 1)
                        {
                            AddToMass(mas, -1);
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        else
                        {
                            //�� �����������
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;

                        }

                        //�������� ������� �/��� ����������� 
                        //���� ������ ��� ������������
                    }
                    if (prevX - nowX < 0 && prevY - nowY < 0)
                    {

                        //����������
                        //Debug.Log("����������");


                        if (mas[0].Count < maxX - minX + 1 && mas.Count < maxY - minY + 1)
                        {
                            //Debug.Log("����������������������� ������>" + (maxX - minX + 1));


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

                        //���������
                        //Debug.Log("���������");
                        if (mas[0].Count < maxX - minX + 1)
                        {
                            //�����������
                            AddToMass(mas, 1);

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;

                        }
                        else
                        {
                            //�� �����������

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }

                    }
                    if (prevX - nowX < 0 && prevY - nowY == 0)
                    {

                        //�����
                        //Debug.Log("�����");

                        if (mas[0].Count < maxX - minX + 1)
                        {
                            //������� �� �������
                            AddToMass(mas, 1);

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        else
                        {

                            //�� ������� �� �������
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }

                    }
                    if (prevX - nowX > 0 && prevY - nowY == 0)
                    {

                        //����
                        //Debug.Log("����");



                        if (mas[0].Count < maxX - minX + 1)
                        {
                            //Debug.Log("���� � �������������");

                            AddToMass(mas, -1);

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        else
                        {
                            //Debug.Log("���� ��� ������������");

                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }


                        //�������� ������� �/��� ����������� 
                        //���� ������ ��� ������������
                    }
                    if (prevX - nowX == 0 && prevY - nowY < 0)
                    {

                        //�����

                        if (mas.Count < maxY - minY + 1)
                        {
                            //Debug.Log("����� � �������������");
                            mas.Insert(0, CreateMass(maxX - minX + 1, nowX - minX));
                        }
                        else
                        {
                            //Debug.Log("����� ��� �������������");
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        //�������� ������� �/��� ����������� 
                        //���� ������ � �������������

                    }
                    if (prevX - nowX == 0 && prevY - nowY > 0)
                    {


                        if (mas.Count < maxY - minY + 1)
                        {
                            //Debug.Log("����� � �������������");
                            //mas.add(CreateMass(maxX - minX, nowX - minX));
                            mas.Add(CreateMass(maxX - minX, nowX - minX));
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }
                        else
                        {
                            //Debug.Log("����� ��� �������������");
                            int a = maxY - nowY;
                            int b = nowX - minX;

                            mas[a][b] = 1;
                        }

                        //����
                        //Debug.Log("����");
                        //int a = maxY - nowY;
                        ////int b = mas[0].Count - (maxX - nowX) - 1;
                        //int b = nowX - minX;

                        //mas[a][b] = 1;
                        //�������� ������� �/��� ����������� 
                        //���� ������ ��� ������������

                    }


                    prevX = nowX;
                    prevY = nowY;


                    //���� prevX-nowX!=0, �� ���� �������� �� x
                    //���� prevY-nowY!=0, �� ���� �������� �� y


                    //���� ���� �������� �� y:
                    //  -��������� ����� ������ �� ���������� == 0, ����� ����, ���� �������������. 
                    //  -���� prevX>nowX, �� � ������ �������� ��������� � ������ 0
                    //  -���� prevX<nowX, �� � ������ �������� ��������� � ����� 
                    //  -���� prevX<nowX, �� � ������ �������� ��������� � ����� 0
                    //����� ���� �������� �� x, �� ��������� ����� �������� � �������
                    //  -���� prevX > nowX, �� � ������ �������� ��������� � ������ 0
                    //  -���� prevX�nowX, �� � ������ �������� ��������� � ����� 0

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

        //int S = CalcSquare(mas);
        //Debug.Log("SQUARE>" + S);
        //Debug.Log("EGG>" + S / 18);
        //�������� ������ ������������� ���������� ���
        //CalcEgg.addSum(S / 15);

        Debug.Log(@"/--------DEBUG-MAS---------\");

        //for (int i = mas.Count - 1; i >= 0; i--)
        //{

        //    string str = "";
        //    for (int j = 0; j <= mas[0].Count - 1; j++)
        //    {
        //        str += heightMap[minX + j][minY + i] + " ";

        //    }
        //    Debug.Log(str);
        //}

        FindEggByHeight(minX, minY, mas, heightMap);

        Debug.Log(@"\--------DEBUG-MAS---------/");

        //TestCenterOfMass(minX, minY, mas, heightMap, testCenterPix);
        MethodOtsu(minX, minY, mas, heightMap, testCenterPix);

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

                if (val >= 0.75f)
                    testCenterPix[(minX + j) + (minY + localMas.Count - 1 - i) * width] = new Color(1f, 1f, 1f);
                else
                    testCenterPix[(minX + j) + (minY + localMas.Count - 1 - i) * width] = new Color(0f, 0f, 0f);


                //if (val >= listBright[maxK].val)
                //    testCenterPix[(minX + j) + (minY + localMas.Count - 1 - i) * width] = new Color(1f, 1f, 1f);
                //else
                //    testCenterPix[(minX + j) + (minY + localMas.Count - 1 - i) * width] = new Color(0f, 0f, 0f);

            }
        }









        return 0;
    }


    struct Coord
    {

        public int x;
        public int y;

        public Coord(int xNew, int yNew)
        {
            x = xNew;
            y = yNew;
        }
    }

    public static int TestCenterOfMass(int minX, int minY, List<List<int>> mas, List<List<float>> heightMap, Color[] testCenterPix)
    {



        int width = heightMap.Count - 1;
        int height = heightMap[0].Count - 1;

        List<List<float>> localMas = new List<List<float>>();
        List<Coord> listCoord = new List<Coord>();

        string str = "";

        for (int i = mas.Count - 1; i >= 0; i--)
        {

            localMas.Add(new List<float>());

            for (int j = 0; j <= mas[i].Count - 1; j++)
            {

                float val = heightMap[minX + j][minY + i];

                float valLeft = heightMap[minX + j - 1][minY + i];
                float valRight = heightMap[minX + j + 1][minY + i];

                float valTop = heightMap[minX + j][minY + i + 1];
                float valBot = heightMap[minX + j][minY + i - 1];


                float valTopLeft = heightMap[minX + j - 1][minY + i + 1];
                float valTopRight = heightMap[minX + j + 1][minY + i + 1];


                float valBotLeft = heightMap[minX + j - 1][minY + i - 1];
                float valBotRight = heightMap[minX + j + 1][minY + i - 1];


                //float power = (val * val + valLeft * valLeft + valRight * valRight + valTop * valTop + valBot * valBot+ valTopLeft * valTopLeft + valTopRight * valTopRight + valBotLeft * valBotLeft+ valBotRight * valBotRight);
                //Math.Pow

                float power = (val * 4 + valLeft * 2 + valRight * 2 + valTop * 2 + valBot * 2 + valTopLeft + valTopRight + valBotLeft + valBotRight) / 16;

                //float power = (val + valLeft + valRight + valTop + valBot + valTopLeft + valTopRight + valBotLeft + valBotRight) / 9;
                //float power = (val + valLeft*2 + valRight*2 + valTop*2 + valBot*2 + valTopLeft + valTopRight + valBotLeft + valBotRight) / 13;

                //float power = MedialFilter(val, valLeft, valRight, valTop, valBot, valTopLeft, valTopRight, valBotLeft, valBotRight);                

                power = (1.45f - power) * 20;

                //str += (power - power%0.001)*1000;
                str += power;
                str += " ";
                localMas[mas.Count - 1 - i].Add(power);


                //power -= 17.75f;

                //if (power > 1f) {
                //    power = 1f;
                //}
                //if (power < 0f)
                //{
                //    power = 0f;
                //}
                //if (power > 0.5f)
                //{
                //    power = 1 - power;
                //}
                //else {
                //    power = 1 - power;
                //}
                //str += power;
                //str += " ";

                //if (power > 0.6f)
                //    testCenterPix[minX + j + (minY + i) * width] = new Color(power, power, power);
                //else
                //    testCenterPix[minX + j + (minY + i) * width] = new Color(0f, 0f, 0f);

                testCenterPix[minX + j + (minY + i) * width] = new Color(power, power, power);

            }
            str += "\n";

        }



        for (int i = 1; i < localMas.Count - 1; i++)
        {
            for (int j = 1; j < localMas[i].Count - 1; j++)
            {
                float val = localMas[i][j];

                float valLeft = localMas[i - 1][j];
                float valRight = localMas[i + 1][j];

                float valTop = localMas[i][j + 1];
                float valBot = localMas[i][j - 1];


                float valTopLeft = localMas[i - 1][j + 1];
                float valTopRight = localMas[i + 1][j + 1];


                float valBotLeft = localMas[i - 1][j - 1];
                float valBotRight = localMas[i + 1][j - 1];

                //testCenterPix[minX + j + (minY + i) * width] = new Color(1f, 1f, 0f);



                if (val > valLeft &&
               val > valRight &&
               val > valTop &&
               val > valBot &&
               val > valTopLeft &&
               val > valTopRight &&
               val > valBotLeft &&
               val > valBotRight &&
               val > 0.15)
                {
                    CalcEgg.addSum();
                    testCenterPix[(minX + j) + (minY + localMas.Count - 1 - i) * width] = new Color(1f, 0f, 1f);
                    listCoord.Add(new Coord(j, i));
                }

            }


        }
        Debug.Log(@"/--------TestCenterOfMass---------\");
        Debug.Log(str);
        Debug.Log(@"\--------TestCenterOfMass---------/");







        return 0;
    }

    public static float MedialFilter(float val, float valLeft, float valRight, float valTop, float valBot, float valTopLeft, float valTopRight, float valBotLeft, float valBotRight)
    {

        float[] mas = new float[] { val, valLeft, valRight, valTop, valBot, valTopLeft, valTopRight, valBotLeft, valBotRight };
        float min = 2f;
        foreach (float a in mas)
        {
            if (a < min)
            {
                // ������ ������� �������
                min = a;
            }
        }

        return min;
    }


    public static int FindEggByHeight(int minX, int minY, List<List<int>> mas, List<List<float>> heightMap)
    {

        int maxSum = 0;
        bool isPrevConv = false;
        bool isPrevTop = false;
        bool isPrevBot = false;



        for (int i = mas.Count - 1; i >= 0; i--)
        {

            string str = "";
            int sum = 0;
            for (int j = 0; j <= mas[i].Count - 1; j++)
            {
                str += heightMap[minX + j][minY + i] + " ";

                if (j == 0)
                {
                    switch (heightMap[minX + j][minY + i])
                    {
                        case 1.45f:
                            {
                                isPrevConv = true;
                                isPrevTop = false;
                                isPrevBot = false;
                                break;
                            }
                        default:
                            {
                                isPrevConv = false;
                                isPrevTop = true;
                                isPrevBot = false;
                                break;
                            }
                    }
                }
                else
                {
                    float now = heightMap[minX + j][minY + i];

                    float prev = heightMap[minX + j - 1][minY + i];
                    if (heightMap[minX + j][minY + i] == 1.45f)
                    {
                        isPrevConv = true;
                        isPrevTop = false;
                        isPrevBot = false;

                        continue;
                    }

                    if ((heightMap[minX + j][minY + i] - heightMap[minX + j - 1][minY + i]) < 0)
                    {

                        isPrevConv = false;
                        isPrevTop = true;
                        isPrevBot = false;

                    }
                    if ((heightMap[minX + j][minY + i] - heightMap[minX + j - 1][minY + i]) > 0)
                    {

                        if (isPrevTop)
                        {
                            sum++;

                            isPrevConv = false;
                            isPrevTop = false;
                            isPrevBot = true;
                        }
                        else
                        {
                            isPrevConv = false;
                            isPrevTop = false;
                            isPrevBot = true;
                        }

                    }


                }

            }



            if (sum > maxSum)
                maxSum = sum;



            Debug.Log(str);
        }
        Debug.Log("SUM>" + maxSum);
        return 0;



    }
    public static int CalcSquare(List<List<int>> mas)
    {

        int maxX = mas[0].Count;
        int maxY = mas.Count;
        int S = 0;
        for (int y = 0; y < maxY; y++)
        {

            for (int x = 0; x < maxX; x++)
            {

                int element = mas[y][x];

                if (element == 1)
                {
                    S++;
                }
                if (element != 1)
                {
                    //��� ����� �� ���� ���� �����...
                    bool isLeftFind = false;
                    bool isRightFind = false;
                    bool isTopFind = false;
                    bool isBotFind = false;

                    for (int leftX = x; leftX >= 0; leftX--)
                    {
                        if (mas[y][leftX] == 1)
                        {
                            isLeftFind = true;
                            break;
                        }
                    }

                    for (int rightX = x; rightX < maxX; rightX++)
                    {
                        if (mas[y][rightX] == 1)
                        {
                            isRightFind = true;
                            break;
                        }
                    }
                    for (int topY = y; topY < maxY; topY++)
                    {
                        if (mas[topY][x] == 1)
                        {
                            isTopFind = true;
                            break;
                        }

                    }
                    for (int botY = y; botY >= 0; botY--)
                    {
                        if (mas[botY][x] == 1)
                        {
                            isBotFind = true;
                            break;
                        }
                    }

                    if (isLeftFind && isRightFind && isTopFind && isBotFind) S++;
                }
            }

        }


        return S;
    }
    //����� "��������� ����������"
    //����: ������ �������, ������ 1��
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
    //����� "�������� �������� � ������"
    //����: ������, ������ ���� ��������� "-1" �����, "1" ������
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


        float borderVal = GlobalVar.getBorderVal();
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

                if (value >= borderVal)
                    value = 0;
                else
                    value = (((0.10f - value) * 1000) - 21f) / 0.003f / 10000;


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
/*
 public static int TestCenterOfMass(int minX, int minY, List<List<int>> mas, List<List<float>> heightMap, Color[] testCenterPix)
{



    int width = heightMap.Count - 1;
    int height = heightMap[0].Count - 1;

    List<List<float>> localMas = new List<List<float>>();
    List<Coord> listCoord = new List<Coord>();

    string str = "";

    for (int i = mas.Count - 1; i >= 0; i--)
    {

        localMas.Add(new List<float>());

        for (int j = 0; j <= mas[i].Count - 1; j++)
        {

            float val = heightMap[minX + j][minY + i];

            float valLeft = heightMap[minX + j - 1][minY + i];
            float valRight = heightMap[minX + j + 1][minY + i];

            float valTop = heightMap[minX + j][minY + i + 1];
            float valBot = heightMap[minX + j][minY + i - 1];


            float valTopLeft = heightMap[minX + j - 1][minY + i + 1];
            float valTopRight = heightMap[minX + j + 1][minY + i + 1];


            float valBotLeft = heightMap[minX + j - 1][minY + i - 1];
            float valBotRight = heightMap[minX + j + 1][minY + i - 1];


            //float power = (val * val + valLeft * valLeft + valRight * valRight + valTop * valTop + valBot * valBot+ valTopLeft * valTopLeft + valTopRight * valTopRight + valBotLeft * valBotLeft+ valBotRight * valBotRight);
            //Math.Pow

            float power = (val + valLeft + valRight + valTop + valBot + valTopLeft + valTopRight + valBotLeft + valBotRight) / 9;

            //float power = MedialFilter(val, valLeft, valRight, valTop, valBot, valTopLeft, valTopRight, valBotLeft, valBotRight);                

            power = (1.45f - power) * 20;

            str += power;
            str += " ";
            localMas[mas.Count - 1 - i].Add(power);


            //power -= 17.75f;

            //if (power > 1f) {
            //    power = 1f;
            //}
            //if (power < 0f)
            //{
            //    power = 0f;
            //}
            //if (power > 0.5f)
            //{
            //    power = 1 - power;
            //}
            //else {
            //    power = 1 - power;
            //}
            //str += power;
            //str += " ";

            //

            testCenterPix[minX + j + (minY + i) * width] = new Color(power, power, power);


        }
        str += "\n";

    }



    for (int i = 1; i < localMas.Count - 1; i++)
    {
        for (int j = 1; j < localMas[i].Count - 1; j++)
        {
            float val = localMas[i][j];

            float valLeft = localMas[i - 1][j];
            float valRight = localMas[i + 1][j];

            float valTop = localMas[i][j + 1];
            float valBot = localMas[i][j - 1];


            float valTopLeft = localMas[i - 1][j + 1];
            float valTopRight = localMas[i + 1][j + 1];


            float valBotLeft = localMas[i - 1][j - 1];
            float valBotRight = localMas[i + 1][j - 1];

            //testCenterPix[minX + j + (minY + i) * width] = new Color(1f, 1f, 0f);
            bool isFindInList = false;

            foreach (Coord elem in listCoord)
            {
                if (Math.Abs(elem.x - j) <= 3 && Math.Abs(elem.y - i) <= 3)
                    isFindInList = true;

            }


            if (val > valLeft &&
           val > valRight &&
           val > valTop &&
           val > valBot &&
           val > valTopLeft &&
           val > valTopRight &&
           val > valBotLeft &&
           val > valBotRight &&
           val > 0.15&&
           !isFindInList)
            {
                CalcEgg.addSum();
                testCenterPix[(minX + j) + (minY + localMas.Count - 1 - i) * width] = new Color(1f, 0f, 1f);
                listCoord.Add(new Coord(j, i));
            }

        }


    }
    Debug.Log(@"/--------TestCenterOfMass---------\");
    Debug.Log(str);
    Debug.Log(@"\--------TestCenterOfMass---------/");







    return 0;
}
*/
