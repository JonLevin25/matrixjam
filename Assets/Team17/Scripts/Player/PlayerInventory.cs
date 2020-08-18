using UnityEngine;

namespace MatrixJam.Team17
{
    [RequireComponent(typeof(Player))]
    public class PlayerInventory : MonoBehaviour
    {
        //public GrapplerConfig ropeGunConfig;
        public GunConfig[] gunConfigs;
        public Transform weaponAnchor;

        Player player;
        //Grappler ropeGun;
        IPlayerInventoryItem[] items;
        uint equippedItemIndex = 0;

        public IPlayerInventoryItem EquippedItem => (equippedItemIndex < items.Length) ? items[equippedItemIndex] : null;

        void Awake()
        {
            player = GetComponent<Player>();

            // Create utility items
            //ropeGun = Instantiate(ropeGunConfig.grapplerPrefab, weaponAnchor);
            //ropeGun.Owner = player;

            // Create weapons. They're disabled unless equipped.
            items = new IPlayerInventoryItem[gunConfigs.Length];
            for (int i = 0; i < items.Length; i++)
            {
                Gun gun = Instantiate(gunConfigs[i].gunPrefab, weaponAnchor);
                gun.Owner = player;
                gun.enabled = (i == equippedItemIndex);
                items[i] = gun;
            }
        }

        void Update()
        {
            // Equipped item input
            if (EquippedItem is ITrigger trigger)
            {
                if (player.Input.fire1)
                {
                    trigger.TriggerDown();
                }
                else
                {
                    trigger.TriggerUp();
                }
            }

            // TODO generic weapon calls for ChangeLength and RemoteTrigger
            /*
            // Ninja rope input
            if (ropeGun != null)
            {
                if (player.Input.useItem)
                {
                    ropeGun.TriggerDown();
                }
                else
                {
                    ropeGun.TriggerUp();
                }

                if (player.Input.cyclePrev)
                {
                    ropeGun.ChangeLength(1f);
                }
                else if (player.Input.cycleNext && !player.IsGrounded)
                {
                    ropeGun.ChangeLength(-1f);
                }}

                if (player.Input.jump)
                    {
                        ropeGun.Detach();
                
                }
            */
        }
    }
}
