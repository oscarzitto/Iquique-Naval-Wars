using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menu : MonoBehaviour 
{
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void Salir()
    {
        Debug.Log("saliendo del juego");
        Application.Quit();
    }
}