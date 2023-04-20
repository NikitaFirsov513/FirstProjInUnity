using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorWidth : MonoBehaviour
{
    private static float width = 0.01f;

    public static float getWidth(){
        return width;
    }
    public static void setWidth(float newWidth){
        width = newWidth;
    }
    public static void addWidth(float add) {
        width += add;
    }
    public static void diffWidth(float add){
        
        width -= add;
    }
}
