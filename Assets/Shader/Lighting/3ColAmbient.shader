
Shader "Custom/3Col" {
    Properties{
        _Occlusion("OA��ͼ",2d) = "white"{}
        _EnvUpCol("Up_Color",color) = (1.0,1.0,1.0,1.0)
        _EnvSideCol("Side_Color", color) = (0.5, 0.5, 0.5, 1.0)
        _EnvDownCol("Down_Color", color) = (0.0, 0.0, 0.0, 1.0)
    }

        SubShader
        {
            Tags {"RenderType" = "Opaque" }
            Pass {
                Name "FORWARD"
                Tags {"LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma target 3.0

            uniform float3 _EnvUpCol;
            uniform float3 _EnvSideCol;
            uniform float3 _EnvDownCol;
            uniform sampler2D _Occlusion;

            struct VertexInput {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv0 : TEXCOORD0;//uv0������ǵ�һ��uv
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float3 nDirWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            VertexOutput vert(VertexInput v) {
                VertexOutput o = (VertexOutput)0;//�½�һ������ṹ
                   o.pos = UnityObjectToClipPos(v.vertex);
                   o.nDirWS = UnityObjectToWorldNormal(v.normal);
                   o.uv = v.uv0;
                return o;
            }

            float4 frag(VertexOutput i) : COLOR{
                //׼������
                float3 nDir = i.nDirWS;
                //�������������
                float upMask = max(0.0, nDir.g);//ȡ��ͨ������0�Ĳ��֣������ϵĲ���
                float downMask = max(0.0, -nDir.g);
                float sideMask = 1.0-upMask-downMask;
                //��ϻ���ɫ
                float3 envCol = _EnvUpCol * upMask + _EnvSideCol * sideMask + _EnvDownCol * downMask;
                //����Occlusion��ͼ
                float occlusion = tex2D(_Occlusion, i.uv);
                //���㻷������
                float3 envLighting = envCol * occlusion;
                //����������ɫ
                return float4(envLighting, 1);
            }
            ENDCG
        }
    }
        
}
��������������������������������