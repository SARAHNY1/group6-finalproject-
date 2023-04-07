
Shader "Toon" {
    Properties{
        _BaseMap("Texture",2D) = "white"{}
        _SSSMap("Texture",2D) = "black"{}
        _ILMMap("Texture",2D) = "gray"{}
        _ToonTheshold("ToonThreshold",Range(0,1)) = 0.5
        _ToonHardness("ToonHardness", Float) = 20
        //_ShadingCol("ShadingCol",Color)=(0.5,0.5,1.0,1.0)

        //模型描边
        _MainColor("主颜色",Color) = (0,0,0,1)
        _OutlineColor("描边颜色",Color) = (1,1,1,1)
        _OutlineGlowPow("描边等级",Range(0,5)) = 2
        _OutlineStrength("描边强度",Range(0,4)) = 1

    }
    SubShader{
        Tags {"RenderType" = "Opaque" }
        LOD 100

        Pass {

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        #pragma multi_compile_fwdbase

        sampler2D _BaseMap;
        sampler2D _SSSMap;
        sampler2D _ILMMap;//rgba通道分别控制高光，光照偏移，反光，内描边
        float _ToonTheshold;
        float _ToonHardness;
        //float _ShadingCol

        float4 _MainColor;
        fixed4 _OutlineColor;
        fixed _OutlineGlowPow;
        fixed _OutlineStrength;

        struct VertexInput {
            float4 vertex : POSITION;
            float2 uv0 : TEXCOORD0;
            float2 uv1 : TEXCOORD1;
            float3 normal : NORMAL;
            float4 color : COLOR;
        };

        struct VertexOutput {
            float4 uv : TEXCOORD0;//同时存储两套UV
            float4 pos : SV_POSITION;
            float3 pos_world : TEXCOORD1;
            float3 nDirWS : TEXCOORD2;
            float4 vertex_color : TEXCOORD3;
            float3 normal:TEXCOORD4;
        };

        VertexOutput vert(VertexInput v) {
            VertexOutput o = (VertexOutput)0;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.pos_world = mul(unity_ObjectToWorld, v.vertex).xyz;//世界空间坐标
            o.nDirWS = UnityObjectToWorldNormal(v.normal);
            o.uv = float4(v.uv0, v.uv1);//同时存储两套UV
            o.vertex_color = v.color;
            o.normal = UnityObjectToWorldNormal(v.normal);
            return o;
        }

        float4 frag(VertexOutput i) : COLOR {
            half2 uv1 = i.uv.xy;
            half2 uv2 = i.uv.zw;//拆分两套UV

            //向量准备
            float3 normalDir = normalize(i.nDirWS);
            float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

            //Base 亮部颜色
            half4 base_map = tex2D(_BaseMap, uv1);
            half3 base_color = base_map.rgb;
            //sss 暗部颜色
            half4 sss_map = tex2D(_SSSMap, uv1);
            half3 sss_color = (sss_map.rgb - 0.5, sss_map.rgb - 0.5, sss_map.rgb - 0.5);//* _ShadingCol;

            half NdotL = dot(normalDir, lightDir);
            half half_lambert = (NdotL + 1.0) * 0.5;
            half toon_diffuse = saturate((half_lambert - _ToonTheshold) * _ToonHardness);

            float3 final_diffuse = lerp(sss_color, base_map, toon_diffuse);
            return fixed4(final_diffuse,1.0);

        }

        ENDCG
        }
    }

}