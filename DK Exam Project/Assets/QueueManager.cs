using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public List<GameObject> patients;

    public GameObject Prefab;

    // Start is called before the first frame update
    void Start()
    {
        SetQueuePos();
    }

    public float minTime, maxTime;
    float timer;

    // Update is called once per frame
    void Update()
    {

        timer -= Time.fixedDeltaTime;

        if (timer <= 0)
        {
            if (patients.Count != 0)
            {
                timer = Random.Range(minTime, maxTime);
                RemovePatient();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (patients.Count == 0)
                return;
            RemovePatient();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnAPatient(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnAPatient(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnAPatient(3);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnAPatient(4);
        }
    }

    /// <summary>
    /// Tar bort den första i kön!
    /// </summary>
    public void RemovePatient()
    {
        PatientScript script = patients[0].GetComponent<PatientScript>();
        script.leave = true;
        patients.RemoveAt(0);
        SetQueuePos();
    }

    /// <summary>
    /// Kallas när en patient ska läggas till!
    /// </summary>
    /// <param name="priority"></param>
    public void SpawnAPatient(int priority)
    {
        NewPatient(priority);
        SetQueuePos();
    }

    /// <summary>
    /// Addar en ny patient med en given priority
    /// </summary>
    /// <param name="priority"></param>
    public void NewPatient(int priority)
    {
        //Object prefab = Resources.Load("Assets/Prefabs/Patient");
        GameObject newPatient = Instantiate(Prefab);
        PatientScript script = newPatient.GetComponent<PatientScript>();
        script.Created(priority);


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
