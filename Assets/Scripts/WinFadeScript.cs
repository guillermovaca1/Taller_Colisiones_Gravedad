using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinFadeScript : MonoBehaviour
{
    [Header("Configuración del Fade")]
    [Tooltip("Arrastra aquí la imagen de la UI que se pondrá blanca")]
    public Image pantallaBlanca;

    [Tooltip("Arrastra aquí el objeto del Texto que dice 'escapaste'")]
    public GameObject textoVictoria;

    [Tooltip("Velocidad a la que la pantalla se vuelve blanca")]
    public float velocidadFade = 1f;

    [Tooltip("Etiqueta del jugador")]
    public string tagJugador = "Player";

    private bool yaGanado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador) && !yaGanado)
        {
            yaGanado = true;

            if (pantallaBlanca != null)
            {
                pantallaBlanca.gameObject.SetActive(true);

                if (textoVictoria != null)
                {
                    textoVictoria.SetActive(false);
                }

                StartCoroutine(HacerFadeABlanco());
            }
        }
    }

    private IEnumerator HacerFadeABlanco()
    {
        Color colorActual = pantallaBlanca.color;


        while (colorActual.a < 1f)
        {

            colorActual.a += velocidadFade * Time.unscaledDeltaTime;
            pantallaBlanca.color = colorActual;
            yield return null;
        }


        colorActual.a = 1f;
        pantallaBlanca.color = colorActual;


        if (textoVictoria != null)
        {
            textoVictoria.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}