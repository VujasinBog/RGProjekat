#version 330 core
out vec4 FragColor;

in vec2 TexCoords;
in vec3 Normal;
in vec3 FragPos;

struct DirLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};


uniform sampler2D texture1;
uniform float shininess;
uniform DirLight dirLight;
uniform vec3 viewPosition;


vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);

void main()
{
    vec4 texColor = texture(texture1, TexCoords);
    if(texColor.a < 0.1)
        discard;

    vec3 normal = normalize(Normal);
    vec3 viewDir = normalize(viewPosition - FragPos);
    vec3 result = CalcDirLight(dirLight, normal, viewDir);
    FragColor = texColor;



}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    // specular shading
    vec3 halfwayDir = normalize(lightDir + viewDir);
    float spec = pow(max(dot(normal, halfwayDir), 0.0), shininess);
//     combine results
    vec3 ambient = light.ambient * vec3(texture(texture1, TexCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(texture1, TexCoords));
    vec3 specular = light.specular * spec * vec3(texture(texture1, TexCoords));
    return (ambient + diffuse + specular);
}