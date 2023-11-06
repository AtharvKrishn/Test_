using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class FhirResourceLoader : MonoBehaviour
{
    public static FhirResourceLoader Instance;

    private string baseUrl = "https://hapi.fhir.org/baseR4/";
    public List<ObservationData> observations = new List<ObservationData>();

    public event System.Action ObservationsLoaded;

    [System.Serializable]
    public class ObservationData
    {
        public string observationCode;
        public float valueQuantity;
        public string valueUnit;
        public string observationId;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            System.IO.File.WriteAllText(logFilePath, string.Empty);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(FetchResources());
    }

    public List<ObservationData> GetObservations()
    {
        return observations;
    }

    private IEnumerator FetchResources()
    {
        string patientId = PlayerPrefs.GetString("SelectedPatientId");
        if (string.IsNullOrEmpty(patientId))
        {
            Debug.LogError("Patient ID not found in PlayerPrefs.");
            yield break;
        }

        string url = $"{baseUrl}Observation?subject=Patient/{patientId}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                ProcessResourceData(webRequest.downloadHandler.text);
            }
        }
    }

    private void ProcessResourceData(string jsonData)
    {
        try
        {
            JObject response = JObject.Parse(jsonData);
            JArray entries = response["entry"] as JArray;

            if (entries != null)
            {
                foreach (JObject entry in entries)
                {
                    JObject resource = entry["resource"] as JObject;
                    if (resource != null && resource["code"] != null && resource["code"]["coding"] != null && resource["valueQuantity"] != null)
                    {
                        ObservationData observation = new ObservationData
                        {
                            observationCode = (resource["code"]["coding"][0]["code"] != null) ? resource["code"]["coding"][0]["code"].ToString() : "Unknown Code",
                            valueQuantity = (resource["valueQuantity"]["value"] != null) ? (float)resource["valueQuantity"]["value"] : 0f,
                            valueUnit = (resource["valueQuantity"]["unit"] != null) ? resource["valueQuantity"]["unit"].ToString() : "Unknown Unit",
                            observationId = (resource["id"] != null) ? resource["id"].ToString() : "Unknown ID"
                        };

                        Debug.Log($"Observation ID: {observation.observationId}, Code: {observation.observationCode}, Value: {observation.valueQuantity}");

                        observations.Add(observation);
                    }
                }
            }
            ObservationsLoaded?.Invoke();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to process resource data: " + e.Message);
        }
    }
    private string logFilePath = "UnityConsoleLogs.txt"; // Name of the file to save the logs

    void OnEnable()
    {
        // Redirect Unity's default log output to our custom handler
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Restore the original log handler when this object is destroyed
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Save the log message to the specified file
        System.IO.File.AppendAllText(logFilePath, logString + "\n");

    }

}
