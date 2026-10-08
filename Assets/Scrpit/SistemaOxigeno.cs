using UnityEngine;
using UnityEngine.UI;

public class SistemaOxigeno : MonoBehaviour
{
    [Header("Configuración de Oxígeno")]
    [SerializeField] private float oxigenoMaximo = 100f;
    private float oxigenoActual;

    [SerializeField] private float velocidadConsumo = 5f;

    [Header("Configuración de Asfixia")]
    [SerializeField] private float danoPorAsfixia = 10f;

    [Header("Interfaz de Usuario (UI)")]
    [SerializeField] private Image barraOxigeno;

    private PlayerHealth vidaJugador;

    private void Start()
    {
        oxigenoActual = oxigenoMaximo;
        ActualizarInterfaz();

        vidaJugador = GetComponent<PlayerHealth>();

        if (vidaJugador == null)
        {
            Debug.LogError("No se encontró el script PlayerHealth en el jugador. Asegúrate de que estén en el mismo GameObject.");
        }
    }

    private void Update()
    {
        if (oxigenoActual > 0)
        {
            oxigenoActual -= velocidadConsumo * Time.deltaTime;

            if (oxigenoActual < 0)
            {
                oxigenoActual = 0;
            }

            ActualizarInterfaz();
        }
        else
        {
            AplicarDanoAsfixia();
        }
    }

    private void ActualizarInterfaz()
    {
        if (barraOxigeno != null)
        {
            barraOxigeno.fillAmount = oxigenoActual / oxigenoMaximo;
        }
    }

    public void RecargarOxigeno(float cantidad)
    {
        oxigenoActual += cantidad;

        if (oxigenoActual > oxigenoMaximo)
        {
            oxigenoActual = oxigenoMaximo;
        }

        ActualizarInterfaz();
        Debug.Log("¡Oxígeno recuperado!");
    }

    private void AplicarDanoAsfixia()
    {
        if (vidaJugador != null)
        {
            vidaJugador.TakeDamage(danoPorAsfixia * Time.deltaTime);
            Debug.LogWarning("¡Te estás asfixiando! Perdiendo vida...");
        }
    }
}