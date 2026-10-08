using UnityEngine;

public class CheckpointOxigeno : MonoBehaviour
{
    [Tooltip("Cantidad de oxígeno que devuelve este checkpoint")]
    [SerializeField] private float cantidadARecuperar = 50f;

    [Tooltip("¿Se destruye el checkpoint al usarlo?")]
    [SerializeField] private bool destruirAlUsar = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SistemaOxigeno sistemaOxigeno = other.GetComponent<SistemaOxigeno>();

            if (sistemaOxigeno != null)
            {
                sistemaOxigeno.RecargarOxigeno(cantidadARecuperar);

                if (destruirAlUsar)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}