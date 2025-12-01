Shader "Custom/SliceByAngleSpriteUpgrade"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _FillColor  ("Filled Color", Color) = (1,1,0,1)
        _EmptyColor ("Empty Color", Color)  = (0.2,0.2,0.2,1)
        _HoldColor  ("Hold Color", Color)   = (1,1,1,1)

        _UpgradeProgress ("Upgrade Progress", Range(0,1)) = 0
        _HoldProgress    ("Hold Progress", Range(0,1)) = 0

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
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            fixed4 _Color;
            fixed4 _FillColor, _EmptyColor, _HoldColor;

            float _UpgradeProgress, _HoldProgress;
            float _Clockwise, _StartAngleDeg, _UseTexture;

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
                a += 1.57079632679; // 90° ββεπυ
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

                float upg = saturate(_UpgradeProgress);
                float hold = saturate(_HoldProgress);

                // WHITE (hold)
                if ((_Clockwise >= 0.5 && a01 < hold) ||
                    (_Clockwise < 0.5 && a01 > 1.0 - hold))
                {
                    fixed3 rgb = lerp(_HoldColor.rgb, tex.rgb, _UseTexture);
                    return fixed4(rgb, tex.a);
                }

                // YELLOW (upgrade)
                if ((_Clockwise >= 0.5 && a01 < upg) ||
                    (_Clockwise < 0.5 && a01 > 1.0 - upg))
                {
                    fixed3 rgb = lerp(_FillColor.rgb, tex.rgb, _UseTexture);
                    return fixed4(rgb, tex.a);
                }

                // GREY (empty)
                {
                    fixed3 rgb = lerp(_EmptyColor.rgb, tex.rgb, _UseTexture);
                    return fixed4(rgb, tex.a);
                }
            }

            ENDCG
        }
    }
}
