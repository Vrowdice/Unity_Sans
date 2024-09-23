Shader "Custom/RedOutlineWithTransparency"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (1,0,0,0.5) // ������ + 50% ����
        _Outline("Outline width", Range(.002, 0.03)) = .005 // �׵θ� �β�
    }

        SubShader
    {
        Tags {"Queue" = "Transparent"} // ���� ������ ���� Queue�� Transparent�� ����
        Pass
        {
            // �⺻ ������Ʈ ������
            Name "BASE"
            Cull Back
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha // ���� ���� Ȱ��ȭ

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }

        Pass
        {
                // �ܰ��� ������
                Name "OUTLINE"
                Cull Front
                ZWrite On
                Blend SrcAlpha OneMinusSrcAlpha // ���� ���� Ȱ��ȭ

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                };

                float _Outline;
                float4 _OutlineColor;

                v2f vert(appdata v)
                {
                    // ������ ����Ͽ� �ܰ����� Ȯ���ϴ� ���
                    v.vertex.xyz += v.normal * _Outline;
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    return _OutlineColor; // ������ + ���� ����
                }
                ENDCG
            }
    }

        FallBack "Diffuse"
}

