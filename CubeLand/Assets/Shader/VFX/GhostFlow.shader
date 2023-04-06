
Shader "GhostFlow" {
    Properties{
        _MainTex("RGB:颜色 A：透贴",2d) = "white"{}
        _Opacity("透明度",range(0,1)) = 1
        _NoiseTex("噪声贴图",2d) = "gray"{}
        _NoiseInt("噪声强度",range(0,5)) = 0.5
        _FlowSpeed("流动速度",range(0,10)) = 5
    }
    SubShader{
        Tags {
            "Queue" = "Transparent"         //调整渲染顺序（从从前往后改为从后往前（？）
            "RenderType" = "Transparent"    //从Opaque改为Cutout
            "ForceNoShadowCasting" = "True" //关闭阴影投射
            "IgnoreProjector" = "True"      //不影响投射器
        }
        Pass {
            Name "FORWARD"
            Tags {"LightMode" = "ForwardBase"}

        Blend One OneMinusSrcAlpha//修改混合模式
        //或者是Blend One OneMinusSrcAlpha

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        #pragma multi_compile_fwdbase_fullshadows
        #pragma target 3.0

        uniform sampler2D _MainTex; 
        uniform half _Opacity;
        uniform sampler2D _NoiseTex; uniform float4 _NoiseTex_ST;//启用NoiseTex的Tilling
        uniform half _NoiseInt;
        uniform half _FlowSpeed;

        struct VertexInput {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct VertexOutput {
            float4 pos : SV_POSITION;
            float2 uv0 : TEXCOOED0;//采样MainTex
            float2 uv1 : TEXCOOED1;//采样NoiseTex
        };

        VertexOutput vert(VertexInput v) {
            VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv0 = v.uv;
                o.uv1 = TRANSFORM_TEX(v.uv, _NoiseTex);         //使UV1支持Tilling
                o.uv1.y = o.uv1.y + frac(-_Time.x * _FlowSpeed);//UV1 y轴(UV的V轴）流动
                //-_Time是Unity内置时间，有x,y,z,w四个分量可以拿，其中x最慢，四者成倍数增长
                //frac的意思是取余，只取时间增量的小数部分。理解：对于Tilling而言，加1.5和加0.5没有区别
            return o;
        }

        float4 frag(VertexOutput i) : COLOR{
            half4 var_MainTex = tex2D(_MainTex,i.uv0);              //采样MainTex
            half var_NoiseTex = tex2D(_NoiseTex, i.uv1).r;          //采样NoiseTex
            half3 finalRGB = var_MainTex.rgb;
            half noise = lerp(1.0, var_NoiseTex * 2.0, _NoiseInt);  //Ramp噪声，从0到1映射到0到2
            noise = max(0.0, noise);                                //除去负数值
            half opacity = var_MainTex.a * _Opacity * noise;        //透贴*总体不透明度*噪声
            return half4(finalRGB* opacity,opacity);                         //返回值
        }

        ENDCG
        }
    }

}