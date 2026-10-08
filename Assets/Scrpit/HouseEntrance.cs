using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseEntrance : MonoBehaviour
{
    public GameObject enterMessage;
    public string sceneToLoad;
    public string spawnPoint;
    public ScreenFade fadeManager;

    private bool playerInRange = false;
    private bool isTransitioning = false;

    void Update()
    {
       if (playerInRange && !isTransitioning && Input.GetKeyDown(KeyCode.E))
        {
            isTransitioning = true;
            SpawnManager.spawnPointName = spawnPoint;

            StartCoroutine(fadeManager.FadeOut(sceneToLoad));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            enterMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            enterMessage.SetActive(false);
        }
    }
}