using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponDetailsSO))]
public class WeaponDetailsSOEditor : Editor {

    // Based
    SerializedProperty weaponNameProp;
    SerializedProperty weaponSpriteProp;
    SerializedProperty weaponTypeProp;

    // Ranged
    SerializedProperty weaponShootPositionProp;
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
    SerializedProperty meleeSwingDurationProp;
    SerializedProperty meleeCooldownProp;
    SerializedProperty meleeKnockbackProp;
    SerializedProperty meleeCanPierceProp;

    private void OnEnable()
    {
        // Base
        weaponNameProp = serializedObject.FindProperty("weaponName");
        weaponSpriteProp = serializedObject.FindProperty("weaponSprite");
        weaponTypeProp = serializedObject.FindProperty("weaponType");

        // Ranged
        weaponShootPositionProp = serializedObject.FindProperty("weaponShootPosition");
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
        meleeSwingDurationProp = serializedObject.FindProperty("meleeSwingDuration");
        meleeCooldownProp = serializedObject.FindProperty("meleeCooldown");
        meleeKnockbackProp = serializedObject.FindProperty("meleeKnockback");
        meleeCanPierceProp = serializedObject.FindProperty("meleeCanPierce");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

         // --- Base ---
        EditorGUILayout.LabelField("Weapon Base Details", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(weaponNameProp);
        EditorGUILayout.PropertyField(weaponSpriteProp);
        EditorGUILayout.PropertyField(weaponTypeProp);
        
        EditorGUILayout.Space();
        
        WeaponType type = (WeaponType)weaponTypeProp.enumValueIndex;

        if (type == WeaponType.Ranged)
        {
            EditorGUILayout.LabelField("Ranged Weapon Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(weaponShootPositionProp);
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
            EditorGUILayout.PropertyField(meleeSwingDurationProp);
            EditorGUILayout.PropertyField(meleeCooldownProp);
            EditorGUILayout.PropertyField(meleeKnockbackProp);
            EditorGUILayout.PropertyField(meleeCanPierceProp);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
