using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class VFXSpawner : MonoBehaviour
{
    public VisualEffect deadExplotionPrefab;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GenerateDeadExplotion()
    {
        VisualEffect deadExplotionInstance = Instantiate(deadExplotionPrefab, gameObject.transform.position,quaternion.identity);
    }
}
