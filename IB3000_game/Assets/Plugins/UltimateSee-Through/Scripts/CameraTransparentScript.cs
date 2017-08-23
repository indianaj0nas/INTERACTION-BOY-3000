using UnityEngine;
using System.Collections.Generic;
using System;

namespace NSUltimateSeeThrough
{
    public class CameraTransparentScript : MonoBehaviour
    {
        [SerializeField]
        private Transform playerTransform, cameraTransform;
        [SerializeField]
        private int raycastInterval;
        private int currentRaycastCounter, numberOfRaycastHits;
        private LayerMask layerMask;
        private RaycastHit[] raycastHits;
        //[SerializeField]
        private List<Transform> listOfTransforms, previousList;
        private Transform tempTransform;
        [SerializeField]
        private UltimateSeeThroughEventManager ultimateSeeThroughEventManagerScript;
        private Ray ray;
        private Vector3 distanceVector;


        void Start()
        {
            if (playerTransform == null)
            {
                playerTransform = GameObject.Find("Player").transform;
            }
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
            listOfTransforms = new List<Transform>(10);
            previousList = new List<Transform>(10);
            currentRaycastCounter = raycastInterval;
            layerMask = (1 << 1);
            ray = new Ray();
            raycastHits = new RaycastHit[10];
        }


        void Update()
        {
            //Debug.Log(System.Environment.Version);
            if (currentRaycastCounter == 0)
            {
                if (raycastInterval != 0) currentRaycastCounter = raycastInterval;
                distanceVector = playerTransform.position - cameraTransform.position;
                ray.origin = cameraTransform.position;
                ray.direction = distanceVector;
                numberOfRaycastHits = Physics.RaycastNonAlloc(ray, raycastHits, GetMagnitude(), layerMask);

                if (listOfTransforms.Count > 0)
                {
                    if (numberOfRaycastHits > 0)
                    {
                        for (int i = 0; i < listOfTransforms.Count; i++)
                        {
                            previousList.Add(listOfTransforms[i]);
                        }
                        CheckRaycastResults();

                        for (int i = 0; i < previousList.Count; i++)
                        {
                            ultimateSeeThroughEventManagerScript.SetTransformObjectToTransparentOrNonTransparentMethod(previousList[i], false);

                            for (int j = 0; j < listOfTransforms.Count; j++)
                            {
                                if (listOfTransforms[j] == previousList[i])
                                {
                                    listOfTransforms.RemoveAt(j);
                                }
                            }
                        }
                        previousList.Clear();
                    }
                    else
                    {
                        for (int i = 0; i < listOfTransforms.Count; i++)
                        {
                            ultimateSeeThroughEventManagerScript.SetTransformObjectToTransparentOrNonTransparentMethod(listOfTransforms[i], false);
                        }
                        listOfTransforms.Clear();
                    }
                }
                else
                {
                    if (numberOfRaycastHits > 0) CheckRaycastResults();
                }
            }
            else
            {
                currentRaycastCounter--;
            }
        }


        private float GetMagnitude()
        {
            return (float)Math.Sqrt(distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y + distanceVector.z * distanceVector.z);
        }


        public void SetUltimateSeeThroughEventManager(UltimateSeeThroughEventManager ultimateSeeThroughEventManagerScript)
        {
            this.ultimateSeeThroughEventManagerScript = ultimateSeeThroughEventManagerScript;
        }


        private void CheckRaycastResults()
        {
            for (int i = 0; i < numberOfRaycastHits; i++)
            {
                tempTransform = raycastHits[i].transform;
                if (tempTransform == null) return;

                if (listOfTransforms.Count == 0)
                {
                    listOfTransforms.Add(tempTransform);
                    ultimateSeeThroughEventManagerScript.SetTransformObjectToTransparentOrNonTransparentMethod(tempTransform, true);
                }
                else
                {
                    for (int j = 0; j < listOfTransforms.Count; j++)
                    {
                        if (listOfTransforms[j] == tempTransform)
                        {
                            for (int k = 0; k < previousList.Count; k++)
                            {
                                if (previousList[k] == tempTransform)
                                {
                                    previousList.RemoveAt(k);
                                    break;
                                }
                            }

                            break;
                        }
                        if (j == (listOfTransforms.Count - 1))
                        {
                            listOfTransforms.Add(tempTransform);
                            ultimateSeeThroughEventManagerScript.SetTransformObjectToTransparentOrNonTransparentMethod(tempTransform, true);
                        }
                    }
                }
            }
        }


        public void SetCameraTransform(Transform cameraTransform)
        {
            this.cameraTransform = cameraTransform;
        }


        public void ReceivePlayerTransform(Transform arg)
        {
            playerTransform = arg;
        }
    }
}