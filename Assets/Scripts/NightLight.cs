using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightLight : MonoBehaviour
{
    float randomStart;

    private void Start()
    {
        randomStart = Random.Range(-GameManager.manager.dayLength / 12f, GameManager.manager.dayLength/12f);
        GetComponent<Light>().enabled = Mathf.Abs(Mathf.Sin(Mathf.PI * Time.time / GameManager.manager.dayLength)) > 0.5f + randomStart;
        StartCoroutine(NightCycle());
    }

    private IEnumerator NightCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.manager.dayLength/4f + randomStart);
            GetComponent<Light>().enabled = Mathf.Abs(Mathf.Sin(Mathf.PI * Time.time / GameManager.manager.dayLength)) > 0.5f + randomStart;
        }
    }
}
