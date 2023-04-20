using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateButtons : MonoBehaviour
{


    public Text textField;

    // Start is called before the first frame update

    private void Start()
    {
        RefreshTextField();
    }
    void Update()
    {
        RefreshTextField();
    }
    public void IncUpdate()
    {
        GlobalVar.IncSensorUpdateDelay(0.01f);
        RefreshTextField();
    }
    public void DicUpdate()
    {

        if (GlobalVar.getSensorUpdateDelay() - 0.01f < 0.01f)
            return;
        GlobalVar.DicSensorUpdateDelay(0.01f);
        RefreshTextField();
    }
    public void RefreshTextField()
    {
        textField.text = GlobalVar.getSensorUpdateDelay().ToString();
    }
}