using UnityEngine;

public class Cam : MonoBehaviour
{
    private ObjectFader _fader;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (_player != null)
        {
            Vector3 dirToPlayer = _player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dirToPlayer);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                HandleHitObject(hit.collider.gameObject);
            }
        }
    }

    private void HandleHitObject(GameObject hitObject)
    {
        
        if (hitObject == _player)
        {
            if (_fader != null)
            {
                _fader.DoFade = false;
                _fader = null;
            }
        }
        else
        {
            ObjectFader newFader = hitObject.GetComponent<ObjectFader>();
            if (newFader != null && newFader != _fader)
            {
                if (_fader != null)
                {
                    _fader.DoFade = false;
                }
                _fader = newFader;
                _fader.DoFade = true;
            }
        }
    }
}
