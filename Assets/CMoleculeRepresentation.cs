using System.IO;
using AminoAcids;
using UnityEngine;

public class CMoleculeRepresentation
{
    // The initial position of the molecule is little strange.
    // To see the molecule well, we have to move it a little.
    private const float YBias = 50F;
    private const float ZBias = -100F;

    private readonly string _file;

    private CMolecula _molecule;

    public CMoleculeRepresentation(string file)
    {
        _file = file;
    }

    /// <summary>
    /// Display the spheres.
    /// </summary>
    /// <param name="baseScale">
    /// The scale of the spheres.
    /// </param>
    public void Display(float baseScale)
    {
        var atoms = CAtom.ParseFromFile(GetFile());
        var name = Path.GetFileNameWithoutExtension(GetFile());
        var molecule = new CMolecula(atoms, name);

        SetMolecule(molecule);

        molecule.Display(); // Order is important!

        Scale(baseScale);
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

        if (ret.x > 0 && ret.y > 0 && ret.z > 0) handler.transform.localScale = ret;
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

    private void Scale(float s)
    {
        GetMolecule().Handle.transform.localScale = new Vector3(s, s, s);
    }

    private void Centralize()
    {
        if (Camera.main is null) return;

        var handler = GetMolecule().Handle;
        var wp = Camera.main.WorldToScreenPoint(handler.transform.position);

        handler.transform.position = new Vector3(wp.x, wp.y + YBias, +ZBias);
    }

    private string GetFile() => _file;

    private void SetMolecule(CMolecula molecule) => _molecule = molecule;

    private CMolecula GetMolecule() => _molecule;
}