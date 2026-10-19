using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    public float velocidad = 5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float radio = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    public bool estaEnSuelo;

    [Header("Wall Flip")]
    public LayerMask wallLayer;
    private bool mirandoAdelante = true;

    [Header("Jump")]
    public float fuerzaSalto = 7f;
    public int saltosMaximos = 2;

    private int saltosRealizados = 0;
    private bool quiereSaltar;

    [Header("Retroceso Suave")]
    public float velocidadRetroceso = 8f;
    public float duracionRetroceso = 0.25f;
    public float cooldownRetroceso = 2f; // Tiempo de espera entre cada retroceso

    private bool estaRetrocediendo = false;
    private float tiempoRestanteRetroceso = 0f;
    private float proximoRetrocesoPermitido = 0f; // Controla cuándo se puede volver a retroceder

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Registrar la pulsación de la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            quiereSaltar = true;
        }

        // Registrar la pulsación de la tecla E para retroceder
        // Solo permite retroceder si NO está retrocediendo y si ya pasó el cooldown
        if (Input.GetKeyDown(KeyCode.E) && !estaRetrocediendo && Time.time >= proximoRetrocesoPermitido)
        {
            estaRetrocediendo = true;
            tiempoRestanteRetroceso = duracionRetroceso;

            // Establecer el tiempo futuro en el que se podrá volver a usar la habilidad
            proximoRetrocesoPermitido = Time.time + cooldownRetroceso;
        }
    }

    void FixedUpdate()
    {
        // Comprobar si está tocando el suelo
        estaEnSuelo = Physics.CheckSphere(
            groundCheck.position,
            radio,
            groundLayer
        );

        // Restablecer los saltos al tocar el suelo
        if (estaEnSuelo && rb.linearVelocity.y <= 0f)
        {
            saltosRealizados = 0;
        }

        // Dirección del movimiento normal en Z
        float direccion = mirandoAdelante ? 1f : -1f;

        // --- LÓGICA DE MOVIMIENTO Y RETROCESO ---
        if (estaRetrocediendo)
        {
            // Mover suavemente en la dirección opuesta
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                rb.linearVelocity.y,
                -direccion * velocidadRetroceso
            );

            // Reducir el tiempo de retroceso
            tiempoRestanteRetroceso -= Time.fixedDeltaTime;

            // Si se acaba el tiempo, terminar el retroceso
            if (tiempoRestanteRetroceso <= 0f)
            {
                estaRetrocediendo = false;
            }
        }
        else
        {
            // Mantener el movimiento normal por Z si no está retrocediendo
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                rb.linearVelocity.y,
                velocidad * direccion
            );
        }

        // --- LÓGICA DE SALTO ---
        // Ejecutar el salto si quedan saltos disponibles
        if (quiereSaltar && saltosRealizados < saltosMaximos)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

            rb.AddForce(
                Vector3.up * fuerzaSalto,
                ForceMode.Impulse
            );

            saltosRealizados++;
        }

        // Consumir la solicitud de salto
        quiereSaltar = false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                radio
            );
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Comprobar si chocó con una pared
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            // Girar 180 grados en Y
            transform.Rotate(0f, 180f, 0f);

            // Cambiar la dirección del movimiento
            mirandoAdelante = !mirandoAdelante;
        }
    }
}