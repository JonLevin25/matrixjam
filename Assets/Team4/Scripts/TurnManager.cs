using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class TurnManager
    {
        private List<Player> _playersList;
        int _turnIndex;

        public TurnManager(List<Player> players)
        {
            _turnIndex = 1;//To start on 0 when we choose "next"
            _playersList = players;
            foreach (var player in players)
            {
                player.MyUnits = new List<Unit>();
                for (int i = 1; i <= 9; i++)
                {
                    player.MyUnits.Add(new Unit(player, i));
                }

            }
        }

        public Player GetNextPlayer()
        {
            AdvanceIndex();
            return _playersList[_turnIndex];
        }

        //advances the index keeping record of the players turn, reset if all
        //players had played
        private void AdvanceIndex()
        {
            _turnIndex += 1;
            if ( _turnIndex == _playersList.Count)
            {
                _turnIndex = 0;
            }
        }

     
    }
}
