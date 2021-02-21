using System;
using EnemyCore;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
	private void OnSceneGUI()
	{
		var fov = target as FieldOfView;
		var fovPosition = fov.transform.position;
		
		Handles.color = Color.white;
		Handles.DrawWireArc(fovPosition, Vector3.up, Vector3.forward, 360, fov.ViewRadius);

		Vector3 angleA = fov.DirFromAngle(-fov.FOVAngle / 2, false);
		Vector3 angleB = fov.DirFromAngle(fov.FOVAngle / 2, false);
		
		Handles.DrawLine(fovPosition, fovPosition + angleA * fov.ViewRadius);
		Handles.DrawLine(fovPosition, fovPosition + angleB * fov.ViewRadius);

		Handles.color = Color.red;
		foreach (var visibleTarget in fov.VisibleTargets)
		{
			Handles.DrawLine(fov.transform.position, visibleTarget.position);
		}
		
	}
}
