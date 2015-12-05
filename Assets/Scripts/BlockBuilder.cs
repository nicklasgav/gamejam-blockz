using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class BlockBuilder : MonoBehaviour 
{
	public GameObject Slot;
	public GameObject Block;   

	private Vector2 _blockSize;
	private Vector2 _blockSpacing;

	private int _gridSize;
	private ICollection<GameObject> _blocks;
	private ICollection<GameObject> _slots;

	void Start() 
	{
		SetBlock (new string[][] {
			new string[] {
				"FFF000",
				"FF0000",
				null
			},
			new string[] {
				"FF0000",
				"FF0000",
				"A71232"
			},
			new string[] {
				null,
				"FF0000",
				null
			}
		});
	}

	private BlockController _dragBlock;
	private Vector3 _dragStartPosition;
	private Vector3 _mouseStartDragPosition;

	public void DragStart(BlockController block)
	{
		SetNormalSized ();

		_dragBlock = block;
		_dragStartPosition = transform.position;
		_mouseStartDragPosition = Input.mousePosition;
		//_dragStartPosition = startPosition;

		foreach (var b in _blocks) {
			var slot = b.GetComponent<BlockController>().Slot;
			b.gameObject.transform.SetParent(slot.transform);
		}

		gameObject.SetActive(true);
	}

	public void Dragging(Vector3 mousePosition)
	{
		transform.position = mousePosition; // TODO
		Vector3 delta = _mouseStartDragPosition - Input.mousePosition;


		var gameGrid = GameObject.FindWithTag ("GameController");

		var gameSlots = gameGrid.GetComponentsInChildren<SlotController> ();

		foreach (var slot in gameSlots)
			slot.GetComponent<Image> ().color = new Color(Color.white.r, Color.white.g, Color.white.b, .25f);

		foreach (var block in _blocks) {
			block.GetComponent<BlockController>().SetPreviewColor(delta);
		}
	}

	public void DragEnd()
	{
		// Kolla om vi kan släppa slots
		Vector3 delta = _mouseStartDragPosition - Input.mousePosition;
		bool canBeDropped = true;

		foreach (var block in _blocks) 
		{
			if(!block.GetComponent<BlockController>().CanBeDropped(delta))
			{
				canBeDropped = false;
				break;
			}
		}

		// Återställ om vi inte kan släppa
		if (canBeDropped) {
			// Släpp ner block
			foreach(var block in _blocks)
				block.GetComponent<BlockController>().Drop(delta);

			gameObject.SetActive(false);
		} else {
			// TODO: Kolla om den är från brädet eller från toybox
			SetToyboxSize ();
			transform.position = _dragStartPosition;
		}

		_dragBlock = null;
	}

	private void SetBlock(string[][] blockDesign) 
	{
		// [[1,0,1,0,1], [1,2,3,4,5], [1,2,3,4,5], [1,2,3,4,5], [1,2,3,4,5]]

		if (blockDesign.Length == 0 || blockDesign [0].Length != blockDesign.Length)
			throw new InvalidOperationException ();

		_gridSize = blockDesign.Length;

		GridLayoutGroup layout = GetComponent<GridLayoutGroup> ();
		_blockSpacing = layout.spacing;
		_blockSize = layout.cellSize;

		// Bygg upp grid
		_blocks = new List<GameObject> ();
		_slots = new List<GameObject> ();

		for (int i = 0; i < _gridSize; i++) 
		{
			for(int j = 0; j < _gridSize; j++) 
			{
				// Skapa ett slot för blocket (Även om vi inte ska lägga in något block)
				GameObject slot = Instantiate(Slot);
				slot.SetActive(true);
				slot.transform.SetParent(gameObject.transform);
				_slots.Add(slot);

				var blockColor = blockDesign[j][i];
				if(blockColor != null) 
				{
					// Skapa blocket
					GameObject block = Instantiate(Block);
					BlockController blockController = block.GetComponent<BlockController>();

					block.SetActive(true);
					block.transform.SetParent(slot.transform);
					blockController.SetBuilder(this);
					blockController.SetSlot(slot);
					block.name = "Block " + i + ", " + j;

					// Uppdater färgen på blocket
					var c = HexToColor(blockColor);
					block.GetComponent<Image>().color = c;
				

					_blocks.Add(block);
				}
			}
		}

		SetToyboxSize ();
	}

	public void SetNormalSized()
	{
		Scale (1);
	}

	public void SetToyboxSize()
	{
		Scale (.24f);
	}

	public void Scale(float scaleRelativeToOriginal)
	{
		GridLayoutGroup layout = GetComponent<GridLayoutGroup> ();
		layout.cellSize = _blockSize * scaleRelativeToOriginal;
		layout.spacing = _blockSpacing * scaleRelativeToOriginal;

		Vector2 gridVector = (layout.cellSize + layout.spacing) * _gridSize;

		RectTransform rectTransform = layout.GetComponent<RectTransform> ();
		rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, gridVector.x);
		rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, gridVector.y);

		foreach (var slot in _slots) {
			GridLayoutGroup slotLayout = slot.GetComponent<GridLayoutGroup> ();
			slotLayout.cellSize = _blockSize * scaleRelativeToOriginal;
		}
	}

	private Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color(r/255f,g/255f,b/255f, 1f);
	}
}
