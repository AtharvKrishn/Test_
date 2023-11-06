using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Singleton instance
    public static DataManager Instance { get; private set; }

    // ObservationData class definition
    public class ObservationData
    {
        public string observationCode;
        public float valueQuantity;
        public string valueUnit;
        public string observationId;
    }

    // List to store Observation data
    public List<ObservationData> Observations { get; private set; } = new List<ObservationData>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to update Observation data
    public void UpdateObservations(List<ObservationData> newObservations)
    {
        Observations = newObservations;
    }

    // Any other methods to manage or retrieve data can be added here
}
