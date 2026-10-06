using UnityEngine;
public sealed class MetaNuevo : MonoBehaviour
{
    Vector3 escalaBase;
    void Awake(){escalaBase=transform.localScale;}
    void Update(){transform.localScale=escalaBase*(1f+Mathf.Sin(Time.time*3f)*.035f);}
}
