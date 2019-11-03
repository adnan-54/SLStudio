using System;
using System.Windows.Media.Effects;

namespace SLStudio.Studio.Core.Framework.ShaderEffects
{
    internal static class ShaderEffectUtility
    {
        public static PixelShader GetPixelShader(string name)
        {
            return new PixelShader
            {
                UriSource = new Uri(@"pack://application:,,,/SLStudio.Studio.Core;component/Framework/ShaderEffects/" + name + ".ps")
            };
        }
    }
}