using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

public class OrbitalCamera : MonoBehaviour
{
    [SerializeField] float m_StartRho;
    [SerializeField] float m_StartThetaDeg;
    [SerializeField] float m_StartPhiDeg;

    float m_Rho;
    float m_Theta;
    float m_Phi;

    float m_TargetRho;
    float m_TargetTheta;
    float m_TargetPhi;

    [SerializeField] float m_RhoMin;
    [SerializeField] float m_RhoMax;

    [SerializeField] float m_PhiMinDeg;
    [SerializeField] float m_PhiMaxDeg;

    [SerializeField] float m_RhoSpeed;
    [SerializeField] float m_ThetaSpeed;
    [SerializeField] float m_PhiSpeed;
   
    [SerializeField] float m_RhoLerpSpeed;
    [SerializeField] float m_ThetaLerpSpeed;
    [SerializeField] float m_PhiLerpSpeed;


    [SerializeField] Transform m_Target;

    Vector3 m_PreviousMousePos;

    void SetSphericalPosition(Spherical sphPos)
    {
        transform.position = m_Target.position + CoordConvert.SphericalToCartesian(sphPos);
        transform.LookAt(m_Target);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Rho = m_StartRho;
        m_Theta = m_StartThetaDeg * Mathf.Deg2Rad;
        m_Phi = m_StartPhiDeg * Mathf.Deg2Rad;

        m_TargetRho = m_Rho;
        m_TargetTheta = m_Theta;
        m_TargetPhi = m_Phi;

        SetSphericalPosition(new Spherical(m_Rho, m_Theta, m_Phi));
        m_PreviousMousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 currentMousePos = Input.mousePosition;
        Vector3 mouseVect = currentMousePos - m_PreviousMousePos;

        m_TargetRho = Mathf.Clamp(m_TargetRho + m_RhoSpeed * Input.mouseScrollDelta.y, m_RhoMin, m_RhoMax);

        if(Input.GetMouseButton(1))
        {
            m_TargetTheta += mouseVect.x * m_ThetaSpeed;
            m_TargetPhi = Mathf.Clamp(m_TargetPhi + mouseVect.y * m_PhiSpeed, m_PhiMinDeg*Mathf.Deg2Rad, m_PhiMaxDeg*Mathf.Deg2Rad) ;
        }

        m_Rho = Mathf.Lerp(m_Rho,m_TargetRho,Time.deltaTime*m_RhoLerpSpeed);
        m_Theta = Mathf.Lerp(m_Theta, m_TargetTheta, Time.deltaTime*m_ThetaLerpSpeed);
        m_Phi = Mathf.Lerp(m_Phi, m_TargetPhi, Time.deltaTime*m_PhiLerpSpeed);

        SetSphericalPosition(new Spherical(m_Rho, m_Theta, m_Phi));

        m_PreviousMousePos = currentMousePos;
    }
}
