using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

[RequireComponent(typeof(MeshRenderer))]
public class Animate : MonoBehaviour
{
    MeshRenderer m_MeshRenderer;

    delegate Vector3 ComputePositionDelegate(float t);

    ComputePositionDelegate m_AnimateFunction;

    Transform m_Transform;

    [SerializeField] float m_Rho=1;
    [SerializeField] float m_Speed=1;

    Vector3 Animation1(float t)
    {
        Cylindrical cyl = new Cylindrical(m_Rho, t, Mathf.PingPong(t*m_Speed,10));
        return CoordConvert.CylindricalToCartesian(cyl);
    }

    Vector3 Animation2(float t)
    {
        Spherical sph = new Spherical(m_Rho, t, Mathf.PI*(.5f+.15f*Mathf.Sin(t*m_Speed)));
        return CoordConvert.SphericalToCartesian(sph);
    }

    private void Awake()
    {
        m_Transform = transform;
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_AnimateFunction = Animation2;
    }

    // Update is called once per frame
    void Update()
    {
        m_Transform.position = m_AnimateFunction(Time.time);

        //Cylindrical cyl = CoordConvert.CartesianToCylindrical(m_Transform.position);
        //m_MeshRenderer.material.color= Color.white * (cyl.theta / (Mathf.PI * 2));
        Spherical sph = CoordConvert.CartesianToSpherical(m_Transform.position);
        m_MeshRenderer.material.color = sph.phi < Mathf.PI / 2f ? Color.white : Color.black;

    }
}
