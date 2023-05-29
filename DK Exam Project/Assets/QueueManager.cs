using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public List<GameObject> patients;
    public List<GameObject> patientsPrefab;

    public GameObject Prefab;
    public Material standardMaterial;
    // Start is called before the first frame update
    void Start()
    {
        SetQueuePos();
    }

    // Update is called once per frame
    void Update()
    {
        //tar bort en patient manuellt
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (patients.Count == 0)
                return;
            RemovePatient();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnAPatient(1,100f, standardMaterial);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnAPatient(2,100f,standardMaterial);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnAPatient(3, 100f, standardMaterial);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnAPatient(4, 100f, standardMaterial);
        }
    }

    /// <summary>
    /// Tar bort den f�rsta i k�n!
    /// </summary>
    public void RemovePatient()
    {
        PatientScript script = patients[3].GetComponent<PatientScript>();
        script.leave = true;
        patients.RemoveAt(3);
        SetQueuePos();
    }
    public void RemovePatientAtPos(int pos)
    {
        PatientScript script = patients[pos].GetComponent<PatientScript>();
        script.leave = true;
        patients.RemoveAt(pos);
        SetQueuePos();
    }

    /// <summary>
    /// Kallas n�r en patient ska l�ggas till!
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
        //Object prefab = Resources.Load("Assets/Prefabs/Patient");

        // Choose a random prefab (animal) from the patients prefab list
        int randomIndex = Random.Range(0, patientsPrefab.Count);
        GameObject newPatient = Instantiate(patientsPrefab[randomIndex]);


        PatientScript script = newPatient.GetComponent<PatientScript>();
        script.Created(priority, totalTime,material);

        if (patients.Count == 0)
        {
            patients.Add(newPatient);
            //Debug.Log("Added when empty");
            return;
        }

        //Debug.Log(script.priority);

        for (int i = patients.Count - 1; i >= 0; i--)
        {
            if (patients[i].GetComponent<PatientScript>().priority < script.priority && i != 0)
            {
                Debug.Log("The queue one: " + patients[i].GetComponent<PatientScript>().priority + " | The new one: " + script.priority);
            }
            else
            {
                if (i == 0)
                {
                    if(patients[i].GetComponent<PatientScript>().priority < script.priority)
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
                
                //ska inte kunna g� f�rbi en med samma eller h�gre prioritet. 
                break;
            }
        }
    }

    /// <summary>
    /// Uppdaterar alla med r�tt position i k�n
    /// </summary>
    public void SetQueuePos()
    {
        for (int i = 0; i < patients.Count; i++)
        {
            PatientScript script = patients[i].GetComponent<PatientScript>();
            script.UpdateIndex(i);
        }
    }
}
