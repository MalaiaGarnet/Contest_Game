using UnityEngine;
public static class MatShaderModifyr
{
    public static void ChangeBlendRenderType(Material _Mat, BlendMode _BlendMode, string _TagName = "Opaque")
    {
        switch (_BlendMode)
        {
            case BlendMode.NONE: // 아무것도 없을때
                _Mat.SetOverrideTag("RenderType", _TagName.ToString());
                break;
            case BlendMode.Opaque: // Opaque(평소)
                _Mat.SetOverrideTag("RenderType", _TagName);
                break;
            case BlendMode.Cutout: // 컷아웃 모드일때
                _Mat.SetOverrideTag("RenderType", _TagName);
                break;
            case BlendMode.Fade: // 페이드일때
                _Mat.SetOverrideTag("RenderType", _TagName);
                break;
            case BlendMode.Transparent: // 알파적용된 투명모드일때
                _Mat.SetOverrideTag("RenderType", _TagName);
                break;
            default:
                break;
        }
    }

    public static void ChangeBlendQueueType(Material _Mat, BlendMode _BlendMode, RenderOrder _Order)
    {
        switch (_BlendMode)
        {
            case BlendMode.NONE: // 아무것도 없을때
                _Mat.SetOverrideTag("Queue", "Opaque");
                break;
            case BlendMode.Opaque: // Opaque(평소)
                _Mat.SetOverrideTag("Queue", _Order.ToString());
                break;
            case BlendMode.Cutout: // 컷아웃 모드일때
                _Mat.SetOverrideTag("Queue", _Order.ToString());
                break;
            case BlendMode.Fade: // 페이드일때
                _Mat.SetOverrideTag("Queue", _Order.ToString());
                break;
            case BlendMode.Transparent: // 알파적용된 투명모드일때
                _Mat.SetOverrideTag("Queue", _Order.ToString());
                break;
            default:
                _Mat.SetOverrideTag("Queue", _Order.ToString());
                break;
        }
    }
}
public enum BlendMode
{
    NONE,
    Opaque,                
    Cutout,                 
    Fade,
    Transparent,
    TransparentCutout,
    Background,
    Overlay,
    TreeOpaque,
    TreeTransparentCutout,
    TreeBillboard,
    Grass,
    GrassBillboard
}

public enum RenderOrder
{
    NONE,
    Opaque,
    Background,
    Geometry,
    AlphaTest,
    Transparent,
    Overlay
}


