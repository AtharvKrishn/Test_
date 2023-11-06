using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text; // Make sure you have this using directive

public class JointInteractionHandler : MonoBehaviour
{
    private string baseUrl = "https://hapi.fhir.org/baseR4/";
    public Button updateButton;
    [System.Serializable]
    public class JointInfo
    {
        public Transform jointTransform;
        public InputField inputField;
        public float lowerLimit = 0f;
        public float upperLimit = 100f;
        public Material jointMaterial;

        public void UpdateJoint(float inputValue)
        {
            // Calculate the rotation angle based on inputValue and apply it relative to the current rotation


            // Updating color based on input value
            UpdateJointColor(inputValue);
        }


        private void UpdateJointColor(float result)
        {
            float normalizedResult = Mathf.Clamp01((result - lowerLimit) / (upperLimit - lowerLimit));
            jointMaterial.color = Color.Lerp(Color.red, Color.green, normalizedResult);
        }
    }

    public List<JointInfo> joints;
    public Transform modelTransform;
    public Camera mainCamera;
    public TextMeshProUGUI patientNameText;

    void Start()
    {
        foreach (JointInfo joint in joints)
        {
            joint.inputField.gameObject.SetActive(false);
            joint.jointMaterial = joint.jointTransform.GetComponent<Renderer>().material;
            joint.inputField.onValueChanged.AddListener(delegate { InputFieldValueChanged(joint); });
        }

        FhirResourceLoader.Instance.ObservationsLoaded += OnObservationsLoaded;



        StartCoroutine(FetchPatientData());

        if (updateButton != null)
        {
            updateButton.onClick.AddListener(OnUpdateButtonClick);
        }
        else
        {
            Debug.LogError("Update Button is not assigned in the inspector!");
        }
    }

    private void OnDestroy()
    {
        if (FhirResourceLoader.Instance != null)
        {
            FhirResourceLoader.Instance.ObservationsLoaded -= OnObservationsLoaded;
        }
    }



    private void RotateModel()
    {
        float rotationSpeed = 10f;
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        //float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;
        modelTransform.Rotate(Vector3.up, mouseX, Space.World);
        //modelTransform.Rotate(Vector3.right, mouseY, Space.World);
    }

