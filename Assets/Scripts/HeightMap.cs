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
        //const int iterBorder = heightMap.Count-1;

        string str = "";
        for (int i = 1; i < heightMap.Count; i++)
        {

            int sensorCount = heightMap[i].Count - 2;
            if (sensorCount == -1) return;
            
            
            float val = heightMap[i][sensorCount+1];
            if(val != 1.45f) str +="  ("+i+")"+"|";

            if (heightMap[i][sensorCount] == 1.45f) continue;


            if (CheckZone(i, sensorCount)) continue;


            CalcEgg.addSum();


            /*
            Debug.Log("-----------------------------------------------------");
            Debug.Log("ITER>----------------" + heightMap[0].Count);
            Debug.Log("SENSOR>--------------" + i);
            Debug.Log("SUM>-----------------" + CalcEgg.getSum());
            Debug.Log("VAL>-----------------" + val);
            Debug.Log("-----------------------------------------------------");*/
        }
        //if (str.Contains('|'))
        //{
        //    Debug.Log(str);
        //    Debug.Log("SUM = " + CalcEgg.getSum());
        //}
        TextureGenerator.UpdateImage(heightMap);

    }


    private static bool CheckZone(int i, int sensorCount)
    {

        //œ–Œ¬≈– ¿ «ŒÕ€

        //if (i == 0) return CheckLeftZone(i, sensorCount);
        //if (i == 1) return CheckLeft1Zone(i, sensorCount);

        //if (i == heightMap.Count-1) return CheckRightZone(i, sensorCount);
        //if (i == heightMap.Count - 2) return CheckRight1Zone(i, sensorCount);



        //float prevVar = heightMap[i][sensorCount - 1];
        //bool isPrevIter—ounted = prevVar != 1.45f;


        //float prevIterVar = heightMap[i - 1][sensorCount];
        //bool isPrevSensorNotTouchConv = (prevIterVar != 1.45f);


        //bool isPrev1InerNextCount = heightMap[i + 1][sensorCount - 1] != 1.45f;
        //bool isPrev2InerNextCount = heightMap[i + 2][sensorCount - 1] != 1.45f;

        //bool isPrevM1InerNextCount = heightMap[i - 1][sensorCount - 1] != 1.45f;
        //bool isPrevM2InerNextCount = heightMap[i - 2][sensorCount - 1] != 1.45f;


        //return (
        //    isPrevSensorNotTouchConv ||
        //    isPrevIter—ounted ||
        //    isPrev1InerNextCount ||
        //    isPrev2InerNextCount ||
        //    isPrevM1InerNextCount ||
        //    isPrevM2InerNextCount);
        if (heightMap[i][sensorCount +1] != 1.45f && heightMap[i-1][sensorCount + 1] != 1.45f) return true;
        if (heightMap[i-1][sensorCount ] == 1.45f) return true;
        if (heightMap[i][sensorCount - 1] == 1.45f || heightMap[i - 1][sensorCount - 1] == 1.45f) return true;
        if (heightMap[i][sensorCount - 2] == 1.45f || heightMap[i - 1][sensorCount - 2] == 1.45f) return true;
        if(i>1 &&
            heightMap[i - 2][sensorCount] != 1.45f && 
            heightMap[i - 2][sensorCount-1] != 1.45f && 
            heightMap[i - 2][sensorCount - 2] != 1.45f) return true;
        if (i > 1 &&
            heightMap[i - 2][sensorCount] != 1.45f &&
            heightMap[i - 2][sensorCount - 1] != 1.45f &&
            heightMap[i - 2][sensorCount +1] != 1.45f &&
            heightMap[i - 1][sensorCount + 1] != 1.45f) return true;
        if (i > 1 &&
            heightMap[i - 2][sensorCount - 1] != 1.45f &&
            heightMap[i - 2][sensorCount - 2] != 1.45f &&
            heightMap[i - 2][sensorCount - 3] != 1.45f &&
            heightMap[i - 1][sensorCount - 3] != 1.45f) return true;
        return false;
    }

    //‡Î„ÓËÚÏ ÓÔÂ‰ÂÎÂÌËÂ ˆÂÌÚ‡ Ï‡ÒÒ
    //private static bool CheckLeftZone(int i, int sensorCount)
    //{
    //    float prevIterVar = heightMap[i][sensorCount-1];
    //    bool isPrevIter—ounted = (prevIterVar != 1.45f);



    //    bool isPrev1InerNextCount = heightMap[i + 1][sensorCount - 1] != 1.45f;
    //    bool isPrev2InerNextCount = heightMap[i + 2][sensorCount - 1] != 1.45f;


    //    return (isPrevIter—ounted || isPrev1InerNextCount || isPrev2InerNextCount);
    //}
    //private static bool CheckLeft1Zone(int i, int sensorCount)
    //{
    //    float prevIterVar = heightMap[i][sensorCount - 1];
    //    bool isPrevIter—ounted = (prevIterVar != 1.45f);


    //    bool isPrevM1InerNextCount = heightMap[i - 1][sensorCount - 1] != 1.45f;

    //    bool isPrev1InerNextCount = heightMap[i + 1][sensorCount - 1] != 1.45f;
    //    bool isPrev2InerNextCount = heightMap[i + 2][sensorCount - 1] != 1.45f;


    //    return (isPrevIter—ounted || isPrevM1InerNextCount || isPrev1InerNextCount || isPrev2InerNextCount);
    //}
    //private static bool CheckRightZone(int i, int sensorCount)
    //{
    //    float prevIterVar = heightMap[i - 1][sensorCount];
    //    bool isPrevSensorNotTouchConv = (prevIterVar != 1.45f);



    //    bool isPrevM1InerNextCount = heightMap[i - 1][sensorCount - 1] != 1.45f;
    //    bool isPrevM2InerNextCount = heightMap[i - 2][sensorCount - 1] != 1.45f;


    //    return (isPrevSensorNotTouchConv || isPrevM1InerNextCount || isPrevM2InerNextCount);
    //}
    //private static bool CheckRight1Zone(int i, int sensorCount)
    //{
    //    float prevIterVar = heightMap[i - 1][sensorCount];
    //    bool isPrevSensorNotTouchConv = (prevIterVar != 1.45f);


    //    bool isPrev1InerNextCount = heightMap[i +1][sensorCount - 1] != 1.45f;

    //    bool isPrevM1InerNextCount = heightMap[i - 1][sensorCount - 1] != 1.45f;
    //    bool isPrevM2InerNextCount = heightMap[i - 2][sensorCount - 1] != 1.45f;


    //    return (isPrevSensorNotTouchConv || isPrevM1InerNextCount || isPrevM2InerNextCount || isPrev1InerNextCount);
    //}
}
