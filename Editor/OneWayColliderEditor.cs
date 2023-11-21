namespace nekomimiStudio.oneWayCollider
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(OneWayCollider))]
    internal class OneWayColliderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var that = target as OneWayCollider;
            var colliders = that.GetComponents<BoxCollider>();
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
                trigger = that.gameObject.AddComponent<BoxCollider>();
                trigger.isTrigger = true;
            }
            if (collider == null)
            {
                that.gameObject.AddComponent<BoxCollider>();
            }

            OneWayCollider.EnsureColliderSize(that);
        }

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected, typeof(OneWayCollider))]
        private static void DrawGizmo(OneWayCollider that, GizmoType gizmoType)
        {
            Gizmos.matrix = Matrix4x4.TRS(that.transform.position, that.transform.rotation, that.transform.lossyScale);

            var colliders = that.GetComponents<BoxCollider>();
            BoxCollider trigger = null;
            BoxCollider collider = null;
            foreach (var c in colliders)
            {
                if (!c.isTrigger)
                    collider = c;
                else
                    trigger = c;
            }
            Gizmos.color = new Color(.5f, .9f, .5f, .5f);
            Gizmos.DrawCube(new Vector3(collider.center.x, collider.center.y + collider.size.y / 2 + 0.005f, collider.center.z), new Vector3(collider.size.x, 0.01f, collider.size.z));
            if (gizmoType != GizmoType.Selected)
            {
                Gizmos.DrawWireCube(new Vector3(collider.center.x, collider.center.y, collider.center.z), collider.size * 0.9999f);
            }
        }
    }
}
