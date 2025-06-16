using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necesitamos esto para la lógica de guardado al salir

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
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        // --- MODIFICADO: Lógica de vida y carga de datos ---
        // 1. Establece la vida máxima por defecto
        vidaActual = vidaMaxima;
        if (barraVida != null)
        {
            barraVida.minValue = 0;
            barraVida.maxValue = vidaMaxima;
        }

        // 2. Intenta cargar los datos guardados. Esto sobreescribirá los valores por defecto si existen.
        CargarDatos();
        // --- FIN DE LA MODIFICACIÓN ---

        DetenerTodasLasEstelas();
        if (estelaCentral != null)
        {
            estelaCentral.loop = true;
            estelaCentral.playOnAwake = true;
            estelaCentral.Play();
        }
    }

    void Update()
    {
        if (usarJoystick && joystick != null)
        {
            entradaMovimiento = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
    }

    void FixedUpdate()
    {
        Vector2 dir = entradaMovimiento;

        if (ConFisicas)
        {
            rb.AddForce(dir * velocidad * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else
        {
            transform.Translate(dir * velocidad * Time.fixedDeltaTime);
        }

        if (animator != null)
            animator.SetFloat("movement", dir.magnitude);

        float objetivoZ = -dir.x * inclinacionMaxima;
        float actualZ = transform.rotation.eulerAngles.z;
        if (actualZ > 180) actualZ -= 360f;
        float suavizadoZ = Mathf.LerpAngle(actualZ, objetivoZ, suavizadoInclinacion * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, suavizadoZ);

        ControlarEstelas(dir);

        if (timonypumba != null)
        {
            float giro = dir.x * velocidadGiroTimon * Time.fixedDeltaTime;
            timonypumba.transform.Rotate(0, 0, -giro);
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
            RecibirDanio(10);
    }
    
    // --- AÑADIDO: Funciones de Guardado y Cargado ---

    public void GuardarDatos()
    {
        // Guardamos los datos actuales en el disco
        PlayerPrefs.SetInt("VidaGuardada", vidaActual);
        PlayerPrefs.SetFloat("PosicionX", transform.position.x);
        PlayerPrefs.SetFloat("PosicionY", transform.position.y);
        PlayerPrefs.SetFloat("PosicionZ", transform.position.z); // Guardamos Z por si acaso, no hace daño en 2D
        
        PlayerPrefs.Save(); // Aplica los cambios guardados
        Debug.Log("Datos del jugador guardados en Posición: " + transform.position + " y Vida: " + vidaActual);
    }

    public void CargarDatos()
    {
        // Comprobamos si hay datos guardados para evitar errores
        if (PlayerPrefs.HasKey("VidaGuardada"))
        {
            // Cargamos la vida y actualizamos la barra de vida
            vidaActual = PlayerPrefs.GetInt("VidaGuardada");
            ActualizarBarraVida();

            // Cargamos la posición
            float posX = PlayerPrefs.GetFloat("PosicionX");
            float posY = PlayerPrefs.GetFloat("PosicionY");
            float posZ = PlayerPrefs.GetFloat("PosicionZ");
            transform.position = new Vector3(posX, posY, posZ);
            
            Debug.Log("Datos del jugador cargados. Posición: " + transform.position + " | Vida: " + vidaActual);
        }
        else
        {
            // Si no hay datos, simplemente actualizamos la barra con la vida máxima inicial
            ActualizarBarraVida();
            Debug.Log("No se encontraron datos guardados. Empezando de cero.");
        }
    }

    // --- FIN DE LAS FUNCIONES AÑADIDAS ---

    void ControlarEstelas(Vector2 movimiento)
    {
        ActivarEstela(estelaCentral);
        if (movimiento.x < -0.1f) ActivarEstela(estelaIzquierda);
        else DesactivarEstela(estelaIzquierda);
        if (movimiento.x > 0.1f) ActivarEstela(estelaDerecha);
        else DesactivarEstela(estelaDerecha);
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
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarBarraVida();
        if (vidaActual <= 0) Debug.Log("¡Jugador destruido!");
    }

    public void AplicarEntradaManual(float movimientoX)
    {
        entradaMovimiento.x = Mathf.Clamp(movimientoX / velocidadGiroTimon, -1f, 1f);
    }
}