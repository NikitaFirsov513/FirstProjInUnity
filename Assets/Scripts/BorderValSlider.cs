using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BorderValSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Text textField;
    public TMP_InputField inputField;



    void Start()
    {
        slider.onValueChanged.AddListener((v) => { 
            textField.text = v.ToString();
            GlobalVar.setBorderVal(v);
            inputField.text = v.ToString();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
