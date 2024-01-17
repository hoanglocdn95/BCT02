using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDespawn : MonoBehaviour
{
    public virtual void DespawnItem()
    {
        Destroy(transform.gameObject);
    }
}
