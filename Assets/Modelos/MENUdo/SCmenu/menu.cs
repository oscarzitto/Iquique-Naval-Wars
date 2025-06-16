using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Jugar()
    {
        Debug.Log("Jugar() invocado");
        Time.timeScale = 1f; // Asegura que el tiempo esté normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Carga la siguiente escena
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
        Application.Quit(); // Cierra la aplicación
    }
}
