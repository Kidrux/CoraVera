using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Configuración de Daño")]
    public float damageToInflict = 20f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damageToInflict);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damageToInflict);
        }
    }
}