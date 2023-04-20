using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSpawner : MonoBehaviour
{
    public GameObject sensor;
    //public float boxWidth = 0.2f;
    private Stack<GameObject> sensorStack = new Stack<GameObject>();
    private float localConvWidth;
    private float flagWidth;
    void Start()
    {        //Time.fixedDeltaTime = 0.01f;

        Spawn();
    }

    public void Spawn() {
        flagWidth=SensorWidth.getWidth();
        localConvWidth = ConvWidthScript.getWidth();
        int colBox = (int)(localConvWidth / SensorWidth.getWidth());
        float positionX = -((SensorWidth.getWidth() * colBox) / 2);
        //init height mass

        //Debug.Log("colbox" + colBox);
        HeightMap.InitMass();

        for (int i = 0; i <= colBox; i++) {
            GameObject sensorGameObj = Instantiate(sensor, new Vector3(positionX + SensorWidth.getWidth()*i, 3f, -8f), Quaternion.identity);
            IResetable isad = sensorGameObj.GetComponent<IResetable>();
            isad.AddMas(i);
            sensorGameObj.transform.Rotate(90f, 0f, 0f);
            sensorGameObj.transform.localScale = new Vector3(SensorWidth.getWidth(), 0.2f, 0.2f);
            sensorStack.Push(sensorGameObj);
        }

    }
    public void Delate(){
        
        while (sensorStack.Count>0) {
            Destroy(sensorStack.Pop());
        }


    }

    // Update is called once per frame
    void Update(){
        if (localConvWidth != ConvWidthScript.getWidth() ||  flagWidth != SensorWidth.getWidth() ) {

            Delate();
            Spawn();

        }
        
    }




}
