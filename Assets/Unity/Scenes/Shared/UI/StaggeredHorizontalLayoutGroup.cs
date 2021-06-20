using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scenes.Shared.UI
{
    [AddComponentMenu("Layout/Staggered Vertical Layout Group", 152)]
    /// <summary>
    /// Layout child layout elements similar to vertical, but staggered along the horizontal.
    /// </summary>
    public class StaggeredHorizontalLayoutGroup : HorizontalOrVerticalLayoutGroup
    {
        protected StaggeredHorizontalLayoutGroup()
        {
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            CalcAlongAxis(0, false);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void CalculateLayoutInputVertical()
        {
            CalcAlongAxis(1, false);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void SetLayoutHorizontal()
        {
            SetChildrenAlongAxis(0, false);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void SetLayoutVertical()
        {
            SetChildrenAlongAxis(1, false);

            float ySize = rectTransform.rect.size[1] - padding.vertical;
            float yOffset = ySize / 3f;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];
                Vector3 pos = child.localPosition;
                if (i % 2 == 0)
                {
                    pos.y += yOffset;
                }
                else
                {
                    pos.y -= yOffset;
                }

                child.localPosition = pos;
            }
        }
    }
}