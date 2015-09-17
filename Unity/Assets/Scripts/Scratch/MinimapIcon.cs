using UnityEngine;
using System.Collections;

public class MinimapIcon : MonoBehaviour
{
	public UnityEngine.UI.Image image {
		get;
		private set;
	}

	RectTransform rectTransform;
	// Use this for initialization
	void Awake ()
	{
		rectTransform = GetComponent<RectTransform> ();
		image = GetComponent<UnityEngine.UI.Image> ();
	}

	public void UpdateWithData (RectTransform parentRect = null, Camera minimapCamera = null, Vector3 offset = default(Vector3), Transform unitTransform = null)
	{
		// Transform this position into minimap-space
		var viewportPoint = minimapCamera.WorldToViewportPoint (unitTransform.position + offset);
		// Now transform this viewport point to parent rect space
		var width = parentRect.rect.width;
		var height = parentRect.rect.height;
		var newPosition = new Vector2 (-width / 2 + width * viewportPoint.x, -height / 2 + height * viewportPoint.y);
		// Set this position;
		rectTransform.anchoredPosition = newPosition;
	}
}
