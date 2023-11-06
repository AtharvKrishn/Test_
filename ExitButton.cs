using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    void Start()
    {
        // Get the Button component attached to this GameObject
        Button exitButton = GetComponent<Button>();

        // Add an onClick listener to the button
        exitButton.onClick.AddListener(ExitApplication);
    }

    private void ExitApplication()
    {
        // Output this to console when the Button is clicked
        Debug.Log("Exit button clicked. Application closing...");

        // Close the application
        Application.Quit();

        // If we are running in the Unity editor, stop playing the scene
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}