/**
 * Vertex + pixel shader that renders a shadow. Use this shader on a projector to create the terrain brush preview.
 */
Shader "KS/Cutout"
{
    Properties
    {
        _MainTex("Cookie", 2D) = "gray" {}
        m_colour("Color", Color) = (1, 0, 0, 1)
    }
    
    Subshader
    {
        Tags { "Queue" = "Transparent" }
        Pass
        {
            ZWrite Off
            ColorMask RGB
            Blend DstColor Zero
            Offset -1, -1

            CGPROGRAM

            fixed4 m_colour;

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            /**
             * Vertex shader output
             */
            struct v2f
            {
                float4 uvShadow : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            float4x4 unity_Projector;

            /**
             * Vertex shader. Compute vertex position in render space.
             *
             * @param   float4 pos - vertex position in world space.
             * @return  v2f vertex position in render space.
             */
            v2f vert(float4 vertex : POSITION)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                o.uvShadow = mul(unity_Projector, vertex);
                return o;
            }

            fixed4 _Color;
            sampler2D _MainTex;

            /**
             * Pixel shader. Draw a shadow with _MainTex's shape and m_colour's colour.
             *
             * @return  float4 colour to render.
             */
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texS = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.uvShadow));
                texS.a *= step(i.uvShadow.x, 1) * step(i.uvShadow.y, 1) * step(0, i.uvShadow.x) * step(0, i.uvShadow.y);
                texS.a = 1.0 - texS.a;
                fixed4 res = lerp(m_colour, texS, texS.a);
                return res;
            }
            ENDCG
        }
    }
}