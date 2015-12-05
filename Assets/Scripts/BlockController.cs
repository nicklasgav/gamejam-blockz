using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
	private GameObject itemBeingDragged;
	private Vector3 startPosition;
	private Transform startParent;


	public GameObject ConnectedBlock;
	private Vector3 startPositionConnectedBlock;
	private Transform connectedBlockStartParent;

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		Debug.Log ("Drag start");

		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;

		if (ConnectedBlock != null) {
			startPositionConnectedBlock = ConnectedBlock.transform.position;
			connectedBlockStartParent = ConnectedBlock.transform.parent;
		}
		//GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}

	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		Debug.Log ("Dragging");
		transform.position = Input.mousePosition;

		if (ConnectedBlock != null)
			ConnectedBlock.transform.position = Input.mousePosition - (startPosition - startPositionConnectedBlock);
	}
	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		Debug.Log ("Drag end");
		itemBeingDragged = null;

		//if(transform.parent == startParent)
		transform.position = startPosition;

		if(ConnectedBlock != null)
			ConnectedBlock.transform.position = startPositionConnectedBlock;
		//GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}

	#endregion
}
