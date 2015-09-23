using UnityEngine;
using System.Collections.Generic;

public class MinimapUnit : MonoBehaviour
{
	public static HashSet<MinimapUnit> minimappables = new HashSet<MinimapUnit> ();
	public Color color = Color.white;
	public Vector2 size = new Vector2 (12f, 12f);
	public int channel = 0;
	public MinimapIcon uiIconOverride = null;

	// Use this for initialization
	void Start ()
	{
		if (enabled) {
			AddToMinimap ();
		}
	}

	void OnDestroy ()
	{
		RemoveFromMinimap ();
	}

	void OnEnable ()
	{
		AddToMinimap ();
	}

	void OnDisable ()
	{
		RemoveFromMinimap ();
	}

	protected void RemoveFromMinimap ()
	{
		minimappables.Remove (this);
	}

	protected void AddToMinimap ()
	{
		minimappables.Add (this);
	}
}
