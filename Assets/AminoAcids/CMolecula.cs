using UnityEngine;

namespace AminoAcids
{
    public class CMolecula // Molecule?
    {
        public string Name { get; }

        public GameObject Handle { get; private set; } // The container

        private CAtom[] Atoms { get; }

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

            Handle = new GameObject {name = name};
        }

        /// <summary>
        /// Display the entire molecule's representation.
        /// The composer pattern delegates this action to its particles.
        /// The result is wrapped into an empty game object.
        /// Every time the method is called, the previous representation is destroyed.
        /// </summary>
        /// <returns>
        /// The created wrapper.
        /// </returns>
        public GameObject Display()
        {
            Destroy();

            foreach (var atom in Atoms) atom.Display();

            return WrapAtoms();
        }

        /// <summary>
        /// Destroy the entire representation of the molecule.
        /// The method also destroys the wrapper (Handler).
        /// The composer pattern delegates the action to its particles.
        /// </summary>
        public void Destroy()
        {
            foreach (var atom in Atoms) atom.DestroySphere();

            Object.Destroy(Handle);
        }

        /// <summary>
        /// Wrap the atoms into an empty game object.
        /// The wrapper element will have the name of the molecule.
        /// </summary>
        /// <returns>
        /// The created wrapper.
        /// </returns>
        public GameObject WrapAtoms()
        {
            Handle = new GameObject {name = Name};

            foreach (var atom in Atoms) atom.Sphere.transform.parent = Handle.transform;

            return Handle;
        }
    }
}