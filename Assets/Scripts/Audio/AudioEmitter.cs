using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
  [SerializeField]
  private string id;
  public string Id => id;

  public Vector3 Position => gameObject.transform.position;
}