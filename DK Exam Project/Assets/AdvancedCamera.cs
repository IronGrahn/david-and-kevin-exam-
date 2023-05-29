using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedCamera : MonoBehaviour
{
    public Transform parentObject;
    public float cameraDistance = 10f;
    public float minFOV = 10f;
    public float maxFOV = 60f;
    public float padding = 2f;

    public Camera mainCamera;
    public Bounds bounds;

    private void Start()
    {

    }

    private void Update()
    {

        CalculateBounds();
        AdjustCamera();

    }

    void CalculateBounds()
    {
        bounds = new Bounds(parentObject.position, Vector3.zero);

        Renderer[] childRenderers = parentObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in childRenderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
    }

    void AdjustCamera()
    {
        Vector3 boundsCenter = bounds.center;
        Vector3 boundsExtents = bounds.extents;

        float objectSize = Mathf.Max(boundsExtents.x, boundsExtents.y, boundsExtents.z);
        float targetDistance = objectSize / Mathf.Tan(Mathf.Deg2Rad * (mainCamera.fieldOfView / 2)) + padding;

        cameraDistance = Mathf.Lerp(cameraDistance, targetDistance, Time.deltaTime * 5f);
        cameraDistance = Mathf.Clamp(cameraDistance, objectSize, Mathf.Infinity);

        mainCamera.transform.position = boundsCenter - mainCamera.transform.forward * cameraDistance;
        mainCamera.fieldOfView = Mathf.Lerp(minFOV, maxFOV, Mathf.InverseLerp(objectSize, 0f, cameraDistance));
    }


}
