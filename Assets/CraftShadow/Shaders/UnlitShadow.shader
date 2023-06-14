Shader "Custom/UnlitShadow" 
{
	Properties
	{
		_StencilRef("Stencil Ref", Int) = 4
		_Color("Main Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent-1"
			"IgnoreProjector" = "True"
		}
		LOD 100

		Pass
		{
			ColorMask 0

			ZWrite Off
			ZTest LEqual
			Cull Front
			Blend Off

            Stencil
            {
                Ref [_StencilRef]
                Comp Greater
                ZFail Replace
            }
		}
		
		Pass
		{
			ZWrite Off
			ZTest LEqual
			Cull Back
			Blend SrcAlpha OneMinusSrcAlpha

            Stencil
            {
                Ref [_StencilRef]
                Comp Equal
                Pass Invert
                ZFail Keep
            }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

            fixed4 _Color; 

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
			    fixed4 col = _Color;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}