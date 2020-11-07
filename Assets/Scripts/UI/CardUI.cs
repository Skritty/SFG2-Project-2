using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This script goes onto the root gameobject of a card to manage all of its innter parts.
/// </summary>
public class CardUI : MonoBehaviour
{
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI powerCost;
    public Sprite image;
    public GameObject selectBorder;
    public GameObject backface;
    public bool upsideDown = true;
    public Construction info;

    Vector3 initialPosition;
    Vector3 initialScale;
    Quaternion initialRotation;

    private void Start()
    {
        UpdateInitialTransform();
    }

    public void Flip()
    {
        upsideDown = !upsideDown;
        backface.SetActive(!upsideDown);
    }

    public void Flip(bool down)
    {
        upsideDown = down;
        backface.SetActive(down);
    }

    public void UpdateInitialTransform()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        initialRotation = transform.rotation;
    }

    public void ResetTransform()
    {
        transform.position = initialPosition;
        transform.localScale = initialScale;
        transform.rotation = initialRotation;
    }

    public void Transform(Vector3 newPos, float scaleMulti, bool posRelativeFromInitial = false)
    {
        if (posRelativeFromInitial) transform.position = initialPosition + newPos;
        else transform.position = newPos;
        transform.localScale = initialScale * scaleMulti;
    }
}
