#include "Driver.h"
#include "DsUsb.tmh"


//
// Sends a custom buffer to the device's control endpoint.
// 
NTSTATUS
SendControlRequest(
	_In_ PDEVICE_CONTEXT Context,
	_In_ WDF_USB_BMREQUEST_DIRECTION Direction,
	_In_ WDF_USB_BMREQUEST_TYPE Type,
	_In_ BYTE Request,
	_In_ USHORT Value,
	_In_ USHORT Index,
	_In_ PVOID Buffer,
	_In_ ULONG BufferLength)
{
	NTSTATUS                        status;
	WDF_USB_CONTROL_SETUP_PACKET    controlSetupPacket;
	WDF_REQUEST_SEND_OPTIONS        sendOptions;
	WDF_MEMORY_DESCRIPTOR           memDesc;
	ULONG                           bytesTransferred;

	WDF_REQUEST_SEND_OPTIONS_INIT(
		&sendOptions,
		WDF_REQUEST_SEND_OPTION_TIMEOUT
	);

	WDF_REQUEST_SEND_OPTIONS_SET_TIMEOUT(
		&sendOptions,
		DEFAULT_CONTROL_TRANSFER_TIMEOUT
	);

	switch (Type)
	{
	case BmRequestClass:
		WDF_USB_CONTROL_SETUP_PACKET_INIT_CLASS(&controlSetupPacket,
			Direction,
			BmRequestToInterface,
			Request,
			Value,
			Index);
		break;

	default:
		return STATUS_INVALID_PARAMETER;
	}

	WDF_MEMORY_DESCRIPTOR_INIT_BUFFER(&memDesc,
		Buffer,
		BufferLength);

	status = WdfUsbTargetDeviceSendControlTransferSynchronously(
		Context->Connection.Usb.UsbDevice,
		WDF_NO_HANDLE,
		&sendOptions,
		&controlSetupPacket,
		&memDesc,
		&bytesTransferred);

	if (!NT_SUCCESS(status))
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_DSUSB,
			"WdfUsbTargetDeviceSendControlTransferSynchronously failed with status %!STATUS! (%d)\n",
			status, bytesTransferred);
	}

	return status;
}

NTSTATUS
DsUsbConfigContReaderForInterruptEndPoint(
	_In_ WDFDEVICE Device
)
{
	WDF_USB_CONTINUOUS_READER_CONFIG contReaderConfig;
	NTSTATUS status;
	PDEVICE_CONTEXT pDeviceContext;

	TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DSUSB, "%!FUNC! Entry");

	pDeviceContext = DeviceGetContext(Device);

	WDF_USB_CONTINUOUS_READER_CONFIG_INIT(&contReaderConfig,
		DsUsb_EvtUsbInterruptPipeReadComplete,
		Device,    // Context
		INTERRUPT_IN_BUFFER_LENGTH);   // TransferLength

	contReaderConfig.EvtUsbTargetPipeReadersFailed = DsUsbEvtUsbInterruptReadersFailed;

	//
	// Reader requests are not posted to the target automatically.
	// Driver must explicitly call WdfIoTargetStart to kick start the
	// reader.  In this sample, it's done in D0Entry.
	// By default, framework queues two requests to the target
	// endpoint. Driver can configure up to 10 requests with CONFIG macro.
	//
	status = WdfUsbTargetPipeConfigContinuousReader(pDeviceContext->Connection.Usb.InterruptInPipe,
		&contReaderConfig);

	if (!NT_SUCCESS(status)) {
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_DSUSB,
			"WdfUsbTargetPipeConfigContinuousReader failed %x\n",
			status);
		return status;
	}

	TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DSUSB, "%!FUNC! Exit");

	return status;
}

BOOLEAN
DsUsbEvtUsbInterruptReadersFailed(
	_In_ WDFUSBPIPE Pipe,
	_In_ NTSTATUS Status,
	_In_ USBD_STATUS UsbdStatus
)
{
	UNREFERENCED_PARAMETER(UsbdStatus);
	UNREFERENCED_PARAMETER(Pipe);

	TraceEvents(TRACE_LEVEL_ERROR, TRACE_DSUSB,
		"DsUsbEvtUsbInterruptReadersFailed called with status %!STATUS!",
		Status);

	return TRUE;
}