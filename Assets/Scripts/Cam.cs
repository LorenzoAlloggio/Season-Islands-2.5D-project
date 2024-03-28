using UnityEngine;

public class Cam : MonoBehaviour
{
    public float collisionRadius = 0.2f;
    public float collisionOffset = 0.1f;
    public float collisionSmoothing = 10f;

    private ObjectFader _fader;
    private GameObject _player;
    private Vector3 _desiredPosition;

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
                _desiredPosition = hit.point + hit.normal * collisionOffset;
            }
            else
            {
                _desiredPosition = _player.transform.position;
            }

            SmoothCollisionAvoidance();
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

    private void SmoothCollisionAvoidance()
    {
        transform.position = Vector3.Lerp(transform.position, _desiredPosition, Time.deltaTime * collisionSmoothing);
    }
}
