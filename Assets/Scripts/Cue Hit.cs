using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CueHit : MonoBehaviour
{
    //state
    public bool isDeciding;
    public bool isAiming;
    public bool isShooting;
    public bool isFadingOut;

    public GameObject aimingAnchor;
    public Renderer renderer;
    public float hitSpeed = 20f;

    float hitT0;
    float hitTime = 0.07f;
    float fadeSpeed = 1f;
    Vector3 dir;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, (aimingAnchor.transform.position - transform.position).normalized,
            Color.red, 1);

        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Shoot");
            isShooting = true;
            dir = (aimingAnchor.transform.position - transform.position).normalized;
            hitT0 = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            var col = renderer.material.color;
            gameObject.transform.position -= new Vector3(0f, 2f, 0f);
            renderer.material.color = new Color(col.r, col.g, col.b, 1f);
        }
    }

    void FixedUpdate()
    {
        if (Time.time - hitT0 > hitTime)
        {
            isShooting = false;
        }
        else if (isShooting)
        {
             transform.Translate((transform.TransformPoint(aimingAnchor.transform.position) - transform.TransformPoint(transform.position)).normalized * hitSpeed * Time.deltaTime);
        }

        if (isFadingOut)
        {
            var col = renderer.material.color;
            float fadeAmount = col.a - (fadeSpeed * Time.deltaTime);
            var newCol = new Color(col.r, col.g, col.b, fadeAmount);

            renderer.material.color = newCol;
            if (col.a <= 0)
            {
                isFadingOut = false;
                gameObject.transform.position += new Vector3(0f, 2f, 0f);
            }
        }
    }
}
