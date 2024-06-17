using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGTest
{
    internal class ThickLine2D: Shape2D
    {
        Vector2 lineEndPos = Vector2.Zero;
        Color lineEndColor = Color.White;
        float lineEndWidth = 0f;

        public override PrimitiveType PrimitiveType => PrimitiveType.TriangleStrip;
        public override int PrimitiveCount => vertexCount - 2;


        public void LineBegin(Vector2 position, Color color, float width)
        {
            lineEndPos = position;
            lineEndColor = color;
            lineEndWidth = width;
            NewOutlineIndexGoup();
        }

        public void LineBegin(Vector2 position, float width)
        {
            lineEndPos = position;
            lineEndColor = defaultColor;
            lineEndWidth = width;
            NewOutlineIndexGoup();
        }

        public void LineTo(Vector2 position, Color color, float width)
        {
            Vector2 direction = position - lineEndPos;
            Vector2 directionLeft = new Vector2(-direction.Y, direction.X);
            directionLeft.Normalize();
            Vector2 directionHalfLeftBegin = directionLeft * 0.5f * lineEndWidth;
            Vector2 directionHalfLeftEnd = directionLeft * 0.5f * width;
            Vector2 startLeft = lineEndPos + directionHalfLeftBegin;
            Vector2 startRight = lineEndPos - directionHalfLeftBegin;
            Vector2 endLeft = position + directionHalfLeftEnd;
            Vector2 endRight = position - directionHalfLeftEnd;
            lineEndPos = position;

            // left handed
            int index1 = AddVertex(startRight, lineEndColor); 
            int index2 = AddVertex(startLeft, lineEndColor); 
            int index3 = AddVertex(endRight, color); 
            int index4 = AddVertex(endLeft, color);

            if(LastIndicesGroupCount == 0)
            {
                AppendOutlineIndex(index3);
                AppendOutlineIndex(index1);
                AppendOutlineIndex(index2);
                AppendOutlineIndex(index4);
            }
            else
            {
                PependOutlineIndex(index3);
                AppendOutlineIndex(index4);
            }

        }

        public void LineTo(Vector2 position, float width)
        {
            LineTo(position, lineEndColor, width);
        }

        public void LineTo(Vector2 position, Color color)
        {
            LineTo(position, color, lineEndWidth);
        }

        public void LineTo(Vector2 position)
        {
            LineTo(position, lineEndColor, lineEndWidth);
        }
    }
}
