//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("triggers")]
    [AutoDoc("This component destroys the attached GameObject once the attacked ParticleSystem reaches a state where IsAlive(true) returns false.")]
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterParticleDeath : MonoBehaviour
    {

        #region Fields

        private ParticleSystem particles;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            particles = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!particles.IsAlive(true))
            {
                InterfaceManager.ObjectManagement.DestroyObject(gameObject);
            }
        }

        #endregion

    }
}