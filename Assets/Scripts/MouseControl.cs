using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        gameObject.GetComponent<Collider>().enabled = false;
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        var mousePos = GetMouseAsWorldPoint();
        if (mousePos.x > 8.45 || mousePos.x < -8.45 
            || mousePos.z > -6.45 || mousePos.z < -13.66)
        {
            OnMouseUp();
        }
        else
        {
            transform.position = mousePos + mOffset;
            //Debug.Log("X: " + mousePos.x + "Z:" + mousePos.z);
        }
    }

    void OnMouseUp()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
