using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class PruebaRuntimeNuevo:MonoBehaviour
{
    public string escena;
    IEnumerator Start()
    {
        yield return null;
        bool captura=System.Array.Exists(System.Environment.GetCommandLineArgs(),a=>a=="--captura-entrega");
        if(captura)
        {
            if(escena=="Menu"){SceneManager.LoadScene("Pradera");yield break;}
            if(escena=="Pradera")
            {
                if(FindFirstObjectByType<JugadorNuevo>()==null)Fallar("no hay jugadora para la captura");
                yield return new WaitForSecondsRealtime(1.5f);
                string ruta=Path.Combine(Directory.GetParent(Application.dataPath).FullName,"Captura_Unity_Ejecucion.png");
                ScreenCapture.CaptureScreenshot(ruta,2);
                yield return new WaitForSecondsRealtime(2f);
                Debug.Log("CAPTURA_ENTREGA_OK: "+ruta);Application.Quit(0);yield break;
            }
        }
        if(escena=="Menu")
        {
            if(!BotonesConImagen(2)||!AudioIniciado())Fallar("menú, botones o audio no inicializados");
            Debug.Log("SMOKE_MENU_OK: dos botones de imagen, EventSystem y música.");
            var botones=FindObjectsByType<Button>(FindObjectsSortMode.None);
            var jugar=System.Array.Find(botones,b=>b.name.Contains("JUGAR"));
            if(jugar==null){Fallar("el botón JUGAR no tiene imagen o acción");yield break;}
            jugar.onClick.Invoke();yield break;
        }
        if(escena=="Pradera")
        {
            var nectar=FindObjectsByType<NectarNuevo>(FindObjectsSortMode.None);
            var enemigos=FindObjectsByType<EnemigoNuevo>(FindObjectsSortMode.None);
            var jugador=FindFirstObjectByType<JugadorNuevo>();
            if(nectar.Length!=5||enemigos.Length!=3||jugador==null||EstadoPartidaNuevo.Vidas!=3)Fallar($"nivel incompleto: néctar={nectar.Length}, enemigos={enemigos.Length}, jugadora={(jugador!=null)}, vidas={EstadoPartidaNuevo.Vidas}");
            var rb=jugador.GetComponent<Rigidbody2D>();var golpe=enemigos[0].GetComponent<Collider2D>();
            jugador.transform.position=new Vector3(enemigos[0].transform.position.x-.7f,enemigos[0].transform.position.y,0);rb.linearVelocity=Vector2.zero;
            jugador.SendMessage("OnTriggerEnter2D",golpe);
            if(EstadoPartidaNuevo.Vidas!=2)Fallar("la colisión lateral no descontó una vida");
            var pisada=enemigos[1];jugador.transform.position=pisada.transform.position+Vector3.up*.7f;rb.linearVelocity=Vector2.down;
            jugador.SendMessage("OnTriggerEnter2D",pisada.GetComponent<Collider2D>());
            if(EstadoPartidaNuevo.Puntos!=150)Fallar("pisar al enemigo no aplicó daño/puntos");
            foreach(var gota in nectar)jugador.SendMessage("OnTriggerEnter2D",gota.GetComponent<Collider2D>());
            if(EstadoPartidaNuevo.Puntos!=650||EstadoPartidaNuevo.Nectar!=5)Fallar("el contador no coincide con recogidas y ataque");
            if(System.Array.Exists(System.Environment.GetCommandLineArgs(),a=>a=="--smoke-test-death"))
            {EstadoPartidaNuevo.Herir();EstadoPartidaNuevo.Herir();}
            else{var meta=FindFirstObjectByType<MetaNuevo>();jugador.SendMessage("OnTriggerEnter2D",meta.GetComponent<Collider2D>());}
            yield return null;yield return null;
            if(SceneManager.GetActiveScene().name!="Final")Fallar("no cambió a la escena final");
            yield break;
        }
        if(escena=="Final")
        {
            if(!BotonesConImagen(2)||FindObjectsByType<Text>(FindObjectsSortMode.None).Length<2)Fallar("pantalla final sin interfaz o botones");
            bool perder=System.Array.Exists(System.Environment.GetCommandLineArgs(),a=>a=="--smoke-test-death");
            if(PlayerPrefs.GetInt("Victoria",0)!=(perder?0:1))Fallar("el estado de victoria/derrota no se guardó correctamente");
            Debug.Log("SMOKE_FINAL_OK: escena de resultado, botones, audio y transición completada.");
            Application.Quit(0);
        }
    }
    bool BotonesConImagen(int cantidad)
    {
        var botones=FindObjectsByType<Button>(FindObjectsSortMode.None);if(botones.Length!=cantidad)return false;
        foreach(var boton in botones){var imagen=boton.GetComponent<Image>();if(imagen==null||imagen.sprite==null)return false;}return true;
    }
    bool AudioIniciado()
    {return AudioJuegoNuevo.Instancia!=null&&AudioJuegoNuevo.Instancia.GetComponents<AudioSource>().Length>=2&&AudioJuegoNuevo.Instancia.GetComponent<AudioSource>().isPlaying;}
    void Fallar(string motivo){Debug.LogError("SMOKE_FAIL: "+motivo);Application.Quit(1);}
}
