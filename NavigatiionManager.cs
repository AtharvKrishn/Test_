using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{
    void Start()
    {
        // Get the Button component attached to this GameObject
        Button backButton = GetComponent<Button>();

        // Add an onClick listener to the button
        backButton.onClick.AddListener(GoBack);
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
