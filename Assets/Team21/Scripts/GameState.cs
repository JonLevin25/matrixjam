using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team21 {
	public enum State {
		PLAY,
		PAUSE
	}
    public class GameState : MonoBehaviour {
        public static State state = State.PAUSE;
    }
}
