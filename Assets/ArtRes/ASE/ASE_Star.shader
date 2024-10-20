// Made with Amplify Shader Editor v1.9.6.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE_Star001"
{
	Properties
	{
		_EdgeWidth("EdgeWidth", Range( 0 , 1.5)) = 1.2
		[HDR]_EdgeColor("EdgeColor", Color) = (1,0,0.9830198,1)
		[HDR]_MainColor("MainColor", Color) = (0.772549,0.31761,1,1)

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Off
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _MainColor;
			uniform float _EdgeWidth;
			uniform float4 _EdgeColor;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_texcoord1.zw = v.ase_texcoord2.xy;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 texCoord1 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float blendOpSrc2 = texCoord1.x;
				float blendOpDest2 = texCoord1.y;
				float temp_output_2_0 = ( saturate( abs( blendOpSrc2 - blendOpDest2 ) ));
				float blendOpSrc4 = texCoord1.x;
				float blendOpDest4 = ( 1.0 - texCoord1.y );
				float temp_output_4_0 = ( saturate( abs( blendOpSrc4 - blendOpDest4 ) ));
				float blendOpSrc7 = temp_output_2_0;
				float blendOpDest7 = temp_output_4_0;
				float blendOpSrc8 = temp_output_4_0;
				float blendOpDest8 = temp_output_2_0;
				float saferPower10 = abs( ( ( saturate( ( blendOpDest7/ max( 1.0 - blendOpSrc7, 0.00001 ) ) )) * ( saturate( ( blendOpDest8/ max( 1.0 - blendOpSrc8, 0.00001 ) ) )) ) );
				float temp_output_10_0 = pow( saferPower10 , 1.6 );
				float2 texCoord29 = i.ase_texcoord1.zw * float2( 1,1 ) + float2( 0,0 );
				float temp_output_28_0 = ( 1.0 - texCoord29.x );
				float temp_output_16_0 = step( temp_output_10_0 , temp_output_28_0 );
				float saferPower14 = abs( temp_output_10_0 );
				
				
				finalColor = ( ( temp_output_16_0 * _MainColor ) + ( ( step( pow( saferPower14 , _EdgeWidth ) , temp_output_28_0 ) - temp_output_16_0 ) * _EdgeColor ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19603
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1072,-208;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;5;-816,128;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;2;-560,-192;Inherit;True;Difference;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;4;-560,96;Inherit;True;Difference;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;7;-224,-192;Inherit;True;ColorDodge;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;8;-224,96;Inherit;True;ColorDodge;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;144,-48;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;176,256;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;1.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;608,416;Inherit;False;Property;_EdgeWidth;EdgeWidth;0;0;Create;True;0;0;0;False;0;False;1.2;1.2;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;10;496,128;Inherit;True;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;624,-208;Inherit;False;2;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;14;928,128;Inherit;True;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;28;912,-336;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;18;1312,-32;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;16;1312,-336;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;19;1632,-192;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;24;1632,144;Inherit;False;Property;_EdgeColor;EdgeColor;2;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0.9830198,1;766.9958,0,753.9721,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ColorNode;25;1312,-656;Inherit;False;Property;_MainColor;MainColor;3;1;[HDR];Create;True;0;0;0;False;0;False;0.772549,0.31761,1,1;0.772549,0.3176099,1,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;1936,-32;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;1664,-544;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;17;608,-336;Inherit;False;Property;_float1;float1;1;0;Create;True;0;0;0;False;0;False;0.02498656;0.9069601;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;2192,-384;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;2720,-320;Float;False;True;-1;2;ASEMaterialInspector;100;5;ASE_Star001;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;2;5;False;;10;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;True;True;2;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
WireConnection;5;0;1;2
WireConnection;2;0;1;1
WireConnection;2;1;1;2
WireConnection;4;0;1;1
WireConnection;4;1;5;0
WireConnection;7;0;2;0
WireConnection;7;1;4;0
WireConnection;8;0;4;0
WireConnection;8;1;2;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;10;0;9;0
WireConnection;10;1;13;0
WireConnection;14;0;10;0
WireConnection;14;1;15;0
WireConnection;28;0;29;1
WireConnection;18;0;14;0
WireConnection;18;1;28;0
WireConnection;16;0;10;0
WireConnection;16;1;28;0
WireConnection;19;0;18;0
WireConnection;19;1;16;0
WireConnection;22;0;19;0
WireConnection;22;1;24;0
WireConnection;26;0;16;0
WireConnection;26;1;25;0
WireConnection;21;0;26;0
WireConnection;21;1;22;0
WireConnection;0;0;21;0
ASEEND*/
//CHKSM=AE77541B54E9B6515BBE4A4467ECAF837A9F2FB8