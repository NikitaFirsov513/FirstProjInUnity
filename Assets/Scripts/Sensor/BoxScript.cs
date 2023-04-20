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
        //float width = ConvWidthScript.getWidth();

        //if (Time.time >= nextUpdate)
        {
            nextUpdate = Time.time + GlobalVar.getSensorUpdateDelay();


            Ray ray = new Ray(transform.position, transform.forward * 1.5f);
            Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.blue);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, 1.5f))
                return;

            mas.Add(hit.distance);


            bool isIndexLast = (index == HeightMap.getMass().Count - 1);
            //Debug.Log(isIndexLast);
            if (isIndexLast) {
                HeightMap.CheckSensors();
            }


            if (hit.distance == 1.45f || mas.Count < 2)
                return;

            bool a = mas[mas.Count - 1] != 1.45f;
            bool b = mas[mas.Count - 2] == 1.45f;
            if (!(a && b))
                return;



            hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
            Destroy(hit.collider.gameObject, 5f);
            //CalcEgg.addSum();
            //Debug.Log("�����=>" + CalcEgg.getSum());

           


        }
    }

}