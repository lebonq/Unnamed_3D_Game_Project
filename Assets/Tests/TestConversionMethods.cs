using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

public class TestConversionMethods : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        for (float theta = 0; theta <= Mathf.PI*2; theta+=Mathf.PI/20)
        {
            Cylindrical cyl = new Cylindrical(1, theta, 0);
            Cylindrical newCyl = CoordConvert.CartesianToCylindrical(CoordConvert.CylindricalToCartesian(cyl));
            Debug.Log("theta = " + theta + "     newTheta = " + newCyl.theta);
        }

        for (float theta = 0; theta <= Mathf.PI * 2; theta += Mathf.PI / 20)
        {
            Vector3 pos = CoordConvert.CylindricalToCartesian(new Cylindrical(1, theta, 0));
            float angle = Mathf.Atan2(pos.z, pos.x);
            Debug.Log(angle);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
