using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
	private GameObject itemBeingDragged;
	private Vector3 startPosition;

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		Debug.Log ("Drag start");

		itemBeingDragged = gameObject.transform.parent.parent.gameObject;
		startPosition = itemBeingDragged.transform.position;

		// Hämta ut blockbuilder
		var blockBuilder = gameObject.GetComponentInParent<BlockBuilder> ();
		blockBuilder.SetNormalSized ();
	}

	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		Debug.Log ("Dragging");
		itemBeingDragged.transform.position = Input.mousePosition;
	}
	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		Debug.Log ("Drag end");
		itemBeingDragged.transform.position = startPosition;
		itemBeingDragged = null;

		var blockBuilder = gameObject.GetComponentInParent<BlockBuilder> ();
		blockBuilder.SetToyboxSize ();
	}

	#endregion
}
