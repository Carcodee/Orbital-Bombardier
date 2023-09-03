using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OnDeadVfxController : MonoBehaviour
{
    public float lifeSpan;
    [SerializeField]
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator Life()
    {
        yield return new WaitForSeconds (lifeSpan);
        Destroy(gameObject);
    }
}
