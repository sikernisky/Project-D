using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// Controls the scene's Camera.
/// </summary>
public class CameraControl : MonoBehaviour
{
    ///<summary>The Camera of the GameObject that this script is attached to. </summary>
    private Camera myCam;

    ///<summary>The Vector3 position when the user starts dragging. </summary>
    private Vector3 dragStartPos;

    ///<summary>The minimum orthographic camera size. </summary>
    private int minCamSize = 1;

    ///<summary>The maximum orthographic camera size. </summary>
    private int maxCamSize = 13;

    ///<summary>How much the camera zooms in or out per scroll wheel tick. Set in the inspector for accessibility.</summary>
    [SerializeField]
    private int zoomPerTick;

    ///<summary>The upper Y-boundary that this Camera may not cross.</summary>
    private float topBound;

    ///<summary>The lower Y-boundary that this Camera may not cross.</summary>
    private float botBound;

    ///<summary>The leftmost X-boundary that this Camera may not cross.</summary>
    private float leftBound;

    ///<summary>The rightmost X-boundary that this Camera may not cross.</summary>
    private float rightBound;

    ///<summary>True if the player can drag the camera around.</summary>
    public static bool CanDragCamera;

    ///<summary>The TileGrid this Camera's bounds are based on. </summary>
    private TileGrid field;



    private void Start()
    {
        CanDragCamera = true;
        myCam = GetComponent<Camera>();
        FitCamera();
        MoveMiddle();
        SetDragBounds();
    }

    private void Update()
    {
        if (CanDragCamera) MoveCamera();
        CheckZoom();
    }

    /// ///<summary>
    /// Attempts to move the Camera associated with this CameraControl if the player drags their mouse 
    /// within this CameraControl's GridTile.
    /// </summary>
    private void MoveCamera()
    {
        if (Input.GetMouseButtonDown(0)) dragStartPos = myCam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            Vector3 differenceFromStartPos = dragStartPos - myCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 proposedPosition = myCam.transform.position + differenceFromStartPos;
            float acceptedPositionX = Mathf.Clamp(proposedPosition.x, leftBound, rightBound);
            float acceptedPositionY = Mathf.Clamp(proposedPosition.y, botBound, topBound);
            Vector3 acceptedPosition = new Vector3(acceptedPositionX, acceptedPositionY, -10);
            myCam.transform.position = acceptedPosition;
        }
    }


    /// ///<summary>
    /// Zooms in or out accordingly when the player scrolls their mouse wheel.
    /// </summary>
    private void CheckZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) ZoomIn();
        if (Input.GetAxis("Mouse ScrollWheel") < 0) ZoomOut();
    }


    /// ///<summary>
    /// Zooms the Camera in by decreasing its orthographic size.
    /// </summary>
    private void ZoomIn()
    {
        float newZoom = myCam.orthographicSize -= zoomPerTick;
        myCam.orthographicSize = Mathf.Clamp(newZoom, minCamSize, maxCamSize);
        SetDragBounds();
    }

    /// ///<summary>
    /// Zooms the Camera in by decreasing its orthographic size by <c>z</c>.
    /// </summary>
    private void ZoomIn(float z)
    {
        float newZoom = myCam.orthographicSize -= z;
        myCam.orthographicSize = Mathf.Clamp(newZoom, minCamSize, maxCamSize);
        SetDragBounds();
    }


    /// ///<summary>
    /// Zooms the Camera out by increasing its orthographic size.
    /// </summary>
    private void ZoomOut()
    {
        float newZoom = myCam.orthographicSize += zoomPerTick;
        myCam.orthographicSize = Mathf.Clamp(newZoom, minCamSize, maxCamSize);
        SetDragBounds();
    }

    /// ///<summary>
    /// Sets this CameraControl's TileGrid. The CameraControl script uses this grid to 
    /// determine boundaries and Camera positions.
    /// <br></br><em>Precondition:</em> <c>grid</c> is not null.
    /// </summary>
    public void SetGrid(TileGrid grid)
    {
        Assert.IsNotNull(grid, "Parameter grid cannot be null.");
        field = grid;
    }

    /// ///<summary>
    /// Sets the left, right, top, and bot position boundaries for the Camera associated with this CameraControl.
    /// </summary>
    private void SetDragBounds()
    {
        float halfHeight = myCam.orthographicSize;
        float halfWidth = ((halfHeight * 2) * myCam.aspect)/2;
        float offset = TileGrid.tileSize / 2;

        leftBound = field.MiddleOfGrid().x - ((field.Size() / 2) - halfWidth) + offset;
        rightBound = field.MiddleOfGrid().x + ((field.Size() / 2) - halfWidth) + offset;

        botBound = field.MiddleOfGrid().y - ((field.Size() / 2) - halfHeight) + offset;
        topBound = field.MiddleOfGrid().y + ((field.Size() / 2) - halfHeight) + offset;

        PushCameraInBounds();
    }

    /// ///<summary>
    /// Pushes the Camera associated with this Camera Control within its position boundaries.
    /// </summary>
    private void PushCameraInBounds()
    {
        float pushedX = Mathf.Clamp(myCam.transform.position.x, leftBound, rightBound);
        float pushedY = Mathf.Clamp(myCam.transform.position.y, botBound, topBound);
        myCam.transform.position = new Vector3(pushedX, pushedY, -10);
    }

    /// ///<summary>
    /// Moves the Camera associated with this CameraControl to the middle of its TileGrid.
    /// </summary>
    private void MoveMiddle()
    {
        myCam.transform.position = field.MiddleOfGrid();
    }

    /// <summary>
    /// Adjust <c>maxCamSize</c> and <c>minCamSize</c> if the Camera is larger than this CameraControl's TileGrid.
    /// </summary>
    private void FitCamera()
    {
        while(CameraSize().x > field.Size() || CameraSize().y > field.Size())
        {
            maxCamSize = (int) myCam.orthographicSize;
            ZoomIn(.1f);
        }
    }

    /// <summary>
    /// <strong>Returns:</strong> the width and height of this CameraControl's Camera as x and y, respectively.
    /// </summary>
    private Vector2 CameraSize()
    {
        return new Vector2(2f * myCam.orthographicSize, (2f * myCam.orthographicSize) * myCam.aspect);
    }



}