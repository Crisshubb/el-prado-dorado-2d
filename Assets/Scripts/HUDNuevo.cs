using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class HUDNuevo
{
    static Text estado;
    static Sprite botonSprite;
    static Transform canvasPartida;
    static GameObject pausaPanel;
    public static bool Pausado { get; private set; }
    public static void Actualizar(){if(estado!=null)estado.text=$"NÉCTAR  {EstadoPartidaNuevo.Nectar}/{EstadoPartidaNuevo.TotalNectar}     PUNTOS  {EstadoPartidaNuevo.Puntos}     VIDAS  {new string('♥',Mathf.Max(0,EstadoPartidaNuevo.Vidas))}";}
    public static void CrearPartida()
    {
        Time.timeScale=1;Pausado=false;pausaPanel=null;
        var c=CanvasNuevo();canvasPartida=c.transform;
        Panel(c.transform,new Vector2(.5f,1),new Vector2(0,-52),new Vector2(1330,78),new Color(.04f,.13f,.2f,.72f));
        Panel(c.transform,new Vector2(.5f,0),new Vector2(0,32),new Vector2(1330,54),new Color(.04f,.13f,.2f,.58f));
        estado=Texto(c.transform,"NÉCTAR  0     PUNTOS  0     VIDAS  ♥♥♥",28,new Vector2(.5f,1),new Vector2(0,-52),new Vector2(1100,62),TextAnchor.MiddleCenter,new Color(.98f,.9f,.64f));
        var ayuda=Texto(c.transform,"MOVER: A/D o ←/→        SALTAR: ESPACIO / W / ↑        SALTA SOBRE LOS PINCHOS",17,new Vector2(.5f,0),new Vector2(0,32),new Vector2(1250,42),TextAnchor.MiddleCenter,Color.white);
        Actualizar();
    }
    public static void CrearMenu(bool final)
    {
        Time.timeScale=1;Pausado=false;pausaPanel=null;canvasPartida=null;
        var c=CanvasNuevo();
        Panel(c.transform,new Vector2(.5f,.5f),Vector2.zero,new Vector2(840,620),new Color(.04f,.13f,.2f,.82f));
        if(!final)
        {
            Texto(c.transform,"EL PRADO DORADO",56,new Vector2(.5f,.74f),Vector2.zero,new Vector2(900,110),TextAnchor.MiddleCenter,new Color(1f,.82f,.22f));
            Texto(c.transform,"Aventura original de plataformas",25,new Vector2(.5f,.63f),Vector2.zero,new Vector2(800,60),TextAnchor.MiddleCenter,Color.white);
            Boton(c.transform,"JUGAR",new Vector2(.5f,.46f),()=>{AudioJuegoNuevo.Instancia?.Boton();UnityEngine.SceneManagement.SceneManager.LoadScene("Pradera");});
            Boton(c.transform,"SALIR",new Vector2(.5f,.34f),()=>{AudioJuegoNuevo.Instancia?.Boton();Application.Quit();});
            Texto(c.transform,"Recoge todo el néctar. Pisa a los pinchos para vencerlos.\nTres vidas. ¡Llega al portal!",21,new Vector2(.5f,.19f),Vector2.zero,new Vector2(760,100),TextAnchor.MiddleCenter,new Color(.83f,.91f,.92f));
        }
        else
        {
            bool gano=PlayerPrefs.GetInt("Victoria")==1;
            Texto(c.transform,gano?"¡PRADO A SALVO!":"FIN DE LA PARTIDA",48,new Vector2(.5f,.7f),Vector2.zero,new Vector2(900,100),TextAnchor.MiddleCenter,gano?new Color(1,.82f,.22f):new Color(1,.48f,.4f));
            Texto(c.transform,$"Puntos: {PlayerPrefs.GetInt("PuntosFinales")}     Néctar: {PlayerPrefs.GetInt("NectarFinal")}",30,new Vector2(.5f,.56f),Vector2.zero,new Vector2(800,70),TextAnchor.MiddleCenter,Color.white);
            Boton(c.transform,"JUGAR DE NUEVO",new Vector2(.5f,.4f),()=>{AudioJuegoNuevo.Instancia?.Boton();UnityEngine.SceneManagement.SceneManager.LoadScene("Pradera");});
            Boton(c.transform,"MENÚ PRINCIPAL",new Vector2(.5f,.28f),()=>{AudioJuegoNuevo.Instancia?.Boton();UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");});
        }
    }
    public static void AlternarPausa()
    {
        if(Pausado)
        {
            Time.timeScale=1;Pausado=false;
            if(pausaPanel!=null)UnityEngine.Object.Destroy(pausaPanel);pausaPanel=null;return;
        }
        if(canvasPartida==null)return;
        Pausado=true;Time.timeScale=0;
        pausaPanel=new GameObject("Panel de pausa",typeof(RectTransform));pausaPanel.transform.SetParent(canvasPartida,false);
        Panel(pausaPanel.transform,new Vector2(.5f,.5f),Vector2.zero,new Vector2(560,350),new Color(.04f,.13f,.2f,.93f));
        Texto(pausaPanel.transform,"PAUSA",46,new Vector2(.5f,.69f),Vector2.zero,new Vector2(500,84),TextAnchor.MiddleCenter,new Color(1,.82f,.22f));
        Boton(pausaPanel.transform,"CONTINUAR",new Vector2(.5f,.48f),AlternarPausa);
        Boton(pausaPanel.transform,"MENÚ PRINCIPAL",new Vector2(.5f,.27f),()=>{Time.timeScale=1;Pausado=false;UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");});
    }
    static Canvas CanvasNuevo(){var go=new GameObject("Interfaz",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));var c=go.GetComponent<Canvas>();c.renderMode=RenderMode.ScreenSpaceOverlay;go.GetComponent<CanvasScaler>().uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;go.GetComponent<CanvasScaler>().referenceResolution=new Vector2(1920,1080);if(EventSystem.current==null)new GameObject("EventSystem",typeof(EventSystem),typeof(StandaloneInputModule));return c;}
    static void Panel(Transform parent,Vector2 anchor,Vector2 pos,Vector2 size,Color color){var go=new GameObject("Panel",typeof(RectTransform),typeof(Image));go.transform.SetParent(parent,false);var r=go.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=anchor;r.anchoredPosition=pos;r.sizeDelta=size;go.GetComponent<Image>().color=color;}
    static Text Texto(Transform parent,string value,int size,Vector2 anchor,Vector2 pos,Vector2 box,TextAnchor align,Color color){var go=new GameObject("Texto",typeof(RectTransform),typeof(Text));go.transform.SetParent(parent,false);var r=go.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=anchor;r.anchoredPosition=pos;r.sizeDelta=box;var t=go.GetComponent<Text>();t.font=Font.CreateDynamicFontFromOSFont("Consolas",size);t.fontSize=size;t.fontStyle=FontStyle.Bold;t.alignment=align;t.color=color;t.text=value;t.horizontalOverflow=HorizontalWrapMode.Wrap;t.verticalOverflow=VerticalWrapMode.Overflow;return t;}
    static void Boton(Transform parent,string label,Vector2 anchor,UnityEngine.Events.UnityAction click){var go=new GameObject("Botón - "+label,typeof(RectTransform),typeof(Image),typeof(Button));go.transform.SetParent(parent,false);var r=go.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=anchor;r.sizeDelta=new Vector2(400,78);var image=go.GetComponent<Image>();image.sprite=ImagenBoton();image.type=Image.Type.Sliced;image.color=Color.white;var b=go.GetComponent<Button>();b.targetGraphic=image;b.onClick.AddListener(click);var palette=b.colors;palette.normalColor=Color.white;palette.highlightedColor=new Color(1.08f,1.08f,1.08f);palette.pressedColor=new Color(.78f,.82f,.9f);b.colors=palette;var t=Texto(go.transform,label,27,new Vector2(.5f,.5f),Vector2.zero,new Vector2(380,70),TextAnchor.MiddleCenter,new Color(.08f,.13f,.2f));t.raycastTarget=false;}
    static Sprite ImagenBoton()
    {
        if(botonSprite!=null)return botonSprite;
        const int w=128,h=48;var tex=new Texture2D(w,h,TextureFormat.RGBA32,false){filterMode=FilterMode.Point};
        for(int y=0;y<h;y++)for(int x=0;x<w;x++)
        {
            bool esquina=(x<6&&y<6&&x+y<11)||(x<6&&y>=h-6&&x+(h-1-y)<11)||(x>=w-6&&y<6&&(w-1-x)+y<11)||(x>=w-6&&y>=h-6&&(w-1-x)+(h-1-y)<11);
            if(esquina){tex.SetPixel(x,y,new Color(0,0,0,0));continue;}
            bool borde=x<3||x>=w-3||y<3||y>=h-3;
            bool brillo=y>=h-7&&y<h-4&&x>7&&x<w-8;
            bool marca=(x%23<4&&y%13<3);
            Color c=borde?new Color(.25f,.14f,.12f):brillo?new Color(1f,.84f,.36f):marca?new Color(1f,.75f,.25f):new Color(.93f,.53f,.16f);
            tex.SetPixel(x,y,c);
        }
        tex.Apply();botonSprite=Sprite.Create(tex,new Rect(0,0,w,h),new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect,new Vector4(8,8,8,8));botonSprite.name="Botón pixel art dorado";return botonSprite;
    }
}
