using UnityEngine;

namespace MatrixJam.Team4
{
    public abstract class AiBrain : MonoBehaviour
    {
        public abstract TurnObject MakeDecision(TurnData turnData, Player player);
    }
}