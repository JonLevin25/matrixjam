using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    [CreateAssetMenu(fileName = "Recipe")]
    public class Recipe : ScriptableObject
    {
        public IngridientType[] IngridientOrder;
        public GameObject ProductPrefab;
        public Texture RecipeIcon;
        public Vector3 Axis;
        public Vector3 Steps;
    }
}
