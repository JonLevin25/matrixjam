using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// TODO:
// - Start the game itself in the ButtonHandler_PlayGame() function
// - Call MainMenu.NotifyGameIsWon() when game is won and before switching to main menu


namespace MatrixJam.TeamMeta
{
    public class MainMenu : MonoBehaviour
    {
        private static bool gameWasWon = false;
        
        // ---------------- Main menu
        
        [Header("Main Menu buttons")]
        [SerializeField]
        private List<Button> mainButtons;
        
        [Header("Submenus")]
        [SerializeField]
        private GameObject rules;
        [SerializeField]
        private GameObject credits;
        
        [Header("Submenu additional graphics")]
        [SerializeField]
        public GameObject youWinGraphics;
        
        public void Start()
        {
            // Main menu
            rules.SetActive(false);
            credits.SetActive(false);
            
            MaybeShowYouWon();
        }
        
        private void SetMainButtons(bool newInteractableStatus)
        {
            foreach (var button in mainButtons) {
                button.interactable = newInteractableStatus;
            }
        }
        
        
        
        
        
        // ---------------- After you win game
        
        static public void NotifyGameIsWon()
        {
            gameWasWon = true;
        }
        
        private void MaybeShowYouWon()
        {
            // If won game
            if (gameWasWon) {
                youWinGraphics.SetActive(true);
                ButtonHandler_ShowCredits();
                // Disable
                gameWasWon = false;
            } else {
                youWinGraphics.SetActive(false);
            }
        }
        
        
        
        
        
        // ---------------- Main menu button handlers
        
        public void ButtonHandler_PlayGame()
        {
            FindObjectOfType<SceneManager>().LoadRandomScene();
        }
        
        public void ButtonHandler_ShowRules()
        {
            rules.SetActive(true);
            SetMainButtons(false);
        }
        
        public void ButtonHandler_ShowCredits()
        {
            credits.SetActive(true);
            SetMainButtons(false);
        }
        
        public void ButtonHandler_ExitGame()
        {
            Application.Quit();
        }
        
        
        
        
        // ---------------- Submenu button handlers
        
        public void ButtonHandler_BackToMenu()
        {
            rules.SetActive(false);
            credits.SetActive(false);
            SetMainButtons(true);
        }
    }
}
