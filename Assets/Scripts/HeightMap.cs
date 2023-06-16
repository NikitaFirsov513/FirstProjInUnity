using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class HeightMap : MonoBehaviour
{
    // Start is called before the first frame update

    private static List<List<float>> heightMap;





    public static void ReadFile()
    {
        string path = "C:\\Users\\NIKITA-PC\\Desktop\\data\\file.bin";


        //File.Open("C:\\Users\\NIKITA-PC\\Desktop\\data\\file.bin", FileMode.OpenOrCreate);

        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {


            int count = reader.ReadInt32();
            int iteration = reader.ReadInt32();
            int col = reader.ReadInt32();

            float update = (float)reader.ReadSingle();
            float width = (float)reader.ReadSingle();

            Debug.Log($"count: {count}  iteration: {iteration}  col: {col}  update: {update}  width: {width}");

            for (int j = 0; j < iteration - 1; j++)
            {
                for (int i = 0; i <= count - 1; i++)
                {
                    Debug.Log((float)reader.ReadSingle());
                }
            }
        }
    }
    public static void WriteFile()
    {

        string path = "C:\\Users\\NIKITA-PC\\Desktop\\data\\file.bin";


        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
        {
            //количество датчиков.
            writer.Write(heightMap.Count);
            //количество опросов.
            writer.Write(heightMap[0].Count);
            //количество яиц
            writer.Write(CalcEgg.getEggSpawnCol());
            //частота обновления.
            writer.Write(GlobalVar.getSensorUpdateDelay());
            //ширина конвеера
            writer.Write(ConvWidthScript.getWidth());


            for (int j = 0; j < heightMap[0].Count - 1; j++)
            {
                for (int i = 0; i <= heightMap.Count - 1; i++)
                {
                    writer.Write((float)heightMap[i][j]);
                }
            }
            Debug.Log("File has been written");
        }

    }

    public static void InitMass()
    {
        heightMap = new List<List<float>>();
    }
    public static List<List<float>> getMass()
    {
        return heightMap;
    }
    public static void AddSensor(List<float> mas)
    {
        heightMap.Add(mas);
        //heightMap.;
    }

    public static void CheckSensors()
    {

        //проверка центра

        if (heightMap[0].Count < 5)
            return;

        //List<float> newHeightMap = new List<float>();
        //newHeightMap = MovingAverage(heightMap);

        //for (int i = 0; i <= heightMap.Count - 1; i++)
        //{

        //    heightMap[i][heightMap[i].Count - 2] = newHeightMap[i];


        //}


        TextureGenerator.UpdateImage(heightMap);
    }
    public static List<float> MovingAverage(List<List<float>> heightMap)
    {

        List<float> newHeightMap = new List<float>();
        //int j = 0; j <= heightMap[i].Count - 1; j++
        //int i = 1; i < heightMap.Count - 1; i++
        for (int j = heightMap[0].Count - 2; j < heightMap[0].Count - 1; j++)
        {
            for (int i = 0; i <= heightMap.Count - 1; i++)
            {

                //if (i == 0)
                //{
                //    newHeightMap.Add(new List<float>());
                //}


                if (i == 0 || i == heightMap.Count - 1)
                {
                    float powerOne = heightMap[i][j];
                    //powerOne = (1.45f - powerOne) * 20;

                    newHeightMap.Add(powerOne);
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

                newHeightMap.Add(power);





            }
        }


        return newHeightMap;
    }

}
