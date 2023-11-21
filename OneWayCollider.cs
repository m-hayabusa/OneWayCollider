
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class OneWayCollider : UdonSharpBehaviour
{
    private BoxCollider _mainCollider;
    private VRCPlayerApi _localPlayer;
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
        trigger.size = new Vector3(_mainCollider.size.x * 1.5f, _mainCollider.size.y * 2f, _mainCollider.size.z * 1.5f);
        trigger.center = new Vector3(_mainCollider.center.x, _mainCollider.center.y + _mainCollider.size.y, _mainCollider.center.z);
        _mainCollider.enabled = false;
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
        if (player.GetPosition().y >= transform.position.y - 0.1f)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
            _mainCollider.enabled = true;
            Debug.Log(Time.frameCount + " " + name + " On");
        }
    }
    private float _lastPos;
    public void Update()
    {
        if (name == "Test")
        {
            var pos = _localPlayer.GetPosition().y;
            if (Mathf.Round(_lastPos * 100) != Mathf.Round(pos * 100))
                Debug.Log(Time.frameCount + " " + pos);
            _lastPos = pos;
        }
        if (_mainCollider.enabled && _localPlayer.GetPosition().y < transform.position.y - 0.1)
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
            _mainCollider.enabled = false;
            Debug.Log(Time.frameCount + " " + name + " Off");
        }
    }
}
