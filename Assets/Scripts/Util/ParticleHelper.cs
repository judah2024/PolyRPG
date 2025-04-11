using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;

public class ParticleHelper
{

    static public void Generator(string _resPath, Vector3 _pos, Vector3 _dir)
    {
        ParticleSystem loadedParticle = Resources.Load<ParticleSystem>(_resPath);
        ParticleSystem hitParticle = GameObject.Instantiate(loadedParticle);
        hitParticle.transform.position = _pos;
        hitParticle.transform.forward = _dir;
    }

}
