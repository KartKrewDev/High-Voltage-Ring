using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeImp.DoomBuilder.Rendering
{
    public class Effect
    {
        public static Effect FromStream(Stream stream, ShaderFlags flags, out string errors)
        {
            errors = "";
            return new Effect();
        }

        public void SetTexture(EffectHandle handle, BaseTexture texture)
        {
        }

        public void SetValue<T>(EffectHandle handle, T value) where T : struct
        {
        }

        public EffectHandle GetParameter(EffectHandle parameter, string name)
        {
            if (!Parameters.ContainsKey(name))
            {
                Parameters[name] = new EffectHandle();
            }
            return Parameters[name];
        }

        public void CommitChanges()
        {
        }

        public void Begin()
        {
        }

        public void BeginPass(int index)
        {
        }

        public void EndPass()
        {
        }

        public void End()
        {
        }

        public void Dispose()
        {
        }

        Dictionary<string, EffectHandle> Parameters = new Dictionary<string, EffectHandle>();
    }

    public class EffectHandle
    {
        public void Dispose()
        {
        }
    }
}
