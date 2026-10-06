using UnityEngine;

public sealed class AudioJuegoNuevo : MonoBehaviour
{
    public static AudioJuegoNuevo Instancia { get; private set; }
    AudioSource musica, efectos;
    void Awake()
    {
        if (Instancia != null) { Destroy(gameObject); return; }
        Instancia = this; DontDestroyOnLoad(gameObject);
        musica = gameObject.AddComponent<AudioSource>(); efectos = gameObject.AddComponent<AudioSource>();
        musica.clip = CrearTono("Brisa del prado", 8f, true, 0.07f); musica.loop = true; musica.volume = .35f; musica.Play();
        efectos.volume = .5f;
    }
    public static void Asegurar()
    {
        if (Instancia != null) return;
        var go = new GameObject("Audio del juego"); go.AddComponent<AudioJuegoNuevo>();
    }
    public void Abeja() => Tocar(820, .19f, 1320); public void Pisoton() => Tocar(180, .2f, 75);
    public void Danio() => Tocar(125, .3f, 68); public void Boton() => Tocar(620, .09f, 790);
    public void Final() => Tocar(660, .42f, 990);
    void Tocar(float hz, float duracion, float hzFinal)
    { efectos.PlayOneShot(CrearTono("SFX", duracion, false, .28f, hz, hzFinal)); }
    static AudioClip CrearTono(string nombre, float segundos, bool melodia, float amplitud, float hz = 440, float hzFinal = 0)
    {
        int rate = 22050, n = Mathf.CeilToInt(rate * segundos); var samples = new float[n];
        for (int i=0;i<n;i++) { float t=(float)i/rate; float wave;
            if (melodia)
            {
                int paso=(int)(t*2)%8;float[] notas={261.63f,329.63f,392f,329.63f,293.66f,349.23f,440f,349.23f};
                float f=notas[paso];float baseMusical=Mathf.Sin(2*Mathf.PI*f*t);
                float armonico=Mathf.Sin(4*Mathf.PI*f*t)*.24f;
                float acompanamiento=Mathf.Sin(2*Mathf.PI*(f*.5f)*t)*.18f;
                wave=(baseMusical+armonico+acompanamiento)*.5f;
            }
            else { float f=hzFinal>0?Mathf.Lerp(hz,hzFinal,t/segundos):hz; float golpe=Mathf.Sin(2*Mathf.PI*f*t); wave=(golpe+.16f*Mathf.Sin(4*Mathf.PI*f*t))*Mathf.Exp(-t*11f); }
            float borde=melodia?Mathf.Clamp01(Mathf.Min(t,segundos-t)*16f):1f;
            samples[i]=wave*amplitud*borde;
        }
        var clip=AudioClip.Create(nombre,n,1,rate,false); clip.SetData(samples,0); return clip;
    }
}
