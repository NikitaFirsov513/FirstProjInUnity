using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBtnActions : MonoBehaviour
{
    // Start is called before the first frame update
    public void generate()
    {
        HeightMap.WriteFile();
        //HeightMap.ReadFile();
        //TextureGenerator.GetTexture(HeightMap.getMass());
        //ImageProcessing.Start(HeightMap.getMass());
        //LocalMax.Start(HeightMap.getMass());
    }
}
