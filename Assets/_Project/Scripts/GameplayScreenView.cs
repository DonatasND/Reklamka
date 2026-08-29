using UnityEngine;
using UnityEngine.UI;

/// <summary>Static gameplay-screen composition. Mechanics and input are intentionally absent.</summary>
public sealed class GameplayScreenView : MonoBehaviour
{
    private const float W = 1080f;
    private const float H = 1920f;
    private Sprite rounded, glow, ring, vessel, selector;
    private Sprite[] fragments, colourButtons, topIcons;

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
        Build();
    }

    private void Build()
    {
        var canvasObject = new GameObject("Gameplay Screen", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
        canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(W, H); scaler.matchWidthOrHeight = .5f;
        var root = canvasObject.GetComponent<RectTransform>();
        var background = Image("Cold White Background", root, Background(), Color.white); Stretch(background.rectTransform);
        Top(root); Vessel(root); Selector(root); Next(root);
    }

    private void Top(RectTransform root)
    {
        var top = Layer("Top UI", root); Rect(top, Vector2.zero, new Vector2(900, 84)); top.anchoredPosition = new Vector2(0, 806);
        var back = Image("Back", top, topIcons[0], new Color(.20f, .25f, .30f)); Rect(back.rectTransform, new Vector2(-443, 0), new Vector2(44, 44));
        Text("Level", top, "Level 1", 35, TextAnchor.MiddleCenter, new Color(.15f, .19f, .24f), Vector2.zero, new Vector2(280, 58));
        var settings = Image("Settings", top, topIcons[2], new Color(.20f, .25f, .30f)); Rect(settings.rectTransform, new Vector2(443, 0), new Vector2(49, 49));
        var stars = Layer("Empty Stars", root); Rect(stars, new Vector2(0, 691), new Vector2(220, 60));
        foreach (var x in new[] { -68f, 0f, 68f }) { var star = Image("Empty Star", stars, topIcons[1], new Color(.24f, .29f, .34f)); Rect(star.rectTransform, new Vector2(x, 0), new Vector2(48, 48)); }
    }

    private void Vessel(RectTransform root)
    {
        // 1040px sprite includes transparent exterior padding; visible shell resolves to canonical 0.80W.
        var field = Layer("Layered Glass Container", root); Rect(field, new Vector2(0, 53), new Vector2(1040, 950));
        var ground = Image("Vessel Ground Shadow", field, glow, new Color(.18f, .24f, .30f, .12f)); Rect(ground.rectTransform, new Vector2(0, -454), new Vector2(850, 80));
        var cavity = Image("Inner Cavity Tint", field, rounded, new Color(.975f, .973f, .968f, .98f)); Rect(cavity.rectTransform, new Vector2(0, 18), new Vector2(820, 780));
        var depth = Image("Inner Bottom Ambient Depth", field, glow, new Color(.31f, .30f, .28f, .09f)); Rect(depth.rectTransform, new Vector2(0, -282), new Vector2(790, 205));
        var pile = new GameObject("Dense Dry Fragment Pile", typeof(RectTransform), typeof(RectMask2D)).GetComponent<RectTransform>(); pile.SetParent(field, false); Rect(pile, new Vector2(0, -125), new Vector2(790, 600));
        Fragments(pile);
        var inner = Image("Inner Cavity Highlight", field, rounded, new Color(1, 1, 1, .045f)); Rect(inner.rectTransform, new Vector2(0, 84), new Vector2(808, 615));
        var body = Image("Outer Shell Volume", field, vessel, new Color(1f, .97f, .93f, .22f)); Rect(body.rectTransform, new Vector2(0, 4), new Vector2(1040, 950));
        var rim = Image("Glass Rim and Integrated Drain", field, vessel, new Color(1f, .985f, .95f, .44f)); Rect(rim.rectTransform, Vector2.zero, new Vector2(1040, 950));
        var highlight = Image("Outer Rim Highlight", field, vessel, new Color(1, 1, 1, .04f)); Rect(highlight.rectTransform, new Vector2(-3, 8), new Vector2(1024, 930));
    }

    private void Fragments(RectTransform parent)
    {
        var positions = new[] {
            new Vector2(-296,-257), new Vector2(-98,-267), new Vector2(103,-255), new Vector2(296,-263),
            new Vector2(-306,-135), new Vector2(-104,-121), new Vector2(103,-140), new Vector2(305,-123),
            new Vector2(-320,-6), new Vector2(-160,13), new Vector2(4,-10), new Vector2(166,17), new Vector2(320,-5),
            new Vector2(-126,132), new Vector2(86,178), new Vector2(278,110)};
        var scales = new[] { 1.05f,1.09f,1.02f,1.08f,1.10f,1.03f,1.08f,1.04f,1.05f,1.10f,1.03f,1.07f,1.02f,1.08f,1.05f,1.00f };
        var rotation = new[] { -10f,8f,-13f,10f,11f,-8f,14f,-9f,-7f,13f,-11f,9f,7f,-12f,10f,-6f };
        var sprite = new[] { 0,1,2,5,6,3,7,1,5,0,4,6,3,7,1,2 };
        for (var i = 0; i < positions.Length; i++)
        {
            var size = new Vector2(330, 255) * scales[i];
            var shadow = Image("Fragment Contact Shadow", parent, glow, new Color(.16f,.14f,.12f,.16f)); Rect(shadow.rectTransform, positions[i] + new Vector2(4,-18), new Vector2(size.x*.84f,size.y*.31f)); shadow.rectTransform.localEulerAngles = new Vector3(0,0,rotation[i]);
            var fragment = Image("Soft Matte Fragment", parent, fragments[sprite[i]], Color.white); Rect(fragment.rectTransform, positions[i], size); fragment.rectTransform.localEulerAngles = new Vector3(0,0,rotation[i]);
        }
    }

    private void Selector(RectTransform root)
    {
        var holder = Layer("Premium Colour Selector", root); Rect(holder, new Vector2(0,-593), new Vector2(972,246));
        var shadow = Image("Selector Soft Shadow", root, glow, new Color(.18f,.16f,.13f,.15f)); Rect(shadow.rectTransform, new Vector2(0,-616), new Vector2(1015,220)); shadow.transform.SetSiblingIndex(holder.GetSiblingIndex());
        var baseVolume = Image("Selector Interior Volume", holder, Capsule(320, 96), new Color(.99f, .982f, .965f, 1f)); Rect(baseVolume.rectTransform, Vector2.zero, new Vector2(972,246));
        var shell = Image("Authored Selector Capsule", holder, selector, new Color(1f,.995f,.982f,.38f)); Rect(shell.rectTransform, Vector2.zero, new Vector2(972,320));
        var blueGlow = Image("Selected Blue Soft Glow", holder, glow, new Color(.24f,.66f,1,.12f)); Rect(blueGlow.rectTransform, new Vector2(324,0), new Vector2(184,184));
        var blueRing = Image("Selected Blue Ring", holder, ring, new Color(.87f,.98f,1,1)); Rect(blueRing.rectTransform, new Vector2(324,0), new Vector2(204,204));
        var x = new[] { -324f,-108f,108f,324f }; for (var i=0;i<4;i++) { var button=Image("Static Colour Button",holder,colourButtons[i],Color.white); Rect(button.rectTransform,new Vector2(x[i],0),new Vector2(194,194)); }
    }

    private void Next(RectTransform root)
    {
        var shadow = Image("NEXT Soft Shadow", root, glow, new Color(.18f,.16f,.13f,.15f)); Rect(shadow.rectTransform,new Vector2(0,-813),new Vector2(420,135));
        var next = Image("NEXT Pill", root, Capsule(260,88), new Color(.99f,.982f,.965f,1)); Rect(next.rectTransform,new Vector2(0,-799),new Vector2(378,127));
        var rim = Image("NEXT Top Highlight",next.transform,Capsule(260,88),new Color(1,1,1,.34f)); Rect(rim.rectTransform,new Vector2(0,2),new Vector2(366,115));
        var indicator=Image("NEXT Colour Indicator",next.transform,rounded,new Color(1,.73f,.11f)); Rect(indicator.rectTransform,new Vector2(-99,0),new Vector2(49,49));
        Text("NEXT Label",next.transform,"NEXT",28,TextAnchor.MiddleLeft,new Color(.16f,.20f,.24f),new Vector2(7,0),new Vector2(132,50));
    }

    private static RectTransform Layer(string name, Transform parent) { var layer=new GameObject(name,typeof(RectTransform)).GetComponent<RectTransform>(); layer.SetParent(parent,false); return layer; }
    private static Image Image(string name, Transform parent, Sprite sprite, Color color) { var image=new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image)).GetComponent<Image>(); image.transform.SetParent(parent,false); image.sprite=sprite; image.color=color; image.raycastTarget=false; return image; }
    private static void Text(string name,Transform parent,string value,int size,TextAnchor alignment,Color color,Vector2 pos,Vector2 dimensions) { var text=new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer),typeof(Text)).GetComponent<Text>(); text.transform.SetParent(parent,false); text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); text.text=value; text.fontSize=size; text.alignment=alignment; text.color=color; Rect(text.rectTransform,pos,dimensions); }
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
