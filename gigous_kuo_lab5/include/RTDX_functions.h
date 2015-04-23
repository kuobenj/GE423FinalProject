
#ifdef DSP28_BIOS
#ifndef __RDTX_FUNCTIONS_H__
#define __RDTX_FUNCTIONS_H__

void init_RTDX_readchannel_float(RTDX_inputChannel *ch, float *var, int length);
void init_RTDX_readchannel_int16(RTDX_inputChannel *ch, int16 *var, int length);
void init_RTDX_readchannel_int32(RTDX_inputChannel *ch, int32 *var, int length);

void init_RTDX_writechannel(RTDX_outputChannel *ch);

void RTDX_readchannel_float(RTDX_inputChannel *ch, float *var, int length);
void RTDX_readchannel_int16(RTDX_inputChannel *ch, int16 *var, int length);
void RTDX_readchannel_int32(RTDX_inputChannel *ch, int32 *var, int length);

void RTDX_writechannel_float(RTDX_outputChannel *ch, float *var, int length);
void RTDX_writechannel_int16(RTDX_outputChannel *ch, int16 *var, int length);
void RTDX_writechannel_int32(RTDX_outputChannel *ch, int32 *var, int length);

#endif /* __RDTX_FUNCTIONS_H__ */
#endif /* DSP28_BIOS */
