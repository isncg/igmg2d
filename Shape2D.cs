using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MGTest
{
    internal abstract class Shape2D
    {
        protected VertexPositionColorTexture[] vertex = new VertexPositionColorTexture[4];
        protected List<List<int>> outlineIndices = new ();
        protected Color defaultColor = Color.White;
        protected int vertexCount = 0;
        public virtual PrimitiveType PrimitiveType => PrimitiveType.PointList;
        public virtual int PrimitiveCount => vertexCount;

        public virtual Texture2D Texture => null;
        protected int AddVertex(Vector2 position, Color color, Vector2 uv)
        {
            int capacity = vertex.Length;
            if (capacity - vertexCount < 1)
            {
                Array.Resize(ref vertex, capacity * 2);
            }
            vertex[vertexCount++] = new VertexPositionColorTexture
            {
                Position = new Vector3(position, 0),
                Color = color,
                TextureCoordinate = uv,
            };
            return vertexCount - 1;
        }

        protected void AppendOutlineIndex(int index)
        {
            int count = outlineIndices.Count;
            if(count > 0)
                outlineIndices[count-1].Add(index);
        }

        protected int LastIndicesGroupCount
        {
            get
            {
                int count = outlineIndices.Count;
                if (count > 0)
                    return outlineIndices[count - 1].Count;
                return -1;
            }

        }

        protected void PependOutlineIndex(int index)
        {
            int count = outlineIndices.Count;
            if (count > 0)
                outlineIndices[count - 1].Insert(0, index);
        }

        protected void NewOutlineIndexGoup()
        {
            outlineIndices.Add(new List<int>());
        }

        protected int AddVertex(Vector2 position, Color color)
        {
            return AddVertex(position, color, Vector2.Zero);
        }

        protected int AddVertex(Vector2 position)
        {
            return AddVertex(position, defaultColor, Vector2.Zero);
        }

        public void Clear()
        {
            vertexCount = 0;
            outlineIndices.Clear();
        }

        public void Draw(GraphicsDevice device, BasicEffect effect)
        {
            effect.Texture = this.Texture;
            effect.TextureEnabled = Texture != null;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserPrimitives(PrimitiveType, vertex, 0, PrimitiveCount);
            }
        }

        public void DrawOutline(Canvas2D canvas, float thickness, Color color)
        {
            foreach(var outlineidx in outlineIndices)
            {
                if(outlineidx.Count > 2)
                {
                    Vector3 start = (vertex[outlineidx[0]].Position + vertex[outlineidx[outlineidx.Count - 1]].Position) * 0.5f;
                    Vector2 startpos = new Vector2(start.X, start.Y);
                    canvas.ThickLineBegin(startpos, color, thickness);
                    foreach(var i in outlineidx)
                    {
                        var vert = vertex[i];
                        canvas.ThickLineTo(new Vector2(vert.Position.X, vert.Position.Y));
                    }
                    canvas.ThickLineTo(startpos);
                }
            }
        }
    }
}
