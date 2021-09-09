using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

public class OrbitalCamera : MonoBehaviour
{
    [SerializeField] float m_Rho;
    [SerializeField] float m_Theta;
    [SerializeField] float m_Phi;

    [SerializeField] Transform m_Target;

    Vector3 m_PreviousMousePos;
    Vector2 m_PreviousMouseScroll;

    // Start is called before the first frame update
    void Start()
    {
        m_PreviousMousePos = Input.mousePosition;
        m_PreviousMouseScroll = Input.mouseScrollDelta;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentMousePos = Input.mousePosition;
        Vector2 currentMouseScroll = Input.mouseScrollDelta;

        Vector3 mouseVect = currentMousePos - m_PreviousMousePos;
        Vector2 scrollVect = currentMouseScroll - m_PreviousMouseScroll;

        if ((Mathf.Approximately(m_Rho, 0f) & currentMouseScroll.y > 0f))
        {
           //On est au max du zoom on ne fait rien
        }
        else
        {
            m_Rho -= currentMouseScroll.y / 2; //On divise par 2 pour avoir plus de precision sur le zoom
        }

        m_Theta += (mouseVect.x * Mathf.PI * 2 / 800);  //Un deplacemnt de 800 pixel est un tour complet
        float phi_temp = m_Phi - (mouseVect.y * Mathf.PI * 2 / 800);

        if(phi_temp < Mathf.PI & phi_temp > 0.0f){
            m_Phi = phi_temp;
        }

        Spherical m_SphCoord = new Spherical(m_Rho, m_Theta, m_Phi);
        transform.position = m_Target.position + CoordConvert.SphericalToCartesian(m_SphCoord);
        transform.LookAt(m_Target);

        m_PreviousMousePos = currentMousePos;
        m_PreviousMouseScroll = currentMouseScroll;
    }
}
