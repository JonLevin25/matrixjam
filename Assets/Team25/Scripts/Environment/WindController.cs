using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatrixJam.Team25.Scripts.Environment
{
    public class WindController : MonoBehaviour
    {
        public float randomizerInterval;
        private WindZone windZone;
        private Coroutine windRandomizer;

        private void Awake()
        {
            windZone = GetComponent<WindZone>();
            windRandomizer = StartCoroutine(RandomizeWind());
        }

        private IEnumerator RandomizeWind()
        {
            while (true)
            {
                float yRot = Random.Range(0f, 1f) > 0.5f ? 90 : -90;
                Vector3 rot = new Vector3(
                    windZone.transform.rotation.eulerAngles.x,
                    yRot,
                    windZone.transform.rotation.eulerAngles.z
                );
                windZone.transform.rotation = Quaternion.Euler(rot);
                yield return new WaitForSeconds(randomizerInterval);
            }
        }
    }
}