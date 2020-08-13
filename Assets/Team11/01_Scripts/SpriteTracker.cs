using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class SpriteTracker : MonoBehaviour
    {
        [SerializeReference] public SpriteRenderer Source;

        [SerializeReference] public SpriteRenderer[] Targets;

        // TODO: put back the check.
        /* #currentSprite
        private Sprite _currentSprite = null;
        */

        // Update is called once per frame
        void Update()
        {
            /* #currentSprite
            if (object.ReferenceEquals(this._currentSprite, this.Source.sprite))
            {
                return;
            }

            this._currentSprite = this.Source.sprite;
            */

            foreach (var target in this.Targets)
            {
                target.sprite = this.Source.sprite;

                target.flipX = this.Source.flipX;
                target.flipY = this.Source.flipY;
            }
        }
    }
}
