Shader "Unlit/CircleTransition"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _AnimationProgress("Animation Progress", Float) = 0.5
        _Colour("Colour", Color) = (0, 0, 0, 1)
        _Spacing("Spacing", Float) = 50.0
        _DotSize("Dot Size", Float) = 1.0
        
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        //LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
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

            float _AnimationProgress;
            fixed4 _Colour;
            float _Spacing;
            float _DotSize;

            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


            //static const float spacing = 100.0;
            //static const float animation_progress = 0.5;
            //static const float dot_size = 1.0;

            fixed4 frag (v2f i) : SV_Target
            {
                //return fixed4(_Colour.rgb, 1);
                
                //discard;

                if (_AnimationProgress <= 0.0) {
                    discard;
                }
                
    
                // getting screen and dividing it into a grid
                float2 screen_size = _ScreenParams.xy;
                float2 grid_count = floor(screen_size / _Spacing);
    
                float2 uv = i.uv;
                
                float2 norm_pos = uv * screen_size / (grid_count * _Spacing);
    
                float delay = (norm_pos.x + norm_pos.y) * 0.5;
    
                float visible_threshold = delay * 0.1 + 0.01;
    
                /*
                if (_AnimationProgress < visible_threshold) {
                    discard;
                }
                */
    
                float transition = 0.3;
                float scale = smoothstep(
                    delay - transition,
                    delay + transition,
                    _AnimationProgress * (1.0 + transition)
                );
    
                
                if (scale < 0.005) {
                    discard;
                }
                
    
                float2 grid_centre = (floor(uv * screen_size / _Spacing) + 0.5) * _Spacing;
                float dist = length(uv * screen_size - grid_centre);
                float dot_radius = _DotSize * scale * _Spacing * 0.8;
    
                float alpha = 1.0 - smoothstep(
                    max(dot_radius - 1.5, 0.0),
                    dot_radius + 1.5,
                    dist
                );
    
                
                if (alpha < 0.01) {
                    discard;
                }
                

                return fixed4(_Colour.rgb, alpha);
    
            }


            
            ENDCG
        }
    }
}
