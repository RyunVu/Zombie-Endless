using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponDetailsSO))]
public class WeaponDetailSsSOEditor : Editor {
    SerializedProperty weaponTypeProp;

    // Ranged
    SerializedProperty weaponCurrentAmmoProp;
    SerializedProperty hasInfiniteAmmoProp;
    SerializedProperty hasInfiniteClipCapacityProp;
    SerializedProperty weaponClipAmmoCapacityProp;
    SerializedProperty weaponAmmoCapacityProp;
    SerializedProperty weaponFireRateProp;
    SerializedProperty weaponPrechargeTimeProp;
    SerializedProperty weaponReloadTimeProp;

    // Melee
    SerializedProperty meleeDamageProp;
    SerializedProperty meleeRangeProp;
    SerializedProperty meleeArcProp;
    SerializedProperty meleeCooldownProp;

    private void OnEnable()
    {
        weaponTypeProp = serializedObject.FindProperty("weaponType");

        // Ranged
        weaponCurrentAmmoProp = serializedObject.FindProperty("weaponCurrentAmmo");
        hasInfiniteAmmoProp = serializedObject.FindProperty("hasInfiniteAmmo");
        hasInfiniteClipCapacityProp = serializedObject.FindProperty("hasInfiniteClipCapacity");
        weaponClipAmmoCapacityProp = serializedObject.FindProperty("weaponClipAmmoCapacity");
        weaponAmmoCapacityProp = serializedObject.FindProperty("weaponAmmoCapacity");
        weaponFireRateProp = serializedObject.FindProperty("weaponFireRate");
        weaponPrechargeTimeProp = serializedObject.FindProperty("weaponPrechargeTime");
        weaponReloadTimeProp = serializedObject.FindProperty("weaponReloadTime");

        // Melee
        meleeDamageProp = serializedObject.FindProperty("meleeDamage");
        meleeRangeProp = serializedObject.FindProperty("meleeRange");
        meleeArcProp = serializedObject.FindProperty("meleeArc");
        meleeCooldownProp = serializedObject.FindProperty("meleeCooldown");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(weaponTypeProp);

        WeaponType type = (WeaponType)weaponTypeProp.enumValueIndex;

        if (type == WeaponType.Ranged)
        {
            EditorGUILayout.LabelField("Ranged Weapon Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(weaponCurrentAmmoProp);
            EditorGUILayout.PropertyField(hasInfiniteAmmoProp);
            EditorGUILayout.PropertyField(hasInfiniteClipCapacityProp);
            EditorGUILayout.PropertyField(weaponClipAmmoCapacityProp);
            EditorGUILayout.PropertyField(weaponAmmoCapacityProp);
            EditorGUILayout.PropertyField(weaponFireRateProp);
            EditorGUILayout.PropertyField(weaponPrechargeTimeProp);
            EditorGUILayout.PropertyField(weaponReloadTimeProp);
        }
        else if (type == WeaponType.Melee)
        {
            EditorGUILayout.LabelField("Melee Weapon Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(meleeDamageProp);
            EditorGUILayout.PropertyField(meleeRangeProp);
            EditorGUILayout.PropertyField(meleeArcProp);
            EditorGUILayout.PropertyField(meleeCooldownProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}