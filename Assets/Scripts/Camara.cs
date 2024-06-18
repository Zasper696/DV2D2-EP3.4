using UnityEngine;

public class Camara : MonoBehaviour
{
    [SerializeField] public Transform objetivo; // El objeto que seguir� la c�mara (tu jugador) // Atributo
    [SerializeField] public float limiteXMinimo; // L�mite m�nimo en el eje X // Atributo
    [SerializeField] public float limiteXMaximo; // L�mite m�ximo en el eje X // Atributo
    [SerializeField] public float limiteYMinimo; // L�mite m�nimo en el eje Y // Atributo
    [SerializeField] public float limiteYMaximo; // L�mite m�ximo en el eje Y // Atributo

    [SerializeField] public float smoothSpeed = 0.125f; // Velocidad de suavizado // Atributo
    [SerializeField] private Vector3 velocity = Vector3.zero; // Velocidad actual // Atributo

    void Start()
    {
        // Buscar al jugador autom�ticamente al inicio del juego
        objetivo = GameObject.FindGameObjectWithTag("Player").transform; // Condicional
    }

    void FixedUpdate() // Usar FixedUpdate para el seguimiento de la c�mara
    {
        if (objetivo != null) // Condicional
        {
            // Obtener la posici�n actual del objetivo
            Vector3 posicionObjetivo = objetivo.position;

            // Limitar la posici�n en el eje X
            posicionObjetivo.x = Mathf.Clamp(posicionObjetivo.x, limiteXMinimo, limiteXMaximo); // Comparador

            // Limitar la posici�n en el eje Y
            posicionObjetivo.y = Mathf.Clamp(posicionObjetivo.y, limiteYMinimo, limiteYMaximo); // Comparador

            // Mantener la misma posici�n en el eje Z
            posicionObjetivo.z = transform.position.z;

            // Asignar la nueva posici�n a la c�mara con suavizado
            transform.position = Vector3.SmoothDamp(transform.position, posicionObjetivo, ref velocity, smoothSpeed); // Propiedad
        }
    }
}
