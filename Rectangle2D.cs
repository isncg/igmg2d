using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGTest
{
    internal class Rectangle2D: Shape2D
    {
        public override int PrimitiveCount => vertexCount / 3;
        public override PrimitiveType PrimitiveType => PrimitiveType.TriangleList;
        private Texture2D _texture;
        public override Texture2D Texture => _texture;
        private static Vector4 DefaultSpriteBorders = new Vector4(0, 0, 1, 1);// left, top, right, bottom
        private static Vector2 DefaultPivot = new Vector2(0.5f, 0.5f);

        public void SetTexture(Texture2D texture)
        {
            _texture = texture;
        }

        public void Rect(Vector4 spriteBorders, Vector2 position, Vector2 size, Vector2 pivot, float rotation)
        {
            float m11 = MathF.Cos(rotation);
            float m12 = -MathF.Sin(rotation);
            float m21 = -m12;
            float m22 = m11;

            unsafe
            {
                var localCorners = stackalloc Vector2[4];
                localCorners[0].X = localCorners[1].X = -size.X * pivot.X;
                localCorners[2].X = localCorners[3].X = size.X * (1 - pivot.X);
                
                localCorners[0].Y = localCorners[3].Y = -size.Y * pivot.Y;
                localCorners[1].Y = localCorners[2].Y = size.Y * (1 - pivot.Y);

                for (int i = 0; i < 4; i++ )
                {
                    // rotation
                    float tx = m11 * localCorners[i].X + m12 * localCorners[i].Y;
                    float ty = m21 * localCorners[i].X + m22 * localCorners[i].Y;
                    localCorners[i].X = tx;
                    localCorners[i].Y = ty;
                    // position
                    localCorners[i] += position;
                }
                int index1 = AddVertex(localCorners[0], defaultColor, new Vector2(spriteBorders.X, spriteBorders.W));
                int index2 = AddVertex(localCorners[1], defaultColor, new Vector2(spriteBorders.X, spriteBorders.Y));
                int index3 = AddVertex(localCorners[2], defaultColor, new Vector2(spriteBorders.Z, spriteBorders.Y));

                AddVertex(localCorners[0], defaultColor, new Vector2(spriteBorders.X, spriteBorders.W));
                AddVertex(localCorners[2], defaultColor, new Vector2(spriteBorders.Z, spriteBorders.Y));
                int index4 = AddVertex(localCorners[3], defaultColor, new Vector2(spriteBorders.Z, spriteBorders.W));
                NewOutlineIndexGoup();
                AppendOutlineIndex(index1);
                AppendOutlineIndex(index2);
                AppendOutlineIndex(index3);
                AppendOutlineIndex(index4);
            }

        }

        public void Rect(Vector2 position, Vector2 size)
        {
            Rect(DefaultSpriteBorders, position, size, DefaultPivot, 0);
        }
    }
}
