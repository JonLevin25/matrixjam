using System;
using UnityEngine;

namespace MatrixJam.Team14
{
    public class TrainKiller : MonoBehaviour
    {
        private TrainController _train;
        private Func<Transform, bool> _condition;
        private bool _destroyed;

        public void Init(TrainController train, Func<Transform, bool> condition)
        {
            _train = train;
            _condition = condition;
        }

        private void Update()
        {
            if (_destroyed) return; // just in case
            if (!_condition(transform)) return;
            
            Debug.Log("TrainKiller: Condition true! executing");
            _train.KillTrain();
            
            _destroyed = true;
            Destroy(this);
        }
    }
}