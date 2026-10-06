using UnityEngine;

public static class EfectosVisualesNuevo
{
    public static void Destello(Vector3 posicion, Color color, int particulas=12)
    {
        var objeto=new GameObject("Destello de partida");objeto.transform.position=posicion;
        var sistema=objeto.AddComponent<ParticleSystem>();
        var main=sistema.main;main.duration=.42f;main.loop=false;main.playOnAwake=false;
        main.startLifetime=new ParticleSystem.MinMaxCurve(.28f,.58f);
        main.startSpeed=new ParticleSystem.MinMaxCurve(1.3f,3.2f);
        main.startSize=new ParticleSystem.MinMaxCurve(.07f,.15f);
        main.startColor=color;main.gravityModifier=.35f;main.simulationSpace=ParticleSystemSimulationSpace.World;
        var emission=sistema.emission;emission.enabled=false;
        var forma=sistema.shape;forma.shapeType=ParticleSystemShapeType.Sphere;forma.radius=.12f;
        var renderer=sistema.GetComponent<ParticleSystemRenderer>();renderer.sortingOrder=7;
        sistema.Play();sistema.Emit(particulas);Object.Destroy(objeto,1.1f);
    }
}
