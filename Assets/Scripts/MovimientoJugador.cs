using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Para reiniciar escena o volver al menú

public class MovimientoJugador : MonoBehaviour
{
    [Header("Joystick Móvil")]
    public Joystick joystick;
    public bool usarJoystick = true;

    [Header("Movimiento y Animación")]
    public float velocidad = 5f;
    public bool ConFisicas = false;
    public Rigidbody2D rb;
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

    [Header("Game Over UI")]
    [Tooltip("Arrastra aquí el GameObject con tu script GameOverUI")]
    public GameOverUI gameOverUI;

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
        // Rigidbody2D setup
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        // Vida y barra
        vidaActual = vidaMaxima;
        if (barraVida != null)
        {
            barraVida.minValue = 0;
            barraVida.maxValue = vidaMaxima;
            ActualizarBarraVida();
        }

        // Carga guardado
        CargarDatos();

        // Partículas
        DetenerTodasLasEstelas();
        if (estelaCentral != null)
        {
            estelaCentral.loop = true;
            estelaCentral.playOnAwake = true;
            estelaCentral.Play();
        }

        // Referencia a GameOverUI
        if (gameOverUI == null)
            gameOverUI = FindObjectOfType<GameOverUI>();
    }

    void Update()
    {
        if (usarJoystick && joystick != null)
            entradaMovimiento = new Vector2(joystick.Horizontal, joystick.Vertical);
    }

    void FixedUpdate()
    {
        Vector2 dir = entradaMovimiento;

        // Movimiento
        if (ConFisicas)
            rb.AddForce(dir * velocidad * Time.fixedDeltaTime, ForceMode2D.Impulse);
        else
            transform.Translate(dir * velocidad * Time.fixedDeltaTime);

        // Animación
        if (animator != null)
            animator.SetFloat("movement", dir.magnitude);

        // Inclinación
        float objetivoZ = -dir.x * inclinacionMaxima;
        float actualZ  = transform.rotation.eulerAngles.z;
        if (actualZ > 180) actualZ -= 360f;
        float suavizadoZ = Mathf.LerpAngle(actualZ, objetivoZ, suavizadoInclinacion * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, suavizadoZ);

        // Estelas
        ControlarEstelas(dir);

        // Girar timón
        if (timonypumba != null)
        {
            float giro = dir.x * velocidadGiroTimon * Time.fixedDeltaTime;
            timonypumba.transform.Rotate(0, 0, -giro);
        }

        // Prueba de daño con H
        if (Keyboard.current.hKey.wasPressedThisFrame)
            RecibirDanio(10);
    }

    #region Guardado y Carga
    public void GuardarDatos()
    {
        PlayerPrefs.SetInt("VidaGuardada", vidaActual);
        PlayerPrefs.SetFloat("PosicionX", transform.position.x);
        PlayerPrefs.SetFloat("PosicionY", transform.position.y);
        PlayerPrefs.SetFloat("PosicionZ", transform.position.z);
        PlayerPrefs.Save();
        Debug.Log($"[SAVE] Posición: {transform.position} | Vida: {vidaActual}");
    }

    public void CargarDatos()
    {
        if (PlayerPrefs.HasKey("VidaGuardada"))
        {
            vidaActual = PlayerPrefs.GetInt("VidaGuardada");
            ActualizarBarraVida();

            float x = PlayerPrefs.GetFloat("PosicionX");
            float y = PlayerPrefs.GetFloat("PosicionY");
            float z = PlayerPrefs.GetFloat("PosicionZ");
            transform.position = new Vector3(x, y, z);

            Debug.Log($"[LOAD] Posición: {transform.position} | Vida: {vidaActual}");
        }
        else
        {
            ActualizarBarraVida();
            Debug.Log("No se encontraron datos guardados. Empezando con valores por defecto.");
        }
    }
    #endregion

    #region Estelas
    void ControlarEstelas(Vector2 mov)
    {
        ActivarEstela(estelaCentral);
        if (mov.x < -0.1f) ActivarEstela(estelaIzquierda); else DesactivarEstela(estelaIzquierda);
        if (mov.x >  0.1f) ActivarEstela(estelaDerecha);  else DesactivarEstela(estelaDerecha);
    }

    void ActivarEstela(ParticleSystem ps)   { if (ps != null && !ps.isPlaying) ps.Play(); }
    void DesactivarEstela(ParticleSystem ps){ if (ps != null && ps.isPlaying)  ps.Stop(); }
    void DetenerTodasLasEstelas()           { DesactivarEstela(estelaCentral); DesactivarEstela(estelaIzquierda); DesactivarEstela(estelaDerecha); }
    #endregion

    #region Vida y Game Over
    void ActualizarBarraVida()
    {
        if (barraVida != null)
            barraVida.value = vidaActual;
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual = Mathf.Clamp(vidaActual - cantidad, 0, vidaMaxima);
        ActualizarBarraVida();
        if (vidaActual <= 0)
            Morir();
    }

    void Morir()
    {
        Debug.Log("¡Jugador destruido!");
        if (gameOverUI != null)
            gameOverUI.ShowGameOver();

        // Si quieres puedes desactivar tu script de movimiento:
        // enabled = false;
    }
    #endregion

    /// <summary>
    /// Útil para sliders u otros controles que manden un float
    /// </summary>
    public void AplicarEntradaManual(float movimientoX)
    {
        entradaMovimiento.x = Mathf.Clamp(movimientoX / velocidadGiroTimon, -1f, 1f);
    }
}
