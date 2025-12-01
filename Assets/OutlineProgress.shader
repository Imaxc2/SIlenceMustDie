Shader "Custom/SliceByAngleSprite"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _FillColor  ("Filled Color", Color) = (1,1,1,1)
        _EmptyColor ("Empty Color", Color)  = (0.2,0.2,0.2,1)

        _Progress   ("Progress (0..1)", Range(0,1)) = 0
        _Clockwise  ("Clockwise (0/1)", Float) = 1
        _StartAngleDeg ("Start Angle (deg, 0=up)", Range(0,360)) = 0

        _UseTexture ("Blend with Sprite (0..1)", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            fixed4 _Color;

            fixed4 _FillColor, _EmptyColor;
            float _Progress, _Clockwise, _StartAngleDeg, _UseTexture;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;
            };
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert(appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            float angle01(float2 q){
                float a = atan2(q.y, q.x);
                a += 1.57079632679;
                if (a < 0) a += 6.28318530718;
                return a / 6.28318530718;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv) * i.color;
                if (tex.a <= 0) discard;

                float2 uvCentered = i.uv - 0.5;
                float2 q = float2(
                    uvCentered.x / _MainTex_TexelSize.x,
                    uvCentered.y / _MainTex_TexelSize.y
                );

                float a01 = angle01(q);

                float start01 = frac(_StartAngleDeg / 360.0);
                a01 = frac(a01 - start01);

                float prog = saturate(_Progress);
                float filled = (_Clockwise >= 0.5) ? step(a01, prog)
                                                   : step(1.0 - prog, a01);

                fixed4 pieCol = lerp(_EmptyColor, _FillColor, filled);

                fixed3 rgb = lerp(pieCol.rgb, tex.rgb, _UseTexture);
                return fixed4(rgb, tex.a);
            }
            ENDCG
        }
    }
}
