using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class InicializadorRuntimeNuevo
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RegistrarEscenas()
    {
        SceneManager.sceneLoaded-=ConstruirEscena;
        SceneManager.sceneLoaded+=ConstruirEscena;
    }
    static void ConstruirEscena(Scene escena,LoadSceneMode modo)
    {
        if(escena.name!="Menu"&&escena.name!="Pradera"&&escena.name!="Final")return;
        AudioJuegoNuevo.Asegurar();
        if(escena.name=="Menu"){MundoRuntimeNuevo.CrearFondo(new Color(.38f,.72f,.69f));HUDNuevo.CrearMenu(false);}
        else if(escena.name=="Final"){MundoRuntimeNuevo.CrearFondo(new Color(.23f,.41f,.48f));HUDNuevo.CrearMenu(true);}
        else{EstadoPartidaNuevo.Iniciar(5);MundoRuntimeNuevo.CrearNivel();HUDNuevo.CrearPartida();}
        string[] args=Environment.GetCommandLineArgs();
        if(Array.Exists(args,a=>a=="--smoke-test"||a=="--smoke-test-death"||a=="--captura-entrega"))new GameObject("Prueba automática").AddComponent<PruebaRuntimeNuevo>().escena=escena.name;
    }
}

static class MundoRuntimeNuevo
{
    public static void CrearFondo(Color fondo)
    {
        var camara=new GameObject("Cámara principal");camara.transform.position=new Vector3(0,0,-10);var cam=camara.AddComponent<Camera>();cam.orthographic=true;cam.orthographicSize=6.75f;cam.backgroundColor=fondo;cam.clearFlags=CameraClearFlags.SolidColor;camara.AddComponent<AudioListener>();
        Forma("Sol",SpriteFormaNuevo.Forma.Circulo,new Vector2(8.7f,4.4f),new Vector2(1.7f,1.7f),new Color(1,.77f,.3f),-5);
        Forma("Colina lejana",SpriteFormaNuevo.Forma.Circulo,new Vector2(-3,-6.4f),new Vector2(18,6),new Color(.2f,.54f,.48f),-4);
        Forma("Colina cercana",SpriteFormaNuevo.Forma.Circulo,new Vector2(5,-7.1f),new Vector2(17,5),new Color(.15f,.4f,.39f),-3);
        Forma("Nube",SpriteFormaNuevo.Forma.Circulo,new Vector2(-8,4),new Vector2(3.4f,1.1f),new Color(.82f,.94f,.88f),-2);
    }
    public static void CrearNivel()
    {
        CrearFondo(new Color(.51f,.79f,.69f));
        Solido("Suelo",new Vector2(0,-4f),new Vector2(26,1.2f),new Color(.21f,.48f,.31f));
        Solido("Plataforma musgo 1",new Vector2(-7,-2.2f),new Vector2(4.3f,.55f),new Color(.3f,.61f,.36f));
        Solido("Plataforma musgo 2",new Vector2(-2,.15f),new Vector2(4.2f,.55f),new Color(.3f,.61f,.36f));
        Solido("Plataforma musgo 3",new Vector2(3,-1.15f),new Vector2(4.2f,.55f),new Color(.3f,.61f,.36f));
        Solido("Plataforma musgo 4",new Vector2(7.5f,.7f),new Vector2(4.3f,.55f),new Color(.3f,.61f,.36f));
        Solido("Plataforma musgo 5",new Vector2(10,-1.55f),new Vector2(3.5f,.55f),new Color(.3f,.61f,.36f));
        Jugadora();
        Vector2[] gotas={new(-7,-1.35f),new(-2,1.15f),new(3,-.4f),new(7.5f,1.7f),new(10,-.8f)};
        foreach(var p in gotas){var go=Forma("Gota de néctar",SpriteFormaNuevo.Forma.Rombo,p,new Vector2(.65f,.82f),new Color(1,.76f,.12f),2);go.AddComponent<NectarNuevo>();var hit=go.AddComponent<CircleCollider2D>();hit.isTrigger=true;}
        Enemigo(-2.5f,.475f);Enemigo(4.3f,-.825f);Enemigo(9.2f,.575f);
        var meta=Forma("Portal de salida",SpriteFormaNuevo.Forma.Circulo,new Vector2(11.35f,-2.5f),new Vector2(1.1f,2.2f),new Color(.49f,.3f,.9f),1);meta.AddComponent<MetaNuevo>();var mc=meta.AddComponent<BoxCollider2D>();mc.isTrigger=true;
    }
    static void Jugadora()
    {
        var go=Forma("Exploradora",SpriteFormaNuevo.Forma.Cuadro,new Vector2(-10,-2.85f),new Vector2(.85f,1.05f),new Color(.18f,.73f,.77f),3,true);
        var rb=go.AddComponent<Rigidbody2D>();rb.freezeRotation=true;rb.collisionDetectionMode=CollisionDetectionMode2D.Continuous;rb.gravityScale=3.4f;
        var box=go.AddComponent<BoxCollider2D>();box.size=new Vector2(.8f,.95f);go.AddComponent<JugadorNuevo>();
    }
    static void Enemigo(float x,float y)
    {var go=Forma("Pincho errante",SpriteFormaNuevo.Forma.Circulo,new Vector2(x,y+.4f),new Vector2(.9f,.9f),new Color(.94f,.34f,.23f),3,true);go.AddComponent<EnemigoNuevo>();var col=go.AddComponent<CircleCollider2D>();col.isTrigger=true;}
    static void Solido(string nombre,Vector2 pos,Vector2 size,Color color)
    {
        var go=Forma(nombre,SpriteFormaNuevo.Forma.Cuadro,pos,size,color,0);go.AddComponent<BoxCollider2D>();
        Forma(nombre+" borde de hierba",SpriteFormaNuevo.Forma.Cuadro,pos+Vector2.up*(size.y*.41f),new Vector2(size.x,.12f),new Color(.47f,.72f,.39f),1);
    }
    static GameObject Forma(string nombre,SpriteFormaNuevo.Forma forma,Vector2 pos,Vector2 size,Color color,int orden,bool cara=false)
    {var go=new GameObject(nombre);go.transform.position=pos;go.transform.localScale=new Vector3(size.x,size.y,1);var sr=go.AddComponent<SpriteRenderer>();sr.sortingOrder=orden;go.AddComponent<SpriteFormaNuevo>().Configurar(forma,color,cara);return go;}
}
