using AminoAcids;
using UnityEngine;

public class CAtomRepresentation
{
    private CAtom Atom { get; }

    private GameObject Sphere { get; }

    public CAtomRepresentation(CAtom atom)
    {
        Atom = atom;

        Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    /// <summary>
    /// Build the graphical representation and place it on the screen.
    /// </summary>
    public void Display()
    {
        Sphere.name = Atom.atomname;
        Sphere.GetComponent<Renderer>().material.color = Atom.GetColor();
        Sphere.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
        Sphere.transform.localScale = new Vector3(Atom.GetRadius(), Atom.GetRadius(), Atom.GetRadius());
        Sphere.transform.position = new Vector3(Atom.x, Atom.y, Atom.z);
    }

    /// <summary>
    /// Destroy the graphical representation.
    /// </summary>
    public void Destroy()
    {
        Object.Destroy(Sphere);
    }

    /// <summary>
    /// Attach the atom to another game object.
    /// </summary>
    /// <param name="handler">
    /// The new parent.
    /// </param>
    public void SetParent(GameObject handler)
    {
        Sphere.transform.parent = handler.transform;
    }
}