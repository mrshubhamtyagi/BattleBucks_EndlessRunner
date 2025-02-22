using UnityEngine;

namespace Shubham.Tyagi
{
    public static class Utils
    {
        public static Vector3 QuantizePosition(this Vector3 _position)
        {
            float _factor = 100f;
            return new Vector3(
                Mathf.Round(_position.x * _factor) / _factor,
                Mathf.Round(_position.y * _factor) / _factor,
                Mathf.Round(_position.z * _factor) / _factor
            );
        }
    }
}