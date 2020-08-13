using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MatrixJam.Team13
{
	[CustomEditor(typeof(Maze))]
    public class MazeEditor : Editor {
		Maze maze;

		private float _distance = 2.5f;
		private float _rectHalfSize = 1.25f;
		private int _newHeight;
		private int _newWidth;

        void OnSceneGUI(){
			maze = (Maze)target;

			if(maze.horizontalEdges == null){
				maze.horizontalEdges = new MultiGameObjectArray(maze.width, maze.height + 1);
			}
			if(maze.verticalEdges == null){
				maze.verticalEdges = new MultiGameObjectArray(maze.width + 1, maze.height);
			}
			if(maze.edgeIntersections == null){
				maze.edgeIntersections = new MultiGameObjectArray(maze.width + 1, maze.height + 1);
			}

			Handles.BeginGUI();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical(GUILayout.Width(100));
			EditorGUILayout.LabelField("Width", GUILayout.Width(100));
			_newWidth = EditorGUILayout.IntField(_newWidth, GUILayout.Width(100));
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(GUILayout.Width(100));
			EditorGUILayout.LabelField("Height", GUILayout.Width(100));
			_newHeight = EditorGUILayout.IntField(_newHeight, GUILayout.Width(100));
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			if(_newHeight != maze.height || _newWidth != maze.width){
				if(GUILayout.Button("Apply", GUILayout.Width(100))){
					Clear();
					maze.width = _newWidth;
					maze.height = _newHeight;
					maze.horizontalEdges = new MultiGameObjectArray(maze.width, maze.height + 1);
					maze.verticalEdges = new MultiGameObjectArray(maze.width + 1, maze.height);
					maze.edgeIntersections = new MultiGameObjectArray(maze.width + 1, maze.height + 1);
				}
				if(GUILayout.Button("Reset", GUILayout.Width(100))){
					Debug.Log("Canceling resize");
					_newWidth = maze.width;
					_newHeight = maze.height;
				}
			}
			
			EditorGUILayout.EndHorizontal();
			
			/*#region new height check
			if(newHeight > maze.height){
				MultiGameObjectArray newArr = new MultiGameObjectArray(maze.width + 1, newHeight);
				//MultiGameObjectArray pointsArr = new MultiGameObjectArray(maze.width + 1, newHeight + 1);
				//TODO resize edge points array
				for(int i = 0; i <= maze.width; i++){
					for(int j = 0; j < maze.height; j++){
						newArr[i, j] = maze.verticalEdges[i, j];
					}
				}
				maze.verticalEdges = newArr;
				maze.height = newHeight;
			}else if(newHeight < maze.height){
				if(EditorUtility.DisplayDialog("Delete extra?", "Changing the dimensions to smaller values will delete some of the design. Continue?", "Yes", "No")){
					MultiGameObjectArray newArr = new MultiGameObjectArray(maze.width + 1, newHeight);
					for(int i = 0; i <= maze.width; i++){
						for(int j = 0; j < maze.height; j++){
							if(j < newHeight){
								newArr[i, j] = maze.verticalEdges[i, j];
							}else{
								DestroyImmediate(maze.verticalEdges[i, j]);
							}
						}
					}
					maze.verticalEdges = newArr;
					maze.height = newHeight;
				}
			}
			#endregion

			#region new width check
			if(newWidth > maze.width){
				MultiGameObjectArray newArr = new MultiGameObjectArray(newWidth, maze.height + 1);
				for(int i = 0; i < maze.width; i++){
					for(int j = 0; j <= maze.height; j++){
						newArr[i, j] = maze.horizontalEdges[i, j];
					}
				}
				maze.horizontalEdges = newArr;
				maze.width = newWidth;
			}else if(newWidth < maze.width){
				if(EditorUtility.DisplayDialog("Delete extra?", "Changing the dimensions to smaller values will delete some of the design. Continue?", "Yes", "No")){
					MultiGameObjectArray newArr = new MultiGameObjectArray(newWidth, maze.height + 1);
					for(int i = 0; i < maze.width; i++){
						for(int j = 0; j <= maze.height; j++){
							if(i < newWidth){
								newArr[i, j] = maze.horizontalEdges[i, j];
							}else{
								DestroyImmediate(maze.horizontalEdges[i, j]);
							}
						}
					}
					maze.horizontalEdges = newArr;
					maze.width = newWidth;
				}
			}
			#endregion*/

			GUILayout.Space(25);
			if(GUILayout.Button("Clear All", GUILayout.Width(100))){
				Debug.Log("Clear");
				Clear();
			}
			Handles.EndGUI();

			DrawHorizontalEdges();
			DrawVerticalEdges();
		}

		private void DrawVerticalEdges(){
			Vector3 currentPos = maze.transform.position;
			currentPos.z += _rectHalfSize;
			for(int i = 0; i <= maze.width; i++){
				for(int j = 0; j < maze.height; j++){
					Handles.color = Color.red;
					if(maze.verticalEdges[i, j] == null){
						Handles.color = Color.green;
					}
					if(Handles.Button(currentPos, Quaternion.Euler(0, 90, 0), _rectHalfSize, _rectHalfSize, Handles.RectangleHandleCap)){
						if(maze.verticalEdges[i, j] == null){
							/*GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(maze.verticalWall, maze.transform);
							go.transform.position = currentPos;
							maze.verticalEdges[i, j] = go;*/
							AddEdge(false, j, i, currentPos);
						}else{
							DestroyImmediate(maze.verticalEdges[i, j]);
							maze.verticalEdges[i, j] = null;
						}
					}
					currentPos.z += _distance;
				}
				currentPos.z = maze.transform.position.z + _rectHalfSize;
				currentPos.x += _distance;
			}
		}

		private void DrawHorizontalEdges(){
			Vector3 currentPos = maze.transform.position;
			currentPos.x += _rectHalfSize;
			for(int i = 0; i <= maze.height; i++){
				for(int j = 0; j < maze.width; j++){
					Handles.color = Color.red;
					if(maze.horizontalEdges[j, i] == null){
						Handles.color = Color.green;
					}
					if(Handles.Button(currentPos, Quaternion.Euler(0, 0, 0), _rectHalfSize, _rectHalfSize, Handles.RectangleHandleCap)){
						if(maze.horizontalEdges[j, i] == null){
							/*GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(maze.horizontalWall, maze.transform);
							go.transform.position = currentPos;
							maze.horizontalEdges[j, i] = go;*/
							AddEdge(true, i, j, currentPos);
						}else{
							DestroyImmediate(maze.horizontalEdges[j, i]);
							maze.horizontalEdges[j, i] = null;
						}
					}
					currentPos.x += _distance;
				}
				currentPos.x = maze.transform.position.x + _rectHalfSize;
				currentPos.z += _distance;
			}
		}

		private void Clear(){
			for(int i = 0; i <= maze.height; i++){
				for(int j = 0; j < maze.width; j++){
					try{
						if(maze.horizontalEdges[j, i] != null){
							DestroyImmediate(maze.horizontalEdges[j, i]);
							maze.horizontalEdges[j, i] = null;
						}
					}catch(System.IndexOutOfRangeException){
						Debug.Log("Trying to access an out of range index when clearing horizontal edges");
					}
				}
			}

			for(int i = 0; i <= maze.width; i++){
				for(int j = 0; j < maze.height; j++){
					try{
						if(maze.verticalEdges[i, j] != null){
							DestroyImmediate(maze.verticalEdges[i, j]);
							maze.verticalEdges[i, j] = null;
						}
					}catch(System.IndexOutOfRangeException){
						Debug.Log("Trying to access an out of range index when clearing vertical edges");
					}
				}
			}

			for(int i = 0; i <= maze.width; i++){
				for(int j = 0; j <= maze.height; j++){
					try{
						if(maze.edgeIntersections[i, j] != null){
							DestroyImmediate(maze.edgeIntersections[i, j]);
							maze.edgeIntersections[i, j] = null;
						}
					}catch(System.IndexOutOfRangeException){
						Debug.Log("Trying to access an out of range index when clearing edge intersections");
					}	
				}
			}
		}

		private void AddEdge(bool horizontal, int row, int col, Vector3 pos){
			GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(horizontal ? maze.horizontalWall : maze.verticalWall, maze.transform);
			go.transform.position = pos;
			if(horizontal){
				maze.horizontalEdges[col, row] = go;

				if(row < maze.height){ //If lower than top edge
					Debug.Log("Below top edge");
					if(maze.verticalEdges[col, row] != null){	//Check the line above, left side
						Debug.Log("Wall on top-left");
						AddEdgePoint(row, col);
					}
					if(maze.verticalEdges[col + 1, row] != null){ //Check the line above, right side
						Debug.Log("Wall on top-right");
						AddEdgePoint(row, col + 1);
					}
				}
				if(row > 0){ //If higher than bottom edge
					Debug.Log("Above bottom edge");
					if(maze.verticalEdges[col, row - 1] != null){ //Check the line below, left side
						Debug.Log("Wall on bottom-left");
						AddEdgePoint(row, col);
					}
					if(maze.verticalEdges[col + 1, row - 1] != null){ //Check the line below, right side
						Debug.Log("Wall on bottom-right");
						AddEdgePoint(row, col + 1);
					}
				}

			}else{
				maze.verticalEdges[col, row] = go;

				if(col < maze.width){ //If before right edge 
					Debug.Log("Before right edge");
					if(maze.horizontalEdges[col, row] != null){ //Check the line below, to the right
						Debug.Log("Wall on bottom-right");
						AddEdgePoint(row, col);	
					}
					if(maze.horizontalEdges[col, row + 1]){ //Check the line above, to the right
						Debug.Log("Wall on top-right");
						AddEdgePoint(row + 1, col);
					}
				}
				if(col > 0){ // If after left edge
					Debug.Log("After left edge");
					if(maze.horizontalEdges[col - 1, row]){ //Check the line below, to the left
						Debug.Log("Wall on bottom-left");
						AddEdgePoint(row, col);
					}
					if(maze.horizontalEdges[col - 1, row + 1]){ //Check the line above, to the left
						Debug.Log("Wall on top-left");
						AddEdgePoint(row + 1, col);
					}
				}
			}
		}

		private void AddEdgePoint(int row, int col){
			if(maze.edgeIntersections[col, row] == null){
				Debug.Log("Adding edge point");
				GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(maze.edgePoint, maze.transform);
				Vector3 pos = maze.transform.position;
				pos.x += col * _distance;
				pos.z += row * _distance;
				go.transform.position = pos;
				maze.edgeIntersections[col, row] = go;
			}
		}
    }
}
