using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team21 {
    public class GameManager : MonoBehaviour {
		public Animator introText;
		public GameObject restartText;
		public float mouseIndicatorWait = 30f;
		public float mouseIndicatorTimeToShow = 5f;
		public GameObject mouseIndicator;
		public GameObject obstacles;
		public GameObject intro;
		public GameObject scrumbleIntro;
		public GameObject ending1;
		public GameObject ending2;
		public GameObject manInBoat;

		private IEnumerator mouseIndicatorWaitCoroutine;
		private int pastPlayed = 0;

		void Start() {
			pastPlayed = LevelHolder.Level.PastPlayed();
			HanlePastPlayed();
			mouseIndicatorWaitCoroutine = MouseIndicatorWaitRoutine();
			StartCoroutine(mouseIndicatorWaitCoroutine);

		}

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
				restartText.SetActive(false);
				GameState.state = State.PLAY;
				if (scrumbleIntro.activeSelf) {
					scrumbleIntro.GetComponent<Animator>().SetBool("IsPlay", true);
				}
				introText.SetBool("IsPlay", true);
				mouseIndicator.SetActive(false);
				StopCoroutine(mouseIndicatorWaitCoroutine);
			}
			if (Input.GetKeyDown(KeyCode.R)) {
				GameState.state = State.PAUSE;
				LevelHolder.Level.Restart();
			}
        }

		void HanlePastPlayed() {
			if (pastPlayed == 1) {
				intro.GetComponent<TMPro.TextMeshProUGUI>().text = "You already saved the world...Welcome Back anyway!";
				obstacles.SetActive(false);
			} else if (pastPlayed == 2) {
				intro.SetActive(false);
				obstacles.transform.Rotate(new Vector3(0f, 0f, -5f));
			} else if (pastPlayed == 3) {
				intro.SetActive(false);
				scrumbleIntro.SetActive(true);
				ending1.transform.Translate(0f, -2.82f, 0f);
				obstacles.GetComponent<Animator>().SetBool("IsPlay", true);
			} else if (pastPlayed == 4) {
				obstacles.SetActive(false);
				ending1.transform.Translate(0f, -2.82f, 0f);
				ending2.SetActive(false);
				manInBoat.transform.localScale = new Vector3(2f, 2f, 2f);
			}
		}

		IEnumerator MouseIndicatorWaitRoutine() {
			yield return new WaitForSeconds(mouseIndicatorWait);
			mouseIndicator.SetActive(true);
			// StartCoroutine(HideMouseIndicatorRoutine());
			yield return null;
		}

		IEnumerator HideMouseIndicatorRoutine() {
			yield return new WaitForSeconds(mouseIndicatorTimeToShow);
			mouseIndicator.SetActive(false);
			yield return null;
		}
    }
}
