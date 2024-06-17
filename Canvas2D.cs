using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MGTest
{
    class Canvas2D
    {
        List<Shape2D> shapeList = new List<Shape2D>();
        GraphicsDevice device;
        BasicEffect effect;
        bool outlineEnabled = false;
        float outlineThickness = 0;
        Color outlineColor = Color.White;
        Dictionary<Type, List<Shape2D>> shapePool = new Dictionary<Type, List<Shape2D>>();
        public Canvas2D(GraphicsDevice device)
        {
            this.device = device;
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;
        }

        public void Clear()
        {
            foreach(var shape in shapeList)
            {
                shape.Clear();
                if(shapePool.TryGetValue(shape.GetType(), out var pool))
                    pool.Add(shape);
                else
                {
                    pool = new List<Shape2D>();
                    shapePool[shape.GetType()] = pool;
                    pool.Add(shape);
                }
            }
            shapeList.Clear();
        }
        public void Draw()
        {
            var viewport = device.Viewport;
            Matrix M = Matrix.Identity;
            if (viewport.Width > viewport.Height)
            {
                M.M11 = (float)viewport.Height / viewport.Width;
            }
            else
            {
                M.M22 = (float)viewport.Width / viewport.Height;
            }
            effect.View = M;

            foreach (var shape in shapeList)
            {
                shape.Draw(device, effect);
            }
        }


        private T GetLastShape<T>() where T : Shape2D
        {
            if (shapeList.Count > 0)
            {
                var last = shapeList.Last() as T;
                if (last != null)
                    return last;
            }
            return null;
        }
        private T GetShape<T>() where T:Shape2D, new()
        {
            var lastShape = GetLastShape<T>();
            if(null != lastShape)
                return lastShape;
            return NewShape<T>();
        }

        public T NewShape<T>() where T:Shape2D, new()
        {
            if(shapePool.TryGetValue(typeof(T), out var pool))
            {
                int count = pool.Count;
                if(count > 0)
                {
                    var last = pool.Last() as T;
                    pool.RemoveAt(count - 1);
                    shapeList.Add(last);
                    return last;
                }
            }
            var shape = new T();
            shapeList.Add(shape);
            return shape;
        }

        public void Line(Vector2 from, Vector2 to)
        {
            var line2d = GetShape<Line2D>();
            line2d.AddLine(from, to);
        }

        public void LineBegin(Vector2 from, Color color)
        {
            var line2d = GetShape<Line2D>();
            line2d.LineBegin(from, color);
        }

        public void LineBegin(Vector2 from)
        {
            var line2d = GetShape<Line2D>();
            line2d.LineBegin(from);
        }

        public void LineTo(Vector2 to)
        {
            var lastLine2d = GetLastShape<Line2D>();
            if (null != lastLine2d)
            {
                lastLine2d.LineTo(to);
                return;
            }
        }

        public void ThickLineBegin(Vector2 from, Color color, float width)
        {
            var thickLine2d = NewShape<ThickLine2D>();
            thickLine2d.LineBegin(from, color, width);
        }

        public void ThickLineBegin(Vector2 from, float width)
        {
            var thickLine2d = NewShape<ThickLine2D>();
            thickLine2d.LineBegin(from, width);
        }

        public void ThickLineTo(Vector2 to, Color color, float width)
        {
            var thickLine2d = GetLastShape<ThickLine2D>();
            if (null != thickLine2d)
            {
                thickLine2d.LineTo(to, color, width);
            }

        }

        public void ThickLineTo(Vector2 to, float width)
        {
            var thickLine2d = GetLastShape<ThickLine2D>();
            if (null != thickLine2d)
            {
                thickLine2d.LineTo(to, width);
            }
        }

        public void ThickLineTo(Vector2 to, Color color)
        {
            var thickLine2d = GetLastShape<ThickLine2D>();
            if (null != thickLine2d)
            {
                thickLine2d.LineTo(to, color);
            }
        }

        public void ThickLineTo(Vector2 to)
        {
            var thickLine2d = GetLastShape<ThickLine2D>();
            if (null != thickLine2d)
            {
                thickLine2d.LineTo(to);
            }
        }

        public void ThickLine(Vector2 from, Color fromColor, float fromWidth, Vector2 to, Color toColor, float toWidth)
        {
            var thickLine2d = NewShape<ThickLine2D>();
            thickLine2d.LineBegin(from, fromColor, fromWidth);
            thickLine2d.LineTo(to, toColor, toWidth);
        }

        public void Rectangle(Vector2 position, Vector2 size, Texture2D texture)
        {
            var rect = GetLastShape<Rectangle2D>();
            if (null == rect || rect.Texture != texture)
            {
                rect = NewShape<Rectangle2D>();
                rect.SetTexture(texture);
            }
            rect.Rect(position, size);
        }

        public void Outline(float thickness, Color color)
        {
            if(shapeList.Count > 0)
            {
                var shape = shapeList.Last();
                //NewShape<ThickLine2D>();
                shape.DrawOutline(this, thickness, color);
            }

            //outlineThickness = thickness;
            //outlineColor = color;
        }

        //public void EnableOutline(bool enabled)
        //{
        //    outlineEnabled = enabled;
        //}
    }
}
