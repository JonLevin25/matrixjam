using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11
{
    public class OtherPillEntrance : AbstractSceneEntrance
    {
        [SerializeReference] protected SceneEntrance _bluePill;
        [SerializeReference] protected SceneEntrance _redPill;

        public SceneEntrance GetOtherPill()
        {
            if (ReferenceEquals(this._manager.CurrentScenePrefab, this._bluePill.TargetScene))
            {
                return this._redPill;
            }
            else if (ReferenceEquals(this._manager.CurrentScenePrefab, this._redPill.TargetScene))
            {
                return this._bluePill;
            }
            else
            {
                throw new System.Exception($"{this.name}: GetOtherPill should not be used within non-pill-scenes!");
            }
        }

        public override void Enter()
        {
            this.GetOtherPill().Enter();
        }
    }
}
