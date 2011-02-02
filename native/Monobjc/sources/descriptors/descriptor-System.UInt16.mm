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
/*!
 * \file    descriptor-System.UInt16.mm
 * \brief   Contains the descriptor code to handle the System.UInt16 type.
 * \author  Laurent Etiemble
 * \date    2009-2010
 */
#include "logging.h"
#include "marshal.h"

/*!
 * \brief   Allocates a storage zone large enough to hold a marshalled System.UInt16.
 * \param   descriptor  The descriptor.
 * \param   native      TRUE if the storage zone has the native type width (direct access)
 *                      or FALSE if the zone is at least the size of pointer storage (as FFI requires it).
 * \return  A pointer to the allocated storage.
 *
 * \remark  The storage zone is at least the size of pointer storage, due to libffi requirement.
 */
static void *__alloc_native_storage_for_System_UInt16(MonobjcTypeDescriptor *descriptor, boolean_t native) {
    //LOG_DEBUG(MONOBJC_DOMAIN_MARSHALLING, "__alloc_native_storage_for_System_UInt16()");

    if (native) {
        // Allocate a storage zone
        uint16_t *ptr = g_new(uint16_t, 1);
        return ptr;
    } else {
        // Allocate a storage zone
        uint32_t *ptr = g_new(uint32_t, 1);
        return ptr;
    }
}

/*!
 * \brief   Marshal a native value into a System.UInt16 object.
 * \param   descriptor  The descriptor.
 * \param   ptr         The pointer to the storage zone that contains the native object.
 * \param   native      TRUE if the storage zone has the native type width (direct access)
 *                      or FALSE if the zone is at least the size of pointer storage (as FFI requires it).
 * \return  A fully initialized System.UInt16 object.
 *
 * \remark  This function only use the storage zone. It is up to the caller to free it.
 *          Due to libffi requirement, the storage zone used is at least the size of pointer storage.
 *          This mean that smaller types are promoted before being boxed.
 */
static MonoObject *__marshal_from_native_for_System_UInt16(MonobjcTypeDescriptor *descriptor, void *ptr, boolean_t native) {
    //LOG_DEBUG(MONOBJC_DOMAIN_MARSHALLING, "__marshal_from_native_for_System_UInt16(%p)", ptr);

    if (native) {
        // Promote the value and box it into a managed object
        uint16_t value = *((uint16_t *) ptr);
        MonoObject *obj = mono_value_box(mono_domain_get(), mono_get_uint16_class(), &value);
        return obj;
    } else {
        // Promote the value and box it into a managed object
        uint32_t value = *((uint32_t *) ptr);
        uint16_t promoted = (uint16_t) value;
        MonoObject *obj = mono_value_box(mono_domain_get(), mono_get_uint16_class(), &promoted);
        return obj;
    }
}

/*!
 * \brief   Marshal a System.UInt16 object to its native value.
 * \param   descriptor  The descriptor.
 * \param   obj         The managed object.
 * \param   ptr         The pointer to the storage zone that will contain the native object.
 * \param   native      TRUE if the storage zone has the native type width (direct access)
 *                      or FALSE if the zone is at least the size of pointer storage (as FFI requires it).
 *
 * \remark  This function does not allocate the storage zone. It is up to the caller to allocate it and free it.
 */
static void __marshal_to_native_for_System_UInt16(MonobjcTypeDescriptor *descriptor, MonoObject *obj, void *ptr, boolean_t native) {
    //LOG_DEBUG(MONOBJC_DOMAIN_MARSHALLING, "__marshal_to_native_for_System_UInt16()");

    // Copy the value directly from the unboxed result
    uint16_t value = *((uint16_t *) mono_object_unbox(obj));
    *(uint16_t *)ptr = value;
}

/*!
 * \brief   Free the previously allocated storage zone.
 * \param   ptr         The pointer to the storage zone.
 */
static void __free_native_storage_for_System_UInt16(void *ptr) {
    //LOG_DEBUG(MONOBJC_DOMAIN_MARSHALLING, "__free_native_storage_for_System_UInt16(%p)", ptr);

    // Free the storage zone
    g_free(ptr);
}

MonobjcTypeDescriptor *monobjc_create_descriptor_for_System_UInt16() {
    LOG_DEBUG(MONOBJC_DOMAIN_MARSHALLING, "monobjc_create_descriptor_for_System_UInt16");
    
    // Allocate the descriptor structure
    MonobjcTypeDescriptor *descriptor = g_new0(MonobjcTypeDescriptor, 1);
    
    // Set the type
    descriptor->type = mono_class_get_type(mono_get_uint16_class());
    descriptor->boxed = 1;

    // Set the various marshaling functions
    descriptor->alloc_native_storage = __alloc_native_storage_for_System_UInt16;
    descriptor->marshal_from_native = __marshal_from_native_for_System_UInt16;
    descriptor->marshal_to_native = __marshal_to_native_for_System_UInt16;
    descriptor->free_native_storage = __free_native_storage_for_System_UInt16;

    // Set the Objective-C encoding
    descriptor->encoding = strdup("S");
    descriptor->size = sizeof(uint16_t);
    descriptor->alignment = MIN(log2(descriptor->size), log2(sizeof(void *)));

    // Set the type to use with libffi
    descriptor->foreign_type = &ffi_type_uint16;

    return descriptor;
}