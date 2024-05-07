using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    double time;
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            Destroy(this.gameObject);
            Debug.Log("tweak");
        }
    }
}
