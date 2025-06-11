using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour 
{
    public void Jugar()
    {
        Debug.Log("Jugar() invocado");
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

    public void Salir()
    {
        Debug.Log("Salir() invocado");
        Application.Quit();
    }
}
