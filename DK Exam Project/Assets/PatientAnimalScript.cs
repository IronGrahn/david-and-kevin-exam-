using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientAnimalScript : MonoBehaviour
{
    public int queueIndex;
    public Vector3 queuePos;
    public bool leave;
    public int priority;

    public QueueAnimalManager queueAnimalManager;
    public float currentTime;
    public float totalTime;
    public bool removed;

    public float moveSpeed;

    //Doors
    [Header("Door Settings")]
    public bool isRotated;
    public float rotationSpeed = 1.0f;
    private float t = 0.0f;
    private Quaternion initialLeftRotation;
    private Quaternion targeLefttRotation;
    private Quaternion initialRightRotation;
    private Quaternion targeRighttRotation;

    void Start()
    {
        currentTime = 0f;
        moveSpeed = 1f; 
        removed = false;
        isRotated = false;


        GameObject foundObject = GameObject.Find("QueueAnimalManager");

        queueAnimalManager = foundObject.GetComponentInChildren<QueueAnimalManager>();
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
    }

    void Update()
    {
        if (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
     //       Debug.Log("Priority: " + priority + "Current time: " + currentTime + " Queue Index: " + queueIndex);
        }
        else if (!removed)
        {
            queueAnimalManager.RemovePatientAtPos(queueIndex);
            removed = true;
        }

        if (leave)
        {
            MoveCharacter(queueAnimalManager.entrancePosition.transform.position, "OUT");
        }
        else if (queuePos.x != transform.position.x)
        {
            transform.Translate(new Vector3((queuePos.x - transform.position.x) * 0.1f, 0, 0));
            if (Mathf.Abs(queuePos.x - transform.position.x) < 0.01f)
                transform.position = new Vector3(queuePos.x, 0, transform.position.z);
        }
        else if (queuePos.z != transform.position.z)
        {
            transform.Translate(new Vector3(0, 0, (queuePos.z - transform.position.z) * 0.1f));
            if (Mathf.Abs(queuePos.z - transform.position.z) < 0.01f)
                transform.position = new Vector3(transform.position.x, 0, queuePos.z);
        }

    }


    //Movements

    private bool isMoving;
    private IEnumerator MoveOutPosition(Vector3 targetPos, System.Action onMoveComplete)
    {
        isMoving = true;
        // RotateDoors(isRotated);

        while (transform.position.z >= targetPos.z+0.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            Debug.Log("MOVING TO TARGET");
            yield return null;
        }
        onMoveComplete?.Invoke();
    }
    //Move Out Methods
    public void MoveCharacter(Vector3 targetPosition, string action)
    {
        Debug.Log("Move String is: " + action);        
        if (!isMoving && action == "OUT")
        {
            Debug.Log("MOVING TO EXIT");
            StartCoroutine(MoveOutPosition(targetPosition, OnMoveToEntranceComplete));
        }     
        if (!isMoving && action == "OUT2")
        {
            Debug.Log("MOVING OUTSIDE");
            StartCoroutine(MoveOutPosition(targetPosition, OnMoveOutComplete));
        }  
    }
    private void OnMoveToEntranceComplete()
    {
        Debug.Log("MOVED TO Entrance");
        isMoving = false;
        MoveCharacter(queueAnimalManager.exitPosition.transform.position, "OUT2");
    }
    private void OnMoveOutComplete()
    {
        Debug.Log("MOVED To Final Destination");
       // ResetDoors(isRotated);
        isMoving = false;
        Destroy(gameObject);
    }

    //Index 
    public void UpdateIndex(int newIndex)
    {
        queueIndex = newIndex;
        queuePos = new Vector3(queueIndex - 10, 0, 0);
    }


    //Rotate doors

    /*
    public void RotateDoors(bool rotated)
    {
            queueAnimalManager.leftDoor.transform.rotation = Quaternion.Euler(0f, -100f, 0f);
            queueAnimalManager.rightDoor.transform.rotation = Quaternion.Euler(0f, 220f, 0f);
        
        Debug.Log("Rotated Doors!");
    }
    public void ResetDoors(bool rotated)
    {

            queueAnimalManager.leftDoor.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            queueAnimalManager.rightDoor.transform.rotation =  Quaternion.Euler(0f, 180f, 0f);

        Debug.Log("Reset Doors!");
    }
    */
}
