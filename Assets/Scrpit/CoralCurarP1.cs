using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralCurarP1 : MonoBehaviour
{
    public GameObject interactMessage;
    public GameObject holdMessage;

    public CameraMoveToTarget cameraScript;
    public Transform cameraPoint;
    public Transform playerCameraPoint;

    public MonoBehaviour playerMovement;

    public float holdTimeRequired = 5f;

    private float holdTimer = 0f;
    private bool playerInRange = false;
    private bool isInteracting = false;
    private bool isHolding = false;
    public GameObject indicatorLight;
    private bool isCompleted = false;

    private bool misionActivada = false;

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

        if (playerInRange && !isInteracting && Input.GetKeyDown(KeyCode.E))
        {
            interactMessage.SetActive(false);

            if (cameraScript != null)
            {
                cameraScript.targetPosition = cameraPoint;
                cameraScript.MoveCamera();
            }

            if (playerMovement != null)
                playerMovement.enabled = false;

            holdMessage.SetActive(true);
            isInteracting = true;
        }

        if (isInteracting && Input.GetKey(KeyCode.B))
        {
            isHolding = true;
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTimeRequired)
            {
                CompleteHealing();
            }
        }
        else if (isInteracting)
        {
            isHolding = false;
            holdTimer = 0f;
        }

        if (isInteracting && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitInteraction();
        }
    }

    void CompleteHealing()
    {
        Debug.Log("Coral curado");

        PartidaManager.Instance.RegistrarCoralCurado();

        isCompleted = true;

        if (indicatorLight != null)
            indicatorLight.SetActive(false);

        if (interactMessage != null)
            interactMessage.SetActive(false);

        if (holdMessage != null)
            holdMessage.SetActive(false);

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        ExitInteraction();

        this.enabled = false;
    }

    void ExitInteraction()
    {
        if (cameraScript != null)
        {
            cameraScript.targetPosition = playerCameraPoint;
            cameraScript.MoveCamera();
        }

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (holdMessage != null)
            holdMessage.SetActive(false);

        holdTimer = 0f;
        isInteracting = false;

        if (playerInRange && !isCompleted && interactMessage != null)
        {
            interactMessage.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!misionActivada)
            return;

        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!isInteracting)
                interactMessage.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactMessage != null)
                interactMessage.SetActive(false);
        }
    }
}