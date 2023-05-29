using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpeedConv : MonoBehaviour
{
    private static float speedConv = 0.20f;

    public static float getSpeedConv()
    {
        return speedConv;
    }
    public static void IncSpeedConv(float newVar)
    {
        speedConv += newVar;
        //GlobalVar.setSensorUpdateDelay(speedConv);
    }
    public static void DicSpeedConv(float newVar)
    {
        speedConv -= newVar;
        //GlobalVar.setSensorUpdateDelay(speedConv);
    }

    public static void setSpeedConv(float speed)
    {
        speedConv = 0.01f / speed;
    }
}
