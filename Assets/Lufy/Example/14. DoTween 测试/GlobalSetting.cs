// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-24 11:14:18
// ========================================================
using DG.Tweening;
using UnityEngine;

public class GlobalSetting : MonoBehaviour
{
    public GameObject cube;

    public GameObject otherCube;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.logBehaviour = LogBehaviour.Verbose;

        //DOTween.useSmoothDeltaTime = true;
        //DOTween.maxSmoothUnscaledTime = 0.5f;

        //DOTween.timeScale = 1f;

        //DOTween.defaultAutoPlay = AutoPlay.None;
        //DOTween.defaultLoopType = LoopType.Yoyo;
        //DOTween.defaultUpdateType = UpdateType.Manual;

        //DOTween.To(() => 1, (x) =>
        //  {
        //      Debug.Log(x);
        //  }, 10, 1).SetEase(Ease.Linear);

        //Time.timeScale = 0;
        //Tweener tweener = cube.transform.DOMove(Vector3.one * 2, 5);
        //Tweener tweener = cube.transform.DOMove(Vector3.one * 2, 5).SetUpdate(true);
        //tweener.Pause();
        //tweener.Play();

        #region Tweener and Sequence settings

        //Tweener tweener = cube.transform.DOMove(Vector3.one * 2, 5);
        //tweener.timeScale = 0.1f;

        //cube.transform.DOScale(2, 1); 
        //cube.transform.DOScale(2, 1).SetAs(tweener);

        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.Flash, 16, 1);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.Flash, 16, -1);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InBack);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InBounce);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InCirc);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InCubic);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InElastic);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InExpo);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InFlash);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InQuad);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InQuart);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InQuint);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InSine);

        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutBack);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutBounce);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutCirc);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutCubic);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutElastic);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutExpo);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutFlash);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutQuad);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutQuart);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutQuint);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.InOutSine);

        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.Linear);

        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutBack);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutBounce);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutCirc);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutCubic);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutElastic);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutExpo);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutFlash);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutQuad);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutQuart);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutQuint);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.OutSine);

        //cube.transform.DOMove(Vector3.one * 2, 5).SetEase(Ease.Unset);

        //cube.transform.DOMove(Vector3.one * 2, 5).SetId(1000);
        //DOTween.Pause(1000);

        //Tweener tweener = cube.transform.DOMove(Vector3.one * 2, 5).SetId(1000);
        //tweener.SetLink(otherCube, LinkBehaviour.KillOnDisable);

        //Tweener tweener = cube.transform.DOMove(Vector3.one * 2, 5).SetId(1000);
        //tweener.SetLoops(-1, LoopType.Restart);
        //tweener.SetLoops(-1, LoopType.Yoyo);
        //tweener.SetLoops(-1, LoopType.Incremental);

        //cube.transform.DOMove(Vector3.one * 2, 5).SetRelative(true);

        //DOTween.To(() => cube.transform.position, (pos) =>
        //  {
        //      cube.transform.position = pos;
        //  }, Vector3.one * 2, 5).SetTarget(cube);

        //cube.transform.DOMove(Vector3.one * 2, 5).SetUpdate(UpdateType.Fixed, true);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetUpdate(UpdateType.Late, true);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetUpdate(UpdateType.Manual, true);
        //cube.transform.DOMove(Vector3.one * 2, 5).SetUpdate(UpdateType.Normal, true);

        //cube.transform.DOMove(Vector3.one * 2, 5)
        //    .OnComplete(() =>
        //    {
        //        Debug.Log("complete");
        //    })
        //    .OnKill(()=>
        //    {
        //        Debug.Log("kill");
        //    })
        //    .OnPlay(()=>
        //    {
        //        Debug.Log("play");
        //    })
        //    .OnPause(() =>
        //    {
        //        Debug.Log("pause");
        //    })
        //    .OnRewind(() =>
        //    {
        //        Debug.Log("rewind");
        //    })
        //    .OnStart(() =>
        //    {
        //        Debug.Log("start");
        //    })
        //    .OnPause(() =>
        //    {
        //        Debug.Log("pause");
        //    })
        //    .OnStepComplete(() =>
        //    {
        //        Debug.Log("step");
        //    })
        //    .OnUpdate(() =>
        //    {
        //        Debug.Log("update");
        //    })
        //    .OnWaypointChange((index) =>
        //    {
        //        Debug.Log("point change " + index);
        //    });

        #endregion

        #region Tweener - specific settings and options

        //cube.transform.DOMove(Vector3.one * 2, 5).From();
        //cube.transform.DOMove(Vector3.one * 2, 5).From(true);

        //cube.transform.DOMove(Vector3.one * 2, 5).SetDelay(1);

        //cube.transform.DOMove(Vector3.one * 2, 5).SetSpeedBased(true);
        //cube.transform.DOMove(Vector3.one * 2, 0.1f).SetSpeedBased(true);

        //Tweener tweener = cube.transform.DOMove(Vector3.one * 2, 5);
        //TweenParams tParms = new TweenParams().SetLoops(-1).SetEase(Ease.OutElastic);
        //tweener.SetAs(tParms);

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
