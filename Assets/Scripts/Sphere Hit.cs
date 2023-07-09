using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHit : MonoBehaviour
{
    public float hitPower = 1100f;
    public bool wasHit = true;
    public Material mat;
    float fadeSpeed = 1/2f;
    public bool isFadingOut;
    public bool isFadingIn;
    public bool isInPocket;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cue Stick")
        {
            gameObject.GetComponent<Rigidbody>().AddForce((collision.gameObject.GetComponent<CueHit>().dir).normalized * hitPower);
            collision.gameObject.GetComponentInParent<CueHit>().isFadingOut = true;
            wasHit = true;
        }
        else if (collision.gameObject.tag == "Ball")
        {
            Debug.DrawRay(collision.gameObject.transform.position, (gameObject.transform.position - collision.gameObject.transform.position).normalized, Color.green, 10);
            var dir = (gameObject.transform.position - collision.gameObject.transform.position).normalized;
            gameObject.GetComponent<Rigidbody>().AddForce(dir * (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude) * 25f);
            collision.gameObject.GetComponent<Rigidbody>().velocity *= 0.75f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hole")
        {
            Debug.Log("Entered Pocket");
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            isFadingOut = true;
            isInPocket = true;
            
            // Increase counter
            ScoreTracker.Instance.ballsJustHitIn += 1;
            print(ScoreTracker.Instance.ballsJustHitIn);
            ScoreTracker.Instance.PlayShot();
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude);
        if (gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < 2f) {
            gameObject.GetComponent<Rigidbody>().velocity *= 0.95f * Time.deltaTime;

        }
        else if (gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < 1f)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            hitPower = 1100f;
        }

        if (isFadingOut)
        {
            Fade("out");
        }
        else if (isFadingIn)
        {
            Fade("in");
        }
    }

    void Fade(string mode)
    {
        if (mode == "out")
        {
            gameObject.GetComponent<Collider>().isTrigger = true;
            Vector3 newScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.zero, Time.deltaTime);
            gameObject.transform.localScale -= fadeSpeed * Time.deltaTime * Vector3.one;

            if (gameObject.transform.localScale.x <= 0f)
            {
                isFadingOut = false;
                gameObject.transform.localScale = Vector3.zero;
            }
        }
        else if (mode == "in")
        {
            gameObject.GetComponent<Collider>().isTrigger = false;
            Vector3 newScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.one, Time.deltaTime);
            gameObject.transform.localScale += fadeSpeed * Time.deltaTime * Vector3.one;

            Debug.Log("Scalex: " + gameObject.transform.localScale);
            if (gameObject.transform.localScale.x >= 0.4)
            {
                isFadingIn = false;
                gameObject.transform.localScale = Vector3.one * 0.4f;
            }
        }
    }
}
