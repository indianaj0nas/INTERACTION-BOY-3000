using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public enum SimpleTweenEaseMethod
{
    In,
    Out,
    InOut
}


public enum SimpleTweenEaseType
{
    Linear,
    Quadratic,
    Cubic,
    Quartic,
    Quintic,
    Sinus,
    Exponential,
    Circular,
    Back,
    Bounce,
    Elastic
}


public class SimpleTween : MonoBehaviour
{
    private static SimpleTween simpleTween;
    private static IEnumerator[] cRTweenArray;
    private static int currentArrayCounter, maxArrayCount;
    private static float[] arrayOfCurrentDuration;
    private static bool isArrayCounterReset;
    private static Func<float, float, float> linearEasingType;
    private static SimpleTweenEaseType sTETLinear;
    private static bool[] arrayOfPause;


    void Awake()
    {
        sTETLinear = SimpleTweenEaseType.Linear;
        linearEasingType = Linear.EaseIn;
        simpleTween = this;
        maxArrayCount = 500;
        cRTweenArray = new IEnumerator[maxArrayCount];
        arrayOfCurrentDuration = new float[maxArrayCount];
        arrayOfPause = new bool[maxArrayCount];
    }


    public static void PauseOrUnpauseTween(int tweenID)
    {
        arrayOfPause[tweenID] = !arrayOfPause[tweenID];
    }


    public static void CancelTween(int numberInArray)
    {
        simpleTween.StopCoroutine(cRTweenArray[numberInArray]);
    }


    private static void CheckIfArrayCounterReset()
    {
        if (isArrayCounterReset)
        {
            if (cRTweenArray[currentArrayCounter] != null)
                simpleTween.StopCoroutine(cRTweenArray[currentArrayCounter]);

            if (arrayOfPause[currentArrayCounter]) arrayOfPause[currentArrayCounter] = false;
        }
    }


    private static bool CheckIfEasingTypeEqualsLinear(SimpleTweenEaseType sTET)
    {
        return sTET == sTETLinear;
    }


    public static int MaterialAlphaColor(Material mat, SimpleTweenEaseMethod sTEM, SimpleTweenEaseType sTET,
        float currentDuration, float totalDuration, float startValue, float changeInAmount, Action actionOnComplete)
    {
        CheckIfArrayCounterResetAndSetCurrentDuration(currentDuration);

        /*simpleTween.StartCoroutine(CRMaterialAlphaColor(mat, linearEasingType,
                totalDuration, currentArrayCounter, actionOnComplete, startValue, changeInAmount, mat.color));
        return GetCurrentTweenID();*/

        if (CheckIfEasingTypeEqualsLinear(sTET))
        {
            cRTweenArray[currentArrayCounter] = CRMaterialAlphaColor(mat, linearEasingType,
                totalDuration, currentArrayCounter, actionOnComplete, startValue, changeInAmount, mat.color);
        }
        else
        {
            cRTweenArray[currentArrayCounter] = CRMaterialAlphaColor(mat, GetEasingFunc(sTET, sTEM),
                totalDuration, currentArrayCounter, actionOnComplete, startValue, changeInAmount, mat.color);
        }
        simpleTween.StartCoroutine(cRTweenArray[currentArrayCounter]);
        return GetCurrentTweenID();
    }


    public static float GetCurrentDuration(int tweenID)
    {
        return arrayOfCurrentDuration[tweenID];
    }

