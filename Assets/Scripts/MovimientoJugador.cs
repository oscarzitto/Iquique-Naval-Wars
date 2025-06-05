using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public Animator animator;

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
        rb.freezeRotation = true; // Muy importante: evita que rote
    }

    void FixedUpdate()
    {
        // Movimiento sin rotación, en ejes globales (arriba/abajo y izquierda/derecha)
        Vector2 movimiento = entradaMovimiento * velocidad;
        rb.linearVelocity = movimiento;

        // Animación basada en movimiento vertical
        animator.SetFloat("movement", Mathf.Abs(entradaMovimiento.y));
    }
}
