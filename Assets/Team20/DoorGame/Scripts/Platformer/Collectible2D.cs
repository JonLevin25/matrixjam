using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
    {
        namespace MatrixJam.Team20
{
    public class Collectible2D : MonoBehaviour
        {
            private enum CollectibleType
            {
                Coin
            }
            [SerializeField] private CollectibleType _collectibleType;


            private void OnTriggerEnter2D(Collider2D other)
            {
                if (other.CompareTag("Tag0"))
                {
                    PlayerComponent player = other.GetComponent<PlayerComponent>();
                    if (_collectibleType == CollectibleType.Coin && player != null)
                    {
                        player.AddCoins(1);
                        Destroy(gameObject);
                    }
                }
            }


            private void OnTriggerStay2D(Collider2D other)
            {

            }
        }
    }
}
