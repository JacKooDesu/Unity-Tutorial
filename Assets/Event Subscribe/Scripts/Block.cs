using UnityEngine;

public class Block : MonoBehaviour
{
    public void Move()
    {
        transform.Translate(Vector3.up * Time.deltaTime);
    }
}