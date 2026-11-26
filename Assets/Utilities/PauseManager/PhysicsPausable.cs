using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Managers.PauseSystem;

public class PhysicsPausable : MonoBehaviour, IPausable
{
    [SerializeField] private Rigidbody2D rigidbodyToPause;
    private Vector3 _originalVelocity;
    private float _originalAngularVelocity;
    private RigidbodyType2D _beforePauseRigidbodyType;
    bool _wasKinematic;
    void Start()
    {
        if (!rigidbodyToPause)
        {
            rigidbodyToPause = GetComponentInChildren<Rigidbody2D>();
        }

        PauseManager.Instance.RegisterPausable(this);
    }

    void OnDestroy()
    {
        PauseManager.Instance?.UnregisterPausable(this);
    }

    public void OnPause()
    {
        if (rigidbodyToPause == null){ return;}
        _originalVelocity = rigidbodyToPause.linearVelocity;
        _originalAngularVelocity = rigidbodyToPause.angularVelocity;
        rigidbodyToPause.linearVelocity = Vector3.zero;
        rigidbodyToPause.angularVelocity = 0;
        _beforePauseRigidbodyType = rigidbodyToPause.bodyType;
        rigidbodyToPause.bodyType = RigidbodyType2D.Kinematic;
    }

    public void OnResume()
    {
        if (rigidbodyToPause == null){ return;}
        rigidbodyToPause.bodyType = _beforePauseRigidbodyType;
        rigidbodyToPause.linearVelocity = _originalVelocity;
        rigidbodyToPause.angularVelocity = _originalAngularVelocity;

    }
}