    private static IEnumerator CRMaterialAlphaColor(Material mat, Func<float, float, float> func,
        float totalDuration, int arrayNumber, Action actionOnComplete,
        float startValue, float changeInAmount, Color startColor)
    {
        while (true)
        {
            arrayOfCurrentDuration[arrayNumber] += Time.deltaTime;
            if (arrayOfCurrentDuration[arrayNumber] >= totalDuration)
            {
                startColor.a = startValue + (func(totalDuration, totalDuration) * changeInAmount);
                mat.color = startColor;
                if (actionOnComplete != null) actionOnComplete();
                yield break;
            }
            else
            {
                startColor.a = startValue + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount);
                mat.color = startColor;
            }
            yield return null;
        }
    }


    private static int GetCurrentTweenID()
    {
        currentArrayCounter++;
        if (currentArrayCounter >= maxArrayCount)
        {
            currentArrayCounter = 0;
            if (!isArrayCounterReset) isArrayCounterReset = true;
            return maxArrayCount - 1;
        }
        return currentArrayCounter - 1;
    }


    public static int TextAlphaColor(Text text, SimpleTweenEaseMethod sTEM, SimpleTweenEaseType sTET,
        float currentDuration, float totalDuration, float startValue, float changeInAmount, Action actionOnComplete)
    {
        CheckIfArrayCounterResetAndSetCurrentDuration(currentDuration);

        cRTweenArray[currentArrayCounter] = CRTweenAlphaTextColor(text, GetEasingFunc(sTET, sTEM),
            totalDuration, actionOnComplete, currentArrayCounter, text.color.a, changeInAmount,
            text.color);
        simpleTween.StartCoroutine(cRTweenArray[currentArrayCounter]);

        return GetCurrentTweenID();
    }


    private static IEnumerator CRTweenAlphaTextColor(Text text, Func<float, float, float> func,
        float totalDuration, Action actionOnComplete, int arrayNumber,
        float valueToTweenFrom, float changeInAmount, Color startColor)
    {
        while (true)
        {
            arrayOfCurrentDuration[arrayNumber] += Time.deltaTime;
            if (arrayOfCurrentDuration[arrayNumber] >= totalDuration)
            {
                startColor.a = valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount);
                text.color = startColor;
                actionOnComplete();
                yield break;
            }
            else
            {
                startColor.a = valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount);
                text.color = startColor;
            }
            yield return null;
        }
    }


    /*private static void CheckIfArrayCounterResetAndResetCurrentDuration()
    {
        CheckIfArrayCounterReset();
        if (arrayOfCurrentDuration[currentArrayCounter] != 0)
            arrayOfCurrentDuration[currentArrayCounter] = 0;
    }*/


    private static void CheckIfArrayCounterResetAndSetCurrentDuration(float currentDuration)
    {
        CheckIfArrayCounterReset();
        if (arrayOfCurrentDuration[currentArrayCounter] != currentDuration)
            arrayOfCurrentDuration[currentArrayCounter] = currentDuration;
    }


    public static int MoveY(Transform trans, SimpleTweenEaseMethod sTEM, SimpleTweenEaseType sTET,
        float currentDuration, float totalDuration, float startValue, float changeInAmount, Action actionOnComplete)
    {
        CheckIfArrayCounterResetAndSetCurrentDuration(currentDuration);

        cRTweenArray[currentArrayCounter] = CRMoveY(trans, GetEasingFunc(sTET, sTEM),
            totalDuration, actionOnComplete, currentArrayCounter, startValue, changeInAmount, trans.position);
        simpleTween.StartCoroutine(cRTweenArray[currentArrayCounter]);

        return GetCurrentTweenID();
    }


    private static IEnumerator CRMoveY(Transform trans, Func<float, float, float> func,
        float totalDuration, Action actionOnComplete, int arrayNumber,
        float valueToTweenFrom, float changeInAmount, Vector3 startVector3)
    {
        while (true)
        {
            while (arrayOfPause[arrayNumber])
            {
                yield return null;
            }
            arrayOfCurrentDuration[arrayNumber] += Time.deltaTime;
            if (arrayOfCurrentDuration[arrayNumber] >= totalDuration)
            {
                trans.position = new Vector3(startVector3.x, valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount), startVector3.z);
                actionOnComplete();
                yield break;
            }
            else
            {
                trans.position = new Vector3(startVector3.x, valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount), startVector3.z);
            }
            yield return null;
        }
    }


    public static int MoveX(Transform trans, SimpleTweenEaseMethod sTEM, SimpleTweenEaseType sTET,
        float currentDuration, float totalDuration, float startValue, float changeInAmount, Action actionOnComplete)
    {
        CheckIfArrayCounterResetAndSetCurrentDuration(currentDuration);

        cRTweenArray[currentArrayCounter] = CRMoveX(trans, GetEasingFunc(sTET, sTEM),
         totalDuration, actionOnComplete, currentArrayCounter, trans.position.x, changeInAmount, trans.position);
        simpleTween.StartCoroutine(cRTweenArray[currentArrayCounter]);

        return GetCurrentTweenID();
    }


    private static IEnumerator CRMoveX(Transform trans, Func<float, float, float> func,
        float totalDuration, Action actionOnComplete, int arrayNumber,
        float valueToTweenFrom, float changeInAmount, Vector3 startVector3)
    {
        while (true)
        {
            while (arrayOfPause[arrayNumber])
            {
                yield return null;
            }
            arrayOfCurrentDuration[arrayNumber] += Time.deltaTime;
            if (arrayOfCurrentDuration[arrayNumber] >= totalDuration)
            {
                trans.position = new Vector3(valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount), startVector3.y, startVector3.z);
                actionOnComplete();
                yield break;
            }
            else
            {
                trans.position = new Vector3(valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount), startVector3.y, startVector3.z);
            }
            yield return null;
        }
    }


    public static int MoveZ(Transform trans, SimpleTweenEaseMethod sTEM, SimpleTweenEaseType sTET,
        float currentDuration, float totalDuration, float startValue, float changeInAmount, Action actionOnComplete)
    {
        CheckIfArrayCounterResetAndSetCurrentDuration(currentDuration);

        cRTweenArray[currentArrayCounter] = CRMoveZ(trans, GetEasingFunc(sTET, sTEM),
            totalDuration, actionOnComplete, currentArrayCounter, trans.position.z, changeInAmount, trans.position);
        simpleTween.StartCoroutine(cRTweenArray[currentArrayCounter]);

        return GetCurrentTweenID();
    }


    private static IEnumerator CRMoveZ(Transform trans, Func<float, float, float> func,
        float totalDuration, Action actionOnComplete, int arrayNumber,
        float valueToTweenFrom, float changeInAmount, Vector3 startVector3)
    {
        while (true)
        {
            while (arrayOfPause[arrayNumber])
            {
                yield return null;
            }
            arrayOfCurrentDuration[arrayNumber] += Time.deltaTime;
            if (arrayOfCurrentDuration[arrayNumber] >= totalDuration)
            {
                trans.position = new Vector3(startVector3.x, startVector3.y, valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount));
                actionOnComplete();
                yield break;
            }
            else
            {
                trans.position = new Vector3(startVector3.x, startVector3.y, valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount));
            }
            yield return null;
        }
    }


    public static int RotateY(Transform trans, SimpleTweenEaseMethod sTEM, SimpleTweenEaseType sTET,
       float currentDuration, float totalDuration, float startValue, float changeInAmount, Action actionOnComplete)
    {
        CheckIfArrayCounterResetAndSetCurrentDuration(currentDuration);

        cRTweenArray[currentArrayCounter] = CRRotateY(trans, GetEasingFunc(sTET, sTEM),
            totalDuration, actionOnComplete, currentArrayCounter, trans.localEulerAngles.y, changeInAmount, trans.localEulerAngles);
        simpleTween.StartCoroutine(cRTweenArray[currentArrayCounter]);

        return GetCurrentTweenID();
    }


    private static IEnumerator CRRotateY(Transform trans, Func<float, float, float> func,
        float totalDuration, Action actionOnComplete, int arrayNumber,
        float valueToTweenFrom, float changeInAmount, Vector3 startVector3)
    {
        while (true)
        {
            while (arrayOfPause[arrayNumber])
            {
                yield return null;
            }
            arrayOfCurrentDuration[arrayNumber] += Time.deltaTime;
            if (arrayOfCurrentDuration[arrayNumber] >= totalDuration)
            {
                trans.localEulerAngles = new Vector3(startVector3.x, valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount), startVector3.z);
                actionOnComplete();
                yield break;
            }
            else
            {
                trans.localEulerAngles = new Vector3(startVector3.x, valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount), startVector3.z);
            }
            yield return null;
        }
    }


    public static int RotateX(Transform trans, SimpleTweenEaseMethod sTEM, SimpleTweenEaseType sTET,
        float currentDuration, float totalDuration, float startValue, float changeInAmount, Action actionOnComplete)
    {
        CheckIfArrayCounterResetAndSetCurrentDuration(currentDuration);

        cRTweenArray[currentArrayCounter] = CRRotateX(trans, GetEasingFunc(sTET, sTEM),
            totalDuration, actionOnComplete, currentArrayCounter, trans.localEulerAngles.x, changeInAmount, trans.localEulerAngles);
        simpleTween.StartCoroutine(cRTweenArray[currentArrayCounter]);

        return GetCurrentTweenID();
    }


    private static IEnumerator CRRotateX(Transform trans, Func<float, float, float> func,
        float totalDuration, Action actionOnComplete, int arrayNumber,
        float valueToTweenFrom, float changeInAmount, Vector3 startVector3)
    {
        while (true)
        {
            while (arrayOfPause[arrayNumber])
            {
                yield return null;
            }
            arrayOfCurrentDuration[arrayNumber] += Time.deltaTime;
            if (arrayOfCurrentDuration[arrayNumber] >= totalDuration)
            {
                trans.localEulerAngles = new Vector3(valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount), startVector3.y, startVector3.z);
                actionOnComplete();
                yield break;
            }
            else
            {
                trans.localEulerAngles = new Vector3(valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount), startVector3.y, startVector3.z);
            }
            yield return null;
        }
    }


    public static int RotateZ(Transform trans, SimpleTweenEaseMethod sTEM, SimpleTweenEaseType sTET,
        float currentDuration, float totalDuration, float startValue, float changeInAmount, Action actionOnComplete)
    {
        CheckIfArrayCounterResetAndSetCurrentDuration(currentDuration);

        cRTweenArray[currentArrayCounter] = CRRotateZ(trans, GetEasingFunc(sTET, sTEM),
            totalDuration, actionOnComplete, currentArrayCounter, trans.localEulerAngles.z, changeInAmount, trans.localEulerAngles);
        simpleTween.StartCoroutine(cRTweenArray[currentArrayCounter]);

        return GetCurrentTweenID();
    }


    private static IEnumerator CRRotateZ(Transform trans, Func<float, float, float> func,
        float totalDuration, Action actionOnComplete, int arrayNumber,
        float valueToTweenFrom, float changeInAmount, Vector3 startVector3)
    {
        while (true)
        {
            while (arrayOfPause[arrayNumber])
            {
                yield return null;
            }
            arrayOfCurrentDuration[arrayNumber] += Time.deltaTime;
            if (arrayOfCurrentDuration[arrayNumber] >= totalDuration)
            {
                trans.localEulerAngles = new Vector3(startVector3.x, startVector3.y, valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount));
                actionOnComplete();
                yield break;
            }
            else
            {
                trans.localEulerAngles = new Vector3(startVector3.x, startVector3.y, valueToTweenFrom + (func(arrayOfCurrentDuration[arrayNumber], totalDuration) * changeInAmount));
            }
            yield return null;
        }
    }


    private static Func<float, float, float> GetEasingFunc(SimpleTweenEaseType easeType, SimpleTweenEaseMethod easeMethod)
    {
        switch (easeType)
        {
            case SimpleTweenEaseType.Quadratic:
                switch (easeMethod)
                {
                    case SimpleTweenEaseMethod.In:
                        return Quadratic.EaseIn;
                    case SimpleTweenEaseMethod.Out:
                        return Quadratic.EaseOut;
                    case SimpleTweenEaseMethod.InOut:
                        return Quadratic.EaseInOut;
                }
                break;
            case SimpleTweenEaseType.Quartic:
                switch (easeMethod)
                {
                    case SimpleTweenEaseMethod.In:
                        return Quartic.EaseIn;
                    case SimpleTweenEaseMethod.Out:
                        return Quartic.EaseOut;
                    case SimpleTweenEaseMethod.InOut:
                        return Quartic.EaseInOut;
                }
                break;
            case SimpleTweenEaseType.Quintic:
                switch (easeMethod)
                {
                    case SimpleTweenEaseMethod.In:
                        return Quintic.EaseIn;
                    case SimpleTweenEaseMethod.Out:
                        return Quintic.EaseOut;
                    case SimpleTweenEaseMethod.InOut:
                        return Quintic.EaseInOut;
                }
                break;
            case SimpleTweenEaseType.Sinus:
                switch (easeMethod)
                {
                    case SimpleTweenEaseMethod.In:
                        return Sinus.EaseIn;
                    case SimpleTweenEaseMethod.Out:
                        return Sinus.EaseOut;
                    case SimpleTweenEaseMethod.InOut:
                        return Sinus.EaseInOut;
                }
                break;
            case SimpleTweenEaseType.Linear:
                return Linear.EaseIn;
            case SimpleTweenEaseType.Exponential:
                switch (easeMethod)
                {
                    case SimpleTweenEaseMethod.In:
                        return Exponential.EaseIn;
                    case SimpleTweenEaseMethod.Out:
                        return Exponential.EaseOut;
                    case SimpleTweenEaseMethod.InOut:
                        return Exponential.EaseInOut;
                }
                break;
            case SimpleTweenEaseType.Cubic:
                switch (easeMethod)
                {
                    case SimpleTweenEaseMethod.In:
                        return Cubic.EaseIn;
                    case SimpleTweenEaseMethod.Out:
                        return Cubic.EaseOut;
                    case SimpleTweenEaseMethod.InOut:
                        return Cubic.EaseInOut;
                }
                break;
            case SimpleTweenEaseType.Circular:
                switch (easeMethod)
                {
                    case SimpleTweenEaseMethod.In:
                        return Circular.EaseIn;
                    case SimpleTweenEaseMethod.Out:
                        return Circular.EaseOut;
                    case SimpleTweenEaseMethod.InOut:
                        return Circular.EaseInOut;
                }
                break;
            case SimpleTweenEaseType.Back:
                switch (easeMethod)
                {
                    case SimpleTweenEaseMethod.In:
                        return Back.EaseIn;
                    case SimpleTweenEaseMethod.Out:
                        return Back.EaseOut;
                    case SimpleTweenEaseMethod.InOut:
                        return Back.EaseInOut;
                }
                break;
            case SimpleTweenEaseType.Bounce:
                switch (easeMethod)
                {
                    case SimpleTweenEaseMethod.In:
                        return Bounce.EaseIn;
                    case SimpleTweenEaseMethod.Out:
                        return Bounce.EaseOut;
                    case SimpleTweenEaseMethod.InOut:
                        return Bounce.EaseInOut;
                }
                break;
            case SimpleTweenEaseType.Elastic:
                switch (easeMethod)
                {
                    case SimpleTweenEaseMethod.In:
                        return Elastic.EaseIn;
                    case SimpleTweenEaseMethod.Out:
                        return Elastic.EaseOut;
                    case SimpleTweenEaseMethod.InOut:
                        return Elastic.EaseInOut;
                }
                break;
        }
        return null;
    }
}


