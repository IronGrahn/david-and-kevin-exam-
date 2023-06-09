using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientScript : MonoBehaviour
{
    public int queueIndex;
    public Vector3 queuePos;
    public bool leave;
    public int priority;

    public QueueManager queueManager;
    public float currentTime;
    public float totalTime;
    public bool removed;
    public float moveSpeed; 

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
        removed = false;
        GameObject foundObject = GameObject.Find("QueueManager");
        queueManager = foundObject.GetComponentInChildren<QueueManager>();
    }

    public void Created(int priority, float totalTime, Material material)
    {
        this.totalTime = totalTime;
        this.priority = priority;

        Transform childMesh = gameObject.transform;
        Renderer childRenderer = childMesh.GetComponent<Renderer>();
        if (childRenderer != null)
        {
            childRenderer.material = material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = queueManager.speed;
        if (currentTime < totalTime)
        {
            currentTime += Time.deltaTime * queueManager.speed;
            Debug.Log("Priority: " +priority + "Current time: " + currentTime + " Queue Index: " + queueIndex);
        }
        else if(!removed)
        {
            queueManager.RemovePatientAtPos(queueIndex);
            removed = true;
        }

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
        queueIndex = newIndex;
        queuePos = new Vector3(queueIndex -10 , 0, 0);
    }
}
