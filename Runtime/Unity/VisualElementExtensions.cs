using UnityEngine.UIElements;

namespace Ametrin.Utils.Unity
{
    public static class VisualElementExtensions
    {
        public static void Margin(this IStyle style, float value) => style.Margin(value, value, value, value);
        public static void Margin(this IStyle style, float horizontal, float vertical) => style.Margin(horizontal, vertical, horizontal, vertical);
        public static void Margin(this IStyle style, float left, float top, float right, float bottom)
        {
            style.marginLeft = left;
            style.marginTop = top;
            style.marginRight = right;
            style.marginBottom = bottom;
        }

        public static void Padding(this IStyle style, float value) => style.Padding(value, value, value, value);
        public static void Padding(this IStyle style, float horizontal, float vertical) => style.Padding(horizontal, vertical, horizontal, vertical);
        public static void Padding(this IStyle style, float left, float top, float right, float bottom)
        {
            style.paddingLeft = left;
            style.paddingTop = top;
            style.paddingRight = right;
            style.paddingBottom = bottom;
        }

        public static void BorderRadius(this IStyle style, float value) => style.BorderRadius(value, value, value, value);
        public static void BorderRadius(this IStyle style, float topLeft, float topRight, float bottomRight, float bottomLeft)
        {
            style.borderTopLeftRadius = topLeft;
            style.borderTopRightRadius = topRight;
            style.borderBottomLeftRadius = bottomLeft;
            style.borderBottomRightRadius = bottomRight;
        }

        public static void Anchor(this IStyle style, float left, float top, float right, float bottom)
        {
            style.left = left;
            style.top = top;
            style.right = right;
            style.bottom = bottom;
        }
    }
}
