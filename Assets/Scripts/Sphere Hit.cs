using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHit : MonoBehaviour
{
    public float hitPower = 1100f;
    public bool wasHit = true;

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
            collision.gameObject.GetComponent<Rigidbody>().velocity *= 0.5f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Pocket");
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
    }
}
