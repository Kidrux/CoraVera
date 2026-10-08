using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractionBuzo : MonoBehaviour
{
    public GameObject interactMessage;

    public CameraMoveToTarget cameraScript;
    public Transform targetCameraPoint;
    public Transform playerCameraPoint;

    public MonoBehaviour playerMovement;

    public GameObject objectToActivate;
    public GameObject textUI;
    public TextMeshProUGUI dialogueText;

    [Header("Paneles y Botones de Opción")]
    public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;

    [Header("Cambio de Escena")]
    public string nextSceneName;

    [Header("Diálogos por Fases")]
    [TextArea(2, 4)] public string[] initialDialogues;
    [TextArea(2, 4)] public string[] repeatDialogues;
    [TextArea(2, 4)] public string[] finalDialogues;

    [Header("Configuración de Texto")]
    public float typingSpeed = 0.04f;

    [Header("Eventos de Misión")]
    public UnityEvent onInteractionStart;
    public UnityEvent onLevelComplete;

    private int currentDialogueIndex = 0;
    private int conversationStage = 1;
    private string[] currentActiveDialogues;

    private bool playerRange = false;
    private bool Interacting = false;
    private bool waitingForChoice = false;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Start()
    {
        if (yesButton != null) yesButton.onClick.AddListener(OnYesOptionSelected);
        if (noButton != null) noButton.onClick.AddListener(OnNoOptionSelected);

        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    void Update()
    {
        if (waitingForChoice)
            return;

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

        switch (conversationStage)
        {
            case 1:
                currentActiveDialogues = initialDialogues;
                break;

            case 2:
                currentActiveDialogues = repeatDialogues;
                break;

            default:
                currentActiveDialogues = finalDialogues;
                break;
        }

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

        if (currentDialogueIndex >= currentActiveDialogues.Length)
        {
            if (conversationStage == 3)
            {
                ShowChoiceButtons();
            }
            else
            {
                EndInteraction();
            }
        }
        else
        {
            StartTypingDialogue();
        }
    }

    void ShowChoiceButtons()
    {
        waitingForChoice = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (choicePanel != null)
            choicePanel.SetActive(true);
    }

    public void OnYesOptionSelected()
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);

        waitingForChoice = false;

        PartidaManager.Instance.FinalizarNivel(nextSceneName);
    }

    public void OnNoOptionSelected()
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);

        waitingForChoice = false;
        EndInteraction();
    }

    void StartTypingDialogue()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (currentActiveDialogues != null &&
            currentActiveDialogues.Length > 0 &&
            dialogueText != null)
        {
            typingCoroutine = StartCoroutine(TypeText(currentActiveDialogues[currentDialogueIndex]));
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
            StopCoroutine(typingCoroutine);

        dialogueText.text = currentActiveDialogues[currentDialogueIndex];
        isTyping = false;
    }

    void EndInteraction()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        isTyping = false;
        waitingForChoice = false;

        if (choicePanel != null)
            choicePanel.SetActive(false);

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
            interactMessage.SetActive(true);

        if (conversationStage < 3)
            conversationStage++;
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