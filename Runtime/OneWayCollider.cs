namespace nekomimiStudio.oneWayCollider
{
    using UdonSharp;
    using UnityEngine;
    using VRC.SDKBase;

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class OneWayCollider : UdonSharpBehaviour
    {
        public Vector3 triggerSize = new Vector3(0.5f, 0.5f, 0.5f);
        private BoxCollider _mainCollider;
        private VRCPlayerApi _localPlayer;
        private float _colliderSize;
        private float _colliderOffset;
        public void Start()
        {
            _localPlayer = Networking.LocalPlayer;
            EnsureColliderSize(this);

            _mainCollider.enabled = false;

            _colliderOffset = _mainCollider.center.y * transform.lossyScale.y;
            _colliderSize = _mainCollider.size.y * transform.lossyScale.y / 2;
        }
        protected void SetMainCollider(BoxCollider collider) { _mainCollider = collider; }
        public static void EnsureColliderSize(OneWayCollider target)
        {
            var colliders = target.GetComponents<BoxCollider>();
            BoxCollider trigger = null;
            BoxCollider collider = null;
            foreach (var c in colliders)
            {
                if (!c.isTrigger)
                    collider = c;
                else
                    trigger = c;
            }
            if (trigger == null)
            {
                Debug.LogWarning($"{target.name}: TriggerつきのBoxColliderがアタッチされていない");
                target.enabled = false;
                return;
            }
            if (collider == null)
            {
                Debug.LogWarning($"{target.name}: BoxColliderがアタッチされていない");
                target.enabled = false;
                return;
            }
            target.SetMainCollider(collider);
            if (target.transform.lossyScale.x == 0 || target.transform.lossyScale.y == 0 || target.transform.lossyScale.z == 0)
                return;

            trigger.size = new Vector3(
                collider.size.x + target.triggerSize.x / Mathf.Abs(target.transform.lossyScale.x),
                collider.size.y + target.triggerSize.y / Mathf.Abs(target.transform.lossyScale.y),
                collider.size.z + target.triggerSize.z / Mathf.Abs(target.transform.lossyScale.z)
            );
            trigger.center = new Vector3(collider.center.x, collider.center.y + trigger.size.y / 2, collider.center.z);
        }
        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            Debug.Log(Time.frameCount + " " + name + " Enter");
            CheckPlayerOnCollider(player);
        }
        public override void OnPlayerTriggerStay(VRCPlayerApi player)
        {
            CheckPlayerOnCollider(player);
        }
        private void CheckPlayerOnCollider(VRCPlayerApi player)
        {
            if (!player.isLocal || _mainCollider.enabled) return;
            if (player.GetPosition().y >= transform.position.y + _colliderOffset + _colliderSize && player.GetVelocity().y <= 0)
            {
                _mainCollider.enabled = true;
                Debug.Log(Time.frameCount + " " + name + " On");
            }
        }
        public void Update()
        {
            if (!_mainCollider.enabled) return;
            if (_localPlayer.GetPosition().y < transform.position.y + _colliderOffset - _colliderSize)
            {
                _mainCollider.enabled = false;
                Debug.Log(Time.frameCount + " " + name + " Off");
            }
        }
    }
}
