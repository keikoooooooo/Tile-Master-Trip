using System.Collections;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
   private readonly float rotationSpeed = 2f; // tốc độ xoay
   
   private Quaternion initialRotation;
   private Quaternion targetRotation;
   
   private Coroutine _rotateObjectCoroutine;


   public void StartRotate(Quaternion _current, Quaternion _target)
   {
      initialRotation = _current;
      targetRotation = _target;

      if(_rotateObjectCoroutine != null) 
         StopCoroutine(_rotateObjectCoroutine);
      _rotateObjectCoroutine = StartCoroutine(RotateObjectCoroutine());
   }
   
   
   private IEnumerator RotateObjectCoroutine()
   {
      float t = 0;
      while (t < 1)
      {
         t += Time.deltaTime * rotationSpeed;
         transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
         yield return null;
      }
      transform.rotation = targetRotation; // luôn đảm bảo nó sẽ hoàn toàn xoay về phía mục tiêu
   }


}

