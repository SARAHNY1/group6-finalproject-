
Shader "GhostWrap" {
    Properties{
        _MainTex("RGB:��ɫ A��͸��",2d) = "gray"{}
        _Opacity("͸����",range(0,1)) = 0.5
        _WrapTex("�Ŷ���ͼ",2d) = "gray"{}
        _WrapInt("�Ŷ�ǿ��",range(0,1)) = 0.5
        //_NoiseTex("������ͼ",2d) = "gray"{}
        _NoiseInt("����ǿ��",range(0,5)) = 0.5
        _FlowSpeed("�����ٶ�",range(0,10)) = 5
    }
    SubShader{
        Tags {
            "Queue" = "Transparent"         //������Ⱦ˳�򣨴Ӵ�ǰ�����Ϊ�Ӻ���ǰ������
            "RenderType" = "Transparent"    //��Opaque��ΪCutout
            "ForceNoShadowCasting" = "True" //�ر���ӰͶ��
            "IgnoreProjector" = "True"      //��Ӱ��Ͷ����
        }
        Pass {
            Name "FORWARD"
            Tags {"LightMode" = "ForwardBase"}

        Blend One OneMinusSrcAlpha//�޸Ļ��ģʽ
        //������Blend One OneMinusSrcAlpha

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        #pragma multi_compile_fwdbase_fullshadows
        #pragma target 3.0

        uniform sampler2D _MainTex; 
        uniform half _Opacity;
        //uniform sampler2D _NoiseTex; uniform float4 _NoiseTex_ST;//����NoiseTex��Tilling
        uniform sampler2D _WrapTex; uniform float4 _WrapTex_ST;
        uniform half _WrapInt;
        uniform half _NoiseInt;
        uniform half _FlowSpeed;

        struct VertexInput {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct VertexOutput {
            float4 pos : SV_POSITION;
            float2 uv0 : TEXCOOED0;//����MainTex
            float2 uv1 : TEXCOOED1;//����NoiseTex
        };

        VertexOutput vert(VertexInput v) {
            VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv0 = v.uv;
                o.uv1 = TRANSFORM_TEX(v.uv, _WrapTex);         //ʹUV1֧��Tilling
                o.uv1.y = o.uv1.y + frac(-_Time.x * _FlowSpeed);//UV1 y��(UV��V�ᣩ����
                //-_Time��Unity����ʱ�䣬��x,y,z,w�ĸ����������ã�����x���������߳ɱ�������
                //frac����˼��ȡ�ֻ࣬ȡʱ��������С�����֡���⣺����Tilling���ԣ���1.5�ͼ�0.5û������
            return o;
        }

        float4 frag(VertexOutput i) : COLOR{
            half3 var_WrapTex = tex2D(_WrapTex,i.uv1).rgb;          //��������ͼ
            float2 uvBias = (var_WrapTex.rg - 0.5) * _WrapInt;      //����uvƫ��ֵ
            //��һ��0��1��ֵ��ȥ0.5���൱��ӳ�䵽��-0.5,5����rgͨ���ֱ��Ӧuvͨ��
            float2 uv0 = i.uv0 + uvBias;                            //Ӧ��uvƫ����
            half4 var_MainTex = tex2D(_MainTex,uv0);              //��ƫ�ƹ���UV��ȥ����MainTex
            half3 finalRGB = var_MainTex.rgb;
            half noise = lerp(1.0, var_WrapTex * 2.0, _NoiseInt);  //Ramp��������0��1ӳ�䵽0��2
            noise = max(0.0, noise);                                //��ȥ����ֵ
            half opacity = var_MainTex.a * _Opacity * noise;        //͸��*���岻͸����*����
            return half4(finalRGB,opacity);                         //����ֵ
        }

        ENDCG
        }
    }

}