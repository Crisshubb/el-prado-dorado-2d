using System.Collections;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public sealed class JugadorNuevo : MonoBehaviour
{
    [SerializeField] float velocidad=7f, salto=13.5f;
    Rigidbody2D rb; SpriteRenderer sr; bool invulnerable, cayo; float tiempoSuelo,saltoEnCola; Vector3 inicio;
    void Awake(){rb=GetComponent<Rigidbody2D>();sr=GetComponent<SpriteRenderer>();inicio=transform.position;}
    void Update()
    {
        if(EstadoPartidaNuevo.Terminado)return;
        if(Input.GetKeyDown(KeyCode.F12))GuardarCaptura();
        float x=Input.GetAxisRaw("Horizontal"); rb.linearVelocity=new Vector2(x*velocidad,rb.linearVelocity.y);
        if(x!=0)sr.flipX=x<0;
        bool pedirSalto=Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow);
        tiempoSuelo=Mathf.Max(0,tiempoSuelo-Time.deltaTime);
        saltoEnCola=pedirSalto ? .14f : Mathf.Max(0,saltoEnCola-Time.deltaTime);
        if(saltoEnCola>0&&tiempoSuelo>0){rb.linearVelocity=new Vector2(rb.linearVelocity.x,salto);saltoEnCola=0;tiempoSuelo=0;}
        if(transform.position.y < -7f&&!cayo){cayo=true;if(EstadoPartidaNuevo.Herir())StartCoroutine(Reaparecer());}
    }
    void OnCollisionEnter2D(Collision2D c){if(c.contacts.Length>0&&c.contacts[0].normal.y>.55f)tiempoSuelo=.12f;}
    void OnCollisionStay2D(Collision2D c){if(c.contacts.Length>0&&c.contacts[0].normal.y>.55f)tiempoSuelo=.12f;}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<NectarNuevo>()!=null){other.gameObject.SetActive(false);EstadoPartidaNuevo.Recoger();return;}
        if(other.GetComponent<EnemigoNuevo>()!=null)
        {
            bool pisando=rb.linearVelocity.y<-.2f && transform.position.y>other.bounds.center.y+.25f;
            if(pisando){other.gameObject.SetActive(false);EstadoPartidaNuevo.DerrotarEnemigo();rb.linearVelocity=new Vector2(rb.linearVelocity.x,7f);tiempoSuelo=0;}
            else if(!invulnerable){if(EstadoPartidaNuevo.Herir())StartCoroutine(Parpadeo());rb.linearVelocity=new Vector2(-Mathf.Sign(other.transform.position.x-transform.position.x)*7f,7f);}
        }
        if(other.GetComponent<MetaNuevo>()!=null&&EstadoPartidaNuevo.Nectar>=EstadoPartidaNuevo.TotalNectar)EstadoPartidaNuevo.Ganar();
    }
    IEnumerator Parpadeo(){invulnerable=true;for(int i=0;i<8;i++){sr.enabled=!sr.enabled;yield return new WaitForSeconds(.09f);}sr.enabled=true;invulnerable=false;}
    IEnumerator Reaparecer(){yield return new WaitForSeconds(.55f);if(!EstadoPartidaNuevo.Terminado){rb.linearVelocity=Vector2.zero;transform.position=inicio;cayo=false;}}
    static void GuardarCaptura()
    {
        string carpeta=Application.isEditor?Path.Combine(Directory.GetParent(Application.dataPath).FullName,"Entregables"):Directory.GetParent(Application.dataPath).FullName;
        Directory.CreateDirectory(carpeta);ScreenCapture.CaptureScreenshot(Path.Combine(carpeta,"Captura_Unity_Ejecucion.png"),2);
    }
}
