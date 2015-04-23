#include <tistdtypes.h>
#include <coecsl.h>
#include "user_includes.h"
#include "28335_dma.h"
#include "28335_spi.h"
#include "28335_inits.h"
#include "mcbsp_com.h"
#include "i2c.h"
#include <math.h>

void updateData(void);
void sendData(void);

extern volatile int new_irdata_i2c;
extern int adc1_i2c;
extern int adc2_i2c;
extern int adc3_i2c;
extern int adc4_i2c;
extern int adc5_i2c;
extern int adc6_i2c;
extern int adc7_i2c;
extern int adc8_i2c;
extern int CompassNew;
extern int ir1_i2c,ir2_i2c,ir3_i2c,ir4_i2c,ir5_i2c;

extern int SPIenc_state;
extern long SPIenc1_reading;
extern long SPIenc2_reading;
extern long SPIenc3_reading;
extern long SPIenc4_reading;

extern unsigned int nocomm;
extern int newOMAPdata;
extern int PIVEL_enable;
extern float mcbsp_vref;
extern float mcbsp_turn;
extern float mcbsp_pwm1;
extern float mcbsp_pwm2;
extern float mcbsp_dac1;
extern float mcbsp_dac2;
extern long McBSP_COMerr;
extern int McBSP_RecGoodData_ReadyTX;
extern long McBSPb_int_count;
extern int McBSPB_rdy;
extern mcbsp28x_com TXBuff;
extern mcbspL138_com RXBuff;

unsigned long noi2c = 0;

float gyro_x = 0;
float gyro_y = 0;

float Enc1_rad = 0;
float Enc2_rad = 0;
float Enc3_rad = 0;
float Enc4_rad = 0;

int switchstate = -2;

int newF28335_Extra = 0;
float F28335_Extra1 = 0.0;
float F28335_Extra2 = 0.0;
float F28335_Extra3 = 0.0;
float F28335_Extra4 = 0.0;

int raw_adc_A0 = 0;
int raw_adc_A1 = 0;
int raw_adc_A2 = 0;
int raw_adc_A3 = 0;
int raw_adc_A4 = 0;
int raw_adc_A5 = 0;
int raw_adc_A6 = 0;
int raw_adc_A7 = 0;
int raw_adc_B0 = 0;
int raw_adc_B1 = 0;
int raw_adc_B2 = 0;
int raw_adc_B3 = 0;
int raw_adc_B4 = 0;
int raw_adc_B5 = 0;
int raw_adc_B6 = 0;
int raw_adc_B7 = 0;

float omap_vref = 0;
float omap_turn = 0;
float omap_pwm1 = 0;
float omap_pwm2 = 0;
float omap_dac1 = 0;
float omap_dac2 = 0;

eqep_t enc1;
eqep_t enc2;
float value_enc1 = 0;
float value_enc2 = 0;


unsigned long timeint = 0;

float p_current1 = 0;
float p_old1 = 0;
float p_current2 = 0;
float p_old2 = 0;

float v1 = 0;
float v2 = 0;

