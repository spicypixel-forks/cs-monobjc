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
using System.Runtime.InteropServices;
using Monobjc.Types;
using NUnit.Framework;

namespace Monobjc
{
    [TestFixture]
    [Category("Runtime")]
    [Category("Exposal")]
    [Category("Messaging")]
    [Description("Test messaging to the exposed classes")]
    public class ClassesMessagingTests : RuntimeTests
    {
        [Test]
        public void TestVoidMessaging()
        {
            IntPtr instance = objc_sendMsg_IntPtr(Class.Get(typeof (MessageTest01)).NativePointer, this.sel_alloc);
            Assert.AreNotEqual(IntPtr.Zero, instance, "Instance allocation failed");
            instance = objc_sendMsg_IntPtr(instance, this.sel_init);
            Assert.AreNotEqual(IntPtr.Zero, instance, "Instance initialization failed");

            objc_sendMsg_void(instance, this.sel_release);
            Assert.IsTrue(true);
        }

        [Test]
        public void TestBoolMessaging()
        {
            IntPtr instance = objc_sendMsg_IntPtr(Class.Get(typeof (MessageTest01)).NativePointer, this.sel_alloc);
            Assert.AreNotEqual(IntPtr.Zero, instance, "Instance allocation failed");
            instance = objc_sendMsg_IntPtr(instance, this.sel_init);
            Assert.AreNotEqual(IntPtr.Zero, instance, "Instance initialization failed");

            IntPtr sel = sel_registerName("doWithBool:");

            bool value1 = true;
            bool value2 = objc_sendMsg_bool_bool(instance, sel, value1);
            Assert.AreEqual(value1, value2, "Bool values must be equal");

            value1 = false;
            value2 = objc_sendMsg_bool_bool(instance, sel, value1);
            Assert.AreEqual(value1, value2, "Bool values must be equal");

            objc_sendMsg_void(instance, this.sel_release);
            Assert.IsTrue(true);
        }

        [Test]
        public void TestShortMessaging()
        {
            MessageTest01 instance = new MessageTest01();

            short value1 = (short) new Random().Next(-5000, 5000);
            short value2 = ObjectiveCRuntime.SendMessage<short>(instance, "doWithShort:", value1);
            Assert.AreEqual(value1, value2, "Short values must be equal");

            instance.Release();
            Assert.IsTrue(true);
        }

        [Test]
        public void TestIntMessaging()
        {
            MessageTest01 instance = new MessageTest01();

            int value1 = new Random().Next(-65000, 65000);
            int value2 = ObjectiveCRuntime.SendMessage<int>(instance, "doWithInt:", value1);
            Assert.AreEqual(value1, value2, "Int values must be equal");

            instance.Release();
            Assert.IsTrue(true);
        }

        [Test]
        public void TestLongMessaging()
        {
            MessageTest01 instance = new MessageTest01();

            long value1 = new Random().Next(-65000, 65000);
            long value2 = ObjectiveCRuntime.SendMessage<long>(instance, "doWithLong:", value1);
            Assert.AreEqual(value1, value2, "Long values must be equal");

            instance.Release();
            Assert.IsTrue(true);
        }