public static class Back
{
    public static float EaseIn(float t, float d)
    {
        float s = 1.70158f;
        return (t /= d) * t * ((s + 1) * t - s);
    }

    public static float EaseOut(float t, float d)
    {
        float s = 1.70158f;
        return ((t = t / d - 1) * t * ((s + 1) * t + s) + 1);
    }

    public static float EaseInOut(float t, float d)
    {
        float s = 1.70158f;
        if ((t /= d * 0.5f) < 1) return 0.5f * (t * t * (((s *= (1.525f)) + 1) * t - s));
        return 0.5f * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2);
    }
}


public static class Bounce
{
    public static float EaseOut(float t, float d)
    {
        if ((t /= d) < (1 / 2.75f))
        {
            return (7.5625f * t * t);
        }
        else if (t < (2 / 2.75f))
        {
            return (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f);
        }
        else if (t < (2.5f / 2.75f))
        {
            return (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f);
        }
        else
        {
            return (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f);
        }
    }

    public static float EaseIn(float t, float d)
    {
        return 1 - EaseOut(d - t, d);
    }

    public static float EaseInOut(float t, float d)
    {
        if (t < d * 0.5f) return EaseIn(t * 2, d) * .5f;
        else return EaseOut(t * 2 - d, d) * .5f + .5f;
    }

}


