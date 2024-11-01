﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraScannerWrapper.Enums
{
    public enum ScannerStatus
    {
        SUCCESS = 0,
        STATUS_LOCKED = 10,
        ERROR_INVALID_APPHANDLE = 100,
        ERROR_COMMLIB_UNAVAILABLE = 101,
        ERROR_NULL_BUFFER_POINTER = 102,
        ERROR_INVALID_BUFFER_POINTER = 103,
        ERROR_INCORRECT_BUFFER_SIZE = 104,
        ERROR_DUPLICATE_TYPES = 105,
        ERROR_INCORRECT_NUMBER_OF_TYPES = 106,
        ERROR_INVALID_ARG = 107,
        ERROR_INVALID_SCANNERID = 108,
        ERROR_INCORRECT_NUMBER_OF_EVENTS = 109,
        ERROR_DUPLICATE_EVENTID = 110,
        ERROR_INVALID_EVENTID = 111,
        ERROR_DEVICE_UNAVAILABLE = 112,
        ERROR_INVALID_OPCODE = 113,
        ERROR_INVALID_TYPE = 114,
        ERROR_ASYNC_NOT_SUPPORTED = 115,
        ERROR_OPCODE_NOT_SUPPORTED = 116,
        ERROR_OPERATION_FAILED = 117,
        ERROR_REQUEST_FAILED = 118,
        ERROR_OPERATION_NOT_SUPPORTED_FOR_AUXILIARY_SCANNERS = 119,
        ERROR_DEVICE_BUSY = 120,
        ERROR_ALREADY_OPENED = 200,
        ERROR_ALREADY_CLOSED = 201,
        ERROR_CLOSED = 202,
        ERROR_INVALID_INXML = 300,
        ERROR_XMLREADER_NOT_CREATED = 301,
        ERROR_XMLREADER_INPUT_NOT_SET = 302,
        ERROR_XMLREADER_PROPERTY_NOT_SET = 303,
        ERROR_XMLWRITER_NOT_CREATED = 304,
        ERROR_XMLWRITER_OUTPUT_NOT_SET = 305,
        ERROR_XMLWRITER_PROPERTY_NOT_SET = 306,
        ERROR_XML_ELEMENT_CANT_READ = 307,
        ERROR_XML_INVALID_ARG = 308,
        ERROR_XML_WRITE_FAIL = 309,
        ERROR_XML_INXML_EXCEED_LENGTH = 310,
        ERROR_XML_EXCEED_BUFFER_LENGTH = 311,
        ERROR_NULL_POINTER = 400,
        ERROR_DUPLICATE_CLIENT = 401,
        ERROR_FW_INVALID_DATFILE = 500,
        ERROR_FW_UPDATE_FAILED_IN_SCN = 501,
        ERROR_FW_READ_FAILED_DATFILE = 502,
        ERROR_FW_UPDATE_INPROGRESS = 503,
        ERROR_FW_UPDATE_ALREADY_ABORTED = 504,
        ERROR_FW_UPDATE_ABORTED = 505,
        ERROR_FW_SCN_DETTACHED = 506,
        STATUS_FW_SWCOMP_RESIDENT = 600
    }
}