void main(void)
{

	//  Init and zero encoders
	init_EQEP(&enc1, EQEP1, 3000, 1, 0.0);
	init_EQEP(&enc2, EQEP2, 3000, -1, 0.0);
	EQep1Regs.QPOSCNT = 0;
	EQep2Regs.QPOSCNT = 0;

	// Initialize PWMs
	init_PWM(EPWM1);
	init_PWM(EPWM2);
	init_PWM_AS_RCSERVO(EPWM3B);  // sets up EPWM3A and EPWM3B for RCservo
	init_PWM_AS_RCSERVO(EPWM4B); // sets up EPWM4A and EPWM4B for RCservo
	init_PWM_AS_RCSERVO(EPWM5);  // sets up EPWM5A for RCservo

	// System initializations
	pre_init();

	// initialize serial port A to 115200 baud
	init_serial(&SerialA,115200,NULL);
	// initialize serial port B to 57600 baud
	init_serial(&SerialB,57600,NULL);
	// initialize serial port C to 19200 baud
	init_serial(&SerialC,19200,NULL);

	EALLOW;  // set up LED GPIOs
		GpioCtrlRegs.GPAMUX2.bit.GPIO30 = 0;
		GpioDataRegs.GPACLEAR.bit.GPIO30 = 1;
		GpioCtrlRegs.GPADIR.bit.GPIO30 = 1;
		GpioCtrlRegs.GPAMUX2.bit.GPIO31 = 0;
		GpioDataRegs.GPACLEAR.bit.GPIO31 = 1;
		GpioCtrlRegs.GPADIR.bit.GPIO31 = 1;
		GpioCtrlRegs.GPBMUX1.bit.GPIO34 = 0;
		GpioDataRegs.GPBSET.bit.GPIO34 = 1;
		GpioCtrlRegs.GPBDIR.bit.GPIO34 = 1;

		// set up GPIO3 for amp enable or disable
		GpioCtrlRegs.GPAMUX1.bit.GPIO3 = 0;
		GpioDataRegs.GPACLEAR.bit.GPIO3 = 1;
		GpioCtrlRegs.GPADIR.bit.GPIO3 = 1;

	EDIS; 

	init_dma_mcbsp( (Uint32)&TXBuff.darray[0],(Uint32)&RXBuff.darray[0]);
	InitMcbspbGpio();
	InitMcbspb();
	InitMcbspb32bit();
	init_SPI();
	InitI2CGpio();
	Init_i2c();
	
	// Add your inits here

	// Finalize inits and start DMA/McBSP
	post_init();

}

void start_dataCollection(void) {

	// Start SPI
	start_SPI();

}

float pos_x = 0.0;
float u1 = 0;
float u2 = 0;

float r1 = 0;
float r2 = 0;

float error1 = 0;
float error2 = 0;

float error_integral1 = 0;
float error_integral2 = 0;

float error1_old=0;
float error2_old=0;

char ctrl_alg[50];

const float Kp = 3;
const float Ki = 8;
const float Kp_turn = 3.0;
float turn = 0;
float e_steer = 0;

