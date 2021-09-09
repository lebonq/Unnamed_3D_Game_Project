using System.Collections;

using System.Collections.Generic;

using UnityEngine;

namespace MyMathTools

{

  [System.Serializable] public struct Spherical
  {
    public float rho {get;}
    public float theta {get;}
    public float phi {get;}

    public Spherical(Spherical sph)

    {

      this.rho = sph.rho;

      this.theta = sph.theta;

      this.phi = sph.phi;

    }

    public Spherical(float rho, float theta, float phi)

    {

      this.rho = rho;

      this.theta = theta;

      this.phi = phi;

    }

    public Vector3 ToCartesian()

    {

      return CoordConvert.SphericalToCartesian(this);

    }

    public Spherical Lerp(Spherical targetSph, float k)

    {

      return new Spherical(Mathf.Lerp(this.rho, targetSph.rho, k), Mathf.Lerp(this.theta, targetSph.theta, k), Mathf.Lerp(this.phi, targetSph.phi, k));

    }

  }

  [System.Serializable]

  public struct Cylindrical

  {

    public float rho {
      get;
    }

    public float theta {
      get;
    }

    public float y {
      get;
    }

    public Cylindrical(float rho, float theta, float y)

    {

      this.rho = rho;

      this.theta = theta;

      this.y = y;

    }

    public Cylindrical(Cylindrical cyl)

    {

      this.rho = cyl.rho;

      this.theta = cyl.theta;

      this.y = cyl.y;

    }

    public Vector3 ToCartesian()

    {

      return CoordConvert.CylindricalToCartesian(this);

    }

    public Cylindrical Lerp(Cylindrical targetCyl, float k)

    {

      return new Cylindrical(Mathf.Lerp(this.rho, targetCyl.rho, k), Mathf.Lerp(this.theta, targetCyl.theta, k), Mathf.Lerp(this.y, targetCyl.y, k));

    }

  }

  [System.Serializable]

  public struct Polar

  {

    public float rho {
      get;
    }

    public float theta {
      get;
    }

    public Polar(float rho, float theta)

    {

      this.rho = rho;

      this.theta = theta;

    }

  }

  public static class CoordConvert

  {

    public static Vector3 CylindricalToCartesian(Cylindrical cyl)

    {

      return new Vector3(cyl.rho * Mathf.Cos(cyl.theta), cyl.y, cyl.rho * Mathf.Sin(cyl.theta));

    }

    public static Cylindrical CartesianToCylindrical(Vector3 pos)

    {

      float theta = 0;

      if (Mathf.Approximately(pos.x, 0)) // epsilon if (pos.x==0)  if(1f==10f/10f)

      {

        if (Mathf.Approximately(pos.z, 0)) theta = 0;

        else theta = Mathf.PI / 2f * Mathf.Sign(pos.z);

      } else

      {

        theta = Mathf.Atan2(pos.z, pos.x);

      }

      if (theta < 0) theta += Mathf.PI * 2;

      return new Cylindrical(Mathf.Sqrt(pos.x * pos.x + pos.z * pos.z), theta, pos.y);

    }

    public static Vector3 SphericalToCartesian(Spherical sph)

    {

      return new Vector3(sph.rho * Mathf.Cos(sph.theta) * Mathf.Sin(sph.phi), sph.rho * Mathf.Cos(sph.phi), sph.rho * Mathf.Sin(sph.theta) * Mathf.Sin(sph.phi));

    }
    public static Spherical CartesianToSpherical(Vector3 pos)

    {

      float rho = Mathf.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);

      if (Mathf.Approximately(rho, 0)) return new Spherical(0, 0, 0);

      float theta = 0;

      if (Mathf.Approximately(pos.x, 0))

      {

        if (Mathf.Approximately(pos.z, 0)) theta = 0;

        else theta = Mathf.PI / 2f * Mathf.Sign(pos.z);

      } else

      {

        theta = Mathf.Atan2(pos.z, pos.x);

      }

      if (theta < 0) theta += Mathf.PI * 2;

      // y = rho * cos(phi)  phi = acos(y/rho)

      return new Spherical(rho, theta, Mathf.Acos(pos.y / rho));

    }

  }

}