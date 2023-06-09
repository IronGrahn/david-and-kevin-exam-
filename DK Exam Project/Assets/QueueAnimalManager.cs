using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueAnimalManager : MonoBehaviour
{
    [Header("Patient Management")]
    public List<GameObject> patients;
    public List<GameObject> leavingPatients;
    public List<GameObject> patientPrefabs;
    public GameObject animalSimulator;
    public float speed;

    [Header("Camera Management")]
    public AdvancedCamera advancedCamera;

    [Header("Door Settings")]
    public bool toRotate;
    public bool oldRotate;
    public GameObject entrancePosition;
    public GameObject exitPosition;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float rotationSpeed = 0.002f;
    public float t = 0.0f;
    private Quaternion initialLeftRotation;
    private Quaternion targeLeftRotation;
    private Quaternion initialRightRotation;
    private Quaternion targetRightRotation;



    void Start()
    {
        speed = animalSimulator.GetComponent<SimulationAnimal>().speed;
        oldRotate = false;
        initialLeftRotation = leftDoor.transform.rotation;
        initialRightRotation = rightDoor.transform.rotation;
        targeLeftRotation = initialLeftRotation * Quaternion.Euler(0, -100, 0);
        targetRightRotation = initialRightRotation * Quaternion.Euler(0, 40, 0);
        SetQueuePos();
    }

    void Update()
    { 

      speed = animalSimulator.GetComponent<SimulationAnimal>().speed;

        for (int i = 0; i < leavingPatients.Count; i++)
        {
            if (leavingPatients[i] != null && leavingPatients[i].GetComponent<PatientAnimalScript>().leave)
            {
                toRotate = true;
                Debug.Log("In the for-loop  : Result :" + toRotate);
                break;
            }
            else toRotate = false;
            Debug.Log("In the for-loop  : Result :" + toRotate);
        }
        if (toRotate && !oldRotate || !toRotate && oldRotate)
        {
            if (oldRotate) oldRotate= false;
            else if (!oldRotate) oldRotate = true;
            t = 0f; 
        }

        if (toRotate)
        {
            t += rotationSpeed * Time.deltaTime;
            Debug.Log("Should Rotate now!");
            leftDoor.transform.rotation = Quaternion.Lerp(initialLeftRotation, targeLeftRotation, t);
            rightDoor.transform.rotation = Quaternion.Lerp(initialRightRotation, targetRightRotation, t);
        }
        else if(!toRotate)
        {
            t += rotationSpeed * Time.deltaTime;
            Debug.Log("Should Rotate back now!");
            leftDoor.transform.rotation = Quaternion.Lerp(targeLeftRotation, initialLeftRotation, t);
            rightDoor.transform.rotation = Quaternion.Lerp(targetRightRotation, initialRightRotation, t);
        }

    }
    /// <summary>
    /// Tar bort den första i kön!
    /// </summary>
    public void RemovePatientAtPos(int pos)
    {
        PatientAnimalScript script = patients[pos].GetComponent<PatientAnimalScript>();
        script.leave = true;
        leavingPatients.Add(patients[pos]);
        patients.RemoveAt(pos);

        SetQueuePos();
    }

    /// <summary>
    /// Kallas när en patient ska läggas till!
    /// </summary>
    /// <param name="priority"></param>
    public void SpawnAPatient(int priority, float totalTime, Material material)
    {
        NewPatient(priority, totalTime, material);
        SetQueuePos();
    }

    /// <summary>
    /// Addar en ny patient med en given priority
    /// </summary>
    /// <param name="priority"></param>
    public void NewPatient(int priority, float totalTime, Material material)
    {

        //Spawn Random Character
        // GameObject newPatient = Instantiate(Prefab); //OLD
        int randomIndex = Random.Range(0, patientPrefabs.Count);
        GameObject newPatient = Instantiate(patientPrefabs[randomIndex], gameObject.transform);

        //Get the Patient Script
        PatientAnimalScript script = newPatient.GetComponent<PatientAnimalScript>();
        script.Created(priority, totalTime, material);

        if (patients.Count == 0)
        {
            patients.Add(newPatient);
            //Debug.Log("Added when empty");
            return;
        }

        //Debug.Log(script.priority);

        for (int i = patients.Count - 1; i >= 0; i--)
        {
            if (patients[i].GetComponent<PatientAnimalScript>().priority < script.priority && i != 0)
            {
                Debug.Log("The queue one: " + patients[i].GetComponent<PatientAnimalScript>().priority + " | The new one: " + script.priority);
            }
            else
            {
                if (i == 0)
                {
                    if (patients[i].GetComponent<PatientAnimalScript>().priority < script.priority)
                    {
                        patients.Insert(i, newPatient);
                    }
                    else
                    {
                        patients.Insert(i + 1, newPatient);
                    }
                }
                else
                {
                    patients.Insert(i + 1, newPatient);
                    break;
                }

                //ska inte kunna gå förbi en med samma eller högre prioritet. 
                break;
            }
        }
    }

    /// <summary>
    /// Uppdaterar alla med rätt position i kön
    /// </summary>
    public void SetQueuePos()
    {
        for (int i = 0; i < patients.Count; i++)
        {
            PatientAnimalScript script = patients[i].GetComponent<PatientAnimalScript>();
            script.UpdateIndex(i);
        }
    }

    float CalculateFieldOfView(Bounds bounds, float aspectRatio)
    {
        float objectSize = bounds.extents.magnitude;
        float distance = objectSize / Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView * 0.5f);
        float boundsWidth = bounds.size.x / aspectRatio;
        float fieldOfView = Mathf.Rad2Deg * 2f * Mathf.Atan(boundsWidth / distance);
        return fieldOfView;
    }
}
