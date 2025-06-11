using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public Animator animator;

    public float inclinacionMaxima = 1f; // grados de inclinación máxima
    public float suavizadoInclinacion = 10f; // qué tan rápido vuelve al centro

    public ParticleSystem estelaCentral;   // 💧 Partícula del motor
    public ParticleSystem estelaIzquierda; // 🔁 Partícula al moverse a la izquierda
    public ParticleSystem estelaDerecha;   // 🔁 Partícula al moverse a la derecha

    private Rigidbody2D rb;
    private Vector2 entradaMovimiento;

    private ControlesJugador controles;

    public Slider barraVida; // Asigna el slider de vidaChile en el Inspector
    public int vidaMaxima = 100;
    private int vidaActual;

    void Awake()
    {
        controles = new ControlesJugador();
    }

    void OnEnable()
    {
        controles.Jugador.Enable();
        controles.Jugador.Mover.performed += ctx => entradaMovimiento = ctx.ReadValue<Vector2>();
        controles.Jugador.Mover.canceled += ctx => entradaMovimiento = Vector2.zero;
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

        DetenerTodasLasEstelas();

        // Inicialización de vida
        vidaActual = vidaMaxima;

        if (barraVida != null)
        {
            barraVida.minValue = 0;
            barraVida.maxValue = vidaMaxima;
            barraVida.value = vidaActual;
        }
    }

    void FixedUpdate()
    {
        Vector2 movimiento = entradaMovimiento * velocidad;
        rb.linearVelocity = movimiento;

        // 🎞 Activar animación según movimiento total
        animator.SetFloat("movement", entradaMovimiento.magnitude);

        // 🔁 Inclinación del sprite al moverse lateralmente
        float anguloObjetivo = -entradaMovimiento.x * inclinacionMaxima;
        float anguloActual = transform.rotation.eulerAngles.z;
        if (anguloActual > 180) anguloActual -= 360f;
        float anguloSuavizado = Mathf.LerpAngle(anguloActual, anguloObjetivo, suavizadoInclinacion * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, anguloSuavizado);

        // 🔥 Control de partículas según dirección
        ControlarEstelas(movimiento);

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            RecibirDanio(10); // Baja 10 de vida con tecla H **** DE PRUEBA *****
        }
    }
    void ActualizarBarraVida()
    {
        if (barraVida != null)
            barraVida.value = vidaActual; // Valor entero directo (0 a 100)
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarBarraVida();

        if (vidaActual <= 0)
        {
            Debug.Log("¡Jugador destruido!");
            // Aquí puedes desactivar al jugador, reproducir animación, etc.
        }
    }


    void ControlarEstelas(Vector2 movimiento)
    {
        // Motor: cuando hay movimiento hacia arriba
        if (movimiento.y > 0.1f)
            ActivarEstela(estelaCentral);
        else
            DesactivarEstela(estelaCentral);

        // Izquierda
        if (movimiento.x < -0.1f)
            ActivarEstela(estelaIzquierda);
        else
            DesactivarEstela(estelaIzquierda);

        // Derecha
        if (movimiento.x > 0.1f)
            ActivarEstela(estelaDerecha);
        else
            DesactivarEstela(estelaDerecha);
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
}
