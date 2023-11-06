using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class FHIRPatientSender : MonoBehaviour
{
    // Reference to the input fields
    public TMP_InputField patientNameInputField;
    public TMP_InputField ageInputField;
    public TMP_InputField bloodTypeInputField;
    public TMP_InputField genderInputField;
    public string modelSceneName = "Scene2"; // Assign the name of your 3D model scene here
    private const string serverBaseUrl = "https://hapi.fhir.org/baseR4";
    private string new_id; // This will store the new patient ID

    // This method should be called when the create patient button is clicked
    public void OnCreatePatientButtonClicked()
    {
        // You may want to validate input fields before sending them
        string patientJson = ConstructPatientJson();
        StartCoroutine(PostPatientData(patientJson));
    }

    private IEnumerator PostPatientData(string jsonPayload)
    {
        string url = $"{serverBaseUrl}/Patient";

        using (UnityWebRequest webRequest = UnityWebRequest.Put(url, jsonPayload))
        {
            webRequest.method = "POST"; // Change to POST for creating new resource
            webRequest.SetRequestHeader("Content-Type", "application/fhir+json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Patient created successfully.");
                Debug.Log("Response: " + webRequest.downloadHandler.text);

                // Parse the response and extract the patient id
                var responseJson = JsonUtility.FromJson<WebResponse>(webRequest.downloadHandler.text);
                new_id = responseJson.id; // Store the new patient id
                PlayerPrefs.SetString("SelectedPatientId", new_id);
                Debug.Log("New Patient ID: " + new_id); // Display the new patient ID on the console

                // Load the 3D model scene if the patient was created successfully
                SceneManager.LoadScene(modelSceneName);
            }
        }
    }

    private string ConstructPatientJson()
    {
        // Constructs the JSON payload for creating a new Patient resource.
        string[] nameParts = patientNameInputField.text.Split(' ');
        string givenName = nameParts.Length > 0 ? nameParts[0] : "";
        string familyName = nameParts.Length > 1 ? nameParts[1] : "";
        return @"
        {
            ""resourceType"": ""Patient"",
            ""text"": {
                ""status"": ""generated"",
                ""div"": ""<div xmlns='http://www.w3.org/1999/xhtml'>" + patientNameInputField.text + @"</div>""
            },
            ""name"": [
                {
                    ""family"": """ + familyName + @""",
                    ""given"": [""" + givenName + @"""]
                }
            ],
            ""gender"": """ + genderInputField.text.ToLower() + @""",
            ""extension"": [
                {
                    ""url"": ""http://hl7.org/fhir/StructureDefinition/patient-age"",
                    ""valueAge"": {
                        ""value"": " + ageInputField.text + @",
                        ""unit"": ""years"",
                        ""system"": ""http://unitsofmeasure.org"",
                        ""code"": ""a""
                    }
                },
                {
                    ""url"": ""http://example.org/do-not-use/fhir-extensions/bloodType"",
                    ""valueString"": """ + bloodTypeInputField.text + @"""
                }
            ]
        }".Replace(System.Environment.NewLine, ""); // Remove any inadvertent newline characters that might cause issues
    }
}

// Helper class to parse the web response
[System.Serializable]
public class WebResponse
{
    public string resourceType;
    public string id;
    // Add other fields as necessary
}
