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

using Monobjc;
using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Runtime.InteropServices;
using Monobjc.Foundation;

namespace Monobjc.Kernel
{
    public static partial class Dispatch
    {
#if MACOSX_10_7
		public const String DISPATCH_LIBRARY = "/usr/lib/system/libdispatch";
#else
		public const String DISPATCH_LIBRARY = "/usr/lib/libSystem";
#endif
	
		/// <summary>
		/// Items dispatched to the queue run at high priority; the queue is scheduled for execution before any default priority or low priority queue.
		/// </summary>
		public const short DISPATCH_QUEUE_PRIORITY_HIGH = 2;
		/// <summary>
		/// Items dispatched to the queue run at the default priority; the queue is scheduled for execution after all high priority queues have been scheduled, but before any low priority queues have been scheduled.
		/// </summary>
		public const short DISPATCH_QUEUE_PRIORITY_DEFAULT = 0;
		/// <summary>
		/// Items dispatched to the queue run at low priority; the queue is scheduled for execution after all default priority and high priority queues have been scheduled.
		/// </summary>
		public const short DISPATCH_QUEUE_PRIORITY_LOW = -2;
#if MACOSX_10_7
		/// <summary>
		/// Items dispatched to the queue run at background priority; the queue is scheduled for execution after all high priority queues have been scheduled and the system runs items on a thread whose priority is set for background status. Such a thread has the lowest priority and any disk I/O is throttled to minimize the impact on the system.
		/// </summary>
		public const short DISPATCH_QUEUE_PRIORITY_BACKGROUND = Int16.MinValue;
#endif
	}

    /// <summary>
    /// <para></para>
    /// <para>Original signature is 'typedef void (*dispatch_function_t)(void *);'</para>
    /// <para>Available in Mac OS X v10.6 and later.</para>
    /// </summary>
	public delegate void dispatch_function_t(IntPtr context);
	
    /// <summary>
    /// <para></para>
    /// <para>Original signature is 'typedef void (*work)(void *, size_t)'</para>
    /// <para>Available in Mac OS X v10.6 and later.</para>
    /// </summary>
	public delegate void dispatch_function_tc(IntPtr context, NSUInteger count);
    
    /// <summary>
    /// <para></para>
    /// <para>Original signature is 'typedef void (^dispatch_block_t)(void);'</para>
    /// <para>Available in Mac OS X v10.6 and later.</para>
    /// </summary>
    public delegate void dispatch_block_t();

    /// <summary>
    /// <para></para>
    /// <para>Original signature is 'typedef bool (^dispatch_data_applier_t)(dispatch_data_t region, size_t offset, const void *buffer, size_t size);'</para>
    /// <para>Available in Mac OS X v10.6 and later.</para>
    /// </summary>
    public delegate bool dispatch_data_applier_t(IntPtr region, NSUInteger offset, IntPtr buffer, NSUInteger size);
    
    /// <summary>
    /// <para></para>
    /// <para>Original signature is 'typedef void (^dispatch_io_handler_t)(bool done, dispatch_data_t data, int error);'</para>
    /// <para>Available in Mac OS X v10.6 and later.</para>
    /// </summary>
    public delegate void dispatch_io_handler_t(bool done, IntPtr data, int error);
}
