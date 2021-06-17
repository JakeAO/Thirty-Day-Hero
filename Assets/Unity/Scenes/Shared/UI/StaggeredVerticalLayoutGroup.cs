using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scenes.Shared.UI
{
    [AddComponentMenu("Layout/Staggered Vertical Layout Group", 152)]
    /// <summary>
    /// Layout child layout elements similar to vertical, but staggered along the horizontal.
    /// </summary>
    public class StaggeredVerticalLayoutGroup : HorizontalOrVerticalLayoutGroup
    {
        protected StaggeredVerticalLayoutGroup()
        {
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            CalcAlongAxis(0, true);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void CalculateLayoutInputVertical()
        {
            CalcAlongAxis(1, true);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void SetLayoutHorizontal()
        {
            SetChildrenAlongAxis(0, true);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void SetLayoutVertical()
        {
            SetChildrenAlongAxis(1, true);

            float xSize = rectTransform.rect.size[0] - padding.horizontal;
            float xOffset = xSize / 3f;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];
                Vector3 pos = child.localPosition;
                if (i % 2 == 0)
                {
                    pos.x += xOffset;
                }
                else
                {
                    pos.x -= xOffset;
                }

                child.localPosition = pos;
            }
        }
    }
}