// 
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2011 - Laurent Etiemble
// 
// Monobjc is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// any later version.
// 
// Monobjc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with Monobjc. If not, see <http://www.gnu.org/licenses/>.
// 
/**
 * @file    icalls-Monobjc.Runtime.Platform.mm
 * @brief   Contains the internal calls for the Monobjc.Runtime.Platform type.
 * @author  Laurent Etiemble <laurent.etiemble@monobjc.net>
 * @date    2009-2011
 */
#include "constants.h"
#include "icalls.h"
#include "logging.h"
#include "support-os.h"

#pragma mark ----- Internal Calls -----

/**
 * @brief   Return the descriptor of the processor of the system.
 * @return  A system dependent description.
 */
MonoString *icall_Monobjc_Runtime_Platform_GetProcessor(void) {
    const NXArchInfo *info = monobjc_get_arch_info();
    return mono_string_new(mono_domain_get(), info->description);
}

/**
 * @brief   Return the current version of Mac OS X.
 * @return  A normalized value for the version. For example:
 *          @li 1040 for Mac OS X 10.4, whatever the patch level is.
 *          @li 1050 for Mac OS X 10.5, whatever the patch level is.
 */
OSVersion icall_Monobjc_Runtime_Platform_GetOSVersion(void) {
    SInt32 version = monobjc_get_os_version();
    
    if (version < MACOS_10_0) {
        return MACOS_Unrecognized;
    }
    if (version < MACOS_10_1) {
        return MACOS_10_0;
    }
    if (version < MACOS_10_2) {
        return MACOS_10_1;
    }
    if (version < MACOS_10_3) {
        return MACOS_10_2;
    }
    if (version < MACOS_10_4) {
        return MACOS_10_3;
    }
    if (version < MACOS_10_5) {
        return MACOS_10_4;
    }
    if (version < MACOS_10_6) {
        return MACOS_10_5;
    }
    if (version < MACOS_10_7) {
        return MACOS_10_6;
    }
    return MACOS_10_7;
}

/**
 * @brief   Return whether the platform is 32 or 64 bits.
 * @return  TRUE if the platform is 64 bits, FALSE otherwise.
 */
boolean_t icall_Monobjc_Runtime_Platform_Is64Bits(void) {
    return IS64BITS;
}

/**
 * @brief   Return whether the platform uses the big and little endianness.
 * @return  TRUE if the platform uses big-endianness, FALSE otherwise.
 */
boolean_t icall_Monobjc_Runtime_Platform_IsBigEndian(void) {
#if TARGET_RT_BIG_ENDIAN
    return true;
#elif TARGET_RT_LITTLE_ENDIAN
    return false;
#else
    #error Unsupported Endianness
#endif
}
