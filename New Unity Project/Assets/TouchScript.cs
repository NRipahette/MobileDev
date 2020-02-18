using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScript : MonoBehaviour
{
    BehaviorScriptCube1 selectedObject;
    private float ObjDistance;
    Camera my_camera;
    private float zoomSpeed = 3;
    private float cameraSpeed = 0.01f;
    private float timePassed = 0f;
    private bool IsTap = true;
    private bool IsRotating= false;
    private bool IsScaling= false;
    private float globalMagnitudeDiff = 0;
    private float globalAngle = 0;
    private Vector2 firstPos;
    private Vector3 startingPoint;

    // Start is called before the first frame update
    void Start()
    {
        my_camera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                IsScaling = false;
                IsRotating = false;

                timePassed += Time.deltaTime;
                if (timePassed >= 0.25f) { IsTap = false; }
                else
                {
                    IsTap = true;
                }
                Touch touchZero = Input.GetTouch(0);
                if (touchZero.phase == TouchPhase.Began)
                {
                    IsTap = true;
                    timePassed = 0;
                    //startingPoint = new Vector2(touchZero.position.x, touchZero.position.y);
                    Debug.Log(startingPoint.x);
                }
                else if (touchZero.phase == TouchPhase.Ended)
                {
                    startingPoint = Vector3.zero;
                    if (IsTap)
                    {





                        Ray raycast = my_camera.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit raycastHit;

                        if (Physics.Raycast(raycast, out raycastHit))
                        {
                            ObjDistance = raycastHit.transform.position.z - my_camera.transform.position.z;
                            //if (Input.GetTouch(0).phase == TouchPhase.Began)
                            if (IsTap)
                            {

                                if (raycastHit.collider.tag == "Object")
                                {
                                    selectedObject = raycastHit.collider.gameObject.GetComponent<BehaviorScriptCube1>();
                                    selectedObject.ChangeColor();
                                    //raycastHit.collider.gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
                                }
                                else
                                {
                                    selectedObject = null;
                                }

                                /*if (raycastHit.collider.name == "Cube")
                                 {
                                     raycastHit.collider.gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f); ;
                                 }*/
                            }






                        }
                        else
                        {
                            selectedObject = null;
                        }
                        //timePassed = 0;
                    }
                    else
                    {
                        //IsTap = false;

                        //timePassed = 0;
                    }
                }
                else if (touchZero.phase == TouchPhase.Moved || touchZero.phase == TouchPhase.Stationary && !IsTap)
                {
                    // Debug.Log(touchZero.position.x - startingPoint.x);
                    if (selectedObject != null)
                    {
                        selectedObject.Drag(touchZero, startingPoint, ObjDistance);
                    }
                    else
                    {

                        my_camera.transform.Translate(-touchZero.deltaPosition.x * cameraSpeed, -touchZero.deltaPosition.y * cameraSpeed, 0);
                    }
                }
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);

                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroInitialPos;
                Vector2 touchOneInitialPos;

                if (touchOne.phase == TouchPhase.Began)
                {
                    touchZeroInitialPos = touchZero.position;
                    touchOneInitialPos = touchOne.position;
                }
                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in distances between each frame.
                float deltaMagnitudeDiff = (prevTouchDeltaMag - touchDeltaMag) * 0.01f;

                var prevPos1 = touchZero.position - touchZero.deltaPosition;  // Generate previous frame's finger positions
                var prevPos2 = touchOne.position - touchOne.deltaPosition;
                Vector2 prevDir = prevPos2 - prevPos1;
                Vector2 currDir = touchOne.position - touchZero.position;
                float angle = 0;
                angle = Vector2.SignedAngle(prevDir, currDir);
                
                if (selectedObject != null)
                {
                    if (!IsScaling && !IsRotating)
                    {
                        globalMagnitudeDiff += deltaMagnitudeDiff;
                        globalAngle += angle;//Vector2.SignedAngle(firstPos, currDir);
                        if (globalMagnitudeDiff >= 2|| globalMagnitudeDiff <= -2)
                        {
                            IsScaling = true;
                            IsRotating = false;
                        }
                        else if (globalAngle >= 2 || globalAngle <= -2)
                        {
                            IsScaling = false;
                            IsRotating = true;
                        }

                    }

                    if (IsScaling)
                    {
                        Vector3 newScale = selectedObject.transform.localScale - new Vector3(deltaMagnitudeDiff, deltaMagnitudeDiff, deltaMagnitudeDiff);
                        if (newScale.x <= 0)
                            selectedObject.transform.localScale = Vector3.zero;
                        else
                            selectedObject.transform.localScale = newScale;
                    }




                    if (IsRotating)
                    {
                        selectedObject.transform.Rotate(0, 0, angle);

                    }

                }
                else
                {

                    if (!IsScaling && !IsRotating)
                    {
                        globalMagnitudeDiff += deltaMagnitudeDiff;
                        globalAngle += angle;//Vector2.SignedAngle(firstPos, currDir);
                        if (globalMagnitudeDiff >= 1.3 || globalMagnitudeDiff <= -1.3)
                        {
                            IsScaling = true;
                            IsRotating = false;
                        }
                        else if (globalAngle >= 3 || globalAngle <= -3 )
                        {
                            if (globalMagnitudeDiff <= 0.5 && globalMagnitudeDiff >= -0.5)
                            {
                                IsScaling = false;
                                IsRotating = true;
                            }
                        }

                    }

                    if (IsScaling)
                    {
                        float newFov = my_camera.fieldOfView + deltaMagnitudeDiff * zoomSpeed;
                        if (newFov <= 0)
                            my_camera.fieldOfView = 0;
                        else
                            my_camera.fieldOfView = newFov;
                    }

                    if (IsRotating)
                    {
                        my_camera.transform.Rotate(0, 0, -angle);
                    }
                }

            }
            else
            {


                IsScaling = false;
                IsRotating = false;
                globalMagnitudeDiff = 0;
                globalAngle = 0;
                
            }


            //if (raycastHit.collider.GetComponent<BehaviorScriptCube1>() == selectedObject)
            //{
            //    if (Physics.Raycast(raycast, out raycastHit))
            //    {
            //        raycastHit.collider.gameObject.GetComponent<BehaviorScriptCube1>().Drag(Input.GetTouch(0));
            //        //material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            //        /*if (raycastHit.collider.name == "Cube")
            //         {
            //             raycastHit.collider.gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f); ;
            //         }*/
            //    }
            //}
        }
        else
        {
            IsScaling = false;
            IsRotating = false;
        }


        }


    
}
