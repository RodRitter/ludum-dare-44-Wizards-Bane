using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{

    public float secondsUntilDeath = 5;

    void Start()
    {
        StartCoroutine(StartCounter());
    }

    IEnumerator StartCounter()
    {
        yield return new WaitForSeconds(secondsUntilDeath);
        Destroy(gameObject);
    }
}
