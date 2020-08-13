using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MatrixJam.Team13
{
	[ExecuteInEditMode]
    public class Maze : MonoBehaviour {
    	[HideInInspector] public int width;
		[HideInInspector] public int height;

		public GameObject verticalWall;
		public GameObject horizontalWall;
		public GameObject edgePoint;

		public MultiGameObjectArray horizontalEdges;
		public MultiGameObjectArray verticalEdges;

		public MultiGameObjectArray edgeIntersections;

		/*[SerializeField]private MultiGameObjectArray _ei;
		public MultiGameObjectArray edgeIntersections{
			get{ return _ei;}
			set{ _ei = value;}
		}*/
		/*
		private GameObject[,] _horizontalEdges;
		private GameObject[,] _verticalEdges;
		public GameObject[,] horizontalEdges{
			get{
				return _horizontalEdges;
			}
			set{
				_horizontalEdges = value;
			}
		}

		public GameObject[,] verticalEdges{
			get{
				return _verticalEdges;
			}
			set{
				_verticalEdges = value;
			}
		}*/

		/*public void AddHorizontalEdge(int x, int y, GameObject go){
			horizontalEdges[x, y] = go;
		}*/
    }
}
