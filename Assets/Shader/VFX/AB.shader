
Shader "AB" {
    Properties{
        _MainTex("RGB:��ɫ A��͸��",2d) = "white"{}
        _Opacity("͸����",range(0,1)) = 1
    }
    SubShader{
        Tags {
            "Queue" = "Transparent"//������Ⱦ˳�򣨴Ӵ�ǰ�����Ϊ�Ӻ���ǰ������
            "RenderType" = "Transparent"//��Opaque��ΪCutout
            "ForceNoShadowCasting" = "True"//�ر���ӰͶ��
            "IgnoreProjector" = "True"//��Ӱ��Ͷ����
        }
        Pass {
            Name "FORWARD"
            Tags {"LightMode" = "ForwardBase"}

        Blend SrcAlpha OneMinusSrcAlpha//�޸Ļ��ģʽ
        //������Blend One OneMinusSrcAlpha

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        #pragma multi_compile_fwdbase_fullshadows
        #pragma target 3.0

        uniform sampler2D _MainTex; uniform float4 _MainTex_ST;//����Maintex��λ�����ſ���
        uniform half _Opacity;

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
            half4 var_MainTex = tex2D(_MainTex,i.uv);//������ͼ
            half3 finalRGB = var_MainTex.rgb;
            half opacity = var_MainTex.a * _Opacity;
            return half4(finalRGB*opacity,opacity);
        }

        ENDCG
        }
    }

}