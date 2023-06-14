using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMax : MonoBehaviour
{
    // Start is called before the first frame update
    public static void Start(List<List<float>> heightMap)
    {

        //ѕрименение скольз€щего среднего


        List<List<float>> averageHeightMap = MovingAverage(heightMap);



        int count = (FindObjects(averageHeightMap, 0.2f));
        Debug.Log(count);

        //применение преобразовани€ карты высот




        //¬ызов нахождени€ границ с изначальным значением(шум * 2, преобразованна€ карты высот)




    }

    public static int FindObjects(List<List<float>> heightMap, float border)
    {


        int width = heightMap.Count - 1;
        int height = heightMap[0].Count - 1;
        int count = 0;
        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {



                if (heightMap[x][y] <= border)
                    continue;

                if (x == width && heightMap[x - 1][y] <= border)
                {
                    count += FindBorder(heightMap, border, x, y);
                }

                if (x == 0 && y == 0)
                {
                    count += FindBorder(heightMap, border, x, y);
                }
                if (x == 0 && y != 0 && y != height)
                {
                    if (heightMap[x + 1][y + 1] <= border &&
                    heightMap[x][y + 1] <= border)
                    {
                        count += FindBorder(heightMap, border, x, y);
                    }
                }
                if (x != 0 && x != width && y == 0)
                {
                    if (heightMap[x - 1][y] <= border)
                    {
                        count += FindBorder(heightMap, border, x, y);
                    }
                }



                if (x != 0 && y != 0 &&
                    x != width && y != height)
                {
                    if (heightMap[x - 1][y] <= border &&
                    heightMap[x + 1][y + 1] < border &&
                    heightMap[x][y + 1] <= border &&
                    heightMap[x - 1][y + 1] <= border)
                    {
                        count += FindBorder(heightMap, border, x, y);
                    }
                    //алгоритм нахождени€ пикселей
                    count += FindBorder(heightMap, border, x, y);
                }
            }
        }





        return count;
    }

    public static int FindBorder(List<List<float>> heightMap, float border, int startX, int startY)
    {


        int nowX = startX, nowY = startY;
        int prevX = startX, prevY = startY;
        int minX = startX, maxX = startX;
        int minY = startY, maxY = startY;

        int width = heightMap.Count - 1;
        int heigh = heightMap[0].Count - 1;

        int countLoop = 0;
        int lastVector = 4;



        do
        {
            countLoop++;

            bool isFind = false;

            for (int j = 1; j <= 9; j++)
            {
                int i = lastVector + 4 + j;


                if (i > 8)
                    i -= 8;
                if (i > 8)
                    i -= 8;


                switch (i)
                {

                    case 1:
                        {
                            if (nowX == width || nowY == heigh)
                                break;
                            if (heightMap[nowX + 1][nowY + 1] > border)
                            {

                                nowX++;
                                nowY++;

                                if (nowX > maxX)
                                    maxX = nowX;
                                if (nowX < minX)
                                    minX = nowX;

                                if (nowY > maxY)
                                    maxY = nowY;
                                if (nowY < minY)
                                    minY = nowY;

                                isFind = true;
                            }



                            break;
                        }
                    case 2:
                        {
                            if (nowX == width)
                                break;
                            if (heightMap[nowX + 1][nowY] > border)
                            {

                                nowX++;

                                if (nowX > maxX)
                                    maxX = nowX;
                                if (nowX < minX)
                                    minX = nowX;

                                if (nowY > maxY)
                                    maxY = nowY;
                                if (nowY < minY)
                                    minY = nowY;

                                isFind = true;
                            }



                            break;
                        }
                    case 3:
                        {
                            if (nowX == width || nowY == 0)
                                break;
                            if (heightMap[nowX + 1][nowY - 1] > border)
                            {

                                nowX++;
                                nowY--;

                                if (nowX > maxX)
                                    maxX = nowX;
                                if (nowX < minX)
                                    minX = nowX;

                                if (nowY > maxY)
                                    maxY = nowY;
                                if (nowY < minY)
                                    minY = nowY;

                                isFind = true;
                            }



                            break;
                        }
                    case 4:
                        {
                            if (nowY == 0)
                                break;
                            if (heightMap[nowX][nowY - 1] > border)
                            {

                                nowY--;

                                if (nowX > maxX)
                                    maxX = nowX;
                                if (nowX < minX)
                                    minX = nowX;

                                if (nowY > maxY)
                                    maxY = nowY;
                                if (nowY < minY)
                                    minY = nowY;

                                isFind = true;
                            }



                            break;
                        }
                    case 5:
                        {
                            if (nowX == 0 || nowY == 0)
                                break;
                            if (heightMap[nowX - 1][nowY - 1] > border)
                            {
                                nowX--;
                                nowY--;

                                if (nowX > maxX)
                                    maxX = nowX;
                                if (nowX < minX)
                                    minX = nowX;

                                if (nowY > maxY)
                                    maxY = nowY;
                                if (nowY < minY)
                                    minY = nowY;

                                isFind = true;
                            }



                            break;
                        }
                    case 6:
                        {
                            if (nowX == 0)
                                break;
                            if (heightMap[nowX - 1][nowY] > border)
                            {
                                nowX--;

                                if (nowX > maxX)
                                    maxX = nowX;
                                if (nowX < minX)
                                    minX = nowX;

                                if (nowY > maxY)
                                    maxY = nowY;
                                if (nowY < minY)
                                    minY = nowY;

                                isFind = true;
                            }



                            break;
                        }
                    case 7:
                        {
                            if (nowX == 0 || nowY == heigh)
                                break;
                            if (heightMap[nowX - 1][nowY + 1] > border)
                            {
                                nowX--;
                                nowY++;



                                if (nowX > maxX)
                                    maxX = nowX;
                                if (nowX < minX)
                                    minX = nowX;

                                if (nowY > maxY)
                                    maxY = nowY;
                                if (nowY < minY)
                                    minY = nowY;

                                isFind = true;
                            }



                            break;
                        }
                    case 8:
                        {
                            if (nowY == heigh)
                                break;
                            if (heightMap[nowX][nowY + 1] > border)
                            {
                                nowY++;



                                if (nowX > maxX)
                                    maxX = nowX;
                                if (nowX < minX)
                                    minX = nowX;

                                if (nowY > maxY)
                                    maxY = nowY;
                                if (nowY < minY)
                                    minY = nowY;

                                isFind = true;
                            }



                            break;
                        }
                    default: { break; }



                }



                if (isFind)
                {
                    lastVector = i;
                    break;
                }





            }



        } while (nowX != startX || nowY != startY);

        List<List<float>> newList = new List<List<float>>();
        List<float> planList = new List<float>();

        for (int y = minY; y <= maxY; y++)
        {
            string str = "";
            for (int x = minX; x <= maxX; x++)
            {
                if (y - minY == 0)
                {
                    newList.Add(new List<float>());
                }
                if (heightMap[x][y] > border)
                {
                    planList.Add(heightMap[x][y]);
                }
                str += heightMap[x][y] + " ";
                newList[x - minX].Add(heightMap[x][y]);
            }
            Debug.Log(str + "\n");
        }
        planList.Sort();
        float val = planList[planList.Count / 2];
        Debug.Log("MAX X>" + maxX + "MAX Y>" + maxY);
        Debug.Log("MIN X>" + minX + "MIN Y>" + minY);
        Debug.Log("VAL>" + val);

        if (planList.Count > 1)
        {
            return FindObjects(newList, val);

        }
        if (planList.Count == 1)
        {
            return 1;
        }


        return 0;
    }

    public static List<List<float>> MovingAverage(List<List<float>> heightMap)
    {

        List<List<float>> newHeightMap = new List<List<float>>();
        //int j = 0; j <= heightMap[i].Count - 1; j++
        //int i = 1; i < heightMap.Count - 1; i++
        for (int j = 0; j <= heightMap[0].Count - 1; j++)
        {
            for (int i = 0; i <= heightMap.Count - 1; i++)
            {

                if (j == 0)
                {
                    newHeightMap.Add(new List<float>());
                }


                if (i == 0 || j == 0 || i == heightMap.Count - 1 || j == heightMap[i].Count - 1)
                {
                    float powerOne = heightMap[i][j];
                    powerOne = (1.45f - powerOne) * 20;

                    newHeightMap[i].Add(powerOne);
                    continue;
                }


                float val = heightMap[i][j];

                float valLeft = heightMap[i - 1][j];
                float valRight = heightMap[i + 1][j];

                float valTop = heightMap[i][j + 1];
                float valBot = heightMap[i][j - 1];


                float valTopLeft = heightMap[i - 1][j + 1];
                float valTopRight = heightMap[i + 1][j + 1];


                float valBotLeft = heightMap[i - 1][j - 1];
                float valBotRight = heightMap[i + 1][j - 1];

                float power = (val * 4 + valLeft * 2 + valRight * 2 + valTop * 2 + valBot * 2 + valTopLeft + valTopRight + valBotLeft + valBotRight) / 16;

                power = (1.45f + GlobalVar.getNoise() - power) * 20;
                newHeightMap[i].Add(power);





            }
        }


        return newHeightMap;
    }
}
