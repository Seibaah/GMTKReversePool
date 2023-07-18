using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float sensitivity = 200f;
    public List<GameObject> balls = new List<GameObject>();
    GameObject selectedBall = null;
    public GameObject cursor;
    public GameObject mainCamera;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(Input.touchCount-1);

            // Touch screen has been pressed
            // cube coords 0 1.2 -10.25
            Vector3 touchPosition = touch.position;
            Vector3 touchWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, -12.8f));

            //cursor.transform.position = new Vector3(-touchWorldPos.x, 1.2f, -touchWorldPos.z -20.0f);
            Vector3 pos = new Vector3(-touchWorldPos.x, 1.2f, -touchWorldPos.z - 20.0f);

            //Debug.Log("World Pos: " + pos);

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("TouchPhase Started");
                // Find closest ball
                float minSqrDist = Mathf.Infinity;
                selectedBall = null;

                foreach (GameObject ball in balls)
                {
                    float sqrDist = (ball.transform.position - pos).sqrMagnitude;
                    if (sqrDist < minSqrDist)
                    {
                        minSqrDist = sqrDist;
                        selectedBall = ball;
                    }
                }
                Debug.Log("Name: " + selectedBall.transform.name + " SQRDist: " +minSqrDist);

                if (minSqrDist > sensitivity)
                {
                    selectedBall = null;
                }

                if (selectedBall != null)
                {
                    Debug.Log("Selected ball: " + selectedBall.transform.name);
                    selectedBall.GetComponent<Collider>().enabled = false;
                    //selectedBall.transform.position = pos;
                }
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Debug.Log("TouchPhase Moved");
                if (selectedBall != null)
                {

                    if (pos.x > 8.45 || pos.x < -8.45
                        || pos.z > -6.45 || pos.z < -13.66)
                    {
                        //selectedBall.GetComponent<Collider>().enabled = true;
                        //selectedBall = null;
                    }
                    else
                    {
                        selectedBall.transform.position = pos;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("TouchPhase Ended");
                if (selectedBall != null)
                {
                    selectedBall.GetComponent<Collider>().enabled = true;
                }
                selectedBall = null;
            }
        }
    }

}
