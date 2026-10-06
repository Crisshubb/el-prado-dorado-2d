using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class HUDNuevo
{
    static Text estado;
    static Sprite botonSprite;
    public static void Actualizar(){if(estado!=null)estado.text=$"NÉCTAR  {EstadoPartidaNuevo.Nectar}/{EstadoPartidaNuevo.TotalNectar}     PUNTOS  {EstadoPartidaNuevo.Puntos}     VIDAS  {new string('♥',Mathf.Max(0,EstadoPartidaNuevo.Vidas))}";}
    public static void CrearPartida()
    {
        var c=CanvasNuevo();
        Panel(c.transform,new Vector2(.5f,1),new Vector2(0,-52),new Vector2(1330,78),new Color(.04f,.13f,.2f,.72f));
        Panel(c.transform,new Vector2(.5f,0),new Vector2(0,32),new Vector2(1330,54),new Color(.04f,.13f,.2f,.58f));
        estado=Texto(c.transform,"NÉCTAR  0     PUNTOS  0     VIDAS  ♥♥♥",28,new Vector2(.5f,1),new Vector2(0,-52),new Vector2(1100,62),TextAnchor.MiddleCenter,new Color(.98f,.9f,.64f));
        var ayuda=Texto(c.transform,"MOVER: A/D o ←/→        SALTAR: ESPACIO / W / ↑        SALTA SOBRE LOS PINCHOS",17,new Vector2(.5f,0),new Vector2(0,32),new Vector2(1250,42),TextAnchor.MiddleCenter,Color.white);
        Actualizar();
    }
    public static void CrearMenu(bool final)
    {
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
    static Canvas CanvasNuevo(){var go=new GameObject("Interfaz",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));var c=go.GetComponent<Canvas>();c.renderMode=RenderMode.ScreenSpaceOverlay;go.GetComponent<CanvasScaler>().uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;go.GetComponent<CanvasScaler>().referenceResolution=new Vector2(1920,1080);if(EventSystem.current==null)new GameObject("EventSystem",typeof(EventSystem),typeof(StandaloneInputModule));return c;}
    static void Panel(Transform parent,Vector2 anchor,Vector2 pos,Vector2 size,Color color){var go=new GameObject("Panel",typeof(RectTransform),typeof(Image));go.transform.SetParent(parent,false);var r=go.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=anchor;r.anchoredPosition=pos;r.sizeDelta=size;go.GetComponent<Image>().color=color;}
    static Text Texto(Transform parent,string value,int size,Vector2 anchor,Vector2 pos,Vector2 box,TextAnchor align,Color color){var go=new GameObject("Texto",typeof(RectTransform),typeof(Text));go.transform.SetParent(parent,false);var r=go.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=anchor;r.anchoredPosition=pos;r.sizeDelta=box;var t=go.GetComponent<Text>();t.font=Font.CreateDynamicFontFromOSFont("Arial",size);t.fontSize=size;t.fontStyle=FontStyle.Bold;t.alignment=align;t.color=color;t.text=value;t.horizontalOverflow=HorizontalWrapMode.Wrap;t.verticalOverflow=VerticalWrapMode.Overflow;return t;}
    static void Boton(Transform parent,string label,Vector2 anchor,UnityEngine.Events.UnityAction click){var go=new GameObject("Botón - "+label,typeof(RectTransform),typeof(Image),typeof(Button));go.transform.SetParent(parent,false);var r=go.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=anchor;r.sizeDelta=new Vector2(400,78);var image=go.GetComponent<Image>();image.sprite=ImagenBoton();image.type=Image.Type.Sliced;image.color=Color.white;var b=go.GetComponent<Button>();b.targetGraphic=image;b.onClick.AddListener(click);var palette=b.colors;palette.normalColor=Color.white;palette.highlightedColor=new Color(1.08f,1.08f,1.08f);palette.pressedColor=new Color(.78f,.82f,.9f);b.colors=palette;var t=Texto(go.transform,label,27,new Vector2(.5f,.5f),Vector2.zero,new Vector2(380,70),TextAnchor.MiddleCenter,new Color(.08f,.13f,.2f));t.raycastTarget=false;}
    static Sprite ImagenBoton(){if(botonSprite!=null)return botonSprite;const int w=128,h=48;var tex=new Texture2D(w,h,TextureFormat.RGBA32,false){filterMode=FilterMode.Bilinear};for(int y=0;y<h;y++)for(int x=0;x<w;x++){int cx=x<10?10:x>=w-10?w-11:x,cy=y<10?10:y>=h-10?h-11:y;bool dentro=(x-cx)*(x-cx)+(y-cy)*(y-cy)<=100;float brillo=.88f+.12f*y/(h-1f);tex.SetPixel(x,y,dentro?new Color(.98f*brillo,.67f*brillo,.17f*brillo):new Color(0,0,0,0));}tex.Apply();botonSprite=Sprite.Create(tex,new Rect(0,0,w,h),new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect,new Vector4(10,10,10,10));botonSprite.name="Botón ilustrado ámbar";return botonSprite;}
}
