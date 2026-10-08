using UnityEngine;
using System.Collections; 
using TMPro;
using UnityEngine.Events;

public class InteractionSystem : MonoBehaviour
{
    public GameObject interactMessage;

    public CameraMoveToTarget cameraScript;
    public Transform targetCameraPoint;
    public Transform playerCameraPoint;

    public MonoBehaviour playerMovement;

    public GameObject objectToActivate; 
    public GameObject textUI; 
    public TextMeshProUGUI dialogueText; 

    [TextArea(3, 5)] 
    public string[] dialogues; 

    [Header("Configuración de Texto")]
    [Tooltip("Tiempo de espera entre cada letra (en segundos)")]
    public float typingSpeed = 0.04f; 

    private int currentDialogueIndex = 0; 
    private bool playerRange = false;
    private bool Interacting = false;
    
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    [Header("Eventos de Misión")]
    [Tooltip("Eventos que se ejecutarán al interactuar")]
    public UnityEvent onInteractionStart;

    void Update()
    {

        if (playerRange && !Interacting && Input.GetKeyDown(KeyCode.E))
        {
            StartInteraction();
        }
        else if (Interacting && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                CompleteTextImmediately();
            }
            else
            {
                AdvanceDialogue();
            }
        }

        if (Interacting && Input.GetKeyDown(KeyCode.Escape))
        {
            EndInteraction();
        }
    }

    public void StartInteraction()
    {
        Interacting = true;
        interactMessage.SetActive(false);

        if (cameraScript != null && targetCameraPoint != null)
        {
            cameraScript.targetPosition = targetCameraPoint;
            cameraScript.MoveCamera();
        }

        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        if (textUI != null)
            textUI.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        currentDialogueIndex = 0;
        StartTypingDialogue();
        onInteractionStart.Invoke();
    }

    void AdvanceDialogue()
    {
        currentDialogueIndex++; 

        if (currentDialogueIndex >= dialogues.Length)
        {
            EndInteraction();
        }
        else
        {
            StartTypingDialogue();
        }
    }

    void StartTypingDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (dialogues.Length > 0 && dialogueText != null)
        {
            typingCoroutine = StartCoroutine(TypeText(dialogues[currentDialogueIndex]));
        }
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = ""; 

        foreach (char letter in textToType.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void CompleteTextImmediately()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        
        dialogueText.text = dialogues[currentDialogueIndex]; 
        isTyping = false;
    }

    void EndInteraction()
    {
        Debug.Log("Fin de la interacción");

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping = false;

        if (cameraScript != null && playerCameraPoint != null)
        {
            cameraScript.targetPosition = playerCameraPoint;
            cameraScript.MoveCamera();
        }

        if (objectToActivate != null)
            objectToActivate.SetActive(false);

        if (textUI != null)
            textUI.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        Interacting = false;
        
        if (playerRange)
        {
            interactMessage.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = true;
            if (!Interacting)
                interactMessage.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = false;
            if (!Interacting)
                interactMessage.SetActive(false);
        }
    }
}