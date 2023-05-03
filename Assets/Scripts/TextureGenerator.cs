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

        //������� ������
        //���� ����� �������, �� ���� �� �������
        //��� ������� ������ � �������
        //���� ����� �� ��������� �����, �� ������ ������
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
                    //�������� ���/����
                    


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

                        //���������
                       


                        Debug.Log("���������");
                        //���� �� ������� �������
                        //  -��������� ����� ������ � 1��� � ������ �����
                        //  -��������� � ���-� �������� "0" �����

                        //���� �� ������� �� ������� �������
                        //  -��������� ����� ������ � 1��� � ������ �����

                        if (mas[0].Count <= maxX - minX)
                        {
                            Debug.Log("����������������������� ������>" + (maxX - minX+1));

                            
                            AddToMass(mas, -1);
                            mas.Insert(0, CreateMass(maxX - minX + 1, 0));
                        }
                        if (mas[0].Count > maxX - minX){

                            //�� ������� �� ������� 

                        }

                    }
                    if (prevX - nowX > 0 && prevY - nowY > 0)
                    {

                        //��������

                        Debug.Log("��������");
                        int a = maxY - nowY;
                        int b = nowX - minX;

                        mas[a][b] = 1;

                        //�������� ������� �/��� ����������� 
                        //���� ������ ��� ������������
                    }
                    if (prevX - nowX < 0 && prevY - nowY < 0)
                    {

                        //����������
                        Debug.Log("����������");


                        if (mas[0].Count < maxX - minX)
                        {
                            //�����������
                            AddToMass(mas, 1);

                            mas.Insert(0, CreateMass(maxX - minX + 1, nowX - minX));


                        }
                        if (mas[0].Count >= maxX - minX)
                        {
                            //�� �����������

                            mas.Insert(0, CreateMass(maxX - minX + 1, nowX - minX));

                        }
                      

                    }
                    if (prevX - nowX < 0 && prevY - nowY > 0)
                    {

                        //���������
                        Debug.Log("���������");
                        if (mas[0].Count <= maxX - minX)
                        {
                            //�����������
                            AddToMass(mas, 1);

                            int a = maxY - nowY-1;
                            int b = nowX - minX - 1;

                            mas[a][b] = 1;


                        }
                        if (mas[0].Count > maxX - minX)
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
                        Debug.Log("�����");

                        if (mas[0].Count <= maxX - minX) {
                            //������� �� �������
                            AddToMass(mas, 1);
                            
                            int a = maxY - nowY;
                            int b = nowX - minX - 1;

                            mas[a][b] = 1;
                        }
                        if (mas[0].Count > maxX - minX)
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
                        Debug.Log("����");
                        int a = maxY - nowY;
                        int b = nowX - minX;

                        mas[a][b] = 1;

                        //�������� ������� �/��� ����������� 
                        //���� ������ ��� ������������
                    }
                    if (prevX - nowX == 0 && prevY - nowY < 0)
                    {

                        //�����
                        Debug.Log("�����");
                        mas.Insert(0, CreateMass(maxX - minX+1 , nowX - minX));
                        //�������� ������� �/��� ����������� 
                        //���� ������ � �������������

                    }
                    if (prevX - nowX == 0 && prevY - nowY > 0)
                    {

                        //����
                        Debug.Log("����");
                        int a = maxY - nowY;
                        int b = nowX - minX;

                        mas[a][b] = 1;
                        //�������� ������� �/��� ����������� 
                        //���� ������ ��� ������������

                    }

                    prevX = nowX; 
                    prevY=nowY;


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
