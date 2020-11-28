﻿using UnityEngine;
public static class MatShaderModifyr
{
    public static void ChangeBlendRenderType(Material _Mat, BlendMode _BlendMode, string _TagName)
    {
        switch (_BlendMode)
        {
            case BlendMode.NONE: // 아무것도 없을때
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
}
public enum BlendMode
{
    NONE,
    Opaque,
    Cutout,
    Fade,
    Transparent
}


