using UnityEngine;

namespace NSUltimateSeeThrough
{
    public class ColliderTrigger : MonoBehaviour
    {
        [SerializeField]
        private ChangeMaterial[] scriptsToTriggerTransparencyFor;
        private int collidersPresent;


        void OnTriggerEnter()
        {
            collidersPresent++;

            if (collidersPresent == 1)
            {
                for (int i = 0; i < scriptsToTriggerTransparencyFor.Length; i++)
                {
                    if (scriptsToTriggerTransparencyFor[i] != null)
                    {
                        scriptsToTriggerTransparencyFor[i].SetToTransparentOrNonTransparent(true);
                    }
                }
            }
        }


        void OnTriggerExit()
        {
            collidersPresent--;

            if (collidersPresent == 0)
            {
                for (int i = 0; i < scriptsToTriggerTransparencyFor.Length; i++)
                {
                    if (scriptsToTriggerTransparencyFor[i] != null)
                    {
                        scriptsToTriggerTransparencyFor[i].SetToTransparentOrNonTransparent(false);
                    }
                }
            }
        }
    }
}