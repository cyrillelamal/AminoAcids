using System.IO;
using AminoAcids;
using UnityEngine;

public class CMoleculeRepresentation
{
    // The initial position of the molecule is little strange.
    // To see the molecule well, we have to move it a little.
    private const float XBias = 100F;
    private const float YBias = 100F;
    private const float ZBias = -100F;

    private readonly CMolecula _molecule;

    /// <summary>
    /// Create a new molecule representation using the contents of a .pdb file.
    /// </summary>
    /// <param name="file">
    /// The .pdb file, that the molecule is parsed from.
    /// </param>
    public static CMoleculeRepresentation ParseFromFile(string file)
    {
        var name = Path.GetFileNameWithoutExtension(file);
        var atoms = CAtom.ParseFromFile(file);

        return new CMoleculeRepresentation(atoms, name);
    }

    private CMoleculeRepresentation(CAtom[] atoms, string name)
    {
        _molecule = new CMolecula(atoms, name);
    }

    /// <summary>
    /// Display the spheres.
    /// </summary>
    /// <param name="baseScale">
    /// The scale of the spheres.
    /// </param>
    public void Display(float baseScale)
    {
        // The order of calling is important!
        GetMolecule().Display();

        Scale(new Vector3(baseScale, baseScale, baseScale));
        Centralize();
    }

    /// <summary>
    /// Destroy the spheres.
    /// </summary>
    public void Destroy()
    {
        Object.Destroy(GetMolecule().Handle);
    }

    /// <summary>
    /// Scale the handler using some scale rate.
    /// </summary>
    /// <param name="delta">
    /// The scale rate delta.
    /// </param>
    public void ScaleWithWheel(float delta)
    {
        var handler = GetMolecule().Handle;

        var ret = handler.transform.localScale + new Vector3(delta, delta, delta);

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

        GetMolecule().Handle.transform.Rotate(new Vector3(diff.y, diff.x, 0));
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
        var handler = GetMolecule().Handle;

        switch (keyCode)
        {
            case KeyCode.UpArrow:
                handler.transform.position += Vector3.down * velocity;
                break;
            case KeyCode.RightArrow:
                handler.transform.position += Vector3.left * velocity;
                break;
            case KeyCode.DownArrow:
                handler.transform.position += Vector3.up * velocity;
                break;
            case KeyCode.LeftArrow:
                handler.transform.position += Vector3.right * velocity;
                break;
        }
    }

    /// <summary>
    /// Update the scale rate of the handler.
    /// </summary>
    /// <param name="s">
    /// The new scale rate.
    /// </param>
    private void Scale(Vector3 s)
    {
        GetMolecule().Handle.transform.localScale = s;
    }

    private void Centralize()
    {
        if (Camera.main is null) return;

        var handler = GetMolecule().Handle;
        var wp = Camera.main.WorldToScreenPoint(handler.transform.position);

        handler.transform.position = new Vector3(wp.x + XBias, wp.y + YBias, ZBias);
    }

    private CMolecula GetMolecule() => _molecule;
}