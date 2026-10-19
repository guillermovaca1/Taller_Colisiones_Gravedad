using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;

    void Start()
    {
        // Guardar la distancia original entre cámara y jugador
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Seguir al jugador manteniendo el offset original
        transform.position = player.position + offset;
    }
}
