using UnityEngine;
using UnityEngine.SceneManagement;

public static class EstadoPartidaNuevo
{
    public static int Puntos { get; private set; }
    public static int Vidas { get; private set; }
    public static int Nectar { get; private set; }
    public static int TotalNectar { get; private set; }
    public static bool Terminado { get; private set; }
    public static void Iniciar(int total)
    { Puntos=0; Vidas=3; Nectar=0; TotalNectar=total; Terminado=false; }
    public static void Recoger()
    { if(Terminado)return; Nectar++; Puntos+=100; AudioJuegoNuevo.Instancia?.Abeja(); HUDNuevo.Actualizar(); }
    public static bool Herir()
    { if(Terminado)return false; Vidas--; AudioJuegoNuevo.Instancia?.Danio(); HUDNuevo.Actualizar(); if(Vidas<=0) Terminar(false); return true; }
    public static void DerrotarEnemigo()
    { if(Terminado)return; Puntos+=150; AudioJuegoNuevo.Instancia?.Pisoton(); HUDNuevo.Actualizar(); }
    public static void Ganar() { if(Terminado)return; Terminar(true); }
    static void Terminar(bool gano)
    { Terminado=true; AudioJuegoNuevo.Instancia?.Final(); PlayerPrefs.SetInt("PuntosFinales",Puntos); PlayerPrefs.SetInt("NectarFinal",Nectar); PlayerPrefs.SetInt("Victoria",gano?1:0); SceneManager.LoadScene("Final"); }
}
