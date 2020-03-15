Shader "Unlit/VertexColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		[Toggle(USE_EXTRA_INTERPOLATION)] _UseExtraInterpolation("Use ExtraInterpolation", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma shader_feature DO_INTERP

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float2 barycentric : TEXCOORD1;
            };

            struct v2f
            {
				float2 uv : TEXCOORD0;
				float4 color : TEXCOORD1;
				float2 barycentric : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _ExtraColorData;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				o.barycentric = v.barycentric;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
#ifdef DO_INTERP
			return i.color + i.barycentric.x * i.barycentric.y * _ExtraColorData;
#else
			return i.color;
#endif
            }
            ENDCG
        }
    }
}
