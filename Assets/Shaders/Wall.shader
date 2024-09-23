Shader "Custom/TransparentShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,0.5) // �ʱ� ���� ����
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" }
            LOD 200

            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha // ���� ����
                Cull Back // �޸� �ø�
                ZWrite Off // ���� ���� ��Ȱ��ȭ

                // �⺻ ���̴� �ڵ�
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    col *= 0.1; // ���� �� ���� ����
                    return col;
                }
                ENDHLSL
            }
        }
            FallBack "Diffuse"
}