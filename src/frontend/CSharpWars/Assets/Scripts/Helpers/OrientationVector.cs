using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class OrientationVector
    {
        public static readonly Vector3 North = new Vector3(1f, 0f, 0f);
        public static readonly Vector3 East = new Vector3(0f, 0f, -1f);
        public static readonly Vector3 South = new Vector3(-1f, 0f, 0f);
        public static readonly Vector3 West = new Vector3(0f, 0f, 1f);

        public static Vector3 CreateFrom(PossibleOrientations orientation)
        {
            switch (orientation)
            {
                case PossibleOrientations.North:
                    return North;
                case PossibleOrientations.East:
                    return East;
                case PossibleOrientations.South:
                    return South;
                case PossibleOrientations.West:
                    return West;
            }
            return Vector3.zero;
        }
    }
}