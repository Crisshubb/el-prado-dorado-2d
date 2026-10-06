using UnityEngine;

public sealed class SpriteFormaNuevo : MonoBehaviour
{
    public enum Forma { Cuadro, Circulo, Rombo }
    public Forma forma; public Color color=Color.white; public bool cara;
    void Awake()
    {
        Dibujar();
    }
    public void Configurar(Forma nuevaForma,Color nuevoColor,bool mostrarCara=false)
    {forma=nuevaForma;color=nuevoColor;cara=mostrarCara;Dibujar();}
    void Dibujar()
    {
        const int size=64; var tex=new Texture2D(size,size,TextureFormat.RGBA32,false); tex.filterMode=FilterMode.Point;tex.wrapMode=TextureWrapMode.Repeat;
        for(int y=0;y<size;y++)for(int x=0;x<size;x++)
        {
            float dx=(x+0.5f-size/2f)/(size/2f),dy=(y+0.5f-size/2f)/(size/2f);
            bool inside=forma==Forma.Circulo?dx*dx+dy*dy<.92f:forma==Forma.Rombo?Mathf.Abs(dx)+Mathf.Abs(dy)<1.4f:true;
            var c=inside?color:new Color(0,0,0,0);
            if(forma==Forma.Cuadro&&inside)
            {
                int celdaX=x/4,celdaY=y/4;int grano=(celdaX*17+celdaY*29+celdaX*celdaY*7)%11;
                int altoHierba=56+(celdaX%5==0?3:0);
                if(y>=altoHierba)c=grano<3?Color.Lerp(color,new Color(.56f,.78f,.39f),.45f):color;
                else if(y>=altoHierba-3)c=Color.Lerp(color,new Color(.12f,.29f,.16f),.35f);
                else if(y<3)c=new Color(.25f,.16f,.13f);
                else if(grano==0||grano==6)c=new Color(.55f,.35f,.2f);
                else if(grano==2)c=new Color(.34f,.2f,.14f);
                else c=new Color(.43f,.27f,.17f);
            }
            if(cara&&inside&&y<39&&y>26&&((x>17&&x<25)||(x>39&&x<47))) c=Color.white;
            if(cara&&inside&&y<38&&y>29&&((x>20&&x<24)||(x>42&&x<46))) c=new Color(.08f,.13f,.2f);
            tex.SetPixel(x,y,c);
        }
        tex.Apply(); var sprite=Sprite.Create(tex,new Rect(0,0,size,size),new Vector2(.5f,.5f),64,0,SpriteMeshType.FullRect); sprite.name="Baldosa pixel art de tierra";
        var sr=GetComponent<SpriteRenderer>();if(sr==null)sr=gameObject.AddComponent<SpriteRenderer>();sr.sprite=sprite;sr.color=Color.white;
    }
}
