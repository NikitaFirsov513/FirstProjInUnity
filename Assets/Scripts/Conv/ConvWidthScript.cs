using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvWidthScript : MonoBehaviour
{
    // Start is called before the first frame update

    private static float width = 1.5f;


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
