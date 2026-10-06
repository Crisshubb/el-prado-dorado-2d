using UnityEngine;

public sealed class EnemigoNuevo : MonoBehaviour
{
    public float recorrido=1.5f, velocidad=1.7f; float inicio; int dir=1;
    void Start(){inicio=transform.position.x;}
    void Update(){transform.position+=Vector3.right*(velocidad*dir*Time.deltaTime);if(transform.position.x>inicio+recorrido)dir=-1;else if(transform.position.x<inicio-recorrido)dir=1;}
}
