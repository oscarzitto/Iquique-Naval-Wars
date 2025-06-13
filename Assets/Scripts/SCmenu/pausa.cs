using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;  // necesario para SceneManager

public class ControladorDePausa : MonoBehaviour
{
    [Header("UI")]
    public GameObject menuPausa;

    [Header("Escena")]
    [Tooltip("Nombre exacto de la escena de menú tal como está en Build Settings")]
    public string nombreEscenaMenu = "Menu";

    private bool juegoPausado = false;
    private InputAction pausarInput;

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

    // Ahora guarda antes de cargar el menú
    public void VolverAlMenu()
    {
        Time.timeScale = 1f;                       // desbloqueamos el tiempo
        GameManager.Instance.SaveGameState();      // <— guardamos aquí el estado
        SceneManager.LoadScene(nombreEscenaMenu, 
                               LoadSceneMode.Single);
    }
}
