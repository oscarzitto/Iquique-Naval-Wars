using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    // Nombre exacto de tu escena de menú principal
    [SerializeField] string menuSceneName = "MenuPrincipal";

    // Método a enlazar en el OnClick de VolverButton
    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
