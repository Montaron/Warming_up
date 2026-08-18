Shader "Custom/LightningGlow"
{
    Properties
    {
        _MainTex ("Lightning Texture", 2D) = "white" {}

        _CoreColor ("Core Color", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (0.05,0.35,1,1)

        _CoreIntensity ("Core Intensity", Float) = 8
        _GlowIntensity ("Glow Intensity", Float) = 4

        _GlowPower ("Glow Power", Range(0.1, 8)) = 2

        _Alpha ("Alpha", Range(0,1)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _CoreColor;
            fixed4 _GlowColor;

            float _CoreIntensity;
            float _GlowIntensity;
            float _GlowPower;
            float _Alpha;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.color = v.color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);

                // Sprite transparency
                float alpha = tex.a * i.color.a * _Alpha;

                // Ignore completely transparent pixels
                if (alpha <= 0.001)
                    discard;

                // --------------------------------------------------
                // Create a blue/cyan outer glow based on alpha
                // --------------------------------------------------

                float glowMask = pow(alpha, _GlowPower);

                float3 glow = _GlowColor.rgb * _GlowIntensity;

                // --------------------------------------------------
                // White-hot lightning core
                // --------------------------------------------------

                float3 core = _CoreColor.rgb * _CoreIntensity;

                // --------------------------------------------------
                // Combine core + glow
                // --------------------------------------------------

                float3 finalColor = core + glow;

                return float4(finalColor, alpha);
            }

            ENDCG
        }
    }
}