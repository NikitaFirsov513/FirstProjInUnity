using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BorderValueInput : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public TMP_InputField inputField;

    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
    public void Change() {

        slider.value = float.Parse(inputField.text);



    }
}
