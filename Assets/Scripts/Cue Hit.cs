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
    public bool isWaitingForSpawn;

    public GameObject cueBall; //our pivot
    public float hitAngle = 0f;
    public float tolerance = 10f;
    public Vector3 dir;

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
    public LineRenderer lr;
    public Vector3 lrDir;
    public Spawner sp;

    void Start()
    {
        isFadingIn = true;
    }

    void Update()
    {
        if (lr.enabled == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(cueBall.transform.position, lrDir, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                lr.SetPosition(0, new Vector3(cueBall.transform.position.x, 1.2f, cueBall.transform.position.z));
                lr.SetPosition(1, new Vector3(hit.point.x, 1.2f, hit.point.z));
            }
        }
    }

    void FixedUpdate()
    {
        //Debug.Log("CurAngle: " + transform.rotation.eulerAngles.y);
        // For paused scenarios (Static variable)
        if (PauseMenu.IsPaused || TutorialMenu.IsTutorial || GameOverScreen.IsGameOver) return;
        
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
            hitAngle = UnityEngine.Random.Range(0, 359);
            isDeciding = false;
            isRotating = true;
            Debug.Log("ANGLE: " + hitAngle);

            int quadrant = (int)(hitAngle / 90);
            Debug.Log("Quadrant " + quadrant);

            float x, z;
            if (quadrant == 0)
            {
                z = Mathf.Cos((hitAngle) * Mathf.PI / 180);
                x = Mathf.Sin((hitAngle) * Mathf.PI / 180);
            }
            else if (quadrant == 1)
            {
                x = Mathf.Cos((hitAngle - 90) * Mathf.PI / 180);
                z = - Mathf.Sin((hitAngle - 90) * Mathf.PI / 180);
            }
            else if (quadrant == 2)
            {
                z = - Mathf.Cos((hitAngle - 180) * Mathf.PI / 180);
                x = - Mathf.Sin((hitAngle - 180) * Mathf.PI / 180);
            }
            else
            {
                x = - Mathf.Cos((hitAngle - 270) * Mathf.PI / 180);
                z = Mathf.Sin((hitAngle - 270) * Mathf.PI / 180);
            }


            lrDir = (new Vector3(cueBall.transform.position.x + x, 1.2f, cueBall.transform.position.z + z) 
                - new Vector3(cueBall.transform.position.x, 1.2f, cueBall.transform.position.z)).normalized;
            lr.enabled = true;
        }
        else if (isRotating)
        {
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
            lr.enabled = false;

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
            if (cueBall.gameObject.GetComponent<Rigidbody>().velocity == Vector3.zero) 
            {
                isResting = false;
                isWaitingForSpawn = true;
            }
        }
        else if (isWaitingForSpawn)
        {
            sp.SpawnMissingBalls();
            isWaitingForSpawn = false;
            isFadingIn = true;
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
