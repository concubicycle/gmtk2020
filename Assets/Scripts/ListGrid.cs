using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    enum DynamicGridDirection
	{ 
		Vertical,
		Horizontal
	}

	[ExecuteInEditMode]
	[RequireComponent(typeof(GridLayoutGroup))]
	class ListGrid : MonoBehaviour
	{
		public DynamicGridDirection direction;
		public bool StretchElements = true;


		private GridLayoutGroup _gridLayoutGroup;
		private RectTransform _rect;

        private void Start()
        {
			OnRectTransformDimensionsChange();
		}

        void OnRectTransformDimensionsChange()
		{
			_gridLayoutGroup = GetComponent<GridLayoutGroup>();
			_rect = GetComponent<RectTransform>();

			float totalWidth = _rect.rect.width;
			float totalHeight = _rect.rect.height;

			if (direction == DynamicGridDirection.Horizontal)
            {
				float paddingSize = _gridLayoutGroup.spacing.x * (_rect.childCount + 1);
				float gridSizeX = StretchElements
					? (totalWidth - paddingSize) / _rect.childCount
					: _gridLayoutGroup.cellSize.x;
				_gridLayoutGroup.cellSize = new Vector2(gridSizeX, totalHeight);
			}
			else
            {
				float paddingSize = _gridLayoutGroup.spacing.y * (_rect.childCount + 1);
				float gridSizeY = StretchElements
					? (totalHeight - paddingSize) / _rect.childCount
					: _gridLayoutGroup.cellSize.y;
				_gridLayoutGroup.cellSize = new Vector2(totalWidth, gridSizeY);
			}
		}
	}
}
