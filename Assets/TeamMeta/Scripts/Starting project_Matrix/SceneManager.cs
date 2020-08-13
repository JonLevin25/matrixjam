using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MatrixJam
{
    public class SceneManager : MonoBehaviour
    {

        public LevelConnects[] all_connects;
        public Object[] play_scenes;
        private int num_entrence;
        private static SceneManager scenemg;
        public static SceneManager SceneMang
        {
            get
            {
                if (scenemg == null)
                {
                    scenemg = GameObject.FindObjectOfType<SceneManager>();
                }
                return scenemg;
            }
        }
        public int Numentrence
        {
            get
            {
                return num_entrence;
            }
        }
        public void LoadScene(int num_sce, int num_port)
        {
            //load the scene from given level number and level entry.
            //this does start the gameplay in that scene.
            // if num_sce = -1 will load the start scene.
            // if num_sce = -2 will load the end scene.
            switch (num_sce)
            {
                case -1:
                    {
                        LoadSceneFromName("Start");
                        break;
                    }
                case -2:
                    {
                        LoadSceneFromName("End");
                        break;
                    }
                default:
                    {
                        if (num_sce > 0 && num_sce <= play_scenes.Length)
                        {
                            num_entrence = num_port;
                            PlayerData.Data.current_level = num_sce;
                            LoadSceneFromNumber(num_sce);
                        }
                        break;
                    }
            }

        }
        public void LoadSceneFromConnectionMem(Connection con)
        {
            //load the scene & entry at the start of given connection
            LoadScene(con.scene_from, con.portal_from);
        }
        public void LoadSceneFromExit(int num_sce, int int_exit)
        {
            //load scene & entry that connects to given exit.
            
            Connection ent_point = FindConnectTo(num_sce, int_exit);
            LoadScene(ent_point.scene_to, ent_point.portal_to);
        }
        void LoadSceneFromNumber(int num_scn)
        {
            //load the scene of given number. 
            //This do not start the gameplay in the scene!
            LoadSceneFromName(play_scenes[num_scn - 1].name);
        }

       void LoadSceneFromName(string name)
        {
            //the scene from memory.
            //This do not start the gameplay in the scene!
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        }
        public Connection FindConnectTo(int level, int exit)
        {
            //find the connection to the next scene of given exit
            return all_connects[level].FindConnect(exit, true);
        }
        public void LoadRandomScene()
        {
            Debug.Log("Starting random scene");
            Debug.Log($"PlayerData.Data.NumGames {PlayerData.Data.NumGames}");
            //choose a random scene and load it.
            //this also start the gameplay in that scene.
            int start_sce = 1 + Random.Range(0, PlayerData.Data.NumGames);
            LoadScene(start_sce, 0);
        }
        public void ResetLevelScene()
        {
            //restart the level from the start
            //this start the gameplay in the scene.
            if (LevelHolder.Level != null)
            {
                LevelHolder.Level.Restart();
            }
        }
        public void MatrixOver()
        {
            //end the matrix and start the end scene
            Debug.Log("MatrixOver!");
            LoadScene(-2, 0);
        }
    }
}

