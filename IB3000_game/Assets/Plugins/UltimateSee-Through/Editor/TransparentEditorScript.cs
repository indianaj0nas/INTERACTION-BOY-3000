using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace NSUltimateSeeThrough
{
    [InitializeOnLoad]
    [CustomEditor(typeof(ChangeMaterial))]
    public class TransparentEditorScript : Editor
    {
        private static string nameOfOldMaterial;
        private static Color previousColor, tempColor;
        private static GameObject[] gos;
        private static Material material, material2;
        private static Material[] nonTransparentMaterials, transparentMaterials, tempMaterials;
        private static string[] findTransparentVersionsOfMaterial;
        private static ChangeMaterial[] tempArrayWithChangeMaterialScripts, 
            changeMaterialInChildren;
        private static ChangeMaterial tempChangeMaterialScript, checkIfChangedChangeMaterial;
        private static GameObject playerObject;
        private static int counter, tempInt;
        private static bool isCurrentMeshATreeBark, previousValueOfTestingTransparency;
        private static float[] transparencyAmountsArray;
        private static bool[] isUsingSpeedTreeShaderArray;
        private static Object tempObject;
        //private static List<ChangeMaterial> parentsWhoNeedsToUpdateLinkedTransparency;
        private static SavedLists savedArraysScript;
        private static GameObject tempGameObject, previousGameObject;
        private static Collider[] childColliders2;

        private static List<GameObject> listOfCopiedValuesGameObjects;
        private static List<float[]> listOfAlphaValueArrays;
        private static List<float> listOfNumberOfUpdatesToTurnTransparent,
            listOfNumberOfUpdatesToTurnNonTransparent;
        private static List<SimpleTweenEaseMethod[]> listOfEasingMethodToTransparentArray, listOfEasingMethodToNonTransparentArray;
        private static List<SimpleTweenEaseType[]> listOfEasingTypeToTransparentArray, listOfEasingTypeToNonTransparentArray;
        private static float[] tempListOfAlphaValueArrays;
        private static float tempListOfNumberOfUpdatesToTurnTransparent,
            tempListOfNumberOfUpdatesToTurnNonTransparent;
        private static SimpleTweenEaseMethod[] tempEasingMethodArrayToTransparent, tempEasingMethodArrayToNonTransparent;
        private static SimpleTweenEaseType[] tempEasingTypeArrayToTransparent, tempEasingTypeArrayToNonTransparent;


        [MenuItem("Window/Ultimate See-Through/Regular Transparency (standard) %g")]
        [CanEditMultipleObjects]
        static void CreateTransparentMaterial()
        {
            CreateNeededScriptsForCamera();

            CreateTransparentMaterialAndScripts(false);
        }


        TransparentEditorScript()
        {
            SavedLists.UpdateListsEvent += UpdateSavedLists;
        }


        private void UpdateSavedLists()
        {
            if (!EditorApplication.isPlaying)
            {
                tempGameObject = Selection.activeGameObject;
                checkIfChangedChangeMaterial = tempGameObject.GetComponent<ChangeMaterial>();

                SetTempValues();

                if (savedArraysScript == null)
                {
                    savedArraysScript = Camera.main.GetComponent<SavedLists>();
                }
                if (listOfAlphaValueArrays == null || listOfCopiedValuesGameObjects == null ||
                    listOfNumberOfUpdatesToTurnTransparent == null || listOfNumberOfUpdatesToTurnNonTransparent == null ||
                    listOfEasingMethodToTransparentArray == null || listOfEasingTypeToTransparentArray == null ||
                    listOfEasingMethodToNonTransparentArray == null || listOfEasingTypeToNonTransparentArray == null)
                {
                    listOfAlphaValueArrays = new List<float[]>();
                    listOfCopiedValuesGameObjects = new List<GameObject>();
                    listOfNumberOfUpdatesToTurnTransparent = new List<float>();
                    listOfNumberOfUpdatesToTurnNonTransparent = new List<float>();
                    listOfEasingMethodToTransparentArray = new List<SimpleTweenEaseMethod[]>();
                    listOfEasingTypeToTransparentArray = new List<SimpleTweenEaseType[]>();
                    listOfEasingMethodToNonTransparentArray = new List<SimpleTweenEaseMethod[]>();
                    listOfEasingTypeToNonTransparentArray = new List<SimpleTweenEaseType[]>();

                    FetchSavedArrays();

                    return;
                }
                SaveToListScript();
            }
            else
            {
                if (listOfAlphaValueArrays == null || listOfCopiedValuesGameObjects == null || listOfNumberOfUpdatesToTurnTransparent == null ||
                    listOfNumberOfUpdatesToTurnNonTransparent == null || listOfEasingMethodToTransparentArray == null ||
                    listOfEasingTypeToTransparentArray == null || listOfEasingMethodToNonTransparentArray == null ||
                    listOfEasingTypeToNonTransparentArray == null)
                {
                    listOfAlphaValueArrays = new List<float[]>();
                    listOfCopiedValuesGameObjects = new List<GameObject>();
                    listOfNumberOfUpdatesToTurnTransparent = new List<float>();
                    listOfNumberOfUpdatesToTurnNonTransparent = new List<float>();
                    listOfEasingMethodToTransparentArray = new List<SimpleTweenEaseMethod[]>();
                    listOfEasingMethodToNonTransparentArray = new List<SimpleTweenEaseMethod[]>();
                    listOfEasingTypeToTransparentArray = new List<SimpleTweenEaseType[]>();
                    listOfEasingTypeToNonTransparentArray = new List<SimpleTweenEaseType[]>();

                    if (savedArraysScript == null)
                    {
                        savedArraysScript = Camera.main.GetComponent<SavedLists>();
                    }
                    FetchSavedArrays();
                }
            }
        }


        private static void FetchSavedArrays()
        {
            if (!savedArraysScript.AreListsEmpty())
            {
                listOfCopiedValuesGameObjects = savedArraysScript.listOfCopiedValuesGameObjects;
                listOfAlphaValueArrays = savedArraysScript.GetBothArrayLists(out listOfEasingMethodToTransparentArray, out listOfEasingTypeToTransparentArray,
                    out listOfEasingMethodToNonTransparentArray, out listOfEasingTypeToNonTransparentArray);
                listOfNumberOfUpdatesToTurnTransparent = savedArraysScript.listOfNumberOfUpdatesToTurnTransparent;
                listOfNumberOfUpdatesToTurnNonTransparent = savedArraysScript.listOfNumberOfUpdatesToTurnNonTransparent;
            }
        }


        void CopyValues()
        {
            if (savedArraysScript == null)
            {
                savedArraysScript = Camera.main.GetComponent<SavedLists>();
            }

            if (listOfAlphaValueArrays == null || listOfCopiedValuesGameObjects == null || listOfNumberOfUpdatesToTurnTransparent == null ||
                listOfNumberOfUpdatesToTurnNonTransparent == null || listOfEasingMethodToTransparentArray == null || listOfEasingTypeToTransparentArray == null ||
                listOfEasingMethodToNonTransparentArray == null || listOfEasingTypeToNonTransparentArray == null)
            {
                /*Debug.Log(listOfAlphaValueArrays);
                Debug.Log(listOfCopiedValuesGameObjects);
                Debug.Log(listOfNumberOfUpdatesToTurnTransparent);
                Debug.Log(listOfNumberOfUpdatesToTurnNonTransparent);
                Debug.Log(listOfTransparencyChangeSpeedArrays);*/
                listOfAlphaValueArrays = new List<float[]>();
                listOfCopiedValuesGameObjects = new List<GameObject>();
                listOfNumberOfUpdatesToTurnTransparent = new List<float>();
                listOfNumberOfUpdatesToTurnNonTransparent = new List<float>();
                listOfEasingMethodToTransparentArray = new List<SimpleTweenEaseMethod[]>();
                listOfEasingTypeToTransparentArray = new List<SimpleTweenEaseType[]>();
                listOfEasingMethodToNonTransparentArray = new List<SimpleTweenEaseMethod[]>();
                listOfEasingTypeToNonTransparentArray = new List<SimpleTweenEaseType[]>();

                if (savedArraysScript == null)
                {
                    savedArraysScript = Camera.main.GetComponent<SavedLists>();
                }
                if (!savedArraysScript.AreListsEmpty())
                {
                    listOfCopiedValuesGameObjects = savedArraysScript.listOfCopiedValuesGameObjects;
                    listOfAlphaValueArrays = savedArraysScript.GetBothArrayLists(out listOfEasingMethodToTransparentArray, out listOfEasingTypeToTransparentArray,
                        out listOfEasingMethodToNonTransparentArray, out listOfEasingTypeToNonTransparentArray);
                    listOfNumberOfUpdatesToTurnTransparent = savedArraysScript.listOfNumberOfUpdatesToTurnTransparent;
                    listOfNumberOfUpdatesToTurnNonTransparent = savedArraysScript.listOfNumberOfUpdatesToTurnNonTransparent;
                }
            }

            tempChangeMaterialScript = Selection.activeGameObject.GetComponent<ChangeMaterial>();

            if (!listOfCopiedValuesGameObjects.Contains(tempChangeMaterialScript.gameObject))
            {
                listOfCopiedValuesGameObjects.Add(tempChangeMaterialScript.gameObject);
                listOfAlphaValueArrays.Add(new float[tempChangeMaterialScript.alphaAmountsArray.Length]);
                tempChangeMaterialScript.alphaAmountsArray.CopyTo(listOfAlphaValueArrays[listOfAlphaValueArrays.Count - 1], 0);
                listOfNumberOfUpdatesToTurnTransparent.Add(tempChangeMaterialScript.timeToTurnTransparent);
                listOfNumberOfUpdatesToTurnNonTransparent.Add(tempChangeMaterialScript.timeToTurnNonTransparent);
                listOfEasingMethodToTransparentArray.Add(new SimpleTweenEaseMethod[tempChangeMaterialScript.alphaAmountsArray.Length]);
                listOfEasingTypeToTransparentArray.Add(new SimpleTweenEaseType[tempChangeMaterialScript.alphaAmountsArray.Length]);
                listOfEasingMethodToNonTransparentArray.Add(new SimpleTweenEaseMethod[tempChangeMaterialScript.alphaAmountsArray.Length]);
                listOfEasingTypeToNonTransparentArray.Add(new SimpleTweenEaseType[tempChangeMaterialScript.alphaAmountsArray.Length]);
                tempChangeMaterialScript.easingMethodToTransparentArray.CopyTo(listOfEasingMethodToTransparentArray[listOfEasingMethodToTransparentArray.Count - 1], 0);
                tempChangeMaterialScript.easingTypeToTransparentArray.CopyTo(listOfEasingTypeToTransparentArray[listOfEasingTypeToTransparentArray.Count - 1], 0);
                tempChangeMaterialScript.easingMethodToNonTransparentArray.CopyTo(listOfEasingMethodToNonTransparentArray[listOfEasingMethodToNonTransparentArray.Count - 1], 0);
                tempChangeMaterialScript.easingTypeToNonTransparentArray.CopyTo(listOfEasingTypeToNonTransparentArray[listOfEasingTypeToNonTransparentArray.Count - 1], 0);
            }
            else
            {
                tempInt = listOfCopiedValuesGameObjects.IndexOf(tempChangeMaterialScript.gameObject);
                tempChangeMaterialScript.alphaAmountsArray.CopyTo(listOfAlphaValueArrays[tempInt], 0);
                listOfNumberOfUpdatesToTurnTransparent[tempInt] = tempChangeMaterialScript.timeToTurnTransparent;
                listOfNumberOfUpdatesToTurnNonTransparent[tempInt] = tempChangeMaterialScript.timeToTurnNonTransparent;

                tempChangeMaterialScript.easingMethodToTransparentArray.CopyTo(listOfEasingMethodToTransparentArray[tempInt], 0);
                tempChangeMaterialScript.easingTypeToTransparentArray.CopyTo(listOfEasingTypeToTransparentArray[tempInt], 0);
                tempChangeMaterialScript.easingMethodToNonTransparentArray.CopyTo(listOfEasingMethodToNonTransparentArray[tempInt], 0);
                tempChangeMaterialScript.easingTypeToNonTransparentArray.CopyTo(listOfEasingTypeToNonTransparentArray[tempInt], 0);
            }
            if (!EditorApplication.isPlaying)
            {
                SaveToListScript();
            }
        }


        private void SaveToListScript()
        {
            savedArraysScript.SaveLists(listOfCopiedValuesGameObjects, listOfAlphaValueArrays, listOfNumberOfUpdatesToTurnTransparent,
                listOfEasingMethodToTransparentArray, listOfNumberOfUpdatesToTurnNonTransparent, listOfEasingTypeToTransparentArray,
                listOfEasingMethodToNonTransparentArray, listOfEasingTypeToNonTransparentArray);
        }


        void PasteValues()
        {
            if (Application.isPlaying)
            {
                Debug.Log("Exit play mode first.");
                return;
            }
            if (listOfCopiedValuesGameObjects == null || listOfCopiedValuesGameObjects.Count == 0 ||
                listOfAlphaValueArrays == null || listOfAlphaValueArrays.Count == 0 ||
                listOfNumberOfUpdatesToTurnTransparent == null || listOfNumberOfUpdatesToTurnTransparent.Count == 0 ||
                listOfNumberOfUpdatesToTurnNonTransparent == null || listOfNumberOfUpdatesToTurnNonTransparent.Count == 0 ||
                listOfEasingMethodToTransparentArray == null || listOfEasingMethodToTransparentArray.Count == 0 ||
                listOfEasingTypeToTransparentArray == null || listOfEasingTypeToTransparentArray.Count == 0 ||
                listOfEasingMethodToNonTransparentArray == null || listOfEasingMethodToNonTransparentArray.Count == 0 ||
                listOfEasingTypeToNonTransparentArray == null || listOfEasingTypeToNonTransparentArray.Count == 0)
            {
                if (savedArraysScript == null)
                {
                    savedArraysScript = Camera.main.GetComponent<SavedLists>();
                }
                if (savedArraysScript.AreListsEmpty())
                {
                    Debug.Log("No Values To Paste");
                    return;
                }
                else
                {
                    listOfCopiedValuesGameObjects = savedArraysScript.listOfCopiedValuesGameObjects;
                    listOfAlphaValueArrays = savedArraysScript.GetBothArrayLists(out listOfEasingMethodToTransparentArray, out listOfEasingTypeToTransparentArray,
                        out listOfEasingMethodToNonTransparentArray, out listOfEasingTypeToNonTransparentArray);
                    listOfNumberOfUpdatesToTurnTransparent = savedArraysScript.listOfNumberOfUpdatesToTurnTransparent;
                    listOfNumberOfUpdatesToTurnNonTransparent = savedArraysScript.listOfNumberOfUpdatesToTurnNonTransparent;
                }

            }
            GameObject[] allObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));

            for (int i = 0; i < listOfCopiedValuesGameObjects.Count; i++)
            {
                if (listOfCopiedValuesGameObjects[i] == null)
                {
                    Debug.Log("Failed to copy one of the values because the target was destroyed");
                    continue;
                }
                tempChangeMaterialScript = listOfCopiedValuesGameObjects[i].GetComponent<ChangeMaterial>();
                listOfAlphaValueArrays[i].CopyTo(tempChangeMaterialScript.alphaAmountsArray, 0);
                tempChangeMaterialScript.timeToTurnTransparent = listOfNumberOfUpdatesToTurnTransparent[i];
                tempChangeMaterialScript.timeToTurnNonTransparent = listOfNumberOfUpdatesToTurnNonTransparent[i];
                listOfEasingMethodToTransparentArray[i].CopyTo(tempChangeMaterialScript.easingMethodToTransparentArray, 0);
                listOfEasingTypeToTransparentArray[i].CopyTo(tempChangeMaterialScript.easingTypeToTransparentArray, 0);
                listOfEasingMethodToNonTransparentArray[i].CopyTo(tempChangeMaterialScript.easingMethodToNonTransparentArray, 0);
                listOfEasingTypeToNonTransparentArray[i].CopyTo(tempChangeMaterialScript.easingTypeToNonTransparentArray, 0);

                if (Selection.activeGameObject == listOfCopiedValuesGameObjects[i])
                {
                    for (int j = 0; j < tempChangeMaterialScript.alphaAmountsArray.Length; j++)
                    {
                        tempListOfAlphaValueArrays[j] = checkIfChangedChangeMaterial.alphaAmountsArray[j];
                        tempEasingMethodArrayToTransparent[j] = checkIfChangedChangeMaterial.easingMethodToTransparentArray[j];
                        tempEasingTypeArrayToTransparent[j] = checkIfChangedChangeMaterial.easingTypeToTransparentArray[j];
                        tempEasingMethodArrayToNonTransparent[j] = checkIfChangedChangeMaterial.easingMethodToNonTransparentArray[j];
                        tempEasingTypeArrayToNonTransparent[j] = checkIfChangedChangeMaterial.easingTypeToNonTransparentArray[j];
                    }
                    tempListOfNumberOfUpdatesToTurnTransparent = checkIfChangedChangeMaterial.timeToTurnTransparent;
                    tempListOfNumberOfUpdatesToTurnNonTransparent = checkIfChangedChangeMaterial.timeToTurnNonTransparent;
                }

                if (PrefabUtility.GetPrefabParent(tempChangeMaterialScript.gameObject) == null)
                {
                    continue;
                }
                else if (PrefabUtility.GetPrefabType(listOfCopiedValuesGameObjects[i]).Equals(PrefabType.ModelPrefab) || PrefabUtility.GetPrefabType(listOfCopiedValuesGameObjects[i]).Equals(PrefabType.ModelPrefabInstance))
                {
                    Debug.Log("The following prefab was not updated since it is a model prefab: " + PrefabUtility.GetPrefabParent(listOfCopiedValuesGameObjects[i]).name);
                }
                else
                {
                    tempObject = PrefabUtility.GetPrefabParent(listOfCopiedValuesGameObjects[i]);
                    PrefabUtility.ReplacePrefab(listOfCopiedValuesGameObjects[i], tempObject, ReplacePrefabOptions.ConnectToPrefab);

                    foreach (GameObject gO in allObjects)
                    {
                        if (PrefabUtility.GetPrefabParent(gO) != null && PrefabUtility.GetPrefabParent(gO) == tempObject)
                        {
                            PrefabUtility.RevertPrefabInstance(gO);
                        }
                    }
                }
            }
        }


        static void CreateNeededScriptsForCamera()
        {
            if (Camera.main != null)
            {
                if (Camera.main.gameObject.GetComponent<SimpleTween>() == null)
                    Camera.main.gameObject.AddComponent<SimpleTween>();
                if (Camera.main.gameObject.GetComponent<UltimateSeeThroughEventManager>() == null)
                    Camera.main.gameObject.AddComponent<UltimateSeeThroughEventManager>();
                if (Camera.main.gameObject.GetComponent<CameraTransparentScript>() == null)
                {
                    Camera.main.gameObject.AddComponent<CameraTransparentScript>();
                    Camera.main.gameObject.GetComponent<CameraTransparentScript>().SetCameraTransform(Camera.main.transform);
                    Camera.main.gameObject.GetComponent<CameraTransparentScript>().SetUltimateSeeThroughEventManager(Camera.main.gameObject.GetComponent<UltimateSeeThroughEventManager>());

                    playerObject = GameObject.Find("Player");

                    if (playerObject != null)
                    {
                        Camera.main.gameObject.GetComponent<CameraTransparentScript>().ReceivePlayerTransform(playerObject.transform);
                    }
                }
                if (Camera.main.gameObject.GetComponent<SavedLists>() == null)
                {
                    Camera.main.gameObject.AddComponent<SavedLists>();
                }
            }
        }


        static void CreateTransparentMaterialAndScripts(bool arg)
        {
            gos = Selection.gameObjects;

            foreach (GameObject go in gos)
            {
                foreach (Renderer rend in go.GetComponentsInChildren<Renderer>())
                {
                    transparentMaterials = new Material[rend.sharedMaterials.Length];

                    if (rend.GetComponent<Renderer>().GetType().Equals(typeof(BillboardRenderer)))
                    {
                        continue;
                    }

                    transparencyAmountsArray = new float[rend.sharedMaterials.Length];
                    isUsingSpeedTreeShaderArray = new bool[rend.sharedMaterials.Length];

                    for (int k = 0; k < rend.sharedMaterials.Length; k++)
                    {
                        if (rend.gameObject.GetComponent<ChangeMaterial>() == null)
                            rend.gameObject.AddComponent<ChangeMaterial>();
                        tempChangeMaterialScript = rend.gameObject.GetComponent<ChangeMaterial>();
                        tempChangeMaterialScript.SetMeshRenderer(rend);
                        nonTransparentMaterials = rend.sharedMaterials;

                        if (CheckIfUsingNatureShader(nonTransparentMaterials[k].shader,
                            ref nonTransparentMaterials[k], ref transparentMaterials[k], ref transparencyAmountsArray[k], ref isUsingSpeedTreeShaderArray[k]))
                        {
                            continue;
                        }
                        nameOfOldMaterial = nonTransparentMaterials[k].name;
                        findTransparentVersionsOfMaterial = AssetDatabase.FindAssets(nameOfOldMaterial + "Transparent");

                        if (findTransparentVersionsOfMaterial.Length == 0)
                        {
                            if (nonTransparentMaterials[k].shader == Shader.Find("Standard") || nonTransparentMaterials[k].shader == Shader.Find("Standard (Specular setup)"))
                            {
                                AssetDatabase.CreateAsset(new Material(nonTransparentMaterials[k]), "Assets/Plugins/UltimateSee-Through/Resources/TransparentMaterials/" + nameOfOldMaterial + "Transparent.mat");
                            }
                            else
                            {

                                AssetDatabase.CreateAsset(new Material(Shader.Find("Standard")), "Assets/Plugins/UltimateSee-Through/Resources/TransparentMaterials/" + nameOfOldMaterial + "Transparent.mat");

                            }
                            transparentMaterials[k] = Resources.Load("TransparentMaterials/" + nameOfOldMaterial + "Transparent", typeof(Material)) as Material;

                            if (!isCurrentMeshATreeBark)
                            {
                                transparentMaterials[k].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                                transparentMaterials[k].renderQueue = 3000;

                                transparentMaterials[k].SetFloat("_Mode", 3);

                                transparentMaterials[k].EnableKeyword("_ALPHAPREMULTIPLY_ON");

                                previousColor = transparentMaterials[k].color;
                                previousColor.a = 0;
                                transparentMaterials[k].color = previousColor;
                            }
                            else
                            {
                                if (transparentMaterials[k].GetTexture("_MainTex") == null)
                                {
                                    transparentMaterials[k].SetTexture("_MainTex", nonTransparentMaterials[k].GetTexture("_MainTex"));
                                }
                                isCurrentMeshATreeBark = false;
                                transparentMaterials[k].SetFloat("_Mode", 2);

                                transparentMaterials[k].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                                transparentMaterials[k].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                                transparentMaterials[k].SetInt("_ZWrite", 0);
                                transparentMaterials[k].DisableKeyword("_ALPHATEST_ON");
                                transparentMaterials[k].EnableKeyword("_ALPHABLEND_ON");
                                transparentMaterials[k].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                                transparentMaterials[k].renderQueue = 3000;

                                previousColor = transparentMaterials[k].color;
                                previousColor.a = 0.2f;
                                transparentMaterials[k].color = previousColor;
                            }

                            transparencyAmountsArray[k] = previousColor.a;
                        }
                        else
                        {
                            transparentMaterials[k] = Resources.Load("TransparentMaterials/" + nameOfOldMaterial + "Transparent", typeof(Material)) as Material;
                            transparencyAmountsArray[k] = 1 - nonTransparentMaterials[k].color.a;
                        }
                    }
                    rend.gameObject.layer = 1;
                    rend.sharedMaterials = nonTransparentMaterials;
                    tempChangeMaterialScript.SetNonTransparentMaterial(nonTransparentMaterials);
                    tempChangeMaterialScript.SetTransparentMaterials(transparentMaterials);
                    tempChangeMaterialScript.SetTransparencyAmountsArray(transparencyAmountsArray);

                    Transform[] childTransforms = FindChildTransforms(rend.gameObject);
                    if (childTransforms != null) rend.gameObject.GetComponent<ChangeMaterial>().SetArrayOfTransforms(childTransforms);


                    /*if (arg)
                    {
                        changeMaterialInParent = rend.gameObject.GetComponentsInParent<ChangeMaterial>();
                        changeMaterialInChildren = rend.gameObject.GetComponentsInChildren<ChangeMaterial>();

                        if (changeMaterialInChildren.Length + changeMaterialInParent.Length > 2)
                        {
                            rend.GetComponent<ChangeMaterial>().SetMoreThanOneChangeMaterialScriptInFamilyTree(AddChangeMaterialScriptToArray(rend.gameObject.GetComponent<ChangeMaterial>()));

                            for (int i = 0; i < changeMaterialInParent.Length; i++)
                            {
                                if (!changeMaterialInParent[i].linkedTransparencySettings.useLinkedTransparency)
                                {
                                    if (!parentsWhoNeedsToUpdateLinkedTransparency.Contains(changeMaterialInParent[i]))
                                    {
                                        parentsWhoNeedsToUpdateLinkedTransparency.Add(changeMaterialInParent[i]);
                                    }
                                }
                            }
                        }
                    }*/
                    foreach (Collider col in rend.GetComponentsInChildren<Collider>())
                    {
                        col.gameObject.layer = 1;
                    }
                }
            }
            /*if (arg)
            {
                CheckIfNeedToUpdateLinkedTransparencyForParents();
            }*/
        }


        static Transform[] FindChildTransforms(GameObject gO)
        {
            Collider[] childColliders = gO.GetComponentsInChildren<Collider>();

            for (int i = 0; i < childColliders.Length; i++)
            {
                if (childColliders[i].gameObject != gO && (childColliders[i].GetComponent<ChangeMaterial>() != null ||
                    childColliders[i].GetComponent<Renderer>() != null))
                {
                    //Debug.Log(childColliders.Length);
                    childColliders2 = new Collider[i];
                    System.Array.Copy(childColliders, childColliders2, i);
                    //Debug.Log(childColliders2.Length);
                    break;
                }
                if (i == (childColliders.Length - 1))
                {
                    childColliders2 = childColliders;
                }
            }


            Transform[] childTransforms = new Transform[childColliders2.Length];

            for (int i = 0; i < childColliders2.Length; i++)
            {
                childTransforms[i] = childColliders2[i].transform;
            }

            return childTransforms;
        }


        /*static void CheckIfNeedToUpdateLinkedTransparencyForParents()
        {
            foreach (ChangeMaterial cM in parentsWhoNeedsToUpdateLinkedTransparency)
            {
                changeMaterialInParent = cM.GetComponentsInParent<ChangeMaterial>();
                changeMaterialInChildren = cM.GetComponentsInChildren<ChangeMaterial>();

                cM.SetMoreThanOneChangeMaterialScriptInFamilyTree(AddChangeMaterialScriptToArray(cM));
            }
        }*/


        static bool CheckIfUsingNatureShader(Shader shaderOfObject, ref Material nonTransparentMaterial, ref Material transparentMaterial, ref float transparencyAmount,
            ref bool isUsingSpeedTreeShader)
        {
            if (shaderOfObject == Shader.Find("Nature/SpeedTree"))
            {
                for (int i = 0; i < nonTransparentMaterial.shaderKeywords.Length; i++)
                {
                    if (nonTransparentMaterial.shaderKeywords[i] == "GEOM_TYPE_BRANCH" || nonTransparentMaterial.shaderKeywords[i] == "GEOM_TYPE_BRANCH_DETAIL" || nonTransparentMaterial.shaderKeywords[i] == "GEOM_TYPE_FROND")
                    {
                        isCurrentMeshATreeBark = true;
                        return false;
                    }
                }
                nameOfOldMaterial = nonTransparentMaterial.name;

                if (AssetDatabase.FindAssets(nameOfOldMaterial + "Transparent").Length == 0)
                {
                    AssetDatabase.CreateAsset(new Material(nonTransparentMaterial), "Assets/Plugins/UltimateSee-Through/Resources/TransparentMaterials/" + nameOfOldMaterial + "Transparent.mat");

                    transparentMaterial = Resources.Load("TransparentMaterials/" + nameOfOldMaterial + "Transparent", typeof(Material)) as Material;
                    transparentMaterial.SetFloat("_Cutoff", nonTransparentMaterial.GetFloat("_Cutoff"));
                }
                else
                {
                    transparentMaterial = Resources.Load("TransparentMaterials/" + nameOfOldMaterial + "Transparent", typeof(Material)) as Material;
                }
                transparencyAmount = nonTransparentMaterial.GetFloat("_Cutoff");                                                                //SINCE _Cutoff
                isUsingSpeedTreeShader = true;
                return true;
            }
            else if (shaderOfObject == Shader.Find("Nature/Tree Soft Occlusion Leaves") || shaderOfObject == Shader.Find("Nature/Tree Creator Leaves") || shaderOfObject == Shader.Find("Nature/Tree Creator Leaves Fast"))
            {
                nameOfOldMaterial = nonTransparentMaterial.name;

                if (AssetDatabase.FindAssets(nameOfOldMaterial + "Transparent").Length == 0)
                {
                    AssetDatabase.CreateAsset(new Material(nonTransparentMaterial), "Assets/Plugins/UltimateSee-Through/Resources/TransparentMaterials/" + nameOfOldMaterial + "Transparent.mat");

                    transparentMaterial = Resources.Load("TransparentMaterials/" + nameOfOldMaterial + "Transparent", typeof(Material)) as Material;
                    transparentMaterial.SetFloat("_Cutoff", 1);

                }
                else
                {
                    transparentMaterial = Resources.Load("TransparentMaterials/" + nameOfOldMaterial + "Transparent", typeof(Material)) as Material;
                }
                transparencyAmount = nonTransparentMaterial.GetFloat("_Cutoff");                                                                   //SINCE _Cutoff
                isUsingSpeedTreeShader = true;
                return true;
            }
            else if (shaderOfObject == Shader.Find("Nature/Tree Soft Occlusion Bark"))
            {
                isCurrentMeshATreeBark = true;
                return false;
            }
            else
            {
                return false;
            }
        }


        [MenuItem("Window/Ultimate See-Through/Use Collider To Trigger Transparency %h")]
        [CanEditMultipleObjects]
        static void UseColliderTriggerForTransparent()
        {
            GameObject colliderObject;

            for (int i = 0; i < 1000; i++)
            {
                if (!GameObject.Find("colliderObject" + i.ToString()))
                {
                    colliderObject = new GameObject("colliderObject" + i.ToString());
                    colliderObject.AddComponent<ColliderTrigger>();
                    colliderObject.AddComponent<BoxCollider>();
                    colliderObject.GetComponent<BoxCollider>().size = new Vector3(3, 1, 3);
                    colliderObject.transform.position += new Vector3(0, 0.5f, 0);
                    colliderObject.GetComponent<BoxCollider>().isTrigger = true;
                    break;
                }
                else if (i == 999)
                {
                    Debug.Log("Using 999 colliderObjects already, delete some.");
                }
            }
        }


        [MenuItem("Window/Ultimate See-Through/Don't Make Bark Materials Transparent %j")]
        [CanEditMultipleObjects]
        static void DontMakeBarkMaterialsTransparent()
        {
            Material[] tempMaterials;

            tempChangeMaterialScript = Selection.activeGameObject.GetComponent<ChangeMaterial>();
            tempMaterials = tempChangeMaterialScript.nonTransparentMaterials;

            for (int i = 0; i < tempMaterials.Length; i++)
            {
                if (tempMaterials[i].shader == Shader.Find("Nature/SpeedTree"))
                {
                    for (int j = 0; j < tempMaterials[i].shaderKeywords.Length; j++)
                    {
                        if (tempMaterials[i].shaderKeywords[j] == "GEOM_TYPE_BRANCH" || tempMaterials[i].shaderKeywords[j] == "GEOM_TYPE_BRANCH_DETAIL" || tempMaterials[i].shaderKeywords[j] == "GEOM_TYPE_FROND")
                        {
                            tempChangeMaterialScript.transparentMaterials[i] = tempChangeMaterialScript.nonTransparentMaterials[i];
                        }
                    }
                }
                else if (tempMaterials[i].shader == Shader.Find("Nature/Tree Soft Occlusion Bark"))
                {
                    tempChangeMaterialScript.transparentMaterials[i] = tempChangeMaterialScript.nonTransparentMaterials[i];
                }
            }


            if (PrefabUtility.GetPrefabParent(tempChangeMaterialScript.gameObject) == null)
            {
                return;
            }
            else if (!PrefabUtility.GetPrefabType(tempChangeMaterialScript.gameObject).Equals(PrefabType.PrefabInstance) && !PrefabUtility.GetPrefabType(tempChangeMaterialScript.gameObject).Equals(PrefabType.Prefab))
            {
                Debug.Log("Did not update prefab since it is a model prefab");
            }
            else
            {
                PrefabUtility.ReplacePrefab(tempChangeMaterialScript.gameObject, PrefabUtility.GetPrefabParent(tempChangeMaterialScript.gameObject), ReplacePrefabOptions.ConnectToPrefab);
            }
        }


        private void ResetValues()
        {
            listOfAlphaValueArrays = new List<float[]>();
            listOfCopiedValuesGameObjects = new List<GameObject>();
            listOfNumberOfUpdatesToTurnTransparent = new List<float>();
            listOfNumberOfUpdatesToTurnNonTransparent = new List<float>();

            if (savedArraysScript == null)
            {
                savedArraysScript = Camera.main.GetComponent<SavedLists>();
            }
            savedArraysScript.ResetValues();
        }


        [MenuItem("Window/Ultimate See-Through/Don't Make Leaf Materials Transparent %k")]
        [CanEditMultipleObjects]
        static void DontMakeLeafMaterialsTransparent()
        {
            Material[] tempMaterials;

            tempChangeMaterialScript = Selection.activeGameObject.GetComponent<ChangeMaterial>();
            tempMaterials = tempChangeMaterialScript.nonTransparentMaterials;

            for (int i = 0; i < tempMaterials.Length; i++)
            {
                if (tempMaterials[i].shader == Shader.Find("Nature/SpeedTree"))
                {
                    for (int j = 0; j < tempMaterials[i].shaderKeywords.Length; j++)
                    {
                        if (tempMaterials[i].shaderKeywords[j] == "GEOM_TYPE_LEAF")
                        {
                            tempChangeMaterialScript.transparentMaterials[i] = tempChangeMaterialScript.nonTransparentMaterials[i];
                            tempChangeMaterialScript.alphaAmountsArray[i] = 1;
                        }
                    }
                }
                else if (tempMaterials[i].shader == Shader.Find("Nature/Tree Soft Occlusion Leaves") || tempMaterials[i].shader == Shader.Find("Nature/Tree Creator Leaves") || tempMaterials[i].shader == Shader.Find("Nature/Tree Creator Leaves Fast"))
                {
                    tempChangeMaterialScript.transparentMaterials[i] = tempChangeMaterialScript.nonTransparentMaterials[i];
                }
            }


            if (PrefabUtility.GetPrefabParent(tempChangeMaterialScript.gameObject) == null)
            {
                return;
            }
            else if (!PrefabUtility.GetPrefabType(tempChangeMaterialScript.gameObject).Equals(PrefabType.PrefabInstance) && !PrefabUtility.GetPrefabType(tempChangeMaterialScript.gameObject).Equals(PrefabType.Prefab))
            {
                Debug.Log("Did not update prefab since it is a model prefab");
            }
            else
            {
                PrefabUtility.ReplacePrefab(tempChangeMaterialScript.gameObject, PrefabUtility.GetPrefabParent(tempChangeMaterialScript.gameObject), ReplacePrefabOptions.ConnectToPrefab);
            }
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Copy Values"))
            {
                CopyValues();
            }
            if (GUILayout.Button("Paste Values"))
            {
                PasteValues();
            }
            if (GUILayout.Button("Delete All Saved Values"))
            {
                if (listOfCopiedValuesGameObjects == null || listOfCopiedValuesGameObjects.Count == 0)
                {
                    Debug.Log("No values copied yet.");
                }
                else if (EditorUtility.DisplayDialog("Confirm Reset", "This will delete all saved values for " + listOfCopiedValuesGameObjects.Count + " objects, continue?", "Yes", "No"))
                {
                    ResetValues();
                }
            }

            if (previousGameObject == null)
            {
                previousGameObject = Selection.activeGameObject;
            }
            if (Selection.activeGameObject != previousGameObject)
            {
                previousGameObject = Selection.activeGameObject;
            }
            if (Selection.activeGameObject.GetComponent<ChangeMaterial>() != null)
            {
                ChangeLeafButton();
                CheckIfValuesChanged();
            }
        }


        private void SetTempValues()
        {
            if (tempGameObject != null && tempGameObject.GetComponent<ChangeMaterial>() != null)
            {
                tempListOfAlphaValueArrays = new float[tempGameObject.GetComponent<ChangeMaterial>().alphaAmountsArray.Length];
                checkIfChangedChangeMaterial.alphaAmountsArray.CopyTo(tempListOfAlphaValueArrays, 0);
                tempEasingMethodArrayToTransparent = new SimpleTweenEaseMethod[checkIfChangedChangeMaterial.easingMethodToTransparentArray.Length];
                tempEasingMethodArrayToNonTransparent = new SimpleTweenEaseMethod[checkIfChangedChangeMaterial.easingMethodToNonTransparentArray.Length];
                tempEasingTypeArrayToTransparent = new SimpleTweenEaseType[checkIfChangedChangeMaterial.easingTypeToTransparentArray.Length];
                tempEasingTypeArrayToNonTransparent = new SimpleTweenEaseType[checkIfChangedChangeMaterial.easingTypeToNonTransparentArray.Length];
                checkIfChangedChangeMaterial.easingMethodToTransparentArray.CopyTo(tempEasingMethodArrayToTransparent, 0);
                checkIfChangedChangeMaterial.easingTypeToTransparentArray.CopyTo(tempEasingTypeArrayToTransparent, 0);
                checkIfChangedChangeMaterial.easingMethodToNonTransparentArray.CopyTo(tempEasingMethodArrayToNonTransparent, 0);
                checkIfChangedChangeMaterial.easingTypeToNonTransparentArray.CopyTo(tempEasingTypeArrayToNonTransparent, 0);
                tempListOfNumberOfUpdatesToTurnTransparent = checkIfChangedChangeMaterial.timeToTurnTransparent;
                tempListOfNumberOfUpdatesToTurnNonTransparent = checkIfChangedChangeMaterial.timeToTurnNonTransparent;
            }
        }


        private void CheckIfValuesChanged()
        {
            if (tempGameObject != null && tempGameObject != Selection.activeGameObject && PrefabUtility.GetPrefabParent(Selection.activeGameObject) != null)
            {
                tempGameObject = Selection.activeGameObject;
                checkIfChangedChangeMaterial = tempGameObject.GetComponent<ChangeMaterial>();

                SetTempValues();
            }
            else if (tempGameObject == null)
            {
                tempGameObject = Selection.activeGameObject;
                checkIfChangedChangeMaterial = tempGameObject.GetComponent<ChangeMaterial>();

                SetTempValues();
            }
            else
            {
                for (int i = 0; i < tempListOfAlphaValueArrays.Length; i++)
                {
                    if (tempListOfAlphaValueArrays != null && tempListOfAlphaValueArrays[i] != checkIfChangedChangeMaterial.alphaAmountsArray[i])
                    {
                        checkIfChangedChangeMaterial.TurnTransparent();
                        tempListOfAlphaValueArrays = new float[checkIfChangedChangeMaterial.alphaAmountsArray.Length];
                        checkIfChangedChangeMaterial.alphaAmountsArray.CopyTo(tempListOfAlphaValueArrays, 0);
                        return;
                    }
                    if (tempEasingMethodArrayToTransparent[i] != checkIfChangedChangeMaterial.easingMethodToTransparentArray[i])
                    {
                        checkIfChangedChangeMaterial.TurnTransparent();
                        //tempListOfTransparencyChangeSpeedArrays = new float[checkIfChangedChangeMaterial.transparencyChangeSpeed.Length];
                        checkIfChangedChangeMaterial.easingMethodToTransparentArray.CopyTo(tempEasingMethodArrayToTransparent, 0);
                        return;
                    }
                    if (tempEasingTypeArrayToTransparent[i] != checkIfChangedChangeMaterial.easingTypeToTransparentArray[i])
                    {
                        checkIfChangedChangeMaterial.TurnTransparent();
                        //tempListOfTransparencyChangeSpeedArrays = new float[checkIfChangedChangeMaterial.transparencyChangeSpeed.Length];
                        checkIfChangedChangeMaterial.easingTypeToTransparentArray.CopyTo(tempEasingTypeArrayToTransparent, 0);
                        return;
                    }
                    if (tempEasingMethodArrayToNonTransparent[i] != checkIfChangedChangeMaterial.easingMethodToNonTransparentArray[i])
                    {
                        checkIfChangedChangeMaterial.TurnTransparent();
                        //tempListOfTransparencyChangeSpeedArrays = new float[checkIfChangedChangeMaterial.transparencyChangeSpeed.Length];
                        checkIfChangedChangeMaterial.easingMethodToNonTransparentArray.CopyTo(tempEasingMethodArrayToNonTransparent, 0);
                        return;
                    }
                    if (tempEasingTypeArrayToNonTransparent[i] != checkIfChangedChangeMaterial.easingTypeToNonTransparentArray[i])
                    {
                        checkIfChangedChangeMaterial.TurnTransparent();
                        //tempListOfTransparencyChangeSpeedArrays = new float[checkIfChangedChangeMaterial.transparencyChangeSpeed.Length];
                        checkIfChangedChangeMaterial.easingTypeToNonTransparentArray.CopyTo(tempEasingTypeArrayToNonTransparent, 0);
                        return;
                    }
                    if (tempListOfNumberOfUpdatesToTurnTransparent != checkIfChangedChangeMaterial.timeToTurnTransparent)
                    {
                        checkIfChangedChangeMaterial.TurnTransparent();
                        tempListOfNumberOfUpdatesToTurnTransparent = checkIfChangedChangeMaterial.timeToTurnTransparent;
                    }
                    if (tempListOfNumberOfUpdatesToTurnNonTransparent != checkIfChangedChangeMaterial.timeToTurnNonTransparent)
                    {
                        checkIfChangedChangeMaterial.TurnTransparent();
                        tempListOfNumberOfUpdatesToTurnNonTransparent = checkIfChangedChangeMaterial.timeToTurnNonTransparent;
                    }
                    if (checkIfChangedChangeMaterial.timeToTurnNonTransparent < 0)
                    {
                        checkIfChangedChangeMaterial.timeToTurnNonTransparent = 0;
                    }
                    else if (checkIfChangedChangeMaterial.timeToTurnTransparent < 0)
                    {
                        checkIfChangedChangeMaterial.timeToTurnTransparent = 0;
                    }
                }
            }
        }


        private void ChangeLeafButton()
        {
            tempChangeMaterialScript = Selection.activeGameObject.GetComponent<ChangeMaterial>();
            tempMaterials = tempChangeMaterialScript.nonTransparentMaterials;

            for (int j = 0; j < tempMaterials.Length; j++)
            {
                if (tempMaterials[j].shader == Shader.Find("Nature/SpeedTree") && CheckIfCorrectKeyword(tempMaterials[j]) ||
                        tempMaterials[j].shader == Shader.Find("Nature/Tree Soft Occlusion Leaves") ||
                        tempMaterials[j].shader == Shader.Find("Nature/Tree Creator Leaves") ||
                        tempMaterials[j].shader == Shader.Find("Nature/Tree Creator Leaves Fast"))
                {
                    if (GUILayout.Button("Change To Other Tree Shader"))
                    {

                        tempMaterials = tempChangeMaterialScript.transparentMaterials;

                        for (int i = 0; i < tempMaterials.Length; i++)
                        {
                            if (tempMaterials[i].shader == Shader.Find("Legacy Shaders/Transparent/Diffuse"))
                            {
                                tempMaterials[i].shader = tempChangeMaterialScript.nonTransparentMaterials[i].shader;
                                tempChangeMaterialScript.ChangeLegacyTreeShaderToOtherShader(i, false);
                            }
                            else if (tempMaterials[i].shader == Shader.Find("Nature/SpeedTree") && CheckIfCorrectKeyword(tempMaterials[i]) ||
                                tempMaterials[i].shader == Shader.Find("Nature/Tree Soft Occlusion Leaves") ||
                                tempMaterials[i].shader == Shader.Find("Nature/Tree Creator Leaves") ||
                                tempMaterials[i].shader == Shader.Find("Nature/Tree Creator Leaves Fast"))
                            {
                                tempMaterials[i].shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
                                tempChangeMaterialScript.ChangeLegacyTreeShaderToOtherShader(i, true);
                            }
                        }
                    }
                    break;
                }
            }
        }


        private bool CheckIfCorrectKeyword(Material arg)
        {
            for (int j = 0; j < arg.shaderKeywords.Length; j++)
            {
                if (arg.shaderKeywords[j] == "GEOM_TYPE_LEAF")
                {
                    return true;
                }
            }
            return false;
        }
    }
}