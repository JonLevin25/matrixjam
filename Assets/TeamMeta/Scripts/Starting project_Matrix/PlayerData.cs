using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam
{
    public class PlayerData : MonoBehaviour
    {
        public int current_level;
        int complete_levels = 0;
        int skip_levels_num = 0;
        LinkedList<Connection> been_connections = new LinkedList<Connection>();
        LinkedList<int> skip_level = new LinkedList<int>();
        static PlayerData data;
        public static PlayerData Data
        {
            get
            {
                if (data == null)
                {
                    data = GameObject.FindObjectOfType<PlayerData>();
                }
                return data;
            }
        }
        public int NumGames
        {
            get
            {
                return SceneManager.SceneMang.play_scenes.Length;
            }
        }
        public bool AddLevel(int finish_level, int ent, int exit)
        {
            //return true if total matrix game is over
            //add the level and the parh to memory
            if (!HaveLevel(finish_level))
            {
                complete_levels++;
                if (complete_levels + skip_levels_num >= SceneManager.SceneMang.play_scenes.Length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            been_connections.AddLast(new Connection(finish_level, ent, finish_level, exit));
            return false;
        }
        public bool HaveLevel(int have_this)
        {
            //return true if the given level was completed before
            foreach (Connection con in been_connections)
            {
                if (con.scene_from == have_this)
                {
                    return true;
                }
            }
            return false;
        }
        public int NumLevels
        {
            get
            {
                return complete_levels;
            }
        }
        public Connection LastCon
        {
            //return the last connection (i.e level) the player has finished. 
            //get LastCon.scene_from for last level finished, LastCon.portal_from for entrence used, and Lastcon.portal_to for exit used.
            get
            {
                return been_connections.Last.Value;
            }
        }
        public int PastBeen(int lvl)
        {
            //return the number of times this level have been completed before
            int ans = 0;
            foreach(Connection con in been_connections)
            {
                if(con.scene_from == lvl)
                {
                    ans++;
                }
               
            }
            return ans;
        }
    }
}
