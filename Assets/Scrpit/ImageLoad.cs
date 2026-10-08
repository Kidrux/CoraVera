using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImageLoad : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        string sceneName = PlayerPrefs.GetString("NextScene");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            Debug.Log("Cargando: " + operation.progress);
            yield return null;
        }
    }
}