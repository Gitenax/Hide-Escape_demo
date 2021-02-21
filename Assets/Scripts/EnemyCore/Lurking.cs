using UnityEngine;
using DG.Tweening;

namespace EnemyCore
{
    public enum LurkingMode
    {
        Vibration,
        RotateAround
    }
    
    // Имитация рысканья вокруг
    public class Lurking : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private LurkingMode _lurkingMode;
#pragma warning restore CS0649
        
        void Start()
        {
            switch (_lurkingMode)
            {
                case LurkingMode.Vibration:
                    transform.DOLocalRotate(-transform.right, 5f).SetEase(Ease.InOutFlash).SetLoops(1000, LoopType.Yoyo);
                    break;
                case LurkingMode.RotateAround:
                    transform.DOLocalRotate(Vector3.left, 10f, RotateMode.FastBeyond360).SetEase(Ease.InOutFlash).SetLoops(1000, LoopType.Restart);
                    break;
            }
        }

    }
}
