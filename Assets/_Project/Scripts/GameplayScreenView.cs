using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

/// <summary>M0-compatible UGUI presentation and runtime root for the deterministic M1 prototype.</summary>
public sealed class GameplayScreenView : MonoBehaviour
{
    private const float W = 1080f;
    private const float H = 1920f;
    private Sprite rounded, glow, ring, vessel, selector;
    private Sprite[] fragments, colourButtons, topIcons;
    private readonly List<Image> boardImages = new List<Image>();
    private readonly Image[] chargeImages = new Image[4];
    private readonly Button[] chargeButtons = new Button[4];
    private readonly Image[] starIcons = new Image[3];
    private M1BoardModel model;
    private RectTransform pile;
    private Image selectedGlow, selectedRing, nextIndicator;
    private Text levelLabel;
    private int highlightedSlot;

    private void Start()
    {
        rounded = Rounded(160, 34); glow = Glow(192); ring = Ring(192);
        vessel = Load("Gameplay/glass_container");
        selector = Load("Gameplay/color_selector") ?? Capsule(320, 96);
        fragments = new[]
        {
            Load("Gameplay/blob_01"), Load("Gameplay/blob_02"), Load("Gameplay/blob_04"), Load("Gameplay/blob_05"),
            Load("Gameplay/blob_06"), Load("Gameplay/blob_07"), Load("Gameplay/blob_08"), Load("Gameplay/blob_09")
        };
        colourButtons = Sheet("Gameplay/color_buttons", 4, 1, 4);
        topIcons = Sheet("Gameplay/top_icons", 3, 1, 3);
        model = new M1BoardModel();
        Build();
    }

    private void Update()
    {
        if (model == null || model.State != M1TurnState.PlayerReady)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            TryPressCharge(Mouse.current.position.ReadValue());

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            TryPressCharge(Touchscreen.current.primaryTouch.position.ReadValue());
    }

    private void TryPressCharge(Vector2 screenPosition)
    {
        for (var i = 0; i < chargeImages.Length; i++)
        {
            if (chargeImages[i] != null && RectTransformUtility.RectangleContainsScreenPoint(chargeImages[i].rectTransform, screenPosition, null))
            {
                OnChargePressed(i);
                return;
            }
        }
    }

