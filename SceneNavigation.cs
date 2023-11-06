using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    void Start()
    {
        // Make this GameObject persistent across scene changes
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Check if the user presses the back key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    private void GoBack()
    {
        // Determine the current scene
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Scene2")
        {
            // Load Patient Search Scene
            SceneManager.LoadScene("Scene");
        }
        else if (currentScene.name == "Scene1")
        {
            // Load Login Scene
            SceneManager.LoadScene("Scene");
        }
        else if (currentScene.name == "Scene") 
        {
            // Load Login Scene
            SceneManager.LoadScene("New Scene");
        }
        else if (currentScene.name == "New Scene")
        {
            // Exit the application
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
