Shader "Custom/EdgeDetection"{
    Properties{
        _MainTex("MainTex", 2D) = ""{}
        _EdgeOnly("EdgeOnly", Float) = 1.0
        _EdgeColor("EdgeColor", Color) = (0,0,0,1)
        _BackgroundColor("BackgroundColor", Color) = (1,1,1,0)
    }

    SubShader{
        ZWrite Off
        ZTest Always Cull Off

        Pass{
            CGPROGRAM

            #include "UnityCG.cginc"

            #pragma vertex vert  
            #pragma fragment fragSobel

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform half4 _MainTex_TexelSize;
            fixed _EdgeOnly;
            fixed4 _EdgeColor;
            fixed4 _BackgroundColor;

            struct v2f{
                float4 pos : SV_POSITION;
                half2 uv[9] : TEXCOORD0;
            };

            v2f vert(appdata_img v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                half2 uv = v.texcoord;

                o.uv[0] = uv + _MainTex_TexelSize.xy * half2(-1, -1);
                o.uv[1] = uv + _MainTex_TexelSize.xy * half2(0, -1);
                o.uv[2] = uv + _MainTex_TexelSize.xy * half2(1, -1);
                o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 0);
                o.uv[4] = uv + _MainTex_TexelSize.xy * half2(0, 0);
                o.uv[5] = uv + _MainTex_TexelSize.xy * half2(1, 0);
                o.uv[6] = uv + _MainTex_TexelSize.xy * half2(-1, 1);
                o.uv[7] = uv + _MainTex_TexelSize.xy * half2(0, 1);
                o.uv[8] = uv + _MainTex_TexelSize.xy * half2(1, 1);

                return o;
            }

            fixed luminance(fixed4 color) {
                return  0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b; 
            }

            half Sobel(v2f i) {
                // const half Gx[9] = {-1,  0,  1,
                //                         -2,  0,  2,
                //                         -1,  0,  1};
                // const half Gy[9] = {-1, -2, -1,
                //                         0,  0,  0,
                //                         1,  2,  1};     
               // const half Gx[9] = {-3,  0,  3,
            //                            -10,  0,  10,
             //                           -3,  0,  3};
              //  const half Gy[9] = {-3, -10, -3,
               //                         0,  0,  0,
               //                         3,  10,  3};    


				const half Gx[9] = {-6,  0,  6,
                                        -20,  0,  20,
                                        -6,  0,  6};
                const half Gy[9] = {-6, -20, -6,
                                        0,  0,  0,
                                        6,  20,  6};    

                half texColor;
                half edgeX = 0;
                half edgeY = 0;
                for (int it = 0; it < 9; it++) {
                    texColor = luminance(tex2D(_MainTex, i.uv[it]));
                    edgeX += texColor * Gx[it];
                    edgeY += texColor * Gy[it];
                }

                half edge = 1 - abs(edgeX) - abs(edgeY);

                return edge;
            }

            fixed4 fragSobel(v2f i) : SV_Target {
                half edge = Sobel(i);

                //边缘颜色与图片颜色 - 越靠近边缘越接近于设置的边缘颜色否则为图片原来的颜色
                fixed4 withEdgeColor = lerp(_EdgeColor, tex2D(_MainTex, i.uv[4]), edge * 0.5 + 0.7);
                //边缘颜色与背景颜色 - 越靠近边缘越接近于设置的边缘颜色否则为设置的背景颜色
                fixed4 onlyEdgeColor = lerp(_EdgeColor, _BackgroundColor, edge);
                //图片颜色于背景颜色
                return lerp(withEdgeColor, onlyEdgeColor, _EdgeOnly);
            }

            ENDCG
        }
    }
    FallBack Off
}