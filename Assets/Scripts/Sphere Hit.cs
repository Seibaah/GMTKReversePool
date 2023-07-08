using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHit : MonoBehaviour
{
    public float hitPower = 900f; 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cue Stick")
        {
            Debug.Log("HIT");
            gameObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.position - collision.GetContact(0).point).normalized * hitPower);
            collision.gameObject.GetComponentInParent<CueHit>().isFadingOut = true;
        }
    }
}
