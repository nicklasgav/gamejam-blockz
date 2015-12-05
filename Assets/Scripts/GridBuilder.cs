using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GridBuilder : MonoBehaviour
{
	public int GridSize;
	public GameObject Slot;

	private GridLayoutGroup _layout;
	private Canvas _canvas;

	void Start() 
	{
		_canvas = GetComponentInParent<Canvas> ();
		_layout = GetComponent<GridLayoutGroup> ();

		// Bygg upp grid
		for (int i = 0; i < GridSize; i++) 
		{
			for(int j = 0; j < GridSize; j++) 
			{
				GameObject slot = Instantiate(Slot);
				slot.SetActive(true);
				slot.transform.SetParent(gameObject.transform);
			}
		}

		float gridWidth = (_layout.cellSize.x + _layout.spacing.x) * GridSize;
		float gridHeight = (_layout.cellSize.y + _layout.spacing.y) * GridSize;

		var rectTransform = _layout.GetComponent<RectTransform> ();

		rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, gridWidth);
		rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, gridHeight);
	}          
}
