using UnityEngine;
using System;
using System.Collections;

namespace NSUltimateSeeThrough
{
    public enum EDisableMesh
    {
        None,
        Disable,
        ShowShadows
    }


    [Serializable]
    public class ChangeMaterial : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private Renderer myRenderer;
        private bool isAchievingTransparency, isTransitioning; //calledToMakeTransparentThisTurn,
        private int transparencyCallsCounter, turnCounter, numberOfMaterials;
        public float timeToTurnTransparent = 1, timeToTurnNonTransparent = 1;
        [Range(0.0f, 1)]
        public float[] alphaAmountsArray;
        public bool useInstancedMaterialsInstead;
        private Color currentTransparencyAmount;
        private float currentDuration;
        public SimpleTweenEaseMethod[] easingMethodToTransparentArray;
        public SimpleTweenEaseType[] easingTypeToTransparentArray;
        public SimpleTweenEaseMethod[] easingMethodToNonTransparentArray;
        public SimpleTweenEaseType[] easingTypeToNonTransparentArray;
        public Material[] transparentMaterials, nonTransparentMaterials;
        private bool[] useCutoff;
        private Material[] mySharedMaterials;
        [HideInInspector]
        public float[] alphaStartValues;
        private Renderer myMesh;
        [SerializeField]
        private EDisableMesh eDisableMesh;
        [SerializeField]
        private Transform[] arrayOfTransformsWithColliders;
        private Color tempColorI, tempColorJ;
        private int[] arrayOfTweenIDs;
        private Action alphaColorTweeningComplete;


        void Start()
        {
            alphaColorTweeningComplete = AlphaColorTweeningComplete;
            if (GetIfDisableMesh()) myMesh = GetComponent<Renderer>();
            numberOfMaterials = transparentMaterials.Length;
            arrayOfTweenIDs = new int[numberOfMaterials];

            if (alphaStartValues.Length != numberOfMaterials)
            {
                alphaStartValues = new float[numberOfMaterials];
                for (int i = 0; i < numberOfMaterials; i++)
                {
                    alphaStartValues[i] = nonTransparentMaterials[i].color.a;
                }
            }
        }


        void OnEnable()
        {
            UltimateSeeThroughEventManager.SetTransformObjectToTransparentOrNonTransparentEvent += CheckIfTransformMatches;
        }


        void OnDisable()
        {
            UltimateSeeThroughEventManager.SetTransformObjectToTransparentOrNonTransparentEvent -= CheckIfTransformMatches;
        }


        private bool GetIfDisableMesh()
        {
            return ((eDisableMesh == EDisableMesh.Disable) || (eDisableMesh == EDisableMesh.ShowShadows));
        }


        private void AlphaColorTweeningComplete()
        {
            if (isAchievingTransparency)
            {
                if (GetIfDisableMesh()) StartCoroutine(DisableMeshNextFrame());
            }
            else
            {
                myRenderer.sharedMaterials = nonTransparentMaterials;
            }
            isTransitioning = false;
        }


        private void CheckIfTransformMatches(Transform trans, bool transparentOrNonTransparent)
        {
            for (int i = 0; i < arrayOfTransformsWithColliders.Length; i++)
            {
                if (arrayOfTransformsWithColliders[i] == trans)
                {
                    SetToTransparentOrNonTransparent(transparentOrNonTransparent);
                    break;
                }
            }
        }


        void OnDestroy()
        {
            if (useInstancedMaterialsInstead)
            {
                for (int i = 0; i < transparentMaterials.Length; i++)
                {
                    DestroyImmediate(myRenderer.materials[i], true);
                }
            }
        }


        public void SetArrayOfTransforms(Transform[] arrayOfTransformsWithColliders)
        {
            this.arrayOfTransformsWithColliders = arrayOfTransformsWithColliders;
        }


