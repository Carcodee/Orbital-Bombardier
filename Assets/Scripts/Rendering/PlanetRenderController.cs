using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlanetRenderController : MonoBehaviour
{
    public Planet planet;
    public Camera playerCam;
   

    void Start()
    {
        playerCam.useOcclusionCulling = true;    
    }

    void Update()
    {
        
    }
}
