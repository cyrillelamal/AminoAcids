using System;
using System.Globalization;
using UnityEngine;

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

        private CAtom()
        {
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
            switch (symbol.Trim())
            {
                case "O": return Color.Lerp(Color.red, Color.gray, K);
                case "N": return Color.Lerp(Color.blue, Color.gray, K);
                case "C": return Color.Lerp(Color.black, Color.gray, K);
                case "H": return Color.Lerp(Color.white, Color.gray, K);
            }

            return Color.Lerp(Color.green, Color.green, K);
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
            switch (symbol.Trim())
            {
                case "O": return 0.6F;
                case "N": return 0.71F;
                case "C": return 0.76F;
                case "H": return 0.46F;
            }

            return 0.1F;
        }

        public override string ToString() => $"{stringname} ({x}, {y}, {z})"; // TODO: build the entire representation.
    }
}