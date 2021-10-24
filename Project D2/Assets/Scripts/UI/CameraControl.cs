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

    /**How much the camera zooms in or out per scroll wheel tick. Set in the inspector.*/
    [SerializeField]
    private int zoomPerTick;

    /**The smallest float myCam.transform.position.x can be.*/
    public float LeftBound { get; private set; }

    /**The largest float myCam.transform.position.x can be.*/
    public float RightBound { get; private set; }

    /**The largest float myCam.transform.position.y can be.*/
    public float TopBound { get; private set; }

    /**The smallest float myCam.transform.position.y can be.*/
    public float BotBound { get; private set; }



    private void Start()
    {
        myCam = GetComponent<Camera>();
        SetCameraBounds();
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
            Vector3 proposedPosition = myCam.transform.position + differenceFromStartPos;
            float acceptedPositionX = Mathf.Clamp(proposedPosition.x, LeftBound, RightBound);
            float acceptedPositionY = Mathf.Clamp(proposedPosition.y, BotBound, TopBound);
            Vector3 acceptedPosition = new Vector3(acceptedPositionX, acceptedPositionY, -10);
            myCam.transform.position = acceptedPosition;
        }
    }

    private void CheckZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) ZoomIn();
        if (Input.GetAxis("Mouse ScrollWheel") < 0) ZoomOut();
    }

    private void ZoomIn()
    {
        float newZoom = myCam.orthographicSize -= zoomPerTick;
        myCam.orthographicSize = Mathf.Clamp(newZoom, minCamSize, maxCamSize);
        SetCameraBounds();
    }

    private void ZoomOut()
    {
        float newZoom = myCam.orthographicSize += zoomPerTick;
        myCam.orthographicSize = Mathf.Clamp(newZoom, minCamSize, maxCamSize);
        SetCameraBounds();
    }

    private void SetCameraBounds()
    {
        float height = 2f * myCam.orthographicSize;
        float width = height * myCam.aspect;

        LeftBound = (width/2) - GameGridMaster.cellSize / 2;
        RightBound = GameGridMaster.cellSize * GameGridMaster.GridWidth - (width/2) - GameGridMaster.cellSize / 2;

        BotBound = (height / 2) - GameGridMaster.cellSize / 2;
        TopBound = GameGridMaster.cellSize * GameGridMaster.GridHeight - (height / 2) - GameGridMaster.cellSize / 2;

        PushCameraInBounds();

    }

    private void PushCameraInBounds()
    {
        float pushedX = Mathf.Clamp(myCam.transform.position.x, LeftBound, RightBound);
        float pushedY = Mathf.Clamp(myCam.transform.position.y, BotBound, TopBound);
        myCam.transform.position = new Vector3(pushedX, pushedY, -10);
    }



}
