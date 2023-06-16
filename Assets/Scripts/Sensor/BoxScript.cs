using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
interface IResetable { void AddMas(int index); }

public class BoxScript : MonoBehaviour, IResetable
{
    public GameObject conv;
    public float nextUpdate = 0.0f;
    public List<float> mas;
    private int index;
    //public float updeteDelay = 0.1f;
    public void AddMas(int ind)
    {

        index = ind;
        //Debug.Log(index);
        mas = new List<float>();
        HeightMap.AddSensor(mas);

    }
    void FixedUpdate()
    {



        nextUpdate = Time.time + GlobalVar.getSensorUpdateDelay();


        Ray ray = new Ray(transform.position, transform.forward * 0.15f);
        Debug.DrawRay(transform.position, transform.forward * 0.15f, Color.blue);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 0.45f))
            return;
        
        //
        //Debug.Log(hit.distance);
        float randomScale = Random.Range(-GlobalVar.getNoise(), GlobalVar.getNoise());

        mas.Add(hit.distance + randomScale);
        //mas.Add(hit.distance);


        bool isIndexLast = (index == HeightMap.getMass().Count - 1);
        //Debug.Log(isIndexLast);
        if (isIndexLast)
        {
            HeightMap.CheckSensors();
        }


        if (hit.collider.gameObject.GetInstanceID() == -1268)
            return;

        float val = mas[mas.Count - 1];


        if (val >= GlobalVar.getMinDistance())
            return;



        hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
        Destroy(hit.collider.gameObject, 5f);



    }
}
