using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorScriptCube1 : MonoBehaviour
{
    Renderer my_renderer;
    // Start is called before the first frame update
    void Start()
    {
        my_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    internal void ChangeColor()
    {
        my_renderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    internal void Drag(Touch touch, Vector3 starting, float Distance)
    {
        //if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        //{
            // get the touch position from the screen touch to world point
            //Vector3 PrevTouchedPos = Camera.main.ScreenToWorldPoint(new Vector3(starting.x, starting.y, 0));
            Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Distance));
            // lerp and set the position of the current object to that of the touch, but smoothly over time.
            //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(starting.x,starting.y, Distance)) + Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x - starting.x, touch.position.y - starting.y, Distance));
        //Vector3.Lerp(transform.position, touchedPos, Time.deltaTime * 3);
        transform.position = touchedPos;
        //}

    }
    internal void Select()
    {
       
    }
}
