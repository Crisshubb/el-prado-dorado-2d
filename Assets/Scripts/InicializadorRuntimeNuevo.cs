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
        var paisaje=Pixel("Paisaje pixel art","FondoPradera",Vector2.zero,cam.orthographicSize*2,-20);
        if(paisaje==null)Debug.LogError("No se encontró Resources/Art/FondoPradera.png");
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
        foreach(var p in gotas){var go=Pixel("Gota de néctar","Nectar",p,.82f,2);go.AddComponent<NectarNuevo>();var hit=go.AddComponent<CircleCollider2D>();hit.radius=.34f/go.transform.localScale.x;hit.isTrigger=true;}
        Enemigo(-2.5f,.475f);Enemigo(4.3f,-.825f);Enemigo(9.2f,.575f);
        var meta=Pixel("Portal de salida","Portal",new Vector2(11.35f,-2.5f),2.2f,1);meta.AddComponent<MetaNuevo>();var mc=meta.AddComponent<BoxCollider2D>();mc.size=new Vector2(.9f/meta.transform.localScale.x,1.8f/meta.transform.localScale.y);mc.isTrigger=true;
    }
    static void Jugadora()
    {
        var go=Pixel("Exploradora","Exploradora",new Vector2(-10,-2.85f),1.12f,3);
        var rb=go.AddComponent<Rigidbody2D>();rb.freezeRotation=true;rb.collisionDetectionMode=CollisionDetectionMode2D.Continuous;rb.gravityScale=3.4f;
        float escala=go.transform.localScale.x;var box=go.AddComponent<BoxCollider2D>();box.size=new Vector2(.74f/escala,.96f/escala);go.AddComponent<JugadorNuevo>();
    }
    static void Enemigo(float x,float y)
    {var go=Pixel("Pincho errante","Pincho",new Vector2(x,y+.4f),.9f,3);go.AddComponent<EnemigoNuevo>();var col=go.AddComponent<CircleCollider2D>();col.radius=.4f/go.transform.localScale.x;col.isTrigger=true;}
    static void Solido(string nombre,Vector2 pos,Vector2 size,Color color)
    {
        var go=Forma(nombre,SpriteFormaNuevo.Forma.Cuadro,pos,Vector2.one,color,0);var render=go.GetComponent<SpriteRenderer>();render.drawMode=SpriteDrawMode.Tiled;render.size=size;
        var col=go.AddComponent<BoxCollider2D>();col.size=size;
    }
    static GameObject Pixel(string nombre,string recurso,Vector2 posicion,float alto,int orden)
    {
        var sprite=Resources.Load<Sprite>("Art/"+recurso);
        if(sprite==null){Debug.LogError("Sprite pixel art faltante: Resources/Art/"+recurso);return new GameObject(nombre+" (faltante)");}
        var go=new GameObject(nombre);go.transform.position=posicion;float escala=alto/sprite.bounds.size.y;go.transform.localScale=Vector3.one*escala;
        var sr=go.AddComponent<SpriteRenderer>();sr.sprite=sprite;sr.sortingOrder=orden;return go;
    }
    static GameObject Forma(string nombre,SpriteFormaNuevo.Forma forma,Vector2 pos,Vector2 size,Color color,int orden,bool cara=false)
    {var go=new GameObject(nombre);go.transform.position=pos;go.transform.localScale=new Vector3(size.x,size.y,1);var sr=go.AddComponent<SpriteRenderer>();sr.sortingOrder=orden;go.AddComponent<SpriteFormaNuevo>().Configurar(forma,color,cara);return go;}
}
