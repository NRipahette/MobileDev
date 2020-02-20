using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TestPrintTime()
    {
        Debug.Log(Time.time);
    }

    public void PartyLaunch() {
        StartCoroutine(InvokeMethod(party, 0.5f, 10));

    }

    public IEnumerator InvokeMethod(Action method, float interval, int invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
            method();

            yield return new WaitForSeconds(interval);
        }
    }

    private void party() {
        GameObject[] objectList = GameObject.FindGameObjectsWithTag("Object");

        foreach (GameObject obj in objectList)
        {
            obj.GetComponent<BehaviorScriptCube1>().ChangeColor();
        }
    }
}
