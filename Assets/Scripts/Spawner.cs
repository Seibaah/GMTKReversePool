using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool isDone;
    public int numBalls;

    public GameObject[] balls;

    private void Start()
    {
        for (int i = 0; i < numBalls; i++)
        {
            float x = UnityEngine.Random.Range(-8.30f, 8.3f);
            float z = UnityEngine.Random.Range(-6.3f, -13.4f);
            balls[i].transform.position = new Vector3(x, 1.2f, z);
            balls[i].GetComponent<SphereHit>().isFadingIn = true;
            balls[i].GetComponent<Collider>().enabled = true;
        }
    }

    public void SpawnMissingBalls()
    {
        for (int i = 0; i < numBalls; i++)
        {
            if (balls[i].GetComponent<SphereHit>().isInPocket)
            {
                float x = UnityEngine.Random.Range(-8.30f, 8.3f);
                float z = UnityEngine.Random.Range(-6.3f, -13.4f);
                balls[i].transform.position = new Vector3(x, 1.2f, z);
                balls[i].GetComponent<SphereHit>().isFadingIn = true;
                balls[i].GetComponent<Collider>().enabled = true;
                balls[i].GetComponent<SphereHit>().isInPocket = false;
                var rb = balls[i].GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

}
