using UnityEngine;

public class fondomivi : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento;

    private Vector2 offset;

    private Material material;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }
    
    private void Update()
    {
    offset = velocidadMovimiento * Time.deltaTime;
    material.mainTextureOffset += offset;
    }


}
