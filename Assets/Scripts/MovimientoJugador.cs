using UnityEngine;
using UnityEngine.InputSystem;

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
