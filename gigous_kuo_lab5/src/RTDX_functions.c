#ifdef DSP28_BIOS

#include <coecsl.h>
#include <RTDX_functions.h>

void init_RTDX_readchannel_float(RTDX_inputChannel *ch, float *var, int length)
{
	RTDX_enableInput(ch);
	RTDX_readNB(ch, var, length*sizeof(float));
}

void init_RTDX_readchannel_int16(RTDX_inputChannel *ch, int16 *var, int length)
{
	RTDX_enableInput(ch);
	RTDX_readNB(ch, var, length*sizeof(int16));
}

void init_RTDX_readchannel_int32(RTDX_inputChannel *ch, int32 *var, int length)
{
	RTDX_enableInput(ch);
	RTDX_readNB(ch, var, length*sizeof(int32));
}


void init_RTDX_writechannel(RTDX_outputChannel *ch)
{
	RTDX_enableOutput(ch);
}

void RTDX_readchannel_float(RTDX_inputChannel *ch, float *var, int length)
{
	while (1) {
		if (!RTDX_channelBusy(ch)) {
			RTDX_readNB(ch, var, length*sizeof(float));
			break;
		}
		TSK_sleep(20);
	}
}

void RTDX_readchannel_int16(RTDX_inputChannel *ch, int16 *var, int length)
{
	while (1) {
		if (!RTDX_channelBusy(ch)) {
			RTDX_readNB(ch, var, length*sizeof(int16));
			break;
		}
		TSK_sleep(20);
	}
}

void RTDX_readchannel_int32(RTDX_inputChannel *ch, int32 *var, int length)
{
	while (1) {
		if (!RTDX_channelBusy(ch)) {
			RTDX_readNB(ch, var, length*sizeof(int32));
			break;
		}
		TSK_sleep(20);
	}
}

void RTDX_writechannel_float(RTDX_outputChannel *ch, float *var, int length)
{
	RTDX_write(ch, var, length*sizeof(float));
}

void RTDX_writechannel_int16(RTDX_outputChannel *ch, int16 *var, int length)
{
	RTDX_write(ch, var, length*sizeof(int16));
}

void RTDX_writechannel_int32(RTDX_outputChannel *ch, int32 *var, int length)
{
	RTDX_write(ch, var, length*sizeof(int32));
}

#endif /* DSP28_BIOS */

