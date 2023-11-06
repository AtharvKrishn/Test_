using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine.SceneManagement;

public class PatientLoader : MonoBehaviour
{
    public GameObject patientButtonPrefab;
    public Transform contentPanel;
    public InputField searchInputField;
    public Button searchButton;
    public string anatomicalModelSceneName = "Scene2";

    private HashSet<string> loadedPatientIds = new HashSet<string>();

    void Start()
    {
        searchButton.onClick.AddListener(SearchPatient);
        StartCoroutine(LoadPatients("_count=10")); // Load initial patients
    }

    private void ClearPatientList()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        loadedPatientIds.Clear();
    }

    private IEnumerator LoadPatients(string searchParameters)
    {
        ClearPatientList();

        string baseUrl = "https://hapi.fhir.org/baseR4/Patient?";
        string url = baseUrl + searchParameters;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string responseData = webRequest.downloadHandler.text;
                Debug.Log("Server Response: " + responseData);

                JObject response = JObject.Parse(responseData);

                // Check if the response contains a single patient or a list of patients
                if (response["resourceType"] != null && response["resourceType"].ToString() == "Patient")
                {
                    ProcessSinglePatientData(responseData);
                }
                else if (response["entry"] != null)
                {
                    ProcessPatientData(responseData);
                }
                else
                {
                    Debug.LogError("Unexpected response format.");
                }
            }
        }
    }

    private void ProcessPatientData(string jsonData)
    {
        JObject response = JObject.Parse(jsonData);
        IList<JToken> patients = response["entry"].Select(token => token["resource"]).ToList();

        foreach (JToken patient in patients)
        {
            string patientId = patient["id"].ToString();
            string patientName = GetPatientName(patient);

            loadedPatientIds.Add(patientId);
            CreatePatientButton(patientId, patientName);
        }
    }

    private void ProcessSinglePatientData(string jsonData)
    {
        JObject patient = JObject.Parse(jsonData);
        string patientId = patient["id"].ToString();
        string patientName = GetPatientName(patient);

        if (!loadedPatientIds.Contains(patientId))
        {
            loadedPatientIds.Add(patientId);
            CreatePatientButton(patientId, patientName);
        }
        else
        {
            Debug.Log("Patient already loaded: " + patientName);
        }
    }

    private string GetPatientName(JToken patient)
    {
        try
        {
            string firstName = patient["name"][0]["given"][0].ToString();
            string lastName = patient["name"][0]["family"].ToString();
            return firstName + " " + lastName;
        }
        catch
        {
            return "Unknown Name";
        }
    }

    private void CreatePatientButton(string patientId, string patientName)
    {
        GameObject buttonObj = Instantiate(patientButtonPrefab, contentPanel);
        TextMeshProUGUI textComponent = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = patientName + " (" + patientId + ")";
        }

        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => PatientButtonClicked(patientId));
        }
    }

    private void PatientButtonClicked(string patientId)
    {
        Debug.Log("Patient Button Clicked: " + patientId);
        PlayerPrefs.SetString("SelectedPatientId", patientId);
        SceneManager.LoadScene(anatomicalModelSceneName);
    }

    private void SearchPatient()
    {
        string searchQuery = searchInputField.text.Trim();
        string searchParameters = string.IsNullOrEmpty(searchQuery) ? "_count=10" : "_id=" + searchQuery;
        StartCoroutine(LoadPatients(searchParameters));
    }
}
