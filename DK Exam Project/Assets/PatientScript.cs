using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientScript : MonoBehaviour
{
    public int queueIndex;
    public Vector3 queuePos;

    /// <summary>
    /// Is set to true when they're leaving
    /// </summary>
    public bool leave;

    /// <summary>
    /// Displays this patient's current priority.
    /// </summary>
    public int priority;

    /// <summary>
    /// Counts the number of times this patient has been passed. 
    /// </summary>
    int count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Created(int priority)
    {
        this.priority = priority;
        count = 0;
        MeshRenderer r = GetComponent<MeshRenderer>();        

        switch (priority)
        {
            case 1:
                r.material.color = Color.cyan;

                break;
            case 2:
                r.material.color = Color.green;

                break;
            case 3:
                r.material.color = Color.yellow;

                break;
            case 4:
                r.material.color = Color.red;

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (leave)
        {
            transform.Translate(-0.5f, 0, 0);
            if (transform.position.x < -50)
            {
                Destroy(gameObject);
            }
        }
        else if (queuePos.x != transform.position.x)
        {
            transform.Translate(new Vector3((queuePos.x - transform.position.x) * 0.1f, 0, 0));
            if (Mathf.Abs(queuePos.x - transform.position.x) < 0.01f)
                transform.position = new Vector3(queuePos.x, 0, transform.position.z);
        }
        else if(queuePos.z != transform.position.z)
        {
            transform.Translate(new Vector3(0,0,(queuePos.z - transform.position.z) * 0.1f));
            if (Mathf.Abs( queuePos.z- transform.position.z) < 0.01f)
                transform.position = new Vector3(transform.position.x, 0, queuePos.z);
        }
        
        
    }

    public void UpdateIndex(int newIndex)
    {
        if (newIndex > queueIndex && queueIndex!=0)
        {
            count++;
            if (count >= 4)
            {
                priority++;
                count = 0;
            }
        }

        queueIndex = newIndex;
        queuePos = new Vector3(queueIndex - 10 , 0, 0);
    }
}
