using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeTargetColor : MonoBehaviour
{
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");
    Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetColor(ColorProperty, UnityEngine.Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
