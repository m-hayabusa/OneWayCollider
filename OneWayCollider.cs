
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class OneWayCollider : UdonSharpBehaviour
{
    private BoxCollider _mainCollider;
    private VRCPlayerApi _localPlayer;
    private float _colliderSize;
    private float _colliderOffset;
    public void Start()
    {
        _localPlayer = Networking.LocalPlayer;
        var colliders = GetComponents<BoxCollider>();
        BoxCollider trigger = null;
        foreach (var c in colliders)
        {
            if (!c.isTrigger)
                _mainCollider = c;
            else
                trigger = c;
        }
        trigger.size = new Vector3(_mainCollider.size.x * 1.5f, _mainCollider.size.y * 4f, _mainCollider.size.z * 1.5f);
        trigger.center = new Vector3(_mainCollider.center.x, _mainCollider.center.y + trigger.size.y / 2 - 0.1f, _mainCollider.center.z);
        _mainCollider.enabled = false;
        _colliderOffset = _mainCollider.center.y * transform.localScale.y;
        _colliderSize = _mainCollider.size.y * transform.localScale.y / 2;
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        Debug.Log(Time.frameCount + " " + name + " Enter");
        GetComponent<MeshRenderer>().material.color = Color.yellow;
        Check(player);
    }
    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        Check(player);
    }
    private void Check(VRCPlayerApi player)
    {
        if (!player.isLocal || _mainCollider.enabled) return;
        if (player.GetPosition().y >= transform.position.y + _colliderOffset + _colliderSize && _localPlayer.GetVelocity().y <= 0)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
            _mainCollider.enabled = true;
            Debug.Log(Time.frameCount + " " + name + " On");
        }
    }
    public void Update()
    {
        if (!_mainCollider.enabled) return;
        if (_localPlayer.GetPosition().y < transform.position.y + _colliderOffset - _colliderSize)
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
            _mainCollider.enabled = false;
            Debug.Log(Time.frameCount + " " + name + " Off");
        }
    }
}
