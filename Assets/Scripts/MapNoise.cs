using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapNoise
{
    public static class MyNoise
    {
        public static float noiseMap(float x, float z)
        {
            float e = Mathf.PerlinNoise(x ,z)
                   + 0.3f  * Mathf.PerlinNoise(2.0f*x,2.0f*z)
                   + 0.25f * Mathf.PerlinNoise(4.0f*x,4.0f*z)
                   + 0.13f * Mathf.PerlinNoise(8.0f*x,8.0f*z)
                   + 0.06f * Mathf.PerlinNoise(16.0f*x,16.0f*z);

            float d = 2 * Mathf.Max(Mathf.Abs((x/10)-0.5f),Mathf.Abs((z/10)-0.5f));
            return (Mathf.Pow(e,3f) - d + 1)/2;
        }
    }
}
