
#region ================== Copyright (c) 2007 Pascal vd Heiden

/*
 * Copyright (c) 2007 Pascal vd Heiden, www.codeimp.com
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

#region ================== Namespaces

using System;

#endregion

namespace CodeImp.DoomBuilder.Rendering
{
	internal class ShaderManager : IRenderResource, IDisposable
	{
		#region ================== Constants

		#endregion

		#region ================== Variables

		// Shaders
		private VertexDeclaration flatvertexdecl;
        private VertexDeclaration worldvertexdecl;
		
		// Device
		private RenderDevice device;
		
		// Disposing
		private bool isdisposed;

		#endregion

		#region ================== Properties

		public VertexDeclaration FlatVertexDecl { get { return flatvertexdecl; } }
		public VertexDeclaration WorldVertexDecl { get { return worldvertexdecl; } }
		public bool IsDisposed { get { return isdisposed; } }
		internal RenderDevice D3DDevice { get { return device; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public ShaderManager(RenderDevice device)
		{
			// Initialize
			this.device = device;
			
			// Load
			ReloadResource();

			// Register as resource
			device.RegisterResource(this);
			
			// We have no destructor
			GC.SuppressFinalize(this);
		}

		// Disposer
		public void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Clean up
				UnloadResource();

				// Unregister as resource
				device.UnregisterResource(this);
				
				// Done
				device = null;
				isdisposed = true;
			}
		}

		#endregion

		#region ================== Resources

		// Clean up resources
		public void UnloadResource()
		{
            flatvertexdecl.Dispose();
			worldvertexdecl.Dispose();
		}

		// Load resources
		public void ReloadResource()
		{
            flatvertexdecl = new VertexDeclaration(new VertexElement[] {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationUsage.Position),
                new VertexElement(0, 12, DeclarationType.Color, DeclarationUsage.Color),
                new VertexElement(0, 16, DeclarationType.Float2, DeclarationUsage.TextureCoordinate)
            });

            worldvertexdecl = new VertexDeclaration(new VertexElement[] {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationUsage.Position),
                new VertexElement(0, 12, DeclarationType.Color, DeclarationUsage.Color),
                new VertexElement(0, 16, DeclarationType.Float2, DeclarationUsage.TextureCoordinate),
                new VertexElement(0, 24, DeclarationType.Float3, DeclarationUsage.Normal)
            });
        }

        #endregion

        public void SetDisplay2DSettings(float texelx, float texely, float fsaafactor, float alpha, bool bilinear)
        {
            Vector4 values = new Vector4(texelx, texely, fsaafactor, alpha);
            D3DDevice.SetUniform(Uniform.rendersettings, values);
            Matrix world = D3DDevice.GetTransform(TransformState.World);
            Matrix view = D3DDevice.GetTransform(TransformState.View);
            D3DDevice.SetUniform(Uniform.transformsettings, world * view);
            TextureFilter filter = (bilinear ? TextureFilter.Linear : TextureFilter.Point);
            D3DDevice.SetUniform(Uniform.filtersettings, (int)filter);
        }

        public void SetThings2DSettings(float alpha)
        {
            Vector4 values = new Vector4(0.0f, 0.0f, 1.0f, alpha);
            D3DDevice.SetUniform(Uniform.rendersettings, values);
            Matrix world = D3DDevice.GetTransform(TransformState.World);
            Matrix view = D3DDevice.GetTransform(TransformState.View);
            D3DDevice.SetUniform(Uniform.transformsettings, world * view);
        }

        //mxd. Used to render models
        public void SetThings2DTransformSettings(Matrix world)
        {
            Matrix view = D3DDevice.GetTransform(TransformState.View);
            D3DDevice.SetUniform(Uniform.transformsettings, world * view);
        }

        public void SetWorld3DConstants(bool bilinear, float maxanisotropy)
        {
            //mxd. It's still nice to have anisotropic filtering when texture filtering is disabled
            TextureFilter magminfilter = (bilinear ? TextureFilter.Linear : TextureFilter.Point);
            D3DDevice.SetUniform(Uniform.magfiltersettings, magminfilter);
            D3DDevice.SetUniform(Uniform.minfiltersettings, (maxanisotropy > 1.0f ? TextureFilter.Anisotropic : magminfilter));
            D3DDevice.SetUniform(Uniform.mipfiltersettings, (bilinear ? TextureFilter.Linear : TextureFilter.None)); // [SB] use None, otherwise textures are still filtered
            D3DDevice.SetUniform(Uniform.maxanisotropysetting, maxanisotropy);
        }
    }
}
