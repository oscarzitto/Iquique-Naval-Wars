using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento y Animación")]
    public float velocidad = 5f;
    public Animator animator;
    public float inclinacionMaxima = 1f;
    public float suavizadoInclinacion = 10f;

    [Header("Estelas de Humo/Vapor")]
    public ParticleSystem estelaCentral;   // Vapor del motor (siempre ON)
    public ParticleSystem estelaIzquierda;
    public ParticleSystem estelaDerecha;

    [Header("Vida")]
    public Slider barraVida;
    public int vidaMaxima = 100;
    private int vidaActual;

    [Header("Timón")]
    public GameObject timonypumba;
    public float velocidadGiroTimon = 200f;

    private Rigidbody2D rb;
    private Vector2 entradaMovimiento;
    private ControlesJugador controles;

    void Awake()
    {
        controles = new ControlesJugador();
    }

    void OnEnable()
    {
        controles.Jugador.Enable();
        controles.Jugador.Mover.performed += ctx => entradaMovimiento = ctx.ReadValue<Vector2>();
        controles.Jugador.Mover.canceled  += ctx => entradaMovimiento = Vector2.zero;
    }

    void OnDisable()
    {
        controles.Jugador.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        // Apaga todas las estelas primero
        DetenerTodasLasEstelas();

        // ¡Arranca siempre el vapor central!
        if (estelaCentral != null)
        {
            estelaCentral.loop        = true;
            estelaCentral.playOnAwake = true;
            estelaCentral.Play();
        }

        // Configurar barra de vida
        vidaActual = vidaMaxima;
        if (barraVida != null)
        {
            barraVida.minValue = 0;
            barraVida.maxValue = vidaMaxima;
            barraVida.value    = vidaActual;
        }
    }

    void FixedUpdate()
    {
        // Movimiento + velocidad
        Vector2 movimiento     = entradaMovimiento * velocidad;
        rb.linearVelocity      = movimiento;
        animator.SetFloat("movement", entradaMovimiento.magnitude);

        // Inclinación suave del barco
        float objetivoZ = -entradaMovimiento.x * inclinacionMaxima;
        float actualZ  = transform.rotation.eulerAngles.z;
        if (actualZ > 180) actualZ -= 360f;
        float suavizadoZ = Mathf.LerpAngle(actualZ, objetivoZ, suavizadoInclinacion * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, suavizadoZ);

        // Control de estelas
        ControlarEstelas(movimiento);

        // Giro del timón
        if (timonypumba != null)
        {
            float giro = entradaMovimiento.x * velocidadGiroTimon * Time.fixedDeltaTime;
            timonypumba.transform.Rotate(0, 0, -giro);
        }

        // Prueba de daño manual
        if (Keyboard.current.hKey.wasPressedThisFrame)
            RecibirDanio(10);
    }

    void ActualizarBarraVida()
    {
        if (barraVida != null)
            barraVida.value = vidaActual;
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarBarraVida();

        if (vidaActual <= 0)
            Debug.Log("¡Jugador destruido!");
    }

    void ControlarEstelas(Vector2 movimiento)
    {
        // 1) Vapor central siempre encendido
        ActivarEstela(estelaCentral);

        // 2) Estela izquierda al girar hacia la izquierda
        if (movimiento.x < -0.1f) ActivarEstela(estelaIzquierda);
        else                     DesactivarEstela(estelaIzquierda);

        // 3) Estela derecha al girar hacia la derecha
        if (movimiento.x > 0.1f)  ActivarEstela(estelaDerecha);
        else                      DesactivarEstela(estelaDerecha);
    }

    void ActivarEstela(ParticleSystem ps)
    {
        if (ps != null && !ps.isPlaying)
            ps.Play();
    }

    void DesactivarEstela(ParticleSystem ps)
    {
        if (ps != null && ps.isPlaying)
            ps.Stop();
    }

    void DetenerTodasLasEstelas()
    {
        DesactivarEstela(estelaCentral);
        DesactivarEstela(estelaIzquierda);
        DesactivarEstela(estelaDerecha);
    }

    // 🌟 Permite recibir input manual externo (por ejemplo desde UI de timón)
    public void AplicarEntradaManual(float movimientoX)
    {
        entradaMovimiento.x = Mathf.Clamp(movimientoX / velocidadGiroTimon, -1f, 1f);
    }
}
