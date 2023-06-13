using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMax : MonoBehaviour
{
    // Start is called before the first frame update
    public static void Start(List<List<float>> heightMap)
    {

        //Применение скользящего среднего


        List<List<float>> averageHeightMap = MovingAverage(heightMap);


        //применение преобразования карты высот


        //Вызов нахождения границ с изначальным значением(шум * 2, преобразованная карты высот)




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
