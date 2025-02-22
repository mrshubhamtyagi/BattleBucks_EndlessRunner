using UnityEngine;

namespace Shubham.Tyagi
{
    public static class Utils
    {
        private static float FACTOR => 1000;

        public static short[] QuantizePositionToShort(this Vector3 _position) =>
            new short[]
            {
                (short)(_position.x * FACTOR),
                (short)(_position.y * FACTOR),
                (short)(_position.z * FACTOR),
            };


        public static Vector3 QuantizePosition(this Vector3 _position) =>
            new Vector3(
                Mathf.Round(_position.x * FACTOR) / FACTOR,
                Mathf.Round(_position.y * FACTOR) / FACTOR,
                Mathf.Round(_position.z * FACTOR) / FACTOR
            );


        public static Vector3 ReveseQuantizePosition(this short[] _position) =>
            new Vector3(
                _position[0] / FACTOR,
                _position[1] / FACTOR,
                _position[2] / FACTOR
            );

        public static string ToReadableString(this short[] _position) =>
            $"({_position[0]}, {_position[1]}, {_position[2]})";
    }
}