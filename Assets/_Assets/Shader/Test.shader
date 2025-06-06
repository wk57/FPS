Shader "Unlit/Test"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
     
    SubShader
    {
        Tags { "RenderType"="Opaque" }        

        Pass
        {
            CGPROGRAM //programinglanguje for shading
            #pragma vertex vert
            #pragma fragment frag        

            #include "UnityCG.cginc"

            struct VertexInput // mesh data, veex position, vert normal, uvs, tangentes, vert colors
            {
                float4 vertex : POSITION;  
                float4 uv0 : TEXCOORD0;  //define coordenadas 2d
                //float4 colors : COLOR;  
                float3 normal : NORMAL;  
                //float4 tangent : TANGENT;  
                 
                //float4 uv1 : TEXCOORD1;  
            };

            struct VertexOutput //output vertex
            {
                float4 clipsSpacePosition : SV_POSITION; 
                float2 uv0 : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;


            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normal = v.normal;
                o.clipsSpacePosition = UnityObjectToClipPos(v.vertex);                
                return o;
            }

            float4 frag (VertexOutput o) : SV_Target
            {                     
                float2 uv = o.uv0;
                float3 normal = o.normal;
                return float4(normal,0);


            }
            ENDCG
        }
    }
}
