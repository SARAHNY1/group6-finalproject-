
Shader "AC" {
    Properties{
    _MainTex("RGB:颜色 A：透贴",2d) = "white"{}
    _Cutoff("透切阈值",range(0, 1)) = 0.5
    }
    SubShader{
        Tags {
            "RenderType" = "TransparentCutout"//从Opaque改为Cutout
            "ForceNoShadowCasting" = "True"//关闭阴影投射
            "IgnoreProjector" = "True"//不影响投射器
        }
        Pass {
            Name "FORWARD"
            Tags {"LightMode" = "ForwardBase"}

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        #pragma multi_compile_fwdbase_fullshadows
        #pragma target 3.0

        uniform sampler2D _MainTex; uniform float4 _MainTex_ST;//开启Maintex的位移缩放控制
        uniform half _Cutoff;

        struct VertexInput {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct VertexOutput {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOOED0;
        };

        VertexOutput vert(VertexInput v) {
            VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }

        float4 frag(VertexOutput i) : COLOR{
            half4 var_MainTex = tex2D(_MainTex,i.uv);//采样贴图
            half opacity = var_MainTex.a;
            clip(opacity - _Cutoff);//透明剪切
            return var_MainTex;//返回值
        }

        ENDCG
        }
    }

}