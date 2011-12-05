/* 
Scenic Graphics Library
Copyright (C) 2007 Jouni Tulkki

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USApackage scenic;*/
using System;
//UPGRADE_TODO: The type 'java.awt.ImageCapabilities' could not be found. If it was not included in the conversion, there may be compiler issues. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1262_3"'
//using ImageCapabilities = java.awt.ImageCapabilities;
//UPGRADE_TODO: The type 'java.awt.image.VolatileImage' could not be found. If it was not included in the conversion, there may be compiler issues. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1262_3"'
//using VolatileImage = java.awt.image.VolatileImage;
namespace Scenic
{
	
	/// <summary> <p>The ScenicImage class is used to store images that can be directly used
	/// by the Scenic library. This class extends the VolatileImage
	/// class, but it also adds several new methods for reading and writing 
	/// data. The image data is stored in video memory to make
	/// hardware acceleration possible. 
	/// 
	/// <p>The format of the image data is specified by the format 
	/// attribute. Different formats are defined in the Format
	/// class.
	/// </summary>
	public class Image //:VolatileImage
	{
        public int getWidth()
        {
            return 0;
        }
        public int getHeight()
        {
            return 0;
        }

#if PENDING
		/// <summary> Gets the format of the image. The format is one of the constants
		/// defined in the Format class.
		/// </summary>
		virtual public int Format
		{
			get
			{
				return format;
			}
			
		}
		virtual internal int Id
		{
			get
			{
				return id;
			}
			
		}
		virtual public System.Drawing.Bitmap Snapshot
		{
			get
			{
				return convertToBufferedImage((int) System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			}
			
		}
		virtual public ImageCapabilities Capabilities
		{
			get
			{
				return null;
			}
			
		}
		
		//UPGRADE_ISSUE: Field 'java.awt.image.BufferedImage.TYPE_BYTE_GRAY' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageBufferedImageTYPE_BYTE_GRAY_f_3"'
		protected internal static System.Drawing.Bitmap dummyImage = new System.Drawing.Bitmap(1, 1, (System.Drawing.Imaging.PixelFormat) BufferedImage.TYPE_BYTE_GRAY);
		protected internal static System.Drawing.Graphics dummyGraphics = System.Drawing.Graphics.FromImage(dummyImage);
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
		private static Map < java.awt.Image, Image > imageCache = 
		new WeakHashMap < java.awt.Image, Image >();
		private static long totalMemoryUsage;
		private static long garbageCollectionLimit = 64 * 1024 * 1024;
		
		private int id;
		private int width;
		private int height;
		private int format;
		// Read from native code
		private int options;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
		private LinkedList < ImageObserver > observers = new LinkedList < ImageObserver >();
		
		/// <summary>Constructs an empty image</summary>
		public Image()
		{
			create(0, 0, 0);
		}
		
		/// <summary> Constructs an image with given parameters. 
		/// 
		/// </summary>
		/// <param name="width">the width of the image.
		/// </param>
		/// <param name="height">the height of the image.
		/// </param>
		/// <param name="format">the format of the image (must be one of the 
		/// constants defined in the Format class).
		/// </param>
		public Image(int width, int height, int format)
		{
			create(width, height, format);
		}
		
		/// <summary> Constructs an image with given parameters. 
		/// 
		/// </summary>
		/// <param name="width">the width of the image.
		/// </param>
		/// <param name="height">the height of the image.
		/// </param>
		/// <param name="format">the format of the image (must be one of the 
		/// constants defined in the Format class).
		/// </param>
		/// <param name="options">options for the image (must be a combination
		/// of the constants in ImageOptions class). 
		/// </param>
		public Image(int width, int height, int format, int options)
		{
			create(width, height, format, options);
		}
		
		//	private static class ImageUpdater implements 
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
		private int getScenicFormat(int type, ref System.Drawing.Color model)
		{
			if (type == (int) System.Drawing.Imaging.PixelFormat.Format24bppRgb)
				return Format.R8G8B8;
			if (type == (int) System.Drawing.Imaging.PixelFormat.Format32bppRgb)
				return Format.X8R8G8B8;
			if (type == (int) System.Drawing.Imaging.PixelFormat.Format32bppArgb)
				return Format.A8R8G8B8;
			//UPGRADE_ISSUE: Field 'java.awt.image.BufferedImage.TYPE_BYTE_GRAY' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageBufferedImageTYPE_BYTE_GRAY_f_3"'
			if (type == BufferedImage.TYPE_BYTE_GRAY)
				return Format.L8;
			if (type == (int) System.Drawing.Imaging.PixelFormat.Format24bppRgb)
				return Format.A8R8G8B8;
			if (type == (int) System.Drawing.Imaging.PixelFormat.Undefined)
			{
				if (model is System.Drawing.Color)
				{
					System.Drawing.Color m = (System.Drawing.Color) model;
					
					//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.image.DirectColorModel.getRedMask' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043_3"'
					//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.image.DirectColorModel.getGreenMask' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043_3"'
					//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.image.DirectColorModel.getBlueMask' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043_3"'
					//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.image.DirectColorModel.getAlphaMask' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043_3"'
					if (SupportClass.ColorSupport.GetRedMask() == 0x00ff0000 && SupportClass.ColorSupport.GetGreenMask() == 0x0000ff00 && SupportClass.ColorSupport.GetBlueMask() == 0x000000ff && (int) SupportClass.ColorSupport.GetAlphaMask() == 0x01000000)
					{
						return Format.A1R8G8B8;
					}
				}
				throw new System.ArgumentException("Unsupported custom color model (" + model + ")");
			}
			throw new System.ArgumentException("Unsupported image type (" + type + ")");
		}
		
		/// <summary> Creates an image from the given BufferedImage. 
		/// 
		/// </summary>
		/// <param name="image">the source image.
		/// </param>
		public Image(System.Drawing.Bitmap image)
		{
			//		System.out.println("new ScenicImage from BufferedImage " + image);
			//UPGRADE_ISSUE: Method 'java.awt.image.BufferedImage.getColorModel' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageBufferedImagegetColorModel_3"'
			System.Drawing.Color tempAux = image.getColorModel();
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1303_3"'
			int fmt = getScenicFormat((int) image.PixelFormat, ref tempAux);
			
			create(image.Width, image.Height, fmt);
			
			writeImage(image);
		}
		
		//UPGRADE_TODO: Class 'java.awt.image.SampleModel' was converted to 'System.Drawing.Bitmap' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtimageSampleModel_3"'
		private int getPixelSize(System.Drawing.Bitmap model)
		{
			//UPGRADE_ISSUE: Method 'java.awt.image.SampleModel.getNumDataElements' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageSampleModelgetNumDataElements_3"'
			int s = model.getNumDataElements();
			
			//UPGRADE_ISSUE: Method 'java.awt.image.DataBuffer.getDataTypeSize' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageDataBuffergetDataTypeSize_int_3"'
			//UPGRADE_ISSUE: Method 'java.awt.image.SampleModel.getDataType' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageSampleModelgetDataType_3"'
			return s * DataBuffer.getDataTypeSize(model.getDataType()) / 8;
		}
		
		internal virtual void  writeImage(System.Drawing.Bitmap image)
		{
			//UPGRADE_TODO: Class 'java.awt.image.WritableRaster' was converted to 'System.Drawing.Bitmap' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtimageWritableRaster_3"'
			System.Drawing.Bitmap raster = image;
			System.IO.MemoryStream tempDataBuffer;
			tempDataBuffer = new System.IO.MemoryStream();
			//UPGRADE_TODO: Method 'java.awt.image.Raster.getDataBuffer' was converted to 'System.Drawing.Bitmap' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtimageRastergetDataBuffer_3"'
			raster.Save(tempDataBuffer, System.Drawing.Imaging.ImageFormat.Bmp);
			System.Collections.ArrayList buffer = new System.Collections.ArrayList(tempDataBuffer.ToArray());
			//UPGRADE_TODO: Class 'java.awt.image.SampleModel' was converted to 'System.Drawing.Bitmap' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtimageSampleModel_3"'
			System.Drawing.Bitmap sampleModel = raster;
			int sampleSize = getPixelSize(sampleModel);
			//UPGRADE_ISSUE: Method 'java.awt.image.Raster.getSampleModelTranslateX' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageRastergetSampleModelTranslateX_3"'
			//UPGRADE_ISSUE: Method 'java.awt.image.Raster.getSampleModelTranslateY' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageRastergetSampleModelTranslateY_3"'
			int offset = (- raster.getSampleModelTranslateX() - raster.getSampleModelTranslateY() * sampleModel.Width) * sampleSize;
			int pitch = sampleModel.Width * sampleSize;
			
			//		System.out.println("sample size " + sampleSize + " offset " + offset + " pitch " + pitch);
			
			if ((int) image.PixelFormat == (int) System.Drawing.Imaging.PixelFormat.Format24bppRgb)
			{
				//UPGRADE_ISSUE: Method 'java.awt.image.BufferedImage.getColorModel' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageBufferedImagegetColorModel_3"'
				SupportClass.IndexedColorArray cm = (SupportClass.IndexedColorArray) image.getColorModel();
				int w = image.Width;
				int h = image.Height;
				int[] colors = new int[256];
				int[] data = new int[w * h];
				sbyte[] src = SupportClass.ToSByteArray((byte[]) ((System.Collections.ArrayList) buffer)[0]);
				
				//UPGRADE_ISSUE: Method 'java.awt.image.IndexColorModel.getRGBs' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageIndexColorModelgetRGBs_int[]_3"'
				cm.getRGBs(colors);
				for (int i = 0; i < w * h; i++)
				{
					data[i] = colors[(int) src[i] & 0xff];
				}
				write(0, 0, width, height, data, Format.A8R8G8B8);
				return ;
			}
			
			
			if (buffer is System.Collections.ArrayList)
			{
				System.Collections.ArrayList byteBuffer = (System.Collections.ArrayList) buffer;
				write(0, 0, width, height, SupportClass.ToSByteArray((byte[]) byteBuffer[0]), format, offset, pitch);
			}
			else if (buffer is System.Collections.ArrayList)
			{
				System.Collections.ArrayList intBuffer = (System.Collections.ArrayList) buffer;
				
				write(0, 0, width, height, (int[]) intBuffer[0], format, offset / 4, pitch / 4);
			}
			else
				throw new System.ArgumentException("Unsupported DataBuffer type");
		}
		
		//UPGRADE_ISSUE: Interface 'java.awt.image.ImageObserver' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageImageObserver_3"'
		//UPGRADE_ISSUE: Interface 'java.awt.image.TileObserver' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageTileObserver_3"'
		public class ImageUpdater : java.awt.image.ImageObserver, TileObserver
		{
			public System.Drawing.Image source;
			
			public ImageUpdater(System.Drawing.Image source)
			{
				this.source = source;
			}
			
			public virtual bool imageUpdate(System.Drawing.Image img, int infoflags, int x, int y, int width, int height)
			{
				updateImage();
				return true;
			}
			
			//UPGRADE_ISSUE: Interface 'java.awt.image.WritableRenderedImage' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageWritableRenderedImage_3"'
			public virtual void  tileUpdate(WritableRenderedImage source, int tileX, int tileY, bool willBeWritable)
			{
				updateImage();
			}
			
			private void  updateImage()
			{
				Image simg = imageCache.get_Renamed(source);
				
				if (simg != null)
				{
					simg.writeImage(scenic.Image.convertToBufferedImage(source));
				}
			}
		}
		
		public static System.Drawing.Bitmap convertToBufferedImage(System.Drawing.Image img)
		{
			if (img is System.Drawing.Bitmap)
			{
				return (System.Drawing.Bitmap) img;
			}
			else
			{
				int w = img.Width;
				int h = img.Height;
				System.Drawing.Bitmap bimg;
				
				if (w == 0 || h == 0)
					return null;
				
				bimg = new System.Drawing.Bitmap(w, h, (System.Drawing.Imaging.PixelFormat) System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1101_3"'
				System.Drawing.Graphics.FromImage(bimg).DrawImage(img, 0, 0);
				return bimg;
			}
		}
		
		/// <summary> Tries to get a cached ScenicImage that has the content of the
		/// given BufferedImage. If the cache does not contain the desired image
		/// then a new image is created and added to the cache.
		/// 
		/// </summary>
		/// <param name="img">BufferedImage
		/// </param>
		/// <returns> ScenicImage
		/// </returns>
		public static Image convert(System.Drawing.Image img)
		{
			if (img is Image)
				return (Image) img;
			
			Image simg = imageCache.get_Renamed(img);
			
			if (simg == null)
			{
				simg = new Image(convertToBufferedImage(img));
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.prepareImage' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtToolkit_3"'
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtToolkit_3"'
				Toolkit.getDefaultToolkit().prepareImage(img, - 1, - 1, new ImageUpdater(img));
				imageCache.put(img, simg);
			}
			
			return simg;
		}
		
		public static void  update(Image img)
		{
			Image simg = imageCache.get_Renamed(img);
			
			if (simg != null)
				simg.writeImage(convertToBufferedImage(img));
		}
		
		/// <summary> Converts the image to a java.awt.image.BufferedImage object.
		/// The type of the buffered image must be either BufferedImage.TYPE_4BYTE_ABGR
		/// or BufferedImage.TYPE_3BYTE_BGR.
		/// 
		/// </summary>
		/// <param name="type">type of the image
		/// </param>
		/// <returns> the BufferedImage object
		/// </returns>
		public virtual System.Drawing.Bitmap convertToBufferedImage(int type)
		{
			int pixelFormat;
			
			if (type == (int) System.Drawing.Imaging.PixelFormat.Format24bppRgb)
				pixelFormat = Format.R8G8B8;
			else if (type == (int) System.Drawing.Imaging.PixelFormat.Format32bppArgb)
				pixelFormat = Format.R8G8B8A8;
			else
				throw new System.ArgumentException();
			
			
			System.Drawing.Bitmap image = new System.Drawing.Bitmap(width, height, (System.Drawing.Imaging.PixelFormat) type);
			System.IO.MemoryStream tempDataBuffer;
			tempDataBuffer = new System.IO.MemoryStream();
			//UPGRADE_TODO: Method 'java.awt.image.Raster.getDataBuffer' was converted to 'System.Drawing.Bitmap' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaawtimageRastergetDataBuffer_3"'
			image.Save(tempDataBuffer, System.Drawing.Imaging.ImageFormat.Bmp);
			System.Collections.ArrayList buffer = new System.Collections.ArrayList(tempDataBuffer.ToArray());
			System.Collections.ArrayList byteBuffer = (System.Collections.ArrayList) buffer;
			
			read(0, 0, width, height, SupportClass.ToSByteArray((byte[]) byteBuffer[0]), pixelFormat);
			return image;
		}
		
		/// <summary> Converts the image to a java.awt.image.BufferedImage object.
		/// The type of the buffered image is BufferedImage.TYPE_3BYTE_BGR.
		/// 
		/// </summary>
		/// <returns> the BufferedImage object
		/// </returns>
		public virtual System.Drawing.Bitmap convertToBufferedImage()
		{
			return convertToBufferedImage((int) System.Drawing.Imaging.PixelFormat.Format24bppRgb);
		}
		
		~Image()
		{
			free();
		}
		
		/// <summary> Gets the width of the image.</summary>
		public virtual int getWidth()
		{
			return width;
		}
		
		/// <summary> Gets the height of the image.</summary>
		public virtual int getHeight()
		{
			return height;
		}
		
		/// <summary> Creates an image with the given parameters. The new image replaces
		/// the old image. Pixel data is not copied to the new image.
		/// 
		/// </summary>
		/// <param name="width">the width of the image.
		/// </param>
		/// <param name="height">the height of the image.
		/// </param>
		/// <param name="format">the format of the image (must be one of the 
		/// constants defined in the Format class).
		/// </param>
		/// <param name="options">options for the image (must be one of the 
		/// constants defined in the ImageOptions class).
		/// </param>
		public virtual void  create(int width, int height, int format, int options)
		{
			totalMemoryUsage -= this.width * this.height * Format.getPixelSize(this.format);
			this.width = width;
			this.height = height;
			this.format = format;
			this.options = options;
			id = nativeCreate(id, width, height, format, options);
			markChanged();
			
			/* 
			* Keep track of total amount of native memory and initiate garbage collection
			* when the amount of new allocated memory exceeds certain limit. 
			*/
			totalMemoryUsage += width * height * Format.getPixelSize(format);
			//		System.out.println("totalMemoryUsage: " + totalMemoryUsage / 1024 / 1024.0);
			if (totalMemoryUsage > garbageCollectionLimit)
			{
				System.GC.Collect();
				totalMemoryUsage = 0;
			}
		}
		
		/// <summary> Creates an image with the given parameters. The new image replaces
		/// the old image. Pixel data is not copied to the new image.
		/// 
		/// </summary>
		/// <param name="width">the width of the image.
		/// </param>
		/// <param name="height">the height of the image.
		/// </param>
		/// <param name="format">the format of the image (must be one of the 
		/// constants defined in the Format class).
		/// </param>
		public virtual void  create(int width, int height, int format)
		{
			create(width, height, format, 0);
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'addObserver'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027_3"'
		public virtual void  addObserver(ImageObserver observer)
		{
			lock (this)
			{
				observers.add(observer);
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'deleteObserver'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027_3"'
		public virtual void  deleteObserver(ImageObserver observer)
		{
			lock (this)
			{
				observers.remove(observer);
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'markChanged'. Lock expression was added. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1027_3"'
		private void  markChanged()
		{
			lock (this)
			{
				//		setChanged();
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1156_3"'
				foreach(ImageObserver n in observers) 
				n.update(this);
				//		clearChanged();		
			}
		}
		
		/// <summary> Checks if the contents of the image are lost. Some image types
		/// are volatile, meaning that their contents can be lost. Images that
		/// are used as render targets can be lost when using DirectX. On OpenGL
		/// images are never lost. For compatibility, you should always check
		/// if the contents are lost when you are using render target images.
		/// 
		/// </summary>
		/// <returns> true if the contents of the image are lost
		/// </returns>
		public virtual bool contentsLost()
		{
			return nativeContentsLost(id);
		}
		
		/// <summary> Frees the resources associated with the image.</summary>
		public virtual void  free()
		{
			totalMemoryUsage -= width * height * Format.getPixelSize(format);
			if (totalMemoryUsage < 0)
				totalMemoryUsage = 0;
			this.width = 0;
			this.height = 0;
			this.format = 0;
			
			if (id != 0)
			{
				nativeFree(id);
				id = 0;
			}
			markChanged();
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'WriteCommand' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019_3"'
		private class WriteCommand
		{
			private void  InitBlock(Image enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Image enclosingInstance;
			public Image Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private int x;
			private int y;
			private int width;
			private int height;
			private int format;
			private int offset;
			private int pitch;
			private System.Object data;
			
			public WriteCommand(Image enclosingInstance, int x, int y, int width, int height, System.Object data, int offset, int pitch, int format)
			{
				InitBlock(enclosingInstance);
				this.x = x;
				this.y = y;
				this.width = width;
				this.height = height;
				this.data = data;
				this.offset = offset;
				this.pitch = pitch;
				this.format = format;
			}
			
			public virtual void  execute()
			{
				if (data is sbyte[])
					Enclosing_Instance.nativeWriteByte(Enclosing_Instance.id, x, y, width, height, (sbyte[]) data, offset, pitch, format);
				else if (data is int[])
					Enclosing_Instance.nativeWriteInt(Enclosing_Instance.id, x, y, width, height, (int[]) data, offset, pitch, format);
				else if (data is float[])
					Enclosing_Instance.nativeWriteFloat(Enclosing_Instance.id, x, y, width, height, (float[]) data, offset, pitch, format);
			}
			
			public virtual void  waitCompletion()
			{
				execute();
			}
		}
		
		/// <summary> Writes the pixel data of the image in the given rectangle.
		/// 
		/// </summary>
		/// <param name="x">the left side of the rectangle
		/// </param>
		/// <param name="y">the top of the rectangle
		/// </param>
		/// <param name="width">the width of the rectangle
		/// </param>
		/// <param name="height">the height of the rectangle
		/// </param>
		/// <param name="data">the pixel data
		/// </param>
		/// <param name="format">format of the pixel data
		/// </param>
		public virtual void  write(int x, int y, int width, int height, sbyte[] data, int format)
		{
			write(x, y, width, height, data, format, 0, 0);
		}
		
		public virtual void  write(int x, int y, int width, int height, sbyte[] data, int format, int offset, int pitch)
		{
			new WriteCommand(this, x, y, width, height, data, offset, pitch, format).waitCompletion();
			markChanged();
		}
		
		/// <summary> Writes the pixel data of the image in the given rectangle.
		/// 
		/// </summary>
		/// <param name="x">the left side of the rectangle
		/// </param>
		/// <param name="y">the top of the rectangle
		/// </param>
		/// <param name="width">the width of the rectangle
		/// </param>
		/// <param name="height">the height of the rectangle
		/// </param>
		/// <param name="data">the pixel data
		/// </param>
		/// <param name="format">format of the pixel data
		/// </param>
		public virtual void  write(int x, int y, int width, int height, int[] data, int format)
		{
			write(x, y, width, height, data, format, 0, 0);
		}
		
		public virtual void  write(int x, int y, int width, int height, int[] data, int format, int offset, int pitch)
		{
			new WriteCommand(this, x, y, width, height, data, offset, pitch, format).waitCompletion();
			markChanged();
		}
		
		/// <summary> Writes the pixel data of the image in the given rectangle.
		/// 
		/// </summary>
		/// <param name="x">the left side of the rectangle
		/// </param>
		/// <param name="y">the top of the rectangle
		/// </param>
		/// <param name="width">the width of the rectangle
		/// </param>
		/// <param name="height">the height of the rectangle
		/// </param>
		/// <param name="data">the pixel data
		/// </param>
		/// <param name="format">format of the pixel data
		/// </param>
		public virtual void  write(int x, int y, int width, int height, float[] data, int format)
		{
			write(x, y, width, height, data, format, 0, 0);
		}
		
		public virtual void  write(int x, int y, int width, int height, float[] data, int format, int offset, int pitch)
		{
			new WriteCommand(this, x, y, width, height, data, offset, pitch, format).waitCompletion();
			markChanged();
		}
		/// <summary> Reads the pixel data of the image in the given rectangle.
		/// 
		/// </summary>
		/// <param name="x">the left side of the rectangle
		/// </param>
		/// <param name="y">the top of the rectangle
		/// </param>
		/// <param name="width">the width of the rectangle
		/// </param>
		/// <param name="height">the height of the rectangle
		/// </param>
		/// <param name="data">the pixel data
		/// </param>
		/// <param name="format">format of the pixel data
		/// </param>
		public virtual void  read(int x, int y, int width, int height, sbyte[] data, int format)
		{
			nativeReadByte(id, x, y, width, height, data, 0, 0, format);
		}
		
		/// <summary> Reads the pixel data of the image in the given rectangle.
		/// 
		/// </summary>
		/// <param name="x">the left side of the rectangle
		/// </param>
		/// <param name="y">the top of the rectangle
		/// </param>
		/// <param name="width">the width of the rectangle
		/// </param>
		/// <param name="height">the height of the rectangle
		/// </param>
		/// <param name="data">the pixel data
		/// </param>
		/// <param name="format">format of the pixel data
		/// </param>
		public virtual void  read(int x, int y, int width, int height, int[] data, int format)
		{
			nativeReadInt(id, x, y, width, height, data, 0, 0, format);
		}
		
		/// <summary> Reads the pixel data of the image in the given rectangle.
		/// 
		/// </summary>
		/// <param name="x">the left side of the rectangle
		/// </param>
		/// <param name="y">the top of the rectangle
		/// </param>
		/// <param name="width">the width of the rectangle
		/// </param>
		/// <param name="height">the height of the rectangle
		/// </param>
		/// <param name="data">the pixel data
		/// </param>
		/// <param name="format">format of the pixel data
		/// </param>
		public virtual void  read(int x, int y, int width, int height, float[] data, int format)
		{
			nativeReadFloat(id, x, y, width, height, data, 0, 0, format);
		}
		
		private extern int nativeCreate(int id, int width, int height, int format, int options);
		private extern void  nativeFree(int id);
		private extern bool nativeContentsLost(int id);
		private extern void  nativeWriteByte(int id, int x, int y, int width, int height, sbyte[] data, int offset, int pitch, int format);
		private extern void  nativeWriteInt(int id, int x, int y, int width, int height, int[] data, int offset, int pitch, int format);
		private extern void  nativeWriteFloat(int id, int x, int y, int width, int height, float[] data, int offset, int pitch, int format);
		private extern void  nativeReadByte(int id, int x, int y, int width, int height, sbyte[] data, int offset, int pitch, int format);
		private extern void  nativeReadInt(int id, int x, int y, int width, int height, int[] data, int offset, int pitch, int format);
		private extern void  nativeReadFloat(int id, int x, int y, int width, int height, float[] data, int offset, int pitch, int format);
		public virtual System.Drawing.Graphics createGraphics()
		{
			return null;
		}
		//UPGRADE_ISSUE: Class 'java.awt.GraphicsConfiguration' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtGraphicsConfiguration_3"'
		public virtual int validate(GraphicsConfiguration gc)
		{
			return 0;
		}
		//UPGRADE_ISSUE: Interface 'java.awt.image.ImageObserver' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageImageObserver_3"'
		public virtual int getWidth(java.awt.image.ImageObserver observer)
		{
			return width;
		}
		//UPGRADE_ISSUE: Interface 'java.awt.image.ImageObserver' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageImageObserver_3"'
		public virtual int getHeight(java.awt.image.ImageObserver observer)
		{
			return height;
		}
		//UPGRADE_ISSUE: Interface 'java.awt.image.ImageObserver' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtimageImageObserver_3"'
		public virtual System.Object getProperty(System.String name, java.awt.image.ImageObserver observer)
		{
			return UndefinedProperty;
		}
		static Image()
		{
			{
				scenic.jni.RenderCanvas.loadLibrary();
			}
		}
#endif
	}
}