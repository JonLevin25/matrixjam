using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team13
{
    public class DoorLock : MonoBehaviour {
        [SerializeField] private SpriteRenderer _spriteRenderer;

		public void SetSprite(Sprite sprite){
			_spriteRenderer.sprite = sprite;
		}

		public Sprite sprite{
			get{
				return _spriteRenderer.sprite;
			}
		}

    }
}
