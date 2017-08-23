using UnityEngine;
using System.Collections.Generic;


namespace NSUltimateSeeThrough
{
    [ExecuteInEditMode]
    [System.Serializable]
    public class SavedLists : MonoBehaviour
    {
        [HideInInspector]
        public List<GameObject> listOfCopiedValuesGameObjects;
        [HideInInspector]
        public List<float> listOfAlphaValues;
        [HideInInspector]
        public List<float> listOfNumberOfUpdatesToTurnTransparent,
            listOfNumberOfUpdatesToTurnNonTransparent;
        [HideInInspector]
        public int[] numberOfAlphaValuesPerObject;
        [HideInInspector]
        public List<float[]> listOfAlphaValueArrays;
        private float[] tempArray;
        private int tempInt;
        private List<SimpleTweenEaseMethod> listOfEasingMethodsToTransparent, listOfEasingMethodsToNonTransparent;
        private List<SimpleTweenEaseType> listOfEasingTypesToTransparent, listOfEasingTypesToNonTransparent;
        [HideInInspector]
        public List<SimpleTweenEaseMethod[]> listOfEasingMethodToTransparentArrays, listOfEasingMethodToNonTransparentArrays;
        [HideInInspector]
        public List<SimpleTweenEaseType[]> listOfEasingTypeToTransparentArrays, listOfEasingTypeToNonTransparentArrays;
        private SimpleTweenEaseMethod[] easingMethodToTransparentArray, easingMethodToNonTransparentArray;
        private SimpleTweenEaseType[] easingTypeToTransparentArray, easingTypeToNonTransparentArray;



        public delegate void NoneToVoid();
        public static event NoneToVoid UpdateListsEvent;


        void Start()
        {
            UpdateListsMethod();
        }


        public void UpdateListsMethod()
        {
            if (UpdateListsEvent != null)
            {
                UpdateListsEvent();
            }
        }


        /*public List<float> GetListOfTransparencyChangeSpeed()
        {
            return listOfTransparencyChangeSpeed;
        }*/


        public void SaveLists(List<GameObject> listOfCopiedValuesGameObjects, List<float[]> listOfAlphaValueArrays, List<float> listOfNumberOfUpdatesToTurnTransparent,
            List<SimpleTweenEaseMethod[]> listOfEasingMethodToTransparentArrays, List<float> listOfNumberOfUpdatesToTurnNonTransparent, List<SimpleTweenEaseType[]> listOfEasingTypeToTransparentArrays,
            List<SimpleTweenEaseMethod[]> listOfEasingMethodToNonTransparentArrays, List<SimpleTweenEaseType[]> listOfEasingTypeToNonTransparentArrays)
        {
            this.listOfCopiedValuesGameObjects = listOfCopiedValuesGameObjects;

            numberOfAlphaValuesPerObject = new int[listOfCopiedValuesGameObjects.Count];
            listOfAlphaValues = new List<float>();
            listOfEasingMethodsToTransparent = new List<SimpleTweenEaseMethod>();
            listOfEasingTypesToTransparent = new List<SimpleTweenEaseType>();
            listOfEasingMethodsToNonTransparent = new List<SimpleTweenEaseMethod>();
            listOfEasingTypesToNonTransparent = new List<SimpleTweenEaseType>();

            for (int i = 0; i < listOfAlphaValueArrays.Count; i++)
            {
                for (int j = 0; j < listOfAlphaValueArrays[i].Length; j++)
                {
                    listOfAlphaValues.Add(listOfAlphaValueArrays[i][j]);
                    listOfEasingMethodsToTransparent.Add(listOfEasingMethodToTransparentArrays[i][j]);
                    listOfEasingTypesToTransparent.Add(listOfEasingTypeToTransparentArrays[i][j]);
                    listOfEasingMethodsToNonTransparent.Add(listOfEasingMethodToNonTransparentArrays[i][j]);
                    listOfEasingTypesToNonTransparent.Add(listOfEasingTypeToNonTransparentArrays[i][j]);
                }
                numberOfAlphaValuesPerObject[i] = listOfAlphaValueArrays[i].Length;
            }
            this.listOfNumberOfUpdatesToTurnTransparent = listOfNumberOfUpdatesToTurnTransparent;
            this.listOfNumberOfUpdatesToTurnNonTransparent = listOfNumberOfUpdatesToTurnNonTransparent;


            /*Debug.Log(listOfCopiedValuesGameObjects.Count);
            Debug.Log(numberOfAlphaValuesPerObject.Length);
            Debug.Log(listOfNumberOfUpdatesToTurnTransparent.Count);
            Debug.Log(listOfTransparencyChangeSpeed.Count);
            Debug.Log(listOfNumberOfUpdatesToTurnNonTransparent.Count);*/
        }


