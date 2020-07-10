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
				float paddingWidth = (cellCount + 1) * gridLayoutGroup.spacing.x;
				float cellSize = (totalWidth - paddingWidth) / cellCount + 2;
				gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
			}
		}
	}
}
