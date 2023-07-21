using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HeightMap : MonoBehaviour
{
    // Start is called before the first frame update

    private static List<List<float>> heightMap;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct OutData
    {   //количество датчиков (кол)
        public int count;
        //количество обновлений (кол)
        public int iteration;
        //количество €иц (шт)
        public int col;
        //частота обновлени€ (0,033 в юнити) 
        public float update;
        //ширина конвеера (м)
        public float width;
        //рассто€ние ло конвеера (м)
        public float distanceToConv;
        //шум (м)
        public float noise;
    }



    public static void ReadFile()
    {
        string path = "C:\\Users\\NIKITA-PC\\Desktop\\data\\file.bin";


        //File.Open("C:\\Users\\NIKITA-PC\\Desktop\\data\\file.bin", FileMode.OpenOrCreate);

        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {


            int count = reader.ReadInt32();
            Debug.Log($"count: {count}");

            int iteration = reader.ReadInt32();
            int col = reader.ReadInt32();

            float update = (float)reader.ReadSingle();
            float width = (float)reader.ReadSingle();

            float distanceToConv = (float)reader.ReadSingle();
            float noise = (float)reader.ReadSingle();

            Debug.Log($"count: {count}  iteration: {iteration}  col: {col}  update: {update}  width: {width}  distanceToConv: {distanceToConv}  noise: {noise}");

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





            float distanceToConv = 0.1f;

            var outData = new OutData();

            outData.count = heightMap.Count;
            outData.iteration = heightMap[0].Count;
            outData.col = CalcEgg.getEggSpawnCol();

            outData.update = GlobalVar.getSensorUpdateDelay();
            outData.width = ConvWidthScript.getWidth();
            outData.distanceToConv = distanceToConv;
            outData.noise = GlobalVar.getNoise();



            var size = Marshal.SizeOf(outData);
            var buffer = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(outData, ptr, true);
            Marshal.Copy(ptr, buffer, 0, size);
            Marshal.FreeHGlobal(ptr);

            writer.Write(buffer);


            //writer.Write(outData,);

            for (int j = 0; j < heightMap[0].Count - 1; j++)
            {
                for (int i = 0; i <= heightMap.Count - 1; i++)
                {
                    writer.Write((float)heightMap[i][j]);
                }
            }

            writer.Close();


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
