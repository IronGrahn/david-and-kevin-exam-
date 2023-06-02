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

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
        removed = false;
    }

    public void Created(int priority, float totalTime, Material material)
    {
        this.totalTime = totalTime;
        this.priority = priority;

        Transform childMesh = gameObject.transform.Find("Mesh_LOD0");
        GameObject childObjectMesh = childMesh.gameObject;
        Renderer childRenderer = childMesh.GetComponent<Renderer>();
        if (childRenderer != null)
        {
            childRenderer.material = material;
        }

        //  gameObject.transform.rotation = Quaternion.Euler(0, 130, 0);

        /*
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
         */
    }

    // Update is called once per frame
    void Update()
    {

        if (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            Debug.Log("Priority: " +priority + "Current time: " + currentTime + " Queue Index: " + queueIndex);
        }
        else if(!removed)
        {
            GameObject foundObject = GameObject.Find("QueueManager");
            QueueManager queueManager = foundObject.GetComponentInChildren<QueueManager>();
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
