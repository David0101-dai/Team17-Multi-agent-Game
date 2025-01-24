Shader "Custom/DissolveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 贴图
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0 // 溶解进度
        _EdgeColor ("Edge Color", Color) = (1,1,1,1) // 溶解边缘颜色
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
        float _DissolveAmount;
        fixed4 _EdgeColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // 控制溶解效果
            if (c.a < _DissolveAmount)
            {
                discard; // 丢弃像素
            }

            // 边缘颜色
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Transparent/Cutout/Diffuse"
}
