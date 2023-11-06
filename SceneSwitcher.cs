using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string sceneToLoad;

    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(() => LoadScene(sceneToLoad));
        }
        else
        {
            Debug.LogError("Button not assigned in the Inspector!");
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
