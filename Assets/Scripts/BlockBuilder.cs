using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BlockBuilder : MonoBehaviour 
{
	public GameObject Slot;
	public GameObject Block;   

	void Start() 
	{
		SetBlock (new string[][] {
			new string[] {
				"FF00000",
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

		int gridSize = blockDesign.Length;
		
		// Bygg upp grid
		for (int i = 0; i < gridSize; i++) 
		{
			for(int j = 0; j < gridSize; j++) 
			{
				GameObject slot = Instantiate(Slot);
				slot.SetActive(true);
				slot.transform.SetParent(gameObject.transform);

				var blockColor = blockDesign[i][j];
				if(blockColor != null) 
				{
					GameObject block = Instantiate(Block);
					block.SetActive(true);
					block.transform.SetParent(slot.transform);

					var c = HexToColor(blockColor);
					block.GetComponent<Image>().color = c;
				}
			}
		}

		// Gör det fyrkantigt
		GridLayoutGroup layout = GetComponent<GridLayoutGroup> ();
		RectTransform rectTransform = layout.GetComponent<RectTransform> ();

		float gridWidth = (layout.cellSize.x + layout.spacing.x) * gridSize;
		float gridHeight = (layout.cellSize.y + layout.spacing.y) * gridSize;
		
		rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, gridWidth);
		rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, gridHeight);
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
