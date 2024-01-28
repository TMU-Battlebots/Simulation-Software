using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    /*A marker only has finite time before it dissapears
    afterwards it can set a newer marker or transition to move to middle
    */
    [SerializeField] float expTime;
    private float timer;

    void OnEnable()
    {
        StartCoroutine(Countdown(expTime));
    }
    void FixedUpdate()
    {
        
    }

    IEnumerator Countdown(float f)
    {

      yield return new WaitForSeconds(f);
        this.gameObject.SetActive(false);
    }
}
