using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [SerializeField] Vector3 axisOfRotation;

    private void Update()
    {
        transform.Rotate(axisOfRotation, 360f / GameManager.manager.dayLength * Time.deltaTime);
    }
}
