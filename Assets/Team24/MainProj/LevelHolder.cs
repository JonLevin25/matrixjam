using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatrixJam;
using System;
using MatrixJam.TeamMeta;

namespace MatrixJam.Team24
{
    public class LevelHolder : MonoBehaviour
    {
        Entrance[] entries;
        Exit[] exits;
        int num_lvel;
        int ent_num;
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

        private void Start()
        {
            if (SceneManager.SceneMang != null)
            {
                if (SceneManager.SceneMang.Numentrence >= 0)
                {
                    EnterLevel(PlayerData.Data.current_level, SceneManager.SceneMang.Numentrence);
                    return;
                }

            }

            if (PlayerData.Data != null)
            {
                EnterDefault(PlayerData.Data.current_level);
            }
            else
            {
                EnterDefault(GameJamSetup._teamNumber);
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
        public void ExitLevel(Exit exit_to)
        {
            if (SceneManager.SceneMang != null)
            {
                PlayerData.Data.AddLevel(num_lvel, ent_num, exit_to.Num);
                SceneManager.SceneMang.LoadSceneFromExit(num_lvel, exit_to.Num);
            }
            else
            {
                Debug.Log("Game over at exit: " + exit_to.Num);

            }
        }

        public void EnterLevel(int lvl_num, int num_ent)
        {

            //run this to start the level
            num_lvel = lvl_num;
            ent_num = num_ent;
            OrganizeEnters();
            OrganizeExits();
            entries[num_ent].Enter();

        }

        public void EnterDefault(int lvl)
        {
            EnterLevel(lvl, def_ent);
        }
        void OrganizeEnters()
        {
            Entrance[] all_enter = GameObject.FindObjectsOfType<Entrance>();
            entries = new Entrance[all_enter.Length];
            foreach (Entrance runner in all_enter)
            {
                entries[runner.Num] = runner;
            }
        }
        void OrganizeExits()
        {
            Exit[] all_exit = GameObject.FindObjectsOfType<Exit>();
            exits = new Exit[all_exit.Length];
            foreach (Exit runner in all_exit)
            {
                exits[runner.Num] = runner;
            }
        }
        public void Restart()
        {
            if (SceneManager.SceneMang != null)
            {
                SceneManager.SceneMang.LoadScene(num_lvel, ent_num);
            }
        }
    }
}
