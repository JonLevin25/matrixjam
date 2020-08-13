using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team2
{
    public class MaterialPanelController : MonoBehaviour
    {
        [SerializeField] private FloopGunController gun;
        [SerializeField] private Image[] materialIcons;
        [SerializeField] private float activeAlpha;
        [SerializeField] private float inactiveAlpha;


        private FloopableMaterialTypes currentMaterial;

        // Start is called before the first frame update
        void Start()
        {
            currentMaterial = gun.currentMaterial;

            for (int icon = 0; icon < materialIcons.Length; icon++)
            {
                var tempColor = materialIcons[icon].color;
                tempColor.a =  (FloopableMaterialTypes)icon == currentMaterial ? activeAlpha : inactiveAlpha;
                materialIcons[icon].color = tempColor;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (currentMaterial != gun.currentMaterial)
            {
                currentMaterial = gun.currentMaterial;

                for (int icon = 0; icon < materialIcons.Length; icon++)
                {
                    var tempColor = materialIcons[icon].color;
                    tempColor.a = (FloopableMaterialTypes)icon == currentMaterial ? activeAlpha : inactiveAlpha;
                    materialIcons[icon].color = tempColor;
                }
            }
        }
    }
}
