using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	[ExecuteInEditMode]
	class DynamicGrid : MonoBehaviour
	{
		GridLayoutGroup gridLayoutGroup;
		RectTransform rect;
		public int cellCount = 2;

		public float Height = 64;

		void Start()
		{
			gridLayoutGroup = GetComponent<GridLayoutGroup>();
			rect = GetComponent<RectTransform>();
			OnRectTransformDimensionsChange();
		}

		void OnRectTransformDimensionsChange()
		{
			if (gridLayoutGroup != null && rect != null)
			{
				float totalWidth = rect.rect.width;
				gridLayoutGroup.cellSize = new Vector2(totalWidth, Height);
			}
		}
	}
}