    private void Build()
    {
        var canvasObject = new GameObject("Gameplay Screen", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
        canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(W, H); scaler.matchWidthOrHeight = .5f;
        var root = canvasObject.GetComponent<RectTransform>();
        EnsureEventSystem();
        var background = Image("Cold White Background", root, Background(), Color.white); Stretch(background.rectTransform);
        Top(root); Vessel(root); Selector(root); Next(root); RenderAll();
    }

    private void Top(RectTransform root)
    {
        var top = Layer("Top UI", root); Rect(top, Vector2.zero, new Vector2(900, 84)); top.anchoredPosition = new Vector2(0, 806);
        var back = Image("Back", top, topIcons[0], new Color(.20f, .25f, .30f)); Rect(back.rectTransform, new Vector2(-443, 0), new Vector2(44, 44));
        levelLabel = Text("Level", top, "Level 1", 35, TextAnchor.MiddleCenter, new Color(.15f, .19f, .24f), Vector2.zero, new Vector2(280, 58));
        var settings = Image("Settings", top, topIcons[2], new Color(.20f, .25f, .30f)); Rect(settings.rectTransform, new Vector2(443, 0), new Vector2(49, 49));
        var stars = Layer("Empty Stars", root); Rect(stars, new Vector2(0, 691), new Vector2(220, 60));
        var positions = new[] { -68f, 0f, 68f };
        for (var i = 0; i < positions.Length; i++)
        {
            starIcons[i] = Image("Required Star", stars, topIcons[1], new Color(.24f, .29f, .34f));
            Rect(starIcons[i].rectTransform, new Vector2(positions[i], 0), new Vector2(48, 48));
        }
    }

    private void Vessel(RectTransform root)
    {
        // 1040px sprite includes transparent exterior padding; visible shell resolves to canonical 0.80W.
        var field = Layer("Layered Glass Container", root); Rect(field, new Vector2(0, 53), new Vector2(1040, 950));
        var ground = Image("Vessel Ground Shadow", field, glow, new Color(.18f, .24f, .30f, .12f)); Rect(ground.rectTransform, new Vector2(0, -454), new Vector2(850, 80));
        var cavity = Image("Inner Cavity Tint", field, rounded, new Color(.975f, .973f, .968f, .98f)); Rect(cavity.rectTransform, new Vector2(0, 18), new Vector2(820, 780));
        var depth = Image("Inner Bottom Ambient Depth", field, glow, new Color(.31f, .30f, .28f, .09f)); Rect(depth.rectTransform, new Vector2(0, -282), new Vector2(790, 205));
        pile = new GameObject("Dense Dry Fragment Pile", typeof(RectTransform), typeof(RectMask2D)).GetComponent<RectTransform>(); pile.SetParent(field, false); Rect(pile, new Vector2(0, 18), new Vector2(790, 780));
        var inner = Image("Inner Cavity Highlight", field, rounded, new Color(1, 1, 1, .045f)); Rect(inner.rectTransform, new Vector2(0, 84), new Vector2(808, 615));
        var body = Image("Outer Shell Volume", field, vessel, new Color(1f, .97f, .93f, .22f)); Rect(body.rectTransform, new Vector2(0, 4), new Vector2(1040, 950));
        var rim = Image("Glass Rim and Integrated Drain", field, vessel, new Color(1f, .985f, .95f, .44f)); Rect(rim.rectTransform, Vector2.zero, new Vector2(1040, 950));
        var highlight = Image("Outer Rim Highlight", field, vessel, new Color(1, 1, 1, .04f)); Rect(highlight.rectTransform, new Vector2(-3, 8), new Vector2(1024, 930));
    }

    private void Selector(RectTransform root)
    {
        var holder = Layer("Premium Colour Selector", root); Rect(holder, new Vector2(0,-593), new Vector2(972,246));
        var shadow = Image("Selector Soft Shadow", root, glow, new Color(.18f,.16f,.13f,.15f)); Rect(shadow.rectTransform, new Vector2(0,-616), new Vector2(1015,220)); shadow.transform.SetSiblingIndex(holder.GetSiblingIndex());
        var baseVolume = Image("Selector Interior Volume", holder, Capsule(320, 96), new Color(.99f, .982f, .965f, 1f)); Rect(baseVolume.rectTransform, Vector2.zero, new Vector2(972,246));
        var shell = Image("Authored Selector Capsule", holder, selector, new Color(1f,.995f,.982f,.38f)); Rect(shell.rectTransform, Vector2.zero, new Vector2(972,320));
        selectedGlow = Image("Selected Charge Soft Glow", holder, glow, new Color(.24f,.66f,1,.12f)); Rect(selectedGlow.rectTransform, new Vector2(-324,0), new Vector2(184,184));
        selectedRing = Image("Selected Charge Ring", holder, ring, new Color(.87f,.98f,1,1)); Rect(selectedRing.rectTransform, new Vector2(-324,0), new Vector2(204,204));
        var x = new[] { -324f,-108f,108f,324f };
        for (var i = 0; i < 4; i++)
        {
            var slot = i;
            chargeImages[i] = Image("Color Charge", holder, colourButtons[0], Color.white);
            Rect(chargeImages[i].rectTransform, new Vector2(x[i], 0), new Vector2(194, 194));
            chargeImages[i].raycastTarget = true;
            chargeButtons[i] = chargeImages[i].gameObject.AddComponent<Button>();
            chargeButtons[i].transition = Selectable.Transition.None;
            chargeButtons[i].targetGraphic = chargeImages[i];
            chargeButtons[i].onClick.AddListener(() => OnChargePressed(slot));
        }
    }

    private void Next(RectTransform root)
    {
        var shadow = Image("NEXT Soft Shadow", root, glow, new Color(.18f,.16f,.13f,.15f)); Rect(shadow.rectTransform,new Vector2(0,-813),new Vector2(420,135));
        var next = Image("NEXT Pill", root, Capsule(260,88), new Color(.99f,.982f,.965f,1)); Rect(next.rectTransform,new Vector2(0,-799),new Vector2(378,127));
        var rim = Image("NEXT Top Highlight",next.transform,Capsule(260,88),new Color(1,1,1,.34f)); Rect(rim.rectTransform,new Vector2(0,2),new Vector2(366,115));
        nextIndicator=Image("NEXT Colour Indicator",next.transform,rounded,new Color(1,.73f,.11f)); Rect(nextIndicator.rectTransform,new Vector2(-99,0),new Vector2(49,49));
        Text("NEXT Label",next.transform,"NEXT",28,TextAnchor.MiddleLeft,new Color(.16f,.20f,.24f),new Vector2(7,0),new Vector2(132,50));
    }

    private void EnsureEventSystem()
    {
        if (FindAnyObjectByType<EventSystem>() != null) return;
        var eventSystem = new GameObject("M1 Event System", typeof(EventSystem), typeof(InputSystemUIInputModule));
        DontDestroyOnLoad(eventSystem);
    }

    private void OnChargePressed(int slot)
    {
        if (!model.SelectCharge(slot)) return;
        highlightedSlot = slot;
        RenderAll();
        StartCoroutine(ResolveTurn());
    }

    private IEnumerator ResolveTurn()
    {
        yield return new WaitForSeconds(.10f);
        model.Burst(); RenderAll();
        yield return new WaitForSeconds(.28f);
        model.FlowAndSettle(); RenderAll();
        yield return new WaitForSeconds(.24f);
        model.FlowSettling(); RenderAll();
        yield return new WaitForSeconds(.18f);
        model.ClassifyLiquid(); RenderAll();
        yield return new WaitForSeconds(.20f);
        model.BeginFoaming(); RenderAll();
        yield return new WaitForSeconds(.22f);
        model.SolidifyFoam(); RenderAll();
        yield return new WaitForSeconds(.25f);
        model.FinalSettle(); RenderAll();
        yield return new WaitForSeconds(.18f);
        model.FinalizeStars(); RenderAll();
        yield return new WaitForSeconds(.10f);

        var won = model.WinCheck();
        if (!won) model.HandUpdateAndLoseCheck();
        RenderAll();
        Debug.Log("M1 Turn " + model.TurnNumber + ": " + model.State + ", drained=" + model.DrainedThisTurn + ", foam=" + model.FoamCreatedThisTurn + ", dynamicRoute=" + model.DynamicRouteOpenedThisTurn);
    }

    private void RenderAll()
    {
        if (model == null || pile == null) return;
        for (var i = pile.childCount - 1; i >= 0; i--) Destroy(pile.GetChild(i).gameObject);
        boardImages.Clear();

        for (var i = 0; i < model.Solids.Count; i++) RenderSolid(model.Solids[i]);
        for (var i = 0; i < model.Liquids.Count; i++) RenderLiquid(model.Liquids[i]);
        for (var i = 0; i < model.Stars.Count; i++) RenderStar(model.Stars[i]);
        RefreshHand();
        RefreshStars();
        if (levelLabel != null)
            levelLabel.text = model.State == M1TurnState.LevelComplete ? "LEVEL COMPLETE" : model.State == M1TurnState.LevelFailed ? "LEVEL FAILED" : "Level 1";
    }

    private void RenderSolid(M1Fragment solid)
    {
        var position = BoardPosition(solid.Column, solid.Row, solid.VisualSeed);
        var scale = solid.Kind == M1SolidKind.Foam ? .78f : 1f;
        var size = new Vector2(270, 205) * (scale + (solid.VisualSeed % 9) * .012f);
        if (solid.Kind == M1SolidKind.Foam)
        {
            var foamGlow = Image("Foam Return", pile, glow, WithAlpha(ColorFor(solid.Color), .26f));
            Rect(foamGlow.rectTransform, position, size * 1.18f);
        }
        var shadow = Image("Fragment Contact Shadow", pile, glow, new Color(.16f, .14f, .12f, .16f));
        Rect(shadow.rectTransform, position + new Vector2(4, -15), new Vector2(size.x * .84f, size.y * .30f));
        shadow.rectTransform.localEulerAngles = new Vector3(0, 0, Angle(solid.VisualSeed));
        var fragment = Image(solid.Kind == M1SolidKind.Foam ? "Foam Solid Fragment" : "Original Fragment", pile, SpriteFor(solid.Color, solid.VisualSeed), Color.white);
        Rect(fragment.rectTransform, position, size);
        fragment.rectTransform.localEulerAngles = new Vector3(0, 0, Angle(solid.VisualSeed));
        boardImages.Add(fragment);
    }

    private void RenderLiquid(M1Liquid liquid)
    {
        var position = BoardPosition(liquid.Column, liquid.Row, liquid.VisualSeed);
        var halo = Image(model.State == M1TurnState.Foaming ? "Trapped Liquid" : "Flowing Liquid", pile, glow, WithAlpha(ColorFor(liquid.Color), .30f));
        Rect(halo.rectTransform, position + new Vector2(0, -12), new Vector2(220, 180));
        var body = Image("Liquid Mass", pile, rounded, WithAlpha(ColorFor(liquid.Color), .78f));
        Rect(body.rectTransform, position + new Vector2(0, -20), new Vector2(150, 95));
        boardImages.Add(body);
    }

    private void RenderStar(M1Star star)
    {
        if (star.State == M1StarState.Collected) return;
        var position = BoardPosition(star.Column, star.Row, star.Id * 19);
        var color = star.State == M1StarState.Contained ? new Color(1f, .82f, .20f, .82f) : new Color(1f, .77f, .08f, 1f);
        var image = Image(star.State == M1StarState.Contained ? "Contained Star" : "Released Star", pile, topIcons[1], color);
        Rect(image.rectTransform, position, star.State == M1StarState.Contained ? new Vector2(44, 44) : new Vector2(60, 60));
        boardImages.Add(image);
    }

    private void RefreshHand()
    {
        if (chargeImages[0] == null) return;
        var slotPositions = new[] { -324f, -108f, 108f, 324f };
        for (var i = 0; i < chargeImages.Length; i++)
        {
            var charge = model.Hand[i];
            var enabled = model.State == M1TurnState.PlayerReady && model.IsChargeEnabled(i);
            chargeImages[i].sprite = charge.HasValue ? colourButtons[ButtonSprite(charge.Value)] : rounded;
            chargeImages[i].color = charge.HasValue ? new Color(1, 1, 1, enabled ? 1f : .32f) : new Color(.45f, .48f, .50f, .18f);
            chargeButtons[i].interactable = enabled;
        }
        var ringPosition = slotPositions[Mathf.Clamp(highlightedSlot, 0, 3)];
        selectedGlow.rectTransform.anchoredPosition = new Vector2(ringPosition, 0);
        selectedRing.rectTransform.anchoredPosition = new Vector2(ringPosition, 0);
        selectedGlow.color = WithAlpha(ColorFor(model.Hand[Mathf.Clamp(highlightedSlot, 0, 3)] ?? M1Color.Blue), .14f);
        selectedRing.color = new Color(.87f, .98f, 1, model.State == M1TurnState.PlayerReady ? 1f : .52f);
        nextIndicator.color = model.Next.HasValue ? ColorFor(model.Next.Value) : new Color(.45f, .48f, .50f, .25f);
    }

    private void RefreshStars()
    {
        for (var i = 0; i < starIcons.Length; i++)
            if (starIcons[i] != null) starIcons[i].color = model.Stars[i].State == M1StarState.Collected ? new Color(1f, .74f, .09f, 1f) : new Color(.24f, .29f, .34f);
    }

    private Vector2 BoardPosition(int column, int row, int seed)
    {
        var jitterX = ((seed * 17) % 29) - 14;
        var jitterY = ((seed * 23) % 23) - 11;
        return new Vector2(-300 + column * 120 + jitterX, -300 + row * 84 + jitterY);
    }

    private Sprite SpriteFor(M1Color color, int seed)
    {
        switch (color)
        {
            case M1Color.Red: return fragments[seed % 2 == 0 ? 0 : 3];
            case M1Color.Yellow: return fragments[seed % 2 == 0 ? 1 : 5];
            case M1Color.Green: return fragments[seed % 2 == 0 ? 6 : 7];
            default: return fragments[seed % 2 == 0 ? 2 : 4];
        }
    }

    private static int ButtonSprite(M1Color color)
    {
        switch (color)
        {
            case M1Color.Red: return 0;
            case M1Color.Yellow: return 1;
            case M1Color.Green: return 2;
            default: return 3;
        }
    }

    private static Color ColorFor(M1Color color)
    {
        switch (color)
        {
            case M1Color.Red: return new Color(.92f, .22f, .19f, 1f);
            case M1Color.Yellow: return new Color(1f, .72f, .08f, 1f);
            case M1Color.Green: return new Color(.19f, .67f, .24f, 1f);
            default: return new Color(.14f, .43f, .93f, 1f);
        }
    }

    private static Color WithAlpha(Color color, float alpha) { color.a = alpha; return color; }
    private static float Angle(int seed) { return ((seed * 13) % 57) - 28; }

    private static RectTransform Layer(string name, Transform parent) { var layer=new GameObject(name,typeof(RectTransform)).GetComponent<RectTransform>(); layer.SetParent(parent,false); return layer; }
    private static Image Image(string name, Transform parent, Sprite sprite, Color color) { var image=new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image)).GetComponent<Image>(); image.transform.SetParent(parent,false); image.sprite=sprite; image.color=color; image.raycastTarget=false; return image; }
    private static Text Text(string name,Transform parent,string value,int size,TextAnchor alignment,Color color,Vector2 pos,Vector2 dimensions) { var text=new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer),typeof(Text)).GetComponent<Text>(); text.transform.SetParent(parent,false); text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); text.text=value; text.fontSize=size; text.alignment=alignment; text.color=color; Rect(text.rectTransform,pos,dimensions); return text; }
    private static void Rect(RectTransform rect,Vector2 pos,Vector2 size) { rect.anchorMin=rect.anchorMax=rect.pivot=new Vector2(.5f,.5f); rect.anchoredPosition=pos; rect.sizeDelta=size; }
    private static void Stretch(RectTransform rect) { rect.anchorMin=Vector2.zero;rect.anchorMax=Vector2.one;rect.offsetMin=rect.offsetMax=Vector2.zero; }

    private Sprite Load(string path)
    {
        var result = Resources.Load<Sprite>(path);
        if (result != null) return result;
        var texture = Resources.Load<Texture2D>(path);
        if (texture != null) return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100);
        Debug.LogError("Gameplay asset missing: " + path);
        return null;
    }
    private Sprite[] Sheet(string path,int columns,int rows,int count)
    {
        var texture=Resources.Load<Texture2D>(path); if(texture==null) { Debug.LogError("Gameplay sheet missing: "+path); return new Sprite[0]; }
        var result=new Sprite[count]; var width=texture.width/columns; var height=texture.height/rows;
        for(var i=0;i<count;i++) { var column=i%columns; var row=i/columns; result[i]=Sprite.Create(texture,new Rect(column*width,texture.height-(row+1)*height,width,height),new Vector2(.5f,.5f),100); }
        return result;
    }

    private static Sprite Background()
    {
        const int width=32,height=64; var texture=new Texture2D(width,height,TextureFormat.RGBA32,false); texture.filterMode=FilterMode.Bilinear;
        for(var y=0;y<height;y++) { var color=new Color(.968f,.970f,.968f,1); for(var x=0;x<width;x++) texture.SetPixel(x,y,color); }
        texture.Apply(); return Sprite.Create(texture,new Rect(0,0,width,height),new Vector2(.5f,.5f),100);
    }
    private static Sprite Rounded(int size,int radius)
    {
        var texture=new Texture2D(size,size,TextureFormat.RGBA32,false); texture.filterMode=FilterMode.Bilinear; var c=(size-1)*.5f; var inner=c-radius;
        for(var y=0;y<size;y++) for(var x=0;x<size;x++) { var dx=Mathf.Max(Mathf.Abs(x-c)-inner,0);var dy=Mathf.Max(Mathf.Abs(y-c)-inner,0);texture.SetPixel(x,y,new Color(1,1,1,dx*dx+dy*dy<=radius*radius?1:0)); }
        texture.Apply();return Sprite.Create(texture,new Rect(0,0,size,size),new Vector2(.5f,.5f),100);
    }
    private static Sprite Capsule(int width, int height)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false); texture.filterMode = FilterMode.Bilinear;
        var radius = height * .5f; var left = radius; var right = width - radius - 1f; var centerY = (height - 1) * .5f;
        for (var y = 0; y < height; y++) for (var x = 0; x < width; x++)
        {
            var nearX = Mathf.Clamp(x, left, right); var d = Vector2.Distance(new Vector2(x, y), new Vector2(nearX, centerY));
            texture.SetPixel(x, y, new Color(1, 1, 1, d <= radius ? 1 : 0));
        }
        texture.Apply(); return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(.5f,.5f), 100);
    }
    private static Sprite Glow(int size)
    {
        var texture=new Texture2D(size,size,TextureFormat.RGBA32,false); texture.filterMode=FilterMode.Bilinear; var c=(size-1)*.5f;
        for(var y=0;y<size;y++) for(var x=0;x<size;x++) { var d=Vector2.Distance(new Vector2(x,y),new Vector2(c,c))/c; texture.SetPixel(x,y,new Color(1,1,1,Mathf.Exp(-d*d*4.8f))); }
        texture.Apply();return Sprite.Create(texture,new Rect(0,0,size,size),new Vector2(.5f,.5f),100);
    }
    private static Sprite Ring(int size)
    {
        var texture=new Texture2D(size,size,TextureFormat.RGBA32,false);texture.filterMode=FilterMode.Bilinear;var c=(size-1)*.5f;
        for(var y=0;y<size;y++)for(var x=0;x<size;x++){var d=Vector2.Distance(new Vector2(x,y),new Vector2(c,c))/c;var outer=Mathf.Clamp01((1-d)*9);var inner=Mathf.Clamp01((d-.61f)*9);texture.SetPixel(x,y,new Color(1,1,1,outer*inner));}
        texture.Apply();return Sprite.Create(texture,new Rect(0,0,size,size),new Vector2(.5f,.5f),100);
    }
}
