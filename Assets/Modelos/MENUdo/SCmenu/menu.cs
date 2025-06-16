using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Jugar()
    {
        Debug.Log("Jugar() invocado");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Opciones()
    {
        Debug.Log("Opciones() invocado");
        SceneManager.LoadScene("Opciones");
    }

    public void Creditos()
    {
        Debug.Log("Creditos() invocado");
        SceneManager.LoadScene("Creditos");
    }

    public void Tutorial()
    {
        Debug.Log("Tutorial() invocado");
        SceneManager.LoadScene("Tutorial");
    }

    public void Salir()
    {
        Debug.Log("Salir() invocado");
        Application.Quit();
    }

    public void ReiniciarPartida()
    {
        Debug.Log("ReiniciarPartida() invocado");

        // 1) Tiempo normal por si estaba en pausa
        Time.timeScale = 1f;

        // 2) Borrar datos guardados
        PlayerPrefs.DeleteKey("VidaGuardada");
        PlayerPrefs.DeleteKey("PosicionX");
        PlayerPrefs.DeleteKey("PosicionY");
        PlayerPrefs.DeleteKey("PosicionZ");
        PlayerPrefs.Save();

        // 3) Recargar esta escena del menú
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
