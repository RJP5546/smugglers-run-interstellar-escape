using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCollider : MonoBehaviour
{
    [ContextMenu("RemoveColliders")]
    void RemoveColliders()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
                var collider = this.gameObject.transform.GetChild(i).gameObject.GetComponent<Collider>();
                if (collider != null)
                {
                    DestroyImmediate(collider);
                }
            }
        }
    }
