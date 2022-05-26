using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AngleHelpers 
{
    public static float ModuloSignClamp(float number, float modulo_sign)
    {
        return (number % modulo_sign + modulo_sign) % modulo_sign;
    }

    public static int AngleXFactor360(float angle)
    {
        return (angle <= 90 || angle >= 270) ? 1 : -1;
    }

    public static int AngleXFactor180(float angle)
    {
        if(angle > 0)
        {
            return angle <= 90 ? 1 : -1;
        }
        else
        {
            return angle >= -90 ? 1 : -1;
        }
    }
}
