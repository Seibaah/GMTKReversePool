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
            Debug.Log("HIT");
            gameObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.position 
                - collision.gameObject.GetComponent<CueHit>().tip.transform.position).normalized * hitPower);
            collision.gameObject.GetComponentInParent<CueHit>().isFadingOut = true;
            wasHit = true;
        }
    }

    void FixedUpdate()
    {
        Debug.Log(gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude);
        if (gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < 1.2f ) {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