public static class Sinus
{
    public static float EaseIn(float t, float d)
    {
        return -(float)Math.Cos(t / d * ((float)Math.PI * 0.5f)) + 1;
    }

    public static float EaseOut(float t, float d)
    {
        return (float)Math.Sin(t / d * ((float)Math.PI * 0.5f));
    }

    public static float EaseInOut(float t, float d)
    {
        return -0.5f * ((float)Math.Cos((float)Math.PI * t / d) - 1);
    }
}


public static class Quintic
{
    public static float EaseIn(float t, float d)
    {
        return (t /= d) * t * t * t * t;
    }

    public static float EaseOut(float t, float d)
    {
        return ((t = t / d - 1) * t * t * t * t + 1);
    }

    public static float EaseInOut(float t, float d)
    {
        if ((t /= d * 0.5f) < 1) return 0.5f * t * t * t * t * t;
        return 0.5f * ((t -= 2) * t * t * t * t + 2);
    }

}


public static class Quartic
{
    public static float EaseIn(float t, float d)
    {
        return (t /= d) * t * t * t;
    }

    public static float EaseOut(float t, float d)
    {
        return -((t = t / d - 1) * t * t * t - 1);
    }

    public static float EaseInOut(float t, float d)
    {
        if ((t /= d * 0.5f) < 1) return 0.5f * t * t * t * t;
        return -0.5f * ((t -= 2) * t * t * t - 2);
    }

}


