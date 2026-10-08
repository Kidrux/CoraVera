using UnityEngine;

public class BasuraItemController : MonoBehaviour
{
    public GameObject interactMessage;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
{
    PartidaManager.Instance.RegistrarBasura();

    if (interactMessage != null)
        interactMessage.SetActive(false);

    Destroy(gameObject, 0.1f);
}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactMessage != null)
                interactMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactMessage != null)
            
                interactMessage.SetActive(false);
            
        }
    }
}
