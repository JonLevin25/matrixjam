using System;
using System.Collections;
using System.Collections.Generic;
using MatrixJam.Team14;
using UnityEngine;

public class DelayShow : MonoBehaviour
{
    [SerializeField] private float secs;
    [SerializeField] private GameObject obj;

    private void OnEnable()
    {
        obj.SetActive(false);
        StartCoroutine(WaitAndShow(secs));
    }

    private void OnDisable()
    {
        StopAllCoroutines(); // Redundant?
    }

    private IEnumerator WaitAndShow(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(true);
    }
}