public static class Elastic
{
    public static float EaseIn(float t, float d)
    {
        if (t == 0) return 0; if ((t /= d) == 1) return 1; float p = d * .3f;

        float a = 1;
        float s = p / 4;

        return -(a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p));
    }

    public static float EaseOut(float t, float d)
    {
        if (t == 0) return 0; if ((t /= d) == 1) return 1; float p = d * .3f;

        float a = 1;
        float s = p / 4;

        return (a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) + 1);
    }

    public static float EaseInOut(float t, float d)
    {
        if (t == 0) return 0; if ((t /= d * 0.5f) == 2) return 1; float p = d * (.3f * 1.5f);

        float a = 1;
        float s = p / 4;

        if (t < 1) return -.5f * (a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p));
        return a * (float)Math.Pow(2, -10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) * .5f + 1;
    }
}


public static class Quadratic
{
    public static float EaseIn(float t, float d)
    {
        return (t /= d) * t;
    }

    public static float EaseOut(float t, float d)
    {
        return -(t /= d) * (t - 2);
    }

    public static float EaseInOut(float t, float d)
    {
        if ((t /= d * 0.5f) < 1) return 0.5f * t * t;
        return -0.5f * ((--t) * (t - 2) - 1);
    }
}


public static class Linear
{
    public static float EaseIn(float t, float d)
    {
        return t / d;
    }
}


