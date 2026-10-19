using UnityEngine;

public class RecogerItemScript : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastra aquí el objeto de la escena que quieres desactivar")]
    public GameObject objetoADesactivar;

    [Tooltip("Escribe aquí la etiqueta del Jugador (por defecto suele ser 'Player')")]
    public string tagDelJugador = "Player";

    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el objeto que entró al Trigger es el jugador
        if (other.CompareTag(tagDelJugador))
        {
            // 1. Desactivar el objeto objetivo si fue asignado
            if (objetoADesactivar != null)
            {
                objetoADesactivar.SetActive(false);
            }
            else
            {
                Debug.LogWarning("No asignaste ningún objeto a desactivar en el Inspector.");
            }

            // 2. Destruir este Item (el objeto que lleva este script)
            Destroy(gameObject);
        }
    }
}