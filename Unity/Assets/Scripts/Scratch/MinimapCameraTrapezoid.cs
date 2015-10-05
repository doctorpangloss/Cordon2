﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class MinimapCameraTrapezoid : UnityEngine.UI.Image
{
	public Camera gameCamera;
	public Camera minimapCamera;
	public RectTransform parentRectTransform;
	public LayerMask groundLayer;

	Vector3[] points = new Vector3[] {
		new Vector3 (0f, 0f, 0f),
		new Vector3 (0f, 1f, 0f),
		new Vector3 (1f, 1f, 0f),
		new Vector3 (1f, 0f, 0f)
	};

	int[][] correspondences = new int[][] {
		new int[] {0,1,2,3,4,7,12,13,16}, 
		new int[] {5,6,8,9,10,11,17,20,21}, 
		new int[] {18,22,23,29,30,32,33,34,35}, 
		new int[] {14,15,19,24,25,26,27,28,31}
	};

	void Update ()
	{
		base.UpdateGeometry ();
	}

	protected override void OnPopulateMesh (Mesh toFill)
	{
		base.OnPopulateMesh (toFill);
		Debug.Log("Populating mesh?");

		var rect = parentRectTransform.rect;
		var vertices = toFill.vertices;
		for (var i = 0; i < points.Length; i++) {
			var point = points [i];
			var ray = gameCamera.ViewportPointToRay (point);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, gameCamera.farClipPlane, groundLayer)) {
				var transformedPosition = minimapCamera.WorldToViewportPoint (hit.point);
				if (vertices.Length == 36) {
					foreach (var j in correspondences[i]) {
						var vertex = vertices [j];
						vertex.x = vertex.x * transformedPosition.x;
						vertex.y = vertex.y * transformedPosition.y;
						vertices [j] = vertex;
					}
				} else {
					var vboPoint = vertices [i];
					vboPoint = new Vector3 (transformedPosition.x * rect.width, transformedPosition.y * rect.height, 0f);
					vertices [i] = vboPoint;
				}
			}
		}

		toFill.SetVertices (new List<Vector3>(vertices));
	}
}
