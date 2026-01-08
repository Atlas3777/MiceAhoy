Shader "Custom/BalatroBackground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 1.0
        _Scale ("Scale", Float) = 10.0
        _Color1 ("Color 1", Color) = (0.2, 0.05, 0.3, 1)
        _Color2 ("Color 2", Color) = (0.1, 0.2, 0.5, 1)
        _PixelSize ("Pixel Size", Float) = 256
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Speed, _Scale, _PixelSize;
            float4 _Color1, _Color2;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // 1. Пикселизация
                float2 uv = floor(IN.uv * _PixelSize) / _PixelSize;

                float t = _Time.y * _Speed;

                // --- ВОТ СЮДА ДОБАВЛЯЕМ ЗАВИХРЕНИЯ ---
                // Это деформирует сетку координат перед основным расчетом
                uv.x += 0.05 * sin(uv.y * 10.0 + t);
                uv.y += 0.05 * cos(uv.x * 10.0 + t);
                // -------------------------------------

                float2 p = uv * _Scale;

                // 2. Математика плазмы (теперь использует уже искаженные p)
                float val = sin(p.x + t);
                val += sin((p.y + t) * 0.5);
                val += sin((p.x + p.y + t) * 0.5);

                float2 c = p + float2(sin(t * 0.7), cos(t * 0.3));
                val += sin(length(c) + 1.0);

                // 3. Смешивание цветов
                float colorIdx = 0.5 + 0.5 * sin(val);
                return lerp(_Color1, _Color2, colorIdx);
            }
            ENDHLSL
        }
    }
}