using System;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AminoAcids
{
    public class CAtom
    {
        // This is used to distinguish commas and dots in parsed files.
        private static readonly IFormatProvider FormatProvider = new CultureInfo("en-US");

        private const float K = 0.25f; // color coefficient

        public string stringname; // 01-06 "ATOM" // 00-05 "ATOM  "
        public int number; // 07-11 atom serial number
        public string atomname; // 13-16 atom name
        public char altLoc; // 17    alternate location indicator
        public string residue; // 18-20 residue name
        public string chain_id; // 22    chain ID
        public int nresidue; // 23-26 residue sequence number
        public char iCode; // 27    code for insertion of residues 
        public float x; // 31-38 coord x
        public float y; // 39-46 coord y
        public float z; // 47-54 coord z
        public float occupancy; // 55-60 occupancy
        public float temp; // 61-66 temperature factor
        public string symbol; // 77-78 symbol
        public string charge; // 79-80 charge of the atom

        public GameObject Sphere { get; private set; }

        /// <summary>
        /// A stunted factory method for atom models.
        /// </summary>
        /// <param name="str">
        /// The line that atom's parameters are parsed from.
        /// </param>
        /// <returns>
        /// An atom parsed from the passed line.
        /// </returns>
        public static CAtom ParseFromString(string str)
        {
            return new CAtom
            {
                stringname = str.Substring(0, 6).Trim(),
                number = Convert.ToInt32(str.Substring(6, 5).Trim(), FormatProvider),
                atomname = str.Substring(12, 4).Trim(),
                altLoc = str.Substring(16, 1)[0],
                residue = str.Substring(17, 3).Trim(),
                chain_id = str.Substring(21, 1).Trim(),
                nresidue = Convert.ToInt32(str.Substring(22, 4).Trim(), FormatProvider),
                iCode = str.Substring(26, 1)[0],
                x = (float) Convert.ToDouble(str.Substring(30, 8).Trim(), FormatProvider),
                y = (float) Convert.ToDouble(str.Substring(38, 8).Trim(), FormatProvider),
                z = (float) Convert.ToDouble(str.Substring(46, 8).Trim(), FormatProvider),
                occupancy = (float) Convert.ToDouble(str.Substring(54, 6).Trim(), FormatProvider),
                temp = (float) Convert.ToDouble(str.Substring(60, 6).Trim(), FormatProvider),
                symbol = str.Substring(76, 2),
                charge = " 0"
            };
        }

        /// <summary>
        /// Build atoms by use of the file contents.
        /// </summary>
        /// <param name="path">
        /// The path of the file which contents are used to build atom models.
        /// </param>
        /// <returns>
        /// The list of parsed atoms.
        /// </returns>
        public static CAtom[] ParseFromFile(string path)
        {
            return (
                from line in File.ReadLines(path)
                where line.StartsWith("ATOM")
                select ParseFromString(line)
            ).ToArray();
        }

        /// <summary>
        /// Build the graphical representation and place it on the screen.
        /// </summary>
        public void Display()
        {
            Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            Sphere.name = atomname;
            Sphere.GetComponent<Renderer>().material.color = GetColor();
            Sphere.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
            Sphere.transform.localScale = new Vector3(GetRadius(), GetRadius(), GetRadius());
            Sphere.transform.position = new Vector3(-x, y, z);
        }

        /// <summary>
        /// Destroy the graphical representation.
        /// </summary>
        public void DestroySphere()
        {
            Object.Destroy(Sphere);
        }

        /// <summary>
        /// Get the color based on the type of the element.
        /// If the element is not supported, the method returns the green color.
        /// </summary>
        /// <returns>
        /// The color based on the type of the element. 
        /// </returns>
        public Color GetColor()
        {
            var color = Color.Lerp(Color.green, Color.green, K);

            switch (symbol.Trim())
            {
                case "O":
                    color = Color.Lerp(Color.red, Color.gray, K);
                    break;
                case "N":
                    color = Color.Lerp(Color.blue, Color.gray, K);
                    break;
                case "C":
                    color = Color.Lerp(Color.black, Color.gray, K);
                    break;
                case "H":
                    color = Color.Lerp(Color.white, Color.gray, K);
                    break;
            }

            return color;
        }

        /// <summary>
        /// Get the radius value based on the type of the element.
        /// If the element is not supported, the method returns 0.1F.
        /// </summary>
        /// <returns>
        /// The radius value based on the type of the element. 
        /// </returns>
        public float GetRadius()
        {
            var radius = 0.1F;

            switch (symbol.Trim())
            {
                case "O":
                    radius = 0.6F;
                    break;
                case "N":
                    radius = 0.71F;
                    break;
                case "C":
                    radius = 0.76F;
                    break;
                case "H":
                    radius = 0.46F;
                    break;
            }

            return radius;
        }

        public override string ToString() => $"{stringname} ({x}, {y}, {z})"; // TODO: build the entire representation.
    }
}