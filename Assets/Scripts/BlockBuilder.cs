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
				"FFF0000",
				"FF00000",
				null
			},
			new string[] {
				"FF00000",
				"FF00000",
				"FF00000"
			},
			new string[] {
				null,
				"FF00000",
				null
			}
		});
	}

	public void SetBlock(string[][] blockDesign) 
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

				var blockColor = blockDesign[i][j];
				if(blockColor != null) 
				{
					// Skapa blocket
					GameObject block = Instantiate(Block);
					block.SetActive(true);
					block.transform.SetParent(slot.transform);

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

	private string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}
	
	private Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color(r,g,b, 255);
	}
}
