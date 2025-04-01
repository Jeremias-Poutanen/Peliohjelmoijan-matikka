Shader "Unlit/HeightShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PlainsTex ("Plains Texture", 2D) = "white" {}
        _MountainTex ("Mountain Texture", 2D) = "white" {}
        _SnowTex ("Snow Texture", 2D) = "white" {}
        _WaterTex ("Water Texture", 2D) = "white" {}
        // Float parameters for "texturing"
        _SnowHeight ("Snow Height", Range(0.0, 1000.0)) = 100.0
        _MountainHeight ("Mountain Height", Range(0.0, 1000.0)) = 50
        _WaterHeight ("Water Height", Range(-100.0, 100.0)) = 0.1
        _BlendingHeight ("Blending Distance", Range(0.0, 100.0)) = 1.0
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            // Passed parameters
            sampler2D _PlainsTex;
            sampler2D _MountainTex;
            sampler2D _SnowTex;
            sampler2D _WaterTex;
            float _SnowHeight;
            float _WaterHeight;
            float _MountainHeight;
            float _BlendingHeight;

            // This function gives us the t-value for blending
            float HeightBlend(float height, float mid)
            {
                float t = (height - mid) / _BlendingHeight;
                t = t + 0.5;
                if (t > 1.0)
                    t = 1.0;
                else if (t<0.0)
                    t = 0.0;
                return t;
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float height : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.height = v.vertex.y; // Store height data into "outgoing" v2f
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                //   range (0,1)  R  G  B  A
                /*
                fixed4 snowColor = fixed4(1, 1, 1, 1);
                fixed4 mountainColor = fixed4(.1, .1, .1, 1);
                fixed4 sandColor = fixed4(.7, .5, .0, 1);
                fixed4 waterColor = fixed4(0, 0, .8, 1);
                */
                fixed4 snowColor = tex2D(_SnowTex, i.uv);
                fixed4 mountainColor = tex2D(_MountainTex, i.uv);
                fixed4 plainsColor = tex2D(_PlainsTex, i.uv);
                fixed4 waterColor = tex2D(_WaterTex, i.uv);

                // Make snow appear more white
                snowColor *= fixed4(2.5, 2.5, 2.5, 1);

                if(i.height > _SnowHeight + (_BlendingHeight / 2.0))
                {
                    // Snow
                    col = snowColor;
                }
                else if(i.height > _SnowHeight - (_BlendingHeight / 2.0))
                {
                    // Snow Mountain blend
                    float t = HeightBlend(i.height, _SnowHeight);
                    col = (1.0 - t) * mountainColor + t * snowColor;
                }
                else if(i.height > _MountainHeight + (_BlendingHeight / 2.0))
                {
                    // Mountain
                    col = mountainColor;
                }
                else if(i.height > _MountainHeight - (_BlendingHeight / 2.0))
                {
                    // Mountain Sand blend
                    float t = HeightBlend(i.height, _MountainHeight);
                    col = (1.0 - t) * plainsColor + t * mountainColor;
                }
                else if(i.height > _WaterHeight)
                {
                    // Plains
                    col = plainsColor;
                }
                else
                {
                    // Water
                    col = waterColor;
                }

                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
