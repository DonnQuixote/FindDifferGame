using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public MeshCollider meshCollider;

    private Bounds bounds;
    // Start is called before the first frame update
    void Start()
    {
        bounds = meshCollider.bounds;
        Vector3 test = bounds.max;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
