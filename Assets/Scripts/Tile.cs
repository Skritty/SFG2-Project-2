using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Dictionary<Renderer, List<Material>> initialMaterials = new Dictionary<Renderer, List<Material>>();
    public int x;
    public int y;
    Construction _contains = null;
    public Construction contains { get { return _contains; } set { _contains = value; } }
    Construction _rail = null;
    public Construction rail { get { return _rail; } set { _rail = value; } }

    // Tile Buffs
    public int buildsRemaining;

    private void Start()
    {
        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            if(r.tag == "Tile")
            {
                initialMaterials.Add(r, new List<Material>());
                initialMaterials[r].Add(r.sharedMaterial);
            }
        }
    }

    public void SetMaterial(Material m)
    {
        foreach (KeyValuePair<Renderer, List<Material>> r in initialMaterials)
        {
            if(!initialMaterials.ContainsKey(r.Key)) initialMaterials.Add(r.Key, new List<Material>());
            initialMaterials[r.Key].Add(m);
            r.Key.sharedMaterial = m;
        }
    }

    public void RemoveMaterial(Material m)
    {
        foreach (KeyValuePair<Renderer, List<Material>> r in initialMaterials)
        {
            r.Value.Remove(m);
            r.Key.sharedMaterial = r.Value[r.Value.Count - 1];
        }
    }

    public void ResetBuffs()
    {
        buildsRemaining = 1;
    }

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }
}
