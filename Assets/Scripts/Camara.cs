using UnityEngine;

public class Camara : MonoBehaviour
{
    [SerializeField] public Transform objetivo; // El objeto que seguirá la cámara (tu jugador) // Atributo
    [SerializeField] public float limiteXMinimo; // Límite mínimo en el eje X // Atributo
    [SerializeField] public float limiteXMaximo; // Límite máximo en el eje X // Atributo
    [SerializeField] public float limiteYMinimo; // Límite mínimo en el eje Y // Atributo
    [SerializeField] public float limiteYMaximo; // Límite máximo en el eje Y // Atributo

    [SerializeField] public float smoothSpeed = 0.125f; // Velocidad de suavizado // Atributo
    [SerializeField] private Vector3 velocity = Vector3.zero; // Velocidad actual // Atributo

    void Start()
    {
        // Buscar al jugador automáticamente al inicio del juego
        objetivo = GameObject.FindGameObjectWithTag("Player").transform; // Condicional
    }

    void FixedUpdate() // Usar FixedUpdate para el seguimiento de la cámara
    {
        if (objetivo != null) // Condicional
        {
            // Obtener la posición actual del objetivo
            Vector3 posicionObjetivo = objetivo.position;

            // Limitar la posición en el eje X
            posicionObjetivo.x = Mathf.Clamp(posicionObjetivo.x, limiteXMinimo, limiteXMaximo); // Comparador

            // Limitar la posición en el eje Y
            posicionObjetivo.y = Mathf.Clamp(posicionObjetivo.y, limiteYMinimo, limiteYMaximo); // Comparador

            // Mantener la misma posición en el eje Z
            posicionObjetivo.z = transform.position.z;

            // Asignar la nueva posición a la cámara con suavizado
            transform.position = Vector3.SmoothDamp(transform.position, posicionObjetivo, ref velocity, smoothSpeed); // Propiedad
        }
    }
}
