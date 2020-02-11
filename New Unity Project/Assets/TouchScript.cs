﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScript : MonoBehaviour
{
    BehaviorScriptCube1 selectedObject;
    private float ObjDistance;
    Camera my_camera;
    private float timePassed = 0f;
    private bool IsTap = true;
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
                timePassed += Time.deltaTime;
                if (timePassed >= 0.25f) { IsTap = false; }
                else
                {
                    IsTap = true;
                }
                Touch touchZero = Input.GetTouch(0);
                if(touchZero.phase == TouchPhase.Began)
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
                            ObjDistance = raycastHit.distance;
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
                }else if (touchZero.phase == TouchPhase.Moved || touchZero.phase == TouchPhase.Stationary && !IsTap)
                {
                   // Debug.Log(touchZero.position.x - startingPoint.x);
                    selectedObject.Drag(touchZero, startingPoint,  ObjDistance);
                }
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);

                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in distances between each frame.
                float deltaMagnitudeDiff = (prevTouchDeltaMag - touchDeltaMag) * 0.01f;

                Vector3 newScale = selectedObject.transform.localScale - new Vector3(deltaMagnitudeDiff, deltaMagnitudeDiff, deltaMagnitudeDiff);
                if (newScale.x <= 0)
                    selectedObject.transform.localScale = Vector3.zero;
                else
                    selectedObject.transform.localScale = newScale;
            }
            else
            {



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

        
    }


    
}