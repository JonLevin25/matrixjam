using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team13
{
	[System.Serializable]
    public class MultiGameObjectArray {

		[SerializeField]
		private int width;

		[SerializeField]
		private int height;

		[SerializeField]
		private GameObject[] values;

		public int Width { get { return width; } }

		public int Height { get { return height; } }

		public GameObject this[int x, int y] {
			get { return values[y * width + x]; }
			set { values[y * width + x] = value; }
		}

		public MultiGameObjectArray(int width, int height){
			this.width = width;
			this.height = height;

			values = new GameObject[width * height];
		}
	}
}
