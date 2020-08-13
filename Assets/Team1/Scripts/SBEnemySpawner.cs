using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatrixJam.Team1
{
    public class SBEnemySpawner : MonoBehaviour
    {
        [SerializeField] private SBEnemyController enemyController;
        
        [SerializeField] private float nextSpawnTime = 0;
        [SerializeField] private float timer;
        [SerializeField] private AnimationCurve animCurve;
        [SerializeField] private float totalTimer;
        [SerializeField] private float boardSize = 125f;
        [SerializeField] private List<string> matrixNames;

        private List<string> matrixNamesOptions;

        private void Awake()
        {
            matrixNamesOptions = new List<string>(matrixNames);
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            totalTimer += Time.deltaTime;
            
            if (timer >= nextSpawnTime)
            {
                timer = 0;
                var tempEnemy = Instantiate(enemyController,new Vector3(Random.Range(-boardSize, boardSize), 0, Random.Range(-boardSize, boardSize)), Quaternion.identity);

                var nameIndex = Random.Range(0, matrixNamesOptions.Count);
                var nameString = matrixNamesOptions[nameIndex];
                tempEnemy.Init(nameString, matrixNames.IndexOf(nameString));
                matrixNamesOptions.Remove(nameString);

                if (matrixNamesOptions.Count == 0)
                {
                    matrixNamesOptions = new List<string>(matrixNames);
                }
                
                nextSpawnTime = animCurve.Evaluate(totalTimer);
            }
        }
    }
}
