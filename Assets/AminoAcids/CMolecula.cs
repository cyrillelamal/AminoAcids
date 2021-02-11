namespace AminoAcids
{
    public class CMolecula // Molecule?
    {
        public CAtom[] Atoms { get; }
        public string Name { get; }

        // public GameObject Handle { get; private set; } // The container

        /// <summary>
        /// Molecules are composed of atoms.
        /// </summary>
        /// <param name="atoms">
        /// The atoms that compose the molecule.
        /// </param>
        /// <param name="name">
        /// The name of the molecule.
        /// </param>
        public CMolecula(CAtom[] atoms, string name)
        {
            Atoms = atoms;
            Name = name;
        }
    }
}