/*	Info 
	
    Extension Name	: Action Skybox Shader
    Extension Type	: Action
    Author:			: Vazahat Khan (just_in_case)
    Date Created	: December 21, 2022, 11:01 AM
    Description		: Allows you to create a dynamic skybox. 
	
*/
/* 	Donate
	
	If you like my work, please consider "buy me a cup of coffee" to support me.
	You can do that via PayPal :)
	
	PayPal: http://paypal.me/Vazahat
	Discord Server :- https://discord.gg/RKcq89S7uA
	YouTube :- https://www.youtube.com/GlitchedVelocity
	Itch.io :- https://vazahat.itch.io/
	

*/



/* Usage
Simply attach the action to a behavior and fill up the shader properties. Select the skybox as the affecting node, and then provide  Top and Bottom color
and Blend height. You can additionally select wether or not you want the stars to be shown or not.
  
    Itch.io - https://vazahat.itch.io/cc-shader-spritesheet-animation
    Youtube - https://www.youtube.com/channel/UC_yfoGEKkmY63tnyy6hR7ZQ
    Website - https://neophyte.cf
  
*/

/*  <action jsname="action_skybox_shader" description="Skybox shader">
      <property name="Affecting_node" type="scenenode"/>
      <property name="Affect_all_material" type="bool" default="true" />
      <property name="Affecting_material" type="int" default="1" />
      <property name="Base_material_type" type="int" default="0" />
      <property name="Top_Color" type="color" />
      <property name="Bottom_Color" type="color"/>
      <property name="Blend_Height" type="int"	default="3"/>  
      <property name="Show_Stars" type="bool"	default="false"/>  
      <property name="Stars_Count_threshold" type="int"	default="0.08"/>  
      <property name="Stars_Size_threshold" type="int"	default="50"/>  
    </action>
*/

