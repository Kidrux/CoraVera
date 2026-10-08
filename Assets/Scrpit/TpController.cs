using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TpController : MonoBehaviour

{
    public string sceneToLoad;
    public GameObject interactMessage;
    private bool playerInRange = false;
    private bool isTransitioning = false;

    private bool misionActivada = false;
      private bool isCompleted = false;

    public void ActivarMisionCoral()
    {
        misionActivada = true;
        this.enabled = true;
        
        Collider col = GetComponent<Collider>();
        if (col != null && col.bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
        {
            playerInRange = true;
            interactMessage.SetActive(true);
        }
        
        Debug.Log("Misión del coral recibida y activada con éxito.");
    }

    void Update()
    {

        if (!misionActivada || isCompleted) return;

       if (playerInRange && !isTransitioning && Input.GetKeyDown(KeyCode.E))
        {
            isTransitioning = true;
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
