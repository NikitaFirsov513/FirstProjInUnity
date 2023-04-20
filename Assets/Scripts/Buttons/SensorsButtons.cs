using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorsButtons : MonoBehaviour
{
    public Text textField;
    
    void Start(){ 
        RefreshTextField();
    }

    public void RefreshTextField(){
        textField.text = SensorWidth.getWidth().ToString();
    }

    public void AddOne() {
        SensorWidth.addWidth(0.01f);
        RefreshTextField();
    }

    public void DiffOne()
    {

        if (SensorWidth.getWidth() - 0.01f < 0.01f)
            return;
        
        
        SensorWidth.diffWidth(0.01f);
        RefreshTextField();
    }
}