    private void CheckJointSelection()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform hitTransform = hit.transform;
            foreach (JointInfo joint in joints)
            {
                if (hitTransform == joint.jointTransform)
                {
                    bool wasActive = joint.inputField.gameObject.activeSelf;
                    joint.inputField.gameObject.SetActive(!wasActive); // Toggle visibility

                    // If the input field was not active before, activate it now
                    if (!wasActive)
                    {
                        joint.inputField.ActivateInputField();
                    }

                    break;
                }
            }
        }
    }

    private void InputFieldValueChanged(JointInfo joint)
    {
        if (float.TryParse(joint.inputField.text, out float inputValue))
        {
            joint.UpdateJoint(inputValue);
        }
        else
        {
            Debug.LogError("Invalid input! Please enter a valid number.");
        }
    }

    private void OnObservationsLoaded()
    {
        List<FhirResourceLoader.ObservationData> observations = FhirResourceLoader.Instance.GetObservations();
        foreach (var observation in observations)
        {
            if (observation.observationCode == "Neck")
            {
                PlayerPrefs.SetString("SelecteNeck", observation.observationId);
                JointInfo joint = joints[0];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "RShoulder")
            {
                JointInfo joint = joints[1];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "LShoulder")
            {
                JointInfo joint = joints[2];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "RElbow")
            {
                JointInfo joint = joints[3];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "LElbow")
            {
                JointInfo joint = joints[4];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "LWrist")
            {
                JointInfo joint = joints[5];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "RWrist")
            {
                JointInfo joint = joints[6];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "Spine")
            {
                JointInfo joint = joints[7];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "Chest")
            {
                JointInfo joint = joints[8];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "Hip")
            {
                JointInfo joint = joints[9];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "LKnee")
            {
                JointInfo joint = joints[10];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "RKnee")
            {
                JointInfo joint = joints[11];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "LAnkle")
            {
                JointInfo joint = joints[12];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
            if (observation.observationCode == "RAnkle")
            {
                JointInfo joint = joints[13];
                joint.inputField.text = observation.valueQuantity.ToString();
                joint.UpdateJoint(observation.valueQuantity);
                joint.inputField.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator FetchPatientData()
    {
        string patientId = PlayerPrefs.GetString("SelectedPatientId");
        if (string.IsNullOrEmpty(patientId))
        {
            Debug.LogError("Patient ID not found in PlayerPrefs.");
            yield break;
        }

        string url = baseUrl + "Patient/" + patientId;

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
                try
                {
                    JObject response = JObject.Parse(responseData);
                    string patientName = "Unknown";
                    if (response["name"] != null && response["name"].Type == JTokenType.Array)
                    {
                        JArray names = (JArray)response["name"];
                        if (names.Count > 0)
                        {
                            JObject name = (JObject)names[0];
                            if (name["given"] != null && name["given"].Type == JTokenType.Array)
                            {
                                patientName = string.Join(" ", name["given"].ToObject<string[]>());
                            }
                            if (name["family"] != null)
                            {
                                patientName += " " + name["family"].ToString();
                            }
                        }
                    }

                    patientNameText.text = "" + patientName + " (" + patientId + ")";
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed to parse patient data: " + e.Message);
                }
            }
        }
    }
    public void OnSubmitButtonClicked()
    {
        string dataToSave = "Joint Data:\n";

        foreach (JointInfo joint in joints)
        {
            dataToSave += $"Joint Name: {joint.jointTransform.name}, Value: {joint.inputField.text}\n";
        }

        string fileName = "jointData.txt";
        System.IO.File.WriteAllText(fileName, dataToSave);

        Debug.Log($"Data saved to {fileName}");
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RotateModel();
        }

        if (Input.GetMouseButtonDown(0))
        {
            CheckJointSelection();
        }

        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            HideInputFields();
        }
    }

    // Method to hide all input fields
    private void HideInputFields()
    {
        foreach (JointInfo joint in joints)
        {
            joint.inputField.gameObject.SetActive(false);
        }
    }
    public void OnUpdateButtonClick()
    {
        Debug.Log("Update button clicked.  ");
        string pId = PlayerPrefs.GetString("SelectedPatientId");
        string txt = "0";
        string oID = "bogus";
        string c = "xoxo";

        List<FhirResourceLoader.ObservationData> observations = FhirResourceLoader.Instance.GetObservations();
        if (observations.Count == 0)
        {
            string patientJson = (ConstructData(pId));
            StartCoroutine(PostPatientData(patientJson));
            //PostPatientData(ConstructData(pId));
            //Debug.Log(patientJson);
            return;
        }
        foreach (var observation in observations)
        {

            if (observation.observationCode == "Neck")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[0];
                txt = joint.inputField.text;

            }
            else if (observation.observationCode == "RShoulder")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[1];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "LShoulder")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[2];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "RElbow")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[3];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "LElbow")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[4];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "LWrist")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[5];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "RWrist")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[6];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "Spine")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[7];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "Chest")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[8];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "Hip")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[9];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "LKnee")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[10];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "RKnee")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[11];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "LAnkle")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[12];
                txt = joint.inputField.text;
            }
            else if (observation.observationCode == "RAnkle")
            {
                oID = observation.observationId;
                c = observation.observationCode;
                JointInfo joint = joints[13];
                txt = joint.inputField.text;
            }
            else
            {

            }

            Debug.Log($"Observation ID: {observation.observationId}, Code: {observation.observationCode}, Code: {observation.valueQuantity}, joint : {txt}");
            string observationId = oID;
            string patientId = pId;
            float value = float.Parse(txt); ;
            string ocode = c;

            if (ocode == "Neck" || ocode == "RAnkle" || ocode == "LAnkle" || ocode == "RKnee" || ocode == "LKnee" ||
                 ocode == "Hip" || ocode == "Chest" || ocode == "Spine" || ocode == "RWrist" || ocode == "LWrist" ||
                 ocode == "LElbow" || ocode == "RElbow" || ocode == "LShoulder" || ocode == "RShoulder")
            {
                string json = CreateObservationJson(observationId, patientId, value, ocode);
                StartCoroutine(SendObservationData(json, observationId));
            }
            else
            {
                Debug.Log("New");
            }


        }

    }


    private string ConstructData(string pId)
    {
        string patientReference = "Patient/" + pId;
        string[] obsCodes = new string[]
        {
        "Neck", "RShoulder", "LShoulder", "RElbow", "LElbow",
        "RWrist", "LWrist", "Spine", "Chest", "Hip",
        "LKnee", "RKnee", "LAnkle", "RAnkle"
        };

        List<string> entries = new List<string>();

        for (int i = 0; i < obsCodes.Length; i++)
        {
            // Attempt to parse the text value into a float
            if (!float.TryParse(joints[i].inputField.text, out float value))
            {
                value = 0f; // Default to 0 if parsing fails
            }

            // Create the JSON entry for the current observation
            entries.Add($@"
            {{
                ""resource"": {{
                    ""resourceType"": ""Observation"",
                    ""status"": ""final"",
                    ""code"": {{
                        ""coding"": [
                            {{
                                ""system"": ""http://example.com/observation-code"",
                                ""code"": ""{obsCodes[i]}"",
                                ""display"": ""{obsCodes[i]}""
                            }}
                        ]
                    }},
                    ""subject"": {{
                        ""reference"": ""{patientReference}""
                    }},
                    ""valueQuantity"": {{
                        ""value"": {value},
                        ""unit"": ""degrees"",
                        ""system"": ""http://unitsofmeasure.org"",
                        ""code"": ""deg""
                    }}
                }},
                ""request"": {{
                    ""method"": ""POST"",
                    ""url"": ""Observation""
                }}
            }}");
        }

        string jsonBundle = $@"
    {{
        ""resourceType"": ""Bundle"",
        ""type"": ""transaction"",
        ""entry"": [
            {string.Join(",", entries)}
        ]
    }}";

        return jsonBundle.Replace("\r\n", " ").Replace("\n", " ").Trim();
    }





    private string CreateObservationJson(string observationId, string patientId, float value, string ocode)
    {
        var observation = new JObject
        {
            ["resourceType"] = "Observation",
            ["id"] = observationId,
            ["status"] = "final",
            ["code"] = new JObject
            {
                ["coding"] = new JArray
                {
                    new JObject
                    {
                        ["system"] = "http://example.com/observation-code",
                        ["code"] = ocode,

                    }
                }
            },
            ["subject"] = new JObject
            {
                ["reference"] = $"Patient/{patientId}"
            },
            ["valueQuantity"] = new JObject
            {
                ["value"] = value,
                ["unit"] = "degrees",
                ["system"] = "http://unitsofmeasure.org",
                ["code"] = "deg"
            }
        };

        return observation.ToString();
    }

    private IEnumerator SendObservationData(string json, string observationId)
    {
        string url = $"https://hapi.fhir.org/baseR4/Observation/{observationId}";
        int retries = 0;
        float baseDelay = 1.0f; // Start with a 1 second delay

        while (retries < 5) // Limit the number of retries
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Put(url, json))
            {
                webRequest.SetRequestHeader("Content-Type", "application/fhir+json");
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    if (webRequest.responseCode == 429) // Too many requests
                    {
                        retries++;
                        float delay = baseDelay * Mathf.Pow(2, retries); // Exponential backoff
                        Debug.LogWarning($"Request limit reached, retrying after {delay} seconds...");
                        yield return new WaitForSeconds(delay);
                    }
                    else
                    {
                        Debug.LogError(webRequest.error);
                        break; // Break out of the loop for other types of errors
                    }
                }
                else
                {
                    Debug.Log("Observation data updated successfully.");
                    break; // Success! Break out of the loop
                }
            }
        }
    }
    private IEnumerator PostPatientData(string jsonPayload)
    {
        Debug.Log("code is here");
        string url = "https://hapi.fhir.org/baseR4/";

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
            }
        }
    }


}