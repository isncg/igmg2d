using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Color = Microsoft.Xna.Framework.Color;

namespace MGTest
{
    internal class Line2D : Shape2D
    {
        Vector2 lineEndPos = Vector2.Zero;
        Color lineEndColor = Color.White;
        bool lineBegan = false;
        public override PrimitiveType PrimitiveType => PrimitiveType.LineList;
        public override int PrimitiveCount => vertexCount;

        public void AddLine(Vector2 from, Vector2 to)
        {
            AddVertex(from);
            AddVertex(to);
        }

        public void AddLine(Vector2 from, Vector2 to, Color color)
        {
            AddVertex(from, color);
            AddVertex(to, color);
        }

        public void AddLine(Vector2 from, Color fromColor, Vector2 to, Color toColor)
        {
            AddVertex(from, fromColor);
            AddVertex(to, toColor);
        }

        public void LineBegin(Vector2 pos, Color color)
        {
            lineEndPos = pos;
            lineEndColor = color;
            lineBegan = true;
        }

        public void LineBegin(Vector2 pos)
        {
            LineBegin(pos, defaultColor);
        }

        public void LineTo(Vector2 to, Color toColor)
        {
            if (vertexCount == 0)
            {
                if(lineBegan)
                    AddLine(lineEndPos, lineEndColor, to, toColor);
            }
            else
            {
                var last = vertex[vertex.Length - 1];
                var lastPosition = last.Position;
                AddLine(new Vector2(lastPosition.X, lastPosition.Y), last.Color, to, toColor);
            }
        }

        public void LineTo(Vector2 to)
        {
            if (vertexCount == 0)
            {
                if (lineBegan)
                    AddLine(lineEndPos, lineEndColor, to, lineEndColor);
            }
            else
            {
                var last = vertex[vertex.Length - 1];
                var lastPosition = last.Position;
                AddLine(new Vector2(lastPosition.X, lastPosition.Y), last.Color, to, lineEndColor);
            }
        }
    }
}
