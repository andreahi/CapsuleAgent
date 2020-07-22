using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color : MonoBehaviour
{
    Renderer rend;

    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

// Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetColor(ColorProperty, UnityEngine.Color.green);
    }

    // Update is called once per frame
    void Update()
    {
    }
}