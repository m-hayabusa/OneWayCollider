namespace nekomimiStudio.oneWayCollider
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    internal static class OneWayColliderScaleChecker
    {
        static OneWayColliderScaleChecker()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            Undo.postprocessModifications += PostprocessModifications;
        }

        private static IEnumerable<Object> s_target = null;
        private static readonly List<Transform> s_transforms = new List<Transform>();
        private static void OnScaleChanged()
        {
            if (s_target != null)
            {
                foreach (var t in s_target)
                {
                    OneWayCollider.EnsureColliderSize((OneWayCollider)t);
                }
            }
        }
        private static UndoPropertyModification[] PostprocessModifications(UndoPropertyModification[] modifications)
        {
            foreach (var mod in modifications)
            {
                var target = mod.currentValue.target;
                if (target.GetType() == typeof(Transform) && s_transforms.Contains(target) && mod.currentValue.propertyPath.StartsWith("m_LocalScale"))
                    OnScaleChanged();
            }
            return modifications;
        }
        private static void OnHierarchyChanged()
        {
            s_target = Resources.FindObjectsOfTypeAll(typeof(OneWayCollider))
                .Where(obj => (obj.hideFlags & HideFlags.HideInHierarchy) != HideFlags.HideInHierarchy);
            s_transforms.Clear();
            foreach (var target in s_target)
            {
                var t = ((OneWayCollider)target).transform;
                while (t != null)
                {
                    s_transforms.Add(t.parent);
                    t = t.parent;
                }
            }
        }
    }
    public class OneWayColliderEditor : Editor
    {
        [CanEditMultipleObjects]
        [CustomEditor(typeof(OneWayCollider))]
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
