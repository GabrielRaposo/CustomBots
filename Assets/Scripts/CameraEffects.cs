using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour {

	public IEnumerator Shake(float intensity = 1f)
    {
        intensity /= 10;
        transform.position = new Vector3(intensity / 2, 0, -10);
        int dir = -1;
        
        while(intensity > 0)
        {
            yield return null;
            yield return null;

            transform.position = new Vector3(transform.position.x + (intensity * dir), 0, -10);
            dir = -dir;
            intensity -= 0.01f;
        }
    }
}
