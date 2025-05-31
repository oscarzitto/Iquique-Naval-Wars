using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 50f;
    public float torque = 20f;
    public float velocidadAngularMax = 50f;

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
        rb.linearDamping = 1f;
        rb.angularDamping = 3f; // deja algo de inercia
        rb.inertia = 4f;        // más resistencia al giro
    }
    void FixedUpdate()
    {
        Vector2 direccion = transform.up * entradaMovimiento.y;
        rb.AddForce(direccion * velocidad);

        float giro = -entradaMovimiento.x;

        // 🔧 Suavizado progresivo del torque al acercarse al límite de velocidad angular
        float velocidadAngular = Mathf.Abs(rb.angularVelocity);
        float factorLimitador = Mathf.Clamp01(1f - (velocidadAngular / velocidadAngularMax));
        rb.AddTorque(giro * torque * factorLimitador);
    }

}
