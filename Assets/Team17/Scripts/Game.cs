using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace MatrixJam.Team17
{
    [RequireComponent(typeof(PlayerInputListener))]
    public class Game : MonoBehaviour {

        [Space]
        public GameConfig config;
        [Space]
        public MatrixJam.Exit[] exits;

        static Game game;
        public static Game App => game;

        public static GameConfig Config => game.config;
        public static PlayerInputData Input => game.inputListener.data;

        PlayerInputListener inputListener;

        void Awake() // Set early script exceution order
        {
            if (game != null)
            {
                Debug.LogError("Multiple Game instances!");
                return;
            }
          
            game = this;
            inputListener = GetComponent<PlayerInputListener>();
        }

        public void OnStart()
        {
            //Debug.Log("Game start");
        }

        public void Win()
        {
            Exit(0);
        }

        public void GameOver()
        {
            Exit(1);
        }

        public void Exit(int index)
        {
            if (index < exits.Length && index >= 0)
                exits[index].EndLevel();
            else
                Debug.LogError("Invalid exit: " + index);
        }

    }
}