void control(void) {

	updateData();

	// Our code
	//#####################################
	p_current1 = Enc1_rad/192;
	p_current2 = Enc2_rad/192;

	v1 = (p_current1-p_old1)/0.001;
	v2 = (p_current2-p_old2)/0.001;

	pos_x = pos_x + (v1+v2)/2.0*0.001;

	timeint++;

	switch (switchstate)
	{
		case 1: // open loop
			//u1=0;u2=0;

			u1 = Enc3_rad/100;
			u2 = Enc3_rad/100;
			if (v1 > 0.0)
			{
				u1 = u1 + 0.6*(2.3*v1 + 2.4);
			}
			else
			{
				u1 = u1 + 0.6*(2.3*v1 - 2.3);
			}

			if (v2 > 0.0)
			{
				u2 = u2 + 0.6*(2.3*v2 + 2.5);
			}
			else
			{
				u2 = u2 + 0.6*(2.3*v2 - 2.3);
			}

			PWM_out(EPWM1,u1);
			PWM_out(EPWM2,-u2);
			strcpy(ctrl_alg,"Open Loop");
			break;
		case 2: // lksajdf;j
			r1 = Enc3_rad/100;
			r2 = Enc3_rad/100;

			error1 = r1 - v1;
			error2 = r2 - v2;

			error_integral1 += 0.5*(error1+error1_old)*0.001;
			error_integral2 += 0.5*(error2+error2_old)*0.001;

			if ( abs(u1) > 10 )
			{
				error_integral1 *= 0.99;
			}

			if ( abs(u2) > 10 )
			{
				error_integral2 *= 0.99;
			}

			error1_old=error1;
			error2_old=error2;

			u1 = error1*Kp + error_integral1*Ki;
			u2 = error2*Kp + error_integral2*Ki;

			if (v1 > 0.0)
			{
				u1 = u1 + 0.6*(2.3*v1 + 2.4);
			}
			else
			{
				u1 = u1 + 0.6*(2.3*v1 - 2.3);
			}

			if (v2 > 0.0)
			{
				u2 = u2 + 0.6*(2.3*v2 + 2.5);
			}
			else
			{
				u2 = u2 + 0.6*(2.3*v2 - 2.3);
			}

			PWM_out(EPWM1,u1);
			PWM_out(EPWM2,-u2);
			strcpy(ctrl_alg,"Decoupled");
			break;

		case 4: // Coupled
			r1 = Enc3_rad/100;
			r2 = Enc3_rad/100;

			turn = 0;

			e_steer = v2 - v1 + turn;
			error1 = r1 - v1 + e_steer*Kp_turn;
			error2 = r2 - v2 - e_steer*Kp_turn;

			error_integral1 += 0.5*(error1+error1_old)*0.001;
			error_integral2 += 0.5*(error2+error2_old)*0.001;

			if ( abs(u1) > 10 )
			{
				error_integral1 *= 0.99;
			}

			if ( abs(u2) > 10 )
			{
				error_integral2 *= 0.99;
			}

			error1_old=error1;
			error2_old=error2;

			u1 = error1*Kp + error_integral1*Ki;
			u2 = error2*Kp + error_integral2*Ki;

			if (v1 > 0.0)
			{
				u1 = u1 + 0.6*(2.3*v1 + 2.4);
			}
			else
			{
				u1 = u1 + 0.6*(2.3*v1 - 2.3);
			}

			if (v2 > 0.0)
			{
				u2 = u2 + 0.6*(2.3*v2 + 2.5);
			}
			else
			{
				u2 = u2 + 0.6*(2.3*v2 - 2.3);
			}
			PWM_out(EPWM1,u1);
			PWM_out(EPWM2,-u2);
			strcpy(ctrl_alg,"3rd");
			break;
		case 8: // lfgf;j
			r1 = 1.0;
			r2 = 1.0;
			turn = Enc3_rad/100;

			e_steer = v2 - v1 + turn;
			error1 = r1 - v1 + e_steer*Kp_turn;
			error2 = r2 - v2 - e_steer*Kp_turn;

			error_integral1 += 0.5*(error1+error1_old)*0.001;
			error_integral2 += 0.5*(error2+error2_old)*0.001;

			if ( abs(u1) > 10 )
			{
				error_integral1 *= 0.99;
			}

			if ( abs(u2) > 10 )
			{
				error_integral2 *= 0.99;
			}

			error1_old=error1;
			error2_old=error2;

			u1 = error1*Kp + error_integral1*Ki;
			u2 = error2*Kp + error_integral2*Ki;

			if (v1 > 0.0)
			{
				u1 = u1 + 0.6*(2.3*v1 + 2.4);
			}
			else
			{
				u1 = u1 + 0.6*(2.3*v1 - 2.3);
			}

			if (v2 > 0.0)
			{
				u2 = u2 + 0.6*(2.3*v2 + 2.5);
			}
			else
			{
				u2 = u2 + 0.6*(2.3*v2 - 2.3);
			}

			PWM_out(EPWM1,u1);
			PWM_out(EPWM2,-u2);
			strcpy(ctrl_alg,"4th");
			break;

		default: // unknown
			PWM_out(EPWM1,0);
			PWM_out(EPWM2,0);
			strcpy(ctrl_alg,"none");
			break;
	}

	p_old1 = p_current1;
	p_old2 = p_current2;

	F28335_Extra1 = 0;
	F28335_Extra2 = pos_x;
	F28335_Extra3 = (v1+v2)/2.0;
	F28335_Extra4 = 0.0;
	if ( (timeint % 100) == 0 )
	{
		UART_LCDPrintfLine(1, "%s %.1f", ctrl_alg, Enc3_rad/100);
		UART_LCDPrintfLine(2, "v1 %.1f v2 %.1f", v1, v2);
	}
	//#####################################

	sendData();

}

