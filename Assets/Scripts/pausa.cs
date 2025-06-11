using UnityEngine;
using UnityEngine.InputSystem;

public class ControladorDePausa : MonoBehaviour
{
    public GameObject menuPausa;
    public bool juegoPausado = false;

    private InputAction pausarInput;

    private void OnEnable()
    {
        pausarInput = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
        pausarInput.performed += ctx => TogglePausa();
        pausarInput.Enable();
    }

    private void OnDisable()
    {
        pausarInput.Disable();
    }

    private void TogglePausa()
    {
        if (juegoPausado)
            Reanudar();
        else
            Pausar();
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
}
