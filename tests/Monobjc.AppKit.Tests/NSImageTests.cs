//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2013 - Laurent Etiemble
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
using System;
using Monobjc.Foundation;
using NUnit.Framework;

namespace Monobjc.AppKit
{
    [TestFixture]
    [Category("NSImage")]
    [Description("Test with NSImage wrapper")]
    public class NSImageTests : WrapperTests
	{
        [Test]
        public void TestStaticCreation()
        {
			NSImage img;
		
            img = NSImage.ImageNamed(NSImage.NSImageNameComputer);
            Check(img);

			img = NSImage.ImageFromResource(typeof(NSImageTests), "Monobjc.AppKit.Tests.Sample.png");
            Check(img);
        }

		[Test]
		public void TestImageSize()
		{
			NSImage img;
			
			img = NSImage.ImageFromResource(typeof(NSImageTests), "Monobjc.AppKit.Tests.Sample.png");
			Check(img);
			Assert.AreEqual(new NSSize(180, 180), img.Size, "Size should be equal");
		}

		[Test]
		public void TestDecryption ()
		{
			NSData data;
			
			data = NSData.DataFromResource(typeof(NSImageTests), "Monobjc.AppKit.Tests.Encrypted.png");
			Check(data);
			Assert.AreEqual(1092, data.Length, "Length should be equal");

			NSImage img = NSImage.ImageFromEncryptedData(data, "123");
			Check(img);
			Assert.AreEqual(new NSSize(32, 32), img.Size, "Size should be equal");
		}
		
		private static void Check(Id @object)
        {
            Assert.IsNotNull(@object, "Instance cannot be null");
            Assert.AreNotEqual(IntPtr.Zero, @object.NativePointer, "Native pointer cannot be null");
        }
	}
}