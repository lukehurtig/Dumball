using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dumball.Collisions
{
    public static class CollisionHelper
    {
        /// <summary>
        /// Detects a collision between two bounding circles
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>returns true if two circles are coliding</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >=
                Math.Pow(a.Center.X - b.Center.X, 2) +
                Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects a collision between two bounding rectangles
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>returns true if the two rectangles are colliding</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right ||
                     a.Top > b.Bottom || a.Bottom < b.Top);
        }

        /// <summary>
        /// Detects a collision between a rectangle and a circle
        /// </summary>
        /// <param name="c">Bounding circle</param>
        /// <param name="r">Bounding Rectangle</param>
        /// <returns>returns true if the two are colliding</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.X, r.X + r.Width);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Y, r.Y + r.Height);
            return Math.Pow(c.Radius, 2) >=
                Math.Pow(c.Center.X - nearestX, 2) +
                Math.Pow(c.Center.Y - nearestY, 2);
        }
    }
}
