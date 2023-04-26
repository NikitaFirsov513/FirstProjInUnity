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
        
        int sensorCount = heightMap[0].Count - 1;
        for (int i = 1; i < heightMap.Count; i++)
        {

            if (heightMap[i][sensorCount] == 1.45f) continue;

            bool one = heightMap[i - 1][sensorCount] == 1.45f;
            bool two = heightMap[i - 1][sensorCount - 1] == 1.45f;
            bool three = heightMap[i][sensorCount - 1] == 1.45f;
            bool four = heightMap[i + 1][sensorCount - 1] == 1.45f;

            if (one &&
                two &&
                three &&
                four)
                CalcEgg.addSum();

        }
        TextureGenerator.UpdateImage(heightMap);
         }


    private static bool CheckZone(int i, int sensorCount)
    {

        if (heightMap[i][sensorCount + 1] != 1.45f && heightMap[i - 1][sensorCount + 1] != 1.45f) return true;
        if (heightMap[i - 1][sensorCount] == 1.45f) return true;
        if (heightMap[i][sensorCount - 1] == 1.45f || heightMap[i - 1][sensorCount - 1] == 1.45f) return true;
        if (heightMap[i][sensorCount - 2] == 1.45f || heightMap[i - 1][sensorCount - 2] == 1.45f) return true;
        if (i > 1 &&
            heightMap[i - 2][sensorCount] != 1.45f &&
            heightMap[i - 2][sensorCount - 1] != 1.45f &&
            heightMap[i - 2][sensorCount - 2] != 1.45f) return true;
        if (i > 1 &&
            heightMap[i - 2][sensorCount] != 1.45f &&
            heightMap[i - 2][sensorCount - 1] != 1.45f &&
            heightMap[i - 2][sensorCount + 1] != 1.45f &&
            heightMap[i - 1][sensorCount + 1] != 1.45f) return true;
        if (i > 1 &&
            heightMap[i - 2][sensorCount - 1] != 1.45f &&
            heightMap[i - 2][sensorCount - 2] != 1.45f &&
            heightMap[i - 2][sensorCount - 3] != 1.45f &&
            heightMap[i - 1][sensorCount - 3] != 1.45f) return true;
        return false;

    }
}
