using UnityEngine;
public sealed class MetaNuevo : MonoBehaviour
{
    void Update(){transform.localScale=Vector3.one*(1f+Mathf.Sin(Time.time*3f)*.06f);}
}
