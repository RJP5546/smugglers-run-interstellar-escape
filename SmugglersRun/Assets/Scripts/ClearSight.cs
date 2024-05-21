using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class ClearSight : MonoBehaviour
{
    public float DistanceToPlayer;
    [SerializeField] private GameObject _player;
    private Collider _playerCollider;

    private void Awake()
    {
        _playerCollider = _player.GetComponent<Collider>();
    }
    void Update()
    {
        DistanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        var heading = transform.position - _player.transform.position;

        RaycastHit[] hits;
        // you can also use CapsuleCastAll()
        // TODO: setup your layermask it improve performance and filter your hits.
        hits = Physics.RaycastAll(transform.position, -heading, DistanceToPlayer, ~2);
        Debug.DrawRay(transform.position, -heading, Color.green, .1f);
        CheckHits(hits);


    }

    void CheckHits(RaycastHit[] hits)
    {
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == _playerCollider) {continue; }
            //ignores the player

            Renderer R = hit.collider.GetComponent<Renderer>();
            if (R == null)
                continue; // no renderer attached? go to next hit
            // TODO: maybe implement here a check for GOs that should not be affected like the player

            AutoTransparent AT = R.GetComponent<AutoTransparent>();
            if (AT == null) // if no script is attached, attach one
            {
                AT = R.gameObject.AddComponent<AutoTransparent>();
            }
            AT.BeTransparent(); // get called every frame to reset the falloff
        }
    }
}