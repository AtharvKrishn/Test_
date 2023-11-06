using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;

public class FhirResourceSubmitter : MonoBehaviour
{
    [SerializeField] private JointInteractionHandler jointInteractionHandler;
    [SerializeField] private Button submitButton;
    [SerializeField] private string serverUrl = "https://hapi.fhir.org/baseR4/";
    [SerializeField] private float requestDelay = 1f; // Delay in seconds between requests
    private string patientId;
    private void Awake()
    {
        patientId = PlayerPrefs.GetString("SelectedPatientId");
    }

    private void Start()
    {
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(() => StartCoroutine(SubmitObservations()));

        }
        else
        {
            Debug.LogError("Submit Button is not assigned!");
        }
    }

    private string GenerateJSONData()
    {
        string pog = PlayerPrefs.GetString("SelecteNeck");
        Debug.LogError(pog);
        JArray entries = new JArray();

        foreach (var jointInfo in jointInteractionHandler.joints)
        {
            string jointName = jointInfo.jointTransform.name;
            int value;
            if (int.TryParse(jointInfo.inputField.text, out value))
            {
                JObject jsonData = new JObject(
                    new JProperty("resource", new JObject(
                        new JProperty("resourceType", "Observation"),
                        new JProperty("status", "final"),
                        new JProperty("code", new JObject(
                            new JProperty("coding", new JArray(
                                new JObject(
                                    new JProperty("system", "http://example.com/observation-code"),
                                    new JProperty("code", jointName),
                                    new JProperty("display", jointName)
                                )
                            ))
                        )),
                        new JProperty("subject", new JObject(
                            new JProperty("reference", "Patient/" + patientId)
                        )),
                        new JProperty("valueQuantity", new JObject(
                            new JProperty("value", value),
                            new JProperty("unit", "degrees"),
                            new JProperty("system", "http://unitsofmeasure.org"),
                            new JProperty("code", "deg")
                        ))
                    )),
                    new JProperty("request", new JObject(
                        new JProperty("method", "PUT"),
                        new JProperty("url", "Observation/" + jointName)
                    ))
                );
                entries.Add(jsonData);
            }
            else
            {
                Debug.LogError("Invalid input for joint: " + jointName);
            }
        }

        JObject bundle = new JObject(
            new JProperty("resourceType", "Bundle"),
            new JProperty("type", "transaction"),
            new JProperty("entry", entries)
        );

        return bundle.ToString();
    }

    private IEnumerator SubmitObservations()
    {
        if (jointInteractionHandler == null)
        {
            Debug.LogError("JointInteractionHandler is not assigned!");
            yield break;
        }

        string jsonData = GenerateJSONData();
        SaveJsonToFile(jsonData);
        yield return StartCoroutine(SendDataToServer(jsonData));
    }

    private void SaveJsonToFile(string jsonData)
    {
        string path = Application.dataPath + "/GeneratedData.json";
        File.WriteAllText(path, jsonData);
        Debug.Log("Saved JSON data to: " + path);
    }

    private IEnumerator SendDataToServer(string jsonData)
    {
        UnityWebRequest www = new UnityWebRequest(serverUrl, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);
        }
    }


}
