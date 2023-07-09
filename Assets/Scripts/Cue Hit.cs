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
    public float tolerance = 10f;
    Vector3 dir;

    float t0;
    float pauseTime = 1f;
    float aimTime = 0.3f;
    float hitTime = 0.2f;
    float restTime = 2f;
    float fadeSpeed = 2f;
    float hitSpeed = 25f;

    public Renderer renderer;
    public GameObject stick;
    public GameObject tip;

    void Start()
    {
        isFadingIn = true;
    }

    void Update()
    {
        //Debug.DrawRay(transform.position, (cueBall.transform.position - transform.position).normalized,
        //    Color.red, 1);
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
        // For paused scenarios (Static variable)
        if (PauseMenu.IsPaused || TutorialMenu.IsTutorial) return;
        
        if (isFadingIn)
        {
            //TODO make this start once the cue ball has stopped
            transform.position = cueBall.transform.position - new Vector3(6f, 0f, 6f);
            transform.LookAt(cueBall.transform.position);
            Fade("in");
            //Debug.DrawRay(transform.position, cueBall.transform.position - transform.position, Color.red, 10);
        }
        else if (isDeciding)
        {
            hitAngle = UnityEngine.Random.Range(0, 360);
            isDeciding = false;
            isRotating = true;
            //Debug.Log("ANGLE: " + hitAngle);
        }
        else if (isRotating)
        {
            Debug.Log("CurAngle: " + transform.rotation.eulerAngles.y);
            if (transform.rotation.eulerAngles.y % 360 > hitAngle - tolerance &&
                transform.rotation.eulerAngles.y % 360 < hitAngle + tolerance)
            {
                isRotating = false;
                isPausing = true;
                t0 = Time.time;
            }

            transform.RotateAround(cueBall.transform.position, Vector3.up, 50 * Time.deltaTime);
        }
        else if (isPausing)
        {
            if (Time.time - t0 > pauseTime)
            {
                isPausing = false;
                isAiming = true;
                t0 = Time.time;
                dir = (transform.position - cueBall.transform.position).normalized;
                Debug.DrawRay(transform.position, dir, Color.cyan, 10);
            }
        }
        else if (isAiming) {

            if (Time.time - t0 > aimTime)
            {
                isAiming = false;
                isShooting = true;
                t0 = Time.time;
                dir = (cueBall.transform.position - transform.position).normalized;
            }

            transform.Translate(dir * 5 * Time.deltaTime, Space.World);
        }
        else if (isShooting)
        {
            transform.Translate(dir.normalized * hitSpeed * Time.deltaTime, Space.World);
            
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
            gameObject.GetComponent<Collider>().isTrigger = true;
            var col = renderer.material.color;
            float fadeAmount = col.a - (fadeSpeed * Time.deltaTime);
            var newCol = new Color(col.r, col.g, col.b, fadeAmount);

            renderer.material.color = newCol;
            if (col.a <= 0)
            {
                isFadingOut = false;
            }
        }
        else if (mode == "in")
        {
            gameObject.GetComponent<Collider>().isTrigger = false;
            var col = renderer.material.color;
            float fadeAmount = col.a + (fadeSpeed * Time.deltaTime);
            var newCol = new Color(col.r, col.g, col.b, fadeAmount);

            renderer.material.color = newCol;
            if (col.a >= 1)
            {
                isFadingIn = false;
                isDeciding = true;
            }
        }
    }
}
