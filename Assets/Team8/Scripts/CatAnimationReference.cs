using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class CatAnimationReference : MonoBehaviour
    {
        [SerializeField]
        private IngridientSpawner shopKeeper;
        public void TakeIngridient()
        {
            shopKeeper.TakeIngridient();
        }

        public void ThrowIngridient()
        {
            shopKeeper.ThrowIngridient();
        }
    }
}
