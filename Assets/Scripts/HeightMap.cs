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


        for (int i = 1; i < heightMap.Count - 2; i++) {

            int j = heightMap[i].Count-2;
            float value = heightMap[i][j];

            if (heightMap[i][j + 1] > value &&
                heightMap[i][j - 1] > value &&
                heightMap[i + 1][j + 1] > value &&
                heightMap[i + 1][j] > value &&
                heightMap[i + 1][j - 1] > value &&
                heightMap[i - 1][j + 1] > value &&
                heightMap[i - 1][j] > value &&
                heightMap[i - 1][j - 1] > value &&
                value < GlobalVar.getBorderVal()
                ) {
                CalcEgg.addSum();
            }


        }


        TextureGenerator.UpdateImage(heightMap);
    }


    //private static bool CheckZone(int i, int sensorCount)
    //{

    //    if (heightMap[i][sensorCount + 1] != 1.45f && heightMap[i - 1][sensorCount + 1] != 1.45f) return true;
    //    if (heightMap[i - 1][sensorCount] == 1.45f) return true;
    //    if (heightMap[i][sensorCount - 1] == 1.45f || heightMap[i - 1][sensorCount - 1] == 1.45f) return true;
    //    if (heightMap[i][sensorCount - 2] == 1.45f || heightMap[i - 1][sensorCount - 2] == 1.45f) return true;
    //    if (i > 1 &&
    //        heightMap[i - 2][sensorCount] != 1.45f &&
    //        heightMap[i - 2][sensorCount - 1] != 1.45f &&
    //        heightMap[i - 2][sensorCount - 2] != 1.45f) return true;
    //    if (i > 1 &&
    //        heightMap[i - 2][sensorCount] != 1.45f &&
    //        heightMap[i - 2][sensorCount - 1] != 1.45f &&
    //        heightMap[i - 2][sensorCount + 1] != 1.45f &&
    //        heightMap[i - 1][sensorCount + 1] != 1.45f) return true;
    //    if (i > 1 &&
    //        heightMap[i - 2][sensorCount - 1] != 1.45f &&
    //        heightMap[i - 2][sensorCount - 2] != 1.45f &&
    //        heightMap[i - 2][sensorCount - 3] != 1.45f &&
    //        heightMap[i - 1][sensorCount - 3] != 1.45f) return true;
    //    return false;

    //}
}
