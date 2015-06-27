// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace RayTracer.Materials
{
    /// <summary>
    /// A TextureMaterial represents an image-based material, loaded from a bitmap file.
    /// </summary>
    public class TextureMaterial : Material
    {
        private Texture diffuseTexture;
        private Texture specularTexture;

        /// <summary>
        /// Constructs a TextureMaterial from the given texture bitmap file name.
        /// </summary>
        /// <param name="textureFileName"></param>
        public TextureMaterial(string textureFileName)
            : this(textureFileName, opacity: 1.0f)
        {
        }

        /// <summary>
        /// Constructs a TextureMaterial with separate textures for diffuse and specular color.
        /// </summary>
        /// <param name="diffuseFileName"></param>
        /// <param name="specularFileName"></param>
        public TextureMaterial(string diffuseFileName, string specularFileName)
            : this(diffuseFileName)
        {
            this.specularTexture = new Texture(specularFileName);
        }

        /// <summary>
        /// Constructs a TextureMaterial from the given diffuse bitmap file and with the given properties.
        /// </summary>
        /// <param name="diffuseFileName">The bitmap to use for the texture</param>
        /// <param name="opacity">The percentage of light that is absorbed by the material</param>
        /// <param name="reflectivity">The percentage of light that is reflected by the material</param>
        /// <param name="refractivity">The amount of refraction occurring on rays passing through the material</param>
        /// <param name="glossiness">The glossiness of the material, which impacts shiny specular highlighting</param>
        public TextureMaterial(
            string diffuseFileName,
            float opacity = 1.0f,
            float reflectivity = 0.0f,
            float refractivity = 0.0f,
            float glossiness = 1.0f)
            : base(reflectivity, refractivity, opacity, glossiness)
        {
            this.diffuseTexture = new Texture(diffuseFileName);
        }

        /// <summary>
        /// Constructs a TextureMaterial from the given diffuse and specular bitmap files and with the given properties.
        /// </summary>
        /// <param name="diffuseFileName">The bitmap to use for the diffuse texture</param>
        /// <param name="specularFileName">The bitmap to use for the specular texture</param>
        /// <param name="opacity">The percentage of light that is absorbed by the material</param>
        /// <param name="reflectivity">The percentage of light that is reflected by the material</param>
        /// <param name="refractivity">The amount of refraction occurring on rays passing through the material</param>
        /// <param name="glossiness">The glossiness of the material, which impacts shiny specular highlighting</param>
        public TextureMaterial(
            string diffuseFileName,
            string specularFileName,
            float opacity = 1.0f,
            float reflectivity = 0.0f,
            float refractivity = 0.0f,
            float glossiness = 1.0f)
            : base(reflectivity, refractivity, opacity, glossiness)
        {
            this.diffuseTexture = new Texture(diffuseFileName);
            this.specularTexture = new Texture(specularFileName);
        }

        public override Color GetDiffuseColorAtCoordinates(float u, float v)
        {
            return diffuseTexture.GetColorAtUVCoordinate(u, v);
        }

        public override Color GetSpecularColorAtCoordinates(float u, float v)
        {
            if (specularTexture == null)
            {
                return GetDiffuseColorAtCoordinates(u, v);
            }
            else
            {
                return specularTexture.GetColorAtUVCoordinate(u, v);
            }
        }
    }
}
