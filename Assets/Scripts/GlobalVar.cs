using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVar : MonoBehaviour
{
    //storage for global var

    private static float sensorUpdateDelay = 0.04f;

    public static float getSensorUpdateDelay()
    {
        return sensorUpdateDelay;
    }
    public static void IncSensorUpdateDelay(float newVar)
    {

        sensorUpdateDelay += newVar;
        Time.fixedDeltaTime = sensorUpdateDelay;

        //SpeedConv.setSpeedConv(sensorUpdateDelay);
    }
    public static void DicSensorUpdateDelay(float newVar)
    {   
        sensorUpdateDelay -= newVar;
        Time.fixedDeltaTime = sensorUpdateDelay;
        //SpeedConv.setSpeedConv(sensorUpdateDelay);
    }

    public static void setSensorUpdateDelay(float speed)
    {
        sensorUpdateDelay = 0.01f / speed;
    }

}