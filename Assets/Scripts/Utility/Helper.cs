using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Helper
{
    // YOU DONT HAVE TO UNDERSTAND WHY THIS WORKS
    // This sets the up axis without resetting the other axis.
    public static Quaternion SetUpAndYAxisRotation(Vector3 forward, Vector3 up)
    {
        Quaternion zToUp = Quaternion.LookRotation(up, -forward);
        Quaternion yToz = Quaternion.Euler(90, 0, 0);
        return zToUp * yToz;
    }
    
    //Removes specific component from vector3 as normal method
    public static Vector3 RemoveVectorComponent(Vector3 copy, VectorValue toRemove)
    {
        switch (toRemove)
        {
            case VectorValue.X:
                copy.x = 0;
                break;
            
            case VectorValue.Y:
                copy.y = 0;
                break;
            
            case VectorValue.Z:
                copy.z = 0;
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(toRemove), toRemove, null);
        }

        return copy;
    }
    
    /// <summary>
    /// Removes specific component from vector3 as EXTENSION METHOD with REFERENCE PARAMETER
    /// There is no need for a return type since we use the REFERENCE of the vector to directly change the value instead of making a copy.
    /// </summary>
    public static void RemoveVectorComponentExtended(this ref Vector3 original, VectorValue toRemove)
    {
        switch (toRemove)
        {
            case VectorValue.X:
                original.x = 0;
                break;
            
            case VectorValue.Y:
                original.y = 0;
                break;
            
            case VectorValue.Z:
                original.z = 0;
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(toRemove), toRemove, null);
        }
    }
}
