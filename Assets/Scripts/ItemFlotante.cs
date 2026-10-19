using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFlotante : MonoBehaviour
{
    [Header("Ajuste de Flotacion")]
    public float amplitud = 0.3f;
    public float velocidad = 2f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidad) * amplitud;
        transform.position = new Vector3(transform.position.x, nuevaY, transform.position.z);
    }
}