		[Test]
		public void TestByRefMessaging()
		{
			// Create a new instance of a test class, then use the
			// wrapper to invoke it from Objective-C.
			var testClass = new MessageTestByRef();
			var testClassWrapper = new MessageTestByRefWrapper(testClass.NativePointer);

			bool mp1_arg1 = false;
			testClassWrapper.MethodParameter1(ref mp1_arg1);
			Assert.IsTrue(mp1_arg1);

			char mp2_arg1 = 'a';
			short mp2_arg2 = -2;
			int mp2_arg3 = -3;
			long mp2_arg4 = -4;
			int mp2_result = testClassWrapper.MethodParameter2(ref mp2_arg1, ref mp2_arg2, ref mp2_arg3, ref mp2_arg4);
			Assert.AreEqual(0, mp2_result);
			Assert.AreEqual('A', mp2_arg1);
			Assert.AreEqual(-20, mp2_arg2);
			Assert.AreEqual(-30, mp2_arg3);
			Assert.AreEqual(-40, mp2_arg4);

			byte mp3_arg1 = 1;
			ushort mp3_arg2 = 2;
			uint mp3_arg3 = 3;
			ulong mp3_arg4 = 4;
			testClassWrapper.MethodParameter3(ref mp3_arg1, ref mp3_arg2, ref mp3_arg3, ref mp3_arg4);
			Assert.AreEqual(10, mp3_arg1);
			Assert.AreEqual(20, mp3_arg2);
			Assert.AreEqual(30, mp3_arg3);
			Assert.AreEqual(40, mp3_arg4);

			IntPtr mp4_arg1 = new IntPtr(10);
			Class mp4_arg2 = Class.Get("NSObject");
			Id mp4_arg3 = Class.Get("NSObject");
			testClassWrapper.MethodParameter4(ref mp4_arg1, ref mp4_arg2, ref mp4_arg3);
			Assert.AreEqual(100, mp4_arg1.ToInt32());
			Assert.AreSame(Class.Get("NSString"), mp4_arg2);
			Assert.AreSame(Class.Get("NSString"), mp4_arg3);

			TSWindingRule mp5_arg1 = TSWindingRule.NSEvenOddWindingRule;
			testClassWrapper.MethodParameter5(ref mp5_arg1);
			Assert.AreEqual(TSWindingRule.NSNonZeroWindingRule, mp5_arg1);

			float mp6_arg1 = 1.0f;
			double mp6_arg2 = 2.0;
			testClassWrapper.MethodParameter6(ref mp6_arg1, ref mp6_arg2);
			Assert.AreEqual(10.0f, mp6_arg1);
			Assert.AreEqual(20.0, mp6_arg2);
		}

        /*
		[Test]
		public void TestMessagingException()
		{
            IntPtr instance = objc_sendMsg_IntPtr(Class.Get(typeof(MessageTest02)).NativePointer, this.sel_alloc);
            Assert.AreNotEqual(IntPtr.Zero, instance, "Instance allocation failed");
            instance = objc_sendMsg_IntPtr(instance, this.sel_init);
            Assert.AreNotEqual(IntPtr.Zero, instance, "Instance initialization failed");

			//IntPtr sel = sel_registerName("generateNotImplementedException");
			//objc_sendMsg_void(instance, sel);
			ObjectiveCRuntime.SendMessage(instance, "generateNotImplementedException");
			
			//Assert.Throws<NotImplementedException>( () => ObjectiveCRuntime.SendMessage(instance, "generateNotImplementedException") );
			//Assert.Throws<NotImplementedException>( () => ObjectiveCRuntime.SendMessage(instance, "generateNotImplementedExceptionWithValue:", 123) );
			
			objc_sendMsg_void(instance, sel_release);
			Assert.IsTrue(true);
		}
        */
    }

    [ObjectiveCClass]
    public class MessageTest01 : TSObject
    {
        public MessageTest01() {}

        public MessageTest01(IntPtr value) : base(value) {}

        [ObjectiveCMessage("doWithBool:")]
        public bool DoWithBool(bool value)
        {
            return value;
        }

        [ObjectiveCMessage("doWithShort:")]
        public short DoWithShort(short value)
        {
            return value;
        }

        [ObjectiveCMessage("doWithInt:")]
        public int DoWithShort(int value)
        {
            return value;
        }

        [ObjectiveCMessage("doWithLong:")]
        public long DoWithShort(long value)
        {
            return value;
        }
    }

    [ObjectiveCClass]
    public class MessageTest02 : TSObject
    {
        public MessageTest02() {}

        public MessageTest02(IntPtr value) : base(value) {}

        [ObjectiveCMessage("generateNotImplementedException")]
        public void GenerateNotImplementedException()
        {
            //throw new NotImplementedException("TEST");
        }

