using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

public class BlockController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
	private GameObject itemBeingDragged;
	private Vector3 startPosition;
	private BlockBuilder _builder;
	public GameObject Slot {get; private set;}

	public void SetBuilder(BlockBuilder builder)
	{
		_builder = builder;
	}

	public void SetSlot(GameObject slot)
	{
		Slot = slot;
	}

	public void SetPreviewColor(Vector3 delta)
	{
		var collidingSlot = GetCollidingSlot (delta);
		if (collidingSlot != null) {
			var color = gameObject.GetComponent<Image>().color;
			collidingSlot.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.4f);
		}
	}

	#region IBeginDragHandler implementation
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		_builder.DragStart(this);
	}

	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		_builder.Dragging (Input.mousePosition);
	}
	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		_builder.DragEnd ();
	}

	#endregion

	public bool CanBeDropped(Vector3 delta)
	{
		var slot = GetCollidingSlot (delta);
		if (slot != null) {
			var blocks = slot.gameObject.GetComponents<BlockController>();
			return blocks.Length == 0;
		}
		return false;
	}

	public void Drop(Vector3 delta)
	{
		var slot = GetCollidingSlot (delta);
		if (slot != null) {
			transform.SetParent(slot.gameObject.transform);
		}
	}

	private SlotController GetCollidingSlot(Vector3 delta)
	{
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
		GetComponent<BoxCollider2D> ().enabled = false;

		var currentPosition = gameObject.transform.position;
		RaycastHit2D hit = Physics2D.Raycast(currentPosition, Vector2.zero);

		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		GetComponent<BoxCollider2D> ().enabled = true;

		if (hit.collider != null) {
			return hit.collider.gameObject.GetComponent<SlotController> ();
		}
	
		return null;
	}
}
