using System.IO;
using System.Linq;
using AminoAcids;
using UnityEngine;

public class CMoleculeRepresentation
{
    // The initial position of the molecule is little strange.
    // To see the molecule well, we have to move it a little.
    private const float XBias = 100F;
    private const float YBias = 100F;
    private const float ZBias = -100F;

    private CMolecula Molecule { get; }

    private GameObject Handler { get; set; } // The wrapper for atoms
    private CAtomRepresentation[] AtomRepresentations { get; set; } // The spheres

    /// <summary>
    /// Create a new molecule representation using the contents of a .pdb file.
    /// </summary>
    /// <param name="file">
    /// The .pdb file, that the molecule is parsed from.
    /// </param>
    public static CMoleculeRepresentation ParseFromFile(string file)
    {
        var name = Path.GetFileNameWithoutExtension(file);
        var atoms = (
            from line in File.ReadLines(file)
            where line.StartsWith("ATOM")
            select CAtom.ParseFromString(line)
        ).ToArray();

        return new CMoleculeRepresentation(new CMolecula(atoms, name));
    }

    private CMoleculeRepresentation(CMolecula molecule)
    {
        Molecule = molecule;

        AtomRepresentations = new CAtomRepresentation[Molecule.Atoms.Length];
    }

    /// <summary>
    /// Display the spheres.
    /// </summary>
    /// <param name="baseScale">
    /// The scale of the spheres.
    /// </param>
    public void Display(float baseScale)
    {
        Handler = new GameObject {name = Molecule.Name};

        for (var i = 0; i < Molecule.Atoms.Length; i++)
        {
            var ar = new CAtomRepresentation(Molecule.Atoms[i]);

            AtomRepresentations[i] = ar;
            ar.SetParent(Handler);
            ar.Display();
        }

        Scale(new Vector3(baseScale, baseScale, baseScale));
        Centralize();
    }

    /// <summary>
    /// Destroy the spheres.
    /// </summary>
    public void Destroy()
    {
        foreach (var ar in AtomRepresentations) ar.Destroy();

        Object.Destroy(Handler);
    }

    /// <summary>
    /// Scale the handler using some scale rate.
    /// </summary>
    /// <param name="delta">
    /// The scale rate delta.
    /// </param>
    public void ScaleWithWheel(float delta)
    {
        var ret = Handler.transform.localScale + new Vector3(delta, delta, delta);

        if (ret.x > 0 && ret.y > 0 && ret.z > 0) Scale(ret);
    }

    /// <summary>
    /// Rotate the handler using the mouse movements.
    /// </summary>
    /// <param name="cp">
    /// The current mouse position.
    /// </param>
    /// <param name="pp">
    /// The previous mouse position.
    /// </param>
    /// <param name="velocity">
    /// The speed coefficient.
    /// </param>
    public void RotateWithMouse(Vector3 cp, Vector3 pp, float velocity)
    {
        var diff = (cp - pp) * velocity;

        if (diff.x == 0 && diff.y == 0) return;

        Handler.transform.Rotate(new Vector3(diff.y, diff.x, 0));
    }

    /// <summary>
    /// Move the handler with arrow keys.
    /// </summary>
    /// <param name="keyCode">
    /// The pressed key.
    /// </param>
    /// <param name="velocity">
    /// The speed coefficient.
    /// </param>
    public void MoveWithArrows(KeyCode keyCode, float velocity)
    {
        switch (keyCode)
        {
            case KeyCode.UpArrow:
                Handler.transform.position += Vector3.down * velocity;
                break;
            case KeyCode.RightArrow:
                Handler.transform.position += Vector3.left * velocity;
                break;
            case KeyCode.DownArrow:
                Handler.transform.position += Vector3.up * velocity;
                break;
            case KeyCode.LeftArrow:
                Handler.transform.position += Vector3.right * velocity;
                break;
        }
    }

    public void Rotate(Quaternion rot)
    {
        
    }

    /// <summary>
    /// Update the scale rate of the handler.
    /// </summary>
    /// <param name="s">
    /// The new scale rate.
    /// </param>
    private void Scale(Vector3 s)
    {
        Handler.transform.localScale = s;
    }

    private void Centralize()
    {
        if (Camera.main is null) return;

        var wp = Camera.main.WorldToScreenPoint(Handler.transform.position);

        Handler.transform.position = new Vector3(wp.x + XBias, wp.y + YBias, ZBias);
    }
}