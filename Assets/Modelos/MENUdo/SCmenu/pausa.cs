using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControladorDePausa : MonoBehaviour
{
    [Header("UI")]
    public GameObject menuPausa;

    [Header("Escenas")]
    [Tooltip("Nombre exacto de la escena de menú tal como está en Build Settings")]
    public string nombreEscenaMenu = "Menú";

    private bool juegoPausado = false;
    private InputAction pausarInput;
    private string escenaActual;

    private void Awake()
    {
        // Guarda el nombre de la escena activa para ReiniciarPartida
        escenaActual = SceneManager.GetActiveScene().name;
    }

    private void OnEnable()
    {
        pausarInput = new InputAction(type: InputActionType.Button,
                                      binding: "<Keyboard>/escape");
        pausarInput.performed += _ => TogglePausa();
        pausarInput.Enable();
    }

    private void OnDisable()
    {
        pausarInput.performed -= _ => TogglePausa();
        pausarInput.Disable();
    }

    private void TogglePausa()
    {
        if (juegoPausado) Reanudar();
        else Pausar();
    }

    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    /// <summary>
    /// Guarda el estado y vuelve al menú principal
    /// </summary>
    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu, LoadSceneMode.Single);
    }

    /// <summary>
    /// Reinicia la partida: borra datos guardados y recarga la escena actual
    /// </summary>
    public void ReiniciarPartida()
    {
        // 1) Desbloquea el tiempo
        Time.timeScale = 1f;

        // 2) Borra las llaves de PlayerPrefs que usan tus sistemas de guardado
        PlayerPrefs.DeleteKey("VidaGuardada");
        PlayerPrefs.DeleteKey("PosicionX");
        PlayerPrefs.DeleteKey("PosicionY");
        PlayerPrefs.DeleteKey("PosicionZ");
        PlayerPrefs.Save();

        // (Opcional) Si tienes un método ResetData en tu DataManager:
        // DataManager.instance.ResetData();

        // 3) Recarga la escena donde estabas
        SceneManager.LoadScene(escenaActual, LoadSceneMode.Single);
    }
}
