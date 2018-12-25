/**
 * Vertex and pixel shader that renders a grid over an object at close distances, and a solid transparent color when 
 * far away. The grid is made by drawing lines at the intersection planes normal to each axis in object space, blending
 * in the lines when the planes they lie on are not normal to the object's surface.
 */
Shader "KS/Lock"
{                                                                                                                      
    Properties
    {
        [HideInInspector]
        _MainTex("Base (RGB)", 2D) = "white" {}             // Sprite to use the alpha channel of
    
        [HideInInspector]
        m_colour("Color", Color) = (0.5,0.5,0.5,1.0)        // Colour for the lines
        m_lineAlpha("Line Strength", Range(0, 1)) = 0.8     // Alpha values for the lines
        m_fillAlpha("Fill Strength", Range(0, 1)) = 0.0     // Alpha values for regions between the lines
        m_fadeStart("Fade Start", Range(0, 100)) = 12       // Distance at which the lines begin to fade out
        m_fadeEnd("Fade End", Range(0, 100)) = 18           // Distance at which the lines are no longer visible

        m_lineSharpness("Line Sharpness", Range(0, 1)) = 0.65   // How thin the lines are
        m_lineScale("Line Scale", Range(0.01, 10)) = 0.4        // Scale of the lines

        [HideInInspector]
        m_zShift("Z Shift", Range(0, 1)) = 0.01 // Amount to shift vertices along z-axis to avoid z-fighting
    }

    SubShader
    {
        // Batching is disabled because the local space calculations are dependant on per-object scale and orientation.
        // This may be a problem if thousands of objects are selected, but unity already is slow in that case.
        Tags { "Queue" = "Transparent" "DisableBatching" = "True" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vertexShader
            #pragma fragment pixelShader
            #pragma target 3.0
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 m_colour;
            fixed m_lineAlpha;
            fixed m_fillAlpha;
            float m_fadeStart;
            float m_fadeEnd;
            half m_lineSharpness;
            half m_lineScale;
            float m_zShift;

            /**
             * Vertex shader output.
             */
            struct v2f
            {
                float4 svPos : SV_POSITION;     // screen space position
                float2 uv_MainTex : TEXCOORD0;  // UV coordinates for texture
                float3 pos : TEXCOORD1;         // object space position for perspective camera,
                                                // world space for orthographic
                float3 scale : TEXCOORD2;       // scale relative to world
                float3 normal : TEXCOORD3;      // object space normal
                float depth : TEXCOORD4;        // distance from the camera
            };

            /**
             * Vertex shader.
             *
             * @param   float4 pos - vertex position in object space.
             * @param   float2 uv_MainTex - UV coordinates for the main texture.
             * @param   float4 normal - vertex normal in object space.
             * @return  v2f o - information passed to the fragment shader.
             */
            v2f vertexShader(float4 pos : POSITION0, float2 uv_MainTex : TEXCOORD0, float3 normal : NORMAL0)
            {
                v2f o;
                
                // get object space position for perspective camera, and world space for orthographic
                o.pos = (1.0 - unity_OrthoParams.w) * pos + unity_OrthoParams.w * mul(unity_ObjectToWorld, pos);
                
                // gets the object space normal. Normalize is needed for skinned meshes
                o.normal = (1.0 - unity_OrthoParams.w) * normalize(normal) + unity_OrthoParams.w * float3(0, 0, 0);

                // get the scale of the object relative to world space
                o.scale = float3(
                    length(float4(unity_ObjectToWorld[0][0], unity_ObjectToWorld[1][0], unity_ObjectToWorld[2][0], unity_ObjectToWorld[3][0])),
                    length(float4(unity_ObjectToWorld[0][1], unity_ObjectToWorld[1][1], unity_ObjectToWorld[2][1], unity_ObjectToWorld[3][1])),
                    length(float4(unity_ObjectToWorld[0][2], unity_ObjectToWorld[1][2], unity_ObjectToWorld[2][2], unity_ObjectToWorld[3][2])));
                o.scale = (1.0 - unity_OrthoParams.w) * o.scale + unity_OrthoParams.w * float3(1, 1, 1);

                pos = UnityObjectToClipPos(pos); // compute vertex position in screen space
                
                // get the distance between the camera and vertex
                o.depth = (1.0 - unity_OrthoParams.w) * pos.z +
                    2 * unity_OrthoParams.w / (unity_CameraProjection[1][1]);

                // when camera is in perspective, shift vertices closer to the camera proportional to their depth
#if UNITY_REVERSED_Z
                o.depth *= 100;
                pos.z += (1.0 - unity_OrthoParams.w) * m_zShift * o.depth / 2000;
#else
                pos.z -= (1.0 - unity_OrthoParams.w) * m_zShift * o.depth / 2000;
#endif

                o.svPos = pos;
                o.uv_MainTex = uv_MainTex;
                return o;
            }

            /**
             * Pixel shader.
             *
             * @param   v2f i - information passed from the vertex shader.
             * @return  float4 colour to render.
             */
            fixed4 pixelShader(v2f i) : SV_TARGET
            {
                // calculates the blending from lines to solid colour as distance from the camera increases
                half fadeWeight = min(max((i.depth - m_fadeStart) / (m_fadeEnd - m_fadeStart), 0), 1);
                
                // transforms the scale into a frequency
                half frequency = 20 / m_lineScale;

                // adjust the sharpness of the lines
                m_lineSharpness = pow(m_lineSharpness * 8, 2);
                
                // Calculate the line strength in each local axis using a cos function, followed adjusting their
                // falloff with a power function. The result for each axis is limited by the normal in that direction.
                // Only the greatest result of the three axes is used.
                half lineStrength = max(max(
                    min(pow(cos(i.pos.x * i.scale.x * frequency) * 0.5 + 0.5, m_lineSharpness), 1 - abs(i.normal.x)),
                    min(pow(cos(i.pos.y * i.scale.y * frequency) * 0.5 + 0.5, m_lineSharpness), 1 - abs(i.normal.y))),
                    min(pow(cos(i.pos.z * i.scale.z * frequency) * 0.5 + 0.5, m_lineSharpness), 
                    (1 - abs(i.normal.z)) * (1 - unity_OrthoParams.w)));

                // applies the texture alpha
                m_colour.a = tex2D(_MainTex, i.uv_MainTex).a;
                    
                // blends between the lines, fill, and solid colour strength
                return (fadeWeight * m_colour * (m_lineAlpha + m_fillAlpha) / 2) +
                        (1 - fadeWeight) * (m_lineAlpha * m_colour * lineStrength +
                        m_fillAlpha * m_colour * (1 - lineStrength));
            }
            ENDCG
        }
    }
    CustomEditor "LockMaterialInspector"
}