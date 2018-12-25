/**
 * Vertex and pixel shader the renders a billboarded cutout. Quads rendered with this shader will always face the
 * camera.
 */
Shader "KS/BillboardCutout"
{
    Properties
    {
        m_size("Size", Range(0, 1)) = 0.5                   // Scale
        
        [HideInInspector]
        m_texture ("Base (RGB) Trans (A)", 2D) = "white" {} // Texture to render
        
        [HideInInspector]
        m_cutoffAplha ("Alpha cutoff", Range(0,1)) = 0.5    // Alpha below which the texture isn't rendered
        
        [HideInInspector]
        m_colour("Color", Color) = (0.5,0.5,0.5,1.0)        // Tint colour
        
        [HideInInspector]
        m_zShift("Z Shift", Range(0, 1)) = 0.1              // Amount to shift verticies along z-axis to avoid
                                                            // z-fighting
    }
    
    SubShader
    {
        // Batching is disabled to allow for billboarding
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching"="True"}
        LOD 100

        Lighting Off

        Pass
        {  
            CGPROGRAM
                #pragma vertex vertexShader
                #pragma fragment pixelShader
                
                #include "UnityCG.cginc"

                /**
                 * Vertex shader output.
                 */
                struct v2f
                {
                    float4 pos : SV_POSITION;   // screen space position
                    half2 texCoord : TEXCOORD0; // texture uv coordinate
                };

                sampler2D m_texture;
                float4 m_texture_ST;
                fixed m_cutoffAplha;
                fixed4 m_colour;
                float m_size;
                float m_zShift;

                /**
                 * Vertex shader.
                 *
                 * @param   float4 pos - vertex position in object space.
                 * @param   float2 texCoord - texture uv coordinate.
                 * @return  v2f - information passed to the pixel shader.
                 */
                v2f vertexShader(float4 pos : POSITION, float2 texCoord : TEXCOORD0)
                {
                    v2f o;
                    
                    // orient quad towards camera
                    float3 tmp = UnityObjectToViewPos(float3(0.0, 0.0, 0.0));
                    o.pos = mul(UNITY_MATRIX_P,
                        float4(tmp[0], tmp[1], tmp[2], 1.0) +
                        float4(pos.x * m_size, pos.y * m_size, 0.0, 0.0));
                    o.texCoord = TRANSFORM_TEX(texCoord, m_texture);
                    
                    // when camera is in perspective, shift vertices closer to the camera proportional to their depth
                    o.pos.z += m_zShift * o.pos.z / 2000;
                    return o;
                }
                
                /**
                 * Pixel shader.
                 *
                 * @param   v2f i - input from vertex shader.
                 * @return  fixed4 colour to render.
                 */
                fixed4 pixelShader(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(m_texture, i.texCoord);
                    clip(col.a - m_cutoffAplha);
                    return col * m_colour;
                }
            ENDCG
        }
    }
}