        [ObjectiveCMessage("generateNotImplementedExceptionWithValue:")]
        public void GenerateNotImplementedException(int value)
        {
            //throw new NotImplementedException("value=" + value);
        }
    }

	[ObjectiveCClass("MessageTestByRef")]
	public class MessageTestByRef : TSObject
	{
		public MessageTestByRef() {}

		public MessageTestByRef(IntPtr value) : base(value) {}

		[ObjectiveCMessage("methodParameter1")]
		public void MethodParameter1(ref bool arg1) 
		{
			Assert.IsFalse(arg1);
			arg1 = true;
		}

		[ObjectiveCMessage("methodParameter2")]
		public int MethodParameter2(ref char arg1, ref short arg2, ref int arg3, ref long arg4)
		{
			Assert.AreEqual('a', arg1);
			Assert.AreEqual(-2, arg2);
			Assert.AreEqual(-3, arg3);
			Assert.AreEqual(-4, arg4);
			arg1 = 'A';
			arg2 = -20;
			arg3 = -30;
			arg4 = -40;
			return 0;
		}

		[ObjectiveCMessage("methodParameter3")]
		public void MethodParameter3(ref byte arg1, ref ushort arg2, ref uint arg3, ref ulong arg4) 
		{
			Assert.AreEqual(1, arg1);
			Assert.AreEqual(2, arg2);
			Assert.AreEqual(3, arg3);
			Assert.AreEqual(4, arg4);
			arg1 = 10;
			arg2 = 20;
			arg3 = 30;
			arg4 = 40;
		}

		[ObjectiveCMessage("methodParameter4")]
		public int MethodParameter4(ref IntPtr arg1, ref Class arg2, ref Id arg3)
		{
			Assert.AreEqual(10, arg1.ToInt32());
			Assert.AreSame(Class.Get("NSObject"), arg2);
			Assert.AreSame(Class.Get("NSObject"), arg3);
			arg1 = new IntPtr(100);
			arg2 = Class.Get("NSString");
			arg3 = Class.Get("NSString");
			return 0;
		}

		[ObjectiveCMessage("methodParameter5")]
		public int MethodParameter5(ref TSWindingRule arg1)
		{
			Assert.AreEqual(TSWindingRule.NSEvenOddWindingRule, arg1);
			arg1 = TSWindingRule.NSNonZeroWindingRule;
			return 0;
		}

		[ObjectiveCMessage("methodParameter6")]
		public int MethodParameter6(ref float arg1, ref double arg2)
		{
			Assert.AreEqual(1.0f, arg1);
			Assert.AreEqual(2.0, arg2);
			arg1 = 10f;
			arg2 = 20.0;
			return 0;
		}
	}

	public class MessageTestByRefWrapper
	{
		public MessageTestByRefWrapper() {}

		public MessageTestByRefWrapper(IntPtr value) 
		{
			NativePointer = value;
		}

		public IntPtr NativePointer { get; private set; }

		[ObjectiveCMessage("methodParameter1")]
		public void MethodParameter1(ref bool arg1) 
		{
			IntPtr local1 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(bool)));

			Marshal.WriteByte(local1, Convert.ToByte(arg1));

			ObjectiveCRuntime.SendMessage(
				NativePointer, "methodParameter1", 
				local1);

			arg1 = Convert.ToBoolean(Marshal.ReadByte(local1));

