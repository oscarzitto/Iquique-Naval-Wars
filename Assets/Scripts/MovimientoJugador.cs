using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public Animator animator;

    public float inclinacionMaxima = 1f;
    public float suavizadoInclinacion = 10f;

    public ParticleSystem estelaCentral;
    public ParticleSystem estelaIzquierda;
    public ParticleSystem estelaDerecha;

    private Rigidbody2D rb;
    private Vector2 entradaMovimiento;
    private ControlesJugador controles;

    public Slider barraVida;
    public int vidaMaxima = 100;
    private int vidaActual;

    public GameObject timonypumba; // Referencia al timón
    public float velocidadGiroTimon = 200f;

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

        animator.SetFloat("movement", entradaMovimiento.magnitude);

        float anguloObjetivo = -entradaMovimiento.x * inclinacionMaxima;
        float anguloActual = transform.rotation.eulerAngles.z;
        if (anguloActual > 180) anguloActual -= 360f;
        float anguloSuavizado = Mathf.LerpAngle(anguloActual, anguloObjetivo, suavizadoInclinacion * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, anguloSuavizado);

        ControlarEstelas(movimiento);

        if (timonypumba != null)
        {
            float rotacionTimon = entradaMovimiento.x * velocidadGiroTimon * Time.fixedDeltaTime;
            timonypumba.transform.Rotate(0, 0, -rotacionTimon);
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            RecibirDanio(10);
        }
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
        {
            Debug.Log("¡Jugador destruido!");
        }
    }

    void ControlarEstelas(Vector2 movimiento)
    {
        if (movimiento.y > 0.1f)
            ActivarEstela(estelaCentral);
        else
            DesactivarEstela(estelaCentral);

        if (movimiento.x < -0.1f)
            ActivarEstela(estelaIzquierda);
        else
            DesactivarEstela(estelaIzquierda);

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

    // 🌟 Método para recibir input desde el timón
    public void AplicarEntradaManual(float movimientoX)
    {
        entradaMovimiento.x = Mathf.Clamp(movimientoX / velocidadGiroTimon, -1f, 1f);
    }
}
