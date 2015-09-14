using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapPanel : MonoBehaviour
{
	public int channel = 0;
	public Dictionary<MinimapUnit, MinimapIcon> icons = new Dictionary<MinimapUnit, MinimapIcon> ();
	public MinimapIcon defaultIcon = null;
	public Camera minimapCamera;
	RectTransform rectTransform;
	// Use this for initialization
	void Start ()
	{
		minimapCamera = minimapCamera ?? Camera.main;
		rectTransform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Remove icons that have been removed
		var toRemove = new List<KeyValuePair<MinimapUnit, MinimapIcon>> ();
		foreach (var kv in icons) {
			// If the unit has been destroyed, remove it and destroy its icon
			if (kv.Key == null
			    || !kv.Key.gameObject.activeSelf
			    || !kv.Key.isActiveAndEnabled
				|| !MinimapUnit.minimappables.Contains (kv.Key)) {
				toRemove.Add (kv);
			}
		}

		foreach (var kv in toRemove) {
			icons.Remove (kv.Key);
			Destroy (kv.Value.gameObject);
		}

		// Update and add icons that need to be updated/added
		foreach (var unit in MinimapUnit.minimappables) {
			MinimapIcon icon = null;
			var hasValue = icons.TryGetValue (unit, out icon);
			// Check if the value exists. Do a null check in case it exists but the pointer indicates the object is destroyed
			if (!hasValue
				|| (hasValue && icon == null)) {
				var iconToInstantiate = unit.uiIconOverride ?? defaultIcon;
				icon = Instantiate<MinimapIcon> (iconToInstantiate);
				icon.transform.SetParent (this.transform, false);
				icons [unit] = icon;
			}

			// Update the icons
			icon.UpdateWithData (parentRect: rectTransform, minimapCamera: minimapCamera, offset: Vector3.zero, unitTransform: unit.transform);
		}
	}
}
