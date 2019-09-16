using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// These allow us to use the ARFoundation API.
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceGameBoard : MonoBehaviour
{
    // Public variables can be set from the unity UI.
    // We will set this to our Game Board object.
    public GameObject gameBoard;
    public GameObject X1;
    public GameObject X2;
    public GameObject X3;
    public GameObject X4;
    public GameObject X5;
    public GameObject draw;
    public GameObject XWIN;
    public GameObject OWIN;
    public GameObject O1;
    public GameObject O2;
    public GameObject O3;
    public GameObject O4;
    // These will store references to our other components.
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    // This will indicate whether the game board is set.
    private bool placed = false;
    private int turn = 0;
    Vector3 boardpos;
    Vector3 center;
    Vector3 upleft;
    Vector3 upcenter;
    Vector3 upright;
    Vector3 centerleft;
    Vector3 centerright;
    Vector3 downleft;
    Vector3 downcenter;
    Vector3 downright;
    //public int [,] boardscore = new int[3, 3];
    private int[,] boardscore = new int[3, 3];
    // Start is called before the first frame update.
    void Start()
    {
        // GetComponent allows us to reference other parts of this game object.
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    // Update is called once per frame.
    void Update()
    {
        if (!placed)
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;

                // Raycast will return a list of all planes intersected by the
                // ray as well as the intersection point.
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(
                    touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {
                    // The list is sorted by distance so to get the location
                    // of the closest intersection we simply reference hits[0].
                    var hitPose = hits[0].pose;
                    // Now we will activate our game board and place it at the
                    // chosen location.
                    gameBoard.SetActive(true);
                    gameBoard.transform.position = hitPose.position;
                    boardpos = gameBoard.transform.position;
                    placed = true;
                    // After we have placed the game board we will disable the
                    // planes in the scene as we no longer need them.
                    planeManager.SetTrackablesActive(false);

                }
            }
        }
        else
        {
            // The plane manager will set newly detected planes to active by 
            // default so we will continue to disable these.
            planeManager.SetTrackablesActive(false);
            if (Input.touchCount > 0 && turn == 0)
            {
                for(int i=0;i<3;i++)
                {
                    for(int j=0;j<3;j++)
                    {
                        boardscore[i,j] = 0;
                    }
                }
                center[0] = boardpos[0] - (float)0.03;
                center[1] = boardpos[1] + (float)0.02;
                center[2] = boardpos[2] + (float)0.03;
                upleft[0] = center[0] - (float)0.1;
                upleft[2] = center[2] + (float)0.1;
                upleft[1] = center[1];
                upcenter[0] = center[0];
                upcenter[2] = center[2] + (float)0.1;
                upcenter[1] = center[1];
                upright[0] = center[0] + (float)0.1;
                upright[2] = center[2] + (float)0.1;
                upright[1] = center[1];
                centerleft[0] = center[0] - (float)0.1;
                centerleft[2] = center[2];
                centerleft[1] = center[1];
                centerright[0] = center[0] + (float)0.1;
                centerright[2] = center[2];
                centerright[1] = center[1];
                downleft[0] = center[0] - (float)0.1;
                downleft[2] = center[2] - (float)0.1;
                downleft[1] = center[1];
                downcenter[0] = center[0];
                downcenter[2] = center[2] - (float)0.1;
                downcenter[1] = center[1];
                downright[0] = center[0] + (float)0.1;
                downright[2] = center[2] - (float)0.1;
                downright[1] = center[1];
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                    turn = 1;
            }
            if (Input.touchCount > 0 && turn ==1 )
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(
                    touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {
                  
                    var hitPose = hits[0].pose;
                    X1.SetActive(true);
                    Vector3 posi = hitPose.position;
                    int loc = setPosition(posi,1);
                    switch(loc)
                    {
                        case 1:
                            X1.transform.position = upleft;
                            break;
                        case 2:
                            X1.transform.position = upcenter;
                            break;
                        case 3:
                            X1.transform.position = upright;
                            break;
                        case 4:
                            X1.transform.position = centerleft;
                            break;
                        case 5:
                            X1.transform.position = center;
                            break;
                        case 6:
                            X1.transform.position = centerright;
                            break;
                        case 7:
                            X1.transform.position = downleft;
                            break;
                        case 8:
                            X1.transform.position = downcenter;
                            break;
                        case 9:
                            X1.transform.position = downright;
                            break;
                    }
                    Touch touch = Input.GetTouch(0);
                    if(touch.phase == TouchPhase.Ended)
                    turn = 2;                   

                }
                if (winner())
                {
                    xwins();
                }
            }
            else if (Input.touchCount > 0 && turn == 2)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(
                    touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {

                    var hitPose = hits[0].pose;
                    O1.SetActive(true);
                    Vector3 posi = hitPose.position;
                    int loc = setPosition(posi,2);
                    switch (loc)
                    {
                        case 1:
                            O1.transform.position = upleft;
                            break;
                        case 2:
                            O1.transform.position = upcenter;
                            break;
                        case 3:
                            O1.transform.position = upright;
                            break;
                        case 4:
                            O1.transform.position = centerleft;
                            break;
                        case 5:
                            O1.transform.position = center;
                            break;
                        case 6:
                            O1.transform.position = centerright;
                            break;
                        case 7:
                            O1.transform.position = downleft;
                            break;
                        case 8:
                            O1.transform.position = downcenter;
                            break;
                        case 9:
                            O1.transform.position = downright;
                            break;
                    }
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                        turn = 3;

                }
                if (winner())
                {
                    owins();
                }
            }
            else if (Input.touchCount > 0 && turn == 3)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(
                    touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {

                    var hitPose = hits[0].pose;
                    X2.SetActive(true);
                    Vector3 posi = hitPose.position;
                    int loc = setPosition(posi,1);
                    switch (loc)
                    {
                        case 1:
                            X2.transform.position = upleft;
                            break;
                        case 2:
                            X2.transform.position = upcenter;
                            break;
                        case 3:
                            X2.transform.position = upright;
                            break;
                        case 4:
                            X2.transform.position = centerleft;
                            break;
                        case 5:
                            X2.transform.position = center;
                            break;
                        case 6:
                            X2.transform.position = centerright;
                            break;
                        case 7:
                            X2.transform.position = downleft;
                            break;
                        case 8:
                            X2.transform.position = downcenter;
                            break;
                        case 9:
                            X2.transform.position = downright;
                            break;
                    }
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                        turn = 4;

                }
                if (winner())
                {
                    xwins();
                }
            }
            else if (Input.touchCount > 0 && turn == 4)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(
                    touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {

                    var hitPose = hits[0].pose;
                    O2.SetActive(true);
                    Vector3 posi = hitPose.position;
                    int loc = setPosition(posi,2);
                    switch (loc)
                    {
                        case 1:
                            O2.transform.position = upleft;
                            break;
                        case 2:
                            O2.transform.position = upcenter;
                            break;
                        case 3:
                            O2.transform.position = upright;
                            break;
                        case 4:
                            O2.transform.position = centerleft;
                            break;
                        case 5:
                            O2.transform.position = center;
                            break;
                        case 6:
                            O2.transform.position = centerright;
                            break;
                        case 7:
                            O2.transform.position = downleft;
                            break;
                        case 8:
                            O2.transform.position = downcenter;
                            break;
                        case 9:
                            O2.transform.position = downright;
                            break;
                    }
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                        turn = 5;

                }
                if (winner())
                {
                    owins();
                }
            }
            else if (Input.touchCount > 0 && turn == 5)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(
                    touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {

                    var hitPose = hits[0].pose;
                    X3.SetActive(true);
                    Vector3 posi = hitPose.position;
                    int loc = setPosition(posi,1);
                    switch (loc)
                    {
                        case 1:
                            X3.transform.position = upleft;
                            break;
                        case 2:
                            X3.transform.position = upcenter;
                            break;
                        case 3:
                            X3.transform.position = upright;
                            break;
                        case 4:
                            X3.transform.position = centerleft;
                            break;
                        case 5:
                            X3.transform.position = center;
                            break;
                        case 6:
                            X3.transform.position = centerright;
                            break;
                        case 7:
                            X3.transform.position = downleft;
                            break;
                        case 8:
                            X3.transform.position = downcenter;
                            break;
                        case 9:
                            X3.transform.position = downright;
                            break;
                    }
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                        turn = 6;

                }
                if (winner())
                {
                    xwins();
                }
            }
            else if (Input.touchCount > 0 && turn == 6)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(
                    touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {

                    var hitPose = hits[0].pose;
                    O3.SetActive(true);
                    Vector3 posi = hitPose.position;
                    int loc = setPosition(posi,2);
                    switch (loc)
                    {
                        case 1:
                            O3.transform.position = upleft;
                            break;
                        case 2:
                            O3.transform.position = upcenter;
                            break;
                        case 3:
                            O3.transform.position = upright;
                            break;
                        case 4:
                            O3.transform.position = centerleft;
                            break;
                        case 5:
                            O3.transform.position = center;
                            break;
                        case 6:
                            O3.transform.position = centerright;
                            break;
                        case 7:
                            O3.transform.position = downleft;
                            break;
                        case 8:
                            O3.transform.position = downcenter;
                            break;
                        case 9:
                            O3.transform.position = downright;
                            break;
                    }
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                        turn = 7;

                }
                if (winner())
                {
                    owins();
                }
            }
            else if (Input.touchCount > 0 && turn == 7)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(
                    touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {

                    var hitPose = hits[0].pose;
                    X4.SetActive(true);
                    Vector3 posi = hitPose.position;
                    int loc = setPosition(posi,1);
                    switch (loc)
                    {
                        case 1:
                            X4.transform.position = upleft;
                            break;
                        case 2:
                            X4.transform.position = upcenter;
                            break;
                        case 3:
                            X4.transform.position = upright;
                            break;
                        case 4:
                            X4.transform.position = centerleft;
                            break;
                        case 5:
                            X4.transform.position = center;
                            break;
                        case 6:
                            X4.transform.position = centerright;
                            break;
                        case 7:
                            X4.transform.position = downleft;
                            break;
                        case 8:
                            X4.transform.position = downcenter;
                            break;
                        case 9:
                            X4.transform.position = downright;
                            break;
                    }
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                        turn = 8;

                }
                if (winner())
                {
                    xwins();
                }
            }
            else if (Input.touchCount > 0 && turn == 8)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(
                    touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {

                    var hitPose = hits[0].pose;
                    O4.SetActive(true);
                    Vector3 posi = hitPose.position;
                    int loc = setPosition(posi,2);
                    switch (loc)
                    {
                        case 1:
                            O4.transform.position = upleft;
                            break;
                        case 2:
                            O4.transform.position = upcenter;
                            break;
                        case 3:
                            O4.transform.position = upright;
                            break;
                        case 4:
                            O4.transform.position = centerleft;
                            break;
                        case 5:
                            O4.transform.position = center;
                            break;
                        case 6:
                            O4.transform.position = centerright;
                            break;
                        case 7:
                            O4.transform.position = downleft;
                            break;
                        case 8:
                            O4.transform.position = downcenter;
                            break;
                        case 9:
                            O4.transform.position = downright;
                            break;
                    }
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                        turn = 9;
                }
                if (winner())
                {
                    owins();
                }
            }
            else if (Input.touchCount > 0 && turn == 9)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(
                    touchPosition, hits, TrackableType.PlaneWithinPolygon))
                {

                    var hitPose = hits[0].pose;
                    X5.SetActive(true);
                    Vector3 posi = hitPose.position;
                    int loc = setPosition(posi,1);
                    switch (loc)
                    {
                        case 1:
                            X5.transform.position = upleft;
                            break;
                        case 2:
                            X5.transform.position = upcenter;
                            break;
                        case 3:
                            X5.transform.position = upright;
                            break;
                        case 4:
                            X5.transform.position = centerleft;
                            break;
                        case 5:
                            X5.transform.position = center;
                            break;
                        case 6:
                            X5.transform.position = centerright;
                            break;
                        case 7:
                            X5.transform.position = downleft;
                            break;
                        case 8:
                            X5.transform.position = downcenter;
                            break;
                        case 9:
                            X5.transform.position = downright;
                            break;
                    }
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                        turn = 10;

                }
                if (winner())
                {
                    xwins();
                }
                else
                {
                    draw.SetActive(true);
                    draw.transform.position = center;
                }
                

            }
            else if(Input.touchCount >0 && turn == 10)
            {
                resetGameBoard();
            }

        }
    }

    // If the user places the game board at an undesirable location we 
    // would like to allow the user to move the game board to a new location.
    private bool winner()
    {
        if ((boardscore[0, 0] == boardscore[0, 1]) && (boardscore[0, 1] == boardscore[0, 2]) && (boardscore[0, 0] != 0))
            return true;
        else if ((boardscore[0, 0] == boardscore[1, 0]) && (boardscore[1, 0] == boardscore[2, 0]) && (boardscore[0, 0] != 0))
            return true;
        else if ((boardscore[0, 0] == boardscore[1, 1]) && (boardscore[1, 1] == boardscore[2, 2]) && (boardscore[0, 0] != 0))
            return true;
        else if ((boardscore[0, 2] == boardscore[1, 2]) && (boardscore[1, 2] == boardscore[2, 2]) && (boardscore[0, 2] != 0))
            return true;
        else if ((boardscore[1, 0] == boardscore[1, 1]) && (boardscore[1, 1] == boardscore[1, 2]) && (boardscore[1, 0] != 0))
            return true;
        else if ((boardscore[2, 0] == boardscore[2, 1]) && (boardscore[2, 1] == boardscore[2, 2]) && (boardscore[2, 0] != 0))
            return true;
        else if ((boardscore[0, 1] == boardscore[1, 1]) && (boardscore[1, 1] == boardscore[2, 1]) && (boardscore[0, 1] != 0))
            return true;
        else if ((boardscore[2, 0] == boardscore[1, 1]) && (boardscore[1, 1] == boardscore[0, 2]) && (boardscore[2, 0] != 0))
            return true;
        else
            return false;
    }
    private int setPosition(Vector3 posi, int xory)
    {
        if(posi[0]<center[0]-0.05)
        {
            if (posi[2] > center[2] + 0.05)
            {
                boardscore[0, 0] = xory;
                return 1;
               
            }
                
            else if (posi[2] < center[2] - 0.05)
            {
                boardscore[2, 0] = xory;
                return 7;
                
            }
            else
            {
                boardscore[1, 0] = xory;
                return 4;
                
            }
        } 
        else if (posi[0]>center[0]+0.05)
        {
            if (posi[2] > center[2] + 0.05)
            {
            
                boardscore[0, 2] = xory;
                return 3;
            }
            else if (posi[2] < center[2] - 0.05)
            {
                boardscore[2, 2] = xory;
                return 9;
                
            }
            else
            {
                boardscore[1, 2] = xory;
                return 6;
                

            }
        }
        else
        {
            if (posi[2] > center[2] + 0.05)
            {
                boardscore[0, 1] = xory;
                return 2;
                
            }
            else if (posi[2] < center[2] - 0.05)
            {
                boardscore[2, 1] = xory;
                return 8;
                
            }
            else
            {
                boardscore[1, 1] = xory;
                return 5;
            }
        }
    }
    private void xwins()
    {
        XWIN.SetActive(true);
        XWIN.transform.position = centerleft;
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Ended)
            turn = 10;
    }
    private void owins()
    {
        OWIN.SetActive(true);
        OWIN.transform.position = centerleft;
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Ended)
            turn = 10;
    }
    public void resetGameBoard()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Ended)
            turn = 0;
        draw.SetActive(false);
        XWIN.SetActive(false);
        OWIN.SetActive(false);
        X1.SetActive(false);
        X2.SetActive(false);
        X3.SetActive(false);
        X4.SetActive(false);
        X5.SetActive(false);
        O1.SetActive(false);
        O2.SetActive(false);
        O3.SetActive(false);
        O4.SetActive(false);

    }
    public void AllowMoveGameBoard()
    {
        placed = false;
        gameBoard.SetActive(false);
        planeManager.SetTrackablesActive(true);
        resetGameBoard();
    }

    // Lastly we will later need to allow other components to check whether the
    // game board has been places so we will add an accessor to this.
    public bool Placed()
    {
        return placed;
    }
}
