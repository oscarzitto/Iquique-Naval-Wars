using UnityEngine;

public class DisparoEnemigo : MonoBehaviour
{
    public GameObject estelaPrefab;        // arrastra el prefab de la estela
    public GameObject proyectilPrefab;     // el prefab de la bala del enemigo
    public float tiempoAdvertencia = 1f;   // cuánto tiempo se muestra la estela antes del disparo
    public float tiempoEntreDisparos = 3f; // espera entre cada disparo
    public Transform puntoDisparo;         // puede ser el mismo enemigo o un hijo donde aparece el disparo

    private void Start()
    {
        InvokeRepeating(nameof(PrepararDisparo), 2f, tiempoEntreDisparos); // dispara cada cierto tiempo
    }

    void PrepararDisparo()
    {
        StartCoroutine(AdvertenciaYDisparo());
    }

    System.Collections.IEnumerator AdvertenciaYDisparo()
    {
        // Mostrar la estela visual
        GameObject estela = Instantiate(estelaPrefab, new Vector3(puntoDisparo.position.x, puntoDisparo.position.y, 0), Quaternion.identity);

        // Ajustar la estela para que se estire hacia arriba
        estela.transform.localScale = new Vector3(0.2f, 10f, 1f); // o lo que necesites según el tamaño

        yield return new WaitForSeconds(tiempoAdvertencia);

        // Desaparecer la estela
        Destroy(estela);

        // Disparar el proyectil
        Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
    }
}