			Marshal.FreeHGlobal(local1);
		}

		[ObjectiveCMessage("methodParameter2")]
		public int MethodParameter2(ref char arg1, ref short arg2, ref int arg3, ref long arg4)
		{
			IntPtr local1 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(char)));
			IntPtr local2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(short)));
			IntPtr local3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
			IntPtr local4 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(long)));

			Marshal.WriteInt16(local1, arg1);
			Marshal.WriteInt16(local2, arg2);
			Marshal.WriteInt32(local3, arg3);
			Marshal.WriteInt64(local4, arg4);

			var result = ObjectiveCRuntime.SendMessage<int>(
				NativePointer, "methodParameter2", 
				local1,
				local2,
				local3,
				local4);

			arg1 = Convert.ToChar(Marshal.ReadInt16(local1));
			arg2 = Marshal.ReadInt16(local2);
			arg3 = Marshal.ReadInt32(local3);
			arg4 = Marshal.ReadInt64(local4);

			Marshal.FreeHGlobal(local1);
			Marshal.FreeHGlobal(local2);
			Marshal.FreeHGlobal(local3);
			Marshal.FreeHGlobal(local4);

			return result;
		}

		[ObjectiveCMessage("methodParameter3")]
		public void MethodParameter3(ref byte arg1, ref ushort arg2, ref uint arg3, ref ulong arg4) 
		{
			IntPtr local1 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)));
			IntPtr local2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ushort)));
			IntPtr local3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
			IntPtr local4 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ulong)));

			Marshal.WriteByte(local1, arg1);
			Marshal.WriteInt16(local2, Convert.ToInt16(arg2));
			Marshal.WriteInt32(local3, (int)arg3);
			Marshal.WriteInt64(local4, (long)arg4);

			ObjectiveCRuntime.SendMessage(
				NativePointer, "methodParameter3", 
				local1,
				local2,
				local3,
				local4);

			arg1 = Marshal.ReadByte(local1);
			arg2 = (ushort)Marshal.ReadInt16(local2);
			arg3 = (uint)Marshal.ReadInt32(local3);
			arg4 = (ulong)Marshal.ReadInt64(local4);

			Marshal.FreeHGlobal(local1);
			Marshal.FreeHGlobal(local2);
			Marshal.FreeHGlobal(local3);
			Marshal.FreeHGlobal(local4);
		}

		[ObjectiveCMessage("methodParameter4")]
		public int MethodParameter4(ref IntPtr arg1, ref Class arg2, ref Id arg3)
		{
			IntPtr local1 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
			IntPtr local2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
			IntPtr local3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));

			Marshal.WriteIntPtr(local1, arg1);
			Marshal.WriteIntPtr(local2, arg2.NativePointer);
			Marshal.WriteIntPtr(local3, arg3.NativePointer);

			var result = ObjectiveCRuntime.SendMessage<int>(
				NativePointer, "methodParameter4", 
				local1,
				local2,
				local3);

			arg1 = Marshal.ReadIntPtr(local1);
			arg2 = ObjectiveCRuntime.GetInstance<Class>(Marshal.ReadIntPtr(local2));
			arg3 = ObjectiveCRuntime.GetInstance<Id>(Marshal.ReadIntPtr(local3));

			Marshal.FreeHGlobal(local1);
			Marshal.FreeHGlobal(local2);
			Marshal.FreeHGlobal(local3);

			return result;
		}

		[ObjectiveCMessage("methodParameter5")]
		public int MethodParameter5(ref TSWindingRule arg1)
		{
			IntPtr local1 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));

			Marshal.WriteInt32(local1, (Int32)arg1);

			var result = ObjectiveCRuntime.SendMessage<int>(
				NativePointer, "methodParameter5", 
				local1);

			arg1 = (TSWindingRule)Marshal.ReadInt32(local1);

			Marshal.FreeHGlobal(local1);

			return result;
		}

		[ObjectiveCMessage("methodParameter6")]
		public int MethodParameter6(ref float arg1, ref double arg2)
		{
			IntPtr local1 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)));
			IntPtr local2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)));

			Marshal.StructureToPtr(arg1, local1, false);
			Marshal.StructureToPtr(arg2, local2, false);

			var result = ObjectiveCRuntime.SendMessage<int>(
				NativePointer, "methodParameter6", 
				local1,
				local2);

			arg1 = (float)Marshal.PtrToStructure(local1, typeof(float));
			arg2 = (double)Marshal.PtrToStructure(local2, typeof(double));

			Marshal.FreeHGlobal(local1);
			Marshal.FreeHGlobal(local2);

			return result;
		}
	}
}