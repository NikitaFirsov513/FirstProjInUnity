using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvButtons : MonoBehaviour
{
    // Start is called before the first frame update

    public Text textField;
    
    void Start(){ 
        RefreshTextField();
    }


    public void RefreshTextField(){
        textField.text = ConvWidthScript.getWidth().ToString();
    }

    public void AddOne() {
        ConvWidthScript.addWidth(1f);
        RefreshTextField();
    }
    public void DiffOne()
    {
        if(ConvWidthScript.getWidth() > 1) ConvWidthScript.diffWidth(1f);
        RefreshTextField();
    }
    // Update is called once per frame

}
