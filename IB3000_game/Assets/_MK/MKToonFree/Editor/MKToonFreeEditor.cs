using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEditor.Utils;
using UnityEditorInternal;

#if UNITY_EDITOR
namespace MK.Toon
{
    public class MKToonFreeEditor : ShaderGUI
    {
        //hdr config
        private ColorPickerHDRConfig colorPickerHDRConfig = new ColorPickerHDRConfig(0f, 99f, 1 / 99f, 3f);

        //Main
        private MaterialProperty mainColor = null;
        private MaterialProperty mainTex = null;

        //Normalmap
        private MaterialProperty bumpMap = null;

        //Light
        private MaterialProperty lTreshold = null;

        //Render
        private MaterialProperty lightSmoothness = null;
        private MaterialProperty rimSmoothness = null;

        //Custom shadow
        private MaterialProperty shadowColor = null;
        private MaterialProperty highlightColor = null;
        private MaterialProperty shadowIntensity = null;

        //Outline
        private MaterialProperty outlineColor = null;
        private MaterialProperty outlineSize = null;

        //Rim
        private MaterialProperty rimColor = null;
        private MaterialProperty rimSize = null;
        private MaterialProperty rimIntensity = null;

        //Specular
        private MaterialProperty specularIntensity = null;
        private MaterialProperty shininess = null;
        private MaterialProperty specularColor = null;

        //Emission
        private MaterialProperty emissionColor = null;

        public void FindProperties(MaterialProperty[] props, Material mat)
        {
            //Main
            mainColor = FindProperty("_Color", props);
            mainTex = FindProperty("_MainTex", props);

            //Normalmap
            bumpMap = FindProperty("_BumpMap", props);

            //Light
            lTreshold = FindProperty("_LightThreshold", props);

            //Render
            lightSmoothness = FindProperty("_LightSmoothness", props);
            rimSmoothness = FindProperty("_RimSmoothness", props);

            //Custom shadow
            shadowIntensity = FindProperty("_ShadowIntensity", props);
            shadowColor = FindProperty("_ShadowColor", props);
            highlightColor = FindProperty("_HighlightColor", props);

            //Outline
            outlineColor = FindProperty("_OutlineColor", props);
            outlineSize = FindProperty("_OutlineSize", props);

            //Rim
            rimColor = FindProperty("_RimColor", props);
            rimSize = FindProperty("_RimSize", props);
            rimIntensity = FindProperty("_RimIntensity", props);

            //Specular
            shininess = FindProperty("_Shininess", props);
            specularColor = FindProperty("_SpecColor", props);
            specularIntensity = FindProperty("_SpecularIntensity", props);

            //Emission
            emissionColor = FindProperty("_EmissionColor", props);
        }

        //Colorfield
        private void ColorProperty(MaterialProperty prop, bool showAlpha, bool hdrEnabled, GUIContent label)
        {
            EditorGUI.showMixedValue = prop.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            Color c = EditorGUILayout.ColorField(label, prop.colorValue, false, showAlpha, hdrEnabled, colorPickerHDRConfig);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                prop.colorValue = c;
        }

        //Setup GI emission
        private void SetGIFlags()
        {
            foreach (Material obj in emissionColor.targets)
            {
                bool emissive = true;
                if (obj.GetColor("_EmissionColor") == Color.black)
                {
                    emissive = false;
                }
                MaterialGlobalIlluminationFlags flags = obj.globalIlluminationFlags;
                if ((flags & (MaterialGlobalIlluminationFlags.BakedEmissive | MaterialGlobalIlluminationFlags.RealtimeEmissive)) != 0)
                {
                    flags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                    if (!emissive)
                        flags |= MaterialGlobalIlluminationFlags.EmissiveIsBlack;

                    obj.globalIlluminationFlags = flags;
                }
            }
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }
            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            MaterialProperty[] properties = MaterialEditor.GetMaterialProperties(new Material[] { material });
            FindProperties(properties, material);

            SetGIFlags();
        }

        override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material targetMat = materialEditor.target as Material;
            //get properties
            FindProperties(properties, targetMat);

            //main settings
            EditorGUILayout.LabelField("Main", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            ColorProperty(mainColor, false, false, new GUIContent(mainColor.displayName));
            materialEditor.TexturePropertySingleLine(new GUIContent(mainTex.displayName), mainTex);
            materialEditor.TexturePropertySingleLine(new GUIContent(bumpMap.displayName), bumpMap);
            ColorProperty(emissionColor, false, true, new GUIContent(emissionColor.displayName));
            if (emissionColor.colorValue != Color.black)
                materialEditor.LightmapEmissionProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
            Divider();

            //light settings
            EditorGUILayout.LabelField("Light", EditorStyles.boldLabel);
            materialEditor.ShaderProperty(lTreshold, lTreshold.displayName);

            //custom shadow settings
                Divider();
            EditorGUILayout.LabelField("Shadow", EditorStyles.boldLabel);
            ColorProperty(highlightColor, false, false, new GUIContent(highlightColor.displayName));
            ColorProperty(shadowColor, false, false, new GUIContent(shadowColor.displayName));
            materialEditor.ShaderProperty(shadowIntensity, shadowIntensity.displayName);

            Divider();
            //render settings
            EditorGUILayout.LabelField("Render", EditorStyles.boldLabel);
            materialEditor.ShaderProperty(lightSmoothness, lightSmoothness.displayName);
            materialEditor.ShaderProperty(rimSmoothness, rimSmoothness.displayName);

            //specular settings
            Divider();
            EditorGUILayout.LabelField("Specular", EditorStyles.boldLabel);
            ColorProperty(specularColor, false, false, new GUIContent(specularColor.displayName));
            materialEditor.ShaderProperty(shininess, shininess.displayName);
            materialEditor.ShaderProperty(specularIntensity, specularIntensity.displayName);

            //rim settings
            Divider();
            EditorGUILayout.LabelField("Rim", EditorStyles.boldLabel);
            ColorProperty(rimColor, false, false, new GUIContent(rimColor.displayName));
            materialEditor.ShaderProperty(rimSize, rimSize.displayName);
            materialEditor.ShaderProperty(rimIntensity, rimIntensity.displayName);


            //set outline
            Divider();
            EditorGUILayout.LabelField("Outline", EditorStyles.boldLabel);
            ColorProperty(outlineColor, false, false, new GUIContent(outlineColor.displayName));
            materialEditor.ShaderProperty(outlineSize, outlineSize.displayName);

            Divider();
            //uv setup
            EditorGUILayout.LabelField("Uv", EditorStyles.boldLabel);
            materialEditor.TextureScaleOffsetProperty(mainTex);

            ///Other
            if (EditorGUI.EndChangeCheck())
            {
                SetGIFlags();
            }
        }

        private void Divider()
        {
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        }
    }
}
#endif