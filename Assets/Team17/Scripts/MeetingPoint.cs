using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team17
{
    public class MeetingPoint : MonoBehaviour
    {
        List<Player> players = new List<Player>();

        void CheckWin()
        {
            if (players.Count == 2)
                Game.App.Win();
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log(gameObject.name + " meets with " + other.name);

            Player player = other.GetComponentInParent<Player>();
            if (player != null && !players.Contains(player))
                players.Add(player);

            CheckWin();
        }

        void OnTriggerExit(Collider other)
        {
            Debug.Log(gameObject.name + " leaves meeting with " + other.name);

            Player player = other.GetComponentInParent<Player>();
            if (player != null && players.Contains(player))
                players.Remove(player);
        }
    }
}
