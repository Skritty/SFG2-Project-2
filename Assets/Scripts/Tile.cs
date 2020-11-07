using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Dictionary<Renderer, Material> initialMaterials = new Dictionary<Renderer, Material>();
    public int x;
    public int y;
    public List<Construction> contains = new List<Construction>();

    private void Start()
    {
        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            if(r.tag == "Tile")
            {
                initialMaterials.Add(r, r.sharedMaterial);
            }
        }
    }

    public void SetMaterial(Material m)
    {
        foreach (KeyValuePair<Renderer, Material> r in initialMaterials)
        {
            r.Key.sharedMaterial = m;
        }
    }

    public void ResetMaterial()
    {
        foreach (KeyValuePair<Renderer, Material> r in initialMaterials)
        {
            r.Key.sharedMaterial = r.Value;
        }
    }
}
