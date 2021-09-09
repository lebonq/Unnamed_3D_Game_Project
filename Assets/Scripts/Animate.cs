using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

public class Animate : MonoBehaviour
{
    delegate Vector3 ComputePositionDelegate(float t);

    ComputePositionDelegate AnimationMethod;
    [SerializeField] float m_Rho;
    [SerializeField] float m_Speed;

    Vector3 MyAnimationMethod1(float t)
    {
        Cylindrical cyl = new Cylindrical(m_Rho, t*m_Speed, 0);
        return CoordConvert.CylindricalToCartesian(cyl);
    }
    Vector3 MyAnimationMethod2(float t)
    {
        Spherical sph = new Spherical(m_Rho, t * m_Speed, Mathf.PingPong(Mathf.Clamp(t/4, 0, Mathf.PI),1));
        return CoordConvert.SphericalToCartesian(sph);
    }

    // Start is called before the first frame update
    void Start()
    {
        AnimationMethod = MyAnimationMethod2;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(4f * Time.deltaTime,0 ,0);
        transform.position = AnimationMethod(Time.time);
    }
}
