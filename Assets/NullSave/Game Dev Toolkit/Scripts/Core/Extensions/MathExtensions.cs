//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("extensions")]
    [AutoDoc("This class contains methods that extends various math related items.")]
    public static class MathExtensions
    {

        #region Public Methods

        [AutoDoc("Convert any angle to range of -180 to +180")]
        [AutoDocParameter("Value to convert")]
        public static float Convert360to180(this float angle)
        {
            angle = FixAngle(angle);
            if (angle < -180) return angle + 360;
            if (angle > 180) return angle - 360;
            return angle;
        }

        [AutoDoc("Reduce any angle to range of -360 to 360")]
        [AutoDocParameter("Value to convert")]
        public static float FixAngle(this float angle)
        {
            do
            {
                if (angle < -360) angle += 360;
                if (angle > 360) angle -= 360;
            } while (angle < -360 || angle > 360);

            return angle;
        }

        [AutoDoc("Normalize the angle between -180 and 180 degrees")]
        [AutoDocParameter("Value to convert")]
        public static Vector3 NormalizeAngle(this Vector3 eulerAngle)
        {
            var delta = eulerAngle;

            delta.x = Convert360to180(delta.x);
            delta.y = Convert360to180(delta.y);
            delta.z = Convert360to180(delta.z);

            return new Vector3(delta.x, delta.y, delta.z);
        }

        [AutoDoc("Check if a point is inside of a quad")]
        [AutoDocParameter("Point to check")]
        [AutoDocParameter("Quad's top-left")]
        [AutoDocParameter("Quad's top-right")]
        [AutoDocParameter("Quad's bottom-left")]
        [AutoDocParameter("Quad's bottom-right")]
        public static bool PointInQuad(this Vector2 point, Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
        {
            if (PointInTriangle(point, topLeft, topRight, bottomLeft)) return true;
            if (PointInTriangle(point, topRight, bottomRight, bottomLeft)) return true;
            return false;
        }

        [AutoDoc("Check if a point is inside a triangle")]
        [AutoDocParameter("Point to check")]
        [AutoDocParameter("Triangle point 1")]
        [AutoDocParameter("Triangle point 2")]
        [AutoDocParameter("Triangle point 3")]
        public static bool PointInTriangle(this Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            float area = 0.5f * (-b.y * c.x + a.y * (-b.x + c.x) + a.x * (b.y - c.y) + b.x * c.y);
            float s = 1 / (2 * area) * (a.y * c.x - a.x * c.y + (c.y - a.y) * p.x + (a.x - c.x) * p.y);
            float t = 1 / (2 * area) * (a.x * b.y - a.y * b.x + (a.y - b.y) * p.x + (b.x - a.x) * p.y);
            return s >= 0 && t >= 0 && (s + t) <= 1;
        }

        #endregion

    }
}