        public void TurnTransparent()
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                currentDuration = 0;
            }
        }


        private void CreateTransparencyChangeSpeedArray()
        {
            if (easingMethodToTransparentArray == null || easingMethodToTransparentArray.Length == 0)
            {
                easingMethodToTransparentArray = new SimpleTweenEaseMethod[myRenderer.sharedMaterials.Length];
            }
            if (easingTypeToTransparentArray == null || easingTypeToTransparentArray.Length == 0)
            {
                easingTypeToTransparentArray = new SimpleTweenEaseType[myRenderer.sharedMaterials.Length];
            }
            if (easingMethodToNonTransparentArray == null || easingMethodToNonTransparentArray.Length == 0)
            {
                easingMethodToNonTransparentArray = new SimpleTweenEaseMethod[myRenderer.sharedMaterials.Length];
            }
            if (easingTypeToNonTransparentArray == null || easingTypeToNonTransparentArray.Length == 0)
            {
                easingTypeToNonTransparentArray = new SimpleTweenEaseType[myRenderer.sharedMaterials.Length];
            }
        }


        public void ChangeLegacyTreeShaderToOtherShader(int i, bool arg)
        {
            if (nonTransparentMaterials[i].shader != Shader.Find("Nature/SpeedTree"))
            {
                useCutoff[i] = !arg;
            }
        }


        private IEnumerator DisableMeshNextFrame()
        {
            yield return null;

            if (!isTransitioning)
            {
                if (eDisableMesh == EDisableMesh.Disable) myMesh.enabled = false;
                else myMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }


        public void SetTransparencyAmountsArray(float[] arg)
        {
            CreateTransparencyChangeSpeedArray();


            if (useCutoff == null) useCutoff = new bool[transparentMaterials.Length];

            if (alphaAmountsArray == null || alphaAmountsArray.Length == 0)
            {
                alphaAmountsArray = arg;

                for (int i = 0; i < alphaAmountsArray.Length; i++)
                {
                    if (nonTransparentMaterials[i].shader == Shader.Find("Nature/Tree Soft Occlusion Leaves") ||
                        nonTransparentMaterials[i].shader == Shader.Find("Nature/Tree Creator Leaves") ||
                        nonTransparentMaterials[i].shader == Shader.Find("Nature/Tree Creator Leaves Fast"))
                    {

                        alphaAmountsArray[i] = 0;
                        useCutoff[i] = true;
                    }
                }
            }
        }


        public void SetToTransparentOrNonTransparent(bool arg)
        {
            if (arg)
            {
                transparencyCallsCounter++;

                if (!isAchievingTransparency) //&&) !calledToMakeTransparentThisTurn && )
                {
                    //calledToMakeTransparentThisTurn = true;
                    //StartCoroutine(ResetCalledToMakeTransparentThisTurn());

                    if (useInstancedMaterialsInstead)
                    {
                        myRenderer.materials = transparentMaterials;
                    }
                    else
                    {
                        myRenderer.sharedMaterials = transparentMaterials;
                        mySharedMaterials = transparentMaterials;
                    }

                    isAchievingTransparency = true;

                    if (!isTransitioning)
                    {
                        isTransitioning = true;
                        currentDuration = 0;
                    }
                    else if (timeToTurnNonTransparent == timeToTurnTransparent)
                    {
                        currentDuration = timeToTurnTransparent - SimpleTween.GetCurrentDuration(arrayOfTweenIDs[0]);
                        CancelTweens();
                    }
                    else
                    {
                        currentDuration = timeToTurnTransparent - (SimpleTween.GetCurrentDuration(arrayOfTweenIDs[0]) * timeToTurnTransparent / timeToTurnNonTransparent);
                        CancelTweens();
                    }

                    if (!useInstancedMaterialsInstead)
                    {
                        for (int i = 0; i < numberOfMaterials; i++)
                        {
                            arrayOfTweenIDs[i] = SimpleTween.MaterialAlphaColor(mySharedMaterials[i], easingMethodToTransparentArray[i],
                                easingTypeToTransparentArray[i], currentDuration, timeToTurnTransparent, alphaStartValues[i],
                                (alphaAmountsArray[i] - alphaStartValues[i]), alphaColorTweeningComplete);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numberOfMaterials; i++)
                        {
                            arrayOfTweenIDs[i] = SimpleTween.MaterialAlphaColor(myRenderer.materials[i], easingMethodToTransparentArray[i],
                                easingTypeToTransparentArray[i], currentDuration, timeToTurnTransparent, alphaStartValues[i],
                                (alphaAmountsArray[i] - alphaStartValues[i]), alphaColorTweeningComplete);
                        }
                    }
                }
            }
            else
            {
                transparencyCallsCounter--;

                if (transparencyCallsCounter == 0)
                {
                    if (!isTransitioning)
                    {
                        isTransitioning = true;
                        currentDuration = 0;
                        //if (GetIfDisableMesh() && isAchievingTransparency && !myMesh.enabled) myMesh.enabled = true;
                        if (GetIfDisableMesh() && isAchievingTransparency)
                        {
                            if (eDisableMesh == EDisableMesh.Disable) myMesh.enabled = true;
                            else myMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                        }
                    }
                    else if (timeToTurnNonTransparent == timeToTurnTransparent)
                    {
                        currentDuration = timeToTurnNonTransparent - SimpleTween.GetCurrentDuration(arrayOfTweenIDs[0]);
                        CancelTweens();
                    }
                    else
                    {
                        currentDuration = timeToTurnNonTransparent - (SimpleTween.GetCurrentDuration(arrayOfTweenIDs[0]) * timeToTurnNonTransparent / timeToTurnTransparent);
                        CancelTweens();
                    }
                    if (isAchievingTransparency) isAchievingTransparency = false;


                    if (!useInstancedMaterialsInstead)
                    {
                        for (int i = 0; i < numberOfMaterials; i++)
                        {
                            arrayOfTweenIDs[i] = SimpleTween.MaterialAlphaColor(mySharedMaterials[i], easingMethodToTransparentArray[i],
                                easingTypeToTransparentArray[i], currentDuration, timeToTurnNonTransparent, alphaAmountsArray[i],
                                (alphaStartValues[i] - alphaAmountsArray[i]), alphaColorTweeningComplete);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numberOfMaterials; i++)
                        {
                            arrayOfTweenIDs[i] = SimpleTween.MaterialAlphaColor(myRenderer.materials[i], easingMethodToTransparentArray[i],
                                easingTypeToTransparentArray[i], currentDuration, timeToTurnNonTransparent, alphaAmountsArray[i],
                                (alphaStartValues[i] - alphaAmountsArray[i]), alphaColorTweeningComplete);
                        }
                    }
                }
            }
        }


        private void CancelTweens()
        {
            for (int i = 0; i < numberOfMaterials; i++)
            {
                SimpleTween.CancelTween(arrayOfTweenIDs[i]);
            }
        }


        /*private IEnumerator ResetCalledToMakeTransparentThisTurn()
        {
            yield return null;
            calledToMakeTransparentThisTurn = false;
        }*/


        public void SetTransparentMaterials(Material[] transparentMaterials)
        {
            this.transparentMaterials = transparentMaterials;
        }


        public void SetNonTransparentMaterial(Material[] nonTransparentMaterials)
        {
            this.nonTransparentMaterials = nonTransparentMaterials;
            alphaStartValues = new float[nonTransparentMaterials.Length];

            for (int i = 0; i < nonTransparentMaterials.Length; i++)
            {
                alphaStartValues[i] = nonTransparentMaterials[i].color.a;
            }
        }


        public void SetMeshRenderer(Renderer myRenderer)
        {
            this.myRenderer = myRenderer;
        }
    }
}