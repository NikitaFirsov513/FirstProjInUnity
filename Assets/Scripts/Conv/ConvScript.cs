using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ConvScript : MonoBehaviour
{

    public Rigidbody rb;
    public Material mt;
    public float nextUpdate = 0.0f;


    private void Start()
    {
        ScaleConv(ConvWidthScript.getWidth());
    }
    public void ScaleConv(float width)
    {
        transform.localScale = new Vector3(width, 0.1f, 15.6f);

    }
    private void FixedUpdate()
    {
        if (transform.localScale.x != ConvWidthScript.getWidth())
        {
            ScaleConv(ConvWidthScript.getWidth());
        }

        mt.mainTextureOffset = new Vector2(0f, -Time.time * SpeedConv.getSpeedConv() * 11 * Time.deltaTime);
        Vector3 pos = rb.position;
        rb.position += Vector3.forward * SpeedConv.getSpeedConv() * Time.fixedDeltaTime;
        //        rb.position += Vector3.forward * SpeedConv.getSpeedConv() * Time.fixedDeltaTime;
        rb.MovePosition(pos);
    }
}