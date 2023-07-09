using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CueHit : MonoBehaviour
{
    //states
    public bool isFadingIn;
    public bool isDeciding;
    public bool isAligning;
    public bool isRotating;
    public bool isPausing;
    public bool isAiming;
    public bool isShooting;
    public bool isFadingOut;
    public bool isResting;

    public GameObject cueBall; //our pivot
    public float hitAngle = 0f;
    public float tolerance = 25f;
    Vector3 dir;

    float t0;
    float pauseTime = 1f;
    float aimTime = 0.5f;
    float hitTime = 0.07f;
    float restTime = 2f;
    float fadeSpeed = 1f;
    float hitSpeed = 20f;

    public Renderer renderer;
    public GameObject stickGo;


    void Start()
    {
        isFadingIn = true;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, (cueBall.transform.position - transform.position).normalized,
            Color.red, 1);
/*
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Shoot");
            isShooting = true;
            dir = (cueBall.transform.position - transform.position).normalized;
            hitT0 = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            var col = renderer.material.color;
            gameObject.transform.position -= new Vector3(0f, 2f, 0f);
            renderer.material.color = new Color(col.r, col.g, col.b, 1f);
        }*/
    }

    void FixedUpdate()
    {
        if (isFadingIn)
        {
            Fade("in");
        }
        else if (isDeciding)
        {
            hitAngle = UnityEngine.Random.Range(0, 360);
            isDeciding = false;
            isRotating = true;
            Debug.Log("ANGLE: " + hitAngle);
        }
        else if (isRotating)
        {
            Debug.Log("CurAngle: " + stickGo.transform.rotation.eulerAngles.y);
            if (stickGo.transform.rotation.eulerAngles.y % 360 > hitAngle - tolerance &&
                stickGo.transform.rotation.eulerAngles.y % 360 < hitAngle + tolerance)
            {
                isRotating = false;
                isPausing = true;
                t0 = Time.time;
            }

            stickGo.transform.RotateAround(cueBall.transform.position, Vector3.up, 50 * Time.deltaTime);
        }
        else if (isPausing)
        {
            if (Time.time - t0 > pauseTime)
            {
                isPausing = false;
                isAiming = true;
                t0 = Time.time;
            }
        }
        else if (isAiming) {
            
            if (Time.time - t0 > aimTime)
            {
                isAiming = false;
                isShooting = true;
                t0 = Time.time;
            }

            //TODO wrong direction
            dir = (cueBall.transform.position - stickGo.transform.position).normalized;
            stickGo.transform.Translate((stickGo.transform.TransformPoint(stickGo.transform.position) 
                - stickGo.transform.TransformPoint(cueBall.transform.position)).normalized * 5 * Time.deltaTime);
        }
        else if (isShooting)
        {
            stickGo.transform.Translate((stickGo.transform.TransformPoint(cueBall.transform.position) 
                - stickGo.transform.TransformPoint(stickGo.transform.position)).normalized * hitSpeed * Time.deltaTime);
            
            if (Time.time - t0 > hitTime)
            {
                isShooting = false;
                isFadingOut = true;
            }
        }
        else if (isFadingOut)
        {
            if (renderer.material.color.a <= 0)
            {
                isFadingOut = false;
                isResting = true;
                t0 = Time.time;
            }

            Fade("out");
        }
        else if (isResting)
        {
            if (Time.time - t0 < restTime)
            {
                isResting = false;
                isFadingIn = true;
            }
        }
    }

    void Fade(string mode)
    {
        if (mode == "out")
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
        else if (mode == "in")
        {
            var col = renderer.material.color;
            float fadeAmount = col.a + (fadeSpeed * Time.deltaTime);
            var newCol = new Color(col.r, col.g, col.b, fadeAmount);

            renderer.material.color = newCol;
            if (col.a >= 1)
            {
                isFadingIn = false;
                gameObject.transform.position -= new Vector3(0f, 2f, 0f);
                isDeciding = true;
            }
        }
    }
}
