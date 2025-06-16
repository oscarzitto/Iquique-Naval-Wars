using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [SerializeField] string menuSceneName = "MenuPrincipal";

    public void VolverAlMenu()
    {
        // --- AÑADIDO: Lógica para encontrar y guardar los datos del jugador ---
        
        // 1. Busca el script del jugador en la escena.
        MovimientoJugador jugador = FindObjectOfType<MovimientoJugador>();

        // 2. Si el script del jugador existe, llama a su función pública para guardar.
        if (jugador != null)
        {
            jugador.GuardarDatos();
        }
        else
        {
            // Este mensaje aparecerá si intentas volver al menú desde una escena donde no hay jugador.
            Debug.LogWarning("No se encontró un 'MovimientoJugador' en la escena para guardar los datos.");
        }
        
        // --- FIN DEL CÓDIGO AÑADIDO ---

        // 3. Ahora que los datos están guardados, carga la escena del menú.
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}