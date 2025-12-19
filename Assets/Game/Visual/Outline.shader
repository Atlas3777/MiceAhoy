Shader "Custom/URP_2D_Outline"
{
    Properties
    {
        [MainTexture] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [HDR] _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 100)) = 1
        _AlphaThreshold ("Alpha Threshold", Range(0, 1)) = 0.1
        
        // Необходимые свойства для SpriteRenderer
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True" 
            "RenderPipeline" = "UniversalPipeline"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            Name "SpriteOutline"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            // Объявляем текстуры и переменные
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize; // Автоматическая переменная Unity: (1/width, 1/height, width, height)

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _OutlineColor;
                float _OutlineWidth;
                float _AlphaThreshold;
                float4 _RendererColor;
                float4 _Flip;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = vertexInput.positionCS;
                
                OUT.uv = IN.uv;
                // Умножаем цвет вершины на цвет Renderer'а (чтобы работало свойство Color в SpriteRenderer)
                OUT.color = IN.color * _Color * _RendererColor; 

                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // Сэмплируем основной цвет спрайта
                float4 mainColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                
                // Применяем цвет вертексов (tint)
                mainColor *= IN.color;

                // Если пиксель уже достаточно непрозрачный, просто возвращаем его (рисуем сам спрайт)
                // Если хочешь, чтобы Outline рисовался ПОВЕРХ спрайта, убери этот if
                if (mainColor.a > _AlphaThreshold)
                {
                    // Премультиплицируем альфу (для правильного блендинга)
                    mainColor.rgb *= mainColor.a;
                    return mainColor;
                }

                // Логика Обводки:
                // Проверяем 4 стороны вокруг текущего пикселя на наличие непрозрачности
                // Используем _MainTex_TexelSize, чтобы ширина была в пикселях, а не в UV
                float2 offset = _MainTex_TexelSize.xy * _OutlineWidth;

                float alphaSum = 0.0;
                
                // Вверх
                alphaSum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(0, offset.y)).a;
                // Вниз
                alphaSum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv - float2(0, offset.y)).a;
                // Влево
                alphaSum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv - float2(offset.x, 0)).a;
                // Вправо
                alphaSum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(offset.x, 0)).a;

                // Если соседи непрозрачные (сумма альф > 0), значит мы на границе
                float isOutline = step(_AlphaThreshold, alphaSum);

                // Формируем цвет обводки
                float4 outlineCol = _OutlineColor;
                
                // Учитываем прозрачность самого спрайта (Fade out)
                outlineCol.a *= isOutline;
                outlineCol.rgb *= outlineCol.a; // Премультипликация

                return outlineCol;
            }
            ENDHLSL
        }
    }
}