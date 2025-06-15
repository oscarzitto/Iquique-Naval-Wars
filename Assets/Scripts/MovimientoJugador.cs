using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Joystick Móvil")]
    public Joystick joystick;         // Arrastra aquí tu Joystick de la UI
    public bool usarJoystick = true;  // Activa / desactiva la lectura del joystick

    [Header("Movimiento y Animación")]
    public float velocidad = 5f;
    public bool ConFisicas = false;   // Si es true usa AddForce, si no Translate
    public Rigidbody2D rb;            // Arrastra aquí tu Rigidbody2D
    public Animator animator;

    [Header("Inclinación")]
    public float inclinacionMaxima = 5f;
    public float suavizadoInclinacion = 10f;

    [Header("Estelas de Humo/Vapor")]
    public ParticleSystem estelaCentral;
    public ParticleSystem estelaIzquierda;
    public ParticleSystem estelaDerecha;

    [Header("Vida")]
    public Slider barraVida;
    public int vidaMaxima = 100;
    private int vidaActual;

    [Header("Timón")]
    public GameObject timonypumba;
    public float velocidadGiroTimon = 200f;

    // Input System
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
        // Asegurarte de tener el Rigidbody
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.gravityScale   = 0;
        rb.freezeRotation = true;

        // Vida
        vidaActual = vidaMaxima;
        if (barraVida != null)
        {
            barraVida.minValue = 0;
            barraVida.maxValue = vidaMaxima;
            barraVida.value    = vidaActual;
        }

        // Estelas
        DetenerTodasLasEstelas();
        if (estelaCentral != null)
        {
            estelaCentral.loop        = true;
            estelaCentral.playOnAwake = true;
            estelaCentral.Play();
        }
    }

    void Update()
    {
        // Si usamos joystick, sobrescribimos la entrada del InputSystem
        if (usarJoystick && joystick != null)
        {
            entradaMovimiento = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
    }

    void FixedUpdate()
    {
        // 1) Calcula la dirección final
        Vector2 dir = entradaMovimiento;

        // 2) Movimiento según flag ConFisicas
        if (ConFisicas)
        {
            rb.AddForce(dir * velocidad * Time.fixedDeltaTime,
                        ForceMode2D.Impulse);
        }
        else
        {
            transform.Translate(dir * velocidad * Time.fixedDeltaTime);
        }

        // 3) Animación
        if (animator != null)
            animator.SetFloat("movement", dir.magnitude);

        // 4) Inclinación suave del barco
        float objetivoZ = -dir.x * inclinacionMaxima;
        float actualZ  = transform.rotation.eulerAngles.z;
        if (actualZ > 180) actualZ -= 360f;
        float suavizadoZ = Mathf.LerpAngle(actualZ, objetivoZ,
                                           suavizadoInclinacion * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, suavizadoZ);

        // 5) Control de estelas
        ControlarEstelas(dir);

        // 6) Giro del timón
        if (timonypumba != null)
        {
            float giro = dir.x * velocidadGiroTimon * Time.fixedDeltaTime;
            timonypumba.transform.Rotate(0, 0, -giro);
        }

        // 7) Prueba de daño manual (tecla H)
        if (Keyboard.current.hKey.wasPressedThisFrame)
            RecibirDanio(10);
    }

    void ControlarEstelas(Vector2 movimiento)
    {
        ActivarEstela(estelaCentral);
        if (movimiento.x < -0.1f) ActivarEstela(estelaIzquierda);
        else                       DesactivarEstela(estelaIzquierda);

        if (movimiento.x > 0.1f)  ActivarEstela(estelaDerecha);
        else                       DesactivarEstela(estelaDerecha);
    }

    void ActivarEstela(ParticleSystem ps)
    {
        if (ps != null && !ps.isPlaying) ps.Play();
    }

    void DesactivarEstela(ParticleSystem ps)
    {
        if (ps != null && ps.isPlaying) ps.Stop();
    }

    void DetenerTodasLasEstelas()
    {
        DesactivarEstela(estelaCentral);
        DesactivarEstela(estelaIzquierda);
        DesactivarEstela(estelaDerecha);
    }

    void ActualizarBarraVida()
    {
        if (barraVida != null)
            barraVida.value = vidaActual;
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual  = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarBarraVida();
        if (vidaActual <= 0) Debug.Log("¡Jugador destruido!");
    }

    // 🌟 Entrada manual para UI externa (ej. un timón deslizable)
    public void AplicarEntradaManual(float movimientoX)
    {
        entradaMovimiento.x = Mathf.Clamp(
            movimientoX / velocidadGiroTimon, -1f, 1f);
    }
}
