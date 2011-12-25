using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Mogre;


namespace Mogre.Demo.CgTutorials
{
    public class TextExample : Mogre.Demo.ExampleApplication.Example
    {
        static ManualObject.ManualObjectSection DrawStar(ManualObject obj, float x, float y, int starPoints, float R, float r)
        {
            int i;
            float piOverStarPoints = 3.14159f / starPoints, angle = 0.0f;

            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_FAN;

            obj.Begin("CgTutorials/C3E1_Material", operation);
            obj.Position(x, y, 0.0f);  /* Center of star */
            /* Emit exterior vertices for star's points. */
            for (i = 0; i < starPoints; i++)
            {
                obj.Position(x + R * Math.Cos(angle), y + R * Math.Sin(angle), 0.0f);
                angle += piOverStarPoints;
                obj.Position(x + r * Math.Cos(angle), y + r * Math.Sin(angle), 0.0f);
                angle += piOverStarPoints;
            }
            /* End by repeating first exterior vertex of star. */
            angle = 0;
            obj.Position(x + R * Math.Cos(angle), y + R * Math.Sin(angle), 0.0f);
            return obj.End();
        }

        public override void CreateScene()
        {
            const uint constantColor = 123;

            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Tutorial03Node1");
            SceneNode node2 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Tutorial03Node2");

            ManualObject manualObj1 = sceneMgr.CreateManualObject("Tutorial03Object1");
            ManualObject manualObj2 = sceneMgr.CreateManualObject("Tutorial03Object2");


            /*                                  star    outer   inner  */
            /*                     x      y     Points  radius  radius */
            /*                  =====  =====  ======  ======  ====== */
            ManualObject.ManualObjectSection objSection;
            objSection = DrawStar(manualObj1, -10.0f, -10.0f, 5, 50.0f, 20.0f);
            objSection.SetCustomParameter(constantColor, new Vector4(0.0f, 1.0f, 0.0f, 1.0f));// Green

            objSection = DrawStar(manualObj2, 94.0f, 10.0f, 5, 50.0f, 20.0f);
            objSection.SetCustomParameter(constantColor, new Vector4(1.0f, 0.0f, 0.0f, 1.0f)); // Red

            node1.AttachObject(manualObj1);
            node2.AttachObject(manualObj2);

            MovableText text1 = new MovableText("text1", this.sceneMgr, node1, new Size(128, 32));
            text1.SetText("Node 1!", new System.Drawing.Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular, GraphicsUnit.Pixel), Color.Green);
            text1.Offset = new Vector3(-10.0f + 25f, -10.0f + 25f, 5.0f);

            MovableText text2 = new MovableText("text2", this.sceneMgr, node2, new Size(128, 32));
            text2.SetText("Node 2!", new System.Drawing.Font(FontFamily.GenericSerif, 16, FontStyle.Regular, GraphicsUnit.Pixel), Color.Red);
            text2.Offset = new Vector3(94.0f + 25f, 10.0f + 25f, 5.0f);
        }

        public override void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("PlayerCam");

            // Position it at 30 in Z direction
            camera.Position = new Vector3(40, -300, 90);
            // Look at our figure
            camera.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
            camera.Yaw(new Radian(-Math.HALF_PI/4));
            camera.NearClipDistance = 1;
        }

        public override void SetupResources()
        {
            base.SetupResources();
            ResourceGroupManager.Singleton.AddResourceLocation("./CgResources/Chapter3", "FileSystem", "General");
        }

        public override void CreateViewports()
        {
            base.CreateViewports();
            viewport.BackgroundColour = new ColourValue(0.1f, 0.3f, 0.6f, 0.0f);  /* Blue background */
        }

        
        protected override void HandleInput(FrameEvent evt)
        {
            base.HandleInput(evt);
            if (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_C))
            {
                Vector3 pos = this.camera.Position;
            }
        }
    }

    public sealed class MovableText : IDisposable
    {
        private Billboard billboard;
        private BillboardSet billboardSet;
        private MaterialPtr material;
        private SceneManager sceneMgr;
        private Size size;
        private TexturePtr texture;

        public MovableText(string name, SceneManager sceneMgr, SceneNode node, Size size)
        {
            this.texture = TextureManager.Singleton.CreateManual(
                name + "Texture",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                TextureType.TEX_TYPE_2D,
                (uint)size.Width,
                (uint)size.Height,
                0,
                PixelFormat.PF_A8R8G8B8);

            this.material = MaterialManager.Singleton.Create(name + "Material", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            this.material.GetTechnique(0).GetPass(0).CreateTextureUnitState(this.texture.Name);
            this.material.SetSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
            this.material.SetDepthCheckEnabled(false);

            this.billboardSet = sceneMgr.CreateBillboardSet();
            this.billboardSet.SetMaterialName(this.material.Name);
            this.billboardSet.RenderQueueGroup = (byte)RenderQueueGroupID.RENDER_QUEUE_OVERLAY;

            this.billboard = this.billboardSet.CreateBillboard(Vector3.ZERO);
            this.billboard.SetDimensions(size.Width, size.Height);
            this.billboard.Colour = ColourValue.ZERO;

            node.AttachObject(this.billboardSet);
            this.sceneMgr = sceneMgr;
            this.size = size;
        }

        public Vector3 Offset
        {
            get
            {
                return this.billboard.Position;
            }

            set
            {
                this.billboard.Position = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this.billboardSet.Visible;
            }

            set
            {
                this.billboardSet.Visible = value;
            }
        }

        public void Dispose()
        {
            this.billboardSet.DetachFromParent();
            this.sceneMgr.DestroyBillboardSet(this.billboardSet);
            this.billboardSet.Dispose();
            this.billboardSet = null;
            this.sceneMgr = null;

            this.material.Unload();
            MaterialManager.Singleton.Remove(this.material.Handle);
            this.material.Dispose();
            this.material = null;

            this.texture.Unload();
            TextureManager.Singleton.Remove(this.texture.Handle);
            this.texture.Dispose();
            this.texture = null;

            GC.SuppressFinalize(this);
        }

        public void SetText(string text, System.Drawing.Font font, Color colour)
        {
            using (Bitmap bitmap = new Bitmap(this.size.Width, this.size.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    using (SolidBrush brush = new SolidBrush(colour))
                    {
                        g.Clear(Color.Transparent);
                        g.DrawString(text, font, brush, PointF.Empty);
                        ConvertImageToTexture(bitmap, this.texture.Name, this.size);
                    }
                }
            }
        }

        private static unsafe void ConvertImageToTexture(Bitmap image, string textureName, Size size)
        {
            int width = size.Width;
            int height = size.Height;
            using (ResourcePtr rpt = TextureManager.Singleton.GetByName(textureName))
            {
                using (TexturePtr texture = rpt)
                {
                    HardwarePixelBufferSharedPtr texBuffer = texture.GetBuffer();
                    texBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);
                    PixelBox pb = texBuffer.CurrentLock;

                    System.Drawing.Imaging.BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    CopyMemory(pb.data, data.Scan0, (uint)(width * height * 4));
                    image.UnlockBits(data);

                    texBuffer.Unlock();
                    texBuffer.Dispose();
                }
            }
        }

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, uint Length);
    }

}

