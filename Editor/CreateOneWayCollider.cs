namespace nekomimiStudio.oneWayCollider
{
    using UnityEngine;
    using UnityEditor;

    public class CreateOneWayCollider
    {
        [MenuItem("GameObject/nekomimiStudio/OneWayCollider", false, 10)]
        public static void Create(MenuCommand menu)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Packages/studio.nekomimi.onewaycollider/Runtime/OneWayCollider.prefab");
            GameObject res = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            GameObjectUtility.SetParentAndAlign(res, (GameObject)menu.context);
            Undo.RegisterCreatedObjectUndo(res, "OneWayCollider");
            Selection.activeObject = res;
        }
    }
}
