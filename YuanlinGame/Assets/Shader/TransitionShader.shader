Shader "Custom/TransitionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        _TransitionPara("Transition Para", Range(-1, 1)) = 0
        _AdditionalTex("Additional Tex", 2D) = "white"{}
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

        CBUFFER_START(UnityPerMaterial)

        float4 _Color;
        float4 _RendererColor;
        float _TransitionPara;
        float4 _AdditionalTex_ST;
        float4 _AdditionalTex_TexelSize;

        CBUFFER_END

        TEXTURE2D(_AdditionalTex);
        SAMPLER(sampler_AdditionalTex);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        struct appdata
        {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
            float4 color : COLOR;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 pos : SV_POSITION;
            float3 worldPos : TEXCOORD1;
            half4 color : COLOR;
        };

        v2f vert (appdata v)
        {
            v2f o;
            o.pos = TransformObjectToHClip(v.vertex.xyz);
            o.worldPos = TransformObjectToWorld(v.vertex.xyz);
            o.uv = v.texcoord;
            o.color = v.color;
            return o;
        }

        float4 frag (v2f i) : SV_Target
        {
            float y = _TransitionPara >= 0 ? i.uv.y : 1 - i.uv.y;
            float smooth_edge = smoothstep(-0.1, 0.1, abs(_TransitionPara) - y);
            float mask = 1 - abs(abs(_TransitionPara) - y) / 0.1;

            float2 at_uv = i.pos.xy * _AdditionalTex_TexelSize.xy * _AdditionalTex_ST.xy + _AdditionalTex_ST.zw;
            half additional_tex = SAMPLE_TEXTURE2D(_AdditionalTex, sampler_AdditionalTex, at_uv).r;
            half4 main_tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
            //half3 col = main_tex.rgb + saturate(additional_tex * mask * 0.5);
            //return half4(additional_tex * mask, additional_tex * mask, additional_tex * mask, 1);
            float alpha = step(0.5, (1 - saturate(additional_tex * mask)) * smooth_edge);
            return half4((_Color * _RendererColor * i.color).rgb, alpha);
            //return half4(smooth_edge, smooth_edge, smooth_edge, 1);
        }

        ENDHLSL

        Pass
        {
            Tags { "LightMode" = "Universal2D" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            ENDHLSL
        }
    }
}