// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-20 18:21:47
// ========================================================
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CreateTweener : MonoBehaviour
{
    public GameObject cube;

    public AudioSource m_Audio;

    public Light m_Light;

    public LineRenderer m_LineRender;

    public Material m_Material;

    public Rigidbody m_Rigidbody;

    public Rigidbody2D m_Rigidbody2D;

    public SpriteRenderer m_SpriteRender;

    public TrailRenderer m_TrailRenderer;

    public CanvasGroup m_CanvasGroup;

    public Image m_Image;

    public RectTransform m_RectTransform;

    public ScrollRect m_ScrollRect;

    public Slider m_Slider;

    public Text m_Text;

    // Start is called before the first frame update
    void Start()
    {
        // 普通方式
        //Vector3 myVector = Vector3.zero;
        //DOTween.To(() => myVector, x =>{Debug.Log(x);}, new Vector3(3, 8, 4), 100);
        //DOTween.To(() => myVector, x => { Debug.Log(x); }, new Vector3(3, 8, 4), 100).From();

        // 简短方式
        //cube.transform.DOMove(new Vector3(2, 2, 2), 2);
        //cube.transform.DOMove(new Vector3(2, 2, 2), 2).From();

        // AudioSource
        //m_Audio.DOFade(0, 3);
        //m_Audio.DOPitch(0.5f, 3);

        // Camera
        //Camera.main.DOAspect(10, 5);
        //Camera.main.DOColor(Color.red, 5);
        //Camera.main.DOFarClipPlane(1, 5);
        //Camera.main.DOFieldOfView(1, 5);
        //Camera.main.DONearClipPlane(1, 5);
        //Camera.main.DOOrthoSize(1, 5);
        //Camera.main.DOPixelRect(new Rect(new Vector2(0,0), new Vector2(800, 640)), 5);
        //Camera.main.DORect(new Rect(new Vector2(0, 0), new Vector2(0.8f, 1.5f)), 5);
        //Camera.main.DOShakePosition(5, 10, 10, 45, true);
        //Camera.main.DOShakeRotation(5, 10, 10, 45, true);

        // Light
        //m_Light.DOColor(Color.red, 5);
        //m_Light.DOColor(Color.yellow, 5);
        //m_Light.DOIntensity(0.5f, 5);
        //m_Light.DOShadowStrength(0, 5);
        //m_Light.DOBlendableColor(Color.red, 5);
        //m_Light.DOBlendableColor(Color.yellow, 5);

        // LineRender
        //m_LineRender.DOColor(new Color2(Color.white, Color.white), new Color2(Color.green, Color.black), 1);

        // Material
        //m_Material.DOColor(Color.red, 5);
        //m_Material.DOColor(Color.green, "_Color", 1);
        //m_Material.DOColor(Color.yellow, Shader.PropertyToID("_Color"), 1);
        //m_Material.DOFade(0, 5);
        //m_Material.DOFade(1, "_Color", 1);
        //m_Material.DOFade(0, Shader.PropertyToID("_Color"), 1);
        //m_Material.DOFloat(1, "_Glossiness", 1);
        //m_Material.DOFloat(0, Shader.PropertyToID("_Glossiness"), 1);

        //Gradient gradient = new Gradient();
        //gradient.colorKeys = new GradientColorKey[] {
        //    new GradientColorKey(Color.red, 0),
        //    new GradientColorKey(Color.yellow, 0.5f),
        //    new GradientColorKey(Color.black, 1),
        //};
        //m_Material.DOGradientColor(gradient, 5);
        //m_Material.DOGradientColor(gradient, "_Color", 5);

        //m_Material.DOOffset(new Vector2(1,1), 5);
        //m_Material.DOTiling(new Vector2(1, 1), 5);

        //m_Material.DOVector(new Vector4(1, 1, 1, 1), "_V2", 5);
        //m_Material.DOVector(new Vector4(0, 1, 1, 1), Shader.PropertyToID("_V2"), 5);

        //m_Material.DOBlendableColor(Color.red, 5);
        //m_Material.DOBlendableColor(Color.blue, 5);

        // Rigidbody
        // 可穿透
        //m_Rigidbody.DOMove(new Vector3(0, -10, 0), 5);
        //m_Rigidbody.DOMoveX(10, 5);
        //m_Rigidbody.DOMoveX(10, 5, true);
        //m_Rigidbody.DOJump(new Vector3(0, 0, 0), 10, 10, 5);
        //m_Rigidbody.DORotate(new Vector3(360, 0, 0), 5, RotateMode.WorldAxisAdd);
        //m_Rigidbody.transform.DOLookAt(new Vector3(0, 0, 0), 5);

        //Vector3[] pathArr = new Vector3[] {
        //    new Vector3(0, 1, 0),
        //    new Vector3(0, 3, 0),
        //    new Vector3(0, 1, 0),
        //    new Vector3(0, -3, 0),
        //    new Vector3(0, 0, 0),
        //    new Vector3(0, 5, 0)
        //};
        //m_Rigidbody.DOPath(pathArr, 5);
        //m_Rigidbody.DOPath(pathArr, 5, PathType.CubicBezier);

        // Rigidbody2D
        // 可穿透
        //m_Rigidbody2D.DOMove(new Vector2(0, -10), 5);
        //m_Rigidbody2D.DOMoveX(10, 5);
        //m_Rigidbody2D.DOMoveX(10, 5, true);
        //m_Rigidbody2D.DOJump(new Vector3(0, 0, 0), 10, 10, 5);
        //m_Rigidbody2D.DORotate(360, 5);

        //Vector3[] pathArr = new Vector3[] {
        //    new Vector3(0, 1, 0),
        //    new Vector3(0, 3, 0),
        //    new Vector3(0, 1, 0),
        //    new Vector3(0, -3, 0),
        //    new Vector3(0, 0, 0),
        //    new Vector3(0, 5, 0)
        //};
        //m_Rigidbody2D.transform.DOPath(pathArr, 5);
        //m_Rigidbody2D.transform.DOPath(pathArr, 5, PathType.CubicBezier);

        // SpriteRender
        //m_SpriteRender.DOColor(Color.red, 5);
        //m_SpriteRender.DOFade(0, 5);
        //Gradient gradient = new Gradient();
        //gradient.colorKeys = new GradientColorKey[] {
        //    new GradientColorKey(Color.red, 0),
        //    new GradientColorKey(Color.yellow, 0.5f),
        //    new GradientColorKey(Color.black, 1),
        //};
        //m_SpriteRender.DOGradientColor(gradient, 5);
        //m_SpriteRender.DOBlendableColor(Color.red, 5);
        //m_SpriteRender.DOBlendableColor(Color.blue, 5);

        // TrailRenderer
        //m_TrailRenderer.DOResize(1, 0.5f, 5);
        //m_TrailRenderer.DOTime(1, 5);

        // Transform
        //cube.transform.DOScale(Vector3.one * 2, 5);
        //cube.transform.DOPunchPosition(Vector3.one, 5);
        //cube.transform.DOPunchRotation(new Vector3(10,0,0), 5);
        //cube.transform.DOPunchScale(Vector3.one, 5);
        //cube.transform.DOShakePosition(5);
        //cube.transform.DOShakeRotation(5);
        //cube.transform.DOShakeScale(5);

        //cube.transform.DOMove(new Vector3(3, 3, 0), 3);
        //cube.transform.DOMove(new Vector3(-3, 0, 0), 1f).SetLoops(3, LoopType.Yoyo);

        //cube.transform.DOBlendableMoveBy(new Vector3(3, 3, 0), 3);
        //cube.transform.DOBlendableMoveBy(new Vector3(-3, 0, 0), 1f).SetLoops(3, LoopType.Yoyo);

        // CanvasGroup
        //m_CanvasGroup.DOFade(0, 5);

        // Image
        //m_Image.DOColor(Color.red, 5);
        //m_Image.DOFade(0, 5);
        //m_Image.DOFillAmount(0, 5);

        //Gradient gradient = new Gradient();
        //gradient.colorKeys = new GradientColorKey[] {
        //    new GradientColorKey(Color.red, 0),
        //    new GradientColorKey(Color.yellow, 0.5f),
        //    new GradientColorKey(Color.black, 1),
        //};
        //m_Image.DOGradientColor(gradient, 5);

        //m_Image.DOBlendableColor(Color.red, 5);
        //m_Image.DOBlendableColor(Color.blue, 5);

        // RectTransform
        //m_RectTransform.DOAnchorMax(new Vector2(1, 1), 5);
        //m_RectTransform.DOAnchorPos(new Vector2(0, 0), 5);
        //m_RectTransform.DOAnchorPos3D(new Vector3(0, 0, 100), 5);
        //m_RectTransform.DOJumpAnchorPos(Vector2.zero, 10, 5, 5);
        //m_RectTransform.DOSizeDelta(new Vector2(200, 300), 5);

        // ScrollRect
        //m_ScrollRect.DONormalizedPos(new Vector2(0, 1), 5).From();
        //m_ScrollRect.DOVerticalNormalizedPos(1, 5).From();

        // Slider
        //m_Slider.DOValue(0.5f, 5);

        // Text
        //m_Text.DOColor(Color.white, 5);
        //m_Text.DOFade(0, 5);
        //m_Text.DOText("dfjklsdfjklsdjflksdjfkldsjfklsdjfkldjflkd", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
