using System.Collections.Generic;
using MatrixJam.Team;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class UIManager : MonoBehaviour
    {
        
        private static UIManager _instance;
        public NumberPool PlayerNumberPool;
        public NumberPool AiNumberPool;
        public GameObject DarkScreen;
        public GameObject Tooltips;

        public GameObject AttackButtons;
        public NumberButtonScript NumberPrefab;
        public MessageScript LoseMessage;
        public MessageScript WinMessage;
        
        public static IChoiceManager ChoiceManager { get; set; }

        private void Awake()
        {
            _instance = this;
            DarkScreen.SetActive(false);
            Tooltips.SetActive(true);
            NumberButtonsManager.GenerateBoard(NumberPrefab);
            ShowDamageOptions(false);
            
        }

        public static void ShowDarkScreen()
        {
            _instance.DarkScreen.SetActive(true);
        }
        
        public static void HideDarkScreen()
        {
            _instance.DarkScreen.SetActive(false);
        }
        
        public static void NumberChosen(int i)
        {
            ChoiceManager.PickNumber(i);
        }
        
        

        public static void SquareChosen(Vector2 index)
        {
            
            NumberButtonsManager.MakeAllUnselectable();
            _instance.PlayerNumberPool.MakeNoneSelectable();
            ChoiceManager.PickSquare(index);
        }

        public static void ShowSelectablePositions(Vector2[] squares)
        {
            NumberButtonsManager.SetSquaresForPuttingNumberOn(squares);
        }

        public static void ShowDamageOptions(bool interactable)
        {
            _instance.AttackButtons.SetActive(interactable);
        }

        public static void SetNumberOnSquare(Vector2 index, int currentValue, PlayerSide playerColor)
        {
            NumberButtonsManager.SetNumberOnSquare(index, currentValue, playerColor);
        }

        public static void SetPlayerAvailableNumbers(List<int> choices, PlayerSide playerSide, bool selectable)
        {
            if (playerSide == PlayerSide.Human)
            {
                _instance.PlayerNumberPool.SetAvailableNumbers(choices, selectable);
            }
            else
            {
                _instance.AiNumberPool.SetAvailableNumbers(choices, false);
            }
        }

        public void PickAttackRow()
        {
            ChoiceManager.PickAttack(AttackDirection.row);
        }

        public void PickAttackLine()
        {
            ChoiceManager.PickAttack(AttackDirection.colum);
        }
        public void PickAttackBox()
        {
            ChoiceManager.PickAttack(AttackDirection.square);
        }
        
    }
}