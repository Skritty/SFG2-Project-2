using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectInfo : MonoBehaviour
{
    [SerializeField] UICommandElement[] UIElementsInner;
    [SerializeField] UICommandElement[] UIElementsOuter;
    [SerializeField] float innerOffset;
    [SerializeField] float outerOffset;

    private void Start()
    {
        foreach (UICommandElement e in UIElementsInner)
        {
            e.gameObject.SetActive(false);
        }
        foreach (UICommandElement e in UIElementsOuter)
        {
            e.gameObject.SetActive(false);
        }
    }

    public void EnableElements(SelectUIElements elements)
    {
        Vector3 offsetDir = Vector3.up;
        int totalInner = 0;
        int totalOuter = 0;

        // Disable everything and see how many elements total there are
        foreach (UICommandElement e in UIElementsInner)
        {
            e.gameObject.SetActive(false);
            if (elements.HasFlag(e.element)) totalInner++;
        }
        foreach (UICommandElement e in UIElementsOuter)
        {
            e.gameObject.SetActive(false);
            if (elements.HasFlag(e.element)) totalOuter++;
        }

        innerOffset = (totalInner - 1) * 15 + 10;
        outerOffset = innerOffset + 50;

        // Enable and position all the elements
        int i = 0;
        foreach (UICommandElement e in UIElementsInner)
        {
            if (elements.HasFlag(e.element))
            {
                offsetDir = Quaternion.AngleAxis(360f / totalInner * i, Vector3.forward) * Vector3.up;
                e.transform.position = transform.position + offsetDir * innerOffset;
                e.gameObject.SetActive(true);
                i++;
            }
        }
        i = 0;
        foreach (UICommandElement e in UIElementsOuter)
        {
            if (elements.HasFlag(e.element))
            {
                offsetDir = Quaternion.AngleAxis(360f / totalOuter * i, Vector3.forward) * Vector3.up;
                e.transform.position = transform.position + offsetDir * outerOffset;
                e.gameObject.SetActive(true);
                i++;
            }
        }
    }
}
