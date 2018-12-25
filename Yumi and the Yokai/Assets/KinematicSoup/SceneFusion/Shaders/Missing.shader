/**
 * Vertex + pixel shader that renders a solid colour. Uses as a stand-in for missing shaders.
 */
Shader "KS/Missing Shader"
{
    Properties
    {
        [HideInInspector]
        m_colour("Color", Color) = (1, 0, 1, 1)
    }

    SubShader
    {
        Tags{ "Queue" = "Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            fixed4 m_colour;

#pragma vertex vertexShader
#pragma fragment pixelShader
#include "UnityCG.cginc"

            /**
             * Vertex shader output
             */
            struct v2f
            {
                float4 svPos : SV_POSITION;
            };

            /**
             * Vertex shader.
             *
             * @param   float4 pos - vertex position in world space.
             * @return  v2f vertex position in render space.
             */
            v2f vertexShader(float4 pos : POSITION)
            {
                v2f output;
                output.svPos = UnityObjectToClipPos(pos);
                return output;
            }

            /**
             * Pixel shader.
             *
             * @return  float4 colour to render.
             */
            fixed4 pixelShader(v2f input) : SV_TARGET
            {
                return m_colour;
            }

            ENDCG
        }
    }
}