        public void ResetValues()
        {
            listOfCopiedValuesGameObjects = null;
            numberOfAlphaValuesPerObject = null;
            listOfAlphaValues = null;
            listOfNumberOfUpdatesToTurnTransparent = null;
            listOfNumberOfUpdatesToTurnNonTransparent = null;
            listOfEasingMethodsToTransparent = null;
            listOfEasingTypesToTransparent = null;
            listOfEasingMethodsToNonTransparent = null;
            listOfEasingTypesToNonTransparent = null;
        }


        public List<float[]> GetBothArrayLists(out List<SimpleTweenEaseMethod[]> listEasingMethodsToTransparent, out List<SimpleTweenEaseType[]> listEasingTypesToTransparent,
            out List<SimpleTweenEaseMethod[]> listEasingMethodsToNonTransparent, out List<SimpleTweenEaseType[]> listEasingTypesToNonTransparent)
        {
            listOfAlphaValueArrays = new List<float[]>();
            listOfEasingMethodToTransparentArrays = new List<SimpleTweenEaseMethod[]>();
            listOfEasingTypeToTransparentArrays = new List<SimpleTweenEaseType[]>();
            tempInt = 0;

            for (int i = 0; i < numberOfAlphaValuesPerObject.Length; i++)
            {
                tempArray = new float[numberOfAlphaValuesPerObject[i]];
                easingMethodToTransparentArray = new SimpleTweenEaseMethod[numberOfAlphaValuesPerObject[i]];
                easingTypeToTransparentArray = new SimpleTweenEaseType[numberOfAlphaValuesPerObject[i]];

                for (int j = 0; j < numberOfAlphaValuesPerObject[i]; j++)
                {
                    tempArray[j] = listOfAlphaValues[tempInt + j];
                    easingMethodToTransparentArray[j] = listOfEasingMethodsToTransparent[tempInt + j];
                    easingTypeToTransparentArray[j] = listOfEasingTypesToTransparent[tempInt + j];
                }
                tempInt += numberOfAlphaValuesPerObject[i];
                listOfAlphaValueArrays.Add(tempArray);
                listOfEasingMethodToTransparentArrays.Add(easingMethodToTransparentArray);
                listOfEasingTypeToTransparentArrays.Add(easingTypeToTransparentArray);
            }
            listEasingMethodsToTransparent = listOfEasingMethodToTransparentArrays;
            listEasingTypesToTransparent = listOfEasingTypeToTransparentArrays;
            listEasingMethodsToNonTransparent = listOfEasingMethodToNonTransparentArrays;
            listEasingTypesToNonTransparent = listOfEasingTypeToNonTransparentArrays;

            return listOfAlphaValueArrays;
        }


        public bool AreListsEmpty()
        {
            if (listOfCopiedValuesGameObjects == null || listOfCopiedValuesGameObjects.Count == 0 ||
                numberOfAlphaValuesPerObject == null || numberOfAlphaValuesPerObject.Length == 0 ||
                listOfNumberOfUpdatesToTurnTransparent == null || listOfNumberOfUpdatesToTurnTransparent.Count == 0 ||
                listOfEasingMethodToTransparentArrays == null || listOfEasingMethodToTransparentArrays.Count == 0 ||
                listOfEasingTypeToTransparentArrays == null || listOfEasingTypeToTransparentArrays.Count == 0 ||
                listOfEasingMethodToNonTransparentArrays == null || listOfEasingMethodToNonTransparentArrays.Count == 0 ||
                listOfEasingTypeToNonTransparentArrays == null || listOfEasingTypeToNonTransparentArrays.Count == 0 ||
                listOfNumberOfUpdatesToTurnNonTransparent == null || listOfNumberOfUpdatesToTurnNonTransparent.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}