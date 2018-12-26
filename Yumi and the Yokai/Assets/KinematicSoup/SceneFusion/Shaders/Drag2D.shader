/**
 * Vertex + pixel shader for sprites that renders a transparent colour.
 */
Shader "KS/Drag2D"
{
    Properties
    {
        [HideInInspector]
        _MainTex("Base (RGB)", 2D) = "white" {}

        [HideInInspector]
        m_colour("Color", Color) = (1, 0, 0, 0.8)
    }

    SubShader
    {
        Tags{ "Queue" = "Transparent" }
        Pass
        {
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM

            fixed4 m_colour;

#pragma vertex vertexShader
#pragma fragment pixelShader
#include "UnityCG.cginc"

            sampler2D _MainTex;

            /**
             * Vertex shader input
             */
            struct VertexInput
            {
                float4 pos : POSITION;
                float2 uv_MainTex : TEXCOORD0;
            };

            /**
             * Vertex shader output
             */
            struct VertexOutput
            {
                float4 svPos : SV_POSITION;
                float2 uv_MainTex : TEXCOORD0;
            };

            /**
             * Vertex shader.
             *
             * @param  VertexInput input - vertex position in world space and uvs.
             * @return VertexOutput vertex position in render space.
             */
            VertexOutput vertexShader(VertexInput input)
            {
                VertexOutput output;
                output.svPos = UnityObjectToClipPos(input.pos);
                output.uv_MainTex = input.uv_MainTex;
                return output;
            }

            /**
             * Pixel shader.
             *
             * @param   VertexOutput input
             * @return  fixed4 colour to render.
             */
            fixed4 pixelShader(VertexOutput input) : SV_TARGET
            {
                fixed4 output;
                m_colour.a = 0.3;
                output = m_colour;
                half4 c = tex2D(_MainTex, input.uv_MainTex);
                output.a *= c.a;
                return output;
            }

            ENDCG
        }
    }
}