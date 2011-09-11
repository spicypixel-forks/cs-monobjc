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
 * @file    support-objc.mm
 * @brief   Defines Objective-C runtime functions.
 * @author  Laurent Etiemble <laurent.etiemble@monobjc.net>
 * @date    2009-2011
 */
#include "logging.h"
#include "support-objc.h"

int monobjc_compare_encoding(const char *encoding1, const char *encoding2) {
    char *str1 = monobjc_flatten_type(encoding1);
    char *str2 = monobjc_flatten_type(encoding2);
    
    int result = 1;
    size_t index = 0;
    
    if (strlen(str1) != strlen(str2)) {
        goto bail;
    }
    
    while (index < strlen(str1)) {
        char c1 = str1[index];
        char c2 = str2[index];
        
        switch (c1) {
            case '@':
            case '#':
            case '^':
            case ':':
            case '?':
            case '*':
                if (c2 != '@' &&
                    c2 != '#' &&
                    c2 != '^' &&
                    c2 != ':' &&
                    c2 != '?' &&
                    c2 != '*') {
                    goto bail;
                }
                break;
            default:
                if (c1 != c2) {
                    goto bail;
                }
                break;
        }
        
        index++;
    }
    
    result = 0;
    
bail:    
    g_free(str1);
    g_free(str2);
    
    return result;
}

char *monobjc_flatten_type(const char *encoding) {
    if (!encoding) {
        return NULL;
    }
    // Allocate space for the result
    char *result = g_new0(char, strlen(encoding) + 1);
    
    char *current = result;
    size_t index = 0;
    while(encoding[index]) {
        switch (encoding[index]) {
            case 'O':   /* bycopy */
            case 'n':   /* in */
            case 'o':   /* out */
            case 'N':   /* inout */
            case 'r':   /* const */
            case 'V':   /* oneway */
            {
                index++;
                break;
            }
                
            case '{':   /* Structures */
            {
                while (encoding[index] && encoding[index] != '=') {
                    index++;
                }
                index++;
                
                // Get a pointers to the subtype
                while (encoding[index] && encoding[index] != '}') {
                    const char *subtype = encoding + index;
                    const char *end = monobjc_skip_first_type(subtype);
                    
                    // Get flatten result for subtype
                    size_t inner_len = end - subtype;
                    char *inner_result = monobjc_flatten_type(subtype);
                    strncpy(current, inner_result, inner_len);
                    current += strlen(inner_result);
                    g_free(inner_result);
                    
                    index += inner_len;
                }
                
                // Terminate result
                current[0] = 0;
                goto bail;
            }
                
            case '[':   /* Arrays */
            {
                // For now, we skip the array type.
                // This means that we trust the caller to do the right thing.
                while (encoding[index] && encoding[index] != '}') {
                    index++;
                }
                goto bail;
            }

            case '(':   /* Unions */
            {
                LOG_ERROR(MONOBJC_DOMAIN_MESSAGING, "Encoding not supported: '%s'", encoding);
                goto bail;
            }
                
            case '^':   /* pointers */
            default:    /* Basic types */
            {
                result[0] = encoding[index];
                result[1] = 0;
                goto bail;
            }
        }
    }
bail:
    
    return result;
}

int monobjc_subtype_until(const char *type, char end) {
    int level = 0;
    const char *head = type;
    
    // Iterate the type encoding until the end character is found
    while (*type) {
        if (!*type || (!level && (*type == end))) {
            return (int)(type - head);
        }
        // Make sure that parsing level is recorded
        switch (*type) {
            case ']': case '}': case ')': level--; break;
            case '[': case '{': case '(': level += 1; break;
        }
        // Advance into the type encoding
        type += 1;
    }

    LOG_ERROR(MONOBJC_DOMAIN_ENCODING, "End of type encountered prematurely encountered");
    
    return 0;
}

const char *monobjc_skip_first_type(const char *type) {
    while (1) {
        switch (*type++) {
            case 'O':   /* bycopy */
            case 'n':   /* in */
            case 'o':   /* out */
            case 'N':   /* inout */
            case 'r':   /* const */
            case 'V':   /* oneway */
            case '^':   /* pointers */
                break;
                
            case '[':   /* Arrays */
                while ((*type >= '0') && (*type <= '9')) {
                    type += 1;
                }
                return type + monobjc_subtype_until(type, ']') + 1;
                
            case '{':   /* Structures */
                return type + monobjc_subtype_until(type, '}') + 1;
                
            case '(':   /* Unions */
                return type + monobjc_subtype_until(type, ')') + 1;
                
            default:    /* Basic types */
                return type;
        }
    }
}
