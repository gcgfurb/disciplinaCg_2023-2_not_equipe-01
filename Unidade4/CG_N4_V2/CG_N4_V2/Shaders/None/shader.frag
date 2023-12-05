#version 330 core
out vec4 FragColor;

uniform sampler2D texture0;
uniform sampler2D texture1;

in vec2 TexCoords;

void main()
{
    vec3 texColor0 = vec3(texture(texture0, TexCoords));
    vec3 texColor1 = vec3(texture(texture1, TexCoords));

    vec3 mixedTexture = mix(texColor0, texColor1, 0.2);

    FragColor = vec4(mixedTexture, 1.0);
}