public static class Exponential
{
    public static float EaseIn(float t, float d)
    {
        return (t == 0) ? 0 : (float)Math.Pow(2, 10 * (t / d - 1));
    }

    public static float EaseOut(float t, float d)
    {
        return (t == d) ? 1 : (-(float)Math.Pow(2, -10 * t / d) + 1);
    }

    public static float EaseInOut(float t, float d)
    {
        if (t == 0) return 0;
        if (t == d) return 1;
        if ((t /= d * 0.5f) < 1) return 0.5f * (float)Math.Pow(2, 10 * (t - 1));
        return 0.5f * (-(float)Math.Pow(2, -10 * --t) + 2);
    }

}


public static class Cubic
{
    public static float EaseIn(float t, float d)
    {
        return (t /= d) * t * t;
    }

    public static float EaseOut(float t, float d)
    {
        return ((t = t / d - 1) * t * t + 1);
    }

    public static float EaseInOut(float t, float d)
    {
        if ((t /= d * 0.5f) < 1) return 0.5f * t * t * t;
        return 0.5f * ((t -= 2) * t * t + 2);
    }

}


public static class Circular
{
    public static float EaseIn(float t, float d)
    {
        return -((float)Math.Sqrt(1 - (t /= d) * t) - 1);
    }

    public static float EaseOut(float t, float d)
    {
        return (float)Math.Sqrt(1 - (t = t / d - 1) * t);
    }

    public static float EaseInOut(float t, float d)
    {
        if ((t /= d * 0.5f) < 1) return -0.5f * ((float)Math.Sqrt(1 - t * t) - 1);
        return 0.5f * ((float)Math.Sqrt(1 - (t -= 2) * t) + 1);
    }
}