void updateData(void) {

	value_enc1 = EQEP_read(&enc1);
	value_enc2 = EQEP_read(&enc2);

	Enc1_rad = ((float) SPIenc1_reading)*(TWOPI/(2000.0f)); // left encoder
	Enc1_rad = -Enc1_rad;
	Enc2_rad = ((float) SPIenc2_reading)*(TWOPI/(2000.0f)); // right encoder
	Enc3_rad = ((float) SPIenc3_reading)*(TWOPI/(2000.0f));
	Enc4_rad = ((float) SPIenc4_reading)*(TWOPI/(2000.0f));

	
	switchstate = (int)((0x000F & ~(((GpioDataRegs.GPBDAT.all & 0x0C000000)>>24) | ((GpioDataRegs.GPBDAT.all & 0x00030000)>>16))));

	if (newOMAPdata) {
		omap_vref = mcbsp_vref;
		omap_turn = mcbsp_turn;
		omap_pwm1 = mcbsp_pwm1;
		omap_pwm2 = mcbsp_pwm2;
		omap_dac1 = mcbsp_dac1;
		omap_dac2 = mcbsp_dac2;
		newOMAPdata = 0;
	}

}

void sendData(void) {

	if (McBSPB_rdy == 1) {
		TXBuff.data.validcode = VALID_28XTO67X;
		TXBuff.data.DSC_ENC[0] = value_enc1;
		TXBuff.data.DSC_ENC[1] = value_enc2;
		TXBuff.data.SPI_ENC[0] = (float) Enc1_rad;
		TXBuff.data.SPI_ENC[1] = (float) Enc2_rad;
		TXBuff.data.SPI_ENC[2] = (float) Enc3_rad;
		TXBuff.data.SPI_ENC[3] = (float) Enc4_rad;
		TXBuff.data.compass = (float)CompassNew;
		TXBuff.data.switchstate = (float)switchstate;
		TXBuff.data.ADC[0] = (float)raw_adc_A0;
		TXBuff.data.ADC[1] = (float)raw_adc_B0;
		TXBuff.data.ADC[2] = (float)raw_adc_A1;
		TXBuff.data.ADC[3] = (float)raw_adc_B1;
		TXBuff.data.ADC[4] = (float)raw_adc_A2;
		TXBuff.data.ADC[5] = (float)raw_adc_B2;
		TXBuff.data.ADC[6] = (float)raw_adc_A3;
		TXBuff.data.ADC[7] = (float)raw_adc_B3;
		TXBuff.data.ADC[8] = (float)raw_adc_A4;
		TXBuff.data.ADC[9] = (float)raw_adc_B4;
		TXBuff.data.ADC[10] = (float)raw_adc_A5;
		TXBuff.data.ADC[11] = (float)raw_adc_B5;
		TXBuff.data.ADC[12] = (float)raw_adc_A6;
		TXBuff.data.ADC[13] = (float)raw_adc_A7;
		TXBuff.data.ADC[14] = 0; // Reserved   was (float)atmel_adc1;
		TXBuff.data.ADC[15] = 0; // Reserved   was (float)atmel_adc2;
		TXBuff.data.ADC[16] = 0; // Reserved   was (float)atmel_adc3;
		TXBuff.data.ADC[17] = 0; // Reserved   was (float)atmel_adc4;
		TXBuff.data.IR[0] = 0; // Reserved   was (float)atmel_ir1;
		TXBuff.data.IR[1] = 0; // Reserved   was (float)atmel_ir2;
		TXBuff.data.IR[2] = 0; // Reserved   was (float)atmel_ir3;
		TXBuff.data.IR[3] = 0; // Reserved   was (float)atmel_ir4;
		TXBuff.data.IR[4] = 0; // Reserved   was (float)atmel_ir5;
		TXBuff.data.GYRO[0] = (float)gyro_x;
		TXBuff.data.GYRO[1] = (float)gyro_y;
		TXBuff.data.F28335_EXTRA[0] = (float)F28335_Extra1;
		TXBuff.data.F28335_EXTRA[1] = (float)F28335_Extra2;
		TXBuff.data.F28335_EXTRA[2] = (float)F28335_Extra3;
		TXBuff.data.F28335_EXTRA[3] = (float)F28335_Extra4;

		if (McBSP_RecGoodData_ReadyTX == 1) {
			McBSPB_rdy = 0;
			start_dma();
			McBSP_RecGoodData_ReadyTX = 0;
		}
	}
}

