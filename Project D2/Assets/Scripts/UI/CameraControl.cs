using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /**The Camera of the GameObject that this script is attached to. */
    private Camera myCam;

    /**The Vector3 position when the user starts dragging. */
    private Vector3 dragStartPos;

    /**The minimum orthographic camera size. Set in the inspector. */
    [SerializeField]
    private int minCamSize;

    /**The maximum orthographic camera size. Set in the inspector. */
    [SerializeField]
    private int maxCamSize;

    /**How much the camera zooms in or out per scroll wheel tick. */
    [SerializeField]
    private int zoomPerTick;


    private void Start()
    {
        myCam = GetComponent<Camera>();
        SetBounds();
    }

    private void Update()
    {
        CheckZoom();
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (Input.GetMouseButtonDown(0)) dragStartPos = myCam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            Vector3 differenceFromStartPos = dragStartPos - myCam.ScreenToWorldPoint(Input.mousePosition);
            myCam.transform.position += differenceFromStartPos;
        }
    }

    private void CheckZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) ZoomOut();
        if (Input.GetAxis("Mouse ScrollWheel") < 0) ZoomIn();
    }

    private void ZoomIn()
    {
        float newZoom = myCam.orthographicSize += zoomPerTick;
        myCam.orthographicSize = Mathf.Clamp(newZoom, minCamSize, maxCamSize);
    }

    private void ZoomOut()
    {
        float newZoom = myCam.orthographicSize -= zoomPerTick;
        myCam.orthographicSize = Mathf.Clamp(newZoom, minCamSize, maxCamSize);
    }

    private void SetBounds()
    {
        float height = 2f * myCam.orthographicSize;
        float width = height * myCam.aspect;

        Debug.Log("Height is " + height + " and width is " + width);
    }



}
