using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject go;
    private float randomX;
    private float randomScale = 1;

    public float spawnDelay;
    public int spawnCol;
    Vector3 whereToSpawn;    
    float nextSpawn = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float width = ConvWidthScript.getWidth();
        if (Time.time > nextSpawn&&GlobalVar.GetIsSpawn()) {

            nextSpawn = Time.time + spawnDelay;
            CalcEgg.addEggSpawnCol(spawnCol);
            for (int i = 0; i < spawnCol; i++) {
                randomX = Random.Range(-(width-0.2f)/2, (width - 0.2f)/ 2);

                randomScale = Random.Range(0.88f, 1.12f);
                //randomScale = Random.Range(0.88f, 0.89f);

                //randomScale = Random.Range(1.11f, 1.12f);
                float rotateEgg = Random.Range(0.0f, 360.0f);
                //float rotateEgg = 180f;
                whereToSpawn = new Vector3(randomX, 2.0f, -7f);
                GameObject Enemy = Instantiate(go, whereToSpawn, Quaternion.identity);
                Enemy.transform.Rotate(0.0f, rotateEgg,90f);
                Enemy.transform.localScale=new Vector3(0.0105f * randomScale, 0.0105f* randomScale, 0.0105f * randomScale);//0.021

            }
        }
    }
}
