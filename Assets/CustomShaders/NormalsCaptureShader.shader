Shader "Custom/NormalsCaptureShader"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _CameraDepthNormalsTexture;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the camera normals texture and output it as color
                fixed4 depthnormal = tex2D(_CameraDepthNormalsTexture, i.uv);

                //decode depthnormal
                float3 normal;
                float depth;
                DecodeDepthNormal(depthnormal, depth, normal);

                //get depth as distance from camera in units 
                depth = depth * _ProjectionParams.z;

                return float4(normal, 1);
            }
            ENDCG
        }
    }
        //Fallback "VertexLit"
}
