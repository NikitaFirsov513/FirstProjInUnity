using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMap : MonoBehaviour
{
    // Start is called before the first frame update

    private static List<List<float>> heightMap;


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


        


        TextureGenerator.UpdateImage(heightMap);
    }


}
