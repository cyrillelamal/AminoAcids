using System.IO;
using System.Linq;
using UnityEngine;

public class CMenuBuilder : MonoBehaviour
{
    // Files
    private const string Dir = @"Assets\Resources\AminoAcids";
    private const string Ext = ".pdb";

    // Buttons
    private const int ButtonWidth = 50;
    private const int ButtonHeight = 30;
    private const int XMargin = 5;
    private const int YMargin = 4;

    // Representations
    private const float Velocity = 0.1F;
    private const int Scale = 90;

    private string[] _files;
    private Rect[] _rects; // The buttons

    private CMoleculeRepresentation _handler;

    private Vector3 _previousMousePosition;

    private void Awake()
    {
        SetPreviousMousePosition(Input.mousePosition);
    }

    private void Start()
    {
        var files = EnumerateFiles();
        SetFiles(files);

        var rects = new Rect[files.Length];

        for (int i = 0, y = 0; i < rects.Length; i++)
        {
            rects[i] = new Rect(XMargin, YMargin + y, ButtonWidth, ButtonHeight);
            y += ButtonHeight;
        }

        SetRects(rects);
    }

    private void OnGUI()
    {
        for (var i = 0; i < GetRects().Length; i++)
        {
            var rect = GetRects()[i];
            var file = GetFiles()[i];
            var txt = Path.GetFileNameWithoutExtension(file);

            if (GUI.Button(rect, txt))
            {
                GetHandler()?.Destroy();

                var handler = new CMoleculeRepresentation(file);

                SetHandler(handler);
                GetHandler().Display(Scale);
            }
        }
    }

    private void Update()
    {
        if (GetHandler() is null) return; // The user has not yet chosen a molecule.

        // Scale
        if (Input.mouseScrollDelta.y != 0) GetHandler().ScaleWithWheel(Input.mouseScrollDelta.y);

        // Rotations
        var cp = Input.mousePosition;
        var pp = GetPreviousMousePosition();
        if (Input.GetMouseButton(0)) GetHandler().RotateWithMouse(cp, pp, Velocity);

        // Movements
        if (Input.GetKey(KeyCode.UpArrow)) GetHandler().MoveWithArrows(KeyCode.UpArrow, Velocity);
        if (Input.GetKey(KeyCode.RightArrow)) GetHandler().MoveWithArrows(KeyCode.RightArrow, Velocity);
        if (Input.GetKey(KeyCode.DownArrow)) GetHandler().MoveWithArrows(KeyCode.DownArrow, Velocity);
        if (Input.GetKey(KeyCode.LeftArrow)) GetHandler().MoveWithArrows(KeyCode.LeftArrow, Velocity);

        SetPreviousMousePosition(cp); // Unconditional
    }

    private void SetFiles(string[] files) => _files = files;

    private string[] GetFiles() => _files;

    private void SetRects(Rect[] rects) => _rects = rects;

    private Rect[] GetRects() => _rects;

    private void SetHandler(CMoleculeRepresentation handler) => _handler = handler;

    private CMoleculeRepresentation GetHandler() => _handler;

    private void SetPreviousMousePosition(Vector3 pp) => _previousMousePosition = pp;

    private Vector3 GetPreviousMousePosition() => _previousMousePosition;

    /// <summary>
    /// Enumerate all corresponding files in the watched directory.
    /// </summary>
    /// <returns>
    /// The list of fully-qualified paths.
    /// </returns>
    private static string[] EnumerateFiles()
    {
        var ext = Ext.StartsWith(".") ? Ext : $".{Ext}";
        var glob = $"*{ext}";

        return Directory
            .EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), Dir), glob)
            .ToArray();
    }
}