using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Necesario para manejar las escenas
using UnityEngine;
using UnityEngine.InputSystem;

public class ControladorDePausa : MonoBehaviour
{
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;

    private bool juegoPausado = false;
    private PlayerInput playerInput;
    private InputAction pausaAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        pausaAction = playerInput.actions["Pausar"]; // Define una acción "Pausar" en el Input System
    }

    private void OnEnable()
    {
        pausaAction.performed += _ => AlternarPausa();
    }

    private void OnDisable()
    {
        pausaAction.performed -= _ => AlternarPausa();
    }

    private void AlternarPausa()
    {
        juegoPausado = !juegoPausado;
        Time.timeScale = juegoPausado ? 0f : 1f;
        botonPausa.SetActive(!juegoPausado);
        menuPausa.SetActive(juegoPausado);
    }
}