//if (!spritesheetCache)
   // var spritesheetCache = { StartFrame: false, EndFrame: false };

   action_skybox_shader = function () { };

   action_skybox_shader.prototype.execute = function () {
       var timeMs = 1;
       this.Affecting_material -= 1;
       this.nodeName = ccbGetSceneNodeProperty(this.Affecting_node, "Name");
            //print(spritesheetCache.StartFrame);
   
      // if (this.StartFrame == spritesheetCache.StartFrame && this.EndFrame == spritesheetCache.EndFrame) return false;
      // spritesheetCache.StartFrame = this.StartFrame;
      // spritesheetCache.EndFrame = this.EndFrame;
   
   
       //Shader Part
   
   
       var vertexShader = 
       "float4x4 mWorldViewProj;  // World * View * Projection          \n" + 
       "float4x4 mInvWorld;       // Inverted world matrix	 	          \n" + 
       "float4x4 mTransWorld;     // Transposed world matrix          	\n" + 
       "														                                    \n" + 
       "// Vertex shader output structure						                    \n" + 
       "struct VS_OUTPUT										                            \n" + 
       "{														                                    \n" + 
       "	float4 Position   : POSITION;   // vertex position 	          \n" + 
       "	float4 Diffuse    : COLOR0;     // vertex diffuse           	\n" + 
       "	float3 WorldSpacePos    : TEXCOORD1;   // vertex Diffuse color		\n" + 
       "	float2 TexCoord   : TEXCOORD0;  // tex coords	              	\n" + 
       "};													                                  	\n" + 
       "														                                    \n" + 
       "VS_OUTPUT main      ( in float4 vPosition : POSITION,	          \n" + 
       "                      in float3 vNormal   : NORMAL,	            \n" + 
       "                      float2 texCoord     : TEXCOORD0 )         \n" + 
       "{														                                    \n" + 
       "	VS_OUTPUT Output;									                            \n" + 
       "														                                    \n" + 
       "	// transform position to clip space 			                  	\n" + 
       "	Output.Position = mul(vPosition, mWorldViewProj);	            \n" + 
       "														                                    \n" + 
       "	// transformed normal would be this:				                  \n" + 
       "	float3 normal = mul(vNormal, mInvWorld);		                	\n" + 
       "													                                    	\n" + 
       "	// position in world coodinates	would be this:		            \n" + 
       "	// float3 worldpos = mul(mTransWorld, vPosition);           	\n" + 
       "													                                    	\n" + 
       "	Output.Diffuse = float4(1.0, 1.0, 1.0, 1.0);	              	\n" + 
       "	Output.TexCoord = texCoord;						                      	\n" + 
       "	float3 worldSpacePos = mul(mTransWorld, vPosition).xyz;					\n" + 
       "	Output.WorldSpacePos = worldSpacePos  ;							    \n" +
       "														                                    \n" + 
       "	return Output;										                            \n" + 
       "}														";
       
       var fragmentShader = 
       "struct PS_OUTPUT							                                  	\n" + 
       "{											                                          	\n" + 
       "    float4 RGBColor : COLOR0; 		  		                        	\n" +	
       "};											                                        	\n" +
       "												                                          \n" + 
       "sampler2D tex0;							                                    	\n" +
       "sampler2D tex1;							                                    	\n" +
       " float4 topCol;                                                   \n" +
       " float4 bottomCol;                                                  \n" + 
       " float4 pos;                                                  \n" + 
       " float blendHeight;                                                  \n" + 
       " float Stars;                                                  \n" + 
       " float StarsCount;                                                  \n" + 
       " float StarsSize;                                                  \n" + 
       "												                                          \n" +
       //InvLerp
       "float invLerp(float from, float to, float value) { \n" +
       "return (value - from) / (to - from); \n" +
       "} \n" +
       //remap
       "float remap(float origFrom, float origTo, float targetFrom, float targetTo, float value){  \n" +
       "float rel = invLerp(origFrom, origTo, value);  \n" +
       "return lerp(targetFrom, targetTo, rel);  \n" +
       "  }  \n" +
       //Rand
       "float rand(float3 value){  \n" +
        "float3 smallValue = sin(value); \n" +
        "float random = dot(smallValue, float3(12.9898, 78.233, 37.719)); \n" +
        "random = frac(sin(random) * 143758.5453); \n" +
        "return random;  \n" +
        "}  \n" +
       "												                                          \n" +
       "												                                          \n" +
       //rand2dTo1d
       "float rand2dTo1d(float2 value, float2 dotDir = float2(12.9898, 78.233)){ \n" +
        "float2 smallValue = sin(value); \n" +
        "float random = dot(smallValue, dotDir); \n" +
        "random = frac(sin(random) * 143758.5453);   \n" +
        "return random;  \n" +
        "}   \n" +
       "												                                          \n" +
       //rand2dTo2d
       "float2 rand2dTo2d(float2 value){ \n" +
        "return float2(  \n" +
        "    rand2dTo1d(value, float2(12.989, 78.233)),  \n" +
        "    rand2dTo1d(value, float2(39.346, 11.135))   \n" +
        ");  \n" +
        "}   \n" +
       "												                                          \n" +
       //VoronoiNoise
       "float voronoiNoise(float2 value){    \n" +
        "float2 baseCell = floor(value); \n" +
       "												                                          \n" +
       "float minDistToCell = 10;   \n" +
       "float2 closestCell;     \n" +
        "[unroll]        \n" +
        "for(int x=-1; x<=1; x++){   \n" +
        "    [unroll]        \n" +
        "    for(int y=-1; y<=1; y++){       \n" +
        "        float2 cell = baseCell + float2(x, y);      \n" +
        "        float2 cellPosition = cell + rand2dTo2d(cell);      \n" +
        "        float2 toCell = cellPosition - value;       \n" +
        "        float distToCell = length(toCell);      \n" +
        "        if(distToCell < minDistToCell){     \n" +
        "            minDistToCell = distToCell;     \n" +
        "            closestCell = cell;          \n" +
        "        }       \n" +
        "    }       \n" +
        "}       \n" +
        "float random = rand2dTo1d(closestCell);        \n" +
        "return float2(minDistToCell, random);       \n" +
    "}\n" +
    //Pixel Shader
       "PS_OUTPUT main( float2 TexCoord : TEXCOORD0,	                    \n" +
       "                float4 Position : POSITION,	                      \n" +
       "				float3 worldspacepos  : TEXCOORD1,								\n" +
       "                float4 Diffuse  : COLOR0 ) 	                      \n" +
       "{ 												                                        \n" +
       "	PS_OUTPUT Output;							                                  \n" +
       "	float3 Spos  = normalize(pos-worldspacepos);					\n" +
       "	float4 col = tex2D( tex0, TexCoord );  	                    \n" + 
       "	float col2 = remap(-1,1,0,1,Spos.y);  	     \n" + 
       "	col2 = pow(col2,blendHeight); 	     \n" + 
       "	float4 sky = lerp(topCol,bottomCol,col2);  	                    \n" + 
        "   float2 Vpos = (Spos.xz/0.08);  	\n" + 
        "   float2 vNoise = voronoiNoise((TexCoord/StarsCount));    \n" + 
        "   vNoise = 1-saturate(vNoise);  	\n" + 
        "   vNoise = pow(vNoise,StarsSize)/2;  	\n" + 
        "			if (Stars == 1)											    	\n" + 
        "   		{ 																	\n" + 
        "     			Output.RGBColor = (col*sky)+vNoise.x ;	\n" + 
        "  			}																	\n" + 
        "  			else																\n" + 
        "  			 {     																\n" + 
        "     			Output.RGBColor = (col*sky) ;											\n" + 
        "   		 }  																\n" + 
       "	return Output;								\n" +
       "}";

      var me = this;
      myShaderCallBack = function () {
           var topCol = RGB(me.Top_Color);
           var bottomCol = RGB(me.Bottom_Color);
           var pos = ccbGetSceneNodeProperty(ccbGetActiveCamera(),"Position");
           if(me.Show_Stars){var starsShow = 1;}
           else{starsShow = 0;}
   
          ccbSetShaderConstant(2, 'topCol', topCol.x, topCol.y, topCol.z, 1);
          ccbSetShaderConstant(2, 'bottomCol', bottomCol.x, bottomCol.y, bottomCol.z, 1);
          ccbSetShaderConstant(2, 'pos', pos.x, pos.y, pos.z, 0);
          ccbSetShaderConstant(2, 'blendHeight', me.Blend_Height, 0, 0, 0);
          ccbSetShaderConstant(2, 'Stars', starsShow, 0, 0, 0);
          ccbSetShaderConstant(2, 'StarsCount', me.Stars_Count_threshold, 0, 0, 0);
          ccbSetShaderConstant(2, 'StarsSize', me.Stars_Size_threshold, 0, 0, 0);
          ccbSetSceneNodeProperty(me.Affecting_node,"Position",pos);
       }
   
       // creating Material
       var newMaterial = ccbCreateMaterial(vertexShader, fragmentShader, this.Base_material_type,myShaderCallBack);
   
       //Check Material index and apply to specified mat index or to all the materials.
       var matCount = ccbGetSceneNodeMaterialCount(this.Affecting_node);
   
       for (var i = 0; i < matCount; ++i) {
           if (this.Affect_all_material) {
               ccbSetSceneNodeMaterialProperty(this.Affecting_node, i, 'Type', newMaterial);
           }
           else { ccbSetSceneNodeMaterialProperty(this.Affecting_node, this.Affecting_material, 'Type', newMaterial); }
       }
   }
   
// Fixing the Color Property type Parameter of action to get RGB value and clamp them between 0 and 1.
function RGB(decimalcolorcode)
{var color = (decimalcolorcode); // use the property type or put a  decimal color value.
 var Rr = (color & 0xff0000) >> 16; // get red color by bitwise operation  
 var Gg = (color & 0x00ff00) >> 8; // get green color by bitwise operation 
 var Bb = (color & 0x0000ff); // get blue color by bitwise operation 
 var RrGgBb = new vector3d(Rr,Gg,Bb);
 var r = (Rr/255); // dividing red by 255 to clamp b/w 0-1 
 var g = (Gg/255); // dividing green by 255 to clamp b/w 0-1 
 var b = (Bb/255); // dividing blue by 255 to clamp b/w 0-1 
 var rgb = new vector3d (r,g,b); // final rgb value to use in the editor
 return rgb;
 }

 /*End Of Code*/
 
// Above extension is written by Vazahat Khan (just_in_case) //