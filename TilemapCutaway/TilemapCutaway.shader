Shader "Custom/TilemapCutaway"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CutawayCenter ("Cutaway Center", Vector) = (0,0,0,0)
        _CutawayRadius ("Cutaway Radius", Float) = 3
        _FadeSmoothness ("Fade Smoothness", Float) = 1
        _Color ("Color Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // Tilemap tint
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;
            float4 _CutawayCenter;
            float _CutawayRadius;
            float _FadeSmoothness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = distance(i.worldPos.xy, _CutawayCenter.xy);
                float fade = saturate((dist - _CutawayRadius) / _FadeSmoothness);

                fixed4 tex = tex2D(_MainTex, i.uv);

                // Final color multiplies everything (true darkening)
                fixed4 final = tex * i.color;
                final.a *= fade;

                return final;
            }
            ENDCG
        }
    }
}
