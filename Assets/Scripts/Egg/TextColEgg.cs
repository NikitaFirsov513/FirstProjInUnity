using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColEgg : MonoBehaviour
{
    public Text textField;
    void Start()
    {
        RefreshTextField();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshTextField();
    }

    public void RefreshTextField()
    {
        textField.text = CalcEgg.getSum().ToString();
    }
}
