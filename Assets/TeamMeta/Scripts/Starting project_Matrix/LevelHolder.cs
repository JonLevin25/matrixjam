using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatrixJam;
using System;
using MatrixJam.TeamMeta;
    
namespace MatrixJam
{
    public class LevelHolder : MonoBehaviour
    {
        Entrance[] entries;
        Exit[] exits;
        int num_lvel;
        static bool game_start = false;
        static int ent_num;
        public int def_ent;
        static LevelHolder level;
        public static LevelHolder Level
        {
            get
            {
                if (level == null)
                {

                    level = MonoBehaviour.FindObjectOfType<LevelHolder>();
                }
                return level;

            }
        }

        public int Current_Level
        {
            get
            {
                return num_lvel;
            }
        }
        public int Entrnce_Used
        {
            get
            {
                return ent_num;
            }
        }

        private void Start()
        {
            if (game_start)
            {
                entries[ent_num].Enter();
            }
            else
            {
                StartGame();
            }
        }
        void StartGame()
        {
            //set up the game for the first time, then start the game from the entry choosen by scenemanger, or by the default entry.
            Organize();
            if (SceneManager.SceneMang != null)
            {
                if (SceneManager.SceneMang.Numentrence >= 0 && SceneManager.SceneMang.Numentrence < entries.Length)
                {
                    EnterLevel(PlayerData.Data.current_level, SceneManager.SceneMang.Numentrence);
                    game_start = true;
                    return;
                }
            }

            if (PlayerData.Data != null)
            {
                EnterDefault(PlayerData.Data.current_level);
            }
            else
            {
                EnterDefault(GameJamData.TeamNumber);
            }
            game_start = true;
        }


        public void ExitLevel(Exit exit_to)
        {
            //exit the game using the given exit.
            //do not call this directly, it should be called from the choosen exit.
            if (SceneManager.SceneMang != null)
            {
                if (PlayerData.Data.AddLevel(num_lvel, ent_num, exit_to.Num))
                {
                    SceneManager.SceneMang.MatrixOver();
                }
                else
                {
                    SceneManager.SceneMang.LoadSceneFromExit(num_lvel, exit_to.Num);
                }
            }
            else
            {
                Debug.Log("Game over at exit: " + exit_to.Num);

            }
            game_start = false;
        }
        void Organize()
        {
            //orgnazie the entries, exit and other stuff that may be added, into their correct place.
            OrganizeEnters();
            OrganizeExits();
        }

        public void EnterLevel(int lvl_num, int num_ent)
        {
            //set up the game, then start it from the choose entry.
            //This function do load the scene - if you want to restart the game, use Restart().
            num_lvel = lvl_num;
            ent_num = num_ent;
            entries[num_ent].Enter();
        }

       void EnterDefault(int lvl)
        {
            //enter the game from the defult entry.
            //if the defult is not vaild, choose a random entry.
            if (def_ent < 0 || def_ent > entries.Length - 1)
            {
                EnterRandom(lvl);
            }
            else
            {
                EnterLevel(lvl, def_ent);
            }
        }
        void EnterRandom(int lvl)
        {
            //enter the game from a random entry.
            System.Random rand = new System.Random();
            EnterLevel(lvl, rand.Next(0, entries.Length));
        }
        void OrganizeEnters()
        {
            //organize the entries of this game into the array.
            Entrance[] all_enter = GameObject.FindObjectsOfType<Entrance>();
            entries = new Entrance[all_enter.Length];
            foreach (Entrance runner in all_enter)
            {
                entries[runner.Num] = runner;
            }
        }
        void OrganizeExits()
        {
            //organize the exits of this game into the array.
            Exit[] all_exit = GameObject.FindObjectsOfType<Exit>();
            exits = new Exit[all_exit.Length];
            foreach (Exit runner in all_exit)
            {
                exits[runner.Num] = runner;
            }
        }
       public void Restart()
        {
            //will reset the game and the scene, then enter from the same entry.
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                
        }

        public int PastPlayed()
        {
            //will return how much time the player has already completed the current level.
            if (PlayerData.Data == null)
            {
                return 0;
            }
            return PlayerData.Data.PastBeen(num_lvel);
        }
    }
}
