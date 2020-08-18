using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team17
{
    public class Water : MonoBehaviour
    {
        List<Player> players = new List<Player>();

        void CheckGameOver()
        {
            if (players.Count > 0)
                Game.App.GameOver();
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name + " is now wet with " + gameObject.name);

            Player player = other.GetComponentInParent<Player>();
            if (player != null && !players.Contains(player))
                players.Add(player);

            CheckGameOver();
        }

        void OnTriggerExit(Collider other)
        {
            Debug.Log(other.name + " is no longer wet with " + gameObject.name);

            Player player = other.GetComponentInParent<Player>();
            if (player != null && players.Contains(player))
                players.Remove(player);
        }
    }
}
