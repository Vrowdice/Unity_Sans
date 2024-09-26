Shader "Custom/AtkTransparentShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {} // �ؽ�ó ����
        _Color("Color", Color) = (1, 1, 1, 0.5) // �ʱ� ���� ����
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 200

            Pass
            {
                // ���� ���� �� ���� ���� Ȱ��ȭ
                Blend SrcAlpha OneMinusSrcAlpha // ���� ����
                ZWrite On // ���� ���� Ȱ��ȭ
                Cull Back // �޸� �ø�

                // ���̴� �ڵ� ����
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float3 worldNormal : TEXCOORD1;
                    float3 worldPos : TEXCOORD2;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Lambert ���� ���
                    fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz); // ������ ����
                    float NdotL = max(0, dot(normalize(i.worldNormal), lightDir)); // ������ ���� ������ ����

                    // ���� ���� ��ϰ� �ؽ�ó ���� ����
                    fixed4 texColor = tex2D(_MainTex, i.uv); // �ؽ�ó ����
                    texColor.rgb *= _Color.rgb; // �ؽ�ó ���� ��Ƽ���� ���� ���ϱ�
                    texColor.rgb = texColor.rgb * NdotL + 0.1; // ���� ���� ��� ����, �ּҰ� �߰��Ͽ� ������ ������ ����

                    // ���� ����
                    texColor.a *= _Color.a; // ���� �� ���� (���� �ݿ�)

                    return texColor;
                }
                ENDHLSL
            }
        }
            FallBack "Transparent/Diffuse"
}


