using UnityEngine;
using System.Collections;

public class DisparoEnemigo : MonoBehaviour
{
    public GameObject estelaPrefab;         // Arrastra el prefab de la estela
    public GameObject proyectilPrefab;      // El prefab de la bala del enemigo
    public float tiempoAdvertencia = 1f;      // Tiempo que se muestra la estela antes del disparo
    public float tiempoEntreDisparos = 3f;    // Tiempo inicial entre cada disparo
    public Transform puntoDisparo;          // Punto desde el cual se disparan la estela y el proyectil

    // Valores para el decremento del tiempoEntreDisparos
    private float tiempoMinimoEntreDisparos = 1f;
    private float decrementoPorIntervalo = 0.5f;

    private void Start()
    {
        // Iniciamos dos coroutines: una para gestionar la secuencia de disparos y otra para actualizar el intervalo
        StartCoroutine(DisparoLoop());
        StartCoroutine(ActualizarTiempoEntreDisparos());
    }

    IEnumerator DisparoLoop()
    {
        // Delay inicial, ajustable según convenga
        yield return new WaitForSeconds(2f);
        while (true)
        {
            // Ejecuta la secuencia de advertencia y disparo
            yield return StartCoroutine(AdvertenciaYDisparo());
            // Espera según el intervalo actual (que se actualiza dinámicamente)
            yield return new WaitForSeconds(tiempoEntreDisparos);
        }
    }

    IEnumerator AdvertenciaYDisparo()
    {
        // Instanciamos la estela visual para avisar del disparo
        GameObject estela = Instantiate(estelaPrefab, puntoDisparo.position, Quaternion.identity);
        // La estela sigue al punto de disparo
        estela.transform.SetParent(puntoDisparo);

        // Esperamos el tiempo de advertencia
        yield return new WaitForSeconds(tiempoAdvertencia);

        // Destruimos la estela y disparamos el proyectil
        Destroy(estela);
        Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
    }

    IEnumerator ActualizarTiempoEntreDisparos()
    {
        // Cada 20 segundos reducimos el tiempoEntreDisparos en 0.5, hasta un mínimo de 1 segundo.
        while (tiempoEntreDisparos > tiempoMinimoEntreDisparos)
        {
            yield return new WaitForSeconds(20f);  // Aquí se cambia de 30 a 20 segundos
            tiempoEntreDisparos = Mathf.Max(tiempoMinimoEntreDisparos, tiempoEntreDisparos - decrementoPorIntervalo);
            Debug.Log("Nuevo intervalo entre disparos: " + tiempoEntreDisparos);
        }
    }
}
