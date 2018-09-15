Shader "Unlit/SegmentWheel"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,0,0,0)
        _DisableColor("Disabled", Color) = (0.6,0.6,0.6,0.6)
        _InnerRadius("Inner Radius", Range(0,1)) = 0
        _OuterRadius("Outer Radius", Range(0,1)) = 1
        _Thickness("Line Thickness", float) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100
            ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog
                #define PI 3.14159265358979323846

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    //float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    //float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color, _DisableColor;
                float _InnerRadius, _OuterRadius;
                float _Thickness;

                uniform float _Segments;
                float3 _LineSegments[100];
                uniform float _Angle;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 iPos = i.uv - float2(0.5,0.5);
                    float len = 2 * length(iPos);
                    if (len < _InnerRadius || len > _OuterRadius)
                    {
                        discard;
                    }

                    fixed4 col = fixed4(1,1,1,1);
                    float iAngle = atan2(iPos.y, iPos.x) + PI;
                    for (int x = 0; x < _Segments; x++)
                    {
                        float2 lineSeg = _LineSegments[x].xy;
                        float2 perp = lineSeg.yx;
                        lineSeg.x *= -1;
                        if (abs(dot(normalize(perp), iPos)) < _Thickness && dot(lineSeg, iPos) > 0)
                        {
                            discard;
                        }
                        if (_LineSegments[_Segments - x - 1].z == 0 && x * _Angle < iAngle && iAngle < (x+1) * _Angle) {
                            col *= _DisableColor;
                        }
                    }

                    col *= tex2D(_MainTex, i.uv) * _Color;
                    return col;
                }

                ENDCG
            }
        }
}
