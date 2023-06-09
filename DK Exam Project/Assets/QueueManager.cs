using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public List<GameObject> patients;
    public GameObject Prefab;
    public GameObject simulator;
    public float speed; 

    // Start is called before the first frame update
    void Start()
    {
        SetQueuePos();
        speed = simulator.GetComponent<Simulation>().speed;
    }
    private void Update()
    {
        speed = simulator.GetComponent<Simulation>().speed;
    }

    /// <summary>
    /// Tar bort den första i kön!
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
        GameObject newPatient = Instantiate(Prefab, gameObject.transform);
        //Get the Patient Script
        PatientScript script = newPatient.GetComponent<PatientScript>();
        script.Created(priority, totalTime, material);

        if (patients.Count == 0)
        {
            patients.Add(newPatient);
            Debug.Log("Added when empty");
            return;
        }

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
            PatientScript script = patients[i].GetComponent<PatientScript>();
            script.UpdateIndex(i);
        }
    }
}
